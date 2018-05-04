using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ServiceStack.Redis;

namespace RedisConsoleClientTest
{
    internal static class CacheHelper
    {
        public static readonly PooledRedisClientManager pool = null;
        static readonly string[] readWriteHosts = null;

        #region = 构造 =
        static CacheHelper()
        {
            var redisHost = System.Configuration.ConfigurationManager.AppSettings["RedisServers"];
            if (!string.IsNullOrEmpty(redisHost))
            {
                readWriteHosts = redisHost.Split(',');
                if (readWriteHosts.Length > 0)
                {
                    var config = new RedisClientManagerConfig()
                        {
                            MaxWritePoolSize = readWriteHosts.Length * 10,
                            MaxReadPoolSize = readWriteHosts.Length * 10,
                            AutoStart = true
                        };
                    pool = new PooledRedisClientManager(readWriteHosts, readWriteHosts, config, 0);
                }
            }
        } 
        #endregion

       private static T Base<T>(string key, Func<IRedisClient, T> func, string errorTag, string hashKey = null)
        {
            if (string.IsNullOrEmpty(key) )
                throw new ArgumentNullException("key");
            if (pool != null)
            {
                hashKey = hashKey ?? key;
                try
                {
                    using (var r = pool.GetClient(hashKey.RedisIndex()))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            return func(r);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = string.Format("{0}:{1}发生异常!{2}", "cache", errorTag, key);
                    //Logger.Log(typeof(CacheHelper), msg, ex);
                }
            }
            return default(T);
        }

       private static void Base(string key, Action<IRedisClient> action, string errorTag, string hashKey = null)
       {
           if (string.IsNullOrEmpty(key) )
               throw new ArgumentNullException("key");
           if (pool != null)
           {
               hashKey = hashKey ?? key;
               try
               {
                   using (var r = pool.GetClient(hashKey.RedisIndex()))
                   {
                       if (r != null)
                       {
                           r.SendTimeout = 1000;
                           action(r);
                       }
                   }
               }
               catch (Exception ex)
               {
                   string msg = string.Format("{0}:{1}发生异常!{2}", "cache", errorTag, key);
                   //Logger.Log(typeof(CacheHelper), msg, ex);
               }
           }
       }

        #region = Add =
        public static void Add(string key, object value, DateTime expiry, string hashKey = null)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (string.IsNullOrEmpty(key) )
                throw new ArgumentNullException("key");


            if (expiry <= DateTime.Now)
            {
                Remove(key, hashKey);
                return;
            }

            var bytes = Compression.SerializeAndCompress(value);

            if (bytes == null)
            {
                return;
            }

            try
            {
                if (pool != null)
                {
                    hashKey = hashKey ?? key;
                    using (var r = pool.GetClient(hashKey.RedisIndex()))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            if (expiry == DateTime.MaxValue)
                            {
                                r.Set(key, bytes);
                            }
                            else
                            {
                                r.Set(key, bytes, expiry - DateTime.Now);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
                //Logger.Log(typeof(CacheHelper), msg, ex);
            }
        }

        public static void Add(string key, object value, TimeSpan slidingExpiration, string hashKey)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (string.IsNullOrEmpty(key) )
                throw new ArgumentNullException("key");

            if (slidingExpiration.TotalSeconds <= 0)
            {
                Remove(key, hashKey);
                return;
            }

            byte[] bytes = Compression.SerializeAndCompress(value);
            if (bytes == null)
            {
                return;
            }
            try
            {
                if (pool != null)
                {
                    hashKey = hashKey ?? key;
                    using (var r = pool.GetClient(hashKey.RedisIndex()))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, bytes, slidingExpiration);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
                //Logger.Log(typeof(CacheHelper), msg, ex);
            }
        } 
        #endregion

