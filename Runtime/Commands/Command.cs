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
    
    public class Command<TFirst, TSecond>: BaseCommandWithParameters<TFirst, TSecond>
    {
        private readonly Action<Console, ParsedArguments> _execute;
        public Command(string name, string description, Parameters.BaseParameter<TFirst> firstParameter, Parameters.BaseParameter<TSecond> secondParameter, Action<Console, ParsedArguments> execute) : base(name,
            description, firstParameter, secondParameter)
        {
            _execute = execute;
        }

        protected override void Execute(Console console, ParsedArguments arguments) => _execute(console, arguments);
    }
    
    public class Command<TFirst, TSecond, TThird>: BaseCommandWithParameters<TFirst, TSecond, TThird>
    {
        private readonly Action<Console, ParsedArguments> _execute;
        public Command(string name, string description, Parameters.BaseParameter<TFirst> firstParameter, Parameters.BaseParameter<TSecond> secondParameter, Parameters.BaseParameter<TThird> thirdParameter, Action<Console, ParsedArguments> execute) : base(name,
            description, firstParameter, secondParameter, thirdParameter)
        {
            _execute = execute;
        }

        protected override void Execute(Console console, ParsedArguments arguments) => _execute(console, arguments);
    }
    
    public class Command<TFirst, TSecond, TThird, TFourth>: BaseCommandWithParameters<TFirst, TSecond, TThird, TFourth>
    {
        private readonly Action<Console, ParsedArguments> _execute;
        public Command(string name, string description, Parameters.BaseParameter<TFirst> firstParameter, Parameters.BaseParameter<TSecond> secondParameter, Parameters.BaseParameter<TThird> thirdParameter, Parameters.BaseParameter<TFourth> fourthParameter, Action<Console, ParsedArguments> execute) : base(name,
            description, firstParameter, secondParameter, thirdParameter, fourthParameter)
        {
            _execute = execute;
        }

        protected override void Execute(Console console, ParsedArguments arguments) => _execute(console, arguments);
    }
}