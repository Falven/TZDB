using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Windows.Threading;

namespace NUIClockUpdater.Models
{
    public class OlsonDirectoryInfo : DirectoryInfo, System.IDisposable
    {
        #region Fields
        private const string ZONE_TAB_NAME = @"\zone.tab";
        private const string ISO_TAB_NAME = @"\iso3166.tab";
        private static List<string> _validFiles;
        private bool _isValid;
        #endregion

        #region Constructors
        static OlsonDirectoryInfo()
        {
            _validFiles = new List<string> { "africa", "antarctica", "asia", "australasia", "europe", "iso3166.tab", "northamerica", "southamerica", "zone.tab" };
        }

        /// <summary>
        /// Creates a new OlsonDirectoryInfo with the provided directory, to provide information on such directory.
        /// </summary>
        /// <param name="path"></param>
        public OlsonDirectoryInfo(string path)
            : base(path)
        {
            _isValid = false;
            if (Exists)
            {
                ValidOlsonFiles = new BlockingCollection<OlsonFileInfo>();

                bool validTabFiles = true;
                try
                {
                    IsoFile = new OlsonFileInfo(path + ISO_TAB_NAME);
                    ZoneFile = new OlsonFileInfo(path + ZONE_TAB_NAME);
                }
                catch (System.Exception)
                {
                    validTabFiles = false;
                }

                bool validFiles = false;
                var files = GetFiles();
                foreach (var file in files)
                {
                    if (_validFiles.Contains(file.Name))
                    {
                        validFiles = true;
                        break;
                    }
                }

                _isValid = validTabFiles || validFiles;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The iso3166.tab file for this Olson Directory.
        /// </summary>
        public OlsonFileInfo IsoFile { get; private set; }

        /// <summary>
        /// The zone.tab file for this Olson Directory.
        /// </summary>
        public OlsonFileInfo ZoneFile { get; private set; }

        /// <summary>
        /// Collection containing all of the valid olson files for this Directory.
        /// A call to GetValidOlsonFiles must precede accessign this property.
        /// </summary>
        public BlockingCollection<OlsonFileInfo> ValidOlsonFiles { get; private set; }

        /// <summary>
        /// Returns whether this directory is a valid Olson Directory.
        /// </summary>
        public bool IsValid { get { return _isValid; } }
        #endregion

        #region Members
        /// <summary>
        /// Enumerates and returns all of the valid olson files for this directory.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">fileName is null.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.ArgumentException">The file name is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="System.UnauthorizedAccessException">Access to fileName is denied.</exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="System.NotSupportedException">fileName contains a colon (:) in the middle of the string.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The filePath provided does not exist.</exception>
        /// <exception cref="NUIClockUpdater.Models.InvaliOlsonFileException">The file provided is not a valid Olson Database file.</exception>
        public BlockingCollection<OlsonFileInfo> GetValidOlsonFiles()
        {
            try
            {
                string[] paths = null;

                paths = Directory.GetFiles(ToString());
                foreach (string path in paths)
                {
                    try
                    {
                        OlsonFileInfo file = new OlsonFileInfo(path);
                        if (_validFiles.Contains(file.Name.ToLower()))
                        {
                            ValidOlsonFiles.Add(file);
                        }
                    }
                    catch (InvalidOlsonFileException)
                    {
                        continue;
                    }
                }
                return ValidOlsonFiles;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                ValidOlsonFiles.CompleteAdding();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ValidOlsonFiles.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }
        #endregion
    }
}