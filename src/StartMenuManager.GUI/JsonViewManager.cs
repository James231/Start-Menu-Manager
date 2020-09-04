// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Windows;
using static StartMenuManager.GUI.WindowRef;

namespace StartMenuManager.GUI
{
    public static class JsonViewManager
    {
        private static string oldJson;

        public static void InitEvents()
        {
            Wind.JsonView_ResetJsonButton.Click += (sender, args) => ResetJsonButtonPressed(sender, args);
        }

        public static void Init()
        {
            InitEvents();

            ShowLineNumbers(Wind.Settings.JsonLineNumbers);
        }

        public static void ShowLineNumbers(bool enabled)
        {
            Wind.textEditor.ShowLineNumbers = enabled;
        }

        public static void SetJson(string text)
        {
            oldJson = text;
            Wind.textEditor.Text = text;
        }

        public static void ResetJsonButtonPressed(object sender, RoutedEventArgs args)
        {
            Wind.textEditor.Text = oldJson;
        }
    }
}
