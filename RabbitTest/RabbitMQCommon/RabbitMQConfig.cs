using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace RabbitMQCommon
{
    public class RabbitMQConfig
    {
        /// <summary>
        /// 路由
        /// </summary>
        public string exchange { get; set; }
        /// <summary>
        /// 交换模式 direct fanout topic
        /// fanout

        ///   所有bind到此exchange的queue都可以接收消息

        ///   direct

        ///通过routingKey和exchange决定的那个唯一的queue可以接收消息

        ///topic

        ///所有符合routingKey(此时可以是一个表达式)的routingKey所bind的queue可以接收消息
        /// </summary>
        public string exchangeType { get; set; }
        /// <summary>
        /// 路由关键字
        /// </summary>
        public string routingKey { get; set; }
        /// <summary>
        /// 服务地址 禁止修改 从配置读取
        /// </summary>
        public string serverAddress { get; set; }
        /// <summary>
        /// 服务端口 禁止修改 从配置读取
        /// </summary>
        public int serverPort { get; set; }
        /// <summary>
        /// 用户名 禁止修改 从配置读取
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 密码 禁止修改 从配置读取
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 虚拟路由
        /// </summary>
        public string virtualHost { get; set; }
        /// <summary>
        /// 是否持久化
        /// </summary>
        public bool persistMode { get; set; }

        public string queuename { get; set; }

        private byte _cupriority;
        /// <summary>
        /// 设置的消息优先级 默认0-10
        /// </summary>
        public byte cupriority
        {
            get { return _cupriority; }
            set
            {
                if (value > 10)
                {
                    throw new ArgumentException("cupriority set must 0-10");
                }
                this._cupriority = value;
            }
        }

        private int _priority = 0;

        /// <summary>
        /// 队列的priority值  注：一定要和队列里的优先级值完全相等 且为int类型
        /// </summary>
        public int priority
        {
            get { return _priority; }
            set
            {
                if (value < 0 || value > 10)
                {
                    throw new ArgumentException("priority set must 0-10");
                }
                this._priority = value;
            }
        }
    }

    public class RabbitMQConfigManagement
    {
        //private static RabbitMQConfig rabbitMqConfig = new RabbitMQConfig()
        //{
        //    exchange = ConfigurationManager.AppSettings["rabbitexchange"] ?? "routing",
        //    exchangeType = ConfigurationManager.AppSettings["rabbitexchangeType"] ?? "direct",
        //    routingKey = ConfigurationManager.AppSettings["rabbitroutingKey"] ?? "rk",
        //    serverAddress = ConfigurationManager.AppSettings["rabbitserver"],
        //    serverPort = int.Parse((ConfigurationManager.AppSettings["rabbitport"] ?? "5672").ToString()),
        //    userName = ConfigurationManager.AppSettings["rabbitusername"],
        //    password = ConfigurationManager.AppSettings["rabbituserpwd"],
        //    persistMode = bool.Parse(ConfigurationManager.AppSettings["rabbitpersistMode"] ?? "true"),
        //    virtualHost = ConfigurationManager.AppSettings["rabbitvirtualHost"] ?? "/",
        //    queuename = ConfigurationManager.AppSettings["rabbitqueuename"],
        //};
        public static RabbitMQConfig GetDefaultConfig()
        {
            //return rabbitMqConfig;
            return new RabbitMQConfig()
            {
                exchange = ConfigurationManager.AppSettings["rabbitexchange"] ?? "routing",
                exchangeType = ConfigurationManager.AppSettings["rabbitexchangeType"] ?? "direct",
                routingKey = ConfigurationManager.AppSettings["rabbitroutingKey"] ?? "rk",
                serverAddress = ConfigurationManager.AppSettings["rabbitserver"],
                serverPort = int.Parse((ConfigurationManager.AppSettings["rabbitport"] ?? "5672").ToString()),
                userName = ConfigurationManager.AppSettings["rabbitusername"],
                password = ConfigurationManager.AppSettings["rabbituserpwd"],
                persistMode = bool.Parse(ConfigurationManager.AppSettings["rabbitpersistMode"] ?? "true"),
                virtualHost = ConfigurationManager.AppSettings["rabbitvirtualHost"] ?? "/",
                queuename = ConfigurationManager.AppSettings["rabbitqueuename"],
            };
        }
    }
    public class RabbitMQPrivate
    {
        public static readonly int mqtheardcount = int.Parse(ConfigurationManager.AppSettings["rabbitthreadcount"] ?? "2");
    }
}
