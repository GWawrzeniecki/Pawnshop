using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using Prism.Services.Dialogs;
using System;

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
            var dialogParameters = new DialogParameters { { "title", title }, { "message", message } };
            dialogService.ShowDialog(DialogNames.NotificationDialog, dialogParameters, callBack);
        }

        public static void ShowAddClientDialog(this IDialogService dialogService, string title, ClientMode mode, Action<IDialogResult> callBack)
        {
            var dialogParameters = new DialogParameters { { "title", title }, { "mode", mode } };
            dialogService.ShowDialog(DialogNames.AddClientDialog, dialogParameters, callBack);
        }

        public static void ShowAddClientDialog(this IDialogService dialogService, string title, ClientMode mode, Action<IDialogResult> callBack, Client client)
        {
            var dialogParameters = new DialogParameters { { "title", title }, { "client", client }, { "mode", mode } };
            dialogService.ShowDialog(DialogNames.AddClientDialog, dialogParameters, callBack);
        }

        public static void ShowAddContractItemDialog(this IDialogService dialogService, Action<IDialogResult> callBack)
        {
            dialogService.ShowDialog(DialogNames.AddContractItemDialog, callBack);
        }

        public static void ShowLendingRateSettingsDialog(this IDialogService dialogService, Action<IDialogResult> callBack)
        {
            dialogService.Show(DialogNames.LendingRateSettingsDialog, callBack);
        }

        public static void ShowEditLendingRateSettingsDialog(this IDialogService dialogService, LendingRate lendingRate, Action<IDialogResult> callBack)
        {
            var dialogParameters = new DialogParameters { { "lendingRate", lendingRate } };
            dialogService.ShowDialog(DialogNames.EditLendingRateDialog, dialogParameters, callBack);
        }

        public static void ShowWorkerDialog(this IDialogService dialogService, Action<IDialogResult> callBack, string title, WorkerDialogMode workerDialogMode, WorkerBoss workerBoss)
        {
            var dialogParameters = new DialogParameters { { "title", title }, { "workerDialogMode", workerDialogMode }, { "workerBoss", workerBoss } };
            dialogService.ShowDialog(DialogNames.WorkerDialog, dialogParameters, callBack);
        }

        public static void ShowWorkerDialog(this IDialogService dialogService, Action<IDialogResult> callBack, string title)
        {
            var dialogParameters = new DialogParameters { { "title", title }, { "workerDialogMode", WorkerDialogMode.Add } };
            dialogService.ShowDialog(DialogNames.WorkerDialog, dialogParameters, callBack);
        }
    }
}