// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using StartMenuManager.GUI.Dialogs;
using static StartMenuManager.GUI.WindowRef;

namespace StartMenuManager.GUI
{
    public static class TitleBarControl
    {
        private static Point startPos;

        private static Screen[] screens = System.Windows.Forms.Screen.AllScreens;

        public static void InitEvents()
        {
            Wind.TitleBar_MinimizeButton.Click += (sender, e) => Mimimize_Click(sender, e);
            Wind.TitleBar_MaximizeButton.Click += (sender, e) => Maximize_Click(sender, e);
            Wind.TitleBar_CloseButton.Click += (sender, e) => Close_Click(sender, e);
            Wind.TitleBarArea.PreviewMouseDown += (sender, e) => System_MouseDown(sender, e);
            Wind.TitleBarArea.PreviewMouseMove += (sender, e) => System_MouseMove(sender, e);
            Wind.LocationChanged += (sender, e) => Window_LocationChanged(sender, e);
            Wind.StateChanged += (sender, e) => Window_StateChanged(sender, e);
        }

        private static void Window_LocationChanged(object sender, EventArgs e)
        {
            int sum = 0;
            foreach (var item in screens)
            {
                sum += item.WorkingArea.Width;
                if (sum >= Wind.Left + (Wind.Width / 2))
                {
                    Wind.MaxHeight = item.WorkingArea.Height + 7;
                    break;
                }
            }
        }

        private static void System_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount >= 2)
                {
                    Wind.WindowState = (Wind.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
                }
                else
                {
                    startPos = e.GetPosition(null);
                }
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                var pos = Wind.PointToScreen(e.GetPosition(Wind));
                IntPtr hWnd = new System.Windows.Interop.WindowInteropHelper(Wind).Handle;
                IntPtr hMenu = GetSystemMenu(hWnd, false);
                int cmd = TrackPopupMenu(hMenu, 0x100, (int)pos.X, (int)pos.Y, 0, hWnd, IntPtr.Zero);
                if (cmd > 0)
                {
                    SendMessage(hWnd, 0x112, (IntPtr)cmd, IntPtr.Zero);
                }
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

        private static void System_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (Wind.WindowState == WindowState.Maximized && Math.Abs(startPos.Y - e.GetPosition(null).Y) > 2)
                {
                    var point = Wind.PointToScreen(e.GetPosition(null));

                    Wind.WindowState = WindowState.Normal;

                    Wind.Left = point.X - (Wind.ActualWidth / 2);
                    Wind.Top = point.Y - (Wind.border.ActualHeight / 2);
                }

                Wind.DragMove();
            }
        }

        private static void Maximize_Click(object sender, RoutedEventArgs e)
        {
            Wind.WindowState = (Wind.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private static void Close_Click(object sender, RoutedEventArgs e)
        {
            if (Wind.Settings.YesNoDialogsEnabled)
            {
                YesNoDialog.SetMessage("Are you sure you want to quit?", "You will loose any changes.", "Yes", "No", CloseComplete);
                DialogManager.Show(Structures.DialogTypes.YesNoDialog);
            }
            else
            {
                CloseComplete(true);
            }
        }

        private static void CloseComplete(bool shouldClose)
        {
            if (shouldClose)
            {
                Wind.Close();
            }
        }

        private static void Mimimize_Click(object sender, RoutedEventArgs e)
        {
            Wind.WindowState = WindowState.Minimized;
        }

        private static void Window_StateChanged(object sender, EventArgs e)
        {
            if (Wind.WindowState == WindowState.Maximized)
            {
                Wind.main.BorderThickness = new Thickness(0);
                Wind.main.Margin = new Thickness(7, 7, 7, 0);
                Wind.rectMax.Visibility = Visibility.Hidden;
                Wind.rectMin.Visibility = Visibility.Visible;
            }
            else
            {
                Wind.main.BorderThickness = new Thickness(1);
                Wind.main.Margin = new Thickness(0);
                Wind.rectMax.Visibility = Visibility.Visible;
                Wind.rectMin.Visibility = Visibility.Hidden;
            }
        }
    }
}
