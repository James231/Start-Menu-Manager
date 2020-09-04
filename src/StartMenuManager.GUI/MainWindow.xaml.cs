// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.IO;
using System.Windows;
using ImageMagick;
using StartMenuManager.Core.DataStructures;
using StartMenuManager.Core.Serialization;
using StartMenuManager.GUI.Serialization;

namespace StartMenuManager.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowRef.Wind = this;

            Settings = JsonSerializer.GetSettings();
            if (!Settings.DisplayWelcomeBox)
            {
                WelcomeCard.CloseCard();
            }

            if (!CheckSettingsValid())
            {
                Close();
                return;
            }

            InitEvents();
            ThemeManager.InitTheme();
            InitJson();
        }

        public SettingsConfig Settings { get; set; }

        private void InitEvents()
        {
            TitleBarControl.InitEvents();
            TitleBarButtons.InitEvents();
            WelcomeCard.InitEvents();
            LowerMenuBar.InitEvents();
            ShortcutListArea.InitEvents();
            JsonViewManager.Init();
            Dialogs.SettingsDialog.InitEvents();
            Dialogs.MessageDialog.InitEvents();
            Dialogs.YesNoDialog.InitEvents();
        }

        private void InitJson()
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
        }

        private void Window_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = IconExtractorWindow.FocusInstance();
        }

        private void Window_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            e.Handled = IconExtractorWindow.FocusInstance();
        }

        public void SelectedWebsiteImage(MagickImage image, ShortcutControl control)
        {
            image.Format = MagickFormat.Ico;
            string filePath = IconManager.SaveIcon(image, control.Shortcut.Name);
            control.IconSelectedEvent(filePath);
        }

        private bool CheckSettingsValid()
        {
            if (!Directory.Exists(Settings.StartMenuFolder))
            {
                MessageBox.Show("Start Menu Folder not found. Please check the path in settings.json in the application install directory.");
                return false;
            }

            if (!Directory.Exists(Settings.GetShortcutsFilePath()))
            {
                Directory.CreateDirectory(Settings.GetShortcutsFilePath());
            }

            return true;
        }
    }
}
