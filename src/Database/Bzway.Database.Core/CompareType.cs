
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bzway.Database.Core
{
    public enum CompareType
    {
        Startwith,
        EndWith,
        Contains,
        NoLike,
        Like,
        Equal,
        NotEqual = 35,
        GreaterThan = 15,
        GreaterThanOrEqual = 16,
        LessThan = 20,
        LessThanOrEqual = 21,
    }
}