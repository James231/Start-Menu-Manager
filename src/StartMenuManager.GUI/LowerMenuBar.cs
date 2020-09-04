// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Windows;
using StartMenuManager.Core.DataStructures;
using StartMenuManager.Core.Serialization;
using StartMenuManager.GUI.Dialogs;
using StartMenuManager.GUI.Serialization;
using static StartMenuManager.GUI.WindowRef;

namespace StartMenuManager.GUI
{
    public static class LowerMenuBar
    {
        public static void InitEvents()
        {
            Wind.LowerMenu_ResetButton.Click += (sender, args) => ResetButtonPressed(sender, args);
            Wind.LowerMenu_RemoveButton.Click += (sender, args) => RemoveAllButtonPressed(sender, args);
            Wind.LowerMenu_GenerateButton.Click += (sender, args) => GenerateShortcutsButtonPressed(sender, args);
        }

        public static void ResetButtonPressed(object sender, RoutedEventArgs args)
        {
            if (Wind.Settings.YesNoDialogsEnabled)
            {
                YesNoDialog.SetMessage("Are you sure?", "You will loose any changes.", "Yes", "No", ResetJson);
                DialogManager.Show(Structures.DialogTypes.YesNoDialog);
            }
            else
            {
                ResetJson(true);
            }
        }

        public static void RemoveAllButtonPressed(object sender, RoutedEventArgs args)
        {
            if (Wind.Settings.YesNoDialogsEnabled)
            {
                Dialogs.YesNoDialog.SetMessage("Are you sure?", "This will remove all Shortcuts you have entered?", "Yes", "No", RemoveAllConfirmation);
                DialogManager.Show(Structures.DialogTypes.YesNoDialog);
            }
            else
            {
                RemoveAllConfirmation(true);
            }
        }

        private static void RemoveAllConfirmation(bool isYes)
        {
            if (isYes)
            {
                ShortcutListArea.RemoveAllShortcuts();
                Wind.ValiationError.Visibility = Visibility.Collapsed;
            }
        }

        public static void GenerateShortcutsButtonPressed(object sender, RoutedEventArgs args)
        {
            Config config = ShortcutListArea.GetConfig();
            ValidationError err = config.IsValid();

            if (err != null)
            {
                Wind.ValiationError.Visibility = Visibility.Visible;
                Wind.ValidationErrorText.Text = $"Error: \"{err.Shortcut.Name}\" {err.Error}";
                return;
            }
            else
            {
                Wind.ValiationError.Visibility = Visibility.Collapsed;
            }

            Builder.Run(config);
        }

        private static void ResetJson(bool shouldReset)
        {
            if (shouldReset)
            {
                string getJson = JsonSerializer.GetLastShortcutsJson();
                Config shortcutsConfig = null;

                try
                {
                    shortcutsConfig = Serializer.DeserializeConfig(getJson);
                }
                catch
                {
                    shortcutsConfig = new Config();
                    getJson = Serializer.SerializeConfig(shortcutsConfig);
                    JsonSerializer.SaveLastShortcutsJson(getJson);
                }

                JsonViewManager.SetJson(getJson);
                ShortcutListArea.LoadShortcuts(shortcutsConfig.Shortcuts);
                Wind.ValiationError.Visibility = Visibility.Collapsed;
            }
        }
    }
}
