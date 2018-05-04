using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace RabbitMQCommon
{
    public class RabbitMQLibCommon
    {
        /// <summary>
        /// 序列化
        /// </summary>
        public static string Serialize(object obj)
        {
            if (obj != null)
                return new JavaScriptSerializer().Serialize(obj);
            else return string.Empty;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return new JavaScriptSerializer().Deserialize<T>(str);
            }
            else
            {
                return default(T);
            }
        }
    }
}
