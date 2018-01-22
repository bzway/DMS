using Bzway.Module.Wechat.Model;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Interface
{
    public interface IWechatApiService
    {
        WechatBaseResponseModel ActivateCard(string data);
        WechatBaseResponseModel AddConditionalMenu(WechatCustomButtonMenu menu);
        AuditStatusResponseModel AuditStatus();
        WechatBaseResponseModel BatchMoveMemberGroup(BatchMoveMemberGroup batchGroup);
        WechatBaseResponseModel Closedialogue(KFSessionModel session);
        WechatBaseResponseModel ConsumeCard(string cardId, string cardCode);
        WechatBaseResponseModel CreateCards(string data);
        WechatBaseResponseModel CreateCustom(CustomModel custom);
        string CreateGroup(string name);
        WechatBaseResponseModel CreateNewsMaterial(WechatNewsMaterialCreateRequestModel material);
        bool CreateMenu(WechatButtonMenu menu);
        WechatBaseResponseModel CreatePoi(PoiRequestModel model);
        WechatCreateQRCodeTicketResponseModel CreateQRCodeTicket(ActionModel qrCode);
        WechatBaseResponseModel CustomSend(CustomSendModel custom);
        WechatBaseResponseModel DecryptCard(string encrypt_code);
        WechatBaseResponseModel DeleteCard(string cardId);
        WechatBaseResponseModel DeleteConditionalMenu(string menuId);
        WechatBaseResponseModel DeleteCustom(CustomModel custom, string AccessToken);
        WechatBaseResponseModel DeleteGroup(int groupId);
        WechatBaseResponseModel DeleteGroupMsg(string msgId);
        WechatBaseResponseModel DeleteMaterial(string mediaId);
        WechatBaseResponseModel DeleteMenu();
        WechatBaseResponseModel DeletePoi(string poi_id);
        WechatGetTagsResponseModel GetAllGroups();
        WechatGetArticleSummaryResponseModel GetArticleSummary(string start, string end);
        WechatGetArticleTotalResponseModel GetArticleTotal(string start, string end);
        WechatGetJsapiTicketResponseModel GetCardApiTicket();
        WeChatMemberCard GetCardStatus(string cardId);
        WechatBaseResponseModel GetCurrentSelfMenu();
        WechatGetUserInfoResponseModel GetDetailUserInfo(string openId, string lang);
        WechatBaseResponseModel GetDetailUserInfoList(BatchGetUserInfo batchUserInfo);
        WechatGetInterfaceSummaryResponseModel GetInterfaceSummary(string start, string end);
        WechatGetInterfaceSummaryResponseModel GetInterfaceSummaryByHour(string start, string end);
        WechatGetJsapiTicketResponseModel GetJsApiTicket();
        WechatGetKFListResponseModel GetKFList();
        WechatGetNewsMaterialResponseModel GetMaterial(string mediaId);
        WechatGetMaterialCountResponseModel GetMaterialCount();
        WechatGetMaterialListResponseModel GetMaterialList(string type, int offset, int count);
        WechatBaseResponseModel GetMemberGroup(string openId);
        WechatBaseResponseModel GetMenu();
        MsgRecordModel GetMsgRecord(MsgRecordPostModel record);
        WechatGetQRCodeCardResponseModel GetMultipleCardQrCode(MultipleQrCodeCard cardList);
        WechatGetQRCodeCardResponseModel GetOneCardQrCode(string action_name);
        OnLineKFModel GetOnLineKFList();
        PoiResponseModel GetPoi(string poi_id);
        PoiListResponseModel GetPoiList(int begin, int limit = 20);
        WeChatSDKModel GetSDKConfig(string url);
        KFSessionResponseModel Getsession(string openId);
        WechatCreateFShortUrlResponseModel GetShortUrl(string long_url);
        WechatGetUpStreamMsgResponseModel GetUpStreamMsg(string start, string end);
        WechatGetUpStreamMsgResponseModel GetUpStreamMsgByHour(string start, string end);
        WechatGetUpStreamMsgResponseModel GetUpStreamMsgByMonth(string start, string end);
        WechatGetUpStreamMsgResponseModel GetUpStreamMsgByWeek(string start, string end);
        WechatGetUpStreamMsgResponseModel GetUpStreamMsgDist(string start, string end);
        WechatGetUpStreamMsgResponseModel GetUpStreamMsgDistByMonth(string start, string end);
        WechatGetUpStreamMsgResponseModel GetUpStreamMsgDistByWeek(string start, string end);
        wechatGetUserAccessTokenResponseModel GetUserAccessTokenByCode(string appId, string appSecurityKey, string code);
        WechatGetUserCumulateResponseModel GetUserCumulate(string start, string end);
        WeChatGetUserListResponseModel GetUserInfoList(string nextOpenId);
        WechatGetArticleReadResponseModel GetUserRead(string start, string end);
        WechatGetArticleReadByHourResponseModel GetUserReadByHour(string start, string end);
        WechatGetArticleShareResponseModel GetUserShare(string start, string end);
        WechatGetArticleShareByHourResponseModel GetUserShareByHour(string start, string end);
        WechatGetUserCumulateResponseModel GetUserSummary(string start, string end);
        WechatGetUserInfoResponseModel GetWebUserInfo(string openId);
        PoiCategoryResponseModel GetWxCategory();
        WechatBaseResponseModel GrantCard(string cardId);
        WechatBaseResponseModel InvitationCustom(CustomModel custom);
        WechatBaseResponseModel ModifyStock(modifystock stock);
        WechatBaseResponseModel MoveMemberGroup(string openId, int toGroupId);
        WechatBaseResponseModel Opendialogue(KFSessionModel session);
        string RedirectToAuthorizeUrl(string redirectUrl, string appId, string state, string scope);
        WechatBaseResponseModel Register(RegisterRequestModel model);
        WechatBaseResponseModel SendMsgByOpenid(MassContentByOpenidModel byOpenId);
        WechatBaseResponseModel SendMsgByUserGroup(MassContentByGroupidModel byGroup);
        WechatBaseResponseModel SendTemplateMsg(TemplateMsgModel template);
        WechatBaseResponseModel UpdateCard(string data);
        WechatBaseResponseModel UpdateCustom(CustomModel custom);
        WechatBaseResponseModel UpdateGroup(int groupId, string groupName);
        WechatBaseResponseModel UpdateNewsMaterial(WechatNewsMaterialUpdateRequestModel material);
        WechatBaseResponseModel UpdatePoi(UpdatePoiResponseModel model);
        WechatBaseResponseModel UpdateRemark(string openId, string remark);
        string UploadTemplateMaterial(string filePath, string type);
        WechatBaseResponseModel UploadCustomHeadImg(string filePath, string accountName);
        string UploadNewsMaterialImage(string filePath);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wechatOpenId">openID or 微信号</param>
        /// <param name="type">mpnews,text,voice,image,mpvideo,wxcard,</param>
        /// <param name="content"></param>
        /// <returns></returns>
        WechatBaseResponseModel PreviewNewsMaterial(string wechatOpenId, string type, string content);

        /// <summary>
        ///  
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <returns></returns>
        WechatCreateOtherMaterialResponseModel CreateOtherMaterial(string filePath, string type, string title, string introduction);

    }
}