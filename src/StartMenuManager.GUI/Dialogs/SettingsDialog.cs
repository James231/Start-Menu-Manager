// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Diagnostics;
using System.IO;
using System.Windows;
using static StartMenuManager.GUI.WindowRef;

namespace StartMenuManager.GUI.Dialogs
{
    public static class SettingsDialog
    {
        private static bool eventBlock = false;

        public static void InitEvents()
        {
            Wind.SettingsDialog_CloseButton.Click += (sender, args) => CloseDialog(sender, args);
            Wind.SettingsDialog_OpenFolderButton.Click += (sender, args) => OpenStartMenuFolder(sender, args);
            Wind.SettingsDialog_GitHubButton.Click += (sender, args) => OpenGitHubPage(sender, args);
            Wind.SettingsDialog_LineNumbersCheckbox.Checked += (sender, args) => LineNumbersChanged(true);
            Wind.SettingsDialog_LineNumbersCheckbox.Unchecked += (sender, args) => LineNumbersChanged(false);
            Wind.SettingsDialog_YesNoCheckbox.Checked += (sender, args) => ShowConfirmationDialogsChanged(true);
            Wind.SettingsDialog_YesNoCheckbox.Unchecked += (sender, args) => ShowConfirmationDialogsChanged(false);

            Wind.SettingsDialog_ThemeComboBox.SelectionChanged += (sender, args) => ThemeChanged(sender, args);
        }

        public static void InitDialog()
        {
            if (!Directory.Exists(WindowRef.Wind.Settings.GetShortcutsFilePath()))
            {
                Wind.SettingsDialog_NumOfShortcuts.Text = "0";
            }
            else
            {
                string[] files = Directory.GetFiles(WindowRef.Wind.Settings.GetShortcutsFilePath(), "*.lnk");
                Wind.SettingsDialog_NumOfShortcuts.Text = files.Length.ToString();
            }

            if (Wind.Settings.Theme == "dark" && Wind.SettingsDialog_ThemeComboBox.SelectedIndex != 0)
            {
                eventBlock = true;
                Wind.SettingsDialog_ThemeComboBox.SelectedIndex = 0;
            }

            if (Wind.Settings.Theme == "light" && Wind.SettingsDialog_ThemeComboBox.SelectedIndex != 1)
            {
                eventBlock = true;
                Wind.SettingsDialog_ThemeComboBox.SelectedIndex = 1;
            }

            if (Wind.Settings.Theme == "default" && Wind.SettingsDialog_ThemeComboBox.SelectedIndex != 2)
            {
                eventBlock = true;
                Wind.SettingsDialog_ThemeComboBox.SelectedIndex = 2;
            }

            if (Wind.Settings.YesNoDialogsEnabled != Wind.SettingsDialog_YesNoCheckbox.IsChecked)
            {
                eventBlock = true;
                Wind.SettingsDialog_YesNoCheckbox.IsChecked = Wind.Settings.YesNoDialogsEnabled;
            }

            if (Wind.Settings.JsonLineNumbers != Wind.SettingsDialog_LineNumbersCheckbox.IsChecked)
            {
                eventBlock = true;
                Wind.SettingsDialog_LineNumbersCheckbox.IsChecked = Wind.Settings.JsonLineNumbers;
            }
        }

        public static void CloseDialog(object sender, RoutedEventArgs args)
        {
            if (DialogManager.OpenDialogSession != null)
            {
                DialogManager.OpenDialogSession.Close();
                DialogManager.OpenDialogSession = null;
            }
        }

        public static void OpenStartMenuFolder(object sender, RoutedEventArgs args)
        {
            if (Directory.Exists(WindowRef.Wind.Settings.GetShortcutsFilePath()))
            {
                Process.Start(WindowRef.Wind.Settings.GetShortcutsFilePath());
                return;
            }

            if (Directory.Exists(WindowRef.Wind.Settings.StartMenuFolder))
            {
                Process.Start(WindowRef.Wind.Settings.StartMenuFolder);
            }
        }

        public static void OpenGitHubPage(object sender, RoutedEventArgs args)
        {
            Process.Start("https://github.com/James231/Start-Menu-Manager");
        }

        public static void ThemeChanged(object sender, RoutedEventArgs args)
        {
            if (eventBlock)
            {
                eventBlock = false;
                return;
            }

            switch (Wind.SettingsDialog_ThemeComboBox.SelectedIndex)
            {
                case 0:
                    ThemeManager.SetDarkTheme();
                    Wind.Settings.Theme = "dark";
                    break;
                case 1:
                    ThemeManager.SetLightTheme();
                    Wind.Settings.Theme = "light";
                    break;
                case 2:
                    ThemeManager.SetSystemTheme();
                    Wind.Settings.Theme = "default";
                    break;
            }

            Serialization.JsonSerializer.SaveSettings(Wind.Settings);
        }

        public static void LineNumbersChanged(bool show)
        {
            if (eventBlock)
            {
                eventBlock = false;
                return;
            }

            JsonViewManager.ShowLineNumbers(show);
            Wind.Settings.JsonLineNumbers = show;
            Serialization.JsonSerializer.SaveSettings(Wind.Settings);
        }

        public static void ShowConfirmationDialogsChanged(bool show)
        {
            if (eventBlock)
            {
                eventBlock = false;
                return;
            }

            Wind.Settings.YesNoDialogsEnabled = show;
            Serialization.JsonSerializer.SaveSettings(Wind.Settings);
        }
    }
}
