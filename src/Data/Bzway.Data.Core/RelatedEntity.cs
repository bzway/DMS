using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Bzway.Data.Core
{



    public class RelatedEntity : EntityBase, IRelatedEntity
    {
        public string DisplayName { get; set; }

        public RelationType OneToOne { get; set; }

        public List<IEntity> Relates { get; set; }
    }

    public interface IRelatedEntity : IEntity
    {
        RelationType OneToOne { get; set; }

        string DisplayName { get; set; }

        List<IEntity> Relates { get; set; }
    }

    public enum RelationType
    {
        OneToZero,
        OneToOne,
        OneToMany,
    }
}