// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using StartMenuManager.Core.DataStructures;
using StartMenuManager.Core.Serialization;
using StartMenuManager.GUI.Dialogs;
using StartMenuManager.GUI.Serialization;

namespace StartMenuManager.GUI
{
    public static class Builder
    {
        public static void Run(Config config)
        {
            WindowRef.Wind.Settings.NumberOfUsesUntilMessage--;
            if (WindowRef.Wind.Settings.NumberOfUsesUntilMessage == 0)
            {
                WindowRef.Wind.Settings.NumberOfUsesUntilMessage = 4;

                YesNoDialog.SetMessage("Please Consider Donating.", "It took a lot of time to make this software available for free. If you like it, please consider making a donation to show your appreciation.", "Donate", "Ignore", (b) => ContinueRun(config, b));
                DialogManager.Show(Structures.DialogTypes.YesNoDialog);
                Serialization.JsonSerializer.SaveSettings(WindowRef.Wind.Settings);
                return;
            }

            Serialization.JsonSerializer.SaveSettings(WindowRef.Wind.Settings);
            ContinueRun(config, false);
        }

        private static void ContinueRun(Config config, bool donate)
        {
            if (donate)
            {
                System.Diagnostics.Process.Start(@"https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=MLD56V6HQWCKU&source=url");
            }

            string configJson = Serializer.SerializeConfig(config);
            JsonSerializer.SaveLastShortcutsJson(configJson);
            string builderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Builder", "StartMenuManager.Builder.exe");
            string shortcutsJsonPath = WindowRef.Wind.Settings.GetShortcutsFilePath("shortcuts.json");
            ExecuteAsAdmin(builderPath, shortcutsJsonPath);

            // Remove old website icons
            IconManager.ClearUnusedIcons(config);
        }

        private static void ExecuteAsAdmin(string fileName, string arguments)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Arguments = $"\"{arguments}\"";
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }
    }
}
