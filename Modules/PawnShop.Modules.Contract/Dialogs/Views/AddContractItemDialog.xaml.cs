using System.Linq;
using System.Windows.Controls;
using PawnShop.Core.Extensions;
using PawnShop.Modules.Contract.Dialogs.ViewModels;

namespace PawnShop.Modules.Contract.Dialogs.Views
{
    /// <summary>
    /// Interaction logic for AddContractItem
    /// </summary>
    public partial class AddContractItemDialog : UserControl
    {
        public AddContractItemDialog()
        {
            InitializeComponent();
            //this.ContractItemTechnicalConditionRichTextBox.TextChanged += ContractItemTechnicalConditionRichTextBox_TextChanged;
        }

        private void ContractItemTechnicalConditionRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is RichTextBox richTextBox)
            {
                ((AddContractItemDialogViewModel) this.DataContext).ContractItemTechnicalCondition =
                    richTextBox.GetText();
            }
        }
    }
}
