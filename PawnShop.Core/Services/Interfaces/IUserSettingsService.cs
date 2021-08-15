namespace PawnShop.Core.Services.Interfaces
{
    public interface IUserSettingsService
    {
        public T GetValue<T>(string key);
        public void SaveValue(string key, string value);
    }
}