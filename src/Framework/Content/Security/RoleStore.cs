using Microsoft.AspNet.Identity;
using OpenData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenData.Security.Identity
{
    /// <summary>
    /// Class that implements the key ASP.NET Identity role store iterfaces
    /// </summary>
    public class RoleStore<TRole> : IQueryableRoleStore<TRole> where TRole : IdentityRole, new()
    {
        private DefaultDatabase db;
        public IQueryable<TRole> Roles
        {
            get
            {
                List<TRole> list = new List<TRole>();
                foreach (var item in this.db.Entity(null).Query().ToList())
                {
                    list.Add(new TRole() { Id = item.UUID, Name = item["Name"].ToString() });
                }
                return list.AsQueryable();
            }
        }



        /// <summary>
        /// Constructor that takes a MySQLDatabase as argument 
        /// </summary>
        /// <param name="database"></param>
        public RoleStore(DefaultDatabase database)
        {
            this.db = database;
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<Object>(null);
        }

        public Task<TRole> FindByIdAsync(string roleId)
        {
            TRole result = null;

            return Task.FromResult<TRole>(result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            TRole result = null;

            return Task.FromResult<TRole>(result);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            //db.Update(role);

            return Task.FromResult<Object>(null);
        }

        public void Dispose()
        {
        }

    }
}
