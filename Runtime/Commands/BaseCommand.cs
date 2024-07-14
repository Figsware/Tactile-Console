using System.Linq;
using System.Text.RegularExpressions;

namespace Tactile.Console.Commands
{
    public abstract class BaseCommand
    {
        public readonly string Name;
        public readonly string Description;
        
        protected BaseCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public abstract void Execute(Console console, string body);

        public static (string name, string body)? ParseCommand(string body)
        {
            if (body.Length == 0)
                return null;
            
            var nameAndArgs = body.Split(' ', 2);
            return nameAndArgs.Length switch
            {
                0 => null,
                1 => (nameAndArgs[0], string.Empty),
                _ => (nameAndArgs[0], nameAndArgs[1])
            };
        }
    }
}