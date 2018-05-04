using System;
using System.Collections.Generic;
using System.Diagnostics;
using RedisConsoleClientTest;
using RedisTestClientConsole.DAL;
using RedisTestClientConsole.Model;
using ServiceStack.Redis;
using ServiceStack.Redis.Support;

namespace RedisTestClientConsole
{
    internal class Program
    {
        #region ########### CacheKey ####################

        private const string PrefixSkuBaseInfo = "Cache_SKU_BaseInfo_ForObs";
        private const string PrefixSkuProductLineInfo = "Cache_Serial_ProductLine_ForObs";
        private const string PrefixSkuStyleInfo = "Cache_SKU_StyleID_ForObs";
        private const string PrefixSkuSaleCategoryInfo = "Cache_SKU_Category_ForObs";
        #endregion

        private static void Main(string[] args)
        {
            //CacheHelper.Add("name","zcy",DateTime.Now.AddDays(1));
            var stopWatch = new Stopwatch();
            stopWatch.Reset();
            stopWatch.Start();
            Console.WriteLine(CacheHelper.Get("name"));
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);

            #region ########## 普通Key-Value ######################

            //CacheHelper.Add("productInfo", "Miook星纱炫舞甲油 金棕色", DateTime.Now.AddMinutes(5));
            //Console.WriteLine(CacheHelper.Get("productInfo"));
            //var original = CacheHelper.GetOriginal<byte[]>("productInfo", "productInfo");
            //Console.WriteLine(Compression.DecompressAndDeserialze(original));

            //Console.WriteLine("SKU:" + CacheHelper.Exists("Cache_SKU_ForObsInfo_00097548"));

            //CacheHelper.Add("MaxStyleId", "15245", DateTime.Now.AddMinutes(5));
            //Console.WriteLine(CacheHelper.Get("MaxStyleId"));
            //Console.WriteLine(CacheHelper.Exists("MaxStyleId"));
            //Console.WriteLine(CacheHelper.Exists("maxStyleId"));

            #endregion

            #region ########## Value为实体对象 ######################

            //CacheHelper.Add("productInfo_0157203",
            //                new ProductInfoModel {ProductCode = "0157203", ProductName = "Miook星纱炫舞甲油 金棕色", Price = 129},
            //                DateTime.Now.AddMinutes(5));
            ////var productInfo = CacheHelper.Get("productInfo_0157203") as ProductInfoModel;
            ////if (productInfo != null)
            ////{
            ////    Console.WriteLine(string.Format("ProductCode:{0}",productInfo.ProductCode));
            ////    Console.WriteLine(string.Format("ProductName:{0}", productInfo.ProductName));
            ////    Console.WriteLine(string.Format("Price:{0}", productInfo.Price));
            ////}

            #endregion

            #region #################### Value为List #################

            ////var productCodes = new List<string> { "0180543", "0180544", "0180545" };
            //CacheHelper.Remove("productCodes");
            //CacheHelper.Push("productCodes", "0180543", DateTime.Now.AddMinutes(5));
            //CacheHelper.Push("productCodes", "0180544", DateTime.Now.AddMinutes(5));
            //CacheHelper.Push("productCodes", "0180545", DateTime.Now.AddMinutes(5));
            //var list = CacheHelper.GetList("productCodes");
            //if(list.Count>0)
            //{
            //    foreach (var v in list)
            //    {
            //        Console.WriteLine("ProductCode:" + v);
            //    }
            //}
            //Console.WriteLine("List2:");
            //CacheHelper.RemoveItemFromList("productCodes","0180543");
            //var list2 = CacheHelper.GetList("productCodes");
            //if (list2.Count > 0)
            //{
            //    foreach (var v in list2)
            //    {
            //        Console.WriteLine("ProductCode:" + v);
            //    }
            //}

            //Console.WriteLine("List3:");
            //CacheHelper.Add("1", "1", DateTime.Now.AddHours(1));
            //CacheHelper.Add("2", "2", DateTime.Now.AddHours(1));

            //var list3 = CacheHelper.GetAll<string>(new List<string> { "1","2" },null);
            // if (list3!=null && list3.Count > 0)
            //{
            //    foreach (var v in list3)
            //    {
            //        Console.WriteLine("ProductCode:" + v);
            //    }
            //}
            #endregion 

            #region ########## Value为List实体对象 ######################

            //var productList = new List<ProductInfoModel>();
            //productList.Add(new ProductInfoModel { ProductCode = "0157203", ProductName = "Miook星纱炫舞甲油 金棕色", Price = 129 });
            //productList.Add(new ProductInfoModel { ProductCode = "0036983", ProductName = "Miook奢华铆钉时尚休闲女鞋 粉色", Price = 79 });
            //CacheHelper.Add("productInfos",productList,DateTime.Now.AddMinutes(5));
            //var productInfos = CacheHelper.Get("productInfos") as List<ProductInfoModel>;
            //if (productInfos != null)
            //{
            //    foreach (var productInfoModel in productInfos)
            //    {
            //        Console.WriteLine(string.Format("ProductCode:{0}", productInfoModel.ProductCode));
            //        Console.WriteLine(string.Format("ProductName:{0}", productInfoModel.ProductName));
            //        Console.WriteLine(string.Format("Price:{0}", productInfoModel.Price));
            //    }
            //}

            #endregion

            #region ############## 应用实例：获取款式对应的产品线 ################
            //var sw= new Stopwatch();
            //sw.Start();
            //var dal = new ObsServiceDal();
            ////var serials = new List<string> { "39042", "39043", "39044", "39045", "39046" };
            //var serials = new List<string> { "39047", "39048", "39049", "39050", "39051" };
            //var productLines = dal.GetSerialLine(serials);
            //foreach (var productInfoModel in productLines)
            //{
            //    Console.WriteLine(string.Format("ProductSerialCode:{0}", productInfoModel.ProductSerialCode));
            //    Console.WriteLine(string.Format("ProductSerialName:{0}", productInfoModel.ProductSerialName));
            //    Console.WriteLine(string.Format("ProductLine:{0}", productInfoModel.ProductLine));
            //}
            //sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds.ToString());
            #endregion


            //var cache = CacheHelper.Get("Cache_SKU_BaseInfo_ForObs_95611001");
            //var cache = CacheHelper.Get("Cache_Serial_ProductLine_ForObs_9451");

            var cache = CacheHelper.Get(string.Format("{0}_{1}", PrefixSkuStyleInfo, "7153215"));
            Console.WriteLine(cache.ToString());
            cache = CacheHelper.Get(string.Format("{0}_{1}", PrefixSkuSaleCategoryInfo, "357")) as List<SalesCategoryDTO>;//Cache_SKU_Category_ForObs_357
            cache = CacheHelper.Get(string.Format("{0}_{1}", PrefixSkuProductLineInfo, "403"));
            cache = CacheHelper.Get(string.Format("{0}_{1}", PrefixSkuBaseInfo, "71532152"));
            Console.WriteLine(cache.ToString());
            
            Console.ReadLine();
        }

        [Serializable]
        public class SalesCategoryDTO
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}
