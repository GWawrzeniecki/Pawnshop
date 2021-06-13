namespace PawnShop.Services.Interfaces
{
    public interface IPdfService
    {
        public void FillPdfForm(string pdfPath, string pdfSavePath, (string textFieldName, string textFieldValue)[] replaceValueTuples);
        public void PrintPdf(string pdfPath, short copies);
    }
}