using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQCommon;

namespace DTCash.InvokeMQ
{
    public class InvokeMQ
    {
        public static void InsertInvoke(Dictionary<string, object> dictionary)
        {
            RabbitMQConfig rabbitMqConfig = RabbitMQConfigManagement.GetDefaultConfig();
            rabbitMqConfig.queuename = "invokemq";
            rabbitMqConfig.routingKey = "dthinvoke";

            //优先级设置
            if (dictionary.ContainsKey("__priority") && dictionary["__priority"] != null)
            {

                //优先级设置
                if (dictionary.ContainsKey("__cupriority") && dictionary["__cupriority"] != null)
                {
                    byte cu;
                    if (byte.TryParse(dictionary["__cupriority"].ToString(), out cu))
                    {
                        rabbitMqConfig.cupriority = cu;
                    }
                }
                int p;
                if (int.TryParse(dictionary["__priority"].ToString(), out p))
                {
                    rabbitMqConfig.priority = p;
                }

            }
            RabitMQHandler.AddQueue(dictionary, rabbitMqConfig);
        }
    }
}
