using PawnShop.Services.Interfaces;

namespace IntegrationTests.Base
{
    public class FakeMessageBoxService : IMessageBoxService
    {
        public string LastMessage { get; set; }
        public string LastErrorMessage { get; set; }
        public void Show(string message, string title)
        {
            LastMessage = message;
        }

        public void ShowError(string message, string title)
        {
            LastErrorMessage = message;
        }
    }
}