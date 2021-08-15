namespace PawnShop.Services.Interfaces
{
    public interface IConfigurationService
    {
        public T GetValueFromAppConfig<T>(string key);
    }
}
