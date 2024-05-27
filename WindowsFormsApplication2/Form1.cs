﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.ServiceModel;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        CancellationTokenSource cts;

        public Form1()
        {
            InitializeComponent();
        }

        private config readSettings() //чтение настройки из какого-нить файла (xml, ini или что-то еще)
        {
            return config.Read();
        }

        private void download()
        {
            config ini = readSettings();
            String connStr =
                "server=" + ini.db.server + ";" +
                "user=" + ini.db.login + ";" +
                "database=" + ini.db.dbname + ";" +
                "port=" + ini.db.port.ToString() + ";" +
                "password=" + ini.db.pwd + ";"
                ;

            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            int from = 0;

            vtt.PortalServiceClient client;
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding() { MaxReceivedMessageSize = 131072 };
                client = new vtt.PortalServiceClient(binding, new System.ServiceModel.EndpointAddress(ini.vtt.address));
            }
            catch (Exception e0)
            {
                Invoke((MethodInvoker)delegate
                {
                    listBox1.Items.Add("Error: " + e0.Message);
                });

                return;
            }
            
            while (true)
            {
                if (cts.Token.IsCancellationRequested) break;
                int to = from + ini.vtt.size - 1;
                try
                {
                    vtt.ItemPortionDto items = client.GetItemPortion(ini.vtt.login, ini.vtt.pwd, from, to);
                    SaveTodatabase(conn, items);
                    if (items.Items.Length < ini.vtt.size)
                        break;
                }
                catch (System.ServiceModel.FaultException e1)
                {
                    Invoke((MethodInvoker)delegate {
                        listBox1.Items.Add(from.ToString() + " - " + to.ToString() + ": " + e1.Message);
                    });
                }
                catch (Exception e2)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        listBox1.Items.Add(from.ToString() + " - " + to.ToString() + ": " + e2.Message);
                    });
                }
                from = from + ini.vtt.size;
            }

            client.Close();
            conn.Close();
        }

        private void UploadDone()
        {
            toolStripMenuItem1.Enabled = true;
            stopToolStripMenuItem.Enabled = false;
        }

        private void stop_download()
        {
            if (cts == null) return;
            cts.Cancel();
        }

        private async void start_download()
        {
            toolStripMenuItem1.Enabled = false;
            stopToolStripMenuItem.Enabled = true;
            listBox1.Items.Clear();

            cts = new CancellationTokenSource();

            Task t = new Task(() =>
            {
                try
                {
                    download();
                }
                catch (Exception ex)
                {

                }
            });

            t.Start();

            try { await Task.WhenAll(t); }
            finally { UploadDone(); }

            cts.Dispose();
            cts = null;
        }

        private void insertItem(MySqlConnection conn, vtt.ItemDto item)
        {
            MySqlCommand cmd = new MySqlCommand("vtt_ins", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("aid", item.Id);
            cmd.Parameters.AddWithValue("aName", item.Name);
            cmd.Parameters.AddWithValue("aBrand", item.Brand);
            cmd.Parameters.AddWithValue("aDescription", item.Description);
            cmd.Parameters.AddWithValue("aGroup", item.Group);
            cmd.Parameters.AddWithValue("aRootGroup", item.RootGroup);
            cmd.Parameters.AddWithValue("aAvailableQuantity", item.AvailableQuantity);
            cmd.Parameters.AddWithValue("aTransitQuantity", item.TransitQuantity);
            cmd.Parameters.AddWithValue("aPrice", item.Price);
            cmd.Parameters.AddWithValue("aPriceRetail", item.PriceRetail);
            cmd.Parameters.AddWithValue("aWidth", item.Width);
            cmd.Parameters.AddWithValue("aHeight", item.Height);
            cmd.Parameters.AddWithValue("aDepth", item.Depth);
            cmd.Parameters.AddWithValue("aWeight", item.Weight);
            cmd.Parameters.AddWithValue("aPhotoUrl", item.PhotoUrl);
            cmd.Parameters.AddWithValue("aOriginalNumber", item.OriginalNumber);
            cmd.Parameters.AddWithValue("aVendor", item.Vendor);
            cmd.Parameters.AddWithValue("aCompatibility", item.Compatibility);
            cmd.Parameters.AddWithValue("aBarcode", item.Barcode);
            cmd.Parameters.AddWithValue("aMainOfficeQuantity", item.MainOfficeQuantity);
            cmd.Parameters.AddWithValue("aNumberInPackage", item.NumberInPackage);
            cmd.Parameters.AddWithValue("aColorName", item.ColorName);
            cmd.Parameters.AddWithValue("aTransitDate", item.TransitDate);
            cmd.Parameters.AddWithValue("aItemLifeTime", item.ItemLifeTime);
            cmd.Parameters.AddWithValue("aNameAlias", item.NameAlias);
            cmd.ExecuteNonQuery();
        }

        private void SaveTodatabase(MySqlConnection conn, vtt.ItemPortionDto items)
        {
            for (int i = 0; i < items.Items.Length; i++)
            {
                insertItem(conn, items.Items[i]);
            }
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

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stop_download();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            stop_download();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_conf conf_frm = new frm_conf();
            conf_frm.ShowDialog();            
        }
    }
}
