using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.ServiceModel;
using System.Data;
using System.IO;

namespace WindowsFormsApplication2
{
    public class VttEventArgs : EventArgs
    {
        public String type { get; set; }
        public String command { get; set; }
        public String message { get; set; }
        public bool done { get; set; }

        public VttEventArgs(String _command, String _type, String _message, bool _done = false)
        {
            command = _command;
            type = _type;
            message = _message;
            done = _done;
        }
    }

    public class servVTT
    {
        private config ini;
        CancellationTokenSource cts;
        filelogger logger;
        String command;

        public servVTT(config _ini, String _command)
        {
            command = _command;
            ini = _ini;
            logger = new filelogger();
            logger.filePath = logger.GenerateFilename(command, "txt");
        }

        public void Cancel()
        {
            if (cts == null) return;
            cts.Cancel();
        }

        public delegate void Evt(VttEventArgs arg);
        public event Evt Notify;

        private void SendMessage(filelogger logger, VttEventArgs arg) {
            arg.message = DateTime.Now.ToLongTimeString() + ": " + arg.message;
            if(logger != null) logger.Log(arg.message);
            if (Notify != null) Notify(arg);
        }

        private MySqlConnection OpenDB()
        {
            MySqlConnection ret;
            String connStr =
                "server=" + ini.db.server + ";" +
                "user=" + ini.db.login + ";" +
                "database=" + ini.db.dbname + ";" +
                "port=" + ini.db.port.ToString() + ";" +
                "password=" + ini.db.pwd + ";"
                ;
            ret = new MySqlConnection(connStr);
            try
            {
                ret.Open();
            }
            catch (InvalidOperationException e00)
            {
                SendMessage(logger, new VttEventArgs(command, "Error", e00.Message));
                ret = null;
            }
            catch (MySqlException e01)
            {
                SendMessage(logger, new VttEventArgs(command, "Error", e01.Message));
                ret = null;
            }

            return ret;
        }

        private vtt.PortalServiceClient OpenVtt()
        {
            vtt.PortalServiceClient ret = null;
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding() { MaxReceivedMessageSize = 131072 };
                ret = new vtt.PortalServiceClient(binding, new System.ServiceModel.EndpointAddress(ini.vtt.address));
            }
            catch (Exception e0)
            {
                SendMessage(logger, new VttEventArgs(command, "Error", e0.Message));
            }
            return ret;
        }

        public void GetItemPortion_serv(Boolean hide = false)
        {
            SendMessage(logger, new VttEventArgs(command, "", "Начало обработки"));

            MySqlConnection conn = OpenDB();
            if (conn == null) return;

            vtt.PortalServiceClient client = OpenVtt();
            if (client == null) return;

            int from = 0;
            while (true)
            {
                if (cts !=null && cts.Token.IsCancellationRequested) break;
                int to = from + ini.vtt.size - 1;
                try
                {
                    vtt.ItemPortionDto items = client.GetItemPortion(ini.vtt.login, ini.vtt.pwd, from, to);
                    SaveItemPortion(conn, items);
                    if (!ini.OnlyError) SendMessage(logger, new VttEventArgs(command, "Success", "[" + from.ToString() + " - " + to.ToString() + "] Загружено!"));
                    if (items.Items.Length < ini.vtt.size)
                        break;
                }
                catch (System.ServiceModel.FaultException e1)
                {
                    SendMessage(logger, new VttEventArgs(command, "Error", "[" + from.ToString() + " - " + to.ToString() + "] " + e1.Message));
                }
                catch (Exception e2)
                {
                    SendMessage(logger, new VttEventArgs(command, "Error", "[" + from.ToString() + " - " + to.ToString() + "] " + e2.Message));
                }
                from = from + ini.vtt.size;
            }

            client.Close();
            conn.Close();
        }

        public void GetCategories_serv(Boolean hide = false)
        {
            SendMessage(logger, new VttEventArgs(command, "", "Начало обработки"));

            MySqlConnection conn = OpenDB();
            if (conn == null) return;

            vtt.PortalServiceClient client = OpenVtt();
            if (client == null) return;

            if (cts !=null && cts.Token.IsCancellationRequested) return;
            try
            {
                vtt.CategoryDto[] cats = client.GetCategories(ini.vtt.login, ini.vtt.pwd);
                SaveCategory(conn, cats);
                if (!ini.OnlyError) SendMessage(logger, new VttEventArgs(command, "Success", "Загружено!"));
            }
            catch (System.ServiceModel.FaultException e1)
            {
                SendMessage(logger, new VttEventArgs(command, "Error", e1.Message));
            }
            catch (Exception e2)
            {
                SendMessage(logger, new VttEventArgs(command, "Error", e2.Message));
            }
            client.Close();
            conn.Close();
        }

        public async void CallService(Action method)
        {
            cts = new CancellationTokenSource();

            Task t = new Task(() =>
            {
                try
                {
                    method();
                }
                catch (Exception ex)
                {
                    SendMessage(logger, new VttEventArgs(command, "Error", ex.Message));
                }
            });

            t.Start();

            try { await Task.WhenAll(t); }
            finally { UploadDone(); }

            cts.Dispose();
            cts = null;
        }

        public void GetItemPortion()
        {
            CallService( () => GetItemPortion_serv(false) );
        }

        public void GetCategories()
        {
            CallService(() => GetCategories_serv(false));
        }

         private void UploadDone()
        {
            String msg;
            if (cts.Token.IsCancellationRequested)
            {
                msg = "Обработка прервана";
                SendMessage(logger, new VttEventArgs(command, "", msg, true));
            }
            else
            {
                msg = "Обработка завершена";
                SendMessage(logger, new VttEventArgs(command, "", msg, true));
            }
        }

         private void SaveCategory(MySqlConnection conn, vtt.CategoryDto[] cats)
         {
            for (int i = 0; i < cats.Length; i++)
            {
                insertCat(conn, cats[i]);
            }

         }

         private void insertCat(MySqlConnection conn, vtt.CategoryDto cat)
         {
             MySqlCommand cmd = new MySqlCommand("CategoryDto_ins", conn);
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.Parameters.AddWithValue("aId", cat.Id);
             cmd.Parameters.AddWithValue("aParentId", cat.ParentId);
             cmd.Parameters.AddWithValue("aName", cat.Name);
             cmd.ExecuteNonQuery();
         }

         private void SaveItemPortion(MySqlConnection conn, vtt.ItemPortionDto items)
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
