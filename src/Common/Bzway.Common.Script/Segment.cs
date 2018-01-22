using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.Loader;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bzway.Common.Script
{
    public class Segment
    {
        readonly bool isVariable;
        readonly string segment;
        readonly string variableType;
        readonly string defaultValue;
        public bool AllowNull { get; private set; }
        public Segment(string segment)
        {
            this.isVariable = segment.StartsWith("$");
            //不是变量直接返回
            if (!this.isVariable)
            {
                this.segment = segment;
                return;
            }
            //默认值处理
            var defaults = segment.Split('=');
            if (defaults.Length > 1)
            {
                this.defaultValue = defaults[1];
            }
            //约束处理
            var constraints = defaults[0].Split(':');
            if (constraints.Length > 1)
            {
                this.variableType = constraints[1];
            }
            //是否允许为空
            this.AllowNull = constraints[0].EndsWith("?");
            if (this.AllowNull)
            {
                this.segment = constraints[0].TrimStart('$').TrimEnd('?');
            }
            else
            {
                this.segment = constraints[0].TrimStart('$');
            }
        }

        public MatchResult<KeyValuePair<string, object>> IsMatch(string input)
        {
            //非变量直接比较是否相同
            if (!this.isVariable)
            {
                return new MatchResult<KeyValuePair<string, object>>()
                {
                    Result = string.Compare(this.segment, input, true) == 0,
                    Data = new KeyValuePair<string, object>(),
                };
            }
            //如果是空判断是否允许为空
            if (string.IsNullOrEmpty(input))
            {
                return new MatchResult<KeyValuePair<string, object>>()
                {
                    Result = this.AllowNull,
                    Data = new KeyValuePair<string, object>(this.segment, this.defaultValue),
                };
            }

            switch (this.variableType)
            {
                case "int":
                    int intValue;
                    if (int.TryParse(input, out intValue))
                    {
                        return new MatchResult<KeyValuePair<string, object>>()
                        {
                            Result = true,
                            Data = new KeyValuePair<string, object>(this.segment, intValue),
                        };
                    }
                    else
                    {
                        return new MatchResult<KeyValuePair<string, object>>()
                        {
                            Result = false,
                            Data = new KeyValuePair<string, object>(),
                        };
                    }
                case "bool":
                    bool boolValue;
                    if (bool.TryParse(input, out boolValue))
                    {
                        return new MatchResult<KeyValuePair<string, object>>()
                        {
                            Result = true,
                            Data = new KeyValuePair<string, object>(this.segment, boolValue),
                        };
                    }
                    else
                    {
                        return new MatchResult<KeyValuePair<string, object>>()
                        {
                            Result = false,
                            Data = new KeyValuePair<string, object>(),
                        };
                    }
                case "string":
                    return new MatchResult<KeyValuePair<string, object>>()
                    {
                        Result = true,
                        Data = new KeyValuePair<string, object>(this.segment, (input)),
                    };
                case "date":
                    DateTime dateValue;
                    if (DateTime.TryParse(input, out dateValue))
                    {
                        return new MatchResult<KeyValuePair<string, object>>()
                        {
                            Result = true,
                            Data = new KeyValuePair<string, object>(this.segment, dateValue),
                        };
                    }
                    else
                    {
                        return new MatchResult<KeyValuePair<string, object>>()
                        {
                            Result = false,
                            Data = new KeyValuePair<string, object>(),
                        };
                    }
                case null:
                    return new MatchResult<KeyValuePair<string, object>>()
                    {
                        Result = true,
                        Data = new KeyValuePair<string, object>(this.segment, input),
                    };
                default:
                    if (variableType.StartsWith("regex"))
                    {
                        RegexMatcher matcher = new RegexMatcher(variableType);
                        matcher.Match(input);
                    }
                    return new MatchResult<KeyValuePair<string, object>>()
                    {
                        Result = true,
                        Data = new KeyValuePair<string, object>(this.segment, input),
                    };
            }
        }
    }
    public class MatchResult<T>
    {
        public bool Result { get; set; }
        public T Data { get; set; }
    }
}