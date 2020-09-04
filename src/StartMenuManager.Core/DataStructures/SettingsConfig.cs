// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.IO;

namespace StartMenuManager.Core.DataStructures
{
    public class SettingsConfig
    {
        public string StartMenuFolder { get; set; }

        public string SmmShortcutsFolder { get; set; }

        public bool DisplayWelcomeBox { get; set; }

        public string Theme { get; set; }

        public bool JsonLineNumbers { get; set; }

        public bool YesNoDialogsEnabled { get; set; }

        public int NumberOfUsesUntilMessage { get; set; }

        public string GetShortcutsFilePath(string fileName)
        {
            return Path.Combine(StartMenuFolder, SmmShortcutsFolder, fileName);
        }

        public string GetShortcutsFilePath()
        {
            return Path.Combine(StartMenuFolder, SmmShortcutsFolder);
        }

        public static SettingsConfig GetDefaultSettings()
        {
            SettingsConfig settings = new SettingsConfig();
            settings.StartMenuFolder = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs";
            settings.SmmShortcutsFolder = "SmmShortcuts";
            settings.DisplayWelcomeBox = true;
            settings.Theme = "default";
            settings.JsonLineNumbers = false;
            settings.YesNoDialogsEnabled = true;
            settings.NumberOfUsesUntilMessage = 4;
            return settings;
        }
    }
}
