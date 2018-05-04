using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Content;

namespace RabbitMQClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string exchange = "routing";
            string exchangeType = "direct";
            string routingKey = "rk";

            string serverAddress = "amqp://127.0.0.1:5672/";

            ConnectionFactory cf = new ConnectionFactory();
            cf.Uri = serverAddress;

            cf.UserName = "chenguo";
            cf.Password = "chenguo";
            cf.VirtualHost = "/guo";//vhost  
            cf.RequestedHeartbeat = 0;
            //cf.Endpoint = new AmqpTcpEndpoint(uri);  
            using (IConnection conn = cf.CreateConnection())
            {
                using (IModel ch = conn.CreateModel())
                {
                    //普通使用方式BasicGet  
                    //noAck = true，不需要回复，接收到消息后，queue上的消息就会清除  
                    //noAck = false，需要回复，接收到消息后，queue上的消息不会被清除，  
                    //直到调用channel.basicAck(deliveryTag, false);   
                    //queue上的消息才会被清除 而且，在当前连接断开以前，其它客户端将不能收到此queue上的消息  

                    BasicGetResult res = ch.BasicGet("q", false/*noAck*/);
                    if (res != null)
                    {
                        Console.WriteLine(System.Text.UTF8Encoding.UTF8.GetString(res.Body));
                        ch.BasicAck(res.DeliveryTag, false);

                    }
                    else
                    {
                        Console.WriteLine("无内容!!");
                    }
                    ch.Close();
                }
                conn.Close();


            }

            Console.ReadLine();
        }
    }
}
