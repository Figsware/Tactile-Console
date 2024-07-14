using System;
using Tactile.Console.Interfaces;
using UnityEngine;

namespace Tactile.Console.Components
{

    public class IMGUIConsoleWindow : MonoBehaviour
    {
        public Color backgroundColor = Color.black;
        public bool showConsole;
        public Rect windowRect;

        private Console _console;
        private IMGUIConsole _imguiConsole;
        
        private void Awake()
        {
            _console = new Console();
            _imguiConsole = new IMGUIConsole();
        }

        private void OnValidate()
        {
            _imguiConsole ??= new IMGUIConsole();
        }

        private void OnGUI()
        {
            if (!showConsole) return;
            GUI.backgroundColor = backgroundColor;
            var windowStyle = new GUIStyle(GUI.skin.window);
            windowRect = GUILayout.Window(0, windowRect, DrawWindow, "Console", windowStyle);
        }

        private void DrawWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, windowRect.width, 20));
            _imguiConsole.OnGUI();
        }
    }
}