// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.IO;
using Newtonsoft.Json;
using StartMenuManager.Core.DataStructures;
using StartMenuManager.Core.Serialization;

namespace StartMenuManager.GUI.Serialization
{
    public static class JsonSerializer
    {
        public static SettingsConfig GetSettings()
        {
            if (!File.Exists(GetSettingsFilePath()))
            {
                return SettingsConfig.GetDefaultSettings();
            }

            StreamReader reader = new StreamReader(GetSettingsFilePath());
            string contents = reader.ReadToEnd();
            reader.Close();

            return JsonConvert.DeserializeObject<SettingsConfig>(contents);
        }

        public static void SaveSettings(SettingsConfig settings)
        {
            string settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
            StreamWriter writer = new StreamWriter(GetSettingsFilePath(), false);
            writer.WriteLine(settingsJson);
            writer.Close();
        }

        public static string GetLastShortcutsJson()
        {
            if (!Directory.Exists(WindowRef.Wind.Settings.StartMenuFolder))
            {
                return GetDefaultJson();
            }

            if (!Directory.Exists(WindowRef.Wind.Settings.GetShortcutsFilePath()))
            {
                Directory.CreateDirectory(WindowRef.Wind.Settings.GetShortcutsFilePath());
            }

            if (!File.Exists(WindowRef.Wind.Settings.GetShortcutsFilePath("shortcuts.json")))
            {
                return GetDefaultJson();
            }

            StreamReader reader = new StreamReader(WindowRef.Wind.Settings.GetShortcutsFilePath("shortcuts.json"));
            string contents = reader.ReadToEnd();
            reader.Close();
            return contents;
        }

        public static void SaveLastShortcutsJson(string jsonString)
        {
            if (!Directory.Exists(WindowRef.Wind.Settings.StartMenuFolder))
            {
                return;
            }

            if (!Directory.Exists(WindowRef.Wind.Settings.GetShortcutsFilePath()))
            {
                Directory.CreateDirectory(WindowRef.Wind.Settings.GetShortcutsFilePath());
            }

            StreamWriter writer = new StreamWriter(WindowRef.Wind.Settings.GetShortcutsFilePath("shortcuts.json"), false);
            writer.WriteLine(jsonString);
            writer.Close();
        }

        private static string GetDefaultJson()
        {
            Config newConfig = new Config();
            return Serializer.SerializeConfig(newConfig);
        }

        private static string GetSettingsFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
        }
    }
}
