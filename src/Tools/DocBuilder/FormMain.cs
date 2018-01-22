using Bzway.Common.Share;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocBuilder
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var input = this.textBox1.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                return;
            }
            foreach (var item in ParserManager.GetLinks(input))
            {
                var parser = ParserManager.GetNewsParser(item);
                if (parser == null)
                {
                    continue;
                }
                this.newList.Add(new NewsModel()
                {
                    Author = parser.Author,
                    Content = parser.Content,
                    ContentSourceUrl = parser.Url,
                    Url = string.Empty,
                    CreatedBy = string.Empty,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedBy = string.Empty,
                    UpdatedOn = DateTime.UtcNow,
                    Digest = parser.Digest,
                    Id = Guid.NewGuid().ToString("N"),
                    IsReleased = false,
                    LastUpdateTime = DateTime.UtcNow,
                    MaterialID = string.Empty,
                    MediaId = string.Empty,
                    OfficialAccount = string.Empty,
                    ShowCoverPicture = false,
                    SortBy = 0,
                    ThumbMediaId = parser.ThumbMedia,
                    Title = parser.Title
                });
            }

        }
        List<NewsModel> newList = new List<NewsModel>();
        private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = this.openFileDialog1.FileName;
                var list = JsonConvert.DeserializeObject<List<NewsModel>>(File.ReadAllText(path));
                if (list == null)
                {
                    list = new List<NewsModel>();
                }
                list.AddRange(this.newList);
                File.WriteAllText(path, JsonConvert.SerializeObject(list));
            }
        }
    }
}