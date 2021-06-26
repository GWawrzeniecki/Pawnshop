﻿using iText.Forms;
using iText.Kernel.Pdf;
using PawnShop.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace PawnShop.Services.Implementations
{
    public class PdfService : IPdfService
    {

        public void FillPdfForm(string pdfPath, string pdfSavePath, (string textFieldName, string textFieldValue)[] replaceValueTuples)
        {
            if (string.IsNullOrEmpty(pdfPath)) throw new ArgumentException($"'{nameof(pdfPath)}' cannot be null or empty.", nameof(pdfPath));
            if (string.IsNullOrEmpty(pdfSavePath)) throw new ArgumentException($"'{nameof(pdfSavePath)}' cannot be null or empty.", nameof(pdfSavePath));
            if (replaceValueTuples is null) throw new ArgumentNullException(nameof(replaceValueTuples));

            if (!File.Exists(pdfPath)) throw new FileNotFoundException($"Plik: {pdfPath} nie istnieje lub nie maasz do niego uprawnień.");

            var pdfDoc = new PdfDocument(new PdfReader(pdfPath), new PdfWriter(pdfSavePath));
            var form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            foreach (var (textFieldName, textFieldValue) in replaceValueTuples)
            {
                if (textFieldValue != null)
                    form.GetField(textFieldName)?.SetValue(textFieldValue);
            }

            pdfDoc.Close();
        }

        public void PrintPdf(string pdfPath)
        {
            if (string.IsNullOrEmpty(pdfPath)) throw new ArgumentException($"'{nameof(pdfPath)}' cannot be null or empty.", nameof(pdfPath));

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
