using System.Collections.Generic;
using System.Linq;

namespace Tactile.Console.Commands
{
    public abstract class BaseCommandGroup : BaseCommand
    {
        protected readonly Dictionary<string, BaseCommand> Subcommands = new();
        

        public BaseCommandGroup(string name, string description, params BaseCommand[] subcommands) : base(name, description)
        {
            foreach (var subcommand in subcommands)
            {
                Subcommands[subcommand.Name] = subcommand;
            }
        }

        public BaseCommand[] GetSubcommands() => Subcommands.Values.OrderBy(c => c.Name).ToArray();

        protected abstract void Execute(Console console);

        public override void Execute(Console console, string command)
        {
            var nameAndBody = ParseCommand(command);
            if (nameAndBody == null)
            {
                Execute(console);
                return;
            }

            var (name, body) = nameAndBody.Value;
            if (Subcommands.TryGetValue(name, out var foundSubCommand))
            {
                foundSubCommand.Execute(console, body);    
            }
            else
            {
                console.PrintError($"Unknown subcommand: {name}");
            }
        }
    }
}