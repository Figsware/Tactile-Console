using UnityEngine;

namespace Tactile.Console.Printing
{
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