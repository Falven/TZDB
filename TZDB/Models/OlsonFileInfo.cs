using System.Collections.Generic;
namespace NUIClockUpdater.Models
{
    /// <summary>
    /// Provides information about a file belonging to the Olson Database,
    /// also called the TZ database, the zoneinfo database or IANA Time Zone Database.
    /// </summary>
    public sealed class OlsonFileInfo : FileInfo
    {
        #region Fields
        /// <summary>
        /// The extension for an Olson tab file.
        /// </summary>
        private const string TAB_EXTENSION = ".tab";

        /// <summary>
        /// The Pretext can be found at the begining of any valid olson file.
        /// </summary>
        private const string PRETEXT = "# <pre>";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the NUIClockUpdater.Models.OlsonFileInfo class,
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
        /// <exception cref="NUIClockUpdater.Models.InvaliOlsonFileException">The file provided is not a valid Olson Database file.</exception>
        public OlsonFileInfo(string filePath)
            : base(filePath)
        {
            base._fileInfo = new System.IO.FileInfo(filePath);
            if (!base._fileInfo.Exists)
            {
                throw new System.IO.FileNotFoundException("The filePath provided does not exist.", filePath);
            }
            if (!IsValidOlsonFileInfo())
            {
                throw new NUIClockUpdater.Models.InvalidOlsonFileException("The file provided is not a valid Olson Database file.", filePath);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Returns whether this OlsonFile is a valid tab file.
        /// </summary>
        public bool IsTabFile { get; private set; }
        #endregion

        #region Members
        /// <summary>
        /// Validates whether this OlsonFile is a valid Olson Database file.
        /// </summary>
        /// <returns>True if this is a valid Olson Database file, False otherwise.</returns>
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
        private bool IsValidOlsonFileInfo()
        {
            string extension = base._fileInfo.Extension;
            bool hasTabExtension = extension.ToLower().Equals(TAB_EXTENSION);
            bool hasValidExtension = (extension == string.Empty || hasTabExtension);
            bool hasValidPreText;
            // Using underlying FileStream to allow concurrent Read/Write access.
            try
            {
                using (var input = new System.IO.StreamReader(
                    System.IO.File.Open(base._fileInfo.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)))
                {
                    hasValidPreText = input.ReadLine().StartsWith(PRETEXT);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            bool isValidFile = hasValidExtension && hasValidPreText;
            IsTabFile = hasTabExtension && isValidFile;
            return isValidFile;
        }
        #endregion
    }

    #region Exceptions
    /// <summary>
    /// The exception that is thrown when an invalid Olson file is specified.
    /// </summary>
    [System.Serializable]
    public class InvalidOlsonFileException : System.Exception
    {
        private string _fileName;

        /// <summary>
        /// Initializes a new instance of the InvalidOlsonFileException class. 
        /// </summary>
        public InvalidOlsonFileException() : base() { }

        /// <summary>
        /// Initializes a new instance of the InvalidOlsonFileException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidOlsonFileException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the InvalidOlsonFileException class with a specified
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A description of the error. The content of message is intended to be understood by humans.
        /// The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        /// <param name="inner">A reference to the inner exception that is the cause of this exception.</param>
        public InvalidOlsonFileException(string message, System.Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the InvalidOlsonFileException class with its message string set to message,
        /// specifying the invalid file's name.
        /// </summary>
        /// <param name="message">A description of the error. The content of message is intended to be understood by humans.
        /// The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        /// <param name="fileName">The full name of the invalid file.</param>
        public InvalidOlsonFileException(string message, string fileName)
            : base(message)
        {
            this._fileName = fileName;
        }

        /// <summary>
        /// Initializes a new instance of the InvalidOlsonFileException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected InvalidOlsonFileException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
    #endregion
}
