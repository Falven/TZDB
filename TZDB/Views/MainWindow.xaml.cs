using NUIClockUpdater.Properties;
using NUIClockUpdater.ViewModels;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace NUIClockUpdater.Views
{
    /// <summary>
    /// Handles user input and application logic.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        private const string PARSING_THREAD_NAME = "Parsing Thread";

        /// <summary>
        /// The thread where all of the parsing takes place (prevent ui block).
        /// </summary>
        private Thread _parsingThread;
        #endregion

        #region Constructors
        public MainWindow()
        {
            // Load connectionstrings before bindings take effect.
            Updater = new ViewModels.Updater();
            Updater.LoadConnectionString();

            InitializeComponent();

            ConnectionStringTextBox.DataContext = Updater;
            ProgressBar.DataContext = Updater;
            ProgressTextBlock.DataContext = Updater;

            Updater.ParsingFinished += OnNUIClockUpdaterParsingFinished;

            _parsingThread = new Thread(Updater.BeginParsing);
            _parsingThread.Name = PARSING_THREAD_NAME;

            App.Current.Exit += OnAppExit;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The updater that performs the parsing and uploading of the olson and geonames database files.
        /// </summary>
        public NUIClockUpdater.ViewModels.Updater Updater { get; private set; }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Event raised when the Geonames browse button gets clicked.
        /// </summary>
        /// <param name="sender">The Geonames browse button.</param>
        /// <param name="e">The event arguments for this handler.</param>
        private void OnGeonamesBrowseButtonClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            string geoPath = Settings.Default.GeonamesPath;
            fbd.SelectedPath = geoPath;
            DialogResult result = fbd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Settings.Default.GeonamesPath = fbd.SelectedPath;
            }
        }

        /// <summary>
        /// Event raised when the Olson browse button gets clicked.
        /// </summary>
        /// <param name="sender">The Olson browse button.</param>
        /// <param name="e">The event arguments for this handler.</param>
        private void OnOlsonBrowseButtonClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Settings.Default.OlsonPath;
            DialogResult result = fbd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Settings.Default.OlsonPath = fbd.SelectedPath;
            }
        }

        /// <summary>
        /// Event raised when the submit button is clicked.
        /// </summary>
        /// <param name="sender">The submit button.</param>
        /// <param name="e">The event arguments for this handler.</param>
        private void OnSubmitButtonClick(object sender, RoutedEventArgs rea)
        {
            SetControlAvailability(false);
            _parsingThread.Start();
        }

        private void OnNUIClockUpdaterParsingFinished(object sender, ParsingFinishedEventArgs pfea)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                ((System.Action)(() =>
                {
                    if (pfea.Result == ParsingResult.Success)
                    {
                        Updater.ShowOutput();
                    }
                    SetControlAvailability(true);
                    _parsingThread.Abort();
                    _parsingThread = new Thread(Updater.BeginParsing);
                    _parsingThread.Name = PARSING_THREAD_NAME;
                })));
        }

        /// <summary>
        /// Event raised whenever the application is exiting.
        /// </summary>
        /// <param name="sender">The app class that raised the event.</param>
        /// <param name="e">The exit event arguments for this event.</param>
        private void OnAppExit(object sender, ExitEventArgs e)
        {
            if (null != _parsingThread)
            {
                _parsingThread.Abort();
            }
            Updater.SaveConnectionString();
        }
        #endregion

        #region Members
        /// <summary>
        /// Sets all user input controls to the provided availability.
        /// </summary>
        /// <param name="val">True for enabled, false for disabled.</param>
        private void SetControlAvailability(bool val)
        {
            GeonamesBrowseTextBox.IsEnabled = val;
            OlsonBrowseTextBox.IsEnabled = val;
            GeonamesBrowseButton.IsEnabled = val;
            OlsonBrowseButton.IsEnabled = val;
            ConnectionStringTextBox.IsEnabled = val;
            SubmitButton.IsEnabled = val;
        }
        #endregion
    }
}