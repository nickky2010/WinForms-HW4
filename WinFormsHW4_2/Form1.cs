using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsHW4_2
{
    public partial class Form1 : Form
    {
        Dictionary<int, string[]> extendFiles;
        List<FileInfo> fileInfos; 
        public Form1()
        {
            InitializeComponent();
            treeView1.ImageList = imageList1;
            fileInfos = new List<FileInfo>();
            extendFiles = new Dictionary<int, string[]>();
            extendFiles.Add(0, new string[] { "xlsm","xlsx","xls"});
            extendFiles.Add(1, new string[] { "txt", "doc", "docx" });
            extendFiles.Add(2, new string[] { "mp3", "wma", "wav", "ogg", "aac" });
            extendFiles.Add(5, new string[] { "jpg", "jpeg", "png", "bmp", "gif" });
            extendFiles.Add(6, new string[] { "cs", "csproj"});
            extendFiles.Add(8, new string[] { "exe"});
            extendFiles.Add(9, new string[] { "pdf" });
            extendFiles.Add(10, new string[] { "rar", "7z", "zip" });
            extendFiles.Add(11, new string[] { "avi", "mkv", "mp4", "mpeg", "mov" });
            listView1.Columns.Add("Name");
            listView1.Columns.Add("Сreation date");
            listView1.Columns[0].Width = 200;
            listView1.Columns[1].Width = 94;
            richTextBox1.AllowDrop = true;
    }

        void BuildTreeView(string dir, TreeNode node)
        {
            try
            {
                DirectoryInfo curDir = new DirectoryInfo(dir);
                DirectoryInfo[] dirs = curDir.GetDirectories();
                foreach (DirectoryInfo item in dirs)
                {
                    TreeNode curentNode = new TreeNode(item.Name, 3, 3);
                    node.Nodes.Add(curentNode);
                    BuildTreeView(item.FullName, curentNode);
                }
                foreach (FileInfo item in curDir.GetFiles())
                {
                    int indexImage = GetImageIndex(Path.GetExtension(item.Name).Replace(".", ""));
                    TreeNode currentNode = new TreeNode(item.Name, indexImage, indexImage);
                    node.Nodes.Add(currentNode);
                }
            }
            catch (Exception ex)
            {
                node.Nodes.Add(ex.Message);
            }
        }
        private int GetImageIndex(string ext, int key = 4)
        {
            foreach (KeyValuePair<int, string[]> keyValue in extendFiles)
            {
                foreach (string value in keyValue.Value)
                {
                    if (ext == value)
                    {
                        key = keyValue.Key;
                        return key;
                    }
                }
            }
            return key;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text!="")
            {
                treeView1.Nodes.Clear();
                string rootDir = textBox1.Text;
                TreeNode root = new TreeNode(rootDir, 7, 7);
                treeView1.Nodes.Add(root);
                BuildTreeView(rootDir, root);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                fileInfos.Clear();
                listView1.Items.Clear();
                FileInfo file = null;
                foreach (TreeNode n in e.Node.Nodes)
                {
                    file = new FileInfo(n.FullPath);
                    if (file != null)
                    {
                        fileInfos.Add(file);
                        listView1.Items.Add(file.FullName, GetImageIndex(Path.GetExtension(file.Name).Replace(".", ""),3));
                        listView1.Items[listView1.Items.Count - 1].Name = file.Name;
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(file.Name);
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(file.CreationTime.ToShortDateString());
                        listView1.Items[listView1.Items.Count - 1].SubItems[0].Text = file.Name;
                        listView1.Items[listView1.Items.Count - 1].SubItems[1].Text = file.CreationTime.ToShortDateString();
                    }
                }
            }
            catch { }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox1_Leave(new object(), new EventArgs());
                treeView1.Focus();
            }
        }

        private void smallIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.SmallIcon;
        }

        private void largeIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.LargeIcon;
        }

        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.Tile;
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.List;
        }

        private void deatailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.Details;
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                try
                {
                    if (listView1.GetItemAt(e.X, e.Y) != null)
                        listView1.DoDragDrop(listView1.GetItemAt(e.X, e.Y), DragDropEffects.Move);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void richTextBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void richTextBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                try
                {
                    ListViewItem file = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                    if (file != null)
                    {
                        richTextBox1.Clear();
                        textBox3.Text = "";
                        richTextBox1.LoadFile(fileInfos[file.Index].FullName, RichTextBoxStreamType.PlainText);
                        textBox3.Text = file.Name;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
