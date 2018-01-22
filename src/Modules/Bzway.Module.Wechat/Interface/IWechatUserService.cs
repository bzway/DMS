using Bzway.Module.Wechat.Entity;
using System.Collections.Generic;
using System.Linq;

public interface IWechatUserService
{

    IQueryable<WechatUserGroup> GetAllTags();

    IQueryable<WechatUser> GetUsers();

}