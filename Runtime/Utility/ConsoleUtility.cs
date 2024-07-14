using System;
using UnityEngine;

namespace Tactile.Console
{
    public static class ConsoleUtility
    {
        public static Color FromHex(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out var color))
                return color;

            throw new ArgumentException("Not a valid hex string!");
        }

        public static void PrintLine(this Console console) => console.Print(string.Empty);
    }
}