// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;

namespace StartMenuManager.GUI
{
    public static class Runner
    {
        public static void Run(string filePath)
        {
            string runnerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Runner", "StartMenuManager.Runner.exe");
            ExecuteAsAdmin(runnerPath, filePath);
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
