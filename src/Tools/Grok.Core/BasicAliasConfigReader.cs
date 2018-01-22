using System.Collections.Generic;
using System.IO;

namespace Bzway.Tools.Grok.Core
{
    public static class BasicAliasConfigReader
    {
        public static IEnumerable<RegexAlias> Parse(string fileName)
        {
            foreach (var line in File.ReadAllLines(fileName))
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                if (line.StartsWith("#"))
                {
                    continue;
                }
                var spaceIndex = line.IndexOf(' ');
                if (spaceIndex <= 0)
                {
                    continue;
                }

                var name = line.Substring(0, spaceIndex);
                var pattern = line.Substring(spaceIndex + 1);

                yield return new RegexAlias
                {
                    Name = name,
                    RegexPattern = pattern
                };
            }
        }
    }
}
