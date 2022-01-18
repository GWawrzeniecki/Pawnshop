using PawnShop.Core.Constants;
using PawnShop.Core.SharedVariables;
using System;
using System.IO;

namespace Pawnshop.Setup.Scripts
{
    public class Setup : ISetup
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private string _dealDocumentsFolderPath;
        private readonly string _dealDocumentFileName;

        public Setup()
        {
            //ConfigureNlog(); only when using in installer

            _dealDocumentFileName = Constants.DealDocumentPdfFileName;
        }

        public void ConfigureApplication(string applicationPath)
        {

            if (string.IsNullOrEmpty(applicationPath))
                throw new ArgumentException("Ścieżka do folderu z aplikacją jest pusta.");

            if (!Directory.Exists(applicationPath))
                throw new ApplicationException(
                    "Ścieżka do folderu z aplikacją jest nieprawidłowa.");

            var dealDocumentPath = @$"{applicationPath}\{_dealDocumentFileName}";

            if (!File.Exists(dealDocumentPath))
                throw new ApplicationException(
                    "Plik z szablonem umowy nie istnieje.");

            TryToCreateDealDocumentsFolder();
            TryToSetUserSettings(dealDocumentPath);
        }


        private void TryToCreateDealDocumentsFolder()
        {
            try
            {
                SetDealDocumentsFolderPath();
                if (!Directory.Exists(_dealDocumentsFolderPath))
                    Directory.CreateDirectory(_dealDocumentsFolderPath);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Nie udało się stworzyć folderu na umowy.{Environment.NewLine}{e.Message}");
            }

        }

        private void SetDealDocumentsFolderPath()
        {
            _dealDocumentsFolderPath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Pawnshop\DealDocuments";
        }

        private void TryToSetUserSettings(string dealDocumentPath)
        {

            try
            {
                PawnShop.Services.Interfaces.ISettingsService<UserSettings> userSettingsService = new PawnShop.Services.Implementations.SettingsService<UserSettings>(PawnShop.Core.Constants.Constants.UserSettingsFileName);
                if (!userSettingsService.IsSettingsExist())
                {
                    userSettingsService.SaveSettings(new UserSettings
                    {
                        VatPercent = 23,
                        AutomaticSearchingEndedContractsDay = 14,
                        ThemeName = "Light.Blue",
                        DealDocumentsFolderPath = _dealDocumentsFolderPath,
                        DealDocumentPath = dealDocumentPath
                    });
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Nie udało się zapisać ustawien.{Environment.NewLine}{e.Message}");
            }


        }

        private void ConfigureNlog()
        {


            var configuration = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "C:\\logfile.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            configuration.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logfile);
            configuration.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logconsole);

            NLog.LogManager.Configuration = configuration;
        }
    }
}