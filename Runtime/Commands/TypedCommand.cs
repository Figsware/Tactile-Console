using System;
using Tactile.Console.Commands;
using Tactile.Console.Parameters;

namespace Tactile.Console
{
    public abstract class BaseCommandWithParameters<TFirst> : BaseCommandWithParameters
    {
        public BaseCommandWithParameters(string name, string description, BaseParameter firstBaseParameter)
            : base(name, description, firstBaseParameter)
        {
        }

        protected override void Execute(Console console, BaseCommandWithParameters.ParsedArguments arguments)
        {
            Execute(console, new ParsedArguments(arguments));
        }

        protected abstract void Execute(Console console, ParsedArguments arguments);

        public new class ParsedArguments : BaseCommandWithParameters.ParsedArguments
        {
            public readonly TFirst Arg1;

            internal ParsedArguments(BaseCommandWithParameters.ParsedArguments other) : base(other)
            {
                Arg1 = (TFirst)Arguments[0].value;
            }
        }
    }

    public abstract class BaseCommandWithParameters<TFirst, TSecond> : BaseCommandWithParameters
    {
        public BaseCommandWithParameters(string name, string description, BaseParameter firstBaseParameter,
            BaseParameter secondBaseParameter)
            : base(name, description, firstBaseParameter, secondBaseParameter)
        {
        }
        
        protected override void Execute(Console console, BaseCommandWithParameters.ParsedArguments arguments)
        {
            Execute(console, new ParsedArguments(arguments));
        }
        
        protected abstract void Execute(Console console, ParsedArguments arguments);

        public new class ParsedArguments : BaseCommandWithParameters<TFirst>.ParsedArguments
        {
            public readonly TSecond Arg2;

            internal ParsedArguments(BaseCommandWithParameters.ParsedArguments other) : base(other)
            {
                Arg2 = (TSecond)Arguments[1].value;
            }
        }
    }

    public abstract class BaseCommandWithParameters<TFirst, TSecond, TThird> : BaseCommandWithParameters
    {
        public BaseCommandWithParameters(string name, string description, BaseParameter firstBaseParameter,
            BaseParameter secondBaseParameter, BaseParameter thirdBaseParameter)
            : base(name, description, firstBaseParameter,
                secondBaseParameter, thirdBaseParameter)
        {
        }
        
        protected override void Execute(Console console, BaseCommandWithParameters.ParsedArguments arguments)
        {
            Execute(console, new ParsedArguments(arguments));
        }
        
        protected abstract void Execute(Console console, ParsedArguments arguments);

        public new class ParsedArguments : BaseCommandWithParameters<TFirst, TSecond>.ParsedArguments
        {
            public readonly TThird Arg3;

            internal ParsedArguments(BaseCommandWithParameters.ParsedArguments other) : base(other)
            {
                Arg3 = (TThird)Arguments[2].value;
            }
        }
    }

    public abstract class BaseCommandWithParameters<TFirst, TSecond, TThird, TFourth> : BaseCommandWithParameters
    {
        public BaseCommandWithParameters(string name, string description, BaseParameter firstBaseParameter,
            BaseParameter secondBaseParameter, BaseParameter thirdBaseParameter, BaseParameter fourthBaseParameter)
            : base(name, description, firstBaseParameter, secondBaseParameter, thirdBaseParameter,
                fourthBaseParameter)
        {
        }
        
        protected override void Execute(Console console, BaseCommandWithParameters.ParsedArguments arguments)
        {
            Execute(console, new ParsedArguments(arguments));
        }

        protected abstract void Execute(Console console, ParsedArguments arguments);
        
        public new class ParsedArguments : BaseCommandWithParameters<TFirst, TSecond, TThird>.ParsedArguments
        {
            public readonly TFourth Arg4;

            internal ParsedArguments(BaseCommandWithParameters.ParsedArguments other) : base(other)
            {
                Arg4 = (TFourth)Arguments[3].value;
            }
        }
    }
    
    public abstract class BaseCommandWithParameters<TFirst, TSecond, TThird, TFourth, TFifth> : BaseCommandWithParameters
    {
        public BaseCommandWithParameters(string name, string description, BaseParameter firstBaseParameter,
            BaseParameter secondBaseParameter, BaseParameter thirdBaseParameter, BaseParameter fourthBaseParameter, BaseParameter fifthBaseParameter)
            : base(name, description, firstBaseParameter, secondBaseParameter, thirdBaseParameter, fourthBaseParameter)
        {
        }
        
        protected override void Execute(Console console, BaseCommandWithParameters.ParsedArguments arguments)
        {
            Execute(console, new ParsedArguments(arguments));
        }
        
        protected abstract void Execute(Console console, ParsedArguments arguments);

        public new class ParsedArguments : BaseCommandWithParameters<TFirst, TSecond, TThird, TFourth>.ParsedArguments
        {
            public readonly TFifth Arg5;

            internal ParsedArguments(BaseCommandWithParameters.ParsedArguments other) : base(other)
            {
                Arg5 = (TFifth)Arguments[4].value;
            }
        }
    }
}