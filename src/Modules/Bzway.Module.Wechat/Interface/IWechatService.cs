using Bzway.Common.Collections;
using Bzway.Module.Wechat.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Bzway.Module.Wechat.Interface
{
    public interface IWechatService
    {
        #region User
        IQueryable<WechatUserGroup> GetAllTags();

        IQueryable<WechatUser> GetUsers();

        #endregion
        #region Menu


        #endregion
        void SyncMaterial();
        #region Keyword

        IEnumerable<WechatKeyword> GetWechatResponse(string Keyword = "", SearchType SearchType = SearchType.None, string wechatId = "");
        IPagedList<WechatMaterial> GetWechatMaterils(string keyword = "", WechatMaterialType news = WechatMaterialType.All, int index = 0, int pageSize = 10);
        #endregion
    }
}