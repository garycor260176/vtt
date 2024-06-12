using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.ServiceModel;
using MSScriptControl;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        servVTT VTT;

        public Form1()
        {
            InitializeComponent();
        }

        private config readSettings() //чтение настройки из какого-нить файла (xml, ini или что-то еще)
        {
            return config.Read();
        }

        private void StateMenuItems(Boolean state){
            stopMenu.Enabled = !state;

            settingsMenu.Enabled = 
            CategoryMenu.Enabled = 
            getItemsMenu.Enabled = 
            GetItemPortionMenu.Enabled =
            RecalcAll.Enabled = 
            GetCategoriesMenu.Enabled = state;
        }

        private void DisplayMessage(VttEventArgs arg)
        {
            Invoke((MethodInvoker)delegate
            {
                switch (arg.step)
                {
                    case TypeStep.start:
                        listBox1.Items.Add(arg.message);
                        ProgressBar.Value = 0;
                        StatusText.Text = arg.message;
                        break;
                    case TypeStep.end:
                        listBox1.Items.Add(arg.message);
                        ProgressBar.Value = 0;
                        StatusText.Text = arg.message;
                        StateMenuItems(true);
                        break;
                    case TypeStep.saveStart:
                        listBox1.Items.Add(arg.message);
                        ProgressBar.Value = 0;
                        ProgressBar.Maximum = arg.maxProgressValue;
                        StatusText.Text = arg.message;
                        break;
                    case TypeStep.saveEnd:
                        listBox1.Items.Add(arg.message);
                        ProgressBar.Value = 0;
                        StatusText.Text = arg.message;
                        break;
                    case TypeStep.progress:
                        ProgressBar.Value = arg.progressValue;
                        StatusText.Text = arg.message;
                        break;
                    case TypeStep.other:
                        listBox1.Items.Add(arg.message);
                        StatusText.Text = arg.message;
                        break;
                }
            });
        }

        private void GetItemPortion()
        {
            ProgressBar.Value = 0;
            StateMenuItems(false);
            listBox1.Items.Clear();

            Stop_vtt();
            VTT = new servVTT(readSettings(), "GetItemPortion");
            VTT.Notify += DisplayMessage;
            VTT.GetItemPortion();
        }

        private void GetItems()
        {
            ProgressBar.Value = 0;
            StateMenuItems(false);
            listBox1.Items.Clear();
            ProgressBar.Value = 0;

            Stop_vtt();
            VTT = new servVTT(readSettings(), "GetItems");
            VTT.Notify += DisplayMessage;
            VTT.GetItems();
        }

        private void GetCategories()
        {
            ProgressBar.Value = 0;
            StateMenuItems(false);
            listBox1.Items.Clear();

            Stop_vtt();
            VTT = new servVTT(readSettings(), "GetCategories");
            VTT.Notify += DisplayMessage;
            VTT.GetCategories();
        }

        private void DB_Message(String message, db.TypeError type)
        {
            StatusText.Text = "Error: " + message;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StateMenuItems(true);
            StatusText.Text = "";
        }

        private void Stop_vtt()
        {
            ProgressBar.Value = 0;
            if (VTT != null) VTT.Cancel();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop_vtt();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Stop_vtt();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_conf conf_frm = new frm_conf();
            conf_frm.ShowDialog();            
        }

        private void getItemPortionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetItemPortion();
        }

        private void getCategoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetCategories();
        }

        private void CategoryMenu_Click(object sender, EventArgs e)
        {
            frm_category frm = new frm_category();
            frm.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void RecalcAll_Click(object sender, EventArgs e)
        {
            StatusText.Text = "Расчет цен... ждите...";
            db mysql = new db();
            mysql.Notify += DB_Message;
            if (!mysql.RecalcAllPrice()) return;
            StatusText.Text = "Расчет цен завершен!";
        }

        private void getItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetItems();
        }
    }
}
