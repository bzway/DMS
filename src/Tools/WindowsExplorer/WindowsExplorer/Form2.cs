using ExplorerTreeView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowsExplorer
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public List<ContextMenuItem> ContextMenuItems = new List<ContextMenuItem>();
        private void Form2_Load(object sender, EventArgs e)
        {
            this.treeView1.ImageList = this.listView1.SmallImageList = this.imageList1;
            this.treeView1.ShowLines = this.treeView1.ShowPlusMinus = this.treeView1.ShowRootLines = false;
            this.treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            PopulateTreeView();
            this.listView1.ContextMenu = new ContextMenu();
            this.listView1.ContextMenu.Popup += ContextMenu_Popup;
            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                Type = ContextMenuItemType.None,
                Name = "Open",
                Command = "explorer",
                ShortCut = Shortcut.CtrlO,
            });
            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                Type = ContextMenuItemType.File,
                Name = "View",
                Command = "vim",
                ShortCut = Shortcut.CtrlV,
            });
            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                Type = ContextMenuItemType.Dirtory,
                Name = "CMD",
                Command = "CMD",
                ShortCut = Shortcut.CtrlV,
            });
            this.ContextMenuItems = JsonConvert.DeserializeObject<List<ContextMenuItem>>(File.ReadAllText("cmd.json"));
        }
        public class ContextMenuItem
        {
            public string Name { get; set; }
            public Shortcut ShortCut { get; set; }
            public string Command { get; set; }
            public ContextMenuItemType Type { get; set; }
        }
        [Flags]
        public enum ContextMenuItemType
        {
            None,
            File,
            Files,
            Dirtory,
            Multiple,
        }
        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            this.listView1.ContextMenu.MenuItems.Clear();

            if (this.listView1.SelectedItems.Count == 0)
            {
                var path = this.treeView1.SelectedNode.Tag.ToString();
                foreach (var item in this.ContextMenuItems.Where(m => m.Type.HasFlag(ContextMenuItemType.Dirtory)))
                {
                    var menu = new MenuItem(item.Name, this.ContextMenuClick, item.ShortCut);

                    ProcessStartInfo startInfo = new ProcessStartInfo(item.Command);
                    startInfo.WorkingDirectory = path;
                    startInfo.Arguments = path;
                    menu.Tag = startInfo;
                    menu.ShowShortcut = true;
                    this.listView1.ContextMenu.MenuItems.Add(menu);
                }
            }
            else if (this.listView1.SelectedItems.Count == 1)
            {
                var path = this.listView1.SelectedItems[0].Tag.ToString();
                if (File.Exists(path))
                {
                    foreach (var item in this.ContextMenuItems.Where(m => m.Type.HasFlag(ContextMenuItemType.File)))
                    {
                        var menu = new MenuItem(item.Name, this.ContextMenuClick, item.ShortCut);
                        ProcessStartInfo startInfo = new ProcessStartInfo(item.Command);
                        startInfo.WorkingDirectory = new FileInfo(path).Directory.FullName;
                        startInfo.Arguments = path;
                        menu.Tag = startInfo;
                        menu.ShowShortcut = true;
                        this.listView1.ContextMenu.MenuItems.Add(menu);
                    }
                }
                else
                {
                    foreach (var item in this.ContextMenuItems.Where(m => m.Type.HasFlag(ContextMenuItemType.Dirtory)))
                    {
                        var menu = new MenuItem(item.Name, this.ContextMenuClick, item.ShortCut);

                        ProcessStartInfo startInfo = new ProcessStartInfo(item.Command);
                        startInfo.WorkingDirectory = path;
                        startInfo.Arguments = path;
                        menu.Tag = startInfo;
                        menu.ShowShortcut = true;
                        this.listView1.ContextMenu.MenuItems.Add(menu);
                    }
                }
            }
            else
            {
                List<string> args = new List<string>();
                foreach (ListViewItem item in this.listView1.SelectedItems)
                {
                    var pathString = item.Tag.ToString();
                    if (!string.IsNullOrEmpty(pathString))
                    {
                        args.Add(pathString);
                    }
                }
                var arguments = string.Join(" ", args);
                var path = this.treeView1.SelectedNode.Tag.ToString();

                foreach (var item in this.ContextMenuItems.Where(m => m.Type.HasFlag(ContextMenuItemType.Multiple)))
                {
                    var menu = new MenuItem(item.Name, this.ContextMenuClick, item.ShortCut);
                    ProcessStartInfo startInfo = new ProcessStartInfo(item.Command);
                    startInfo.Arguments = arguments;
                    startInfo.WorkingDirectory = path;
                    menu.Tag = startInfo;
                    menu.ShowShortcut = true;
                    this.listView1.ContextMenu.MenuItems.Add(menu);
                }
            }
        }

        private void ContextMenuClick(object sender, EventArgs e)
        {
            var menu = (MenuItem)sender;
            if (menu == null)
            {
                return;
            }
            var startInfo = (ProcessStartInfo)menu.Tag;
            if (startInfo == null)
            {
                return;
            }
            var process = Process.Start(startInfo);

        }
        private void PopulateTreeView()
        {
            foreach (var item in DriveInfo.GetDrives())
            {
                if (item.IsReady)
                {
                    var node = new TreeNode(string.Format("{0} ({1})", item.VolumeLabel == string.Empty ? "Local Disk" : item.VolumeLabel, item.Name.Substring(0, 2)));
                    var key = "Direve:" + item.Name[0];
                    if (!imageList1.Images.ContainsKey(key))
                    {
                        this.imageList1.Images.Add(key, SystemIcon.GetDriverIcon(item.Name[0], true));
                    }

                    node.ImageKey = key;
                    node.SelectedImageKey = key;
                    node.Tag = item.RootDirectory.FullName;
                    this.treeView1.Nodes.Add(node);
                }

            }
            //var shellItem = new ShellItem();
            //foreach (ShellItem item in shellItem.GetSubFolders())
            //{
            //    rootNode = new TreeNode(item.DisplayName);
            //    rootNode.ImageIndex = item.IconIndex;
            //    rootNode.SelectedImageIndex = item.IconIndex;
            //    rootNode.Tag = item.Path;
            //    treeView1.Nodes.Add(rootNode);
            //}
        }
        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            var dirPath = newSelected.Tag.ToString();
            if (string.IsNullOrEmpty(dirPath))
            {
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            if (!dir.Exists)
            {
                return;
            }
            try
            {
                LoadTreeDir(newSelected, dir);
                loadListDir(dir);

                foreach (FileInfo file in dir.GetFiles())
                {
                    var key = file.Extension.ToLower();
                    if (key == ".exe")
                    {
                        key = file.Name;
                    }
                    if (!imageList1.Images.ContainsKey(key))
                    {
                        this.imageList1.Images.Add(key, SystemIcon.GetFileIcon(file.FullName, true));
                    }
                    var item = new ListViewItem(file.Name, key);
                    item.Tag = file.FullName;
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "File"));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString()));
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        void LoadTreeDir(TreeNode newSelected, DirectoryInfo dir)
        {
            var key = "folder";
            if (newSelected.Nodes.Count == 0)
            {
                foreach (var subDir in dir.GetDirectories().Where(m => !m.Attributes.HasFlag(FileAttributes.Hidden)))
                {
                    var node = new TreeNode(subDir.Name);

                    if (!imageList1.Images.ContainsKey(key))
                    {
                        this.imageList1.Images.Add(key, SystemIcon.GetFolderIcon(true));
                    }
                    node.ImageKey = key;
                    node.SelectedImageKey = key;
                    node.Tag = subDir.FullName;
                    newSelected.Nodes.Add(node);
                }
            }

        }
        void loadListDir(DirectoryInfo dir)
        {
            var key = "folder";
            listView1.Items.Clear();
            foreach (var subDir in dir.GetDirectories().Where(m => !m.Attributes.HasFlag(FileAttributes.Hidden)))
            {

                var item = new ListViewItem(subDir.Name, key);
                item.Tag = subDir.FullName;
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "Directory"));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, subDir.LastAccessTime.ToShortDateString()));
                listView1.Items.Add(item);
            }
        }
    }
}