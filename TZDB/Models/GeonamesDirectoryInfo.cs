using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace NUIClockUpdater.Models
{
    public class GeonamesDirectoryInfo : DirectoryInfo, System.IDisposable
    {
        #region Fields
        private bool _isValid;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new GeonamesDirectoryInfo with the provided directory, to provide information on such directory.
        /// </summary>
        /// <param name="path">The path to the directory.</param>
        public GeonamesDirectoryInfo(string path)
            : base(path)
        {
            if (Exists)
            {
                ValidGeonamesFiles = new BlockingCollection<GeonamesFileInfo>();

                _isValid = true;
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    try
                    {
                        new GeonamesFileInfo(file);
                    }
                    catch (System.Exception)
                    {
                        _isValid = false;
                    }
                    return;
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Collection containing all of the valid Geonames files for this directory.
        /// A call to GetValidGeonamesFiles must precede accessign this property.
        /// </summary>
        public BlockingCollection<GeonamesFileInfo> ValidGeonamesFiles { get; private set; }

        /// <summary>
        /// Returns whether this directory is a valid Geonames directory.
        /// </summary>
        public bool IsValid { get { return _isValid; } }
        #endregion

        #region Members
        /// <summary>
        /// Enumerates and returns all of the valid Geonames files for this directory.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">fileName is null.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.ArgumentException">The file name is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="System.UnauthorizedAccessException">Access to fileName is denied.</exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="System.NotSupportedException">fileName contains a colon (:) in the middle of the string.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The filePath provided does not exist.</exception>
        /// <exception cref="NUIClockUpdater.Models.InvaliOlsonFileException">The file provided is not a valid Geonames database file.</exception>
        public BlockingCollection<GeonamesFileInfo> GetValidGeonamesFiles()
        {
            try
            {
                string[] paths = null;

                paths = Directory.GetFiles(ToString());
                foreach (string path in paths)
                {
                    try
                    {
                        ValidGeonamesFiles.Add(new GeonamesFileInfo(path));
                    }
                    catch (InvalidGeonamesFileException)
                    {
                        continue;
                    }
                }
                return ValidGeonamesFiles;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                ValidGeonamesFiles.CompleteAdding();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ValidGeonamesFiles.Dispose();
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