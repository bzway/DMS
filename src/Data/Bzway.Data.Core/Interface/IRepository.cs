using Bzway.Data.Core;
using Bzway.Data.Core.OpenExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Data.Core
{
    public interface IRepository<TSource>
    {
        #region 增加
        void Insert(TSource newData);

        #endregion
        #region 删除
        void Delete(TSource newData);
        void Delete(string uuid);


        #endregion
        #region 修改
        IUpdate<TSource> Update(IWhereExpression where);

        void Update(TSource newData, string uuid = "");

        #endregion
        #region 查询
        IOpenQuery<TSource> Query(params string[] fields);

        IWhere<TSource> Filter();
        #endregion
        object Execute(IOpenQuery<TSource> query);

        bool Execute(IUpdate<TSource> update);
    }
}