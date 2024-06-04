using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class frm_conf : Form
    {
        public frm_conf()
        {
            InitializeComponent();
            config conf = config.Read();
            vtt_address.Text = conf.vtt.address;
            vtt_login.Text = conf.vtt.login;
            vtt_password.Text = conf.vtt.pwd;
            vtt_size.Value = (conf.vtt.size < 1 ? 1 : conf.vtt.size);

            mysql_dbname.Text = conf.db.dbname;
            mysql_login.Text = conf.db.login;
            mysql_password.Text = conf.db.pwd;
            mysql_port.Value = conf.db.port;
            mysql_address.Text = conf.db.server;

            OnlyError.Checked = conf.OnlyError;
            LogToFile.Checked = conf.LogToFile;
            shipping_price.Text = conf.shipping_price.ToString();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            config conf = new config();
            conf.vtt.address = vtt_address.Text;
            conf.vtt.login = vtt_login.Text;
            conf.vtt.pwd = vtt_password.Text;
            conf.vtt.size = Convert.ToInt32(vtt_size.Value);

            conf.db.server = mysql_address.Text;
            conf.db.port = Convert.ToInt32(mysql_port.Value);
            conf.db.login = mysql_login.Text;
            conf.db.pwd = mysql_password.Text;
            conf.db.dbname = mysql_dbname.Text;

            conf.OnlyError = OnlyError.Checked;
            conf.LogToFile = LogToFile.Checked;
            conf.shipping_price = Convert.ToSingle(shipping_price.Text);

            config.Save(conf);
            this.Close();
        }

        private void shipping_price_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !db.CheckKeyPressSum(shipping_price.Text, e);
        }

        private void shipping_price_Validating(object sender, CancelEventArgs e)
        {
            if (!db.CheckSum(shipping_price.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(shipping_price, "Нужно ввести сумму");
            }
            else errorProvider1.SetError(shipping_price, "");
        }
    }
}
