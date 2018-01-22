namespace Bzway.Tools.Grok.Core
{
    public class RegexAlias
    {
        public string Name { get; set; }
        public string RegexPattern { get; set; }

        public override string ToString()
        {
            return string.Format("Alias: {0} ({1})", Name, RegexPattern);
        }
    }
}
