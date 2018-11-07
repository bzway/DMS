using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsExplorer
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        FileManager fileManager = new FileManager();

        private void button1_Click(object sender,EventArgs e)
        {
            fileManager.Add(this.textBox1.Text);
         
        }

        private void button2_Click(object sender,EventArgs e)
        {
            fileManager.Recovery();
        }
    }
}
