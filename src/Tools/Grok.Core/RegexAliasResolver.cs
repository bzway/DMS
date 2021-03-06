﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bzway.Tools.Grok.Core
{
    public static class RapidRegexExtensions
    {
        public static Dictionary<string, string> MatchNamedCaptures(this Regex regex, string input)
        {
            var namedCaptureDictionary = new Dictionary<string, string>();
            GroupCollection groups = regex.Match(input).Groups;
            string[] groupNames = regex.GetGroupNames();
            foreach (string groupName in groupNames)
                if (groups[groupName].Captures.Count > 0)
                    namedCaptureDictionary.Add(groupName, groups[groupName].Value);
            return namedCaptureDictionary;
        }
    }

    public class RegexAliasResolver
    {
        private const string AliasPattern = @"%{\w+}";

        private readonly RegexAlias[] _aliases;

        public RegexAliasResolver(IEnumerable<RegexAlias> regexAliases)
        {
            _aliases = (regexAliases ?? new RegexAlias[0]).Where(x => x != null).ToArray();
            CompileDependentAliases();
        }

        public Regex ResolveToRegex(string aliasedPattern)
        {
            foreach (var alias in _aliases)
            {
                var groupedPattern = "%{(?<fieldname>" + alias.Name + "):(?<tag_name>\\w*)}";
                var nonGroupedPattern = "%{" + alias.Name + "}";
                aliasedPattern = Regex.Replace(aliasedPattern, groupedPattern, "(?<${tag_name}>%{${fieldname}})", RegexOptions.IgnoreCase);
                aliasedPattern = Regex.Replace(aliasedPattern, nonGroupedPattern, alias.RegexPattern, RegexOptions.IgnoreCase);
            }
            return new Regex(aliasedPattern);
        }
        private void CompileDependentAliases()
        {
            foreach (var alias in _aliases)
            {
                // Make sure another alias with this name does not exist
                if (_aliases.Count(x => x.Name == alias.Name) > 1)
                    throw new InvalidOperationException("Multiple aliases exist with the name: " + alias.Name);

                ComputeRawRegex(alias, new List<RegexAlias>());
            }
        }
        private void ComputeRawRegex(RegexAlias alias, List<RegexAlias> computedAliases)
        {
            // Add this as a computed dependency to detect circular dependencies
            computedAliases.Add(alias);

            foreach (var subAliasName in GetSubAliasNames(alias))
            {
                var matchName = string.Concat("%{", subAliasName, "}");

                // Find an alias with the specified name and put its 
                //   regex pattern into the current alias' pattern
                var subAlias = _aliases.FirstOrDefault(x => x.Name.Equals(subAliasName, StringComparison.InvariantCultureIgnoreCase));
                if (subAlias != null)
                {
                    // If the subAlias has already been computed this run,
                    //   we have a circular dependency
                    if (computedAliases.Contains(subAlias))
                        throw new InvalidOperationException("Circular dependency detected while computing alias dependencies");

                    // Compute the sub alias' regex
                    ComputeRawRegex(subAlias, computedAliases.ToList());
                    alias.RegexPattern = alias.RegexPattern.Replace(matchName, subAlias.RegexPattern);
                }
            }
        }
        private IEnumerable<string> GetSubAliasNames(RegexAlias alias)
        {
            return Regex.Matches(alias.RegexPattern, AliasPattern)
                        .Cast<Match>()
                        .Select(x => x.Value.Substring(2, x.Value.Length - 3))
                        .Distinct();
        }
    }
}
