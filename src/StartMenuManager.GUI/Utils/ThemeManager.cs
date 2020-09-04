// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Media;
using HL.Interfaces;
using HL.Manager;
using MaterialDesignThemes.Wpf;
using static StartMenuManager.GUI.WindowRef;

namespace StartMenuManager.GUI
{
    public class ThemeManager
    {
        private static readonly PaletteHelper _paletteHelper = new PaletteHelper();

        public static void InitTheme()
        {
            switch (Wind.Settings.Theme)
            {
                case "light":
                    SetLightTheme();
                    break;
                case "dark":
                    SetDarkTheme();
                    break;
                default:
                    SetSystemTheme();
                    break;
            }
        }

        public static void SetSystemTheme()
        {
            if (IsSystemLightMode())
            {
                SetLightTheme();
            }
            else
            {
                SetDarkTheme();
            }
        }

        public static void SetDarkTheme()
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = new MaterialDesignDarkTheme();
            theme.SetBaseTheme(baseTheme);
            theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString("#243b45"));
            theme.SetSecondaryColor((Color)ColorConverter.ConvertFromString("#3d6475"));
            _paletteHelper.SetTheme(theme);
            SetAvalonTheme(true);
        }

        public static void SetLightTheme()
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString("#7dd8ff"));
            theme.SetSecondaryColor((Color)ColorConverter.ConvertFromString("#b0e7ff"));
            _paletteHelper.SetTheme(theme);
            SetAvalonTheme(false);
        }

        private static bool IsSystemLightMode()
        {
            bool isLightMode = false;
            try
            {
                var v = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", "1");
                if (v != null && v.ToString() == "1")
                {
                    isLightMode = true;
                }
            }
            catch
            {
            }

            return isLightMode;
        }

        private static void SetAvalonTheme(bool isDark)
        {
            IThemedHighlightingManager hm = ThemedHighlightingManager.Instance;
            hm.SetCurrentTheme(isDark ? "VS2019_Dark" : "Light");
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.textEditor.SyntaxHighlighting = hm.GetDefinitionByExtension(".js");
            window.textEditor.Background = new SolidColorBrush(isDark ? Color.FromRgb(30, 30, 30) : Color.FromRgb(255, 255, 255));
            window.textEditor.Foreground = new SolidColorBrush(isDark ? Color.FromRgb(255, 255, 255) : Color.FromRgb(0, 0, 0));
            window.textEditor.LineNumbersForeground = new SolidColorBrush(isDark ? Color.FromRgb(190, 190, 190) : Color.FromRgb(60, 60, 60));
            window.textEditor.EditorCurrentLineBackground = new SolidColorBrush(isDark ? Color.FromRgb(37, 37, 37) : Color.FromRgb(245, 245, 245));
        }
    }
}
