using System;
using System.Collections;
using System.Windows.Forms;

namespace ExplorerTreeView
{
    class ExplorerTreeViewWnd : TreeView
    {
        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            // Remove the placeholder node.
            e.Node.Nodes.Clear();

            // We stored the ShellItem object in the node's Tag property - hah!
            ShellItem shNode = (ShellItem)e.Node.Tag;
         
            foreach (ShellItem shChild in shNode.GetSubFolders())
            {
                TreeNode tvwChild = new TreeNode();
                tvwChild.Text = shChild.DisplayName;
                tvwChild.ImageIndex = shChild.IconIndex;
                tvwChild.SelectedImageIndex = shChild.IconIndex;
                tvwChild.Tag = shChild;

                // If this is a folder item and has children then add a place holder node.
                if (shChild.IsFolder && shChild.HasSubFolder)
                    tvwChild.Nodes.Add("PH");
                e.Node.Nodes.Add(tvwChild);
            }
            
            base.OnBeforeExpand(e);
        }
    }
}
