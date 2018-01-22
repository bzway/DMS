using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Bzway.Common.Script
{
    public class RuleValue<T>
    {
        public T Value { get; set; }
        public List<Rule> Rules { get; set; }
    }

    public class RuleValueString : RuleValue<string> { }
}
