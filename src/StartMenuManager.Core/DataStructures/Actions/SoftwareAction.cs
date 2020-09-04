// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.IO;

namespace StartMenuManager.Core.DataStructures.Actions
{
    /// <summary>
    /// Action which opens software, usually a .exe file.
    /// </summary>
    public class SoftwareAction : Action
    {
        public SoftwareAction(string path)
        {
            Type = "software";
            Path = path;
        }

        public SoftwareAction()
        {
            Type = "software";
            Path = string.Empty;
        }

        public string Path { get; set; }

        public override ValidationError IsValid()
        {
            if (string.IsNullOrEmpty(Path))
            {
                return new ValidationError("Software Path cannot be empty!", this);
            }

            if (!File.Exists(Path))
            {
                return new ValidationError("Software Start File does not exist!", this);
            }

            return null;
        }

        public override Action Duplicate()
        {
            return new SoftwareAction(Path);
        }
    }
}
