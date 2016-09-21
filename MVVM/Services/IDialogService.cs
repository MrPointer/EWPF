using System.IO;
using Microsoft.Win32;

namespace EWPF.MVVM.Services
{
    /// <summary>
    /// An interface declaring methods to show various types of dialogs with almost fully customized content.
    /// <para/>
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
        /// <param name="i_Caption">Dialog's title/caption.</param>
        /// <param name="i_Content">Dialog's content, which can be anything.
        /// The idea behind this design is to allow users to create data templates for different types.</param>
        /// <param name="i_IsCenterOwner">Indicates weather the dialog will be displayed at the center of it's owner window or not.</param>
        /// <returns>Dialog result as a nullable boolean.</returns>
        bool? ShowDialog(string i_Caption, object i_Content, bool i_IsCenterOwner = true);

        /// <summary>
        /// Shows an <see cref="OpenFileDialog"/>, native to the Windows operating system, filtering the results to the given extensions.
        /// </summary>
        /// <param name="i_FileExtenstions">File extensions to browse for.</param>
        /// <param name="i_DefaultExtension">Default file extension to browse for - The one that will be filtered at first.</param>
        /// <returns><see cref="FileInfo"/> object containing the browsed file info.</returns>
        FileInfo BrowseFile(string i_FileExtenstions, string i_DefaultExtension);

        #endregion

        #region Properties

        #endregion
    }
}