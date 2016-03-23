namespace NUIClockUpdater.Models
{
    /// <summary>
    /// Provides information about a file belonging to the Geonames Database.
    /// </summary>
    public sealed class GeonamesFileInfo : FileInfo
    {
        /// <summary>
        /// The extension for a Geonames file.
        /// </summary>
        private const string VALID_EXTENSION = ".txt";

        /// <summary>
        /// Initializes a new instance of the NUIClockUpdater.Models.GeonamesFileInfo class,
        /// which acts as a wrapper for a FileInfo object with added functionality.
        /// </summary>
        /// <param name="fileName"></param>
        /// <exception cref="System.ArgumentNullException">fileName is null.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.ArgumentException">The file name is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="System.UnauthorizedAccessException">Access to fileName is denied.</exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="System.NotSupportedException">fileName contains a colon (:) in the middle of the string.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The filePath provided does not exist.</exception>
        /// <exception cref="NUIClockUpdater.Models.InvalidGeonamesFileException">The file provided is not a valid Geonames Database file.</exception>
        public GeonamesFileInfo(string filePath)
            : base(filePath)
        {
            base._fileInfo = new System.IO.FileInfo(filePath);
            if (!base._fileInfo.Exists)
            {
                throw new System.IO.FileNotFoundException("The filePath provided does not exist.", filePath);
            }
            if (!IsValidGeonamesFile())
            {
                throw new NUIClockUpdater.Models.InvalidGeonamesFileException("The file provided is not a valid Geonames Database file.", filePath);
            }
        }

        /// <summary>
        /// Validates whether this GeonamesFileInfo is a valid Geonames Database file.
        /// </summary>
        /// <returns>True if this is a valid Geonames Database file, False otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">path is null.</exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="System.IO.IOException">
        /// An I/O error occurred while opening the file.</exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// path specified a file that is read-only and access is not Read.
        /// -or-
        /// path specified a directory.-or- The caller does not have the required permission.
        /// -or-
        /// mode is System.IO.FileMode.Create and the specified file is a hidden file.</exception>
        private bool IsValidGeonamesFile()
        {
            if (!base._fileInfo.Extension.ToLower().Equals(VALID_EXTENSION))
            {
                return false;
            }
            bool hasValidPreText;
            // Using underlying FileStream to allow concurrent Read/Write access.
            try
            {
                using (var input = new System.IO.StreamReader(
                    System.IO.File.Open(base._fileInfo.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)))
                {
                    string[] tokens = input.ReadLine().Split('\t');
                    long id;
                    hasValidPreText = long.TryParse(tokens[0], out id) && tokens.Length >= 11;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return hasValidPreText;
        }
    }

    #region Exceptions
    /// <summary>
    /// The exception that is thrown when an invalid Geonames file is specified.
    /// </summary>
    [System.Serializable]
    public class InvalidGeonamesFileException : System.Exception
    {
        private string _fileName;

        /// <summary>
        /// Initializes a new instance of the InvalidGeonamesFileException class. 
        /// </summary>
        public InvalidGeonamesFileException() : base() { }

        /// <summary>
        /// Initializes a new instance of the InvalidGeonamesFileException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public InvalidGeonamesFileException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the InvalidGeonamesFileException class with a specified
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A description of the error. The content of message is intended to be understood by humans.
        /// The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        /// <param name="inner">A reference to the inner exception that is the cause of this exception.</param>
        public InvalidGeonamesFileException(string message, System.Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the InvalidGeonamesFileException class with its message string set to message,
        /// specifying the invalid file's name.
        /// </summary>
        /// <param name="message">A description of the error. The content of message is intended to be understood by humans.
        /// The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        /// <param name="fileName">The full name of the invalid file.</param>
        public InvalidGeonamesFileException(string message, string fileName)
            : base(message)
        {
            this._fileName = fileName;
        }

        /// <summary>
        /// Initializes a new instance of the InvalidGeonamesFileException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected InvalidGeonamesFileException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
    #endregion
}
