namespace RedisTestClient
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtKeyValue = new System.Windows.Forms.TextBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.btnSkuStyle = new System.Windows.Forms.Button();
            this.btnProductLine = new System.Windows.Forms.Button();
            this.btnSaleCategory = new System.Windows.Forms.Button();
            this.btnBaseInfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(16, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "Redis键值：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(10, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "Redis键名：";
            // 
            // txtKeyValue
            // 
            this.txtKeyValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtKeyValue.Location = new System.Drawing.Point(12, 128);
            this.txtKeyValue.Multiline = true;
            this.txtKeyValue.Name = "txtKeyValue";
            this.txtKeyValue.Size = new System.Drawing.Size(468, 201);
            this.txtKeyValue.TabIndex = 13;
            // 
            // txtKey
            // 
            this.txtKey.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtKey.Location = new System.Drawing.Point(12, 29);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(468, 23);
            this.txtKey.TabIndex = 12;
            // 
            // btnSkuStyle
            // 
            this.btnSkuStyle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSkuStyle.Location = new System.Drawing.Point(19, 58);
            this.btnSkuStyle.Name = "btnSkuStyle";
            this.btnSkuStyle.Size = new System.Drawing.Size(103, 23);
            this.btnSkuStyle.TabIndex = 11;
            this.btnSkuStyle.Text = "读取SKU前台款";
            this.btnSkuStyle.UseVisualStyleBackColor = true;
            this.btnSkuStyle.Click += new System.EventHandler(this.btnSkuStyle_Click);
            // 
            // btnProductLine
            // 
            this.btnProductLine.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnProductLine.Location = new System.Drawing.Point(134, 58);
            this.btnProductLine.Name = "btnProductLine";
            this.btnProductLine.Size = new System.Drawing.Size(106, 23);
            this.btnProductLine.TabIndex = 11;
            this.btnProductLine.Text = "读取款式产品线";
            this.btnProductLine.UseVisualStyleBackColor = true;
            this.btnProductLine.Click += new System.EventHandler(this.btnProductLine_Click);
            // 
            // btnSaleCategory
            // 
            this.btnSaleCategory.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaleCategory.Location = new System.Drawing.Point(246, 58);
            this.btnSaleCategory.Name = "btnSaleCategory";
            this.btnSaleCategory.Size = new System.Drawing.Size(101, 23);
            this.btnSaleCategory.TabIndex = 11;
            this.btnSaleCategory.Text = "读取前台款分类";
            this.btnSaleCategory.UseVisualStyleBackColor = true;
            this.btnSaleCategory.Click += new System.EventHandler(this.btnSaleCategory_Click);
            // 
            // btnBaseInfo
            // 
            this.btnBaseInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBaseInfo.Location = new System.Drawing.Point(353, 58);
            this.btnBaseInfo.Name = "btnBaseInfo";
            this.btnBaseInfo.Size = new System.Drawing.Size(112, 23);
            this.btnBaseInfo.TabIndex = 11;
            this.btnBaseInfo.Text = "读取SKU基本信息";
            this.btnBaseInfo.UseVisualStyleBackColor = true;
            this.btnBaseInfo.Click += new System.EventHandler(this.btnBaseInfo_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 330);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKeyValue);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.btnBaseInfo);
            this.Controls.Add(this.btnSaleCategory);
            this.Controls.Add(this.btnProductLine);
            this.Controls.Add(this.btnSkuStyle);
            this.Name = "Form2";
            this.Text = "Redis缓存查看";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtKeyValue;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Button btnSkuStyle;
        private System.Windows.Forms.Button btnProductLine;
        private System.Windows.Forms.Button btnSaleCategory;
        private System.Windows.Forms.Button btnBaseInfo;
    }
}