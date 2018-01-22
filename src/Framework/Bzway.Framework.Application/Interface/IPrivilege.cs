using Bzway.Framework.Application.Entity;
using System.Collections.Generic;

namespace Bzway.Framework.Application
{
    public interface IPrivilege
    {
        string Name { get; }
        string Description { get; }
        string PrivilegeId { get; }
    }
}