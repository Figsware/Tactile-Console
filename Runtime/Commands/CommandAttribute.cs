using System;
using Tactile.Console.Parameters;

namespace Tactile.Console.Commands
{
    public class CommandAttribute : Attribute
    {
        public readonly string Name;
        public readonly string Description;

        public CommandAttribute(string name)
        {
            Name = name;
            Description = string.Empty;
        }

        public CommandAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}