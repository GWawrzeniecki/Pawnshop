using System.Threading.Tasks;

namespace PawnShop.Services.Interfaces
{
    public interface IPdfService
    {
        public void FillPdfForm(string pdfPath, string pdfSavePath, (string textFieldName, string textFieldValue)[] replaceValueTuples);
        public void PrintPdf(string pdfPath);
        public Task FillPdfFormAsync(string pdfPath, string pdfSavePath, (string textFieldName, string textFieldValue)[] replaceValueTuples);
        public Task PrintPdfAsync(string pdfPath);
        public bool CheckIfPdfIsFillAble(string pdfPath);
        public Task<bool> CheckIfPdfIsFillAbleAsync(string pdfPath);

    }
}