namespace PawnShop.Core.Constants
{
    public static class Constants
    {
        //EnvironmentVariablesNames
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

        //AppConfig Key Names
        public const string IsFirstLaunchAppConfigKeyName = "IsFirstLaunch";

        //FileNames
        public const string UserSettingsFileName = "usersettings.json";
        public const string DealDocumentPdfFileName = "UMOWA KUPNA-SPRZEDAZY_V3-Form.pdf";

        //Views names
        public const string HomeModuleHomeViewName = "Home";

    }
}
