using PawnShop.Services.Interfaces;
using System.Windows.Controls;
using System.Windows.Media;

namespace PawnShop.Services.Implementations
{
    public class PrintService : IPrintService
    {
        #region IPrintService
        public void PrintVisualElement(Visual visual)
        {
            PrintDialog printDialog = new();
            printDialog.PrintVisual(visual, "Printing visual element from Pawnshop App.");
        }

        #endregion

        #region PrivateMethods


        #endregion


    }
}