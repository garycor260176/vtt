using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public struct vtt_settings
        {
            public String address;
            public int size;
            public string login;
            public string pwd;
        };
        public struct msql_settings
        {
            public String server;
            public string port;
            public string login;
            public string pwd;
            public string dbname;
        };

        public struct settings
        {
            public vtt_settings vtt;
            public msql_settings mysql;
        };

        settings ini;

        public Form1()
        {
            InitializeComponent();
        }

        private settings readSettings() //чтение настройки из какого-нить файла (xml, ini или что-то еще)
        {
            settings ret;
            ret.vtt.address = "http://api.vtt.ru:8048/Portal.svc";
            ret.vtt.size = 20;
            ret.vtt.login = "am-458";
            ret.vtt.pwd = "fcv34xvysd";

            ret.mysql.server = "192.168.1.40";
            ret.mysql.port = "3306";
            ret.mysql.login = "root";
            ret.mysql.pwd = "iobroker";
            ret.mysql.dbname = "vtt";

            return ret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            ini = readSettings();
            String connStr = 
                "server=" + ini.mysql.server + ";" + 
                "user=" + ini.mysql.login + ";" + 
                "database=" + ini.mysql.dbname + ";" + 
                "port=" + ini.mysql.port + ";" + 
                "password=" + ini.mysql.pwd + ";" 
                ;

            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            int from = 0;
            vtt.PortalServiceClient client = new vtt.PortalServiceClient();
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(ini.vtt.address);
            while (true)
            {
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
                    listBox1.Items.Add(from.ToString() + " - " + to.ToString() + ": " + e1.Message);

                    //e1.Message; куда-нить в лог
                }
                from = from + ini.vtt.size;
            }
            client.Close();
            conn.Close();
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

        }
    }
}
