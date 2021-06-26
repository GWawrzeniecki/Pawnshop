namespace PawnShop.Core.SharedVariables
{
    public interface IConfigData
    {
        public int VatPercent { get; set; }
        public string DealDocumentPath { get; set; }
        public string DealDocumentsFolderPath { get; set; }

    }
}
