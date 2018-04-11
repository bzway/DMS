using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Bzway.Framework.Application.Entity;
using System.Security.Principal;
using System.Linq;

namespace Bzway.Framework.Application
{
    /// <summary>
    /// GrantRequest service
    /// </summary>
    public partial class ResourceManager : BaseService<ResourceManager>, IResourceManager
    {

        #region ctor
        public ResourceManager(ILoggerFactory loggerFactory, ITenant tenant) : base(loggerFactory, tenant)
        {
        }
        #endregion


        public string GetString(string key, string culture)
        {
            var resource = this.db.Entity<LanguageResource>().Query().FirstOrDefault(m => m.Key == key && m.Language == culture);
            if (resource == null)
            {
                resource = new LanguageResource()
                {
                    Key = key,
                    Language = culture,
                    Value = GetValue(key),
                };
                this.db.Entity<LanguageResource>().Insert(resource);
            }
            return resource.Value;
        }
        private string GetValue(string key)
        {
            var i = key.LastIndexOf('.') + 1;
            if (i > 0)
            {
                return key.Substring(i);
            }
            return key;
        }
    }
}