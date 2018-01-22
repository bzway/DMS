using System;
using System.Collections;
using System.Collections.Generic;
namespace Bzway.Data.Core
{
    public interface ISchema : IEnumerable<Schema>
    {
        Schema this[string key] { get; }
        bool RefreshSchema(Schema item);
        bool RemoveSchema(Schema item);
    }
}