using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tactile.Console
{
    public class PrintBuilder
    {
        public PrintFormat Format;
        private readonly StringBuilder _builder = new();
        private readonly Stack<string> _tagStack = new();

        public PrintBuilder()
        {
            Format = new PrintFormat();
        }

        public PrintBuilder(PrintFormat format)
        {
            Format = format;
        }

        public PrintBuilder Print(Func<PrintBuilder, PrintBuilder> print) => print(this);

        #region With
        
        public PrintBuilder WithSize(int withSize, Func<PrintBuilder, PrintBuilder> print) =>
            With(withSize, AppendSize, print);
        
        public PrintBuilder With(Color color, Func<PrintBuilder, PrintBuilder> print) =>
            With(color, AppendColor, print);

        public PrintBuilder With(FontStyle fontStyle, Func<PrintBuilder, PrintBuilder> print) =>
            With(fontStyle, AppendFontStyle, print);

        private static PrintBuilder With<T>(T value, Func<T, PrintBuilder> appendFunction,
            Func<PrintBuilder, PrintBuilder> print) => appendFunction(value).Print(print).PopRichTextTag(); 
        
        #endregion
        
        #region Append
        public PrintBuilder AppendSize(int size) => PushRichTextTag("size", size.ToString());

        public PrintBuilder AppendColor(Color color) => PushRichTextTag("color", $"#{ColorUtility.ToHtmlStringRGB(color)}");

        public PrintBuilder AppendFontStyle(FontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case FontStyle.Bold:
                    PushRichTextTag("b");
                    break;
                
                case FontStyle.Italic:
                    PushRichTextTag("i");
                    break;
                
                case FontStyle.BoldAndItalic:
                    PushRichTextTag("b");
                    PushRichTextTag("i");
                    break;
            }

            return this;
        }

        public PrintBuilder AppendString(string text)
        {
            _builder.Append(text);
            return this;
        }

        public PrintBuilder AppendBoolean(bool value)
        {
            AppendColor(value ? Format.TrueColor : Format.FalseColor);
            AppendString(value.ToString());
            return this;
        }

        public PrintBuilder AppendInteger(int value)
        {
            AppendColor(Format.NumberColor);
            AppendString(value.ToString());
            return this;
        }
        
        public PrintBuilder AppendObject(object obj) {
            switch (obj)
            {
                case bool b:
                    AppendBoolean(b);
                    break;
                case int i:
                    AppendInteger(i);
                    break;
                default:
                    AppendString(obj.ToString());
                    break;
            }
            return this;
        }
        
        #endregion

        public PrintBuilder PopRichTextTag()
        {
            if (!Format.UseRichText) return this;
            var tagName = _tagStack.Pop();
            _builder.Append($"</{tagName}>");
            return this;
        }

        public PrintBuilder PopAllRichTextTags()
        {
            if (!Format.UseRichText) return this;
            while (_tagStack.Count > 0)
                PopRichTextTag();
            return this;
        }

        public PrintBuilder PushRichTextTag(string tagName, string tagArgs = null)
        {
            if (!Format.UseRichText) return this;
            
            _builder.Append(string.IsNullOrEmpty(tagArgs) ? $"<{tagName}>" : $"<{tagName}={tagArgs}>");
            _tagStack.Push(tagName);
            return this;
        }
        
        #region Operators
        
        public static PrintBuilder operator +(PrintBuilder pb, Color color) => pb.AppendColor(color);
        
        public static PrintBuilder operator +(PrintBuilder pb, FontStyle fontStyle) => pb.AppendFontStyle(fontStyle);

        public static PrintBuilder operator +(PrintBuilder pb, bool value) => pb.AppendBoolean(value);
        
        public static PrintBuilder operator +(PrintBuilder pb, string text) => pb.AppendString(text);

        public static PrintBuilder operator +(PrintBuilder pb, PrintBuilder pb2) => pb == pb2 ? pb : pb.AppendString(pb2.ToString());

        public static PrintBuilder operator +(PrintBuilder pb, object obj) => pb.AppendObject(obj);
        
        #endregion
        
        public override string ToString()
        {
            PopAllRichTextTags();
            return _builder.ToString();
        }

        public string BuildStringAndClear()
        {
            var str = ToString();
            Clear();

            return str;
        }

        public void Clear()
        {
            _builder.Clear();
            _tagStack.Clear();
        }
        
        public class PrintFormat
        {
            public bool UseRichText = true;
        
            #region Colors

            public Color PrimaryColor = ConsoleUtility.FromHex("#FF9800");
            public Color SecondaryColor = ConsoleUtility.FromHex("#FF5252");
            public Color TextColor = Color.white;
            public Color TrueColor = Color.green;
            public Color FalseColor = Color.red;
            public Color NumberColor = ConsoleUtility.FromHex("#0288D1");
            public Color WarningColor = Color.yellow;
            public Color ErrorColor = Color.red;
            public Color InputColor = Color.gray;
            public Color DisabledColor = Color.gray;

            #endregion
        }
    }
}