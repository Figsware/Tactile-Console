using System;

namespace Tactile.Console.Parameters
{
    public class BooleanParameter : BaseParameter<bool>
    {
        public BooleanParameter(string name, string description) : base(name, description, true)
        {
        }
        public BooleanParameter(string name, string description, bool isRequired) : base(name, description, isRequired)
        {
        }
        
        protected override void ParseParameter(string parameter, out bool isValid, out string[] autocompleteSuggestions)
        {
            isValid = bool.TryParse(parameter, out _);
            autocompleteSuggestions = Array.Empty<string>();
        }

        protected override bool TryGetValue(string parameter, out bool value)
        {
            return bool.TryParse(parameter, out value);
        }

        
    }
}