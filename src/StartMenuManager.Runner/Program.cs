// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.IO;
using StartMenuManager.Core.DataStructures;
using StartMenuManager.Core.DataStructures.Actions;
using StartMenuManager.Core.Serialization;

namespace StartMenuManager.Runner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args == null)
            {
                return;
            }

            if (args.Length == 0)
            {
                return;
            }

            if (!System.IO.File.Exists(args[0]))
            {
                return;
            }

            string shortcutJsonPath = args[0];

            StreamReader reader = new StreamReader(shortcutJsonPath);
            string shortcutJson = reader.ReadToEnd();
            reader.Close();

            Shortcut shortcut = Serializer.DeserializeShortcut(shortcutJson);

            if (shortcut.IsValid() != null)
            {
                return;
            }

            ExecuteShortcut(shortcut);
        }

        public static void ExecuteShortcut(Shortcut shortcut)
        {
            foreach (Action action in shortcut.Actions)
            {
                switch (action.Type)
                {
                    case "command":
                        CommandAction ca = action as CommandAction;
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                        if (!ca.KeepOpen)
                        {
                            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            startInfo.Arguments = $"/C {ca.Command}";
                        }
                        else
                        {
                            startInfo.Arguments = $"/k {ca.Command}";
                        }

                        startInfo.FileName = "cmd.exe";
                        process.StartInfo = startInfo;
                        process.Start();
                        break;
                    case "file":
                        FileAction fa = action as FileAction;
                        System.Diagnostics.Process.Start(fa.Path);
                        break;
                    case "folder":
                        FolderAction foa = action as FolderAction;
                        System.Diagnostics.Process.Start(foa.Path);
                        break;
                    case "software":
                        SoftwareAction sa = action as SoftwareAction;
                        System.Diagnostics.Process.Start(sa.Path);
                        break;
                    case "website":
                        WebsiteAction wa = action as WebsiteAction;
                        System.Diagnostics.Process.Start(wa.Url);
                        break;
                }
            }
        }
    }
}
