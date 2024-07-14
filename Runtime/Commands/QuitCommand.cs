namespace Tactile.Console.Commands
{
    [GlobalCommand]
    public class QuitCommand : BaseCommandWithParameters
    {
        public QuitCommand() : base("quit", "Quits the application.")
        {
        }

        protected override void Execute(Console console, ParsedArguments arguments)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();   
#endif
        }
    }
}