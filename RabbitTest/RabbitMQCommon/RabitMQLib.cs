using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Content;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQCommon
{
    public class RabitMQLib
    {
        /// <summary>
        /// 获取队列工厂
        /// </summary>
        /// <param name="rabbitMqConfig"></param>
        /// <returns></returns>
        private static ConnectionFactory GetConnectionFactory(RabbitMQConfig rabbitMqConfig)
        {
            ConnectionFactory cf = new ConnectionFactory();

            cf.UserName = rabbitMqConfig.userName;//某个vhost下的用户  
            cf.Password = rabbitMqConfig.password;
            cf.VirtualHost = rabbitMqConfig.virtualHost;//vhost  
            cf.RequestedHeartbeat = 0;
            cf.HostName = rabbitMqConfig.serverAddress;
            cf.Port = rabbitMqConfig.serverPort;
            return cf;
        }
        //  private static List<IConnection> iconnlist = null;
        private static object connlock = new object();
        //private static Random random = new Random();
        //private static IConnection GetConn(RabbitMQConfig rabbitMqConfig)
        //{
        //    if (iconnlist == null)
        //    {
        //        lock (connlock)
        //        {
        //            if (iconnlist == null)
        //            {
        //                List<IConnection> iconnlisttemp = new List<IConnection>();
        //                for (var i = 0; i < RabbitMQPrivate.mqtheardcount; i++)
        //                {
        //                    iconnlisttemp.Add(GetConnectionFactory(rabbitMqConfig).CreateConnection());
        //                }
        //                iconnlist = iconnlisttemp;
        //            }
        //        }
        //    }
        //    return iconnlist[random.Next(0, RabbitMQPrivate.mqtheardcount)];
        //}


        //  private static bool isstart = false;
        private static Queue<RabbitThreadQueueDTO> ob = new Queue<RabbitThreadQueueDTO>();
        private static Dictionary<string, IModel> dictionary = new Dictionary<string, IModel>();
        //private static void ThreadRabbit()
        //{
        //    ThreadPool.QueueUserWorkItem(a =>
        //    {
        //        while (true)
        //        {
        //            if (ob.Count > 0)
        //            {
        //                lock (connlock)
        //                {
        //                    try
        //                    {
        //                        RabbitThreadQueueDTO t = ob.Dequeue();
        //                        if (t == null) continue;
        //                        if (dictionary.ContainsKey(GetKeyByRabbitMQConfig(t.RabbitMqConfig)))
        //                        {
        //                            if (dictionary[GetKeyByRabbitMQConfig(t.RabbitMqConfig)].IsOpen)
        //                            {
        //                                var propertiess =
        //                                    dictionary[GetKeyByRabbitMQConfig(t.RabbitMqConfig)].CreateBasicProperties();
        //                                ///构造消息实体对象并发布到消息队列上   
        //                                if (t.RabbitMqConfig.persistMode)
        //                                {
        //                                    propertiess.DeliveryMode = 2;
        //                                    //设定传输模式  
        //                                }
        //                                dictionary[GetKeyByRabbitMQConfig(t.RabbitMqConfig)].BasicPublish(
        //                                    t.RabbitMqConfig.exchange, t.RabbitMqConfig.routingKey,
        //                                    propertiess, Encoding.UTF8.GetBytes(t.Data));
        //                                continue;
        //                            }
        //                            else
        //                            {
        //                                dictionary.Remove(GetKeyByRabbitMQConfig(t.RabbitMqConfig));
        //                            }
        //                        }
        //                        IModel ch = GetConnectionFactory(t.RabbitMqConfig).CreateConnection().CreateModel();
        //                        if (t.RabbitMqConfig.exchangeType != null)
        //                        {//声明一个路由  
        //                            ch.ExchangeDeclare(t.RabbitMqConfig.exchange, t.RabbitMqConfig.exchangeType);
        //                            //声明一个队列  
        //                            ch.QueueDeclare(t.RabbitMqConfig.queuename, true, false, false, null);
        //                            //将一个队列和一个路由绑定起来。并制定路由关键字    
        //                            ch.QueueBind(t.RabbitMqConfig.queuename, t.RabbitMqConfig.exchange, t.RabbitMqConfig.routingKey);
        //                        }
        //                        ch.BasicQos(0, 1, false);
        //                        var properties = ch.CreateBasicProperties();
        //                        ///构造消息实体对象并发布到消息队列上   
        //                        if (t.RabbitMqConfig.persistMode)
        //                        {
        //                            properties.DeliveryMode = 2;
        //                            //设定传输模式  
        //                        }
        //                        //写入  
        //                        ch.BasicPublish(t.RabbitMqConfig.exchange, t.RabbitMqConfig.routingKey, properties, Encoding.UTF8.GetBytes(t.Data));
        //                        dictionary.Add(GetKeyByRabbitMQConfig(t.RabbitMqConfig), ch);
        //                    }
        //                    catch (Exception exception)
        //                    {
        //                        isstart = false;
        //                        throw exception;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Thread.Sleep(1000);
        //            }
        //        }
        //    });
        //}

        private static string GetKeyByRabbitMQConfig(RabbitMQConfig rabbitMqConfig)
        {
            return
                $"{rabbitMqConfig.queuename}{rabbitMqConfig.routingKey}{rabbitMqConfig.exchange}{rabbitMqConfig.exchangeType}{rabbitMqConfig.virtualHost}";
        }

        /// <summary>
        /// 添加队列
        /// </summary>
        /// <param name="rabbitThreadQueueDTO"></param>
        private static void AddQueue(RabbitThreadQueueDTO rabbitThreadQueueDTO)
        {
            lock (connlock)
            {
                //if (!isstart)
                //{
                //    isstart = true;
                //    ThreadRabbit();
                //}
                //ob.Enqueue(rabbitThreadQueueDTO);
                if (dictionary.ContainsKey(GetKeyByRabbitMQConfig(rabbitThreadQueueDTO.RabbitMqConfig)))
                {
                    if (dictionary[GetKeyByRabbitMQConfig(rabbitThreadQueueDTO.RabbitMqConfig)].IsOpen)
                    {
                        try
                        {
                            var propertiess =
                                                dictionary[GetKeyByRabbitMQConfig(rabbitThreadQueueDTO.RabbitMqConfig)].CreateBasicProperties();
                            //构造消息实体对象并发布到消息队列上   
                            if (rabbitThreadQueueDTO.RabbitMqConfig.persistMode)
                            {
                                propertiess.DeliveryMode = 2;
                                //设定传输模式  
                            }
                            //设置优先级
                            propertiess.Priority = rabbitThreadQueueDTO.RabbitMqConfig.cupriority;
                            dictionary[GetKeyByRabbitMQConfig(rabbitThreadQueueDTO.RabbitMqConfig)].BasicPublish(
                                rabbitThreadQueueDTO.RabbitMqConfig.exchange, rabbitThreadQueueDTO.RabbitMqConfig.routingKey,
                                propertiess, Encoding.UTF8.GetBytes(rabbitThreadQueueDTO.Data));
                        }
                        catch (Exception exception)
                        {
                            dictionary.Remove(GetKeyByRabbitMQConfig(rabbitThreadQueueDTO.RabbitMqConfig));
                            throw exception;
                        }
                        return;
                    }
                    else
                    {
                        dictionary.Remove(GetKeyByRabbitMQConfig(rabbitThreadQueueDTO.RabbitMqConfig));
                    }
                }
                IModel ch = GetConnectionFactory(rabbitThreadQueueDTO.RabbitMqConfig).CreateConnection().CreateModel();
                if (rabbitThreadQueueDTO.RabbitMqConfig.exchangeType != null)
                {//声明一个路由  
                    ch.ExchangeDeclare(rabbitThreadQueueDTO.RabbitMqConfig.exchange, rabbitThreadQueueDTO.RabbitMqConfig.exchangeType);
                    //声明一个队列  
                    if (rabbitThreadQueueDTO.RabbitMqConfig.priority > 0)
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("x-max-priority", rabbitThreadQueueDTO.RabbitMqConfig.priority);
                        ch.QueueDeclare(rabbitThreadQueueDTO.RabbitMqConfig.queuename, true, false, false, dic);
                    }
                    else
                    {
                        ch.QueueDeclare(rabbitThreadQueueDTO.RabbitMqConfig.queuename, true, false, false, null);
                    }
                    //将一个队列和一个路由绑定起来。并制定路由关键字    
                    ch.QueueBind(rabbitThreadQueueDTO.RabbitMqConfig.queuename, rabbitThreadQueueDTO.RabbitMqConfig.exchange, rabbitThreadQueueDTO.RabbitMqConfig.routingKey);
                }
                ch.BasicQos(0, 1, false);
                var properties = ch.CreateBasicProperties();
                //构造消息实体对象并发布到消息队列上   
                if (rabbitThreadQueueDTO.RabbitMqConfig.persistMode)
                {
                    properties.DeliveryMode = 2;
                    //设定传输模式  
                }
                //设置优先级
                properties.Priority = rabbitThreadQueueDTO.RabbitMqConfig.cupriority;
                //写入  
                ch.BasicPublish(rabbitThreadQueueDTO.RabbitMqConfig.exchange, rabbitThreadQueueDTO.RabbitMqConfig.routingKey, properties, Encoding.UTF8.GetBytes(rabbitThreadQueueDTO.Data));
                dictionary.Add(GetKeyByRabbitMQConfig(rabbitThreadQueueDTO.RabbitMqConfig), ch);
            }
        }


        /// <summary>
        /// 添加队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rabbitMqConfig"></param>
        /// <param name="entity"></param>
        public static void AddQueue<T>(RabbitMQConfig rabbitMqConfig, T entity)
        {
            try
            {

                AddQueue(new RabbitThreadQueueDTO() { Data = RabbitMQLibCommon.Serialize(entity), RabbitMqConfig = rabbitMqConfig });
                ////创建并返回一个新连接到具体节点的通道  
                //using (IModel ch = GetConn(rabbitMqConfig).CreateModel())
                //{
                //    if (rabbitMqConfig.exchangeType != null)
                //    {//声明一个路由  
                //        ch.ExchangeDeclare(rabbitMqConfig.exchange, rabbitMqConfig.exchangeType);
                //        //声明一个队列  
                //        ch.QueueDeclare(rabbitMqConfig.queuename, true, false, false, null);
                //        //将一个队列和一个路由绑定起来。并制定路由关键字    
                //        ch.QueueBind(rabbitMqConfig.queuename, rabbitMqConfig.exchange, rabbitMqConfig.routingKey);
                //    }
                //    ch.BasicQos(0, 1, false);
                //    var properties = ch.CreateBasicProperties();
                //    ///构造消息实体对象并发布到消息队列上   
                //    if (rabbitMqConfig.persistMode)
                //    {
                //        properties.DeliveryMode = 2;
                //        //设定传输模式  
                //    }
                //    //写入  
                //    ch.BasicPublish(rabbitMqConfig.exchange, rabbitMqConfig.routingKey, properties, Encoding.UTF8.GetBytes(RabbitMQLibCommon.Serialize(entity)));
                //    //  ch.Close();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取一个数据 若无数据会阻塞 注意使用方式 若设定了优先级 则必须设置priority 且要和队列相等 否则异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rabbitMqConfig"></param>
        /// <returns></returns>
        public static T GetQueue<T>(RabbitMQConfig rabbitMqConfig)
        {
            T t = default(T);
            try
            {
                using (IModel ch = GetConnectionFactory(rabbitMqConfig).CreateConnection().CreateModel())
                {
                    if (rabbitMqConfig.exchangeType != null)
                    {//声明一个路由  
                        ch.ExchangeDeclare(rabbitMqConfig.exchange, rabbitMqConfig.exchangeType);
                        //声明一个队列  
                        if (rabbitMqConfig.priority > 0)
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("x-max-priority", rabbitMqConfig.priority);
                            ch.QueueDeclare(rabbitMqConfig.queuename, true, false, false, dic);
                        }
                        else
                        {
                            ch.QueueDeclare(rabbitMqConfig.queuename, true, false, false, null);
                        }

                        //将一个队列和一个路由绑定起来。并制定路由关键字    
                        ch.QueueBind(rabbitMqConfig.queuename, rabbitMqConfig.exchange, rabbitMqConfig.routingKey);
                    }
                    BasicGetResult res = ch.BasicGet(rabbitMqConfig.queuename, true);
                    if (res != null)
                    {
                        var message = Encoding.UTF8.GetString(res.Body);
                        t = RabbitMQLibCommon.DeSerialize<T>(message);
                    }
                    else
                    {
                    }
                    // ch.Close();
                }
                return t;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 队列自动处理数据 传入一个委托 若设定了优先级 则必须设置priority 且要和队列里的priority相等 否则异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rabbitMqConfig"></param>
        /// <param name="func"></param>
        public static void QueueDequeueHanlder<T>(RabbitMQConfig rabbitMqConfig, Func<T, bool> func)
        {
            try
            {
                using (IModel ch = GetConnectionFactory(rabbitMqConfig).CreateConnection().CreateModel())
                {
                    if (rabbitMqConfig.exchangeType != null)
                    {//声明一个路由  
                        ch.ExchangeDeclare(rabbitMqConfig.exchange, rabbitMqConfig.exchangeType);
                        //声明一个队列  
                        //若传值 则以传为准 否则以配置为准
                        if (rabbitMqConfig.priority > 0)
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("x-max-priority", rabbitMqConfig.priority);
                            ch.QueueDeclare(rabbitMqConfig.queuename, true, false, false, dic);
                        }
                        else
                        {
                            ch.QueueDeclare(rabbitMqConfig.queuename, true, false, false, null);
                        }

                        //将一个队列和一个路由绑定起来。并制定路由关键字    
                        ch.QueueBind(rabbitMqConfig.queuename, rabbitMqConfig.exchange, rabbitMqConfig.routingKey);
                    }
                    ch.BasicQos(0, 2, false);
                    var consumer = new QueueingBasicConsumer(ch);
                    ch.BasicConsume(rabbitMqConfig.queuename, false, consumer);//设置主动获取模式
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        if (message.StartsWith("\"{") && message.EndsWith("}\""))
                        {
                            message = message.Remove(0, 1);
                            message = message.Remove(message.Length - 1, 1);
                        }
                        // ch.BasicAck(ea.DeliveryTag, false);//设置响应

                        var entity = RabbitMQLibCommon.DeSerialize<T>(message);
                        if (func(entity))
                        {
                            ch.BasicAck(ea.DeliveryTag, false);
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class RabbitThreadQueueDTO
    {
        public RabbitMQConfig RabbitMqConfig { get; set; }
        public string Data { get; set; }
    }
}
