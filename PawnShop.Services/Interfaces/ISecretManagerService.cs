namespace PawnShop.Services.Interfaces
{
    public interface ISecretManagerService
    {
        bool GetValue<T>(string key, out string value) where T : class;
    }
}