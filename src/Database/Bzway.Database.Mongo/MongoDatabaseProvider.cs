﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Bzway.Database.Core;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Newtonsoft.Json;
using Bzway.Common.Share;
using System.Reflection;
using System.Threading;

namespace Bzway.Database.Mongo
{
    public class MongoDatabaseProvider : IDatabaseProivder
    {
        public ISystemDatabase GetDatabase(string connectionString, string databaseName)
        {
            return new MongoDatabase(connectionString, databaseName);
        }
    }
}