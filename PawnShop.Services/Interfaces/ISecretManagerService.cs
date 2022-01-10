namespace PawnShop.Services.Interfaces
{
    public interface ISecretManagerService
    {
        bool TryToGetValue<T>(string key, out string value) where T : class;
    }
}