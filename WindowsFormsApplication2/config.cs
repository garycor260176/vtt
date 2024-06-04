
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace WindowsFormsApplication2
{
    public class vtt_ini
    {
        public String address { get; set; }
        public int size { get; set; }
        public string login { get; set; }
        public string pwd { get; set; }
    };
    public class db_ini
    {
        public String server { get; set; }
        public int port { get; set; }
        public string login { get; set; }
        public string pwd { get; set; }
        public string dbname { get; set; }
    };

    public class config
    {
        const string filename = "settings.json";
        public bool OnlyError { get; set; }
        public bool LogToFile { get; set; }
        public float shipping_price { get; set; }

        public vtt_ini vtt;

        public db_ini db;

        public config()
        {
            vtt = new vtt_ini();
            db = new db_ini(); 
        }

        public static void Save(config ini)
        {
            try
            {
                File.WriteAllText(filename, JsonConvert.SerializeObject(ini));
            }
            catch (IOException e) {

            }
        }

        public static config Read()
        {
            config ret = new config();
            try
            {
                ret = JsonConvert.DeserializeObject<config>(File.ReadAllText(filename)); ;
            } 
            catch (IOException e) {

            }
            return ret;
        }
    }
}
