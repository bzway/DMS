using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Bzway.Sites.SmartBackend.Service
{
    public class SendCloudEmail
    {
        String url = "http://api.sendcloud.net/apiv2/mail/send";

        String api_user = "abs51email";
        String api_key = "kSciDADk7E5aMn5Z";
        public void Send(String tos, string subject, string content)
        {

            HttpClient client = null;
            HttpResponseMessage response = null;

            try
            {
                client = new HttpClient();

                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();

                paramList.Add(new KeyValuePair<string, string>("apiUser", api_user));
                paramList.Add(new KeyValuePair<string, string>("apiKey", api_key));
                paramList.Add(new KeyValuePair<string, string>("from", "system@abs51.com"));
                paramList.Add(new KeyValuePair<string, string>("fromName", "绝对无忧"));
                paramList.Add(new KeyValuePair<string, string>("to", tos));
                paramList.Add(new KeyValuePair<string, string>("subject", subject));
                paramList.Add(new KeyValuePair<string, string>("html", content));

                response = client.PostAsync(url, new FormUrlEncodedContent(paramList)).Result;
                String result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("result:{0}", result);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            finally
            {
                if (null != client)
                {
                    client.Dispose();
                }
            }
        }
    }
    public class SendCloudSMS
    {
        String url = "http://www.sendcloud.net/smsapi/send";
        String smsUser = "bzway";
        String smsKey = "Izj6eg6GnFKAgG15z6q1lfL33235h4Ft";
        string templateId = "5061";

        String generate_md5(String str)
        {
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }

        public void Send(string phone, string content)
        {
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<String, String>("smsUser", smsUser));
            paramList.Add(new KeyValuePair<String, String>("templateId", this.templateId));
            paramList.Add(new KeyValuePair<String, String>("phone", phone));
            paramList.Add(new KeyValuePair<String, String>("msgType", "0"));
            paramList.Add(new KeyValuePair<String, String>("vars", JsonConvert.SerializeObject(new { vcode = content })));

            paramList.Sort(delegate (KeyValuePair<String, String> p1, KeyValuePair<String, String> p2) { return p1.Key.CompareTo(p2.Key); });

            var param_str = "";
            foreach (var param in paramList)
            {
                param_str += param.Key.ToString() + "=" + param.Value.ToString() + "&";
            }
            String sign_str = smsKey + "&" + param_str + smsKey;
            String sign = generate_md5(sign_str);
            paramList.Add(new KeyValuePair<String, String>("signature", sign));

            HttpClient client = null;
            HttpResponseMessage response = null;

            try
            {
                client = new HttpClient();
                response = client.PostAsync(url, new FormUrlEncodedContent(paramList)).Result;
                String result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("result:{0}", result);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            finally
            {
                if (null != response)
                {
                    response.Dispose();
                }
                if (null != client)
                {
                    client.Dispose();
                }
            }
        }
    }
}