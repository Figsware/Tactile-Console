using System;
using System.Text.RegularExpressions;

namespace Tactile.Console.Parameters
{
    public abstract class BaseParameter
    {
        public readonly string Name;
        public readonly string Description;
        public readonly bool IsRequired;

        protected BaseParameter(string name, string description, bool isRequired)
        {
            Name = name;
            Description = description;
            IsRequired = isRequired;
        }

        public abstract MatchInfo Match(string parameter);

        public abstract class MatchInfo
        {
            public readonly bool IsValid;
            public readonly string[] AutocompleteSuggestions;
            protected readonly string Argument;

            protected MatchInfo(string argument, bool isValid, string[] autocompleteSuggestions)
            {
                Argument = argument;
                IsValid = isValid;
            }

            public abstract bool TryGetValue(out object value);
        }
    }

    public abstract class BaseParameter<T> : BaseParameter
    {
        public override BaseParameter.MatchInfo Match(string parameter)
        {
            ParseParameter(parameter, out var isValid, out var autocompleteSuggestions);
            var info = new MatchInfo(this, parameter, isValid, autocompleteSuggestions);

            return info;
        }
        
        protected abstract void ParseParameter(string parameter, out bool isValid,
            out string[] autocompleteSuggestions);

        protected abstract bool TryGetValue(string parameter, out T value);

        public new class MatchInfo : BaseParameter.MatchInfo
        {
            private readonly BaseParameter<T> _baseParameter;
            
            internal MatchInfo(BaseParameter<T> baseParameter, string argument, bool isValid, string[] autocompleteSuggestions): base(argument, isValid, autocompleteSuggestions)
            {
                _baseParameter = baseParameter;
            }

            public bool TryGetValue(out T value) => _baseParameter.TryGetValue(Argument, out value);

            public override bool TryGetValue(out object value)
            {
                bool result = TryGetValue(out T tValue);
                value = tValue;

                return result;
            }
        }

        protected BaseParameter(string name, string description, bool isRequired) : base(name, description, isRequired)
        {
        }
    }
}