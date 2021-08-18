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

        //PaymentType
        public const string CashPaymentType = "Gotówka";
        public const string CreditCardPaymentType = "Karta";

        //ContractStates
        public const string CreatedContractState = "Zalozona";
        public const string RenewContractState = "Przedluzona";
        public const string BuyBackContractState = "Wykupiona";

        //UserSettings.json names
        public const string ThemeNameSection = "UserSettings:ThemeName";

        //usersettings.json file name
        public const string UserSettingsFileName = "usersettings.json";

        //usersettings.json key names
        public const string VatPercentKey = "VATPercent";
        public const string DealDocumentPathKey = "DealDocumentPath";
        public const string DealDocumentsFolderPathKey = "DealDocumentsFolderPath";
    }
}
