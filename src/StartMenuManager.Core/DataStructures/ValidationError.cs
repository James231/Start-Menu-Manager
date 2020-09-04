// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

namespace StartMenuManager.Core.DataStructures
{
    public class ValidationError
    {
        public ValidationError(string error, Action action)
        {
            Error = error;
            Action = action;
        }

        public ValidationError(string error, Shortcut shortcut)
        {
            Error = error;
            Shortcut = shortcut;
        }

        public Action Action { get; set; }

        public Shortcut Shortcut { get; set; }

        public string Error { get; set; }
    }
}
