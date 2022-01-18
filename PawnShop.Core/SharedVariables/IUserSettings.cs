namespace PawnShop.Core.SharedVariables
{
    public interface IUserSettings
    {
        public string ThemeName { get; set; }
        public int VatPercent { get; set; }
        public string DealDocumentPath { get; set; }
        public string DealDocumentsFolderPath { get; set; }
    }
}