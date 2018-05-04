using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DTCash.RedisHelper;

namespace DTCash.Redis
{
    #region RedisKey管理

    public struct Dict
    {
        public bool IsGlobal { get; set; }
        public string Identity { get; set; }
        public object[] Param { get; set; }
    }

    public class CacheKey
    {
        public CacheKey() { }
        protected string key;
        protected string WebExt = ConfigurationManager.AppSettings["CacheType"] ?? string.Empty;
        protected virtual void Builder(string appendKey)
        {
            this.key = this.key + appendKey;
        }

        /// <summary>
        /// 获取生成Key
        /// </summary>
        /// <returns></returns>
        public virtual string GetKey()
        {
            if (string.IsNullOrEmpty(this.key))
            {
                throw new ArgumentException("生成Key对象传递错误");
            }

            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(this.dict.Identity))
            {
                builder.AppendFormat("{0}{1}", key, this.dict.Identity);
            }
            else
            {
                builder.AppendFormat("{0}", key);
            }

            if (this.dict.Param != null && this.dict.Param.Length > 0)
            {
                builder.Append("|");
                for (int i = 0; i < this.dict.Param.Length; i++)
                {
                    builder.Append(string.Format("{0}-", this.dict.Param[i]));
                }
                builder.Remove(builder.Length - 1, 1);
            }

            if (!this.dict.IsGlobal)
            {
                builder.AppendFormat("-{0}", WebExt);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 初始化缓存对象
        /// </summary>
        /// <param name="identity">唯一标识（ID）</param>
        /// <param name="param">自定义参数</param>
        /// <returns></returns>
        public static CacheKey Init(string identity, params object[] param)
        {
            CacheKey cache = new CacheKey();
            cache.dict.Identity = identity;
            cache.dict.Param = param;
            return cache;
        }
        /// <summary>
        /// 初始化缓存对象
        /// </summary>
        /// <param name="identity">唯一标识（ID）</param>
        /// <returns></returns>
        public static CacheKey Init(string identity)
        {
            CacheKey cache = new CacheKey();
            cache.dict.Identity = identity;
            return cache;
        }
        /// <summary>
        /// 初始化缓存对象
        /// </summary>
        /// <param name="param">自定义参数</param>
        /// <returns></returns>
        public static CacheKey Init(params object[] param)
        {
            CacheKey cache = new CacheKey();
            cache.dict.Param = param;
            return cache;
        }

        public static CacheKey Init()
        {
            return new CacheKey();
        }
        /// <summary>
        /// 不生成站点后缀
        /// </summary>
        /// <returns></returns>
        public CacheKey HideExt()
        {
            this.dict.IsGlobal = true;
            return this;
        }

        protected Dict dict;

        public User User
        {
            get
            {
                return new User(this.dict);
            }
        }

    }

    public class Item : CacheKey
    {
        public Item(string key, bool global)
        {
            this.key = key;
            this.dict.IsGlobal = global;
        }
        public CacheKey MyProperty
        {
            get
            {
                this.Builder("MyProperty");
                return this;
            }
        }
    }

    public class User : CacheKey
    {
        public User()
        {
            this.key = "111111User";
        }
        public User(Dict dict)
        {
            this.key = "111111User";
            this.dict = dict;
        }
        /// <summary>
        /// 金额
        /// </summary>
        public CacheKey Price
        {
            get
            {
                this.Builder("Price");
                return this;
            }
        }
        /// <summary>
        /// 通知
        /// </summary>
        public CacheKey Message
        {
            get
            {
                this.Builder("Message");
                return this;
            }
        }
        /// <summary>
        /// 认证
        /// </summary>
        public CacheKey Auth
        {
            get
            {
                this.Builder("Auth");
                return this;
            }
        }
        /// <summary>
        /// 礼品卡
        /// </summary>
        public CacheKey GiftCard
        {
            get
            {
                this.Builder("GiftCard");
                return this;
            }
        }
    }

    /// <summary>
    /// 产品相关
    /// </summary>
    public class Product : CacheKey
    {
        public Product()
        {
            this.key = "Product";
        }
        private Product(Dict dict)
        {
            this.key = "Product";
            this.dict = dict;
        }

        /// <summary>
        /// 产品分类
        /// </summary>
        public CacheKey Lcategory
        {
            get
            {
                this.Builder("Lcategory");
                return this;
            }
        }

        /// <summary>
        /// 子订单
        /// </summary>
        public CacheKey OrderItem
        {
            get
            {
                this.Builder("OrderItems");
                return this;
            }

        }
        /// <summary>
        /// 还款
        /// </summary>
        public CacheKey Repayment
        {
            get
            {
                this.Builder("Repayment");
                return this;
            }
        }
        /// <summary>
        /// 列表
        /// </summary>
        public CacheKey List
        {
            get
            {
                this.Builder("List");
                return this;
            }
        }
    }

    /// <summary>
    /// 新闻相关
    /// </summary>
    public class News : CacheKey
    {
        public News()
        {
            this.key = "News";
        }

        public News(Dict dict)
        {
            this.key = "News";
            this.dict = dict;
        }
        /// <summary>
        /// 列表
        /// </summary>
        public CacheKey List
        {
            get
            {
                this.Builder("List");
                return this;
            }
        }
    }

    /// <summary>
    /// 论坛
    /// </summary>
    public class BBS : CacheKey
    {
        public BBS()
        {
            this.key = "BBS";
        }
        public BBS(Dict dict)
        {
            this.key = "BBS";
            this.dict = dict;
        }
        /// <summary>
        /// 列表
        /// </summary>
        public CacheKey List
        {
            get
            {
                this.Builder("List");
                return this;
            }
        }
    }

    #endregion
}
