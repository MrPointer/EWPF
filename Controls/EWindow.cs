// PNotes.NET - open source desktop notes manager
// Copyright (C) 2015 Andrey Gruber

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace EWPF.Controls
{
    public enum EWindowStyle
    {
        Normal,
        MessageBox,
        NoBorder
    }

    internal static class LocalExtensions
    {
        public static void ForWindowFromChild(this object i_ChildDependencyObject, Action<Window> i_Action)
        {
            var element = i_ChildDependencyObject as DependencyObject;
            while (element != null)
            {
                element = VisualTreeHelper.GetParent(element);
                if (element is Window) { i_Action(element as Window); break; }
            }
        }

        public static void ForWindowFromTemplate(this object i_TemplateFrameworkElement, Action<Window> i_Action)
        {
            var window = ((FrameworkElement)i_TemplateFrameworkElement).TemplatedParent as Window;
            if (window != null) i_Action(window);
        }

        public static Window WindowFromTemplate(this object i_TemplateFrameworkElement)
        {
            return ((FrameworkElement)i_TemplateFrameworkElement).TemplatedParent as Window;
        }

        public static IntPtr GetWindowHandle(this Window i_Window)
        {
            var helper = new WindowInteropHelper(i_Window);
            return helper.Handle;
        }
    }

    public partial class EWindow
    {
        public static readonly DependencyProperty WindowBorderBroperty =
            DependencyProperty.RegisterAttached("WindowBorder", typeof(EWindowStyle),
                typeof(EWindow), new UIPropertyMetadata(EWindowStyle.Normal));

        public static EWindowStyle GetWindowBorder(DependencyObject obj)
        {
            return (EWindowStyle)obj.GetValue(WindowBorderBroperty);
        }

        public static void SetWindowBorder(DependencyObject obj, EWindowStyle value)
        {
            obj.SetValue(WindowBorderBroperty, value);
        }

        #region Event Handlers

        #region OnSize

        private void OnSizeSouth(object sender, MouseButtonEventArgs e)
        {
            OnSize(sender, SizingAction.South);
        }

        private void OnSizeNorth(object sender, MouseButtonEventArgs e)
        {
            OnSize(sender, SizingAction.North);
        }

        private void OnSizeEast(object sender, MouseButtonEventArgs e)
        {
            OnSize(sender,
                ((FrameworkElement)sender).FlowDirection == FlowDirection.LeftToRight
                    ? SizingAction.East
                    : SizingAction.West);
        }

        private void OnSizeWest(object sender, MouseButtonEventArgs e)
        {
            OnSize(sender,
                ((FrameworkElement)sender).FlowDirection == FlowDirection.LeftToRight
                    ? SizingAction.West
                    : SizingAction.East);
        }

        private void OnSizeNorthWest(object sender, MouseButtonEventArgs e)
        {
            OnSize(sender,
                ((FrameworkElement)sender).FlowDirection == FlowDirection.LeftToRight
                    ? SizingAction.NorthWest
                    : SizingAction.NorthEast);
        }

        private void OnSizeNorthEast(object sender, MouseButtonEventArgs e)
        {
            OnSize(sender,
                ((FrameworkElement)sender).FlowDirection == FlowDirection.LeftToRight
                    ? SizingAction.NorthEast
                    : SizingAction.NorthWest);
        }

        private void OnSizeSouthEast(object sender, MouseButtonEventArgs e)
        {
            OnSize(sender,
                ((FrameworkElement)sender).FlowDirection == FlowDirection.LeftToRight
                    ? SizingAction.SouthEast
                    : SizingAction.SouthWest);
        }
        private void OnSizeSouthWest(object sender, MouseButtonEventArgs e)
        {
            OnSize(sender,
                ((FrameworkElement)sender).FlowDirection == FlowDirection.LeftToRight
                    ? SizingAction.SouthWest
                    : SizingAction.SouthEast);
        }

        private void OnSize(object sender, SizingAction action)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                sender.ForWindowFromTemplate(w =>
                {
                    if (w.WindowState == WindowState.Normal)
                        DragSize(w.GetWindowHandle(), action);
                });
            }
        }

        #endregion

        #region Titlebar Buttons

        private void IconMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                sender.ForWindowFromTemplate(w => w.Close());
            }
            else
            {
                sender.ForWindowFromTemplate(w =>
                    SendMessage(w.GetWindowHandle(), WM_SYSCOMMAND, (IntPtr)SC_KEYMENU, (IntPtr)' '));
            }
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.Close());
        }

        private void MinButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.WindowState = WindowState.Minimized);
        }

        private void MaxButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(
                i_W =>
                {
                    i_W.WindowState = i_W.WindowState == WindowState.Maximized
                        ? WindowState.Normal
                        : WindowState.Maximized;
                });

        }

        private void TitleBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = sender.WindowFromTemplate();
            if (window == null) return;

            if (IsFullScreenWindow(window))
                return;

            if (e.ClickCount > 1 && (window.ResizeMode == ResizeMode.CanResize ||
                                     window.ResizeMode == ResizeMode.CanResizeWithGrip))
            {
                MaxButtonClick(sender, e);
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                sender.ForWindowFromTemplate(w => w.DragMove());
            }
        }

        private void TitleBarMouseMove(object sender, MouseEventArgs e)
        {
            var window = sender.WindowFromTemplate();
            if (window == null) return;

            if (IsFullScreenWindow(window))
                return;

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            if (window.WindowState != WindowState.Maximized)
                return;

            window.BeginInit();

            const double adjustment = 40.0;
            var mouse1 = e.MouseDevice.GetPosition(window);
            double width1 = Math.Max(window.ActualWidth - 2 * adjustment, adjustment);

            window.WindowState = WindowState.Normal;

            double width2 = Math.Max(window.ActualWidth - 2 * adjustment, adjustment);
            window.Left = (mouse1.X - adjustment) * (1 - width2 / width1);
            window.Top = -7;

            window.EndInit();
            window.DragMove();
        }

        /// <summary>
        /// Checks whether a given window is in full screen mode by checking whether its' 
        /// <see cref="ResizeMode"/> is set to <see cref="ResizeMode.NoResize"/> and its' 
        /// <see cref="WindowState"/> is set to <see cref="WindowState.Maximized"/>.
        /// </summary>
        /// <param name="i_TargetWindow">Window to check.</param>
        /// <returns>True if window is in full screen, false otherwise.</returns>
        private bool IsFullScreenWindow(Window i_TargetWindow)
        {
            return i_TargetWindow.ResizeMode == ResizeMode.NoResize &&
                   i_TargetWindow.WindowState == WindowState.Maximized;
        }

        #endregion

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            sender.ForWindowFromTemplate(
                i_W =>
                {
                    if (i_W.WindowState != WindowState.Maximized) return;
                    i_W.MaxHeight = SystemParameters.WorkArea.Height + 14;
                    i_W.MaxWidth = SystemParameters.WorkArea.Width + 14;
                });
        }

        #endregion

        #region P/Invoke

        private const int WM_SYSCOMMAND = 0x112;
        private const int WM_LBUTTONUP = 0x0202;
        private const int SC_SIZE = 0xF000;
        private const int SC_KEYMENU = 0xF100;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        void DragSize(IntPtr handle, SizingAction sizingAction)
        {
            SendMessage(handle, WM_SYSCOMMAND, (IntPtr)(SC_SIZE + sizingAction), IntPtr.Zero);
            SendMessage(handle, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
        }

        public enum SizingAction
        {
            North = 3,
            South = 6,
            East = 2,
            West = 1,
            NorthEast = 5,
            NorthWest = 4,
            SouthEast = 8,
            SouthWest = 7
        }

        #endregion
    }
}