// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace StartMenuManager.GUI
{
    public static class IconExtractorWindow_TitleBarControl
    {
        private static Point startPos;

        private static Screen[] screens = System.Windows.Forms.Screen.AllScreens;

        public static void InitEvents(IconExtractorWindow wind)
        {
            wind.TitleBar_MaximizeButton.Click += (sender, e) => Maximize_Click(wind, sender, e);
            wind.TitleBar_CloseButton.Click += (sender, e) => Close_Click(wind, sender, e);
            wind.TitleBarArea.PreviewMouseDown += (sender, e) => System_MouseDown(wind, sender, e);
            wind.TitleBarArea.PreviewMouseMove += (sender, e) => System_MouseMove(wind, sender, e);
            wind.LocationChanged += (sender, e) => Window_LocationChanged(wind, sender, e);
            wind.StateChanged += (sender, e) => Window_StateChanged(wind, sender, e);
        }

        private static void Window_LocationChanged(IconExtractorWindow wind, object sender, EventArgs e)
        {
            int sum = 0;
            foreach (var item in screens)
            {
                sum += item.WorkingArea.Width;
                if (sum >= wind.Left + (wind.Width / 2))
                {
                    wind.MaxHeight = item.WorkingArea.Height + 7;
                    break;
                }
            }
        }

        private static void System_MouseDown(IconExtractorWindow wind, object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount >= 2)
                {
                    wind.WindowState = (wind.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
                }
                else
                {
                    startPos = e.GetPosition(null);
                }
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                var pos = wind.PointToScreen(e.GetPosition(wind));
                IntPtr hWnd = new System.Windows.Interop.WindowInteropHelper(wind).Handle;
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

        private static void System_MouseMove(IconExtractorWindow wind, object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (wind.WindowState == WindowState.Maximized && Math.Abs(startPos.Y - e.GetPosition(null).Y) > 2)
                {
                    var point = wind.PointToScreen(e.GetPosition(null));

                    wind.WindowState = WindowState.Normal;

                    wind.Left = point.X - (wind.ActualWidth / 2);
                    wind.Top = point.Y - (wind.border.ActualHeight / 2);
                }

                wind.DragMove();
            }
        }

        private static void Maximize_Click(IconExtractorWindow wind, object sender, RoutedEventArgs e)
        {
            wind.WindowState = (wind.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private static void Close_Click(IconExtractorWindow wind, object sender, RoutedEventArgs e)
        {
            wind.Close();
        }

        private static void Mimimize_Click(IconExtractorWindow wind, object sender, RoutedEventArgs e)
        {
            wind.WindowState = WindowState.Minimized;
        }

        private static void Window_StateChanged(IconExtractorWindow wind, object sender, EventArgs e)
        {
            if (wind.WindowState == WindowState.Maximized)
            {
                wind.main.BorderThickness = new Thickness(0);
                wind.main.Margin = new Thickness(7, 7, 7, 0);
                wind.rectMax.Visibility = Visibility.Hidden;
                wind.rectMin.Visibility = Visibility.Visible;
            }
            else
            {
                wind.main.BorderThickness = new Thickness(1);
                wind.main.Margin = new Thickness(0);
                wind.rectMax.Visibility = Visibility.Visible;
                wind.rectMin.Visibility = Visibility.Hidden;
            }
        }
    }
}
