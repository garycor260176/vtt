﻿namespace WindowsFormsApplication2
{
    partial class frm_conf
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vtt_login = new System.Windows.Forms.TextBox();
            this.vtt_password = new System.Windows.Forms.TextBox();
            this.vtt_size = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.vtt_address = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mysql_dbname = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.mysql_port = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.mysql_login = new System.Windows.Forms.TextBox();
            this.mysql_password = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.mysql_address = new System.Windows.Forms.TextBox();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.OnlyError = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LogToFile = new System.Windows.Forms.CheckBox();
            this.shipping_price = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vtt_size)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mysql_port)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.vtt_login);
            this.groupBox1.Controls.Add(this.vtt_password);
            this.groupBox1.Controls.Add(this.vtt_size);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.vtt_address);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 109);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "vtt";
            // 
            // vtt_login
            // 
            this.vtt_login.Location = new System.Drawing.Point(82, 79);
            this.vtt_login.Name = "vtt_login";
            this.vtt_login.Size = new System.Drawing.Size(100, 20);
            this.vtt_login.TabIndex = 2;
            // 
            // vtt_password
            // 
            this.vtt_password.Location = new System.Drawing.Point(258, 79);
            this.vtt_password.Name = "vtt_password";
            this.vtt_password.PasswordChar = '*';
            this.vtt_password.Size = new System.Drawing.Size(100, 20);
            this.vtt_password.TabIndex = 3;
            // 
            // vtt_size
            // 
            this.vtt_size.Location = new System.Drawing.Point(82, 49);
            this.vtt_size.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.vtt_size.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.vtt_size.Name = "vtt_size";
            this.vtt_size.Size = new System.Drawing.Size(58, 20);
            this.vtt_size.TabIndex = 1;
            this.vtt_size.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(197, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "login:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "size:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "address:";
            // 
            // vtt_address
            // 
            this.vtt_address.Location = new System.Drawing.Point(82, 17);
            this.vtt_address.Name = "vtt_address";
            this.vtt_address.Size = new System.Drawing.Size(276, 20);
            this.vtt_address.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mysql_dbname);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.mysql_port);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.mysql_login);
            this.groupBox2.Controls.Add(this.mysql_password);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.mysql_address);
            this.groupBox2.Location = new System.Drawing.Point(12, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(368, 104);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "mysql";
            // 
            // mysql_dbname
            // 
            this.mysql_dbname.Location = new System.Drawing.Point(82, 48);
            this.mysql_dbname.Name = "mysql_dbname";
            this.mysql_dbname.Size = new System.Drawing.Size(100, 20);
            this.mysql_dbname.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "DB Name:";
            // 
            // mysql_port
            // 
            this.mysql_port.Location = new System.Drawing.Point(300, 22);
            this.mysql_port.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.mysql_port.Name = "mysql_port";
            this.mysql_port.Size = new System.Drawing.Size(58, 20);
            this.mysql_port.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(268, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(28, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "port:";
            // 
            // mysql_login
            // 
            this.mysql_login.Location = new System.Drawing.Point(82, 76);
            this.mysql_login.Name = "mysql_login";
            this.mysql_login.Size = new System.Drawing.Size(100, 20);
            this.mysql_login.TabIndex = 7;
            // 
            // mysql_password
            // 
            this.mysql_password.Location = new System.Drawing.Point(258, 76);
            this.mysql_password.Name = "mysql_password";
            this.mysql_password.PasswordChar = '*';
            this.mysql_password.Size = new System.Drawing.Size(100, 20);
            this.mysql_password.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(197, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "password:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "login:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "address:";
            // 
            // mysql_address
            // 
            this.mysql_address.Location = new System.Drawing.Point(82, 21);
            this.mysql_address.Name = "mysql_address";
            this.mysql_address.Size = new System.Drawing.Size(170, 20);
            this.mysql_address.TabIndex = 4;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(396, 12);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 9;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(396, 66);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 10;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // OnlyError
            // 
            this.OnlyError.AutoSize = true;
            this.OnlyError.Location = new System.Drawing.Point(14, 281);
            this.OnlyError.Name = "OnlyError";
            this.OnlyError.Size = new System.Drawing.Size(155, 17);
            this.OnlyError.TabIndex = 11;
            this.OnlyError.Text = "Выводить только ошибки";
            this.OnlyError.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Location = new System.Drawing.Point(386, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(4, 264);
            this.panel1.TabIndex = 12;
            // 
            // LogToFile
            // 
            this.LogToFile.AutoSize = true;
            this.LogToFile.Location = new System.Drawing.Point(190, 281);
            this.LogToFile.Name = "LogToFile";
            this.LogToFile.Size = new System.Drawing.Size(136, 17);
            this.LogToFile.TabIndex = 13;
            this.LogToFile.Text = "Логгирование в файл";
            this.LogToFile.UseVisualStyleBackColor = true;
            // 
            // shipping_price
            // 
            this.shipping_price.Location = new System.Drawing.Point(127, 249);
            this.shipping_price.Name = "shipping_price";
            this.shipping_price.Size = new System.Drawing.Size(100, 20);
            this.shipping_price.TabIndex = 15;
            this.shipping_price.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.shipping_price_KeyPress);
            this.shipping_price.Validating += new System.ComponentModel.CancelEventHandler(this.shipping_price_Validating);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 253);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(112, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Стоимость доставки";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frm_conf
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(479, 309);
            this.Controls.Add(this.shipping_price);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.LogToFile);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.OnlyError);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_conf";
            this.Text = "Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vtt_size)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mysql_port)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox vtt_login;
        private System.Windows.Forms.TextBox vtt_password;
        private System.Windows.Forms.NumericUpDown vtt_size;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox vtt_address;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox mysql_dbname;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown mysql_port;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox mysql_login;
        private System.Windows.Forms.TextBox mysql_password;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox mysql_address;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.CheckBox OnlyError;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox LogToFile;
        private System.Windows.Forms.TextBox shipping_price;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}