        #region = Get =
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public static object Get(string key, string hashKey = null)
        {
            if (string.IsNullOrEmpty(key) )
                throw new ArgumentNullException("key");

            Byte[] bytes = null;
            try
            {
                if (pool != null)
                {
                    hashKey = hashKey ?? key;
                    using (var r  = pool.GetClient(hashKey.RedisIndex()))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            bytes = r.Get<byte[]>(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", key);
                //Logger.Log(typeof(CacheHelper), msg, ex);
            }
            if (bytes != null)
            {
                var r = BytesToObject(key, bytes, hashKey);
                if (r == null)
                {
                    Remove(key, hashKey);
                }
                return r;
            }

            return null;
        } 
        #endregion

        #region = Exists =
        /// <summary>
        /// 是否存在key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public static bool Exists(string key, string hashKey = null)
        {
            return Base<bool>(key, r => r.ContainsKey(key), "Exists", hashKey);
        } 
        #endregion

        #region = GetOriginal =
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public static T GetOriginal<T>(string key, string hashKey)
        {
            if (string.IsNullOrEmpty(key) )
                throw new ArgumentNullException("key");

            T result = default(T);
            try
            {
                if (pool != null)
                {
                    hashKey = hashKey ?? key;
                    using (var r = pool.GetClient(hashKey.RedisIndex()))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            result = r.Get<T>(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", key);
                //Logger.Log(typeof(CacheHelper), msg, ex);

            }
            return result;
        } 
        #endregion

        #region = Remove =
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        public static void Remove(string key, string hashKey = null)
        {
            Base(key, r => r.Remove(key), "Remove", hashKey);
        } 
        #endregion

        //public static void Increment(string key, string hashKey = null)
        //{
        //    Base(key, r => r.IncrementValueBy(key, 1), "Increment", hashKey);
        //}

        //public static void Decrement(string key, string hashKey = null)
        //{
        //    Base(key, r => r.DecrementValueBy(key, 1), "Increment", hashKey);
        //}

        public static void RemoveItemFromList(string key, string value, string hashKey = null)
        {
            Base(key, r => r.RemoveItemFromList(key, value), "RemoveItemFromList", hashKey);
        }

        #region = Push =
        /// <summary>
        /// 压入项到列表中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <param name="hashKey"></param>
        public static void Push(string key, string value, DateTime expiry, string hashKey = null)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Action<IRedisClient> action = r =>
            {
                r.PushItemToList(key, value);

                if (expiry != DateTime.MaxValue)
                {
                    r.ExpireEntryIn(key, expiry - DateTime.Now);
                }
            };

            Base(key, action, "Push", hashKey);
        } 
        #endregion

        #region = GetList =
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public static List<string> GetList(string key, string hashKey = null)
        {
            return Base<List<string>>(key, r => r.GetAllItemsFromList(key), "GetList", hashKey);
        } 
        #endregion

        #region = PopItemFromList =
        /// <summary>
        /// 弹出列表项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public static string DequeueItemFromList(string key, string hashKey = null)
        {
            return Base<string>(key, r => r.DequeueItemFromList(key), "DequeueItemFromList", hashKey);
        } 
        #endregion

        #region - BytesToObject -
        private static object BytesToObject(string key, Byte[] bytes, string hashKey)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (bytes == null)
                throw new ArgumentNullException("bytes");



            object o = null;
            try
            {
                o = Compression.DecompressAndDeserialze(bytes);
            }
            catch (Exception ex)
            {
                Remove(key, hashKey);
                hashKey = hashKey ?? key;
                string msg = string.Format("{0}:{1}发生异常!{2} {3}", "cache", "解压", key, hashKey.RedisIndex());
                //Logger.Log(typeof(CacheHelper), msg, ex);
            }
            return o;
        } 
        #endregion

        #region = GetAll =
        /// <summary>
        /// 一次获取多个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public static IDictionary<string, T> GetAll<T>(IEnumerable<string> keys, string hashKey)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            keys = keys.Where(k => !string.IsNullOrEmpty(k));
            if (keys.Count() == 0)
                return null;

            if (keys.Count() == 1)
            {
                var cachekey = keys.Single();
                object o = Get(cachekey, hashKey);
                if (o is T)
                {
                    return new Dictionary<string, T>
                               {
                                   {cachekey, (T) o}
                               };
                }
                return null;
            }

            IDictionary<string, byte[]> dict = new Dictionary<string, byte[]>(); //缓存结果集

            if (pool != null)
            {
                #region - 获取缓存方法 -
                Func<IEnumerable<string>, int, IDictionary<string, byte[]>> func = (cachekeys, index) =>
                        {
                            try
                            {
                                using (var r = pool.GetClient(index))
                                {
                                    if (r != null)
                                    {
                                        r.SendTimeout = 1000;
                                        return r.GetAll<byte[]>(cachekeys);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", keys.Aggregate((a, b) => a + "," + b));
                                //Logger.Log(typeof(CacheHelper), msg, ex);
                            }
                            return null;

                        };
                #endregion

                if (string.IsNullOrEmpty(hashKey) == false)
                {
                    dict = func(keys, hashKey.RedisIndex());
                }
                else
                {
                    dict = keys.Select(s => new
                    {
                        Index = s.RedisIndex(),
                        KeyName = s
                    }).GroupBy(p => p.Index)
                    .SelectMany(g => func(g.Select(p => p.KeyName), g.Key))
                    .ToDictionary(x => x.Key, x => x.Value);
                }
            }

            IEnumerable<Tuple<string, object>> result = null;
            if (dict != null)
            {
                result = dict
                    .Where(x => x.Value != null)
                    .Select(d => new Tuple<string, object>(d.Key, BytesToObject(d.Key, d.Value, hashKey)));
            }
            else
            {
                result = keys.Select(key => new Tuple<string, object>(key, Get(key, hashKey)));
            }
            result = result.Where(d => d.Item2 != null && d.Item2 is T);
            return result.ToDictionary(x => x.Item1, x => (T)x.Item2);
        } 
        #endregion

        #region - RedisIndex -
        internal static int RedisIndex(this string hashkey)
        {
            if (string.IsNullOrEmpty(hashkey))
                throw new ArgumentNullException("hashkey");
            if (readWriteHosts == null)
                return 0;

            return hashkey.Sum(c => (int)c) % readWriteHosts.Length;
            //return Math.Abs(hashkey.GetHashCode()) % readWriteHosts.Length;
        } 
        #endregion

    }

    internal static class Compression
    {
        #region = SerializeAndCompress =
        /// <summary>
        /// 序列化并且压缩
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeAndCompress(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, obj);
                return stream.ToArray();
            }
        }
        #endregion

        #region = DecompressAndDeserialze =
        /// <summary>
        /// 解压并反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object DecompressAndDeserialze(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                return new BinaryFormatter().Deserialize(stream);
            }

        }
        #endregion
    }
}