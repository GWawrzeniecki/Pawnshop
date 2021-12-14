namespace PawnShop.Services.Interfaces
{
    public interface IMessageBoxService
    {
        public void Show(string message, string title);
        public void ShowError(string message, string title);
    }
}