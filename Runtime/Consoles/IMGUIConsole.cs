using System;
using System.Text;
using Tactile.Console.Commands;
using Tactile.Console.Parameters;
using UnityEngine;

namespace Tactile.Console.Interfaces
{
    public class IMGUIConsole : Console
    {
        public float CommandBarHeight = 20f;
        public int FontSize = 12;

#if UNITY_EDITOR
        public bool UseEditorGUI = false;
#endif
        
        private readonly StringBuilder _buffer = new();
        private string _input;
        private Vector2 _scrollPos;
        
        private readonly string _inputFieldControlName;
        public event Action OnRepaint;

        public static Font MonospaceFont => GetMonospaceFont();
        private static Font _monospaceFont;
        

        public IMGUIConsole()
        {
            AddConsoleCommand(new Command("clear", "Clears the console", (_, _) =>
            {
                _buffer.Clear();
            }));
            
            AddConsoleCommand(new Command<int>("fontsize", "Sets the font size for the console", new IntegerParameter("fontsize", "The fontsize to set the console to"),
                (_, args) =>
            {
                FontSize = args.Arg1;
            }));
            
            AddConsoleCommand(new Command<bool>("richtext", "Sets whether richtext is enabled", new BooleanParameter("enabled", "Whether richtext is enabled"),
                (_, args) =>
                {
                    Format.UseRichText = args.Arg1;
                }));
            
            _inputFieldControlName = Guid.NewGuid().ToString();
            OnPrintLine += AddLineToBuffer;
        }

        public void OnGUI()
        {
            GUI.skin.font = GetMonospaceFont();

            GUILayout.BeginVertical();
            
            _scrollPos = GUILayout.BeginScrollView(_scrollPos);   
            
            var consoleTextStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.LowerLeft,
                richText = true,
                fontSize = FontSize
            };

            var inputStyle = new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleLeft,
                richText = false
            };
            
            GUILayout.Label(_buffer.ToString(), consoleTextStyle, GUILayout.ExpandHeight(true));
            
            GUILayout.EndScrollView();
            
            GUILayout.BeginHorizontal();
            
            var wasEnterKeyHitInInput = Event.current.type == EventType.KeyUp &&
                                        Event.current.keyCode == KeyCode.Return &&
                                        GUI.GetNameOfFocusedControl() == _inputFieldControlName;
            GUI.SetNextControlName(_inputFieldControlName);
            _input = GUILayout.TextField(_input, inputStyle, GUILayout.ExpandWidth(true), GUILayout.Height(CommandBarHeight));

            var wasButtonClicked = GUILayout.Button("Execute", GUILayout.ExpandWidth(false), GUILayout.Height(CommandBarHeight));
            if (wasEnterKeyHitInInput || wasButtonClicked)
            {
                Print((p, f) => p + f.InputColor + _input);
                Execute(_input);
                
                _input = string.Empty;
                OnRepaint?.Invoke();
            }

            if (GUILayout.Button("Clear", GUILayout.ExpandWidth(false), GUILayout.Height(CommandBarHeight)))
            {
                _buffer.Clear();
                OnRepaint?.Invoke();
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        
        private void AddLineToBuffer(string line)
        {
            _buffer.AppendLine(line);
            _scrollPos = new Vector2(0, float.PositiveInfinity);
            OnRepaint?.Invoke();
        }

        public static Font GetMonospaceFont()
        {
            if (!_monospaceFont)
            {
                _monospaceFont = Font.CreateDynamicFontFromOSFont("Consolas", 12);
            }

            return _monospaceFont;
        }
    }
}