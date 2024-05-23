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
        public struct settings
        {
            public String address;
            public int size;
            public string login;
            public string pwd;
        };

        settings ini;

        public Form1()
        {
            InitializeComponent();
        }

        private settings readSettings() //чтение настройки из какого-нить файла
        {
            settings ret;
            ret.address = "http://api.vtt.ru:8048/Portal.svc";
            ret.size = 20;
            ret.login = "am-458";
            ret.pwd = "fcv34xvysd";
            return ret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ini = readSettings();
            int from = 0;
            vtt.PortalServiceClient client = new vtt.PortalServiceClient();
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(ini.address);
           
            while (true)
            {
                try
                {
                    vtt.ItemPortionDto items = client.GetItemPortion(ini.login, ini.pwd, from, from + ini.size - 1);
                    if(items.Items.Length == 0) break;
                    from = from + ini.size;
                    SaveTodatabase(items);
                }
                catch (System.ServiceModel.FaultException e1)
                { 
                    //e1.Message; куда-нить в лог
                    break;
                }
            }
            client.Close();
        }

        private void SaveTodatabase(vtt.ItemPortionDto items)
        {
            for (int i = 0; i < items.Items.Length; i++)
            {
                //select id
                //    from табличка 
                //    where id = items.Items[i].Id
                //if(запись есть) {
                    //Update табличка set все поля кроме id
                    //    where id = items.Items[i].Id
                //} else {
                    //делаем Insert
                //}
            }
        }
    }
}
