using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Bzway.Data.Core
{
 

    public class DataModel
    {
        public Schema DataSchema { get; set; }
        public DynamicEntity Entity { get; set; }

        public List<RelatedEntity> Catetories { get; set; }

        public List<RelatedEntity> Childred { get; set; }
    }
}