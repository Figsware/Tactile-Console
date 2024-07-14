using UnityEditor;

namespace Tactile.Console.Commands
{
    [GlobalCommand]
    public class ExitCommand : BaseCommandWithParameters
    {
        public ExitCommand() : base("exit", "Exits the application.")
        {
        }

        protected override void Execute(Console console, ParsedArguments arguments)
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}