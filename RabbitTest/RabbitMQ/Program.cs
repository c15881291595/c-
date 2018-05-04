using System.Threading;
using DTCash.Service.CommonService;
using RabbitMQ.Client;
using RabbitMQ.Client.Content;
using RabbitMQCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {

            //Dictionary<string, object> dictionary = new Dictionary<string, object>();

            //dictionary.Add("chanle", "chuanglan");
            ////DTCash.MessageMQ.MessageMQ.InsertLetter(310957, "账户开通", "<p>恭喜您完成实名认证，成功开通第三方资金监管账户！资金安全有保障，保驾护航一辈子！</p>");
            //DTCash.MessageMQ.MessageMQ.InsertNote("13146189662", "恭喜您在邀请小伙伴活动获得现金返还0.13元，大同行会在您小伙伴第一笔回款时将奖励发送至您的账户，一起享受理财的乐趣吧！", dictionary);
            //dictionary = new Dictionary<string, object>();
            //dictionary.Add("cmdid", "registerservice");
            //dictionary.Add("userid", 292125);
            //dictionary.Add("sourceDTO", new DTCash.Service.CommonService.SourceDTO()
            //{
            //    SourceExtendValue = "5442",
            //    SourceExtendTypeID = SourceExtendType.User,
            //    SourceOperationID = SourceOperation.Login,
            //    SourceSiteID = DTCash.Service.CommonService.SourceSite.MasterStation,
            //    SourceTypeID = 0,
            //    Url = "http://www.dtcash.com:802/Login/UserLogin",
            //    UrlReferrer = "https://www.dtcash.com/Login/Index.html",
            //    UserID = 5442,
            //    UserIP = "39.78.102.174",
            //});
            //dictionary.Add("operator", new DTCash.Service.CommonService.OperatorDTO()
            //{
            //    OperatorHostName = "10.144.8.108",
            //    OperatorIP = "39.78.102.174",
            //    OperatorMac = "",
            //    OperatorType = 0,
            //    OperatorID = 5442,
            //    OperatorName = "15910723306",
            //});
            // DTCash.InvokeMQ.InvokeMQ.InsertInvoke(dictionary);


            Console.WriteLine("写入完成");
            //   Console.ReadKey();

            //RabbitMQConfig rabbitMqConfig = RabbitMQConfigManagement.GetDefaultConfig();
            //rabbitMqConfig.queuename = "invokemq";
            //rabbitMqConfig.routingKey = "dthinvoke";
            //RabitMQHandler.QueueDequeueHanlder<Dictionary<string, object>>(
            //    delegate(Dictionary<string, object> dictionarys)
            //    {
            //        Dictionary<string, object> a = dictionarys;
            //        return false;
            //    }, rabbitMqConfig);

            //RabbitMQConfig rabbitMqConfig = RabbitMQConfigManagement.GetDefaultConfig();
            //rabbitMqConfig.queuename = "messagemq";
            //rabbitMqConfig.routingKey = "dthmessage";
            //RabitMQHandler.QueueDequeueHanlder<Dictionary<string, object>>(
            //    delegate(Dictionary<string, object> dictionarys)
            //    {
            //        Dictionary<string, object> a = dictionarys;
            //        return false;
            //    }, rabbitMqConfig);

            //Console.ReadKey();


            //for (int i = 0; i < 100000; i++)
            //{
            //    RabitMQHandler.AddQueue(new Test()
            //    {
            //        id = "123",
            //        name = "guoguoguo",
            //        mobile = "123123213123123123",
            //        createtime = "9839128312983012983012983",
            //        ip = "321o3iu21o3j21kj312lj3",
            //        random = 2,
            //        type = 2
            //    });
            //    RabbitMQConfig rabbitMqConfig = RabbitMQConfigManagement.GetDefaultConfig();
            //    rabbitMqConfig.queuename = "queue1";
            //    rabbitMqConfig.routingKey = "dthmessage11";
            //    RabitMQHandler.AddQueue(new Test()
            //    {
            //        id = "123",
            //        name = "guoguoguo",
            //        mobile = "123123213123123123",
            //        createtime = "9839128312983012983012983",
            //        ip = "321o3iu21o3j21kj312lj3",
            //        random = 2,
            //        type = 2
            //    }, rabbitMqConfig);
            //    Console.WriteLine(i);
            //    if (i % 10000==0)
            //    {
            //        Thread.Sleep(10000);
            //    }
            //}
            //for (int i = 0; i < 100; i++)
            //{
            //    RabbitMQConfig rabbitMqConfig = RabbitMQConfigManagement.GetDefaultConfig();
            //    rabbitMqConfig.queuename = "queue1";
            //    rabbitMqConfig.routingKey = "dthmessage11";
            //    RabitMQHandler.AddQueue(new Test()
            //    {
            //        id = "123",
            //        name = "guoguoguo",
            //        mobile = "123123213123123123",
            //        createtime = "9839128312983012983012983",
            //        ip = "321o3iu21o3j21kj312lj3",
            //        random = 2,
            //        type = 2
            //    }, rabbitMqConfig);
            //}
            //Console.ReadKey();
            // return;
            for (var i = 0; i < 100; i++)
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                d.Add("__priority", 10);
                d.Add("__cupriority", i % 2 == 0 ? 5 : 0);
                DTCash.MessageMQ.MessageMQ.InsertNote("13111111111", $"内容{i}", d);
            }

            RabbitMQConfig rabbitMqConfig = RabbitMQConfigManagement.GetDefaultConfig();
            rabbitMqConfig.queuename = "messagemq1";
            rabbitMqConfig.routingKey = "dthmessage";
            rabbitMqConfig.priority = 10;
            RabitMQHandler.QueueDequeueHanlder<Dictionary<string, object>>(delegate (Dictionary<string, object> d)
            {
                foreach (KeyValuePair<string, object> keyValuePair in d)
                {
                    Console.WriteLine(keyValuePair.Key+"  "+keyValuePair.Value);
                }
                //Thread.Sleep(5000);
                return true;
            }, rabbitMqConfig);




            //RabbitMQConfig rabbitMqConfig = RabbitMQConfigManagement.GetDefaultConfig();
            //rabbitMqConfig.queuename = "invokemq";
            //rabbitMqConfig.routingKey = "dthinvoke";
            //RabitMQHandler.QueueDequeueHanlder<Dictionary<string, object>>(delegate(Dictionary<string, object> dictionary)
            //{
            //    foreach (KeyValuePair<string, object> keyValuePair in dictionary)
            //    {
            //        Console.WriteLine(keyValuePair.Key+"  "+keyValuePair.Value);
            //    }
            //    return 0;
            //}, rabbitMqConfig);
            Console.WriteLine("读取成功");
            Console.ReadKey();
            return;
            // Uri uri = new Uri("amqp://127.0.0.1:5672/");
            string exchange = "routing";//路由  
            string exchangeType = "direct";//交换模式  
            string routingKey = "rk";//路由关键字  
            //是否对消息队列持久化保存  
            bool persistMode = true;
            ConnectionFactory cf = new ConnectionFactory();

            cf.UserName = "chenguo";//某个vhost下的用户  
            cf.Password = "chenguo";
            cf.VirtualHost = "/guo";//vhost  
            cf.RequestedHeartbeat = 0;
            cf.HostName = "127.0.0.1";
            cf.Port = 5672;
            // cf.Endpoint = new AmqpTcpEndpoint(uri);
            using (IConnection conn = cf.CreateConnection())
            {             //创建并返回一个新连接到具体节点的通道  
                using (IModel ch = conn.CreateModel())
                {
                    if (exchangeType != null)
                    {//声明一个路由  
                        ch.ExchangeDeclare(exchange, exchangeType);
                        //声明一个队列  
                        ch.QueueDeclare("q", true, false, false, null);
                        //将一个队列和一个路由绑定起来。并制定路由关键字    
                        ch.QueueBind("q", exchange, routingKey);
                    }
                    ///构造消息实体对象并发布到消息队列上  
                    IMapMessageBuilder b = new MapMessageBuilder(ch);
                    IDictionary target = (IDictionary)b.Headers;
                    target["header"] = "hello world";
                    IDictionary targerBody = (IDictionary)b.Body;
                    targerBody["body"] = "hello world";//这个才是具体的发送内容  
                    if (persistMode)
                    {
                        ((IBasicProperties)b.GetContentHeader()).DeliveryMode = 2;
                        //设定传输模式  
                    }
                    //写入  
                    ch.BasicPublish(exchange, routingKey, (IBasicProperties)b.GetContentHeader(), b.GetContentBody());
                    Console.WriteLine("写入成功");
                    Console.ReadKey();
                }

            }

        }
    }

    [Serializable]
    public class Test
    {
        public string id { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public int type { get; set; }
        public int random { get; set; }
        public string createtime { get; set; }
        public string ip { get; set; }
    }

    public enum UserMessageOperation
    {
        No = 0,
        /// <summary>
        /// 大同行
        /// </summary>
        DTH = 1,
        /// <summary>
        /// 投资
        /// </summary>
        Investment = 2,
        /// <summary>
        /// 回款
        /// </summary>
        Back = 3,
        /// <summary>
        /// 充值
        /// </summary>
        Recharge = 4,
        /// <summary>
        /// 提现
        /// </summary>
        Withdrawals = 5,
        /// <summary>
        /// 买入
        /// </summary>
        Purchase = 6,
        /// <summary>
        /// 卖出
        /// </summary>
        SellOut = 7,
        /// <summary>
        /// 转账
        /// </summary>
        TransferAccounts = 8,

    }
}
