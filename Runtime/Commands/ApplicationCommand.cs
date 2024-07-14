using System;
using System.Reflection;
using Tactile.Console.Parameters;
using UnityEngine;

namespace Tactile.Console.Commands
{
    [GlobalCommand]
    public class ApplicationCommand: BaseCommandWithParameters
    {
        public ApplicationCommand() : base("application", "Lists information about the application") { }

        protected override void Execute(Console console, ParsedArguments arguments)
        {
            var properties = typeof(Application).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var propertyInfo in properties)
            {
                var name = propertyInfo.Name;
                var value = propertyInfo.GetValue(null, null);
                console.Print((p,f) => p + f.PrimaryColor + name + ": " + f.TextColor + value);
            }
        }
    }
}