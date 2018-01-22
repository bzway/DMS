using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Bzway.Module.Wechat.Model
{
    public static class WechatFuntion
    {
        static Dictionary<string, string> dict = null;
        public static Dictionary<string, string> List
        {
            get
            {
                #region init
                if (dict == null)
                {
                    dict = new Dictionary<string, string>();
                    dict.Add("createMenu", "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}");
                    dict.Add("getJsapiTicket", "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi");
                    dict.Add("getCardapiTicket", "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=wx_card");
                    dict.Add("getMenuData", "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}");
                    dict.Add("deleteMenu", "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}");
                    dict.Add("addConditionalMenu", "https://api.weixin.qq.com/cgi-bin/menu/addconditional?access_token={0}");
                    dict.Add("deleteConditionalMenu", "https://api.weixin.qq.com/cgi-bin/menu/delconditional?access_token={0}");
                    dict.Add("getCurrentSelfMenu", "https://api.weixin.qq.com/cgi-bin/get_current_selfmenu_info?access_token={0}");
                    dict.Add("createGroup", "https://api.weixin.qq.com/cgi-bin/groups/create?access_token={0}");
                    dict.Add("getAllGroups", "https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}");
                    dict.Add("getMemberGroup", "https://api.weixin.qq.com/cgi-bin/groups/getid?access_token={0}");
                    dict.Add("updateGroup", "https://api.weixin.qq.com/cgi-bin/groups/update?access_token={0}");
                    dict.Add("moveMemberGroup", "https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token={0}");
                    dict.Add("batchMoveMemberGroup", "https://api.weixin.qq.com/cgi-bin/groups/members/batchupdate?access_token={0}");
                    dict.Add("deleteGroup", "https://api.weixin.qq.com/cgi-bin/groups/delete?access_token={0}");
                    dict.Add("createQRCodeTicket", "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}");
                    dict.Add("showQRCode", "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}");
                    dict.Add("getShortUrl", "https://api.weixin.qq.com/cgi-bin/shorturl?access_token={0}");
                    dict.Add("getDetailUserInfoList", "https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token={0}");
                    dict.Add("updateRemark", "https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token={0}");
                    dict.Add("sendTemplate", "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}");
                    dict.Add("createCards", "https://api.weixin.qq.com/card/create?access_token={0}");
                    dict.Add("getCardStatus", "https://api.weixin.qq.com/card/get?access_token={0}");
                    dict.Add("grantCard", "https://api.weixin.qq.com/card/mpnews/gethtml?access_token={0}");
                    dict.Add("activateCard", "https://api.weixin.qq.com/card/membercard/activate?access_token={0}");
                    dict.Add("updateCard", "https://api.weixin.qq.com/card/update?access_token={0}");
                    dict.Add("modifystock", "https://api.weixin.qq.com/card/modifystock?access_token={0}");
                    dict.Add("deleteCard", "https://api.weixin.qq.com/card/delete?access_token={0}");
                    dict.Add("customSend", "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}");
                    dict.Add("opendialogue", " https://api.weixin.qq.com/customservice/kfsession/create?access_token={0}&lang=zh_CN");
                    dict.Add("closedialogue", "https://api.weixin.qq.com/customservice/kfsession/close?access_token={0}&lang=zh_CN");
                    dict.Add("getMsgRecord", "https://api.weixin.qq.com/customservice/msgrecord/getrecord?access_token={0}");
                    dict.Add("getKFList", "https://api.weixin.qq.com/cgi-bin/customservice/getkflist?access_token={0}");
                    dict.Add("getOnLineKFList", "https://api.weixin.qq.com/cgi-bin/customservice/getonlinekflist?access_token={0}");
                    dict.Add("createCustom", "https://api.weixin.qq.com/customservice/kfaccount/add?access_token={0}");
                    dict.Add("updateCustom", "https://api.weixin.qq.com/customservice/kfaccount/update?access_token={0}");
                    dict.Add("deleteCustom", "https://api.weixin.qq.com/customservice/kfaccount/del?access_token={0}&kf_account={1}");
                    dict.Add("sendMsgByUserGroup", "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}");
                    dict.Add("sendMsgByOpenid", "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}");
                    dict.Add("deleteGroupMsg", "https://api.weixin.qq.com/cgi-bin/message/mass/delete?access_token={0}");
                    dict.Add("getUserSummary", "https://api.weixin.qq.com/datacube/getusersummary?access_token={0}");
                    dict.Add("getUserCumulate", "https://api.weixin.qq.com/datacube/getusercumulate?access_token={0}");
                    dict.Add("getArticleSummary", "https://api.weixin.qq.com/datacube/getarticlesummary?access_token={0}");
                    dict.Add("getArticleTotal", "https://api.weixin.qq.com/datacube/getarticletotal?access_token={0}");
                    dict.Add("getUserRead", "https://api.weixin.qq.com/datacube/getuserread?access_token={0}");
                    dict.Add("getUserReadHour", "https://api.weixin.qq.com/datacube/getuserreadhour?access_token={0}");
                    dict.Add("getUserShare", "https://api.weixin.qq.com/datacube/getusershare?access_token={0}");
                    dict.Add("getUserShareHour", "https://api.weixin.qq.com/datacube/getusersharehour?access_token={0}");
                    dict.Add("getUpStreamMsg", "https://api.weixin.qq.com/datacube/getupstreammsg?access_token={0}");
                    dict.Add("getUpStreamMsgByHour", "https://api.weixin.qq.com/datacube/getupstreammsghour?access_token={0}");
                    dict.Add("getUpStreamMsgByWeek", "https://api.weixin.qq.com/datacube/getupstreammsgweek?access_token={0}");
                    dict.Add("getUpStreamMsgByMonth", "https://api.weixin.qq.com/datacube/getupstreammsgmonth?access_token={0}");
                    dict.Add("getUpStreamMsgDist", "https://api.weixin.qq.com/datacube/getupstreammsgdist?access_token={0}");
                    dict.Add("getUpStreamMsgDistByWeek", "https://api.weixin.qq.com/datacube/getupstreammsgdistweek?access_token={0}");
                    dict.Add("getUpStreamMsgDistByMonth", "https://api.weixin.qq.com/datacube/getupstreammsgdistmonth?access_token={0}");
                    dict.Add("getInterfaceSummary", "https://api.weixin.qq.com/datacube/getinterfacesummary?access_token={0}");
                    dict.Add("getInterfaceSummaryByHour", "https://api.weixin.qq.com/datacube/getinterfacesummaryhour?access_token={0}");
                    dict.Add("getCardQrCode", "https://api.weixin.qq.com/card/qrcode/create?access_token={0}");
                    dict.Add("createMaterial", "https://api.weixin.qq.com/cgi-bin/material/add_news?access_token={0}");
                    dict.Add("getMaterial", "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token={0}");
                    dict.Add("deleteMaterial", "https://api.weixin.qq.com/cgi-bin/material/del_material?access_token={0}");
                    dict.Add("updateMaterial", "https://api.weixin.qq.com/cgi-bin/material/update_news?access_token={0}");
                    dict.Add("getMaterialCount", "https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token={0}");
                    dict.Add("getMaterialList", "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token={0}");
                    dict.Add("uploadImg", "https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={0}");
                    dict.Add("register", "https://api.weixin.qq.com/shakearound/account/register?access_token={0}");
                    dict.Add("auditStatus", "https://api.weixin.qq.com/shakearound/account/auditstatus?access_token={0}");
                    dict.Add("createPoi", "http://api.weixin.qq.com/cgi-bin/poi/addpoi?access_token={0}");
                    dict.Add("getPoi", "http://api.weixin.qq.com/cgi-bin/poi/getpoi?access_token={0}");
                    dict.Add("getPoiList", "https://api.weixin.qq.com/cgi-bin/poi/getpoilist?access_token={0}");
                    dict.Add("updatePoi", "https://api.weixin.qq.com/cgi-bin/poi/updatepoi?access_token={0}");
                    dict.Add("deletePoi", "https://api.weixin.qq.com/cgi-bin/poi/delpoi?access_token={0}");
                    dict.Add("getWxCategory", "http://api.weixin.qq.com/cgi-bin/poi/getwxcategory?access_token={0}");
                    dict.Add("getUserinfo", "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN");
                    dict.Add("getAuthorize", "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect");
                    dict.Add("getAccessTokenByCode", "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code");
                    dict.Add("getsession", "https://api.weixin.qq.com/customservice/kfsession/getsession?access_token={0}&openid={1}&lang=zh_CN");
                    dict.Add("getUserInfoList", "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}");
                    dict.Add("getOpenidToken", "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}");
                    dict.Add("getDetailUserInfo", "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang={2}");
                    dict.Add("getRefershAccessToken", "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}");
                    dict.Add("getMedia", "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}");
                    dict.Add("getPreview", "https://api.weixin.qq.com/cgi-bin/message/mass/preview?access_token={0}");
                    dict.Add("invitationCustom", "https://api.weixin.qq.com/customservice/kfaccount/inviteworker?access_token={0}");//邀请绑定客服
                }
                #endregion
                return dict;
            }
        }
    }
    public class WechatPostRequest
    {

        public string functionName { get; set; }
        public string data { get; set; }

        public string GetFunctionUrl()
        {
            if (WechatFuntion.List.ContainsKey(this.functionName))
            {
                return WechatFuntion.List[this.functionName];
            }
            return this.functionName;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}