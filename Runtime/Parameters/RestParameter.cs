using System;

namespace Tactile.Console.Parameters
{
    /// <summary>
    /// Captures everything in the rest of the command. The rest parameter must come last otherwise an exception will be
    /// thrown.
    /// </summary>
    public class RestParameter : BaseParameter<string>
    {
        public RestParameter(string name, string description, bool isRequired) : base(name, description, isRequired)
        {
        }

        protected override void ParseParameter(string parameter, out bool isValid, out string[] autocompleteSuggestions)
        {
            isValid = true;
            autocompleteSuggestions = Array.Empty<string>();
        }

        protected override bool TryGetValue(string parameter, out string value)
        {
            value = parameter;
            return true;
        }
    }
}