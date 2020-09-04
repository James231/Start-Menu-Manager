// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Windows;
using static StartMenuManager.GUI.WindowRef;

namespace StartMenuManager.GUI.Dialogs
{
    public static class MessageDialog
    {
        public static void InitEvents()
        {
            Wind.MessageDialog_CloseButton.Click += (sender, args) => CloseDialog(sender, args);
        }

        public static void SetMessage(string title, string message)
        {
            Wind.MessageDialog_Message.Text = message;
            Wind.MessageDialog_Title.Text = title;
        }

        public static void CloseDialog(object sender, RoutedEventArgs args)
        {
            if (DialogManager.OpenDialogSession != null)
            {
                DialogManager.OpenDialogSession.Close();
                DialogManager.OpenDialogSession = null;
            }
        }
    }
}
