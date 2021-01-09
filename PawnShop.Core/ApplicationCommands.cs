using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

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
