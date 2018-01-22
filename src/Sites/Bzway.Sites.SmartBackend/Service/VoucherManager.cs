using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Bzway.Database.Core;

namespace Bzway.Sites.SmartBackend.Service
{
    public class Voucher : EntityBase
    {
        public string Code { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public string Remark { get; set; }
    }
    class ShortUrl
    {
        public string error_code { get; set; }
        public string url { get; set; }
    }
    public class VoucherManager
    {
        const string postUrl = "http://duanlianjie.com/actions/ajax/shorten.ajax.action.php";
        public Voucher GenerateVoucher(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<String, String>("url", url));
                    var jsonString = client.PostAsync(postUrl, new FormUrlEncodedContent(paramList)).Result.Content.ReadAsStringAsync().Result;
                    var jsonResult = JsonConvert.DeserializeObject<ShortUrl>(jsonString);
                    if (jsonResult == null)
                    {
                        return null;
                    }
                    var shortUrl = jsonResult.url;
                    if (string.IsNullOrEmpty(shortUrl))
                    {
                        return null;
                    }
                    var result = new Voucher()
                    {
                        Code = shortUrl.Remove(0, shortUrl.LastIndexOf('/') + 1),
                        OriginalUrl = url,
                        ShortUrl = shortUrl,
                        Remark = string.Empty,
                    };

                    return result;
                }
            }
            catch (Exception ex)
            {
                var mess = ex.Message;
            }
            return null;
        }
    }
}