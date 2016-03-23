/* (c) Copyright Francisco Aguilera (Falven)
 * You are free to edit and distribute this
 * source so long as this statement remains
 * in place, here and in all other such files.
 */

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NUIClockUpdater.Models
{
    /// <summary>
    /// Represents the abstract base class for a Parser, containing common,
    /// but exclusive, code from Parsers.
    /// </summary>
    abstract class ParserBase
    {
        #region Fields
        /// <summary>
        /// Regex for splitting file lines into fields.
        /// </summary>
        protected Regex _regex;

        /// <summary>
        /// The number of entries that have been parsed.
        /// </summary>
        protected int _parsedEntries;
        #endregion

        #region Events
        /// <summary>
        /// Raised whenever an entry from a File has been parsed.
        /// </summary>
        public event EventHandler<EntryParsedEventArgs> EntryParsed;

        /// <summary>
        /// Raised whenever a File has begun parsing.
        /// </summary>
        public event EventHandler<FileParseEventArgs> FileParsing;

        /// <summary>
        /// Raised whenever a File has finished parsing.
        /// </summary>
        public event EventHandler<FileParseEventArgs> FileParsed;

        /// <summary>
        /// Raised when all of the files in the provided directory have been parsed.
        /// </summary>
        public event EventHandler<DirectoryParsedEventArgs> DirectoryParsed;
        #endregion

        #region Properties
        /// <summary>
        /// The directory containing the files this parser will parse.
        /// </summary>
        public NUIClockUpdater.Models.DirectoryInfo Directory { get; protected set; }

        /// <summary>
        /// The number of entries that have been parsed.
        /// </summary>
        public int ParsedEntries
        {
            get
            {
                return _parsedEntries;
            }
            protected set
            {
                _parsedEntries = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets all of the counters for this parser to 0.
        /// </summary>
        public void ResetCounters()
        {
            ParsedEntries = 0;
        }

        /// <summary>
        /// Raises the EntryParsed event with the provided DatabaseEntry.
        /// </summary>
        /// <param name="databaseEntry">The entry that was parsed.</param>
        protected void RaiseEntryParsed(object[] databaseEntry)
        {
            if (null != EntryParsed)
            {
                EntryParsed(this, new EntryParsedEventArgs(databaseEntry));
            }
        }

        /// <summary>
        /// Raises the FileParsing event with the provided File.
        /// </summary>
        /// <param name="file">The file being parsed.</param>
        protected void RaiseFileParsing(FileInfo file)
        {
            if (null != FileParsing)
            {
                FileParsing(this, new FileParseEventArgs(file));
            }
        }

        /// <summary>
        /// Raises the FileParsed event with the provided File.
        /// </summary>
        /// <param name="file">The file that was parsed.</param>
        protected void RaiseFileParsed(FileInfo file)
        {
            if (null != FileParsed)
            {
                FileParsed(this, new FileParseEventArgs(file));
            }
        }

        /// <summary>
        /// Raises the DirectoryParsed event with the provided directory.
        /// </summary>
        /// <param name="directory">The directory that was parsed.</param>
        protected void RaiseDirectoryParsed(DirectoryInfo directory)
        {
            if (null != DirectoryParsed)
            {
                DirectoryParsed(this, new DirectoryParsedEventArgs(directory));
            }
        }
        #endregion
    }

    #region Inner Classes
    /// <summary>
    /// Represents the Event Arguments for an EntryParsed event,
    /// containing the entry that was parsed, the number of 
    /// entries parsed, and the total number of entries to parse.
    /// </summary>
    public class EntryParsedEventArgs : EventArgs
    {
        /// <summary>
        /// The entry that was last parsed.
        /// </summary>
        public object[] DatabaseEntry { get; set; }

        /// <summary>
        /// Creates a new instance of an EntryparsedEventArgs with the provided databaseEntry.
        /// </summary>
        /// <param name="databaseEntry">The last entry that was parsed.</param>
        /// <param name="currentBytes">The number of bytes parsed out of TotalBytes.</param>
        /// <param name="totalBytes">The total number of bytes this Parser will parse.</param>
        public EntryParsedEventArgs(object[] databaseEntry)
            : base()
        {
            this.DatabaseEntry = databaseEntry;
        }
    }

    /// <summary>
    /// Represents the Event Arguments for a FileParsing event,
    /// containing the File being or was parsed.
    /// </summary>
    public class FileParseEventArgs : EventArgs
    {
        /// <summary>
        /// The File that was last parsed.
        /// </summary>
        public FileInfo ParsedFile { get; set; }

        /// <summary>
        /// Creates a new instance of a FileParsedEventArgs with the provided parsed file.
        /// </summary>
        /// <param name="parsedFile">The last File that was parsed.</param>
        public FileParseEventArgs(FileInfo parsedFile)
            : base()
        {
            this.ParsedFile = parsedFile;
        }
    }

    /// <summary>
    /// Represents the Event Arguments for a DirectoryParsed event,
    /// containing the directory that was parsed, the number of 
    /// entries parsed, and the total number of entries to parse.
    /// </summary>
    public class DirectoryParsedEventArgs : EventArgs
    {
        /// <summary>
        /// The Directory that was last parsed.
        /// </summary>
        public DirectoryInfo ParsedDirectory { get; set; }

        /// <summary>
        /// Creates a new instance of a DirectoryParsedEventArgs with the provided parsed directory.
        /// </summary>
        /// <param name="parsedFile">The directory that was parsed.</param>
        /// <param name="currentBytes">The number of bytes parsed out of TotalBytes.</param>
        /// <param name="totalBytes">The total number of bytes this Parser will parse.</param>
        public DirectoryParsedEventArgs(DirectoryInfo directoryParsed)
            : base()
        {
            this.ParsedDirectory = directoryParsed;
        }
    }
    #endregion
}