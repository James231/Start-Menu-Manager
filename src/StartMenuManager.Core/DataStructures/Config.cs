// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace StartMenuManager.Core.DataStructures
{
    /// <summary>
    /// A full shortcut configuration (equivalent to the JSON file)
    /// </summary>
    public class Config
    {
        public Config(List<Shortcut> shortcuts)
        {
            Shortcuts = shortcuts;
        }

        public Config()
        {
            Shortcuts = new List<Shortcut>();
        }

        public List<Shortcut> Shortcuts { get; set; }

        public ValidationError IsValid()
        {
            List<string> names = new List<string>();
            foreach (Shortcut shortcut in Shortcuts)
            {
                ValidationError shortcutErr = shortcut.IsValid();
                if (shortcutErr != null)
                {
                    return shortcutErr;
                }

                if (names.Contains(shortcut.Name))
                {
                    return new ValidationError("Duplicate Shortcut Name Found!", shortcut);
                }

                names.Add(shortcut.Name);
            }

            return null;
        }
    }
}
