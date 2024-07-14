using System;

namespace Tactile.Console.Parameters
{
    public class IntegerParameter : BaseParameter<int>
    {
        public IntegerParameter(string name, string description) : base(name, description, true)
        {
        }
        public IntegerParameter(string name, string description, bool isRequired) : base(name, description, isRequired)
        {
        }
        
        protected override void ParseParameter(string parameter, out bool isValid, out string[] autocompleteSuggestions)
        {
            isValid = int.TryParse(parameter, out _);
            autocompleteSuggestions = Array.Empty<string>();
        }

        protected override bool TryGetValue(string parameter, out int value)
        {
            return int.TryParse(parameter, out value);
        }

        
    }
}