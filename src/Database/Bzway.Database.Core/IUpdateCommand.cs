using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace Bzway.Database.Core
{

    public interface IUpdateCommand<T> : ICommand<T>
    {
        IUpdate<T> Update { get; }
        IWhere<T> Where { get; }
    }
  
}
