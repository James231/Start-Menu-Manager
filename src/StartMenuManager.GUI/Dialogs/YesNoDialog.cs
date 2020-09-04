// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Windows;
using static StartMenuManager.GUI.WindowRef;

namespace StartMenuManager.GUI.Dialogs
{
    public static class YesNoDialog
    {
        private static Callback response;

        public delegate void Callback(bool isYes);

        public static void InitEvents()
        {
            Wind.YesNoDialog_YesButton.Click += (sender, args) => YesButtonPressed(sender, args);
            Wind.YesNoDialog_NoButton.Click += (sender, args) => NoButtonPressed(sender, args);
        }

        public static void SetMessage(string title, string message, string yesText, string noText, Callback isYes)
        {
            Wind.YesNoDialog_YesButton.Content = yesText;
            Wind.YesNoDialog_NoButton.Content = noText;
            Wind.YesNoDialog_Message.Text = message;
            Wind.YesNoDialog_Title.Text = title;
            response = isYes;
        }

        public static void YesButtonPressed(object sender, RoutedEventArgs args)
        {
            CloseDialog();
            response.Invoke(true);
        }

        public static void NoButtonPressed(object sender, RoutedEventArgs args)
        {
            CloseDialog();
            response.Invoke(false);
        }

        private static void CloseDialog()
        {
            if (DialogManager.OpenDialogSession != null)
            {
                DialogManager.OpenDialogSession.Close();
                DialogManager.OpenDialogSession = null;
            }
        }
    }
}