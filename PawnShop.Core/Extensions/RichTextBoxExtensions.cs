 using System;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PawnShop.Core.Extensions
{
    public static class RichTextBoxExtensions
    {
        public static string GetText(this RichTextBox richTextBox)
        {
            if (richTextBox?.Document == null) throw new ArgumentNullException(nameof(richTextBox));


            var document = richTextBox.Document;
            return new TextRange(document.ContentStart, document.ContentEnd).Text;
        }
    }
}