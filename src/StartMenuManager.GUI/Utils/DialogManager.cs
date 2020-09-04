// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using MaterialDesignThemes.Wpf;
using StartMenuManager.GUI.Dialogs;
using static StartMenuManager.GUI.WindowRef;

namespace StartMenuManager.GUI
{
    public static class DialogManager
    {
        public static DialogSession OpenDialogSession { get; set; }

        /// <summary>
        /// Opens a dialog
        /// </summary>
        /// <param name="dialogType">The type of dialog to open</param>
        public static async void Show(Structures.DialogTypes dialogType)
        {
            Wind.SettingsDialog.Visibility = System.Windows.Visibility.Collapsed;
            Wind.MessageDialog.Visibility = System.Windows.Visibility.Collapsed;
            Wind.YesNoDialog.Visibility = System.Windows.Visibility.Collapsed;

            switch (dialogType)
            {
                case Structures.DialogTypes.SettingsDialog:
                    SettingsDialog.InitDialog();
                    Wind.SettingsDialog.Visibility = System.Windows.Visibility.Visible;
                    break;
                case Structures.DialogTypes.MessageDialog:
                    Wind.MessageDialog.Visibility = System.Windows.Visibility.Visible;
                    break;
                case Structures.DialogTypes.YesNoDialog:
                    Wind.YesNoDialog.Visibility = System.Windows.Visibility.Visible;
                    break;
            }

            try
            {
                var result = await Wind.dialogHost.ShowDialog(
                    Wind.DialogContent,
                    delegate(object send, DialogOpenedEventArgs args)
                    {
                        DialogManager.OpenDialogSession = args.Session;
                    });
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
