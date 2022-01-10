namespace PawnShop.Services.Interfaces
{
    public interface IEnvironmentVariableService
    {
        public bool TryToGetValue(string name, out string value);
    }
}