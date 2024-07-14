using System;

namespace Tactile.Console.Commands
{
    public class Command: BaseCommandWithParameters
    {
        private readonly Action<Console, ParsedArguments> _execute;
        public Command(string name, string description, Action<Console, ParsedArguments> execute) : base(name,
            description)
        {
            _execute = execute;
        }

        protected override void Execute(Console console, ParsedArguments arguments) => _execute(console, arguments);
    }
    
    public class Command<TFirst>: BaseCommandWithParameters<TFirst>
    {
        private readonly Action<Console, ParsedArguments> _execute;
        public Command(string name, string description, Parameters.BaseParameter<TFirst> firstParameter, Action<Console, ParsedArguments> execute) : base(name,
            description, firstParameter)
        {
            _execute = execute;
        }

        protected override void Execute(Console console, ParsedArguments arguments) => _execute(console, arguments);
    }
}