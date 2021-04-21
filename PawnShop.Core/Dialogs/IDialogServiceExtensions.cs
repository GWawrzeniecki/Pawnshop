using Prism.Services.Dialogs;
using System;
using PawnShop.Business.Models;
using PawnShop.Core.Enums;

namespace PawnShop.Core.Dialogs
{
    public static class IDialogServiceExtensions
    {
        public static void ShowLoginDialog(this IDialogService dialogService, Action<IDialogResult> callBack)
        {
            dialogService.ShowDialog(DialogNames.LoginDialog, callBack);
        }

        public static void ShowNotificationDialog(this IDialogService dialogService, string title, string message, Action<IDialogResult> callBack)
        {
            var dialogParameters = new DialogParameters();
            dialogParameters.Add("title", title);
            dialogParameters.Add("message", message);
            dialogService.ShowDialog(DialogNames.NotificationDialog, dialogParameters, callBack);
        }

        public static void ShowAddClientDialog(this IDialogService dialogService, string title, ClientMode mode, Action<IDialogResult> callBack, Client client = null)
        {
            var dialogParameters = new DialogParameters();
            dialogParameters.Add("title", title);
            dialogParameters.Add("client", client);
            dialogParameters.Add("mode", mode);
            dialogService.Show(DialogNames.AddClientDialog, dialogParameters, callBack);
        }
    }
}