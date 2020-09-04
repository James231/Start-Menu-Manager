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
    public class FileAction : Action
    {
        public FileAction(string path)
        {
            Type = "file";
            Path = path;
        }

        public FileAction()
        {
            Type = "file";
            Path = string.Empty;
        }

        public string Path { get; set; }

        public override ValidationError IsValid()
        {
            if (string.IsNullOrEmpty(Path))
            {
                return new ValidationError("File Path cannot be empty!", this);
            }

            if (IsFolder(Path))
            {
                return new ValidationError("Need File path, not folder path!", this);
            }

            if (!File.Exists(Path))
            {
                return new ValidationError("File Path does not exist!", this);
            }

            return null;
        }

        private static bool IsFolder(string path)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public override Action Duplicate()
        {
            return new FileAction(Path);
        }
    }
}
