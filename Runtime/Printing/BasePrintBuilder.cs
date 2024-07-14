using System;
using System.Text;
using UnityEngine;

namespace Tactile.Console.Printing
{
    public abstract class BasePrintBuilder
    {
        public PrintFormat Format;
        private readonly StringBuilder _builder = new();

        protected BasePrintBuilder()
        {
            Format = new PrintFormat();
        }

        protected BasePrintBuilder(PrintFormat format)
        {
            Format = format;
        }

        public BasePrintBuilder Print(Func<BasePrintBuilder, BasePrintBuilder> print) => print(this);

        protected abstract void PrepareBuildString();

        #region With

        public BasePrintBuilder With(Color color, Func<BasePrintBuilder, BasePrintBuilder> print) => AppendColor(color).Print(print).PopFormat();

        public BasePrintBuilder With(FontStyle fontStyle, Func<BasePrintBuilder, BasePrintBuilder> print) =>
            AppendFontStyle(fontStyle).Print(print).PopFormat();

        #endregion
        
        #region Format

        public abstract BasePrintBuilder PopFormat();

        public abstract BasePrintBuilder ResetFormat();
        
        #endregion

        #region Append

        public abstract BasePrintBuilder AppendColor(Color color);

        public abstract BasePrintBuilder AppendFontStyle(FontStyle fontStyle);

        public BasePrintBuilder AppendString(string text)
        {
            _builder.Append(text);
            return this;
        }

        public BasePrintBuilder AppendBoolean(bool value)
        {
            AppendColor(value ? Format.TrueColor : Format.FalseColor);
            AppendString(value.ToString());
            return this;
        }

        public BasePrintBuilder AppendInteger(int value)
        {
            AppendColor(Format.NumberColor);
            AppendString(value.ToString());
            return this;
        }

        public BasePrintBuilder AppendObject(object obj)
        {
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

        #region Operators

        public static BasePrintBuilder operator +(BasePrintBuilder pb, Color color) => pb.AppendColor(color);

        public static BasePrintBuilder operator +(BasePrintBuilder pb, FontStyle fontStyle) =>
            pb.AppendFontStyle(fontStyle);

        public static BasePrintBuilder operator +(BasePrintBuilder pb, bool value) => pb.AppendBoolean(value);

        public static BasePrintBuilder operator +(BasePrintBuilder pb, string text) => pb.AppendString(text);

        public static BasePrintBuilder operator +(BasePrintBuilder pb, BasePrintBuilder pb2) =>
            pb == pb2 ? pb : pb.AppendString(pb2.ToString());

        public static BasePrintBuilder operator +(BasePrintBuilder pb, object obj) => pb.AppendObject(obj);

        #endregion

        public string Build()
        {
            PrepareBuildString();
            var str = _builder.ToString();
            Clear();
            
            return str;
        }

        public virtual void Clear()
        {
            _builder.Clear();
        }
    }
}