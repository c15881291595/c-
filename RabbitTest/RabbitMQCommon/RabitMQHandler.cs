using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitMQCommon
{
    public class RabitMQHandler
    {
        /// <summary>
        /// 添加队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="rabbitMqConfig"></param>
        public static void AddQueue<T>(T entity, RabbitMQConfig rabbitMqConfig)
        {
            RabitMQLib.AddQueue(rabbitMqConfig, entity);
        }
        /// <summary>
        /// 添加队列 使用默认设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public static void AddQueue<T>(T entity)
        {
            RabbitMQConfig rabbitMqConfig = RabbitMQConfigManagement.GetDefaultConfig();
            RabitMQLib.AddQueue(rabbitMqConfig, entity);
        }
        /// <summary>
        /// 获取队列一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rabbitMqConfig"></param>
        /// <returns></returns>
        public static T GetQueue<T>(RabbitMQConfig rabbitMqConfig)
        {
            return RabitMQLib.GetQueue<T>(rabbitMqConfig);
        }
        /// <summary>
        /// 获取队列一个数据  使用默认设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetQueue<T>()
        {
            RabbitMQConfig rabbitMqConfig = RabbitMQConfigManagement.GetDefaultConfig();
            return RabitMQLib.GetQueue<T>(rabbitMqConfig);
        }
        /// <summary>
        /// 队列委托处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        public static void QueueDequeueHanlder<T>(Func<T, bool> func)
        {
            RabbitMQConfig rabbitMqConfig = RabbitMQConfigManagement.GetDefaultConfig();
            RabitMQLib.QueueDequeueHanlder<T>(rabbitMqConfig, func);
        }
        /// <summary>
        /// 队列委托处理 使用默认设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="rabbitMqConfig"></param>
        public static void QueueDequeueHanlder<T>(Func<T, bool> func, RabbitMQConfig rabbitMqConfig)
        {
            RabitMQLib.QueueDequeueHanlder<T>(rabbitMqConfig, func);
        }
    }
}
