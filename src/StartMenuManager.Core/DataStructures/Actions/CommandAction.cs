// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

namespace StartMenuManager.Core.DataStructures.Actions
{
    /// <summary>
    /// Action which executes a command using Command Prompt.
    /// </summary>
    public class CommandAction : Action
    {
        public CommandAction(string command, bool keepOpen)
        {
            Type = "command";
            Command = command;
            KeepOpen = keepOpen;
        }

        public CommandAction()
        {
            Type = "command";
            Command = string.Empty;
            KeepOpen = true;
        }

        public string Command { get; set; }

        public bool KeepOpen { get; set; }

        public override ValidationError IsValid()
        {
            if (string.IsNullOrEmpty(Command))
            {
                return new ValidationError("Command cannot be empty!", this);
            }

            return null;
        }

        public override Action Duplicate()
        {
            return new CommandAction(Command, KeepOpen);
        }
    }
}
