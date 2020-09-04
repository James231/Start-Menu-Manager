// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.IO;

namespace StartMenuManager.Core.DataStructures.Actions
{
    /// <summary>
    /// Action which opens a file with the default software for that file type.
    /// </summary>
    public class FolderAction : Action
    {
        public FolderAction(string path)
        {
            Type = "folder";
            Path = path;
        }

        public FolderAction()
        {
            Type = "folder";
            Path = string.Empty;
        }

        public string Path { get; set; }

        public override ValidationError IsValid()
        {
            if (string.IsNullOrEmpty(Path))
            {
                return new ValidationError("Folder Path cannot be empty!", this);
            }

            if (IsFile(Path))
            {
                return new ValidationError("Need Folder path, not file path!", this);
            }

            if (!Directory.Exists(Path))
            {
                return new ValidationError("Folder does not exist!", this);
            }

            return null;
        }

        private static bool IsFile(string path)
        {
            if (System.IO.Path.HasExtension(path))
            {
                return true;
            }

            return false;
        }

        public override Action Duplicate()
        {
            return new FolderAction(Path);
        }
    }
}
