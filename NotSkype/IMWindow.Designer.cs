﻿namespace NotSkype
{
    partial class IMWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IMWindow));
            this.panelChatMsgs = new System.Windows.Forms.Panel();
            this.textBoxChatLog = new System.Windows.Forms.TextBox();
            this.textBoxChatMsg = new System.Windows.Forms.TextBox();
            this.buttonSendChat = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panelChatMsgs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelChatMsgs
            // 
            this.panelChatMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelChatMsgs.Controls.Add(this.textBoxChatLog);
            this.panelChatMsgs.Location = new System.Drawing.Point(0, 105);
            this.panelChatMsgs.Name = "panelChatMsgs";
            this.panelChatMsgs.Size = new System.Drawing.Size(484, 279);
            this.panelChatMsgs.TabIndex = 3;
            // 
            // textBoxChatLog
            // 
            this.textBoxChatLog.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxChatLog.Location = new System.Drawing.Point(3, 3);
            this.textBoxChatLog.Multiline = true;
            this.textBoxChatLog.Name = "textBoxChatLog";
            this.textBoxChatLog.ReadOnly = true;
            this.textBoxChatLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxChatLog.Size = new System.Drawing.Size(478, 273);
            this.textBoxChatLog.TabIndex = 0;
            // 
            // textBoxChatMsg
            // 
            this.textBoxChatMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChatMsg.Location = new System.Drawing.Point(12, 392);
            this.textBoxChatMsg.Name = "textBoxChatMsg";
            this.textBoxChatMsg.Size = new System.Drawing.Size(379, 20);
            this.textBoxChatMsg.TabIndex = 4;
            this.textBoxChatMsg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxChatMsg_KeyDown);
            // 
            // buttonSendChat
            // 
            this.buttonSendChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSendChat.Location = new System.Drawing.Point(397, 390);
            this.buttonSendChat.Name = "buttonSendChat";
            this.buttonSendChat.Size = new System.Drawing.Size(75, 23);
            this.buttonSendChat.TabIndex = 5;
            this.buttonSendChat.Text = "Send";
            this.buttonSendChat.UseVisualStyleBackColor = true;
            this.buttonSendChat.Click += new System.EventHandler(this.buttonSendChat_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 476);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(93, 476);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "Hide";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label6.Location = new System.Drawing.Point(81, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 19);
            this.label6.TabIndex = 17;
            this.label6.Text = "uname";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(81, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 23);
            this.label5.TabIndex = 16;
            this.label5.Text = "recepient\'s name";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::NotSkype.Properties.Resources._21627;
            this.pictureBox3.Location = new System.Drawing.Point(25, 36);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(50, 50);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 15;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::NotSkype.Properties.Resources.cloudui;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(195, 52);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 18;
            this.pictureBox2.TabStop = false;
            // 
            // IMWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.ClientSize = new System.Drawing.Size(484, 511);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonSendChat);
            this.Controls.Add(this.textBoxChatMsg);
            this.Controls.Add(this.panelChatMsgs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "IMWindow";
            this.Text = "NotSkype - ";
            this.Load += new System.EventHandler(this.IMWindow_Load);
            this.panelChatMsgs.ResumeLayout(false);
            this.panelChatMsgs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panelChatMsgs;
        private System.Windows.Forms.TextBox textBoxChatMsg;
        private System.Windows.Forms.Button buttonSendChat;
        private System.Windows.Forms.TextBox textBoxChatLog;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}