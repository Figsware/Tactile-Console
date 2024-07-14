using Tactile.Console.Commands;
using UnityEditor;

namespace Tactile.Console.Editor.Commands
{
    [GlobalCommand]
    public class PauseCommand: BaseCommandWithParameters
    {
        public PauseCommand() : base("pause", "Pauses play mode")
        {
        }

        protected override void Execute(Console console, ParsedArguments arguments)
        {
            EditorApplication.isPaused = !EditorApplication.isPaused;
        }
    }
}