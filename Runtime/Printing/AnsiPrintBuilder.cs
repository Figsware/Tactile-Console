using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tactile.Console.Printing
{
    public class AnsiPrintBuilder: BasePrintBuilder
    {
        public const string AnsiEscapeCode = "\u001b";
        public static readonly string ClearEscapeCode = GetAnsiEscapeCodeString("2J") + GetAnsiEscapeCodeString("H");
        
        private Stack<string> _ansiEscapeCodes = new();

        public override void Clear()
        {
            base.Clear();
            _ansiEscapeCodes.Clear();
        }

        protected override void PrepareBuildString()
        {
            ResetFormat();
        }

        public override BasePrintBuilder PopFormat()
        {
            if (_ansiEscapeCodes.Count == 0) return this;
            
            _ansiEscapeCodes.Pop();
            if (_ansiEscapeCodes.TryPeek(out var prev))
            {
                AppendString(prev);
            }
            else
            {
                ResetFormat();
            }
            return this;
        }

        public override BasePrintBuilder ResetFormat()
        {
            return AppendString(GetAnsiEscapeCodeString("0m"));
        }

        public override BasePrintBuilder AppendColor(Color color)
        {
            var r = Mathf.RoundToInt(color.r * 255);
            var g = Mathf.RoundToInt(color.g * 255);
            var b = Mathf.RoundToInt(color.b * 255);
            return PushAnsiEscapeCode($"38;2;{r};{g};{b}m");
        }

        public override BasePrintBuilder AppendFontStyle(FontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case FontStyle.Bold:
                    PushAnsiEscapeCode("1m");
                    break;
                
                case FontStyle.Italic:
                    PushAnsiEscapeCode("3m");
                    break;
                
                case FontStyle.BoldAndItalic:
                    PushAnsiEscapeCode("1m");
                    PushAnsiEscapeCode("3m");
                    break;
            }

            return this;
        }

        private BasePrintBuilder PushAnsiEscapeCode(string code)
        {
            var ansi = GetAnsiEscapeCodeString(code);
            _ansiEscapeCodes.Push(ansi);
            return AppendString(ansi);
        }

        private static string GetAnsiEscapeCodeString(string code)
        {
            return $"{AnsiEscapeCode}[{code}";
        }

        public static string PrintLineAndRestoreCursor(string line, string prefix)
        {
            return $"\r{GetAnsiEscapeCodeString("2K")}{line}\n{prefix}";
        }

        public static string ClearSequence(string prefix)
        {
            return ClearEscapeCode + $"{prefix}";
        }
    }
}