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

            GetItemPortionMenu.Enabled =
            GetCategoriesMenu.Enabled = state;
        }

        private void DisplayMessage(VttEventArgs arg)
        {
            Invoke((MethodInvoker)delegate
            {
                listBox1.Items.Add(arg.message);
            });
            if (arg.done)
            {
                StateMenuItems(true);
            }
        }

        private void GetItemPortion()
        {
            StateMenuItems(false);
            listBox1.Items.Clear();

            Stop_vtt();
            VTT = new servVTT(readSettings(), "GetItemPortion");
            VTT.Notify += DisplayMessage;
            VTT.GetItemPortion();
        }

        private void GetCategories()
        {
            StateMenuItems(false);
            listBox1.Items.Clear();

            Stop_vtt();
            VTT = new servVTT(readSettings(), "GetCategories");
            VTT.Notify += DisplayMessage;
            VTT.GetCategories();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StateMenuItems(true);
        }

        private void Stop_vtt()
        {
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
    }
}
