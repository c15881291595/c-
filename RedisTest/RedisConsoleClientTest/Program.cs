using System;
using System.Collections.Generic;
using ServiceStack.Redis;
using ServiceStack.Redis.Support;
using ServiceStack.Text;

namespace RedisConsoleClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new RedisClient("10.253.6.140", 6379))
            {
                #region ########## 普通的KEY-VALUE ##############

                Console.WriteLine("########## 普通的KEY-VALUE ##############");

                client.Set("username", "leepy_73");
                client.Set("age", 73, new TimeSpan(0, 1, 10));
                Console.WriteLine(client.Get<int>("age"));
                Console.WriteLine(client.Ttl("age"));
                Console.WriteLine();

                #endregion

                #region ############# SET 操作 #############

                Console.WriteLine("########## SET 操作 ##############");

                client.AddItemToSet("sName", "wy");
                client.AddItemToSet("sName", "rbg");
                client.AddItemToSet("sName", "zcy");
                var sname = client.GetAllItemsFromSet("sName");
                foreach (var name in sname)
                {
                    Console.WriteLine(name);
                }
                Console.WriteLine();

                #endregion

                #region ############### ZSET操作 ################

                Console.WriteLine("########## ZSET 操作 ##############");
                client.AddItemToSortedSet("ssName", "zcy");
                client.AddItemToSortedSet("ssName", "wy");
                client.AddItemToSortedSet("ssName", "rbg");
                var ssName = client.GetAllItemsFromSortedSet("ssName");
                foreach (var name in ssName)
                {
                    Console.WriteLine(name);
                }
                Console.WriteLine();

                #endregion
                //Console.WriteLine(client.GetSetCount("sName"));

                #region ############ LIST操作 ###########################

                Console.WriteLine("########## LIST 操作 ##############");
                var productCodeList = new List<string> { "0041920", "0041927", "0028738", "0043652", "0030981", "0030999" };
                client.AddRangeToList("productCodes", productCodeList);
                var products = client.GetRangeFromList("productCodes", 2, 4);
                foreach (var product in products)
                {
                    Console.WriteLine(product);
                }
                Console.WriteLine();


                var productInfos = new List<ProductInfoDto>();
                productInfos.Add(new ProductInfoDto { ProductCode = "0180960", ProductName = "彩色风尚必备铅笔裤 姜黄色", Price = 129 });
                productInfos.Add(new ProductInfoDto { ProductCode = "0180961", ProductName = "彩色风尚必备铅笔裤 玫瑰红", Price = 129 });
                productInfos.Add(new ProductInfoDto { ProductCode = "0180962", ProductName = "彩色风尚必备铅笔裤 黑色", Price = 129 });
                productInfos.Add(new ProductInfoDto { ProductCode = "0180963", ProductName = "彩色风尚必备铅笔裤 橙色", Price = 129 });
                var obj = new ObjectSerializer();
                client.Set<byte[]>("productInfos", obj.Serialize(productInfos));
                //client.Save();
                var productList = obj.Deserialize(client.Get<byte[]>("productInfos")) as List<ProductInfoDto>;
                if (productList != null)
                {
                    foreach (var productInfoDto in productList)
                    {
                        Console.WriteLine(productInfoDto.ProductCode + productInfoDto.ProductName + productInfoDto.Price);
                    }
                }
                Console.WriteLine();

                #endregion

                Console.ReadLine();
            }
        }

        [Serializable]
        public class ProductInfoDto
        {
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
        }
    }
}
