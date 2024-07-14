using Tactile.Console.Parameters;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tactile.Console.Commands
{
    [GlobalCommand]
    public class ScenesCommand : BaseCommandGroup
    {
        public ScenesCommand() : base("scenes", "Manages scenes", new SetActiveSceneCommand())
        {
        }

        protected override void Execute(Console console)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                PrintScene(console, scene, i);
            }
        }

        private static void PrintScene(Console console, Scene scene, int index)
        {
            var color = Color.gray;
            if (SceneManager.GetActiveScene().Equals(scene))
            {
                color = console.Format.SecondaryColor;
            }
            else if (scene.isLoaded)
            {
                color = console.Format.PrimaryColor;
            }
            console.Print(p => p + color + $"[{index}] {scene.name}");
        }

        public class SetActiveSceneCommand : BaseCommandWithParameters<int>
        {
            public SetActiveSceneCommand() : base("set_active", "Sets the active scene", new IntegerParameter("index", "The index of the scene to make active"))
            {
            }

            protected override void Execute(Console console, ParsedArguments arguments)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneAt(arguments.Arg1));
            }
        } 
    }
}