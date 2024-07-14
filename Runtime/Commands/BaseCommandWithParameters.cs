using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Tactile.Console.Parameters;

namespace Tactile.Console.Commands
{
    public abstract class BaseCommandWithParameters: BaseCommand
    {
        public readonly BaseParameter[] Parameters;

        public bool HasParameters => Parameters.Length > 0;

        protected BaseCommandWithParameters(string name, string description, params BaseParameter[] parameters): base(name, description)
        {
            Parameters = parameters;
        }

        protected abstract void Execute(Console console, ParsedArguments arguments);
        
        public override void Execute(Console console, string body)
        {
            var stringArgs = ParseStringArguments(body);
            var parsedArguments = ParseArguments(console, stringArgs);
            if (parsedArguments == null)
                return;

            Execute(console, parsedArguments);
        }

        private ParsedArguments ParseArguments(Console console, string[] arguments)
        {
            var parsedArguments = new List<(string name, object value)>();
            var successful = true;
            for (int i = 0; i < Parameters.Length && successful; i++)
            {
                var param = Parameters[i];
                if (i >= arguments.Length)
                {
                    if (param.IsRequired)
                    {
                        console.PrintError($"Missing required argument: {param.Name}");
                        successful = false;
                    }
                    else
                    {
                        parsedArguments.Add((param.Name, null));
                    }
                }
                else
                {
                    var arg = arguments[i];
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
            }

            return successful ? new ParsedArguments(parsedArguments.ToArray()) : null;
        }
        
        public class ParsedArguments
        {
            public readonly (string name, object value)[] Arguments;
            public object this[int index] => Arguments[index].value;
            public object this[string name] => Arguments.First(a => a.name.Equals(name)).value;

            public ParsedArguments((string name, object value)[] arguments)
            {
                Arguments = arguments;
            }

            protected ParsedArguments(ParsedArguments other)
            {
                Arguments = other.Arguments;
            }
        }

        public override string ToString()
        {
            var description = string.IsNullOrEmpty(Description) ? "No description provided." : Description;
            var parameters = string.Join(" ", Parameters.Select(p => p.IsRequired ? $"({p.Name})" : $"[{p.Name}]").ToArray());
            return $"{Name} {parameters}: {description}";
        }

        protected static Action<ParsedArguments> CreateTypedExecutor<T>(Func<ParsedArguments, T> converter,
            Action<T> onExecute) where T: ParsedArguments
        {
            return info => onExecute(converter(info));
        }
        
        private static readonly Regex ParameterRegex = new (@"(?:""((?:\\""|[^""])+)""|((?:\\""|[^\s""])+))");
        public static string[] ParseStringArguments(string input)
        {
            var matches = ParameterRegex.Matches(input);
            var args = matches
                .Select(c => c.Groups
                    .First(g => !string.IsNullOrEmpty(g.Value))
                    .Value
                )
                .ToArray();

            return args;
        }
    }
}