using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTCash.Redis;

namespace RedisTest
{
    class Program
    {
        static void Main(string[] args)
        {
            object[] array = { "aa", "bb", "cc" };
            //var a = CacheKey.Init().User.Price;
            //var b = CacheKey.Init("123").User.Price;
            //var c = CacheKey.Init(array).User.Price;
            //var d = CacheKey.Init("123", array).Global().User.Price;

            //var a = CacheKey.Init().User.Message;
            //var b = CacheKey.Init("123").User.Message;
            //var c = CacheKey.Init(array).User.Message;
            //var d = CacheKey.Init("123", array).Global().User.Message;

            //var a = CacheKey.Init().User.Auth;
            //var b = CacheKey.Init("123").User.Auth;
            //var c = CacheKey.Init(array).User.Auth;
            //var d = CacheKey.Init("123", array).Global().User.Auth;

            var a = CacheKey.Init().User.GiftCard;
            var b = CacheKey.Init("123").User.GiftCard;
            var c = CacheKey.Init(1,2,3).User.GiftCard;
            var d = CacheKey.Init("123", array).User.GiftCard.HideExt();

            CacheRedis.AddCache("Content" + DateTime.Now.ToString("yyyyMMddhhssff"), a);
            CacheRedis.AddCache("Content" + DateTime.Now.ToString("yyyyMMddhhssff"), b);
            CacheRedis.AddCache("Content" + DateTime.Now.ToString("yyyyMMddhhssff"), c);
            CacheRedis.AddCache("Content" + DateTime.Now.ToString("yyyyMMddhhssff"), d);

            Console.WriteLine(a.GetKey());
            Console.WriteLine(b.GetKey());
            Console.WriteLine(c.GetKey());
            Console.WriteLine(d.GetKey());
       
            Console.ReadKey();
        }
    }
}
