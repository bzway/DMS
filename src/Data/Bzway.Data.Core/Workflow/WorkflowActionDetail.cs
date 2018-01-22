using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bzway.Data.Core
{
    public partial class WorkflowActionDetail : DynamicEntity
    {
        /*
        System.String ActionID, 
        System.String Name, 
        System.String Type, 
        System.String Value, 
        */
        public System.String ActionID
        {
            get
            {
                if (this.ContainsKey("ActionID") && this["ActionID"] != null)
                {
                    return this["ActionID"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["ActionID"] = value;
            }
        }
        public System.String Name
        {
            get
            {
                if (this.ContainsKey("Name") && this["Name"] != null)
                {
                    return this["Name"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["Name"] = value;
            }
        }
        public System.String Type
        {
            get
            {
                if (this.ContainsKey("Type") && this["Type"] != null)
                {
                    return this["Type"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["Type"] = value;
            }
        }
        public System.String Value
        {
            get
            {
                if (this.ContainsKey("Value") && this["Value"] != null)
                {
                    return this["Value"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["Value"] = value;
            }
        }

        public WorkflowAction WorkflowAction
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}