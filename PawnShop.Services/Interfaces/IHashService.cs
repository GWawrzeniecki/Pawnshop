namespace PawnShop.Services.Interfaces
{
    public interface IHashService
    {
        string Hash(string password);

        bool Check(string hash, string password);
    }
}