using System;
using System.Text.RegularExpressions;

namespace Tactile.Console.Parameters
{
    public class StringParameter: BaseParameter<string>
    {
        private readonly Regex _pattern;

        public StringParameter(string name, string description, bool isRequired) : base(name, description, isRequired)
        {
            _pattern = null;
        }

        public StringParameter(string name, string description, bool isRequired, Regex pattern) : base(name, description, isRequired)
        {
            _pattern = pattern;
        }
        
        protected override void ParseParameter(string parameter, out bool isValid, out string[] autocompleteSuggestions)
        {
            isValid = _pattern == null || _pattern.IsMatch(parameter);
            autocompleteSuggestions = Array.Empty<string>();
        }

        protected override bool TryGetValue(string parameter, out string value)
        {
            value = parameter;
            return true;
        }
    }
}