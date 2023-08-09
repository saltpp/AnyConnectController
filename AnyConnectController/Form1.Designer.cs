namespace AnyConnectController
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.linkLabelGitHub = new System.Windows.Forms.LinkLabel();
            this.checkBoxOnTop = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(172, 9);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 19);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(253, 9);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 19);
            this.buttonDisconnect.TabIndex = 1;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(12, 9);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(154, 19);
            this.textBoxPassword.TabIndex = 2;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // linkLabelGitHub
            // 
            this.linkLabelGitHub.AutoSize = true;
            this.linkLabelGitHub.Location = new System.Drawing.Point(212, 34);
            this.linkLabelGitHub.Name = "linkLabelGitHub";
            this.linkLabelGitHub.Size = new System.Drawing.Size(116, 12);
            this.linkLabelGitHub.TabIndex = 4;
            this.linkLabelGitHub.TabStop = true;
            this.linkLabelGitHub.Text = "GitHub (README.md)";
            this.linkLabelGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelGitHub_LinkClicked);
            // 
            // checkBoxOnTop
            // 
            this.checkBoxOnTop.AutoSize = true;
            this.checkBoxOnTop.Location = new System.Drawing.Point(12, 33);
            this.checkBoxOnTop.Name = "checkBoxOnTop";
            this.checkBoxOnTop.Size = new System.Drawing.Size(100, 16);
            this.checkBoxOnTop.TabIndex = 5;
            this.checkBoxOnTop.Text = "Always on Top";
            this.checkBoxOnTop.UseVisualStyleBackColor = true;
            this.checkBoxOnTop.CheckedChanged += new System.EventHandler(this.CheckBoxOnTop_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(128)))), ((int)(((byte)(173)))));
            this.ClientSize = new System.Drawing.Size(339, 50);
            this.Controls.Add(this.checkBoxOnTop);
            this.Controls.Add(this.linkLabelGitHub);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonConnect);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "AnyConnectController";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.LinkLabel linkLabelGitHub;
        private System.Windows.Forms.CheckBox checkBoxOnTop;
    }
}

