using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bzway.Data.Core
{
    public partial class WorkflowAction : DynamicEntity
    {

        public WorkflowTransition WorkflowTransition
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
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

    public partial class WorkflowAction 
    {
        /*
        System.String TransitionID, 
        System.String Name, 
        Bzway.Business.Model.ActionType Type, 
        System.String Module, 
        System.String Method, 
        */
        public System.String TransitionID
        {
            get
            {
                if (this.ContainsKey("TransitionID") && this["TransitionID"] != null)
                {
                    return this["TransitionID"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["TransitionID"] = value;
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
        public ActionType Type
        {
            get
            {
                if (this.ContainsKey("Type") && this["Type"] != null)
                {
                    return (ActionType)this["Type"];
                }
                return 0;
            }
            set
            {
                this["Type"] = value;
            }
        }
        public System.String Module
        {
            get
            {
                if (this.ContainsKey("Module") && this["Module"] != null)
                {
                    return this["Module"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["Module"] = value;
            }
        }
        public System.String Method
        {
            get
            {
                if (this.ContainsKey("Method") && this["Method"] != null)
                {
                    return this["Method"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["Method"] = value;
            }
        }

    }
    public enum ActionType
    {
        SQL,
        WebService,
        Script,
        Function
    }
}
