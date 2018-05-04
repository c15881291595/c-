using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServiceStack.Redis;
using ServiceStack.Redis.Support;

namespace RedisTestClient
{
    public partial class Form1 : Form
    {
        //private static readonly IRedisClientFactory RedisFactory = new RedisCacheClientFactory();//10.253.6.140
        private readonly RedisClient _redisClient = new RedisClient("192.168.1.202", 6379);//10.253.73.25:6379,10.253.74.25:6379

        public Form1()
        {
            InitializeComponent();
        }

        private void btnInvoke_Click(object sender, EventArgs e)
        {
            var username = _redisClient.Get<string>(txtKey.Text.Trim());
            txtKeyValue.Text = username + "|" + _redisClient.DbSize.ToString();
            var tmp = _redisClient.Get("Cache_SKU_Category_ForObs_9636");
            var tmp2 = CacheHelper.Get("Cache_SKU_Category_ForObs_9636");
            //var keys= _redisClient.Keys("Cache_SKU_Category_ForObs*");
            //var keys = _redisClient.SearchKeys("Cache_SKU_Category_ForObs_*");
            //var keylist = new StringBuilder();
            //foreach (var key in keys)
            //{
            //    keylist.AppendFormat("{0},", key);
            //}
            //txtKeyValue.Text = keylist.ToString();

            Console.ReadLine();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtKey.Focus();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            var products = _redisClient.GetAllItemsFromList("productCodes");
            txtKeyValue.Text = string.Join(",", products.ToArray());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            var obj = new ObjectSerializer();
            var products = obj.Deserialize(_redisClient.Get<byte[]>("productInfos")) as List<ProductInfoDto>;
            dgvResult.DataSource = products;
        }

        public class ProductInfoDto
        {
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
        }
    }
}
