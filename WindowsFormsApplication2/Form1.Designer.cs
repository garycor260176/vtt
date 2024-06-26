﻿namespace WindowsFormsApplication2
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
            this.Category = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.GetItemPortionMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.GetCategoriesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.данныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CategoryMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RecalcAll = new System.Windows.Forms.ToolStripMenuItem();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.getItemsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.Category.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Category
            // 
            this.Category.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.stopMenu,
            this.settingsMenu,
            this.данныеToolStripMenuItem});
            this.Category.Location = new System.Drawing.Point(0, 0);
            this.Category.Name = "Category";
            this.Category.Size = new System.Drawing.Size(951, 24);
            this.Category.TabIndex = 4;
            this.Category.Text = "Категории";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GetItemPortionMenu,
            this.GetCategoriesMenu,
            this.getItemsMenu});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(43, 20);
            this.toolStripMenuItem1.Text = "Start";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // GetItemPortionMenu
            // 
            this.GetItemPortionMenu.Name = "GetItemPortionMenu";
            this.GetItemPortionMenu.Size = new System.Drawing.Size(155, 22);
            this.GetItemPortionMenu.Text = "GetItemPortion";
            this.GetItemPortionMenu.Click += new System.EventHandler(this.getItemPortionToolStripMenuItem_Click);
            // 
            // GetCategoriesMenu
            // 
            this.GetCategoriesMenu.Name = "GetCategoriesMenu";
            this.GetCategoriesMenu.Size = new System.Drawing.Size(155, 22);
            this.GetCategoriesMenu.Text = "GetCategories";
            this.GetCategoriesMenu.Click += new System.EventHandler(this.getCategoriesToolStripMenuItem_Click);
            // 
            // stopMenu
            // 
            this.stopMenu.Name = "stopMenu";
            this.stopMenu.Size = new System.Drawing.Size(43, 20);
            this.stopMenu.Text = "Stop";
            this.stopMenu.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // settingsMenu
            // 
            this.settingsMenu.Name = "settingsMenu";
            this.settingsMenu.Size = new System.Drawing.Size(61, 20);
            this.settingsMenu.Text = "Settings";
            this.settingsMenu.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // данныеToolStripMenuItem
            // 
            this.данныеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CategoryMenu,
            this.RecalcAll});
            this.данныеToolStripMenuItem.Name = "данныеToolStripMenuItem";
            this.данныеToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.данныеToolStripMenuItem.Text = "Данные";
            // 
            // CategoryMenu
            // 
            this.CategoryMenu.Name = "CategoryMenu";
            this.CategoryMenu.Size = new System.Drawing.Size(176, 22);
            this.CategoryMenu.Text = "Коэффициенты...";
            this.CategoryMenu.Click += new System.EventHandler(this.CategoryMenu_Click);
            // 
            // RecalcAll
            // 
            this.RecalcAll.Name = "RecalcAll";
            this.RecalcAll.Size = new System.Drawing.Size(176, 22);
            this.RecalcAll.Text = "Пересчитать цены";
            this.RecalcAll.Click += new System.EventHandler(this.RecalcAll_Click);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 24);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(951, 596);
            this.listBox1.TabIndex = 5;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgressBar,
            this.StatusText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 598);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(951, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusText
            // 
            this.StatusText.Name = "StatusText";
            this.StatusText.Size = new System.Drawing.Size(118, 17);
            this.StatusText.Text = "toolStripStatusLabel1";
            // 
            // getItemsMenu
            // 
            this.getItemsMenu.Name = "getItemsMenu";
            this.getItemsMenu.Size = new System.Drawing.Size(155, 22);
            this.getItemsMenu.Text = "GetItems";
            this.getItemsMenu.Click += new System.EventHandler(this.getItemsToolStripMenuItem_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 620);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.Category);
            this.MainMenuStrip = this.Category;
            this.Name = "Form1";
            this.Text = "vtt";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Category.ResumeLayout(false);
            this.Category.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip Category;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem stopMenu;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ToolStripMenuItem settingsMenu;
        private System.Windows.Forms.ToolStripMenuItem GetItemPortionMenu;
        private System.Windows.Forms.ToolStripMenuItem GetCategoriesMenu;
        private System.Windows.Forms.ToolStripMenuItem данныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CategoryMenu;
        private System.Windows.Forms.ToolStripMenuItem RecalcAll;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusText;
        private System.Windows.Forms.ToolStripMenuItem getItemsMenu;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
    }
}

