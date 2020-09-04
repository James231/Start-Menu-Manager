// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Windows;
using StartMenuManager.Core.DataStructures;
using StartMenuManager.Core.Serialization;
using StartMenuManager.GUI.Extensions;
using static StartMenuManager.GUI.WindowRef;

namespace StartMenuManager.GUI
{
    public static class ShortcutListArea
    {
        public static List<ShortcutControl> Shortcuts { get; set; } = new List<ShortcutControl> { };

        public static void InitEvents()
        {
            Wind.ShortcutListArea_AddButton.Click += (sender, args) => AddShortcut(sender, args);
        }

        public static void AddShortcut(object sender, RoutedEventArgs args)
        {
            ShortcutControl newControl = new ShortcutControl();
            Wind.ShortcutListArea_ShortcutsParent.Children.Add(newControl);
            Shortcuts.Add(newControl);
        }

        private static ShortcutControl AddShortcut(Shortcut shortcut)
        {
            return AddShortcut(shortcut, Shortcuts.Count);
        }

        private static ShortcutControl AddShortcut(Shortcut shortcut, int insertIndex)
        {
            ShortcutControl newControl = new ShortcutControl();
            newControl.Shortcut = shortcut;

            if (shortcut.Actions == null)
            {
                shortcut.Actions = new List<Action>();
            }

            if (shortcut.Actions.Count != 1)
            {
                newControl.ShortcutType = Structures.ShortcutType.Multi;
                newControl.CreateGroupActions(shortcut.Actions);
            }
            else
            {
                newControl.ShortcutType = shortcut.Actions[0].ToShortcutType();
            }

            Wind.ShortcutListArea_ShortcutsParent.Children.Insert(insertIndex, newControl);
            Shortcuts.Insert(insertIndex, newControl);

            newControl.UpdateUi();
            return newControl;
        }

        private static bool IsFolder(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return true;
            }

            return false;
        }

        public static void RemoveShortcut(ShortcutControl control)
        {
            if (Wind.Settings.YesNoDialogsEnabled)
            {
                Dialogs.YesNoDialog.SetMessage("Are you sure?", $"Do you want to remove the shortcut \"{control.Shortcut.Name}\"?", "Yes", "No", (isYes) => RemoveShortcutYes(isYes, control));
                DialogManager.Show(Structures.DialogTypes.YesNoDialog);
            }
            else
            {
                RemoveShortcutYes(true, control);
            }
        }

        private static void RemoveShortcutYes(bool isYes, ShortcutControl control)
        {
            if (isYes)
            {
                Shortcuts.Remove(control);
                Wind.ShortcutListArea_ShortcutsParent.Children.Remove(control);
            }
        }

        public static void RemoveAllShortcuts()
        {
            Shortcuts.Clear();
            Wind.ShortcutListArea_ShortcutsParent.Children.Clear();
        }

        public static void LoadShortcuts(List<Shortcut> shortcuts)
        {
            RemoveAllShortcuts();

            if (shortcuts == null)
            {
                return;
            }

            foreach (Shortcut s in shortcuts)
            {
                AddShortcut(s);
            }
        }

        public static Config GetConfig()
        {
            Config newConfig = new Config();
            newConfig.Shortcuts = new List<Shortcut>();

            foreach (ShortcutControl shortcutControl in Shortcuts)
            {
                newConfig.Shortcuts.Add(shortcutControl.Shortcut);
            }

            return newConfig;
        }

        public static void TestRunShortcut(Shortcut shortcut, bool isSub = false)
        {
            // Validate the shortcut (individually) first
            ValidationError err = shortcut.IsValid();

            if (err != null)
            {
                Wind.ValiationError.Visibility = Visibility.Visible;
                Wind.ValidationErrorText.Text = isSub ? $"Error: {err.Error}" : $"Error: \"{err.Shortcut.Name}\" {err.Error}";
                return;
            }
            else
            {
                Wind.ValiationError.Visibility = Visibility.Collapsed;
            }

            string tempShortcutJsonPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Runner", "temp,json");
            string shortcutJson = Serializer.SerializeShortcut(shortcut);
            StreamWriter writer = new StreamWriter(tempShortcutJsonPath, false);
            writer.Write(shortcutJson);
            writer.Close();

            StartMenuManager.GUI.Runner.Run(tempShortcutJsonPath);
        }

        public static void DuplicateShortcut(ShortcutControl control)
        {
            int insertIndex = Shortcuts.IndexOf(control) + 1;
            Shortcut newShortcut = control.Shortcut.Duplicate();
            ShortcutControl newControl = AddShortcut(newShortcut, insertIndex);
        }

        public static void MoveUpShortcut(ShortcutControl control)
        {
            int oldIndex = Shortcuts.IndexOf(control);
            if (oldIndex > 0)
            {
                Shortcuts.Remove(control);
                Wind.ShortcutListArea_ShortcutsParent.Children.Remove(control);
                Shortcuts.Insert(oldIndex - 1, control);
                Wind.ShortcutListArea_ShortcutsParent.Children.Insert(oldIndex - 1, control);
            }
        }

        public static void MoveDownShortcut(ShortcutControl control)
        {
            int oldIndex = Shortcuts.IndexOf(control);
            if (oldIndex < Shortcuts.Count - 1)
            {
                Shortcuts.Remove(control);
                Wind.ShortcutListArea_ShortcutsParent.Children.Remove(control);
                Shortcuts.Insert(oldIndex + 1, control);
                Wind.ShortcutListArea_ShortcutsParent.Children.Insert(oldIndex + 1, control);
            }
        }
    }
}
