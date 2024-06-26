﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

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

        public class ItemNode
        {
            public ItemNode(String _Id, String _CatId, String _Name, int _cntkoefs, Boolean _empty,
                            Boolean _NotAvailable, Boolean _shipping_price_from_item,
                            float _price_for_site, float _shipping_price)
            {
                Id = _Id;
                CatId = _CatId;
                Name = _Name;
                cntkoefs = _cntkoefs;
                empty = _empty;
                NotAvailable = _NotAvailable;
                shipping_price_from_item = _shipping_price_from_item;
                price_for_site = _price_for_site;
                shipping_price = _shipping_price;

            }
            public Boolean NotAvailable { get; set; }
            public Boolean shipping_price_from_item { get; set; }
            public float price_for_site { get; set; }
            public float shipping_price { get; set; }
            public String Id { get; set; }
            public String CatId { get; set; }
            public String Name { get; set; }
            public int cntkoefs { get; set; }
            public List<CatKoef> koefs { get; set; }
            public Boolean selectedKoef { get; set; }
            public Boolean empty { get; set; }
        };

        public class CatNode
        {
            public CatNode(String _Id, String _ParentId, String _Name, int _childs, int _cntkoefs, int _cntitems,
                           float _shipping_price)
            {
                Id = _Id;
                ParentId = _ParentId;
                Name = _Name;
                childs = _childs;
                cntkoefs = _cntkoefs;
                cntitems = _cntitems;
                shipping_price = _shipping_price;
            }
            public float shipping_price { get; set; }
            public String Id { get; set; }
            public String ParentId { get; set; }
            public String Name { get; set; }
            public int childs { get; set; }
            public int cntkoefs { get; set; }
            public int cntitems { get; set; }
            public List<CatKoef> koefs { get; set; }
            public Boolean selectedKoef { get; set; }
            public Boolean selectedItems { get; set; }
        };

        public enum TypeNode
        {
            category,
            item
        };

        public class NodeT
        {
            public NodeT(TypeNode _type, db.CatNode _cat, db.ItemNode _item)
            {
                type = _type;
                cat = _cat;
                item = _item;
            }
            public TypeNode type { get; set; }
            public db.CatNode cat { get; set; }
            public db.ItemNode item { get; set; }
            public Boolean ItemsDownloaded { get; set; }
        }

        public enum TypeError
        {
            Error,
            Success
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

        public delegate void Evt(String message, TypeError type);
        public event Evt Notify;

        private void SendMessage(String message, TypeError type = TypeError.Error)
        {
            if (Notify != null) Notify(message, type);
        }

        public static Boolean CheckSum(String text){
            return System.Text.RegularExpressions.Regex.IsMatch(text, "^\\d{0,8}(\\,\\d{1,2})?$");
        }

        public static Boolean CheckKeyPressSum(String text, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.') e.KeyChar = ',';
            if (e.KeyChar == ',' && text.IndexOf(",") >= 0) return false;

            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 44) return false;

            return true;
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

        public List<CatKoef> GetItemKoef(String Id)
        {
            List<CatKoef> ret = new List<CatKoef>();

            OpenResult open = OpenDB();
            if (open == OpenResult.Error) return ret;

            try
            {
                MySqlCommand cmd = new MySqlCommand("GetItemKoef", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("aItemId", Id);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ret.Add(new CatKoef(reader["ItemId"].ToString(), Convert.ToDecimal(reader["sprice"]),
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

        public Boolean InsertCatsKoefs(List<CatNode> nodes)
        {
            foreach (CatNode node in nodes)
            {
                if (!InsertCatKoefs(node)) return false;
            }
            SendMessage("Сохранено!", TypeError.Success);
            return true;
        }
        public Boolean InsertItemsKoefs(List<ItemNode> nodes)
        {
            foreach (ItemNode node in nodes)
            {
                if (!InsertItemKoefs(node)) return false;
            }
            SendMessage("Сохранено!", TypeError.Success);
            return true;
        }

        public Boolean RecalcAllPrice( )
        {
            Boolean ret = false;

            OpenResult open = OpenDB();
            if (open == OpenResult.Error) return false;

            try
            {
                MySqlCommand cmd = new MySqlCommand("RecalcPriceInCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("aCatId", "");
                cmd.Parameters.AddWithValue("aConfShippingPrice", ini.shipping_price);
                cmd.Parameters.AddWithValue("aNoRecursive", 1);
                cmd.ExecuteNonQuery();
                ret = true;
            }
            catch (MySqlException ex)
            {
                SendMessage(ex.Message);
            }

            if (open == OpenResult.Opened) CloseDB();
            return ret;
        }

        public Boolean InsertCatKoefs(CatNode node)
        {
            Boolean ret = false;
            if (node.koefs == null) return true;

            OpenResult open = OpenDB();
            if (open == OpenResult.Error) return false;

            try
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM koef where CatId = '" + node.Id + "'", conn);
                command.ExecuteNonQuery();

                if (node.koefs.Count > 0)
                {
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
                ret = true;
            }
            catch (MySqlException ex)
            {
                SendMessage(ex.Message);
            }

            try
            {
                MySqlCommand cmd = new MySqlCommand("CategoryDto_upd", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("aId", node.Id);
                cmd.Parameters.AddWithValue("ashipping_price", node.shipping_price);
                cmd.Parameters.AddWithValue("aConfShippingPrice", ini.shipping_price);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                SendMessage(ex.Message);
            }

            if (open == OpenResult.Opened) CloseDB();
            return ret;
        }
        public Boolean InsertItemKoefs(ItemNode node)
        {
            Boolean ret = false;
            if (node.koefs == null) return true;

            OpenResult open = OpenDB();
            if (open == OpenResult.Error) return false;

            try
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM itemkoef where ItemId = '" + node.Id + "'", conn);
                command.ExecuteNonQuery();

                if (node.koefs.Count > 0)
                {
                    String insert = "";
                    foreach (CatKoef koef in node.koefs)
                    {
                        if (IsEmptyKoef(koef)) continue;
                        if (insert.Length > 0) insert = insert + ",";
                        insert = insert + "('" + node.Id + "', " + koef.sPrice.ToString().Replace(",", ".") + ", " +
                                                                    koef.ePrice.ToString().Replace(",", ".") + ", " +
                                                                    koef.koef.ToString().Replace(",", ".") + ")";
                    }
                    command.CommandText = "INSERT INTO itemkoef VALUES" + insert + ";";
                    command.ExecuteNonQuery();
                }
                ret = true;
            }
            catch (MySqlException ex)
            {
                SendMessage(ex.Message);
            }

            try
            {
                MySqlCommand cmd = new MySqlCommand("ItemDto_upd", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("aId", node.Id);
                cmd.Parameters.AddWithValue("ashipping_price", node.shipping_price);
                cmd.Parameters.AddWithValue("aNotAvailable", (node.NotAvailable ? "X" : ""));
                cmd.Parameters.AddWithValue("ashipping_price_from_item", (node.shipping_price_from_item ? "X" : ""));
                cmd.Parameters.AddWithValue("aConfShippingPrice", ini.shipping_price);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                SendMessage(ex.Message);
            }

            if (open == OpenResult.Opened) CloseDB();

            return ret;
        }

        private String readerStringField(object obj)
        {
            return (Convert.IsDBNull(obj) ? "" : obj).ToString();
        }
        private Boolean readerBoolField(object obj)
        {
            return ((Convert.IsDBNull(obj) ? "" : obj).ToString() == "X");
        }
        private float readerFloatField(object obj)
        {
            return Convert.ToSingle((Convert.IsDBNull(obj) ? 0 : obj));
        }
        private int readerIntField(object obj)
        {
            return Convert.ToInt16((Convert.IsDBNull(obj) ? 0 : obj));
        }

        public List<ItemNode> GetItemsByCategory(String CatId, String aItemId = "")
        {
            List<ItemNode> ret = new List<ItemNode>();

            OpenResult open = OpenDB();
            if (open == OpenResult.Error) return ret;

            try
            {
                MySqlCommand cmd = new MySqlCommand("GetCatItems", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("aCatId", CatId);
                cmd.Parameters.AddWithValue("aItemId", aItemId);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ret.Add(new ItemNode(readerStringField(reader["Id"]),
                                         readerStringField(reader["CatId"]),
                                         readerStringField(reader["Name"]),        
                                         readerIntField(reader["countkoefs"]), 
                                         false,
                                         readerBoolField(reader["NotAvailable"]),
                                         readerBoolField(reader["shipping_price_from_item"]),
                                         readerFloatField(reader["price_for_site"]),
                                         readerFloatField(reader["shipping_price"])
                                         ));
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
                    ret.Add(new CatNode(readerStringField(reader["Id"]),
                                        readerStringField(reader["ParentId"]),
                                        readerStringField(reader["Name"]),
                                        readerIntField(reader["childcount"]),
                                        readerIntField(reader["countkoefs"]),
                                        readerIntField(reader["countitems"]),
                                        readerFloatField(reader["shipping_price"])
                                        ));
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
