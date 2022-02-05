using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomatedImageImporter
{
    public class Node
    {
        public enum LineStatus
        {
            Processing,
            Complete,
            Failed,
            Invalid,
            Unknown
        }
        private LineStatus status = LineStatus.Processing;
        public LineStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                Update();
            }
        }
        public string Text { get; set; }
        public TreeNode TreeNode { get; set; }
        public Line Line { get; set; }
        private string formatString;
        public static Node ChunkNode(TreeView tree, int number)
        {
            return new Node(tree, null, "Chunk #{1} - {0}", (++number).ToString());
        }
        public static Node LineNode(TreeNode parent, int number, string line)
        {
            return new Node(parent.TreeView, parent, "{0} - Line #{1}", ++number + ": " + line) { Line = new Line(line, number) };
        }
        private Node(TreeView tree, TreeNode parent, string formatString, string text)
        {
            this.formatString = formatString;
            this.Text = text;
            tree.Invoke(new Action(() =>
            {
                if (parent != null)
                    this.TreeNode = parent.Nodes.Insert(0, "");
                else
                    this.TreeNode = tree.Nodes.Insert(0, "");
                this.TreeNode.Expand();
            }));
            Update();
        }

        public void Update()
        {
            TreeNode.TreeView.Invoke(new Action(() =>
            {
                TreeNode.Text = string.Format(formatString, status.ToString().ToUpper(), Text);
                TreeNode.Collapse();
                if (status == LineStatus.Processing)
                    TreeNode.Expand();

            }));
        }
    }
}
