using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tactile.Console.Printing
{
    public class RichTextPrintBuilder : BasePrintBuilder 
    {
        private readonly Stack<string> _tagStack = new();

        public RichTextPrintBuilder(PrintFormat format) : base(format)
        {
            
        }

        public override BasePrintBuilder PopFormat() => PopRichTextTag();

        public override BasePrintBuilder ResetFormat() => PopAllRichTextTags();
        public override BasePrintBuilder AppendColor(Color color) => PushRichTextTag("color", $"#{ColorUtility.ToHtmlStringRGB(color)}");
        public override BasePrintBuilder AppendFontStyle(FontStyle fontStyle)
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

        protected override void PrepareBuildString()
        {
            PopAllRichTextTags();
        }

        public override void Clear()
        {
            base.Clear();
            _tagStack.Clear();
        }

        public RichTextPrintBuilder PushRichTextTag(string tagName, string tagArgs = null)
        {
            if (!Format.UseRichText) return this;
            
            AppendString(string.IsNullOrEmpty(tagArgs) ? $"<{tagName}>" : $"<{tagName}={tagArgs}>");
            _tagStack.Push(tagName);
            return this;
        }
        public RichTextPrintBuilder PopRichTextTag()
        {
            if (!Format.UseRichText) return this;
            var tagName = _tagStack.Pop();
            AppendString($"</{tagName}>");
            return this;
        }

        public RichTextPrintBuilder PopAllRichTextTags()
        {
            if (!Format.UseRichText) return this;
            while (_tagStack.Count > 0)
                PopRichTextTag();
            return this;
        }

    }
}