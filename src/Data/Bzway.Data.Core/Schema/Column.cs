using System;
using System.Collections.Generic;
using System.Linq; 


namespace Bzway.Data.Core
{
    public class Column
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string ControlType { get; set; }
        public bool AllowNull { get; set; }
        public int Length { get; set; }
        public int Order { get; set; }
        public bool Modifiable { get; set; }
        public bool Indexable { get; set; }
        public bool ShowInGrid { get; set; }
        public string Tooltip { get; set; }
        public string DefaultValue { get; set; }

        #region IsSystemField
        public bool IsSystemField
        {
            get
            {
                return SystemColumn.SystemFields.Where(m => m.Equals(this.Name, StringComparison.OrdinalIgnoreCase)).Count() > 0;
            }
            set { }
        }
        #endregion
    }
}