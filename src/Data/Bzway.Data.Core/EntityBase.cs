using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Bzway.Data.Core
{

    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public abstract class EntityBase : IEntity
    {
        public virtual string CreatedBy { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual string Id { get; set; }

        public virtual string UpdatedBy { get; set; }

        public virtual DateTime UpdatedOn { get; set; }
    }


    public interface IEntity : IEntity<string>
    { }
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime UpdatedOn { get; set; }

        TKey CreatedBy { get; set; }

        TKey UpdatedBy { get; set; }
    }
}
