using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RedisTestClient
{
    public partial class Form2 : Form
    {
        #region ########### CacheKey ####################

        private const string PrefixSkuBaseInfo = "Cache_SKU_BaseInfo_ForObs";
        private const string PrefixSkuProductLineInfo = "Cache_Serial_ProductLine_ForObs";
        private const string PrefixSkuStyleInfo = "Cache_SKU_StyleID_ForObs";
        private const string PrefixSkuSaleCategoryInfo = "Cache_SKU_Category_ForObs";

        #endregion

        public Form2()
        {
            InitializeComponent();
        }


        private void btnSkuStyle_Click(object sender, EventArgs e)
        {
            var style = txtKey.Text.Trim();
            var key = string.Format("{0}_{1}", PrefixSkuStyleInfo, style);
            txtKeyValue.Text = CacheHelper.Get(key).ToString();
        }

        private void btnProductLine_Click(object sender, EventArgs e)
        {
            var serial = txtKey.Text.Trim();
            var key = string.Format("{0}_{1}", PrefixSkuProductLineInfo, serial);
            txtKeyValue.Text = CacheHelper.Get(key).ToString();
        }

        private void btnSaleCategory_Click(object sender, EventArgs e)
        {
            var styleId = txtKey.Text.Trim();
            var key = string.Format("{0}_{1}", PrefixSkuSaleCategoryInfo, styleId);
            var saleCategory = CacheHelper.Get(key);
            //txtKeyValue.Text = CacheHelper.Get(key).ToString();
        }

        private void btnBaseInfo_Click(object sender, EventArgs e)
        {
            var serial = txtKey.Text.Trim();
            var key = string.Format("{0}_{1}", PrefixSkuBaseInfo, serial);
            txtKeyValue.Text = CacheHelper.Get(key).ToString();
        }
    }
}
