// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using StartMenuManager.Core.DataStructures;
using StartMenuManager.Core.Serialization;
using static StartMenuManager.GUI.WindowRef;

namespace StartMenuManager.GUI
{
    public static class TitleBarButtons
    {
        private static bool inJsonView = false;

        public static void InitEvents()
        {
            Wind.TitleBar_SettingsButton.Click += (sender, args) => SettingsButtonPressed(sender, args);
            Wind.TitleBar_JsonButton.Click += (sender, args) => JsonButtonPressed(sender, args);
        }

        public static void SettingsButtonPressed(object sender, RoutedEventArgs args)
        {
            if (DialogManager.OpenDialogSession != null)
            {
                return;
            }

            DialogManager.Show(Structures.DialogTypes.SettingsDialog);
        }

        public static void JsonButtonPressed(object sender, RoutedEventArgs args)
        {
            if (DialogManager.OpenDialogSession != null)
            {
                return;
            }

            if (inJsonView)
            {
                // Attempt to Deserialize Json
                string json = Wind.textEditor.Text;
                Config config = null;
                try
                {
                    config = Serializer.DeserializeConfig(json);
                }
                catch (Exception e)
                {
                    Dialogs.MessageDialog.SetMessage("Json Error", e.Message);
                    DialogManager.Show(Structures.DialogTypes.MessageDialog);
                    return;
                }

                ShortcutListArea.LoadShortcuts(config.Shortcuts);
                Wind.JsonView.Visibility = Visibility.Collapsed;
                Wind.NonJsonView.Visibility = Visibility.Visible;
                Wind.ValiationError.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoadJsonFromShortcuts();
                Wind.JsonView.Visibility = Visibility.Visible;
                Wind.NonJsonView.Visibility = Visibility.Collapsed;
            }

            inJsonView = !inJsonView;
        }

        private static void LoadJsonFromShortcuts()
        {
            Config config = ShortcutListArea.GetConfig();
            string configJson = Serializer.SerializeConfig(config);
            JsonViewManager.SetJson(configJson);
        }
    }
}
