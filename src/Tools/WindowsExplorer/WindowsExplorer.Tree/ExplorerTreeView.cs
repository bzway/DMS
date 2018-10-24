using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ExplorerTreeView
{
    public partial class ExplorerTreeView : UserControl
    {
        public ExplorerTreeView()
        {
            InitializeComponent();

            // Set the TreeView image list to the system image list.
            SystemImageList.SetTVImageList(treeWnd.Handle);
            LoadRootNodes();
        }

        /// <summary>
        /// Loads the root TreeView nodes.
        /// </summary>
        private void LoadRootNodes()
        {
            // Create the root shell item.
            ShellItem m_shDesktop = new ShellItem();

            // Create the root node.
            TreeNode tvwRoot = new TreeNode();
         
            tvwRoot.Text = m_shDesktop.DisplayName;
            tvwRoot.ImageIndex = m_shDesktop.IconIndex;
            tvwRoot.SelectedImageIndex = m_shDesktop.IconIndex;
            tvwRoot.Tag = m_shDesktop;

            // Now we need to add any children to the root node.
           
            foreach (ShellItem shChild in m_shDesktop.GetSubFolders())
            {
                TreeNode tvwChild = new TreeNode();
                tvwChild.Text = shChild.DisplayName;
                tvwChild.ImageIndex = shChild.IconIndex;
                tvwChild.SelectedImageIndex = shChild.IconIndex;
                tvwChild.Tag = shChild;

                // If this is a folder item and has children then add a place holder node.
                if (shChild.IsFolder && shChild.HasSubFolder)
                    tvwChild.Nodes.Add("PH");
                tvwRoot.Nodes.Add(tvwChild);
            }

            // Add the root node to the tree.
            treeWnd.Nodes.Clear();
            treeWnd.Nodes.Add(tvwRoot);
            treeWnd.NodeMouseClick += TreeWnd_NodeMouseClick;
            tvwRoot.Expand();
        }
        public event TreeNodeMouseClickEventHandler NodeMouseClick;
        private void TreeWnd_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (this.NodeMouseClick!=null)
            {
                this.NodeMouseClick(sender, e);
            }
        }
    }
}
