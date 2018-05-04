using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using Vancl.Security;

namespace RedisTestClientConsole
{
	public class DbConn
	{
        private volatile static string productDbConnString;
        private volatile static string productDbReadOnlyConnString;
        private volatile static string otherDbReadOnlyConnString;
	    private static volatile string prodRead;
        private volatile static string _productReducePriceReportDbConnString;
	    private SqlConnection conn_ = null; 
		/// <summary>
		/// 产品数据库连接串
		/// </summary>
		internal static string ProductDbConnString
		{
			get
			{
                if (string.IsNullOrEmpty(productDbConnString))
                {
                    productDbConnString = ConfigurationManager.ConnectionStrings["ProductDbConnString"].ConnectionString;
                    try
                    {//如果解密报错，则返回原串，忽略所有异常
                        productDbConnString = DES.Decrypt3DES(productDbConnString, Encoding.UTF8);
                    }
                    catch{}
                }
			    return productDbConnString;
			}
		}
        /// <summary>
        /// 产品数据库只读连接串
        /// </summary>
        public static string ProductDbReadOnlyConnString
        {
            get
            {
                if (string.IsNullOrEmpty(productDbReadOnlyConnString))
                {
                    productDbReadOnlyConnString =
                        ConfigurationManager.ConnectionStrings["ProductDbReadOnlyConnString"].ConnectionString;
                    try
                    {//如果解密报错，则返回原串，忽略所有异常
                        productDbReadOnlyConnString = DES.Decrypt3DES(productDbReadOnlyConnString, Encoding.UTF8);
                    }
                    catch { }
                }
                return productDbReadOnlyConnString;
            }
        }
        /// <summary>
        /// 其他数据库只读连接串
        /// </summary>
        internal static string OtherDbReadOnlyConnString
        {
            get
            {
                if (string.IsNullOrEmpty(otherDbReadOnlyConnString))
                {
                    otherDbReadOnlyConnString =
                        ConfigurationManager.ConnectionStrings["OtherDbReadOnlyConnString"].ConnectionString;
                    try
                    {//如果解密报错，则返回原串，忽略所有异常
                        otherDbReadOnlyConnString = DES.Decrypt3DES(otherDbReadOnlyConnString, Encoding.UTF8);
                    }
                    catch { }
                }
                return otherDbReadOnlyConnString;
            }
        }
        /// <summary>
        /// 其他数据库只读连接串
        /// </summary>
        internal static string ProdRead
        {
            get
            {
                if (string.IsNullOrEmpty(prodRead))
                {
                    prodRead =
                        ConfigurationManager.ConnectionStrings["ProdRead"].ConnectionString;
                    try
                    {//如果解密报错，则返回原串，忽略所有异常
                        prodRead = DES.Decrypt3DES(prodRead, Encoding.UTF8);
                    }
                    catch { }
                }
                return prodRead;
            }
        }
        /// <summary>
        /// 写入报表数据库连接串
        /// </summary>
        internal static string ProductReducePriceReportDbConnString
        {
            get
            {
                if (string.IsNullOrEmpty(_productReducePriceReportDbConnString))
                {
                    _productReducePriceReportDbConnString = ConfigurationManager.ConnectionStrings["ProductReducePriceReportDbConnString"].ConnectionString;
                    try
                    {   //如果解密报错，则返回原串，忽略所有异常
                        _productReducePriceReportDbConnString = DES.Decrypt3DES(_productReducePriceReportDbConnString, Encoding.UTF8);
                    }
                    catch { }
                }
                return _productReducePriceReportDbConnString;
            }
        }
        /// <summary>
        /// 数据库连接对象
        /// </summary>
	    public SqlConnection Conn
	    {
	        get
	        {
	            if(conn_==null)
	            {
                    conn_ = new SqlConnection(ProductDbConnString);   
	            }
	            return conn_;
	        }
	    }
        /// <summary>
        /// 数据库连接关闭
        /// </summary>
        public void SqlConnClose()
        {
            if(Conn==null)
                return;
            if(Conn.State==ConnectionState.Open)
                Conn.Close();
        }
        /// <summary>
        /// 数据库连接关闭
        /// </summary>
        public void SqlConnOpen()
        {
            if (Conn == null)
                return;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
        }
        /// <summary>
        /// 数据库连接销毁
        /// </summary>
        public void SqlConnDisposed()
        {
            if (Conn == null)
                return;
            else
            {
                Conn.Close();
                Conn.Dispose();
            }
        }
	    public static string GetProductEmployeeInDept
	    {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["ProductEmployeeInDept"].ToString(); 
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
	    }
        
        ///<summary>
        ///存储过程名或SQL文本
        ///</summary>
        protected string queryText;
	}
}
