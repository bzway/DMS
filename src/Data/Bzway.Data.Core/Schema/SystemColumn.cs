using System;
using System.Collections.Generic;
using System.Linq; 


namespace Bzway.Data.Core
{
    public class SystemColumn
    {
        public static string[] SystemFields;
        public static Column[] Columns;

        static SystemColumn()
        {
            List<Column> list = new List<Column>();
            list.Add(new Column()
            {
                Name = "Id",
                Label = "Id",
                ControlType = "Hidden",
                Order = 0,
            });
            list.Add(new Column()
            {
                Name = "UserKey",
                Label = "User Key",
                ControlType = "TextBox",
                AllowNull = true,
                DefaultValue = "",
                Order = 100,
                Tooltip = "An user and SEO friendly content key, it is mostly used to customize the page URL"
            });
            list.Add(new Column()
            {
                Name = "CreatedOn",
                Label = "Created Time",
                AllowNull = true,
                ShowInGrid = true,
                ControlType = "Hidden",
                Order = 98,
            });
            list.Add(new Column()
            {
                Name = "UpdatedOn",
                Label = "Updated Time",
                ControlType = "Hidden",
                Order = 98,
            });
            list.Add(new Column()
            {
                Name = "UpdatedBy",
                Label = "UpdatedBy",
                ControlType = "Hidden",
                Order = 99,
            });
            list.Add(new Column()
            {
                Name = "CreatedBy",
                Label = "CreatedBy",
                ControlType = "Hidden"
            });
            list.Add(new Column()
            {
                Name = "Status",
                Label = "Status",
                ControlType = "Hidden",
                Order = 99,
            });
            SystemColumn.SystemFields = list.Select(m => m.Name).ToArray();
            SystemColumn.Columns = list.ToArray();
        }
    }
}