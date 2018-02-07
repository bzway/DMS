using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Bzway.Sites.OpenApi.Models
{
    public class LoginRequestModel
    {

        public string GrantType { get; set; }
        [Required]
        public string AppId { get; set; }
        public string ValidateCode { get; set; }
        [Required]
        public string SecretKey { get; set; }
        public string Language { get; set; }

    }

    public class AuthorizationModel
    {
        public string ResponseType { get; set; }
        public string CallBack { get; set; }
        public string State { get; set; }
        public string AppId { get; set; }
        public string Scope { get; set; }
    }

    public class FromOrBodyModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }
            HttpRequest request = bindingContext.HttpContext.Request;
            object obj = new object();
            if (request.HasFormContentType)
            {
                var formCollection = await request.ReadFormAsync(default(CancellationToken));
                List<string> list = new List<string>();
                foreach (var item in formCollection)
                {
                    list.Add($"\"{item.Key}\":\"{item.Value}\"");
                }
                obj = JsonConvert.DeserializeObject("{" + string.Join(',', list) + "}", bindingContext.ModelType);
            }
            else
            {
                var length = (int)bindingContext.HttpContext.Request.ContentLength;
                byte[] buffer = new byte[length];

                bindingContext.HttpContext.Request.Body.Read(buffer, 0, buffer.Length);
                var data = Encoding.UTF8.GetString(buffer);
                if (!string.IsNullOrEmpty(data))
                {
                    obj = JsonConvert.DeserializeObject(data, bindingContext.ModelType);
                }
            }
            bindingContext.Result = ModelBindingResult.Success(obj);

        }
    }
}