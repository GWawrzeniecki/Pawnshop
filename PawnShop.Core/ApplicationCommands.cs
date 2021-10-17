using Prism.Commands;

namespace PawnShop.Core
{
    public interface IApplicationCommands
    {
        CompositeCommand NavigateCommand { get; }
        CompositeCommand SetMenuItemCommand { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand NavigateCommand { get; } = new CompositeCommand();
        public CompositeCommand SetMenuItemCommand { get; } = new CompositeCommand();
    }
}