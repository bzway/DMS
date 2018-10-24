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
        string Current_Url = "https://pan.baidu.com/s/1eSa4WPc";
        private void button1_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            for (int i = 0; i < 10000; i++)
            {
                var html = webClient.DownloadString(string.Format("http://www.ireadweek.com/index.php/bookInfo/{0}.html", i));
                var matches = Regex.Matches(html, "<a(\\s\\S*)href=\\\"(?<href>[^\\\"]*?)\\\".+>(.*?)</a>", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    var href = match.Groups["href"].Value;
                    if (string.IsNullOrEmpty(href))
                    {
                        continue;
                    }
                    if (!href.StartsWith("http"))
                    {
                        continue;
                    }
                    if (list.Contains(href))
                    {
                        continue;
                    }
                    list.Add(href);
                    this.Text = string.Format("{0}/{1}", list.Count, i);
                }
            }
            MessageBox.Show("OK");
            File.WriteAllText("result.txt", string.Join("\r\n", list));
         
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var url = e.Url.ToString();
            if (!url.Contains("&parentPath="))
            {
                HtmlElement ele = webBrowser1.Document.CreateElement("script");
                ele.SetAttribute("type", "text/javascript");
                ele.SetAttribute("text", "window.location = '" + Current_Url + "#list/path=" + "'+ yunData.PATH +'&parentPath=' + yunData.FILEINFO[0].parent_path;");
                webBrowser1.Document.Body.AppendChild(ele);
            }
            else
            {
                //HtmlElement ele = webBrowser1.Document.CreateElement("script");
                //ele.SetAttribute("type", "text/javascript");
                //ele.SetAttribute("text", System.IO.File.ReadAllText("site.js"));
                //webBrowser1.Document.Body.AppendChild(ele);
            }
            this.Text = url;
        }
 

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var item in File.ReadLines("result.txt"))
            {
                Current_Url = item;
                this.webBrowser1.Navigate(Current_Url);
                break; ;
                Thread.Sleep(100000);
            }
  
        }
    }
}
