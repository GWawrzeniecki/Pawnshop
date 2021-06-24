namespace PawnShop.Services.Interfaces
{
    public interface IConfigurationService
    {
        public T GetValue<T>(string key);
    }
}
