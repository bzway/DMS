using Bzway.Database.Core;
using System;

namespace Bzway.Framework.Application.Entity
{
    /// <summary>
    /// 为每个应用存储用户的角色
    /// </summary>
    public class UserRole : EntityBase
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
