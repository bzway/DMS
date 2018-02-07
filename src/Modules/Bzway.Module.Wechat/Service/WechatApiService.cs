using System;
using Bzway.Module.Wechat.Entity;
using Bzway.Data.Core;
using Microsoft.Extensions.Logging;
using Bzway.Framework.Application;
using Newtonsoft.Json;
using System.Net.Http;
using Bzway.Module.Wechat.Model;
using System.IO;
using System.Threading.Tasks;
using Bzway.Common.Utility;
using System.Text;
using Bzway.Common.Share;
using Bzway.Module.Wechat.Interface;
using System.Security.Principal;
using Autofac;
namespace Bzway.Module.Wechat.Service
{
    public class WechatApiService : BaseService<WechatService>, IWechatApiService
    {
        #region ctor
        private static HttpClient webClient = new HttpClient();
        public WechatApiService(ILoggerFactory loggerFactory, ITenant tenant, IPrincipal user) : base(loggerFactory, tenant, user) { }

        private string WebGet(string url, string data)
        {
            try
            {
                using (var content = new StringContent(data, Encoding.UTF8))
                {
                    var result = webClient.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.uploadImg{0}", ex);
                return null;
            }
        }
        public string CurrentWechatId
        {
            get
            {
                return this.tenant.Context.Request.Query["Id"];
            }
        }
        private string GetAccessToken()
        {
            try
            {
                var cache = CacheManager.Default.RedisCacheProvider;
                var accessToken = cache.Get<string>("Wechat.AccessToken." + this.CurrentWechatId, () =>
                {
                    var currentWechatOffianalAccount = cache.Get<WechatOfficialAccount>("Wechat.OfficialAccount" + this.CurrentWechatId, () =>
                    {
                        return this.db.Entity<WechatOfficialAccount>().Query().Where(m => m.Id, this.CurrentWechatId, CompareType.Equal).First();
                    });
                    if (currentWechatOffianalAccount == null)
                    {
                        return string.Empty;
                    }
                    var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", currentWechatOffianalAccount.AppID, currentWechatOffianalAccount.AppSecret);
                    var result = webClient.GetStringAsync(url).Result;
                    if (string.IsNullOrEmpty(result))
                    {
                        return string.Empty;
                    }
                    var response = JsonConvert.DeserializeObject<wechatGetUserAccessTokenResponseModel>(result);
                    if (response == null || response.HasError)
                    {
                        this.logger.LogError(response.errmsg);
                        return string.Empty;
                    }
                    return response.accessToken;
                }, 60 * 60 * 2);
                return accessToken;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetAccessToken:{0}", ex);
                return null;
            }

        }
        #endregion

        #region 自定义菜单
        public bool CreateMenu(WechatButtonMenu menu)
        {
            try
            {
                var url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + this.GetAccessToken();
                var request = menu.ToString();
                string result = this.WebGet(url, request);
                var respone = JsonConvert.DeserializeObject<WechatCreateMenuResponseModel>(result);
                return respone.HasError;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.CreateMenu{0}", ex);
                return false;
            }
        }

        public WechatBaseResponseModel GetMenu()
        {
            try
            {
                var url = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + this.GetAccessToken();


                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(url, ""));


                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetMenu{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel DeleteMenu()
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "deleteMenu", data = "" };

                var state = Guid.NewGuid().ToString("N");

                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));
                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.DeleteMenu{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel AddConditionalMenu(WechatCustomButtonMenu menu)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "addConditionalMenu", data = menu.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));
                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.AddConditionalMenu{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel DeleteConditionalMenu(string menuId)
        {
            try
            {
                string data = "{\"menuid\":\"" + menuId + "\"}";
                var request = new WechatPostRequest() { functionName = "deleteConditionalMenu", data = data };

                var state = Guid.NewGuid().ToString("N");

                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));
                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.DeleteConditionalMenu{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel GetCurrentSelfMenu()
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "getCurrentSelfMenu", data = "" };

                var state = Guid.NewGuid().ToString("N");

                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));
                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetCurrentSelfMenu{0}", ex);
                return null;
            }
        }

        #endregion

        #region 消息管理

        public WechatGetKFListResponseModel GetKFList()
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "getKFList", data = "" };
                var state = Guid.NewGuid().ToString("N");
                var model = JsonConvert.DeserializeObject<WechatGetKFListResponseModel>(this.WebGet(state, request.ToString()));
                return model;

            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetKFList{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel CustomSend(CustomSendModel custom)
        {
            try
            {

                var request = new WechatPostRequest() { functionName = "customSend", data = custom.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.CustomSend{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel SendMsgByUserGroup(MassContentByGroupidModel byGroup)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "sendMsgByUserGroup", data = byGroup.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.SendMsgByUserGroup{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel SendMsgByOpenid(MassContentByOpenidModel byOpenId)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "sendMsgByOpenid", data = byOpenId.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.SendMsgByOpenid{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel DeleteGroupMsg(string msgId)
        {
            try
            {
                string data = "{\"msg_id\":\"" + msgId + "\"}";
                var request = new WechatPostRequest() { functionName = "deleteGroupMsg", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.DeleteGroupMsg{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel SendTemplateMsg(TemplateMsgModel template)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "sendTemplate", data = template.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.SendTemplateMsg{0}", ex);
                return null;
            }
        }

        #endregion

        #region 微信网页开发
        public string RedirectToAuthorizeUrl(string redirectUrl, string appId, string state, string scope)
        {
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect", appId, redirectUrl, scope, state);
        }
        public wechatGetUserAccessTokenResponseModel GetUserAccessTokenByCode(string appId, string appSecurityKey, string code)
        {
            try
            {
                var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appId, appSecurityKey, code);
                var respone = JsonConvert.DeserializeObject<wechatGetUserAccessTokenResponseModel>(this.WebGet(url, ""));
                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("GetAccessTokenByCode{0}", ex);
                return null;
            }
        }


        public WeChatSDKModel GetSDKConfig(string url)
        {
            WeChatSDKModel weChatSDKModel = new WeChatSDKModel();
            weChatSDKModel.timeStamp = (int)DateTimeHelper.GetBaseTimeValue(DateTime.Now);

            weChatSDKModel.nonceStr = Guid.NewGuid().ToString("N");
            string jsapi_ticket = this.GetJsApiTicket().ticket;
            string string1 = "jsapi_ticket=" + jsapi_ticket + "&noncestr=" + weChatSDKModel.nonceStr + "&timestamp=" + weChatSDKModel.timeStamp + "&url=" + url;
            weChatSDKModel.signature = Cryptor.EncryptSHA1(string1);
            weChatSDKModel.appId = string.Empty;// AppSettings["appid"];
            return weChatSDKModel;
        }

        public WechatGetJsapiTicketResponseModel GetJsApiTicket()
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "getJsapiTicket", data = "" };

                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetJsapiTicketResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetJsapiTicket{0}", ex);
                return null;
            }
        }

        public WechatGetJsapiTicketResponseModel GetCardApiTicket()
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "getCardapiTicket", data = "" };

                var state = Guid.NewGuid().ToString("N");

                var respone = JsonConvert.DeserializeObject<WechatGetJsapiTicketResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetCardapiTicket{0}", ex);
                return null;
            }
        }

        public WechatGetUserInfoResponseModel GetWebUserInfo(string openId)
        {
            try
            {
                var functionName = "https://api.weixin.qq.com/sns/userinfo?access_token=" + this.GetUserAccessTokenByCode("", "", "") + "&openid=" + openId + "&lang=zh_CN";
                var request = new WechatPostRequest() { functionName = functionName, data = "" };

                var state = Guid.NewGuid().ToString("N");

                var respone = JsonConvert.DeserializeObject<WechatGetUserInfoResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUserinfo{0}", ex);
                return null;
            }
        }

        #endregion

        #region 素材管理

        public WechatBaseResponseModel CreateNewsMaterial(WechatNewsMaterialCreateRequestModel newsMaterial)
        {
            try
            {
                var state = "https://api.weixin.qq.com/cgi-bin/material/add_news?access_token=" + this.GetAccessToken();
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, newsMaterial.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.CreateMaterial{0}", ex);
                return null;
            }
        }

        public WechatGetNewsMaterialResponseModel GetNewsMaterial(string mediaId)
        {
            try
            {
                string data = "{\"media_id\":\"" + mediaId + "\"}";
                var url = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=" + this.GetAccessToken();
                var respone = JsonConvert.DeserializeObject<WechatGetNewsMaterialResponseModel>(this.WebGet(url, data));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetNewsMaterial{0}", ex);
                return null;
            }
        }
        public WechatGetOtherMaterialResponseModel GetOtherMaterial(string mediaId)
        {
            try
            {
                string data = "{\"media_id\":\"" + mediaId + "\"}";
                var url = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=" + this.GetAccessToken();
                var respone = JsonConvert.DeserializeObject<WechatGetOtherMaterialResponseModel>(this.WebGet(url, data));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetOtherMaterial{0}", ex);
                return null;
            }
        }
        public WechatBaseResponseModel DeleteMaterial(string mediaId)
        {
            try
            {
                string data = "{\"media_id\":\"" + mediaId + "\"}";
                var state = "https://api.weixin.qq.com/cgi-bin/material/del_material?access_token=" + this.GetAccessToken();
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, data));
                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.DeleteMaterial{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel UpdateNewsMaterial(WechatNewsMaterialUpdateRequestModel material)
        {
            try
            {
                var url = "https://api.weixin.qq.com/cgi-bin/material/update_news?access_token=" + this.GetAccessToken();
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(url, material.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.UpdateMaterial{0}", ex);
                return null;
            }
        }

        public WechatGetMaterialCountResponseModel GetMaterialCount()
        {
            try
            {
                var url = "https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token=" + this.GetAccessToken();
                var respone = JsonConvert.DeserializeObject<WechatGetMaterialCountResponseModel>(this.WebGet(url, ""));
                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.getMaterialCount{0}", ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">素材的类型，图片（image）、视频（video）、语音 （voice）、图文（news）</param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public WechatGetMaterialListResponseModel GetMaterialList(string type, int offset, int count)
        {
            try
            {
                var url = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=" + this.GetAccessToken();
                var data = new { type = type, offset = offset, count = count };
                var request = JsonConvert.SerializeObject(data);
                var reuqest = this.WebGet(url, request);
                var model = JsonConvert.DeserializeObject<WechatGetMaterialListResponseModel>(reuqest);
                return model;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetMaterialList{0}", ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wechatOpenId">openID or 微信号</param>
        /// <param name="type">mpnews,text,voice,image,mpvideo,wxcard,</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public WechatBaseResponseModel PreviewNewsMaterial(string wechatOpenId, string type, string content)
        {
            try
            {
                var request = "{\"towxname\":\"" + wechatOpenId + "\", \"touser\":\"" + wechatOpenId + "\",\"mpnews\":{\"media_id\":\"" + content + "\"},\"msgtype\":\"mpnews\"}";

                var url = "https://api.weixin.qq.com/cgi-bin/message/mass/preview?access_token=" + this.GetAccessToken();

                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(url, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetOnLineKFList{0}", ex);
                return null;
            }
        }
        /// <summary>
        /// 上传图文消息内的图片获取URL
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string UploadNewsMaterialImage(string filePath)
        {
            try
            {
                string url = "https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token=" + this.GetAccessToken();
                using (var formDataContent = new MultipartFormDataContent())
                {
                    formDataContent.Add(new ByteArrayContent(File.ReadAllBytes(filePath)), "files", filePath);

                    var result = webClient.PostAsync(url, formDataContent).Result.Content.ReadAsByteArrayAsync().Result;

                    var response = JsonConvert.DeserializeObject<WechatUploadNewsMaterialImageResponseModel>(Encoding.UTF8.GetString(result));
                    return response.url;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.uploadImg{0}", ex);
                return null;
            }
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <returns></returns>
        public WechatCreateOtherMaterialResponseModel CreateOtherMaterial(string filePath, string type, string title, string introduction)
        {
            try
            {
                var accessToken = GetAccessToken();
                string url = "https://api.weixin.qq.com/cgi-bin/material/add_material?access_token=" + accessToken + "&type=" + type;
                using (var formDataContent = new MultipartFormDataContent())
                {
                    formDataContent.Add(new ByteArrayContent(File.ReadAllBytes(filePath)), "media", "test");
                    if (!string.IsNullOrEmpty(title))
                    {
                        var data = "{\"title\":\"" + title + "\",\"introduction\":\"" + introduction + "\"}";
                        formDataContent.Add(new System.Net.Http.StringContent(data), "description");
                    }
                    HttpResponseMessage response = webClient.PostAsync(url, formDataContent).Result;
                    var result = Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result);

                    return JsonConvert.DeserializeObject<WechatCreateOtherMaterialResponseModel>(result);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.Upload{0}", ex);
                return null;
            }
        }


        /// <summary>
        /// 媒体文件在后台保存时间为3天，即3天后media_id失效。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <returns></returns>
        public string UploadTemplateMaterial(string filePath, string type)
        {
            try
            {
                var accessToken = GetAccessToken();
                string url = "https://api.weixin.qq.com/cgi-bin/media/upload?access_token=" + accessToken + "&type=" + type;
                using (var formDataContent = new MultipartFormDataContent())
                {
                    formDataContent.Add(new ByteArrayContent(File.ReadAllBytes(filePath)), "files", "test");
                    HttpResponseMessage response = webClient.PostAsync(url, formDataContent).Result;
                    var result = response.Content.ReadAsByteArrayAsync().Result;
                    return Convert.ToBase64String(result);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.Upload{0}", ex);
                return null;
            }
        }

        #endregion

        #region 用户管理

        public string CreateGroup(string name)
        {
            try
            {
                var url = "https://api.weixin.qq.com/cgi-bin/tags/create?access_token=" + this.GetAccessToken();
                var data = new { tag = new { name = name } };
                var request = JsonConvert.SerializeObject(data);
                var respone = JsonConvert.DeserializeObject<WechatCreateTagResponseModel>(this.WebGet(url, request));
                return respone.tag.id;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.CreateGroup{0}", ex);
                return null;
            }
        }

        public WechatGetTagsResponseModel GetAllGroups()
        {
            try
            {
                var request = "";

                var state = "https://api.weixin.qq.com/cgi-bin/tags/get?access_token=" + this.GetAccessToken();

                var respone = JsonConvert.DeserializeObject<WechatGetTagsResponseModel>(this.WebGet(state, request.ToString()));
                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetAllGroups{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel GetMemberGroup(string openId)
        {
            try
            {
                string data = "{\"openid\":\"" + openId + "\"}";
                var request = new WechatPostRequest() { functionName = "getMemberGroup", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetMemberGroup{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel UpdateGroup(int groupId, string groupName)
        {
            try
            {
                string data = "{\"group\":{\"id\":" + groupId + ",\"name\":\"" + groupName + "\"}}";
                var request = new WechatPostRequest() { functionName = "updateGroup", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.UpdateGroup{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel MoveMemberGroup(string openId, int toGroupId)
        {
            try
            {
                string data = "{\"openid\":\"" + openId + "\",\"to_groupid\":" + toGroupId + "}";
                var request = new WechatPostRequest() { functionName = "moveMemberGroup", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.MoveMemberGroup{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel BatchMoveMemberGroup(BatchMoveMemberGroup batchGroup)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "batchMoveMemberGroup", data = batchGroup.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.BatchMoveMemberGroup{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel DeleteGroup(int groupId)
        {
            try
            {
                string data = "{\"group\":{\"id\":" + groupId + "}}";
                var request = new WechatPostRequest() { functionName = "deleteGroup", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.DeleteGroup{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel UpdateRemark(string openId, string remark)
        {
            try
            {
                string data = "{\"openid\":\"" + openId + "\",\"remark\":\"" + remark + "\"}";
                var request = new WechatPostRequest() { functionName = "updateRemark", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.UpdateRemark{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel GetDetailUserInfoList(BatchGetUserInfo batchUserInfo)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "getDetailUserInfoList", data = batchUserInfo.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetDetailUserInfoList{0}", ex);
                return null;
            }
        }

        public WeChatGetUserListResponseModel GetUserInfoList(string nextOpenId)
        {
            try
            {
                var functionName = "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid=" + nextOpenId;
                var request = new WechatPostRequest() { functionName = functionName, data = "" };

                var state = Guid.NewGuid().ToString("N");

                var respone = JsonConvert.DeserializeObject<WeChatGetUserListResponseModel>(this.WebGet(state, request.ToString()));


                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUserInfoList{0}", ex);
                return null;
            }
        }

        public WechatGetUserInfoResponseModel GetDetailUserInfo(string openId, string lang)
        {
            try
            {
                var functionName = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid=" + openId + "&lang=" + lang;
                var request = new WechatPostRequest() { functionName = functionName, data = "" };

                var state = Guid.NewGuid().ToString("N");

                var respone = JsonConvert.DeserializeObject<WechatGetUserInfoResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetDetailUserInfo{0}", ex);
                return null;
            }
        }

        #endregion

        #region 帐号管理

        public WechatCreateQRCodeTicketResponseModel CreateQRCodeTicket(ActionModel qrCode)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "createQRCodeTicket", data = qrCode.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatCreateQRCodeTicketResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.CreateQRCodeTicket{0}", ex);
                return null;
            }
        }
        public class WechatCreateFShortUrlRequestModel
        {
            public string action { get; set; }
            public string long_url { get; set; }
            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public WechatCreateFShortUrlResponseModel GetShortUrl(string long_url)
        {
            try
            {
                var data = new { action = "long2short", long_url = "" };
                var request = JsonConvert.SerializeObject(data);
                var state = "https://api.weixin.qq.com/cgi-bin/shorturl?access_token=" + this.GetAccessToken();
                var respone = JsonConvert.DeserializeObject<WechatCreateFShortUrlResponseModel>(this.WebGet(state, request.ToString()));
                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetShortUrl{0}", ex);
                return null;
            }
        }

        #endregion

        #region 数据统计

        public WechatGetUserCumulateResponseModel GetUserSummary(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUserSummary", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetUserCumulateResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUserSummary{0}", ex);
                return null;
            }
        }

        public WechatGetUserCumulateResponseModel GetUserCumulate(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUserCumulate", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetUserCumulateResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUserCumulate{0}", ex);
                return null;
            }
        }

        public WechatGetArticleSummaryResponseModel GetArticleSummary(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getArticleSummary", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetArticleSummaryResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetArticleSummary{0}", ex);
                return null;
            }
        }

        public WechatGetArticleTotalResponseModel GetArticleTotal(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getArticleTotal", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetArticleTotalResponseModel>(this.WebGet(state, request.ToString()));
                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetArticleTotal{0}", ex);
                return null;
            }
        }

        public WechatGetArticleReadResponseModel GetUserRead(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUserRead", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetArticleReadResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUserRead{0}", ex);
                return null;
            }
        }

        public WechatGetArticleReadByHourResponseModel GetUserReadByHour(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUserReadHour", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetArticleReadByHourResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUserReadByHour{0}", ex);
                return null;
            }
        }

        public WechatGetArticleShareResponseModel GetUserShare(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUserShare", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetArticleShareResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUserShare{0}", ex);
                return null;
            }
        }

        public WechatGetArticleShareByHourResponseModel GetUserShareByHour(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUserShareHour", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetArticleShareByHourResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUserShareByHour{0}", ex);
                return null;
            }
        }

        public WechatGetUpStreamMsgResponseModel GetUpStreamMsg(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUpStreamMsg", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetUpStreamMsgResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUpStreamMsg{0}", ex);
                return null;
            }
        }

        public WechatGetUpStreamMsgResponseModel GetUpStreamMsgByHour(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUpStreamMsgByHour", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetUpStreamMsgResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUpStreamMsgByHour{0}", ex);
                return null;
            }
        }

        public WechatGetUpStreamMsgResponseModel GetUpStreamMsgByWeek(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUpStreamMsgByWeek", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetUpStreamMsgResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUpStreamMsgByWeek{0}", ex);
                return null;
            }
        }

        public WechatGetUpStreamMsgResponseModel GetUpStreamMsgByMonth(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUpStreamMsgByMonth", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetUpStreamMsgResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUpStreamMsgByMonth{0}", ex);
                return null;
            }
        }

        public WechatGetUpStreamMsgResponseModel GetUpStreamMsgDist(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUpStreamMsgDist", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetUpStreamMsgResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUpStreamMsgDist{0}", ex);
                return null;
            }
        }

        public WechatGetUpStreamMsgResponseModel GetUpStreamMsgDistByWeek(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUpStreamMsgDistByWeek", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetUpStreamMsgResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUpStreamMsgDistByWeek{0}", ex);
                return null;
            }
        }

        public WechatGetUpStreamMsgResponseModel GetUpStreamMsgDistByMonth(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getUpStreamMsgDistByMonth", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetUpStreamMsgResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetUpStreamMsgDistByMonth{0}", ex);
                return null;
            }
        }

        public WechatGetInterfaceSummaryResponseModel GetInterfaceSummary(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getInterfaceSummary", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetInterfaceSummaryResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetInterfaceSummary{0}", ex);
                return null;
            }

        }

        public WechatGetInterfaceSummaryResponseModel GetInterfaceSummaryByHour(string start, string end)
        {
            try
            {
                string data = "{\"begin_date\": \"" + start + "\", \"end_date\": \"" + end + "\"}";
                var request = new WechatPostRequest() { functionName = "getInterfaceSummaryByHour", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetInterfaceSummaryResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetInterfaceSummaryByHour{0}", ex);
                return null;
            }

        }

        #endregion

        #region 微信卡券

        public WechatBaseResponseModel CreateCards(string data)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "createCards", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.CreateCards{0}", ex);
                return null;
            }
        }

        public WechatGetQRCodeCardResponseModel GetOneCardQrCode(string action_name)
        {
            try
            {

                var request = new WechatPostRequest() { functionName = "getCardQrCode", data = action_name.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetQRCodeCardResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetOneCardQrCode{0}", ex);
                return null;
            }
        }

        public WechatGetQRCodeCardResponseModel GetMultipleCardQrCode(MultipleQrCodeCard cardList)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "getCardQrCode", data = cardList.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatGetQRCodeCardResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetMultipleCardQrCode{0}", ex);
                return null;
            }
        }

        public WeChatMemberCard GetCardStatus(string cardId)
        {
            try
            {
                string data = "{\"card_id\": \"" + cardId + "\"}";
                var request = new WechatPostRequest() { functionName = "getCardStatus", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WeChatMemberCard>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetCardStatus{0}", ex);
                return null;
            }

        }

        public WechatBaseResponseModel UpdateCard(string data)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "updateCard", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.UpdateCard{0}", ex);
                return null;
            }

        }

        public WechatBaseResponseModel ModifyStock(modifystock stock)
        {
            try
            {
                var accessToken = GetAccessToken();
                var url = "https://api.weixin.qq.com/card/modifystock?access_token=" + accessToken;
                var request = new WechatPostRequest() { functionName = url, data = stock.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.ModifyStock{0}", ex);
                return null;
            }

        }

        public WechatBaseResponseModel DeleteCard(string cardId)
        {
            try
            {
                string data = "{\"card_id\": \"" + cardId + "\"}";
                var request = new WechatPostRequest() { functionName = "deleteCard", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.DeleteCard{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel GrantCard(string cardId)
        {
            try
            {
                string data = "{\"card_id\": \"" + cardId + "\"}";
                var request = new WechatPostRequest() { functionName = "grantCard", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.DeleteCard{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel ActivateCard(string data)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "activateCard", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.ActivateCard{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel ConsumeCard(string cardId, string cardCode)
        {

            try
            {
                string data = "{ \"code\": \"" + cardCode + "\", \"card_id\": \"" + cardId + "\"}";

                var request = new WechatPostRequest() { functionName = "https://api.weixin.qq.com/card/code/consume?access_token={0}", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.DeleteCard{0}", ex);
                return null;
            }

        }

        public WechatBaseResponseModel DecryptCard(string encrypt_code)
        {

            try
            {
                var accessToken = GetAccessToken();
                string data = "{ \"encrypt_code\": \"" + encrypt_code + "\"}";

                var request = new WechatPostRequest() { functionName = "https://api.weixin.qq.com/card/code/decrypt?access_token={0}", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.DecryptCard{0}", ex);
                return null;
            }

        }

        #endregion

        #region 微信门店

        public WechatBaseResponseModel CreatePoi(PoiRequestModel model)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "createPoi", data = model.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.CreatePoi{0}", ex);
                return null;
            }
        }
        public PoiResponseModel GetPoi(string poi_id)
        {
            try
            {
                string data = "{\"poi_id\":\"" + poi_id + "\"}";
                var request = new WechatPostRequest() { functionName = "getPoi", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<PoiResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetPoi{0}", ex);
                return null;
            }
        }

        public PoiListResponseModel GetPoiList(int begin, int limit = 20)
        {
            try
            {
                string data = "{\"begin\": \"" + begin + "\", \"limit\": \"" + limit + "\"}";
                var request = new WechatPostRequest() { functionName = "getPoiList", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<PoiListResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetPoiList{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel UpdatePoi(UpdatePoiResponseModel model)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "updatePoi", data = model.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.UpdatePoi{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel DeletePoi(string poi_id)
        {
            try
            {
                string data = "{\"poi_id\":\"" + poi_id + "\"}";
                var request = new WechatPostRequest() { functionName = "deletePoi", data = data };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.DeletePoi{0}", ex);
                return null;
            }
        }

        public PoiCategoryResponseModel GetWxCategory()
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "getWxCategory", data = "" };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<PoiCategoryResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetWxCategory{0}", ex);
                return null;
            }
        }

        #endregion

        #region 微信多客服功能

        public OnLineKFModel GetOnLineKFList()
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "getOnLineKFList", data = "" };

                var state = Guid.NewGuid().ToString("N");

                var respone = JsonConvert.DeserializeObject<OnLineKFModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetOnLineKFList{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel CreateCustom(CustomModel custom)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "createCustom", data = custom.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.CreateCustom{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel InvitationCustom(CustomModel custom)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "invitationCustom", data = custom.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.InvitationCustom{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel UploadCustomHeadImg(string filePath, string accountName)
        {
            try
            {
                var accessToken = GetAccessToken();
                string url = string.Format("https://api.weixin.qq.com/customservice/kfaccount/uploadheadimg?access_token={0}&kf_account={1}", accessToken, accountName);
                using (var formDataContent = new MultipartFormDataContent())
                {
                    formDataContent.Add(new ByteArrayContent(File.ReadAllBytes(filePath)), "files", "test");

                    var result = webClient.PostAsync(url, formDataContent).Result.Content.ReadAsByteArrayAsync().Result;
                    var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(Encoding.UTF8.GetString(result));
                    return respone;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.UploadHeadImg{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel UpdateCustom(CustomModel custom)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "updateCustom", data = custom.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {

                this.logger.LogError("WechatServiceHelper.UpdateCustom{0}", ex);
                return null;
            }

        }

        public WechatBaseResponseModel DeleteCustom(CustomModel custom, string AccessToken)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = string.Format("https://api.weixin.qq.com/customservice/kfaccount/del?access_token={0}&kf_account={1}", AccessToken, custom.kf_account) };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.DeleteCustom{0}", ex);
                return null;
            }
        }

        public WechatBaseResponseModel Opendialogue(KFSessionModel session)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "opendialogue", data = session.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.Opendialogue{0}", ex);
                return null;
            }

        }

        public WechatBaseResponseModel Closedialogue(KFSessionModel session)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "closedialogue", data = session.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));
                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.Closedialogue{0}", ex);
                return null;
            }

        }

        public KFSessionResponseModel Getsession(string openId)
        {
            try
            {
                var functionName = "https://api.weixin.qq.com/customservice/kfsession/getsession?access_token={0}&openid=" + openId + "&lang=zh_CN";
                var request = new WechatPostRequest() { functionName = functionName, data = "" };

                var state = Guid.NewGuid().ToString("N");

                var respone = JsonConvert.DeserializeObject<KFSessionResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.Getsession{0}", ex);
                return null;
            }
        }

        public MsgRecordModel GetMsgRecord(MsgRecordPostModel record)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "getMsgRecord", data = record.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<MsgRecordModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.GetMsgRecord{0}", ex);
                return null;
            }

        }

        #endregion

        #region 微信摇一摇周边

        public WechatBaseResponseModel Register(RegisterRequestModel model)
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "register", data = model.ToString() };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<WechatBaseResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.Register{0}", ex);
                return null;
            }
        }

        public AuditStatusResponseModel AuditStatus()
        {
            try
            {
                var request = new WechatPostRequest() { functionName = "auditStatus", data = "" };
                var state = Guid.NewGuid().ToString("N");
                var respone = JsonConvert.DeserializeObject<AuditStatusResponseModel>(this.WebGet(state, request.ToString()));

                return respone;
            }
            catch (Exception ex)
            {
                this.logger.LogError("WechatServiceHelper.AuditStatus{0}", ex);
                return null;
            }
        }

        public WechatGetNewsMaterialResponseModel GetMaterial(string mediaId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 微信连WIFI TODO



        #endregion

        #region 微信扫一扫 TODO



        #endregion


    }
}