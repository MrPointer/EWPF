using System.Windows;

namespace EWPF.MVVM.Services
{
    /// <summary>
    /// An interface declaring methods to show a message box at the center of its' owner and on top of it.
    /// <para />
    /// The service can show windows' native message box or a custom one made by the EWPF team.
    /// </summary>
    public interface IMessageBoxService
    {
        #region Fields

        #endregion

        #region Methods

        /// <summary>
        /// Shows an EWPF themed message box based on the given parameters.
        /// </summary>
        /// <param name="i_Caption">Message box's caption(Header).</param>
        /// <param name="i_Content">Message box's content(Body).</param>
        /// <param name="i_Buttons">Message box's button/s.</param>
        /// <param name="i_Icon">Message box's icon.</param>
        /// <param name="i_DefaultResult">Message box's default return result.</param>
        /// <param name="i_ExtraOptions">Extra message box options.</param>
        /// <returns>Message box's result after an interaction with the user.</returns>
        MessageBoxResult Show(string i_Caption, string i_Content, MessageBoxButton i_Buttons, MessageBoxImage i_Icon,
            MessageBoxResult i_DefaultResult = MessageBoxResult.OK,
            MessageBoxOptions i_ExtraOptions = MessageBoxOptions.None);

        /// <summary>
        /// Shows a windows native message box based on the given parameters.
        /// </summary>
        /// <param name="i_Caption">Message box's caption(Header).</param>
        /// <param name="i_Content">Message box's content(Body).</param>
        /// <param name="i_Buttons">Message box's button/s.</param>
        /// <param name="i_Icon">Message box's icon.</param>
        /// <param name="i_DefaultResult">Message box's default return result.</param>
        /// <param name="i_ExtraOptions">Extra message box options.</param>
        /// <returns>Message box's result after an interaction with the user.</returns>
        MessageBoxResult ShowNative(string i_Caption, string i_Content, MessageBoxButton i_Buttons, MessageBoxImage i_Icon,
            MessageBoxResult i_DefaultResult = MessageBoxResult.OK, MessageBoxOptions i_ExtraOptions = MessageBoxOptions.None);

        #endregion

        #region Properties

        #endregion
    }
}