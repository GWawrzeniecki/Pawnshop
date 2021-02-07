namespace PawnShop.Services.Interfaces
{
    public interface IAesService
    {
        string EncryptString(string key, string plainText);

        string DecryptString(string key, string cipherText);
    }
}