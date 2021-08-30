namespace PawnShop.Core.SharedVariables
{
    public class UserSettings : IUserSettings
    {
        public string ThemeName { get; set; }
        public int VatPercent { get; set; }
        public string DealDocumentPath { get; set; }
        public string DealDocumentsFolderPath { get; set; }
        public int AutomaticSearchingEndedContractsDay { get; set; }
    }
}