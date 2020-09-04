// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Windows;
using static StartMenuManager.GUI.WindowRef;

namespace StartMenuManager.GUI
{
    public static class WelcomeCard
    {
        public static void InitEvents()
        {
            Wind.welcomeCard_CloseButton.Click += (sender, args) => CloseCardPressed(sender, args);
        }

        private static void CloseCardPressed(object sender, RoutedEventArgs args)
        {
            CloseCard();
            Wind.Settings.DisplayWelcomeBox = false;
            Serialization.JsonSerializer.SaveSettings(Wind.Settings);
        }

        public static void CloseCard()
        {
            Wind.welcomeCard.Visibility = Visibility.Collapsed;
        }
    }
}
