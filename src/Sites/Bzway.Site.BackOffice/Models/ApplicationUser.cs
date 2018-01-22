using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Sites.BackOffice.Models
{

    public class SecondPrice 
    {
        #region ctor
        public string Code { get; set; }
        public string Name { get; set; }
        public string Open { get; set; }
        public string Last { get; set; }
        public string Current { get; set; }
        public string Max { get; set; }
        public string Min { get; set; }
        public string Buy { get; set; }
        public string Sell { get; set; }
        public string Turnover { get; set; }
        public string Amount { get; set; }
        public string Buy1 { get; set; }
        public string BuyPrice1 { get; set; }
        public string Buy2 { get; set; }
        public string BuyPrice2 { get; set; }
        public string Buy3 { get; set; }
        public string BuyPrice3 { get; set; }
        public string Buy4 { get; set; }
        public string BuyPrice4 { get; set; }
        public string Buy5 { get; set; }
        public string BuyPrice5 { get; set; }
        public string Sell1 { get; set; }
        public string SellPrice1 { get; set; }
        public string Sell2 { get; set; }
        public string SellPrice2 { get; set; }
        public string Sell3 { get; set; }
        public string SellPrice3 { get; set; }
        public string Sell4 { get; set; }
        public string SellPrice4 { get; set; }
        public string Sell5 { get; set; }
        public string SellPrice5 { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string MillSecond { get; set; }
        public string Remark { get; set; }

        private static readonly string[] feilds = new string[]{
            "Code",
            "Name",
            "Open",
            "Last",
            "Current",
            "Max",
            "Min",
            "Buy",
            "Sell",
            "Turnover",
            "Amount",
            "Buy1",
            "BuyPrice1",
            "Buy2",
            "BuyPrice2",
            "Buy3",
            "BuyPrice3",
            "Buy4",
            "BuyPrice4",
            "Buy5",
            "BuyPrice5",
            "Sell1",
            "SellPrice1",
            "Sell2",
            "SellPrice2",
            "Sell3",
            "SellPrice3",
            "Sell4",
            "SellPrice4",
            "Sell5",
            "SellPrice5",
            "Date",
            "Time",
            "MillSecond",
            "Remark"
            };
        SecondPrice() { }
        #endregion
        public static SecondPrice NewStockPrice(string input)
        {
            StringBuilder temp = new StringBuilder();
            var values = input.Split('\"', ',');
            temp.Append("{");
            temp.Append(string.Format("\"{0}\":\"{1}\",", feilds[0], values[0].Replace("var hq_str_", "").Replace("=", "")));
            for (int i = 1; i < values.Length - 1; i++)
            {
                temp.Append(string.Format("\"{0}\":\"{1}\",", feilds[i], values[i].Trim()));
            }
            temp.Append("\"Remark\":\"\"}");
            return JsonConvert.DeserializeObject<SecondPrice>(temp.ToString());
        }
    }
     
    public class MyTest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
