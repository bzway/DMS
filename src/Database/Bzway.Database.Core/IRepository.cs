using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace Bzway.Database.Core
{
    public interface IRepository<T>
    {
        #region 增加
        void Insert(T newData);

        #endregion
        #region 删除
        void Delete(T newData);
        void Delete(string uuid);


        #endregion
        #region 修改
        void Update(IUpdate<T> update, IWhere<T> where);

        void Update(T newData, string uuid = "");

        #endregion
        #region 查询
        IQueryable<T> Query();
        IWhere<T> Filter();
        #endregion
        bool Execute(ICommand<T> command);
    }
}