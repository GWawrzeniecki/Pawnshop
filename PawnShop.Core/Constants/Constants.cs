namespace PawnShop.Core.Constants
{
    public static class Constants
    {
        //
        public const string PepperKeySecret = "Pepper";
        public const string IterationsKeySecret = "Iterations";

        //Schemas names
        public const string ProceduresSchemaName = "PawnshopApp";

        //Contract Item Categories
        public const string Laptop = "Laptop";

        //PaymentType
        public const string CashPaymentType = "Gotówka";
        public const string CreditCardPaymentType = "Karta";

        //ContractStates
        public const string CreatedContractState = "Założona";
        public const string RenewedContractState = "Przedłużona";
        public const string BoughtBackContractState = "Wykupiona";
        public const string NotBoughtBackContractState = "Niewykupiona";

        //UserSettings.json names
        //public const string ThemeNameSection = "UserSettings:ThemeName";

        //usersettings.json file name
        public const string UserSettingsFileName = "usersettings.json";

        //usersettings.json key names
        //public const string VatPercentKey = "VATPercent";
        //public const string DealDocumentPathKey = "DealDocumentPath";
        //public const string DealDocumentsFolderPathKey = "DealDocumentsFolderPath";
    }
}
