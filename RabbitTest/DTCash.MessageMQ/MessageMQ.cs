using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RabbitMQCommon;

namespace DTCash.MessageMQ
{
    public class MessageMQ
    {
        private static readonly string messagecmdid = ConfigurationManager.AppSettings["messagecmdid"] ?? "";

        /// <summary>
        /// 发送短信 //不包含标记
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <param name="cmdid">parms as dth or jgj</param>
        public static void InsertNote(string mobile, string content)
        {
            InsertNote(mobile, content, messagecmdid, null);
        }
        /// <summary>
        /// 发送短信 //不包含标记
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <param name="cmdid">parms as dth or jgj</param>
        public static void InsertNote(string mobile, string content, Dictionary<string, object> dictionary)
        {
            InsertNote(mobile, content, messagecmdid, dictionary);
        }
        /// <summary>
        /// 发送短信 //不包含标记
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <param name="cmdid">parms as dth or jgj</param>
        public static void InsertNote(string mobile, string content, string cmdid, Dictionary<string, object> dictionarys)
        {
            if (!CheckParms(mobile, "note"))
            {
                throw new Exception("parms is error");
            }
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("mobile", mobile);
            dictionary.Add("content", content);
            dictionary.Add("cmdid", cmdid);
            dictionary.Add("type", "note");
            if (dictionarys != null)
            {
                foreach (KeyValuePair<string, object> keyValuePair in dictionarys)
                {
                    if (dictionary.ContainsKey(keyValuePair.Key))
                    {
                        dictionary[keyValuePair.Key] = keyValuePair.Value;
                    }
                    else
                    {
                        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }
            AddMessage(dictionary);
        }

        /// <summary>
        /// 发送站内信
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="content"></param>
        /// <param name="cmdid">parms as dth or jgj</param>
        public static void InsertLetter(int userid, string content)
        {
            InsertLetter(userid, content, "", messagecmdid, null);
        }
        /// <summary>
        /// 发送站内信
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="content"></param>
        /// <param name="cmdid">parms as dth or jgj</param>
        public static void InsertLetter(int userid, string title, string content)
        {
            InsertLetter(userid, title, content, messagecmdid, null);
        }
        /// <summary>
        /// 发送站内信
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="content"></param>
        /// <param name="cmdid">parms as dth or jgj</param>
        public static void InsertLetter(int userid, string title, string content, Dictionary<string, object> dictionary)
        {
            InsertLetter(userid, title, content, messagecmdid, dictionary);
        }
        /// <summary>
        /// 发送站内信
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="content"></param>
        /// <param name="cmdid">parms as dth or jgj</param>
        public static void InsertLetter(int userid, string title, string content, string cmdid, Dictionary<string, object> dictionarys)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("userid", userid);
            dictionary.Add("content", content);
            dictionary.Add("cmdid", cmdid);
            dictionary.Add("title", title);
            dictionary.Add("type", "letter");
            if (dictionarys != null)
            {
                foreach (KeyValuePair<string, object> keyValuePair in dictionarys)
                {
                    if (dictionary.ContainsKey(keyValuePair.Key))
                    {
                        dictionary[keyValuePair.Key] = keyValuePair.Value;
                    }
                    else
                    {
                        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }
            AddMessage(dictionary);

        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email"></param>
        /// <param name="content"></param>
        /// <param name="cmdid">parms as dth or jgj</param>
        public static void InsertEmail(string email, string title, string content)
        {
            InsertEmail(title, email, content, messagecmdid, null);
        } /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email"></param>
        /// <param name="content"></param>
        /// <param name="cmdid">parms as dth or jgj</param>
        public static void InsertEmail(string email, string title, string content, Dictionary<string, object> dictionarys)
        {
            InsertEmail(title, email, content, messagecmdid, dictionarys);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email"></param>
        /// <param name="content"></param>
        /// <param name="cmdid">parms as dth or jgj</param>
        public static void InsertEmail(string title, string email, string content, string cmdid, Dictionary<string, object> dictionarys)
        {
            if (!CheckParms(email, "email"))
            {
                throw new Exception("parms is error");
            }
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("title", title);
            dictionary.Add("email", email);
            dictionary.Add("content", content);
            dictionary.Add("cmdid", cmdid);
            dictionary.Add("type", "email");
            if (dictionarys != null)
            {
                foreach (KeyValuePair<string, object> keyValuePair in dictionarys)
                {
                    if (dictionary.ContainsKey(keyValuePair.Key))
                    {
                        dictionary[keyValuePair.Key] = keyValuePair.Value;
                    }
                    else
                    {
                        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }
            AddMessage(dictionary);
        }

        /// <summary>
        /// 添加app推送
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void InsertAppPush(int userid, string title, string content)
        {
            InsertAppPush(userid, title, content, null);
        }
        /// <summary>
        /// 添加app推送
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void InsertAppPush(int userid, string title, string content, Dictionary<string, object> dictionarys)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("userid", userid);
            dictionary.Add("content", content);
            dictionary.Add("cmdid", messagecmdid);
            dictionary.Add("title", title);
            dictionary.Add("type", "apppush");
            if (dictionarys != null)
            {
                foreach (KeyValuePair<string, object> keyValuePair in dictionarys)
                {
                    if (dictionary.ContainsKey(keyValuePair.Key))
                    {
                        dictionary[keyValuePair.Key] = keyValuePair.Value;
                    }
                    else
                    {
                        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }
            AddMessage(dictionary);
        }
        private static void AddMessage(Dictionary<string, object> dictionary)
        {
            RabbitMQConfig rabbitMqConfig = RabbitMQConfigManagement.GetDefaultConfig();
            rabbitMqConfig.queuename = "messagemq";
            rabbitMqConfig.routingKey = "dthmessage";
            if (dictionary.ContainsKey("createtime"))
            {
                if (dictionary.ContainsKey("repeatcreatetime"))
                {
                    dictionary["repeatcreatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffff");
                }
                else
                {
                    dictionary.Add("repeatcreatetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffff"));
                }
            }
            else
            {
                dictionary.Add("createtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffff"));
            }
            //优先级设置
            if (dictionary.ContainsKey("__priority")&&dictionary["__priority"]!=null)
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
        private static Regex mobileregex = new Regex(@"^1[3-9]\d{9}$");
        private static Regex emailregex = new Regex(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$");
        private static bool CheckParms(string parms, string type)
        {
            if (string.IsNullOrEmpty(parms)) return false;
            if (type == "note")
            {
                return mobileregex.IsMatch(parms);
            }
            else if (type == "email")
            {
                return emailregex.IsMatch(parms);
            }
            return false;
        }
    }
}
