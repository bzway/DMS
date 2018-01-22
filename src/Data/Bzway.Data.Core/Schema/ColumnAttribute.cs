using System;
using System.Collections.Generic;
using System.Linq; 


namespace Bzway.Data.Core
{
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string name, string label = "", string controlType = "input", bool allowNull = true,
        int length = 0, int order = 0, bool modifiable = true, bool indexable = false,
            bool showInGrid = true, string toolTip = "", string defaultValue = "", string RegExp = "")
        {

        }
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
        public string RegExp { get; set; }
    }
}