// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using ImageMagick;
using StartMenuManager.Core.DataStructures;

namespace StartMenuManager.GUI.Serialization
{
    public static class IconManager
    {
        /// <summary>
        /// Saves Icon to temporary file path.
        /// </summary>
        /// <param name="icon">Icon to save.</param>
        /// <param name="shortcutName">Shortcut Name</param>
        /// <returns>Absolute file path of saved file.</returns>
        public static string SaveIcon(MagickImage icon, string shortcutName)
        {
            string websiteIconsFolder = GetWebsiteIconsFolder();
            int fileNum = 0;
            while (File.Exists(Path.Combine(websiteIconsFolder, $"{shortcutName}_{fileNum}.ico")))
            {
                fileNum++;
            }

            string iconPath = Path.Combine(websiteIconsFolder, $"{shortcutName}_{fileNum}.ico");
            icon.Write(iconPath);
            return iconPath;
        }

        /// <summary>
        /// Removes all unused icons from the website icons folder.
        /// </summary>
        /// <param name="config">Shortcut Configuration.</param>
        public static void ClearUnusedIcons(Config config)
        {
            List<string> usedIcons = new List<string>();

            foreach (Shortcut shorctut in config.Shortcuts)
            {
                if (!string.IsNullOrEmpty(shorctut.IconPath))
                {
                    usedIcons.Add(shorctut.IconPath);
                }
            }

            // Get all files from directory. Delete those which arent in usedIcons
            string dir = GetWebsiteIconsFolder();
            string[] filePaths = Directory.GetFiles(dir);
            foreach (string filePath in filePaths)
            {
                if (!filePath.EndsWith(".ico"))
                {
                    return;
                }

                if (!usedIcons.Contains(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        private static string GetWebsiteIconsFolder()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string folder = Path.Combine(baseDir, "Website Icons");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }
    }
}
