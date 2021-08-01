namespace PawnShop.Core.Constants
{
    public static class Constants
    {
        //
        public const string PepperAesKeySecret = "PepperAesKey";
        public const string IterationsKeySecret = "Iterations";
        public const string DBSchemaName = "Pawnshop";

        //Contract Item Categories
        public const string Laptop = "Laptop";

        //App.cfg key names
        public const string VatPercentKey = "VATPercent";
        public const string DealDocumentPath = "DealDocumentPath";
        public const string DealDocumentsFolderPath = "DealDocumentsFolderPath";

        //PaymentType
        public const string CashPaymentType = "Gotówka";
        public const string CreditCardPaymentType = "Karta";

        //ContractStates
        public const string CreatedContractState = "Zalozona";
        public const string RenewContractState = "Przedluzona";
    }
}
