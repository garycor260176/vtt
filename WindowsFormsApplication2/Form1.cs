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

        private void DisplayMessage(VttEventArgs arg)
        {
            Invoke((MethodInvoker)delegate
            {
                listBox1.Items.Add(arg.command + (arg.type.Length == 0 ? "" : " - " + arg.type) + ": " + arg.message);
            });
            if (arg.message == "Завершено!" || arg.message == "Прервано.")
            {
                toolStripMenuItem1.Enabled = true;
                stopToolStripMenuItem.Enabled = false;
            }
        }

        private void start_download()
        {
            toolStripMenuItem1.Enabled = false;
            stopToolStripMenuItem.Enabled = true;
            listBox1.Items.Clear();

            Stop_vtt();
            VTT = new servVTT(readSettings());
            VTT.Notify += DisplayMessage;
            VTT.GetItemPortion();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripMenuItem1.Enabled = true;
            stopToolStripMenuItem.Enabled = false;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            start_download();
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
    }
}
