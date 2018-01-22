using Bzway.Framework.Connect.Entity;
using System.Collections.Generic;

namespace Bzway.Framework.Connect
{
    public interface IPrivilege
    {
        string Name { get; }
        string Description { get; }
        string PrivilegeId { get; }
    }
}