// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using StartMenuManager.Core.DataStructures;
using StartMenuManager.Core.DataStructures.Actions;
using StartMenuManager.GUI.Structures;

namespace StartMenuManager.GUI.Extensions
{
    public static class ActionExtensions
    {
        public static ShortcutType ToShortcutType(this Action action)
        {
            if (action is FileAction)
            {
                return ShortcutType.File;
            }

            if (action is FolderAction)
            {
                return ShortcutType.Folder;
            }

            if (action is SoftwareAction)
            {
                return ShortcutType.Software;
            }

            if (action is CommandAction)
            {
                return ShortcutType.Command;
            }

            return ShortcutType.Web;
        }
    }
}
