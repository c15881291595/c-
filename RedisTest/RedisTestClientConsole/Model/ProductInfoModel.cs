using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisTestClientConsole.Model
{
    [Serializable]
    public class ProductInfoModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int ProductSerialCode { get; set; }
        public string ProductSerialName { get; set; }
        public string ProductLine { get; set; }
    }
}
