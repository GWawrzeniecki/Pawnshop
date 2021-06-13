using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using iText.Forms;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using PawnShop.Services.Interfaces;

namespace PawnShop.Services.Implementations
{
    public class PdfService : IPdfService
    {

        public void FillPdfForm(string pdfPath, string pdfSavePath, (string textFieldName, string textFieldValue)[] replaceValueTuples)
        {
            if (pdfPath == null) throw new ArgumentNullException(nameof(pdfPath));
            if (pdfSavePath == null) throw new ArgumentNullException(nameof(pdfSavePath));
            if (replaceValueTuples == null) throw new ArgumentNullException(nameof(replaceValueTuples));

            if (!File.Exists(pdfPath)) throw new FileNotFoundException($"Plik: {pdfPath} nie istnieje.");

            var pdfDoc = new PdfDocument(new PdfReader(pdfPath), new PdfWriter(pdfSavePath));
            var form = PdfAcroForm.GetAcroForm(pdfDoc, true);


            foreach (var (textFieldName, textFieldValue) in replaceValueTuples)
            {
                form.GetField(textFieldName)?.SetValue(textFieldValue);
            }

            pdfDoc.Close();
        }

        public void PrintPdf(string pdfPath, short copies)
        {
            var dialog = new PrintDialog();

            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value) return;

            var printProcessInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                Verb = "print",
                CreateNoWindow = true,
                FileName = pdfPath,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process printProcess = new Process { StartInfo = printProcessInfo };
            printProcess.Start();
        }
    }


}
