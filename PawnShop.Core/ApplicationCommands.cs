using Prism.Commands;

namespace PawnShop.Core
{
    public interface IApllicationCommands
    {
        CompositeCommand NavigateCommand { get; }
    }

    public class ApplicationCommands : IApllicationCommands
    {
        public CompositeCommand NavigateCommand { get; } = new CompositeCommand();
    }
}