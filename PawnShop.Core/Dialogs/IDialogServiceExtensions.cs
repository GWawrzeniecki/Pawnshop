using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Core.Dialogs
{
    public static class IDialogServiceExtensions
    {
       
        public static void ShowLogInDialog(this IDialogService dialogService, Action<IDialogResult> callBack)
        {
            dialogService.ShowDialog("LoginDialog", callBack);
        }

    }
}
