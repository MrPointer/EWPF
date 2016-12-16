﻿using System.Windows;
using EWPF.MVVM.Services;

namespace EWPF.Utility
{
    /// <summary>
    /// A static utility class providing utility methods to easily implement <see cref="IWindowService"/>'s
    /// methods in a generic straightforward way.
    /// </summary>
    public static class WindowUtility
    {
        #region Events

        #endregion

        #region Fields



        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Closes the given window by setting its' <see cref="Window.DialogResult"/> property to the given result.
        /// </summary>
        /// <param name="i_Window">Reference to the window that should be closed.</param>
        /// <param name="i_WindowResult">Window's dialog result.</param>
        public static void CloseWindow(Window i_Window, bool? i_WindowResult)
        {
            i_Window.DialogResult = i_WindowResult;
            i_Window.Close();
        }

        #endregion

        #region Properties



        #endregion
    }
}