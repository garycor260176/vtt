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
        private void DB_Message(String message, db.TypeError type)
        {
            String lv_type;
            switch (type)
            {
                case db.TypeError.Success: lv_type = ""; break;
                default: lv_type = "Error: "; break;
            }
            StatusLabel.Text = lv_type + message;
        }
        private void frm_category_Load(object sender, EventArgs e)
        {
            mysql = new db();
            mysql.Notify += DB_Message;

            StatusLabel.Text = "";
            tree.ImageList = imageList1;
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
                    ForeColor = (node.cntkoefs > 0 ? Color.Black : Color.Red),
                    Tag = new db.NodeT(db.TypeNode.category, node, null),
                    ImageIndex = 0,
                    SelectedImageIndex = 0,
                    Text = node.Name + (node.cntitems > 0 ? " (" + node.cntitems.ToString() + ")" : "")
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
                if (node.childs == 0 && node.cntitems > 0)
                {
                    TreeNode EmptyItem = new TreeNode
                    {
                        Tag = new db.NodeT(db.TypeNode.item, null, null),
                        ImageIndex = 1,
                        SelectedImageIndex = 1,
                        Text = "Empty"
                    };
                    child.Nodes.Add(EmptyItem);
                }
            }
        }
        private List<db.CatKoef> GetKoefByCat(db.CatNode node)
        {
            if (!node.selectedKoef)
            {
                node.koefs = mysql.GetCatKoef(node.Id);
                node.selectedKoef = true;
            }
            return node.koefs;
        }
        private List<db.CatKoef> GetKoefByItem(db.ItemNode node)
        {
            if (!node.selectedKoef)
            {
                node.koefs = mysql.GetItemKoef(node.Id);
                node.selectedKoef = true;
            }
            return node.koefs;
        }
        private List<db.CatKoef> GetKoeff(db.NodeT node)
        {
            switch (node.type)
            {
                case db.TypeNode.category: return GetKoefByCat(node.cat);
                case db.TypeNode.item: return GetKoefByItem(node.item);
            }
            return null;
        }
        private db.NodeT GetSelectedNode()
        {
            if (tree.SelectedNode == null) return null;
            return (db.NodeT)tree.SelectedNode.Tag;
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
        private void ShowCatKoef(db.CatNode node)
        {
            if (!node.selectedKoef)
            {
                node.selectedKoef = true;
                node.koefs = mysql.GetCatKoef(node.Id);
                node.selectedKoef = true;
            }
        }
        private void ShowItemKoef(db.ItemNode node)
        {
            if (!node.selectedKoef)
            {
                node.selectedKoef = true;
                node.koefs = mysql.GetItemKoef(node.Id);
                node.selectedKoef = true;
            }
        }
        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            db.NodeT node = (db.NodeT)e.Node.Tag;
            switch (node.type)
            {
                case db.TypeNode.category: ShowCatKoef(node.cat); break;
                case db.TypeNode.item: ShowItemKoef(node.item); break;
            }
            refresh();
        }
        private void AddCatKoef(db.CatNode node)
        {
            node.koefs.Add(new db.CatKoef(node.Id, 0, 0, 0));
            refresh();
            GridKoef.CurrentCell = GridKoef.Rows[node.koefs.Count - 1].Cells[2];
        }
        private void AddItemKoef(db.ItemNode node)
        {
            node.koefs.Add(new db.CatKoef(node.Id, 0, 0, 0));
            refresh();
            GridKoef.CurrentCell = GridKoef.Rows[node.koefs.Count - 1].Cells[2];
        }
        private void AddKoef_Click(object sender, EventArgs e)
        {
            db.NodeT node = GetSelectedNode();
            switch (node.type)
            {
                case db.TypeNode.category: AddCatKoef(node.cat); break;
                case db.TypeNode.item: AddItemKoef(node.item); break;
            }
            RecalcColorNode();
        }
        private void DelCatKoef(db.CatNode node)
        {
            for (int i = GridKoef.Rows.Count - 1; i >= 0; i--)
            {
                if (GridKoef.Rows[i].Selected)
                {
                    node.koefs.RemoveAt(i);
                }
            }
        }
        private void DelItemKoef(db.ItemNode node)
        {
            for (int i = GridKoef.Rows.Count - 1; i >= 0; i--)
            {
                if (GridKoef.Rows[i].Selected)
                {
                    node.koefs.RemoveAt(i);
                }
            }
        }
        private void DelKoef_Click(object sender, EventArgs e)
        {
            db.NodeT node = GetSelectedNode();
            switch (node.type)
            {
                case db.TypeNode.category: DelCatKoef(node.cat); break;
                case db.TypeNode.item: DelItemKoef(node.item); break;
            }
            refresh();
            RecalcColorNode();
        }
        private Boolean CheckNode(db.NodeT node)
        {
            String type = "";
            String Name = "";
            if (node == null) return true;
            List<db.CatKoef> koefs = null;
            switch (node.type)
            {
                case db.TypeNode.category:
                    koefs = node.cat.koefs;
                    type = "категории";
                    Name = node.cat.Name;
                    break;
                case db.TypeNode.item:
                    koefs = node.item.koefs;
                    type = "Номенклатуры";
                    Name = node.item.Name;
                    break;
            }
            if (koefs == null) return true;
            for (int i = 0; i < koefs.Count; i++) {
                if (db.IsEmptyKoef(koefs[i])) continue;

                if (koefs[i].sPrice > koefs[i].ePrice)
                {
                    MessageBox.Show("Проверьте дипазон цен для " + type + " '" + Name + "'. Начальная цена превышает конечную.");
                    return false;
                }
                else
                {
                    for (int j = i + 1; j < koefs.Count; j++) {
                        if(!(koefs[i].sPrice > koefs[j].ePrice || koefs[i].ePrice < koefs[j].sPrice))
                        {
                            MessageBox.Show("Проверьте дипазон цен для " + type + " '" + Name + "'. Пересечение диапазона цен.");
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
        private Boolean checkAllNodes()
        {
            Boolean ret = true;
            foreach (TreeNode node in tree.Nodes)
            {
                if (!CheckNode((db.NodeT)node.Tag))
                {
                    ret = false;
                    break;
                }

            }
            return ret;
        }

        private void AddNode2List(List<db.CatNode> CatNodes, List<db.ItemNode> ItemNodes, TreeNode tnode)
        {
            foreach (TreeNode node in tnode.Nodes)
            {
                db.NodeT n = (db.NodeT)node.Tag;
                switch (n.type)
                {
                    case db.TypeNode.category: 
                        CatNodes.Add(n.cat); 
                        AddNode2List(CatNodes, ItemNodes, node);
                        break;
                    case db.TypeNode.item: 
                        if (n.item != null) ItemNodes.Add(n.item); break;
                }
            }
        }

        private void SaveNodes()
        {
            if (mysql.OpenDB() == db.OpenResult.Error) return;
            List<db.CatNode> CatNodes = new List<db.CatNode>();
            List<db.ItemNode> ItemNodes = new List<db.ItemNode>();
            foreach (TreeNode node in tree.Nodes)
            {
                db.NodeT n = (db.NodeT)node.Tag;
                switch (n.type)
                {
                    case db.TypeNode.category: 
                        CatNodes.Add(n.cat); 
                        AddNode2List(CatNodes, ItemNodes, node);
                        break;
                    case db.TypeNode.item: 
                        if(n.item != null) ItemNodes.Add(n.item); break;
                }
            }
            mysql.InsertCatsKoefs(CatNodes);
            mysql.InsertItemsKoefs(ItemNodes);
            mysql.CloseDB();
        }
        private void Save_Click(object sender, EventArgs e)
        {
            if (!checkAllNodes()) return;
            SaveNodes();
        }
        private void GridKoef_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (GridKoef.IsCurrentCellDirty)
            {
                GridKoef.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void RecalcColorNode()
        {
            tree.SelectedNode.ForeColor = Color.Red;
            db.NodeT node = GetSelectedNode();
            List<db.CatKoef> koefs = null;
            switch (node.type)
            {
                case db.TypeNode.category: koefs = node.cat.koefs; break;
                case db.TypeNode.item: koefs = node.item.koefs; break;
            }
            int cnt = 0;
            for (int i = 0; i < koefs.Count; i++)
            {
                if (db.IsEmptyKoef(koefs[i])) continue;
                cnt++;
            }
            if(cnt > 0) tree.SelectedNode.ForeColor = Color.Black;
        }
        private void GridKoef_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            RecalcColorNode();
        }
        private void LoadItems(TreeNode nTree, db.CatNode node)
        {
            if (node.selectedItems) return;

            foreach (TreeNode n in nTree.Nodes)
            {
                db.NodeT dbNode = (db.NodeT)n.Tag;
                if (dbNode.type == db.TypeNode.item && dbNode.item == null)
                {
                    n.Remove();
                    break;
                }
            }

            if (mysql.OpenDB() == db.OpenResult.Error) return;

            List<db.ItemNode> items = mysql.GetItemsByCategory(node.Id);
            foreach (db.ItemNode i in items)
            {
                TreeNode child = new TreeNode
                {
                    Tag = new db.NodeT(db.TypeNode.item, null, i),
                    ImageIndex = 1,
                    SelectedImageIndex = 1,
                    Text = i.Name
                };
                nTree.Nodes.Add(child);
            }

            mysql.CloseDB();
            node.selectedItems = true;
        }
        private void tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            db.NodeT node = (db.NodeT)e.Node.Tag;
            switch (node.type)
            {
                case db.TypeNode.category:
                    LoadItems(e.Node, node.cat);
                    break;
            }
        }
        private void GridKoef_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            GridKoef.Rows[e.RowIndex].ErrorText =
                GridKoef.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";
        }

        private void GridKoef_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            object editedValue = GridKoef[e.ColumnIndex, e.RowIndex].EditedFormattedValue;
            float temp;
            if (!float.TryParse(editedValue.ToString(), out temp))
            {
                GridKoef.Rows[e.RowIndex].ErrorText = 
                    GridKoef.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Ошибка ввода";
                e.Cancel = true;
                e.ThrowException = false;
            }
        }
    }
}