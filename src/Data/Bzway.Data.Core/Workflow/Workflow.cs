using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bzway.Data.Core
{
    public partial class Workflow : DynamicEntity
    {
        /// <summary>
        /// 实体类名
        /// </summary>
        public string Entity
        {
            get
            {
                if (this.ContainsKey("Entity") && this["Entity"] != null)
                {
                    return this["Entity"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["Entity"] = value;
            }
        }
        public System.String Code
        {
            get
            {
                if (this.ContainsKey("Code") && this["Code"] != null)
                {
                    return this["Code"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["Code"] = value;
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
    }
    public partial class Workflow : DynamicEntity
    {
        public bool EnableVersion
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
        public bool HasWorkflow
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
        public string UUID
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
}
