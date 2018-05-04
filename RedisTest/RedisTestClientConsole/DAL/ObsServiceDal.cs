using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using RedisConsoleClientTest;
using RedisTestClientConsole.Model;

namespace RedisTestClientConsole.DAL
{
    public class ObsServiceDal:DbConn
    {
        public IList<ProductInfoModel> GetSerialLine(IList<string> serials)
        {
            var serialsProductLine = new List<ProductInfoModel>();

            if (serials == null || serials.Count == 0)
            {
                return serialsProductLine;
            }
            var counter = 0;
            var serialsXml = new StringBuilder(" <Serials>");
            foreach (var serial in serials)
            {
                var cache = CacheHelper.Get(string.Format("Cache_New_ProductSerial_ProductLine_{0}", serial));
                if (cache == null)
                {
                    counter++;
                    serialsXml.AppendFormat("<Serial>{0}</Serial>", serial);
                    serialsXml.AppendLine();
                }
                else
                {
                    serialsProductLine.Add(cache as ProductInfoModel);
                }
            }
            serialsXml.AppendLine("</Serials>");

            if (counter > 0)
            {
                var strSql = new StringBuilder();
                strSql.AppendLine("SELECT nps.ProductSerialCode,nps.ProductSerialName,pp.PropertyName,ppsi.InputValue ");
                strSql.AppendLine("FROM New_ProductSerial nps(NOLOCK)");
                strSql.AppendLine("INNER JOIN P_Product_Serial_Info ppsi(NOLOCK) ON nps.ProductSerialCode=ppsi.ProductSerialCode");
                strSql.AppendLine("INNER JOIN P_Property pp(nolock) ON ppsi.PropertyID=pp.PropertyId AND pp.PropertyName='产品线'");
                strSql.AppendLine("WHERE nps.ProductSerialCode IN(");
                strSql.AppendLine("    SELECT T.c.value(N'(./text())[1]',N'int')");
                strSql.AppendLine("    FROM @Serials.nodes(N'/Serials/Serial') T(c)");
                strSql.AppendFormat(") "); //产品线属性
                var paras = new[]
                                {
                                    new SqlParameter("@Serials", SqlDbType.Xml)
                                };
                paras[0].Value = serialsXml.ToString();
                using (var reader = SqlHelper.ExecuteReader(ProductDbReadOnlyConnString, CommandType.Text,strSql.ToString(), paras))
                {
                    while (reader.Read())
                    {
                        var productSerialCode = int.Parse(reader["ProductSerialCode"].ToString());
                        var sku = new ProductInfoModel
                                      {
                                          ProductSerialCode = int.Parse(reader["ProductSerialCode"].ToString()),
                                          ProductSerialName = reader["ProductSerialName"].ToString(),
                                          ProductLine = reader["InputValue"].ToString()
                                      };
                        CacheHelper.Add(string.Format("Cache_New_ProductSerial_ProductLine_{0}", productSerialCode), sku, DateTime.Now.AddDays(1));
                        serialsProductLine.Add(sku);
                    }
                }
            }

            #region ############# SKU SiteCategory ###############
            ////根据商品skuId集合获取商品销售分类
            //var list = GetSalesCategoriesBySkus(skuIds);

            //if (list != null && list.Count > 0)
            //{
            //    foreach (var sku in skus)
            //    {
            //        var categories = list.ToList().FindAll(s => s.SkuId == sku.Id);

            //        foreach (var category in categories)
            //        {
            //            var item = sku.SalesCategories.ToList().Find(p => p.Id == category.Id);

            //            if (item == null)
            //            {
            //                sku.SalesCategories.Add(new SalesCategoryDTO
            //                {
            //                    Id = category.Id,
            //                    Name = category.Name
            //                });
            //            }
            //        }
            //    }
            //}
            #endregion

            return serialsProductLine;
        }
    }
}
