using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace WindowsFormsApplication2
{
    class db
    {
        public class CatKoef
        {
            public CatKoef(String _CatId, Decimal _sPrice, Decimal _ePrice, Decimal _koef)
            {
                CatId = _CatId;
                sPrice = _sPrice;
                ePrice = _ePrice;
                koef = _koef;
            }
            public String CatId { get; set; }
            public Decimal sPrice { get; set; }
            public Decimal ePrice { get; set; }
            public Decimal koef { get; set; }
        };

        public class CatNode
        {
            public CatNode(String _Id, String _ParentId, String _Name, int _childs)
            {
                Id = _Id;
                ParentId = _ParentId;
                Name = _Name;
                childs = _childs;
            }
            public String Id { get; set; }
            public String ParentId { get; set; }
            public String Name { get; set; }
            public int childs { get; set; }
            public List<CatKoef> koefs { get; set; }

            public Boolean selectedKoef;
        };

        public enum OpenResult {
            WasOpened,
            Opened,
            Error
        };

        private config ini;
        MySqlConnection conn;

        public db()
        {
            ini = config.Read();
        }

        public delegate void Evt(String message);
        public event Evt Notify;

        private void SendMessage(String message)
        {
            if (Notify != null) Notify(message);
        }

        public OpenResult OpenDB()
        {
            if (conn != null && conn.State == ConnectionState.Open) return OpenResult.WasOpened;

            OpenResult ret = OpenResult.Error;

            String connStr =
                "server=" + ini.db.server + ";" +
                "user=" + ini.db.login + ";" +
                "database=" + ini.db.dbname + ";" +
                "port=" + ini.db.port.ToString() + ";" +
                "password=" + ini.db.pwd + ";"
                ;
            conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                ret = OpenResult.Opened;
            }
            catch (InvalidOperationException e00)
            {
                SendMessage(e00.Message);
            }
            catch (MySqlException e01)
            {
                SendMessage(e01.Message);
            }

            if (ret == OpenResult.Error) conn = null;

            return ret;
        }

        public void CloseDB()
        {
            if (conn != null) conn.Close();
            conn = null;
        }

        public List<CatKoef> GetCatKoef(String CatId)
        {
            List<CatKoef> ret = new List<CatKoef>();

            OpenResult open = OpenDB();
            if (open == OpenResult.Error) return ret;

            try
            {
                MySqlCommand cmd = new MySqlCommand("get_koef", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("aCatId", CatId);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ret.Add(new CatKoef(reader["CatId"].ToString(), Convert.ToDecimal(reader["sprice"]), 
                                        Convert.ToDecimal(reader["eprice"]), Convert.ToDecimal(reader["koef"])));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                SendMessage(ex.Message);
            }

            if (open == OpenResult.Opened) CloseDB();
            return ret;
        }

        public static Boolean IsEmptyKoef(db.CatKoef koef)
        {
            return (koef.ePrice == 0 && koef.sPrice == 0 && koef.koef == 0);
        }

        public void InsertCatKoefs(CatNode node)
        {
            if (node.koefs == null) return;

            OpenResult open = OpenDB();
            if (open == OpenResult.Error) return;

            try
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM koef where CatId = '" + node.Id + "'", conn);
                command.ExecuteNonQuery();

                String insert = "";
                foreach (CatKoef koef in node.koefs)
                {
                    if (IsEmptyKoef(koef)) continue;
                    if (insert.Length > 0) insert = insert + ",";
                    insert = insert + "('" + node.Id + "', " + koef.sPrice.ToString().Replace(",", ".") + ", " +
                                                               koef.ePrice.ToString().Replace(",", ".") + ", " +
                                                               koef.koef.ToString().Replace(",", ".") + ")";
                }
                command.CommandText = "INSERT INTO koef VALUES" + insert + ";";
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                SendMessage(ex.Message);
            }

            if (open == OpenResult.Opened) CloseDB();
        }

        public void insertCat(vtt.CategoryDto cat)
        {
            OpenResult open = OpenDB();
            if (open == OpenResult.Error) return;

            try
            {
                MySqlCommand cmd = new MySqlCommand("CategoryDto_ins", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("aId", cat.Id);
                cmd.Parameters.AddWithValue("aParentId", (cat.ParentId == null ? "" : cat.ParentId));
                cmd.Parameters.AddWithValue("aName", cat.Name);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                SendMessage(ex.Message);
            }

            if (open == OpenResult.Opened) CloseDB();
        }

        public void insertItem(vtt.ItemDto item)
        {
            OpenResult open = OpenDB();
            if (open == OpenResult.Error) return;
            try { 
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
            catch (MySqlException ex)
            {
                SendMessage(ex.Message);
            }

            if (open == OpenResult.Opened) CloseDB();
        }

        public List<CatNode> GetCategoriesLevel(String ParentId)
        {
            List<CatNode> ret = new List<CatNode>();

            OpenResult open = OpenDB();
            if (open == OpenResult.Error) return ret;

            try
            {
                MySqlCommand cmd = new MySqlCommand("GetCats", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("aParentId", ParentId);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ret.Add(new CatNode(reader["Id"].ToString(), reader["ParentId"].ToString(), reader["Name"].ToString(), Convert.ToInt16(reader["childcount"])));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                SendMessage(ex.Message);
            }
          
            if (open == OpenResult.Opened) CloseDB();
            return ret;
        } 
    }
}
