using System;
using System.Collections;
using System.Collections.Generic;
namespace Bzway.Data.Core
{
    public interface IDatabase : IDisposable, ISchema
    {
        IDatabase Clone(string ConnectionString, string DatabaseName);
        IRepository<DynamicEntity> DynamicEntity(Schema schema);
        //IRepository<T> Entity<T>() where T : new();
        IRepository<T> Entity<T>() where T : EntityBase , new();


        ITransaction GetTransaction();

        /// <summary>
        ///     Starts a transaction scope, see GetTransaction() for recommended usage
        /// </summary>
        void BeginTransaction();

        /// <summary>
        ///     Aborts the entire outer most transaction scope
        /// </summary>
        /// <remarks>
        ///     Called automatically by Transaction.Dispose()
        ///     if the transaction wasn't completed.
        /// </remarks>
        void AbortTransaction();
    }
}