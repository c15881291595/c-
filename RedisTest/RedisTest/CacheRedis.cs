using DTCash.RedisHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Cache;
using System.Security.AccessControl;
using System.Text;

namespace DTCash.Redis
{
    public class CacheRedis
    {
        #region 缓存模式
        /// <summary>
        /// 只读
        /// </summary>
        internal static bool IsOnlyRead = ConfigurationManager.AppSettings["CacheMode"] != null && ConfigurationManager.AppSettings["CacheMode"].ToLower() == "onlyread";
        /// <summary>
        /// 只写
        /// </summary>
        internal static bool IsOnlyWrite = ConfigurationManager.AppSettings["CacheMode"] != null && ConfigurationManager.AppSettings["CacheMode"].ToLower() == "onlywrite";
        /// <summary>
        /// 是否启用
        /// </summary>
        internal static bool IsEnableCache = ConfigurationManager.AppSettings["CacheEnable"] != null &&
                                          ConfigurationManager.AppSettings["CacheEnable"].ToLower() == "enablecache";
        internal static int CacheTime = ConfigurationManager.AppSettings["CacheTime"] != null ? int.Parse(ConfigurationManager.AppSettings["CacheTime"]) : 120;

        internal static string RedisCacheNameExt = ConfigurationManager.AppSettings["CacheType"] ?? string.Empty;
        /// <summary>
        /// 是否启用异常拦截
        /// </summary>
        internal static bool CacheCatchEnable = ConfigurationManager.AppSettings["CacheCatchEnable"] != null ? bool.Parse(ConfigurationManager.AppSettings["CacheCatchEnable"]) : false;

        #endregion

        #region 添加
        /// <summary>
        /// 添加Redis缓存
        /// </summary>
        /// <param name="Content">需要缓存的内容</param>
        /// <param name="cacheKey">缓存Key对象</param>
        /// <returns></returns>
        public static bool AddCache(object Content, CacheKey cacheKey)
        {
            try
            {
                return AddCache(new TimeSpan(CacheTime / 60, CacheTime % 60, 0), cacheKey, Content);
            }
            catch (Exception ex)
            {
                if (CacheCatchEnable)
                {
                    return false;
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 添加Redis缓存
        /// </summary>
        /// <param name="timeSpan">缓存时间</param>
        /// <param name="cacheKey">缓存Key对象</param>
        /// <param name="Content">需要缓存的数据</param>
        /// <returns></returns>
        public static bool AddCache(TimeSpan timeSpan, CacheKey cacheKey, object Content)
        {
            try
            {
                if (!IsOnlyRead && IsEnableCache)
                {
                    CacheRedisCommon.AddRedisByRedisCacheDTO(cacheKey.GetKey(), Content, timeSpan);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 检测是否存在
        /// <summary>
        /// 判断Redis缓存是否存在
        /// </summary>
        /// <param name="cacheKey">缓存Key对象</param>
        /// <returns></returns>
        public static bool ExistsRedis(CacheKey cacheKey)
        {
            try
            {
                if (!IsOnlyWrite && IsEnableCache)
                {
                    return CacheRedisCommon.ExistsRedis(cacheKey.GetKey());
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (CacheCatchEnable)
                {
                    return false;
                }
                else
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region 删除

        /// <summary>
        /// 删除redis缓存
        /// </summary>
        /// <param name="cacheKey">缓存Key对象</param>
        public static void DelCacheKey(CacheKey cacheKey)
        {
            try
            {
                if (IsEnableCache)
                {
                    CacheRedisCommon.DelRedisByRedisCacheDTO(cacheKey.GetKey());
                }
            }
            catch (Exception ex)
            {
                if (!CacheCatchEnable)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 删除redis缓存（模糊删除）
        /// </summary>
        /// <param name="cacheKey">缓存Key对象</param>
        public static void DelCacheKeys(CacheKey cacheKey)
        {
            try
            {
                if (IsEnableCache)
                {
                    CacheRedisCommon.DelRedisByRedisCacheDTO(cacheKey.GetKey());
                }
            }
            catch (Exception ex)
            {
                if (!CacheCatchEnable)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region 获取

        /// <summary>
        /// 获取Redis缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static object GetCache(CacheKey cacheKey)
        {
            try
            {
                if (!IsOnlyWrite && IsEnableCache)
                {
                    return CacheRedisCommon.GetRedisByRedisCacheDTO(cacheKey.GetKey());
                }
                return null;
            }
            catch (Exception ex)
            {
                if (!CacheCatchEnable)
                {
                    throw ex;
                }
                return null;
            }
        }

        #endregion

        #region 删除所有Redis

        public static void DelAllCache()
        {
            if (IsEnableCache)
            {
                CacheHelper.RemoveAll();
            }
        }

        #endregion

        #region 获取所有key

        public static List<string> GetAllKey()
        {
            if (IsEnableCache)
            {
                return CacheHelper.GetAllKey();
            }
            else
            {
                return null;
            }
        }

        #endregion

        public static void DelRedisByKey(string LinkKey)
        {
            if (IsEnableCache)
            {
                CacheHelper.Remove(LinkKey);
            }
        }
    }

    public class CacheRedisCommon
    {
        internal static void DelRedisByRedisCacheDTO(string key)
        {
            CacheHelper.Remove(key);
        }

        internal static void AddRedisByRedisCacheDTO(string key, object Value, TimeSpan timeSpan)
        {
            CacheHelper.Add(key, Value, timeSpan, null);
        }

        internal static object GetRedisByRedisCacheDTO(string key)
        {
            return CacheHelper.Get(key, null);
        }

        internal static bool ExistsRedis(string key)
        {
            return CacheHelper.Exists(key, null);
        }
    }
}