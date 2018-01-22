using Bzway.Data.Core;
using System.Data;
using System.Linq;
using System.Text;

namespace Bzway.Data.SQLServer
{
    public class SQLServerDatabase : OpenDatabase, IDatabase
    {
        static bool firstTime = true;
        public SQLServerDatabase()
        {

        }
        public SQLServerDatabase(string connectionString, string databaseName)
        {
            if (firstTime)
            {
                firstTime = false;
                string sql = string.Format(@"IF NOT  EXISTS( SELECT * FROM sys.databases WHERE name ='{0}')
BEGIN
CREATE DATABASE [{0}] CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'{0}', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\{0}.mdf' , SIZE = 5120KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'{0}_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\{0}_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%) 
END", databaseName);
                SqlHelper.ExecuteNonQuery(string.Concat(connectionString, "initial catalog=master"), CommandType.Text, sql);

                sql = @"
IF EXISTS ( SELECT  *
        FROM    sys.objects
        WHERE   name = 'System.Schema'
                AND type = 'u' )
BEGIN
    DROP TABLE [System.Schema]
END

CREATE TABLE [dbo].[System.Schema]
    (
      [ID] [INT] IDENTITY(1, 1)
                 NOT NULL ,
      [Code] [NVARCHAR](MAX) NULL ,
      [Name] [NVARCHAR](MAX) NULL ,
      [Type] [NVARCHAR](MAX) NULL ,
      [OpenID] [NVARCHAR](MAX) NULL ,
      [Status] INT NOT  NULL ,
      [CreatedBy] [INT] NOT NULL ,
      [CreatedOn] [DATETIME] NOT NULL ,
      [UpdatedBy] [INT] NULL ,
      [UpdatedOn] [DATETIME] NULL ,
      [IsActive] [BIT] NOT NULL,
    )";
                SqlHelper.ExecuteNonQuery(string.Concat(connectionString, "initial catalog=", databaseName), CommandType.Text, sql);
            }

            this.ConnectionString = string.Concat(connectionString, "initial catalog=", databaseName);
            this.DatabaseName = databaseName;

        }
        public override IRepository<T> BaseEntity<T>()
        {
            var type = typeof(T);
            var entityName = type.Name;
            var schema = this[entityName];
            if (schema == null)
            {
                schema = GetSchemaFromEntity(type, entityName);
            }
            return new BaseEntitySqlServerRepository<T>(ConnectionString, schema);
        }
        public override IRepository<DynamicEntity> DynamicEntity(Schema schema)
        {
            return new DynamicEntitySqlServerRepository<DynamicEntity>(ConnectionString, schema);
        }
        public override IRepository<T> Entity<T>()
        {
            var type = typeof(T);
            var entityName = type.Name;
            var schema = this[entityName];
            if (schema == null)
            {
                schema = GetSchemaFromEntity(type, entityName);
            }
            return new EntitySqlServerRepository<T>(ConnectionString, schema);
        }
        public override IDatabase Clone(string ConnectionString, string DatabaseName)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                return new SQLServerDatabase();
            }
            if (string.IsNullOrEmpty(DatabaseName))
            {
                return new SQLServerDatabase(ConnectionString, string.Empty);
            }
            return new SQLServerDatabase(ConnectionString, DatabaseName);
        }
        public override bool RefreshSchema(Schema item)
        {
            var sql = item.GetUpdateSQL();
            SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql);
            return base.RefreshSchema(item);
        }
    }
    public static class SchemaHelper
    {
        public static string GetUpdateSQL(this Schema item)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("IF NOT EXISTS ( SELECT TOP 1 1 FROM sys.objects WHERE   name = '{0}' AND type = 'u' )\r\n", item.Name);
            sb.AppendLine("BEGIN");
            sb.AppendFormat("CREATE TABLE {0}\r\n", item.Name);
            sb.AppendLine("(");
            sb.AppendFormat("_id INT NOT NULL IDENTITY CONSTRAINT [PK_dbo.{0}] PRIMARY KEY ,\r\n", item.Name);
            foreach (var c in item.AllColumns.OrderBy(m => m.Order))
            {
                var dbType = "NVARCHAR(MAX)";
                switch (c.ControlType)
                {
                    case "Text":
                        dbType = "NVARCHAR(MAX)";
                        break;
                    default:
                        break;
                }

                sb.AppendFormat("{0} {1} {2} NULL,\r\n", c.Name, dbType, c.AllowNull ? "" : "NOT ");
            }
            sb.AppendLine(")");
            sb.AppendLine("END");
            sb.AppendLine("ELSE");
            sb.AppendLine("BEGIN");

            foreach (var c in item.AllColumns.OrderBy(m => m.Order))
            {
                sb.AppendFormat("IF exists(select * from sys.columns where object_id=object_id('{0}') and name='{1}')\r\n", item.Name, c.Name);
                sb.AppendFormat("BEGIN\r\n");
                sb.AppendFormat("ALTER TABLE {0} DROP COLUMN {1}; \r\n", item.Name, c.Name);
                sb.AppendFormat("END \r\n");
                var dbType = "NVARCHAR(MAX)";
                switch (c.ControlType)
                {
                    case "Text":
                        dbType = "NVARCHAR(MAX)";
                        break;
                    default:
                        break;
                }
                sb.AppendFormat("ALTER TABLE {0} ADD {1} {2} {3} NULL;\r\n", item.Name, c.Name, dbType, c.AllowNull ? "" : "NOT");
            }
            sb.AppendLine("END");
            return sb.ToString();
        }
    }
}