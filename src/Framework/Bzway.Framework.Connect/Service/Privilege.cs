using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Bzway.Framework.Connect.Entity;
using Microsoft.Extensions.Logging;
using Bzway.Common.Utility;
using Bzway.Framework.Application;
using System.Security.Principal;

namespace Bzway.Framework.Connect
{
    public class Privilege : BaseService<Privilege>, IPrivilege
    {
        #region ctor
        public Privilege(ILoggerFactory loggerFactory, ITenant tenant, IPrincipal user) : base(loggerFactory, tenant,   user) { }
        #endregion
        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string PrivilegeId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

    }
}