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
        private CancellationTokenSource cts;
        private filelogger logger;
        private String command;
        private db mysql;

        private void DB_Message(String message, db.TypeError type)
        {
            String lv_type;
            switch (type)
            {
                case db.TypeError.Success: lv_type = "Succcess: "; break;
                default: lv_type = "Error: "; break;
            }
            SendMessage(logger, new VttEventArgs(command, lv_type, message));
        }

        public servVTT(config _ini, String _command)
        {
            command = _command;
            ini = _ini;
            logger = new filelogger();
            logger.filePath = logger.GenerateFilename(command, "txt");

            mysql = new db();
            mysql.Notify += DB_Message;
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
            if(ini.LogToFile && logger != null) logger.Log(arg.message);
            if (Notify != null) Notify(arg);
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

            if (mysql.OpenDB() == db.OpenResult.Error) return;

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
                    SaveItemPortion(items);
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
            mysql.CloseDB();
        }

        public void GetCategories_serv(Boolean hide = false)
        {
            SendMessage(logger, new VttEventArgs(command, "", "Начало обработки"));

            if (mysql.OpenDB() == db.OpenResult.Error) return;

            vtt.PortalServiceClient client = OpenVtt();
            if (client == null) return;

            if (cts !=null && cts.Token.IsCancellationRequested) return;
            try
            {
                vtt.CategoryDto[] cats = client.GetCategories(ini.vtt.login, ini.vtt.pwd);
                SaveCategory(cats);
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
            mysql.CloseDB();
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

         private void SaveCategory(vtt.CategoryDto[] cats)
         {
            for (int i = 0; i < cats.Length; i++)
            {
                mysql.insertCat(cats[i]);
            }

         }

         private void SaveItemPortion(vtt.ItemPortionDto items)
        {
            for (int i = 0; i < items.Items.Length; i++)
            {
                mysql.insertItem(items.Items[i]);
            }
        }
    }
}
