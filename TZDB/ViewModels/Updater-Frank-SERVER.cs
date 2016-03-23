using MetroClockUpdater.Models;
using NUIClockUpdater.Models;
using NUIClockUpdater.Properties;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NUIClockUpdater.ViewModels
{
    public class Updater : UINotifications, System.IDisposable
    {
        #region Fields
        /// <summary>
        /// The default connection string to display.
        /// </summary>
        private const string DEFAULT_CONSTR = "Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;";

        /// <summary>
        /// The name to use when saving the conneciton string to the program's config file.
        /// </summary>
        private const string DEFAULT_CONSTR_NAME = "ConStr";

        /// <summary>
        /// Amount of time before a bulkcopy operation times out.
        /// </summary>
        private const int NET_TIMEOUT = 150;

        /// <summary>
        /// Utility to parse olson database files.
        /// </summary>
        private OlsonParser _olsonParser;

        /// <summary>
        /// Reference to an olson timezone lookup table to apply correct timeZone id's.
        /// </summary>
        private ConcurrentDictionary<string, System.Data.DataRow> _timeZoneLookUpTable;

        /// <summary>
        /// DirectoryInfo containing all of the valid Olson files.
        /// </summary>
        private OlsonDirectoryInfo _olsonDirectoryInfo;

        /// <summary>
        /// Utility to parse geonames database files.
        /// </summary>
        private GeonamesParser _geonamesParser;

        /// <summary>
        /// DirectoryInfo containing all of the valid Geonames files.
        /// </summary>
        private GeonamesDirectoryInfo _geonamesDirectoryInfo;

        /// <summary>
        /// The configuration for this program.
        /// </summary>
        private Configuration _config;

        /// <summary>
        /// The connection string to connect to the Database.
        /// </summary>
        private SqlConnectionStringBuilder _conStrBuilder;

        /// <summary>
        /// Database tools to create the Database tables, retrieve DataTables...
        /// </summary>
        private WorlTimeDatabaseTools _dbTools;

        /// <summary>
        /// The current progress out of maximum.
        /// </summary>
        private double _progressValue;

        /// <summary>
        /// The maximum progress to reach.
        /// </summary>
        private double _progressMaximum;

        /// <summary>
        /// Textual information relating to the current parsing status.
        /// </summary>
        private string _progressText;

        /// <summary>
        /// Lock object used for thread safe access to progress information.
        /// </summary>
        private object _progressLock;

        /// <summary>
        /// Object used to lock concurrent access.
        /// </summary>
        private object _cityTableLock;
        #endregion

        #region Constructors
        public Updater()
        {
            _progressLock = new object();
            _cityTableLock = new object();

            _progressValue = 0D;
            _progressMaximum = 0D;
            _progressText = string.Empty;

            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            _conStrBuilder = new SqlConnectionStringBuilder(DEFAULT_CONSTR);
        }
        #endregion

        #region Events
        /// <summary>
        /// Raised when parsing of the geonames and olson database files has begun.
        /// </summary>
        public event System.EventHandler<System.EventArgs> ParsingStarted;

        /// <summary>
        /// Raised when all of the geonames and olson database files have been parsed and uploaded.
        /// </summary>
        public event System.EventHandler<ParsingFinishedEventArgs> ParsingFinished;
        #endregion

        #region Properties
        /// <summary>
        /// The Connection string being used.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _conStrBuilder.ConnectionString;
            }
            set
            {
                NotifyPropertyChanging("ConnectionString");
                try
                {
                    _conStrBuilder = new SqlConnectionStringBuilder(value);
                }
                catch (System.FormatException e)
                {
                    App.ExceptionLogger.LogMedium(e);
                }
                catch (System.ArgumentException ae)
                {
                    App.ExceptionLogger.LogMedium(ae);
                }
                NotifyPropertyChanged("ConnectionString");
            }
        }

        /// <summary>
        /// The current progress out of maximum.
        /// </summary>
        public double ProgressValue
        {
            get { return _progressValue; }
            private set
            {
                NotifyPropertyChanging("ProgressValue");
                lock (_progressLock)
                {
                    _progressValue = value;
                }
                NotifyPropertyChanged("ProgressValue");
            }
        }

        /// <summary>
        /// The maximum progress to reach.
        /// </summary>
        public double ProgressMaximum
        {
            get { return _progressMaximum; }
            private set
            {
                NotifyPropertyChanging("ProgressMaximum");
                lock (_progressLock)
                {
                    _progressMaximum = value;
                }
                NotifyPropertyChanged("ProgressMaximum");
            }
        }

        /// <summary>
        /// Textual information relating to the current parsing status.
        /// </summary>
        public string ProgressText
        {
            get { return _progressText; }
            private set
            {
                NotifyPropertyChanging("ProgressText");
                lock (_progressLock)
                {
                    _progressText = value;
                }
                NotifyPropertyChanged("ProgressText");
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// Loads the connection string from the configuration file for this application.
        /// </summary>
        public void LoadConnectionString()
        {
            foreach (ConnectionStringSettings setting in _config.ConnectionStrings.ConnectionStrings)
            {
                if (setting.Name.Equals(DEFAULT_CONSTR_NAME))
                {
                    ConnectionString = setting.ConnectionString;
                    return;
                }
            }
        }

        /// <summary>
        /// Saves the provided connectionString to the program's configurations file using the default name, and Rsa encryption.
        /// </summary>
        /// <param name="connectionString">The connection string to save.</param>
        public void SaveConnectionString()
        {
            if (!ConnectionString.Equals(string.Empty) && !ConnectionString.Equals(DEFAULT_CONSTR))
            {
                ConnectionStringsSection section = _config.ConnectionStrings;
                SectionInformation sectionInfo = section.SectionInformation;
                if (!sectionInfo.IsProtected)
                {
                    sectionInfo.ProtectSection("RsaProtectedConfigurationProvider");
                }
                ConnectionStringSettings existingSection = section.ConnectionStrings[DEFAULT_CONSTR_NAME];
                if (null != existingSection)
                {
                    section.ConnectionStrings.Remove(existingSection);
                }
                section.ConnectionStrings.Add(new ConnectionStringSettings(DEFAULT_CONSTR_NAME, _conStrBuilder.ConnectionString));
                _config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }
        }

        /// <summary>
        /// Instantiates and begins parsing the geonames and olson databases.
        /// </summary>
        public void BeginParsing()
        {
            RaiseParsingStarted();

            ProgressText = "Verifying directories";

            string olsonPath = Settings.Default.OlsonPath;
            string geonamesPath = Settings.Default.GeonamesPath;

            bool isValidOlsonDir = false;
            if (!string.IsNullOrWhiteSpace(olsonPath))
            {
                _olsonDirectoryInfo = new OlsonDirectoryInfo(Settings.Default.OlsonPath);
                isValidOlsonDir = _olsonDirectoryInfo.IsValid;
            }

            bool isValidGeonamesDir = false;
            if (!string.IsNullOrWhiteSpace(geonamesPath))
            {
                _geonamesDirectoryInfo = new GeonamesDirectoryInfo(Settings.Default.GeonamesPath);
                isValidGeonamesDir = _geonamesDirectoryInfo.IsValid;
            }

            if (!isValidOlsonDir && !isValidGeonamesDir)
            {
                // No need to lock, because only 1 thread accessing this. (UI Thread never accesses [Only creates it in app.xaml.cs])
                App.ExceptionLogger.LogMedium(new InvalidDirectoryException("You must provide a valid Geonames directory, Olson directory, or both."));
                RaiseParsingFinished(new ParsingFinishedEventArgs(ParsingResult.Failure));
                return;
            }

            ProgressText = "Validating connection string";

            using (SqlConnection conTest = new SqlConnection(ConnectionString))
            {
                try
                {
                    conTest.Open();
                }
                catch (System.Exception e)
                {
                    App.ExceptionLogger.LogMedium(e);
                    RaiseParsingFinished(new ParsingFinishedEventArgs(ParsingResult.Failure));
                    return;
                }
            }

            _dbTools = new WorlTimeDatabaseTools(_conStrBuilder);

            TaskFactory f = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);

            Task stage1 = f.StartNew(() => PopulateFiles(isValidGeonamesDir, isValidOlsonDir));

            Task stage3 = null;
            Task stage4 = null;
            Task stage5 = null;

            if (isValidOlsonDir)
            {
                _olsonParser = new OlsonParser(_dbTools.GetTimeZoneDataTable(), _dbTools.GetRuleDataTable(), _olsonDirectoryInfo);

                _olsonParser.FileParsing += OnParserFileParsing;

                _olsonParser.FileParsed += (s, e) => ProgressValue++;

                _timeZoneLookUpTable = _olsonParser.TimeZoneLookupTable;

                ProgressText = "Creating Olson database tables";
                _dbTools.CreateOlsonDatabaseTables();

                Task stage2 = f.StartNew(
                    () =>
                    {
                        ProgressText = "Parsing Olson database files";
                        _olsonParser.ReadTZDirectory();
                    });
                Task.WaitAll(stage2);

                stage3 = f.StartNew(
                    () =>
                    {
                        ProgressText = "Uploading TimeZones";
                        BulkCopy(_olsonParser.TimeZones, false);
                        ProgressText = "TimeZones successfully uploaded";
                    });
                stage4 = f.StartNew(
                    () =>
                    {
                        ProgressText = "Uploading Rules";
                        BulkCopy(_olsonParser.Rules, false);
                        ProgressText = "Rules successfully uploaded";
                    });
            }

            Task stage6 = null;

            if (isValidGeonamesDir)
            {
                DataTable cityTable = _dbTools.GetCityDataTable();

                _geonamesParser = new GeonamesParser(_dbTools.GetFeatureCodeDataTable(), _geonamesDirectoryInfo);

                ProgressText = "Creating Geonames database table";
                _dbTools.CreateGeonamesDatabaseTables();

                stage6 = f.StartNew(
                    () =>
                    {
                        ProgressMaximum++;
                        ProgressText = "Parsing geonames featurecodes file";
                        DataTable featureCodes = _geonamesParser.ReadFeatureCodesFile();
                        ProgressText = "Uploading geonames featurecodes file";
                        BulkCopy(featureCodes);
                    });

                _geonamesParser.EntryParsed +=
                    (s, e) =>
                    {
                        object[] cityValues = e.DatabaseEntry;
                        DataRow timeZone;

                        string timeZoneName = (string)cityValues[19];
                        if (_timeZoneLookUpTable.TryGetValue(timeZoneName, out timeZone))
                        {
                            cityValues[18] = timeZone[0];
                            lock (_cityTableLock)
                            {
                                cityTable.Rows.Add(cityValues);
                            }
                        }
                        else
                        {
                            App.ExceptionLogger.LogLow(new System.Exception("Could not get the timezone_id for the City: "
                                + (string)cityValues[1] + " with TimeZoneName " + timeZoneName));
                        }
                    };

                _geonamesParser.FileParsing += OnParserFileParsing;

                System.Action<FileParseEventArgs> BatchParsed =
                    (e) =>
                    {
                        string name = e.ParsedFile.Name;
                        ProgressText = "Uploading File " + name;
                        lock (_cityTableLock)
                        {
                            BulkCopy(cityTable);
                            cityTable.Rows.Clear();
                        }
                        ProgressText = name + " successfully uploaded";
                    };

                _geonamesParser.FileParsed += (s, e) => BatchParsed(e);

                ProgressText = "Parsing Geonames city files";
                _geonamesParser.ReadCityFiles();
            }

            Task.WaitAll(stage1);

            if (isValidOlsonDir)
            {
                Task.WaitAll(stage3, stage4);
            }

            if (isValidGeonamesDir)
            {
                Task.WaitAll(stage6);
            }

            RaiseParsingFinished(new ParsingFinishedEventArgs(ParsingResult.Success));
        }

        /// <summary>
        /// Adds the Geonames and Olson filenames to be processed into the first pipeline.
        /// </summary>
        /// <param name="olsonOutput">The pipeline to contain the Geonames and Olson files.</param>
        private void PopulateFiles(bool populateGeonamesFiles, bool populateOlsonFiles)
        {
            try
            {
                ProgressText = "Populating files";

                int progressIncrement = 0;

                if (populateOlsonFiles)
                {
                    _olsonDirectoryInfo.GetValidOlsonFiles();

                    progressIncrement += _olsonDirectoryInfo.ValidOlsonFiles.Count;
                }

                if (populateGeonamesFiles)
                {
                    _geonamesDirectoryInfo.GetValidGeonamesFiles();

                    progressIncrement += _geonamesDirectoryInfo.ValidGeonamesFiles.Count;
                }

                ProgressMaximum += progressIncrement;
            }
            catch (System.Exception e)
            {
                App.ExceptionLogger.LogMedium(e);
            }
        }

        /// <summary>
        /// Bulkcopies the provided DataTable to the Database table denoted by such DataTable's TableName property.
        /// </summary>
        /// <param name="dataTable">The DataTable to bulk load.</param>
        private void BulkCopy(DataTable dataTable, bool updateProgress = true)
        {
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_conStrBuilder.ToString()))
                {
                    bulkCopy.DestinationTableName = dataTable.TableName;
                    // Default is 30 seconds
                    bulkCopy.BulkCopyTimeout = NET_TIMEOUT;
                    bulkCopy.WriteToServer(dataTable);
                    if (updateProgress)
                    {
                        ProgressValue++;
                    }
                }
            }
            catch (System.Exception e)
            {
                App.ExceptionLogger.LogCritical(e);
            }
        }

        /// <summary>
        /// Event handler called whenever a parser is begining to parse a file.
        /// </summary>
        /// <param name="sender">The parser that raised the event.</param>
        /// <param name="e">The fileparseevent arguments for this event.</param>
        private void OnParserFileParsing(object sender, FileParseEventArgs e)
        {
            ProgressText = "Parsing file " + e.ParsedFile.Name;
        }

        /// <summary>
        /// Raises the ParsingStarted event.
        /// </summary>
        protected void RaiseParsingStarted()
        {
            if (null != ParsingStarted)
            {
                ParsingStarted(this, new System.EventArgs());
            }
        }

        /// <summary>
        /// Raises the ParsingFinished event.
        /// </summary>
        protected void RaiseParsingFinished(ParsingFinishedEventArgs args)
        {
            if (null != ParsingFinished)
            {
                ProgressText = "";
                ParsingFinished(this, args);
            }
        }

        /// <summary>
        /// Shows the output of the parsing operation.
        /// </summary>
        public void ShowOutput()
        {
            int olsonEntries = 0;
            if (null != _olsonParser)
            {
                olsonEntries = _olsonParser.ParsedEntries;
                _olsonParser.ResetCounters();
            }

            int geonamesEntries = 0;
            if (null != _geonamesParser)
            {
                geonamesEntries = _geonamesParser.ParsedEntries;
                _geonamesParser.ResetCounters();
            }

            StringBuilder sb = new StringBuilder("Program complete.\n");
            sb.Append("Uploaded:\n");
            sb.Append("\nOlson database entities:\n");
            sb.Append(olsonEntries);
            sb.Append("\n\nGeonames database entities:\n");
            sb.Append(geonamesEntries);
            sb.Append("\n\nTotal entities:\n ");
            sb.Append(olsonEntries + geonamesEntries);
            sb.Append("\n\nFrom: ");
            sb.Append(ProgressValue);
            sb.Append("/");
            sb.Append(ProgressMaximum);
            sb.Append(" total files.");

            ProgressValue = 0;
            ProgressMaximum = 0;

            System.Windows.MessageBox.Show(
                (Window)App.Current.MainWindow,
                sb.ToString(),
                "Update complete.",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _geonamesDirectoryInfo.Dispose();
                _olsonDirectoryInfo.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }
        #endregion
    }

    #region Exceptions
    /// <summary>
    /// The exception that is thrown when an invalid directory is specified.
    /// </summary>
    [System.Serializable]
    public class InvalidDirectoryException : System.Exception
    {
        private string _directoryName;

        /// <summary>
        /// Initializes a new instance of the InvalidDirectoryException class. 
        /// </summary>
        public InvalidDirectoryException() : base() { }

        /// <summary>
        /// Initializes a new instance of the InvalidDirectoryException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the exception. </param>
        public InvalidDirectoryException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the InvalidDirectoryException class with a specified
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A description of the error. The content of message is intended to be understood by humans.
        /// The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        /// <param name="inner">A reference to the inner exception that is the cause of this exception.</param>
        public InvalidDirectoryException(string message, System.Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the InvalidDirectoryException class with its message string set to message,
        /// specifying the invalid directory's name.
        /// </summary>
        /// <param name="message">A description of the error. The content of message is intended to be understood by humans.
        /// The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        /// <param name="fileName">The full name of the invalid directory.</param>
        public InvalidDirectoryException(string message, string directoryName)
            : base(message)
        {
            this._directoryName = directoryName;
        }

        /// <summary>
        /// Initializes a new instance of the InvalidGeonamesFileException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected InvalidDirectoryException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
    #endregion

    #region Event Args
    /// <summary>
    /// Represents the Event Arguments for a ParsingFinished event,
    /// containing the result of the parsing.
    /// </summary>
    public class ParsingFinishedEventArgs : System.EventArgs
    {
        /// <summary>
        /// The result of the parsing operation.
        /// </summary>
        public ParsingResult Result { get; set; }

        /// <summary>
        /// Creates a new instance of an ParsingFinishedEventArgs with the provided ParsingResult.
        /// </summary>
        /// <param name="databaseEntry">The last entry that was parsed.</param>
        public ParsingFinishedEventArgs(ParsingResult result)
            : base()
        {
            this.Result = result;
        }
    }
    #endregion

    #region Enums
    /// <summary>
    /// Represents the result of a parsing operation.
    /// </summary>
    public enum ParsingResult
    {
        Success,
        Failure
    }
    #endregion
}