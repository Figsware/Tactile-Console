using System;
using System.Linq;
using Tactile.Console.Printing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tactile.Console.Commands
{
    [GlobalCommand]
    public class HelpCommand: BaseCommandWithParameters
    {
        public HelpCommand() : base("help", "Shows a list of all available commands.")
        {
        }

        protected override void Execute(Console console, ParsedArguments arguments)
        {
           PrintHelpForCommands(console, console.GetCommands());
        }

        public static void PrintHelpForCommands(Console console, BaseCommand[] commands, string namePrefix = "")
        {
            foreach (var command in commands)
            {
                console.Print((p, f) => p + 
                    p.With(f.PrimaryColor, p => p + namePrefix + command.Name + 
                        p.With(f.SecondaryColor, p => PrintCommandArguments(p, command)))
                    + ": "  + command.Description);

                if (command is BaseCommandGroup commandGroup)
                {
                    PrintHelpForCommands(console, commandGroup.GetSubcommands(), $"{namePrefix}{command.Name} ");
                }
            }
        }

        private static BasePrintBuilder PrintCommandArguments(BasePrintBuilder p, BaseCommand command)
        {
            if (command is not BaseCommandWithParameters { HasParameters: true } parameterCommand)
                return p;
            
            var parameters =
                string.Join(' ',
                    parameterCommand.Parameters.Select(pm => pm.IsRequired ? $"<{pm.Name}>" : $"({pm.Name})"));
            return p + " " + parameters;
        }
    }
}