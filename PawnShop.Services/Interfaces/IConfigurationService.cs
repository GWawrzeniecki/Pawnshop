namespace PawnShop.Services.Interfaces
{
    public interface IConfigurationService
    {
        public T GetValueFromAppConfig<T>(string key);
        public void SaveValueInAppConfig(string key, string value);
    }
}
