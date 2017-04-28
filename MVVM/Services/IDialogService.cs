using System;
using System.IO;
using System.Windows;
using EWPF.Utility;
using Microsoft.Win32;

namespace EWPF.MVVM.Services
{
    /// <summary>
    /// An interface declaring methods to show various types of dialogs 
    /// with almost fully customized content. <br />
    /// This is a service-like interface implementing the DI pattern.
    /// </summary>
    public interface IDialogService : IService
    {
        #region Fields

        #endregion

        #region Methods

        /// <summary>
        /// Shows an EWPF themed dialog on top of the visible window, filled with the given parameters.
        /// </summary>
        /// <param name="i_Content">Dialog's content, which can be anything.</param>
        /// <param name="i_Caption">Dialog's title/caption.</param>
        /// <param name="i_IsCenterOwner">Indicates weather the dialog will be displayed at the center of it's owner window or not.</param>
        /// <returns>Dialog result as a nullable boolean.</returns>
        /// <remarks><para>The method's design provides multiple approaches towards its' implementation.</para>
        /// <para>The recommended 'easy-to-implement' approach is to create an enum for various dialog types and pass them as
        /// the sole argument to this method, parsing them later on the view with a 'switch-case' block.</para>
        /// <para>Another possible approach is to pass some kind of a template, probably represented by a code since this method
        /// is called from the view-model. The template will be the body of the dialog while the passed string will be its' title.</para>
        /// </remarks>
        bool? ShowDialog(object i_Content, string i_Caption = null, bool i_IsCenterOwner = true);

        /// <summary>
        /// Displays a user-designed <see cref="Window"/> on top of the current one, reflecting to it as a modal dialog. <br />
        /// It's highly recommended to simply call the <see cref="DialogUtility.ShowDialog"/> method 
        /// that will find the correct dialog to display based on the <see cref="DialogAttribute"/> it has and the given name.
        /// </summary>
        /// <param name="i_DialogName">Name of the dialog to open. 
        /// Must be the same as stated in the <see cref="DialogAttribute"/>, if stated at all.</param>
        /// <returns>Dialog result.</returns>
        bool? ShowDialog(string i_DialogName);

        /// <summary>
        /// Displays an <see cref="OpenFileDialog"/>, native to the Windows operating system, filtering the results to the given extensions.
        /// </summary>
        /// <param name="i_FileExtensions">File extensions to browse for.</param>
        /// <param name="i_DefaultExtension">Default file extension to browse for - The one that will be filtered at first.</param>
        /// <param name="i_InitialLocation">Dialog's initial location - The default path which the dialog will resolve upon startup.</param>
        /// <returns><see cref="FileInfo"/>FileInfo object containing the browsed file's info.</returns>
        FileInfo BrowseFile(string i_FileExtensions, string i_DefaultExtension, string i_InitialLocation = null);

        /// <summary>
        /// Displays an <see cref="SaveFileDialog"/>, native to the Windows operating system, filtering the results to the given extensions.
        /// </summary>
        /// <param name="i_FileExtensions">File extensions to save as.</param>
        /// <param name="i_DefaultExtension">Default file extension to save as - The one that will be filtered at first.</param>
        /// <param name="i_InitialLocation">Dialog's initial location - The default path which the dialog will resolve upon startup.</param>
        /// <returns><see cref="FileInfo"/>FileInfo object containing the browsed file's info.</returns>
        FileInfo SaveFile(string i_FileExtensions, string i_DefaultExtension, string i_InitialLocation = null);

        #endregion

        #region Properties

        #endregion
    }

    /// <summary>
    /// An attribute class designed to mark a class as Dialog window, 
    /// which could then could be utilized by a <see cref="IDialogService"/> to easily find and open.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DialogAttribute : Attribute
    {
        /// <inheritdoc />
        public DialogAttribute(string i_Name)
        {
            Name = i_Name;
        }

        /// <summary>
        /// Gets or sets the name of the dialog.
        /// </summary>
        public string Name { get; set; }
    }
}