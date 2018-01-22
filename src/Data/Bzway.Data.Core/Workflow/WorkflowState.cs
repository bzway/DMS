using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bzway.Data.Core
{
    public partial class WorkflowState
    {
        public Workflow Workflow { get; set; }

        public   bool EnableVersion
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
        public   bool HasWorkflow
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
        public   string UUID
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }
    }
    public partial class WorkflowState : DynamicEntity
    {

        public System.String WorkflowID
        {
            get
            {
                if (this.ContainsKey("WorkflowID") && this["WorkflowID"] != null)
                {
                    return this["WorkflowID"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["WorkflowID"] = value;
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
        public int Value
        {
            get
            {
                if (this.ContainsKey("Value") && this["Value"] != null)
                {
                    return (int)this["Value"];
                }
                return 0;
            }
            set
            {
                this["Value"] = value;
            }
        }
        public System.String Description
        {
            get
            {
                if (this.ContainsKey("Description") && this["Description"] != null)
                {
                    return this["Description"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["Description"] = value;
            }
        }
        public System.Int32 X
        {
            get
            {
                if (this.ContainsKey("X") && this["X"] != null)
                {
                    return (System.Int32)this["X"];
                }
                return 0;
            }
            set
            {
                this["X"] = value;
            }
        }
        public System.Int32 Y
        {
            get
            {
                if (this.ContainsKey("Y") && this["Y"] != null)
                {
                    return (System.Int32)this["Y"];
                }
                return 0;
            }
            set
            {
                this["Y"] = value;
            }
        }

        public Workflow Workflow1
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