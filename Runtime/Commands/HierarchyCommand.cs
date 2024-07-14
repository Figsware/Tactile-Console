using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tactile.Console.Commands
{
    [GlobalCommand]
    public class HierarchyCommand : BaseCommandWithParameters
    {
        private const int MaxDepth = 8;
        private const int TabSpaces = 2;
        public HierarchyCommand(): base("hierarchy", "Prints out information about the hierarchy")
        {
        }

        protected override void Execute(Console console, ParsedArguments arguments)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);

                console.Print(scene.name);
                console.Print("---");
                foreach (var go in scene.GetRootGameObjects())
                {
                    LogGameObject(console, go, 0);
                }
                
                console.PrintLine();
            }
        }

        private static void LogGameObject(Console console, GameObject go, int depth)
        {
            var goStr = new string(' ', TabSpaces * depth) + go.name;

            if (!go.gameObject.activeInHierarchy)
            {
                console.Print((p, f) => p + FontStyle.Italic + f.DisabledColor + goStr);
            }
            else
            {
                console.Print(goStr);
            }
            
            if (depth >= MaxDepth) return;
            for (var i = 0; i < go.transform.childCount; i++)
            {
                var childGo = go.transform.GetChild(i).gameObject;
                LogGameObject(console, childGo, depth + 1);
            }
        }
    }
}