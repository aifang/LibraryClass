namespace winFormThread
{
    partial class LongTimeForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLongTime = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLongTime
            // 
            this.btnLongTime.Location = new System.Drawing.Point(248, 208);
            this.btnLongTime.Name = "btnLongTime";
            this.btnLongTime.Size = new System.Drawing.Size(213, 51);
            this.btnLongTime.TabIndex = 0;
            this.btnLongTime.Text = "耗时操作";
            this.btnLongTime.UseVisualStyleBackColor = true;
            this.btnLongTime.Click += new System.EventHandler(this.btnLongTime_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(259, 150);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(41, 12);
            this.lblResult.TabIndex = 1;
            this.lblResult.Text = "label1";
            // 
            // LongTimeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 389);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnLongTime);
            this.Name = "LongTimeForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLongTime;
        private System.Windows.Forms.Label lblResult;
    }
}

