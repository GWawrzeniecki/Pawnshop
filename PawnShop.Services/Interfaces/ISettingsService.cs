namespace PawnShop.Services.Interfaces
{
    public interface ISettingsService<T> where T : class
    {
        public T LoadSettings();
        public void SaveSettings(T settings);
        public bool IsSettingsExist();

    }
}