using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using System.Text;
using System.Xml.Serialization;


namespace Bzway.Data.Core
{
    public class Schema
    {
        public static Schema SchemaSchema
        {
            get
            {
                var schema = Schema.EntitySchema("System.Schema", "System Schema");
                schema.AddColumn(new Column { Name = "Name" });
                schema.AddColumn(new Column { Name = "Description" });
                return schema;
            }
        }
        public static Schema ColumnSchema
        {
            get
            {
                var schema = Schema.EntitySchema("System.Column", "System Schema");
                schema.AddColumn(new Column { Name = "SchemaName", Indexable = true });
                schema.AddColumn(new Column { Name = "Name" });
                schema.AddColumn(new Column { Name = "Description" });
                schema.AddColumn(new Column { Name = "Label" });
                schema.AddColumn(new Column { Name = "ControlType" });
                schema.AddColumn(new Column { Name = "AllowNull" });
                schema.AddColumn(new Column { Name = "Length" });
                schema.AddColumn(new Column { Name = "Order" });
                schema.AddColumn(new Column { Name = "Modifiable" });
                schema.AddColumn(new Column { Name = "Indexable" });
                schema.AddColumn(new Column { Name = "ShowInGrid" });
                schema.AddColumn(new Column { Name = "Tooltip" });
                schema.AddColumn(new Column { Name = "DefaultValue" });
                return schema;
            }
        }

        public static Schema EntitySchema(string Name, string Description)
        {
            return new Schema(Name, Description);
        }
        private Schema(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;

        }
        private List<Column> columns = new List<Column>();

        public string Name { get; set; }
        public string Description { get; set; }

        public bool AddColumn(Column column)
        {
            if (this.AllColumns.Where(m => m.Name == column.Name).FirstOrDefault() != null)
            {
                return false;
            }

            this.columns.Add(column);
            return true;
        }
        public int RemoveColumn(Column column)
        {
            var index = this.columns.IndexOf(column);
            this.columns.Remove(column);
            return index;
        }
        public int UpdateColumn(Column oldColumn, Column newColumn)
        {
            var index = RemoveColumn(oldColumn);
            this.columns.Insert(0, newColumn);
            return index;
        }

        public Column this[string columnName]
        {
            get
            {
                return this.AllColumns.Where(it => string.Compare(it.Name, columnName, true) == 0).FirstOrDefault();
            }
        }

        public List<Column> SystemColumns
        {
            get
            {
                return SystemColumn.Columns.ToList();
            }
        }
        public List<Column> AllColumns
        {
            get
            {

                if (this.columns == null)
                {
                    return this.SystemColumns;
                }
                return this.columns.OrderBy(o => o.Order).Concat(SystemColumn.Columns).ToList();
            }
        }
    }
}