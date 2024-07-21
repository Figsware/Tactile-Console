using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Tactile.Console.Parameters;
using UnityEngine;

namespace Tactile.Console.Commands
{
    public abstract class BaseCommandWithParameters: BaseCommand
    {
        public readonly BaseParameter[] Parameters;

        public bool HasParameters => Parameters.Length > 0;
        public bool HasRestParameter => Parameters.Length > 0 && Parameters[^1] is RestParameter;

        protected BaseCommandWithParameters(string name, string description, params BaseParameter[] parameters): base(name, description)
        {
            if (parameters.Where((parameter, i) => parameter is RestParameter && i != parameters.Length - 1).Any())
            {
                throw new ArgumentException("Rest parameter must be at the last argument");
            }

            Parameters = parameters;
        }

        protected abstract void Execute(Console console, ParsedArguments arguments);
        
        public override void Execute(Console console, string body)
        {
            var stringArgs = ParseStringArguments(body, 
                Parameters.Length - (HasRestParameter ? 1 : 0), out var rest);
            var parsedArguments = ParseArguments(console, stringArgs, rest);
            if (parsedArguments == null)
                return;

            Execute(console, parsedArguments);
        }

        private ParsedArguments ParseArguments(Console console, string[] arguments, string rest)
        {
            var parsedArguments = new List<(string name, object value)>();
            var successful = true;
            var totalArgs = arguments.Length + (string.IsNullOrEmpty(rest) ? 0 : 1);
            for (int i = 0; i < Parameters.Length && successful; i++)
            {
                var param = Parameters[i];
                
                if (i < totalArgs)
                {
                    var arg = param is RestParameter ? rest : arguments[i];
                    var match = param.Match(arg);
                    if (match.TryGetValue(out var value))
                    {
                        parsedArguments.Add((param.Name, value));
                    }
                    else
                    {
                        console.PrintError($"Failed to parse argument: {param.Name}");
                        successful = false;
                    }
                }
                else if (param.IsRequired)
                {
                    console.PrintError($"Missing required argument: {param.Name}");
                    successful = false;
                }
                else
                {
                    parsedArguments.Add((param.Name, null));
                }
            }

            return successful ? new ParsedArguments(parsedArguments.ToArray(), rest) : null;
        }
        
        public class ParsedArguments
        {
            public readonly (string name, object value)[] Arguments;
            public readonly string Rest;
            public object this[int index] => Arguments[index].value;
            public object this[string name] => Arguments.First(a => a.name.Equals(name)).value;

            public ParsedArguments((string name, object value)[] arguments, string rest)
            {
                Arguments = arguments;
                Rest = rest;
            }

            protected ParsedArguments(ParsedArguments other)
            {
                Arguments = other.Arguments;
                Rest = other.Rest;
            }
        }

        public override string ToString()
        {
            var description = string.IsNullOrEmpty(Description) ? "No description provided." : Description;
            var parameters = string.Join(" ", Parameters.Select(p => p.IsRequired ? $"({p.Name})" : $"[{p.Name}]").ToArray());
            return $"{Name} {parameters}: {description}";
        }
        
        private static readonly Regex ParameterRegex = new (@"(?:""((?:\\""|[^""])+)""|((?:\\""|[^\s""])+))");
        private static string[] ParseStringArguments(string input, int numArguments, out string rest)
        {
            if (numArguments == 0)
            {
                rest = input;
                return Array.Empty<string>();
            }
            
            var matches = ParameterRegex.Matches(input);
            var args = matches
                .Select(m => 
                    m.Groups
                        .Skip(1)
                        .First(g => !string.IsNullOrEmpty(g.Value))
                        .Value)
                .ToArray();
            
            // Calculate the rest of the string
            if (matches.Count >= numArguments)
            {
                var lastMatch = matches[numArguments - 1];
                var index = lastMatch.Index + lastMatch.Length + 1;
                rest = index < input.Length ? input[index..] : string.Empty;
            }
            else
            {
                rest = null;
            }
            
            return args;
        }
    }
}