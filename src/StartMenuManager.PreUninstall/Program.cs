// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

// This program does some cleaning up before uninstalling.
// It removes all shortcuts from the start menu folder.

using System;
using System.IO;
using Newtonsoft.Json;
using StartMenuManager.Core.DataStructures;

namespace StartMenuManager.PreUninstall
{
    public class Program
    {
        private static SettingsConfig settings;

        public static int Main(string[] args)
        {
            string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\settings.json");
            if (System.IO.File.Exists(settingsPath))
            {
                StreamReader reader = new StreamReader(settingsPath);
                string contents = reader.ReadToEnd();
                reader.Close();
                settings = JsonConvert.DeserializeObject<SettingsConfig>(contents);
            }
            else
            {
                settings = SettingsConfig.GetDefaultSettings();
            }

            string shortcutsFilepath = settings.GetShortcutsFilePath();

            // Delete the folder (within start menu folder) containing all the shortcuts
            if (Directory.Exists(shortcutsFilepath))
            {
                Directory.Delete(shortcutsFilepath, true);
            }

            return 0;
        }
    }
}
