using NUIClockUpdater.Models;
using NUIClockUpdater.Properties;
using NUIClockUpdater.ViewModels;
using NUIClockUpdater.Views;
using System.Threading;
using System.Windows;

namespace NUIClockUpdater
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public partial class App : Application
    {
        #region Constructors
        public App()
        {
            ExceptionLogger = new ExceptionLogger();

            Exit += OnAppExit;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Logs and manages program exceptions.
        /// </summary>
        internal static ExceptionLogger ExceptionLogger { get; private set; }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Event handler raised whenever the application is exiting.
        /// </summary>
        /// <param name="sender">This App class.</param>
        /// <param name="e">The event arguments for this exit event.</param>
        private void OnAppExit(object sender, ExitEventArgs e)
        {
            Settings.Default.Save();
        }
        #endregion
    }
}
