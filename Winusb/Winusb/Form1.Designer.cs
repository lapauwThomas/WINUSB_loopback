namespace Winusb
{
    partial class Form1
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
            this.btnSend = new System.Windows.Forms.Button();
            this.tb_sendData = new System.Windows.Forms.TextBox();
            this.richTextBoxReceived = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(481, 12);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_sendData
            // 
            this.tb_sendData.Location = new System.Drawing.Point(13, 13);
            this.tb_sendData.Name = "tb_sendData";
            this.tb_sendData.Size = new System.Drawing.Size(462, 20);
            this.tb_sendData.TabIndex = 1;
            // 
            // richTextBoxReceived
            // 
            this.richTextBoxReceived.Location = new System.Drawing.Point(13, 50);
            this.richTextBoxReceived.Name = "richTextBoxReceived";
            this.richTextBoxReceived.ReadOnly = true;
            this.richTextBoxReceived.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBoxReceived.Size = new System.Drawing.Size(462, 142);
            this.richTextBoxReceived.TabIndex = 2;
            this.richTextBoxReceived.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 235);
            this.Controls.Add(this.richTextBoxReceived);
            this.Controls.Add(this.tb_sendData);
            this.Controls.Add(this.btnSend);
            this.Name = "Form1";
            this.Text = "LoopBack example";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tb_sendData;
        private System.Windows.Forms.RichTextBox richTextBoxReceived;
    }
}

