// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.IO;
using IWshRuntimeLibrary;
using Newtonsoft.Json;
using StartMenuManager.Core.DataStructures;
using StartMenuManager.Core.Serialization;

namespace StartMenuManager.Builder
{
    public class Program
    {
        private static string jsonFilePath;
        private static SettingsConfig settings;

        public static void Main(string[] args)
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

            if (!GetJsonFilePath(args))
            {
                if (!CouldFindJsonFileInDirectory())
                {
                    Console.WriteLine("Error: Could not find 'shortcuts.json'");
                    Console.WriteLine("Please provide path to 'shortcuts.json' as command line argument.");
                    return;
                }
            }

            string fileContents = GetFileContents();
            Config config = Serializer.DeserializeConfig(fileContents);
            if (config == null)
            {
                Console.WriteLine("Error: Not valid JSON!");
                return;
            }

            ValidationError err = config.IsValid();
            if (err != null)
            {
                Console.WriteLine("Error: One or more shortcuts within JSON file are not valid!");
                Console.WriteLine("More Details:");
                Console.WriteLine($"Shortcut \"{err.Shortcut.Name}\":{err.Error}");
                return;
            }

            if (!Directory.Exists(settings.StartMenuFolder))
            {
                Console.WriteLine("Error: Could not find Start Menu Folder");
                return;
            }

            if (!Directory.Exists(settings.GetShortcutsFilePath()))
            {
                Directory.CreateDirectory(settings.GetShortcutsFilePath());
            }
            else
            {
                ClearDirectoryExceptShortcutsJson(settings.GetShortcutsFilePath());
            }

            foreach (Shortcut shortcut in config.Shortcuts)
            {
                BuildShortcutFile(shortcut);
            }

            Console.WriteLine("All done!");
        }

        public static void BuildShortcutFile(Shortcut shortcut)
        {
            // Build text file
            string shortcutJson = Serializer.SerializeShortcut(shortcut);
            string shortcutTextFilePath = settings.GetShortcutsFilePath($"{shortcut.Name}.txt");
            StreamWriter writer = new StreamWriter(shortcutTextFilePath, false);
            writer.Write(shortcutJson);
            writer.Close();

            // Copy icon
            string iconFilePath = settings.GetShortcutsFilePath($"{shortcut.Name}.ico");
            bool gotIcon = CopyIcon(shortcut.IconPath, iconFilePath);

            // Create shortcut
            string runnerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Runner\StartMenuManager.Runner.exe");
            string shortcutFilePath = settings.GetShortcutsFilePath($"{shortcut.Name}.lnk");
            WshShell shell = new WshShell();
            IWshShortcut wshShortcut = (IWshShortcut)shell.CreateShortcut(shortcutFilePath);
            if (gotIcon)
            {
                wshShortcut.IconLocation = iconFilePath;
            }

            wshShortcut.TargetPath = $"{runnerPath}";
            wshShortcut.Arguments = $"\"{shortcutTextFilePath}\"";
            wshShortcut.Save();
        }

        public static bool CopyIcon(string iconPath, string newIconPath)
        {
            if (string.IsNullOrEmpty(iconPath))
            {
                return false;
            }

            if (Path.GetExtension(iconPath) != ".ico")
            {
                return false;
            }

            if (!System.IO.File.Exists(iconPath))
            {
                return false;
            }

            System.IO.File.Copy(iconPath, newIconPath);
            return true;
        }

        public static bool GetJsonFilePath(string[] args)
        {
            if (args != null)
            {
                if (args.Length > 0)
                {
                    string possiblePath = args[0];
                    if (System.IO.File.Exists(possiblePath))
                    {
                        jsonFilePath = possiblePath;
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool CouldFindJsonFileInDirectory()
        {
            string possiblePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shortcuts.json");
            if (System.IO.File.Exists(possiblePath))
            {
                jsonFilePath = possiblePath;
                return true;
            }

            return false;
        }

        public static string GetFileContents()
        {
            StreamReader reader = new StreamReader(jsonFilePath);
            string contents = reader.ReadToEnd();
            reader.Close();
            return contents;
        }

        public static void ClearDirectoryExceptShortcutsJson(string path)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                if (file.Name == "shortcuts.json")
                {
                    continue;
                }

                file.Delete();
            }

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
