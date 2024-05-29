using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.ServiceModel;
using System.Data;

namespace WindowsFormsApplication2
{
    public class VttEventArgs : EventArgs
    {
        public String type { get; set; }
        public String command { get; set; }
        public String message { get; set; }

        public VttEventArgs(String _command, String _type, String _message)
        {
            command = _command;
            type = _type;
            message = _message;
        }
    }

    public class servVTT
    {
        private config ini;
        CancellationTokenSource cts;

        public servVTT(config _ini)
        {
            ini = _ini;
        }

        public void Cancel()
        {
            if (cts == null) return;
            cts.Cancel();
        }

        public delegate void Evt(VttEventArgs arg);
        public event Evt Notify;

        private void GetItemPortion_serv()
        {
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
                if (Notify != null) Notify(new VttEventArgs("GetItemPortion", "Error", e0.Message));
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
                    if(!ini.OnlyError) if (Notify != null) Notify(new VttEventArgs("GetItemPortion", "Success", "[" + from.ToString() + " - " + to.ToString() + "] Загружено!"));
                    if (items.Items.Length < ini.vtt.size)
                        break;
                }
                catch (System.ServiceModel.FaultException e1)
                {
                    if (Notify != null) Notify(new VttEventArgs("GetItemPortion", "Error", "[" + from.ToString() + " - " + to.ToString() + "] " + e1.Message));
                }
                catch (Exception e2)
                {
                    if (Notify != null) Notify(new VttEventArgs("GetItemPortion", "Error", "[" + from.ToString() + " - " + to.ToString() + "] " + e2.Message));
                }
                from = from + ini.vtt.size;
            }

            client.Close();
            conn.Close();
        }

        public async void GetItemPortion()
        {
            cts = new CancellationTokenSource();

            Task t = new Task(() =>
            {
                try
                {
                    GetItemPortion_serv();
                }
                catch (Exception ex)
                {
                    if (Notify != null) Notify(new VttEventArgs("GetItemPortion", "Error",ex.Message));
                }
            });

            t.Start();

            try { await Task.WhenAll(t); }
            finally { UploadDone(); }

            cts.Dispose();
            cts = null;
        }

        private void UploadDone()
        {
            String msg;
            if (cts.Token.IsCancellationRequested)
            {
                msg = "Прервано.";
                if (Notify != null) Notify(new VttEventArgs("GetItemPortion", "", msg));
            }
            else
            {
                msg = "Завершено!";
                if(!ini.OnlyError) if (Notify != null) Notify(new VttEventArgs("GetItemPortion", "", msg));
            }
        }

        private void SaveTodatabase(MySqlConnection conn, vtt.ItemPortionDto items)
        {
            for (int i = 0; i < items.Items.Length; i++)
            {
                insertItem(conn, items.Items[i]);
            }
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
    }
}
