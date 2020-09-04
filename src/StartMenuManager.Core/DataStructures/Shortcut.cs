// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using StartMenuManager.Core.DataStructures.Actions;

namespace StartMenuManager.Core.DataStructures
{
    /// <summary>
    /// A Shortcut for use within a Config
    /// </summary>
    public class Shortcut
    {
        public Shortcut(string name, string iconPath, List<Action> actions)
        {
            Name = name;
            IconPath = iconPath;
            Actions = actions;
        }

        public Shortcut(string name, string iconPath)
        {
            Name = name;
            IconPath = iconPath;
            Actions = new List<Action>();
        }

        public Shortcut()
        {
            Actions = new List<Action>();
        }

        public string IconPath { get; set; }

        public string Name { get; set; }

        public List<Action> Actions { get; set; }

        public ValidationError IsValid()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return new ValidationError("Shortcut Name Cannot be Empty.", this);
            }

            foreach (Action action in Actions)
            {
                switch (action.Type)
                {
                    case "command":
                        CommandAction a1 = action as CommandAction;
                        ValidationError a1Err = a1.IsValid();
                        if (a1Err != null)
                        {
                            a1Err.Shortcut = this;
                            return a1Err;
                        }

                        break;
                    case "file":
                        FileAction a2 = action as FileAction;
                        ValidationError a2Err = a2.IsValid();
                        if (a2Err != null)
                        {
                            a2Err.Shortcut = this;
                            return a2Err;
                        }

                        break;
                    case "folder":
                        FolderAction a3 = action as FolderAction;
                        ValidationError a3Err = a3.IsValid();
                        if (a3Err != null)
                        {
                            a3Err.Shortcut = this;
                            return a3Err;
                        }

                        break;
                    case "software":
                        SoftwareAction a4 = action as SoftwareAction;
                        ValidationError a4Err = a4.IsValid();
                        if (a4Err != null)
                        {
                            a4Err.Shortcut = this;
                            return a4Err;
                        }

                        break;
                    case "website":
                        WebsiteAction a5 = action as WebsiteAction;
                        ValidationError a5Err = a5.IsValid();
                        if (a5Err != null)
                        {
                            a5Err.Shortcut = this;
                            return a5Err;
                        }

                        break;
                }
            }

            return null;
        }

        public Shortcut Duplicate()
        {
            List<Action> duplicateActions = new List<Action>();
            foreach (Action action in Actions)
            {
                duplicateActions.Add(action.Duplicate());
            }

            return new Shortcut(Name, IconPath, duplicateActions);
        }
    }
}
