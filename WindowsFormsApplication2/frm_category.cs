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
    public partial class frm_category : Form
    {
        private db mysql;

        public frm_category()
        {
            InitializeComponent();
        }

        private void DB_Message(String message)
        {
            StatusLabel.Text = "Error: " + message;
        }

        private void frm_category_Load(object sender, EventArgs e)
        {
            mysql = new db();
            mysql.Notify += DB_Message;

            StatusLabel.Text = "";
            LoadTree();
        }

        private void LoadTree()
        {
            if (mysql.OpenDB() == db.OpenResult.Error) return;
            PopulateTreeView(mysql.GetCategoriesLevel(""), "", null);
            mysql.CloseDB();
        }

        private void PopulateTreeView(List<db.CatNode> nodes, String ParentId, TreeNode treeNode){
            foreach (db.CatNode node in nodes)
            {
                TreeNode child = new TreeNode
                {
                    Text = node.Name,
                    Tag = node
                };
                if (ParentId.Length == 0)
                {
                    tree.Nodes.Add(child);
                    PopulateTreeView(mysql.GetCategoriesLevel(node.Id), child.Tag.ToString(), child);
                }
                else
                {
                    treeNode.Nodes.Add(child);
                }
            }
        }

        private void ExpandAll_Click(object sender, EventArgs e)
        {
            tree.ExpandAll();
        }

        private void CollapseAll_Click(object sender, EventArgs e)
        {
            tree.CollapseAll();
        }


        private List<db.CatKoef> GetKoeff(db.CatNode node){
            if (!node.selectedKoef)
            {
                node.koefs = mysql.GetCatKoef(node.Id);
                node.selectedKoef = true;
            }
            return node.koefs;
        }

        private db.CatNode GetSelectedNode()
        {
            if (tree.SelectedNode == null) return null;
            return (db.CatNode)tree.SelectedNode.Tag;
        }

        private void refresh()
        {
            GridKoef.DataSource = GetKoeff(GetSelectedNode()).ToList();

            GridKoef.Columns["catId"].HeaderText = "catId";
            GridKoef.Columns["catId"].Visible = false;

            GridKoef.Columns["sprice"].HeaderText = "Нач.Цена";

            GridKoef.Columns["eprice"].HeaderText = "Кон.цена";

            GridKoef.Columns["koef"].HeaderText = "Коэф-т";
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            db.CatNode node = (db.CatNode)e.Node.Tag;
            if(!node.selectedKoef) {
                node.selectedKoef = true;
                node.koefs = mysql.GetCatKoef(node.Id);
                node.selectedKoef = true;
            }
            refresh();
        }

        private void AddKoef_Click(object sender, EventArgs e)
        {
            db.CatNode node = GetSelectedNode();
            node.koefs.Add(new db.CatKoef(node.Id, 0,0,0));
            refresh();
            GridKoef.CurrentCell = GridKoef.Rows[node.koefs.Count-1].Cells[2];
        }

        private void DelKoef_Click(object sender, EventArgs e)
        {
            db.CatNode node = GetSelectedNode();
            for (int i = GridKoef.Rows.Count - 1; i >= 0; i--)
            {
                if (GridKoef.Rows[i].Selected)
                {
                    node.koefs.RemoveAt(i);
                }
            }
            refresh();
        }

        private Boolean CheckNode(db.CatNode node)
        {
            if (node == null) return true;
            if (node.koefs == null) return true;
            List<db.CatKoef> koefs = node.koefs;
            for (int i = 0; i < koefs.Count; i++) {
                if (db.IsEmptyKoef(koefs[i])) continue;

                if (koefs[i].sPrice > koefs[i].ePrice)
                {
                    MessageBox.Show("Проверьте дипазон цен для '" + node.Name + "'. Начальная цена превышает конечную.");
                    return false;
                }
                else
                {
                    for (int j = i + 1; j < koefs.Count; j++) {
                        if(!(koefs[i].sPrice > koefs[j].ePrice || koefs[i].ePrice < koefs[j].sPrice))
                        {
                            MessageBox.Show("Проверьте дипазон цен для '" + node.Name + "'. Пересечение диапазона цен.");
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private Boolean ErrorCurrentNode()
        {
            return !CheckNode(GetSelectedNode());
        }

        private void tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = ErrorCurrentNode();
        }

        private Boolean checlAllNodes()
        {
            Boolean ret = true;
            foreach (TreeNode node in tree.Nodes)
            {
                if (!CheckNode((db.CatNode)node.Tag))
                {
                    ret = false;
                    break;
                }

            }
            return ret;
        }

        private void SaveNodes()
        {
            if (mysql.OpenDB() == db.OpenResult.Error) return;
                
            foreach (TreeNode node in tree.Nodes)
            {
                mysql.InsertCatKoefs((db.CatNode)node.Tag);
            }

            mysql.CloseDB();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (!checlAllNodes()) return;
            SaveNodes();
        }

    }
}