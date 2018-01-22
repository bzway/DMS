using System;
using System.Collections;
using System.Collections.Generic;
namespace Bzway.Data.Core
{
    public interface ITransaction
    {
        /// <summary>
        ///     Completes the transaction. Not calling complete will cause the transaction to rollback on dispose.
        /// </summary>
        void Complete();
    }
}