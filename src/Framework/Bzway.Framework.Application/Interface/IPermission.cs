using Bzway.Framework.Application.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bzway.Framework.Application
{
    public interface IPermission
    {
        bool HasPermission(string Role, IPrivilege privilege, PermissionType type = PermissionType.ReadOnly);
    }
    [Flags]
    public enum PermissionType : int
    {
        [Description("ReadOnly")]
        ReadOnly = 0,
        Write,
        Publish,
        Export,
    }
}