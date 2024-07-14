using Tactile.Console.Parameters;
using UnityEditor;
using UnityEngine;

namespace Tactile.Console.Commands
{
    [GlobalCommand]
    public class LogCommand : BaseCommandGroup
    {
        private static readonly BaseCommand[] LogSubcommands = 
        {
            new SetLogEnabledCommand(), 
            new DebugLogCommand("info", "Logs a info/log message to the console", LogType.Log),
            new DebugLogCommand("warning", "Logs a warning message to the console", LogType.Warning),
            new DebugLogCommand("error", "Logs an error message to the console", LogType.Error),
            new DebugLogCommand("assert", "Logs an assert message to the console", LogType.Assert),
            new DebugLogCommand("exception", "Logs an exception message to the console", LogType.Exception)
        };

        public LogCommand() : base("log", LogSubcommands)
        {
        }
        
        private class SetLogEnabledCommand : BaseCommandWithParameters<bool>
        {
            public SetLogEnabledCommand() : base("enabled", "Sets whether to print Unity log messages to the console.",
                new BooleanParameter("enabled", "Whether to print log messages"))
            {
            }

            protected override void Execute(Console console, ParsedArguments arguments)
            {
                console.PrintUnityLogMessages = arguments.Arg1;
            }
        }
        
        private class DebugLogCommand : BaseCommandWithParameters<string>
        {
            private readonly LogType _logType;
            
            public DebugLogCommand(string name, string description, LogType logType) : base(name, description,
                new StringParameter("message", "The message to print", true))
            {
                _logType = logType;
            }

            protected override void Execute(Console console, ParsedArguments arguments)
            {
                Debug.unityLogger.Log(_logType, arguments.Arg1);
            }
        }
    }
}