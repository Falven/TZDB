using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace NUIClockUpdater.Models
{
    /// <summary>
    /// Exposes instance methods for creating, moving, and enumerating through directories and subdirectories.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class DirectoryInfo
    {
        #region Fields
        private System.IO.DirectoryInfo _directoryInfo;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the System.IO.DirectoryInfo class on the specified path.
        /// </summary>
        /// <param name="path">
        /// A string specifying the path on which to create the DirectoryInfo.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// path is null.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// path contains invalid characters such as ", &lt;, &gt;, or |.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// The specified path, file name, or both are too long.
        /// </exception>
        public DirectoryInfo(string path)
        {
            _directoryInfo = new System.IO.DirectoryInfo(path);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        /// <returns>
        /// true if the directory exists; otherwise, false.
        /// </returns>
        public bool Exists { get { return _directoryInfo.Exists; } }

        /// <summary>
        /// Gets the name of this System.IO.DirectoryInfo instance.
        /// </summary>
        /// <returns>
        /// The directory name.
        /// </returns>
        public string Name { get { return _directoryInfo.Name; } }

        /// <summary>
        /// Gets the parent directory of a specified subdirectory.
        /// </summary>
        /// <returns>
        /// The parent directory, or null if the path is null or if the file path denotes
        /// a root (such as "\", "C:", or * "\\server\share").
        /// </returns>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public System.IO.DirectoryInfo Parent { get { return _directoryInfo.Parent; } }

        /// <summary>
        /// Gets the root portion of a path.
        /// </summary>
        /// <returns>
        /// A System.IO.DirectoryInfo object representing the root of a path.
        /// </returns>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public System.IO.DirectoryInfo Root { get { return _directoryInfo.Root; } }
        #endregion

        #region Members
        /// <summary>
        /// Creates a directory.
        /// </summary>
        /// <exception cref="System.IO.IOException">
        /// The directory cannot be created.
        /// </exception>
        public void Create()
        {
            _directoryInfo.Create();
        }

        /// <summary>
        /// Creates a directory using a System.Security.AccessControl.DirectorySecurity
        /// object.
        /// </summary>
        /// <param name="directorySecurity">
        /// The access control to apply to the directory.
        /// </param>
        /// <exception cref="System.IO.IOException">
        /// The directory specified by path is read-only or is not empty.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// path is a zero-length string, contains only white space, or contains one
        /// or more invalid characters as defined by System.IO.Path.InvalidPathChars.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// path is null.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum
        /// length. For example, on Windows-based platforms, paths must be less than
        /// 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The specified path is invalid, such as being on an unmapped drive.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// Creating a directory with only the colon (:) character was attempted.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// The directory specified by path is read-only or is not empty.
        /// </exception>
        public void Create(DirectorySecurity directorySecurity)
        {
            _directoryInfo.Create(directorySecurity);
        }

        /// <summary>
        /// Creates a subdirectory or subdirectories on the specified path. The specified
        /// path can be relative to this instance of the System.IO.DirectoryInfo class.
        /// </summary>
        /// <param name="path">
        /// The specified path. This cannot be a different disk volume or Universal Naming
        /// Convention (UNC) name.
        /// </param>
        /// <returns>
        /// The last directory specified in path.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// path does not specify a valid file path or contains invalid DirectoryInfo
        /// characters.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// path is null.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The specified path is invalid, such as being on an unmapped drive.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// The subdirectory cannot be created.-or- A file or directory already has the
        /// name specified by path.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum
        /// length. For example, on Windows-based platforms, paths must be less than
        /// 248 characters, and file names must be less than 260 characters. The specified
        /// path, file name, or both are too long.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have code access permission to create the directory.-or-The
        /// caller does not have code access permission to read the directory described
        /// by the returned System.IO.DirectoryInfo object. This can occur when the path
        /// parameter describes an existing directory.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// path contains a colon character (:) that is not part of a drive label ("C:\").
        /// </exception>
        public System.IO.DirectoryInfo CreateSubdirectory(string path)
        {
            return _directoryInfo.CreateSubdirectory(path);
        }

        /// <summary>
        /// Creates a subdirectory or subdirectories on the specified path with the specified
        /// security. The specified path can be relative to this instance of the System.IO.DirectoryInfo
        /// class.
        /// </summary>
        /// <param name="path">
        /// The specified path. This cannot be a different disk volume or Universal Naming
        /// Convention (UNC) name.
        /// </param>
        /// <param name="directorySecurity">
        /// The security to apply.
        /// </param>
        /// <returns>
        /// The last directory specified in path.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// path does not specify a valid file path or contains invalid DirectoryInfo
        /// characters.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// path is null.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The specified path is invalid, such as being on an unmapped drive.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// The subdirectory cannot be created.-or- A file or directory already has the
        /// name specified by path.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum
        /// length. For example, on Windows-based platforms, paths must be less than
        /// 248 characters, and file names must be less than 260 characters. The specified
        /// path, file name, or both are too long.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have code access permission to create the directory.-or-The
        /// caller does not have code access permission to read the directory described
        /// by the returned System.IO.DirectoryInfo object. This can occur when the path
        /// parameter describes an existing directory.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// path contains a colon character (:) that is not part of a drive label ("C:\").
        /// </exception>
        [SecuritySafeCritical]
        public System.IO.DirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity)
        {
            return _directoryInfo.CreateSubdirectory(path, directorySecurity);
        }

        /// <summary>
        /// Deletes this System.IO.DirectoryInfo if it is empty.
        /// </summary>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The directory contains a read-only file.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The directory described by this System.IO.DirectoryInfo object does not exist
        /// or could not be found.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// The directory is not empty. -or-The directory is the application's current
        /// working directory.-or-There is an open handle on the directory, and the operating
        /// system is Windows XP or earlier. This open handle can result from enumerating
        /// directories. For more information, see How to: Enumerate Directories and
        /// Files.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        [SecuritySafeCritical]
        public void Delete()
        {
            _directoryInfo.Delete();
        }

        /// <summary>
        /// Deletes this instance of a System.IO.DirectoryInfo, specifying whether to
        /// delete subdirectories and files.
        /// </summary>
        /// <param name="recursive">
        /// true to delete this directory, its subdirectories, and all files; otherwise,
        /// false.
        /// </param>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The directory contains a read-only file.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The directory described by this System.IO.DirectoryInfo object does not exist
        /// or could not be found.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// The directory is read-only.-or- The directory contains one or more files
        /// or subdirectories and recursive is false.-or-The directory is the application's
        /// current working directory. -or-There is an open handle on the directory or
        /// on one of its files, and the operating system is Windows XP or earlier. This
        /// open handle can result from enumerating directories and files. For more information,
        /// see How to: Enumerate Directories and Files.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        [SecuritySafeCritical]
        public void Delete(bool recursive)
        {
            _directoryInfo.Delete(recursive);
        }

        /// <summary>
        /// Returns an enumerable collection of directory information in the current
        /// directory.
        /// </summary>
        /// <returns>
        /// An enumerable collection of directories in the current directory.
        /// </returns>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path encapsulated in the System.IO.DirectoryInfo object is invalid (for
        /// example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public IEnumerable<System.IO.DirectoryInfo> EnumerateDirectories()
        {
            return _directoryInfo.EnumerateDirectories();
        }

        /// <summary>
        /// Returns an enumerable collection of directory information that matches a
        /// specified search pattern.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string. The default pattern is "*", which returns all directories.
        /// </param>
        /// <returns>
        /// An enumerable collection of directories that matches searchPattern.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// searchPattern is null.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path encapsulated in the System.IO.DirectoryInfo object is invalid (for
        /// example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public IEnumerable<System.IO.DirectoryInfo> EnumerateDirectories(string searchPattern)
        {
            return _directoryInfo.EnumerateDirectories(searchPattern);
        }

        /// <summary>
        /// Returns an enumerable collection of directory information that matches a
        /// specified search pattern and search subdirectory option.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string. The default pattern is "*", which returns all directories.
        /// </param>
        /// </param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or all subdirectories. The default
        /// value is System.IO.SearchOption.TopDirectoryOnly.
        /// </param>
        /// <returns>
        /// An enumerable collection of directories that matches searchPattern and searchOption.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// searchPattern is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// searchOption is not a valid System.IO.SearchOption value.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path encapsulated in the System.IO.DirectoryInfo object is invalid (for
        /// example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public IEnumerable<System.IO.DirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            return _directoryInfo.EnumerateDirectories(searchPattern, searchOption);
        }

        /// <summary>
        /// Returns an enumerable collection of file information in the current directory.
        /// </summary>
        /// <returns>
        /// An enumerable collection of the files in the current directory.
        /// </returns>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path encapsulated in the System.IO.FileInfo object is invalid (for example,
        /// it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public IEnumerable<System.IO.FileInfo> EnumerateFiles()
        {
            return _directoryInfo.EnumerateFiles();
        }

        /// <summary>
        /// Returns an enumerable collection of file information that matches a search
        /// pattern.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string. The default pattern is "*", which returns all files.
        /// </param>
        /// <returns>
        /// An enumerable collection of files that matches searchPattern.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// searchPattern is null.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path encapsulated in the System.IO.FileInfo object is invalid, (for example,
        /// it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public IEnumerable<System.IO.FileInfo> EnumerateFiles(string searchPattern)
        {
            return _directoryInfo.EnumerateFiles(searchPattern);
        }

        /// <summary>
        /// Returns an enumerable collection of file information that matches a specified
        /// search pattern and search subdirectory option.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string. The default pattern is "*", which returns all files.
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or all subdirectories. The default
        /// value is System.IO.SearchOption.TopDirectoryOnly.
        /// </param>
        /// <returns>
        /// An enumerable collection of files that matches searchPattern and searchOption.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// searchPattern is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// searchOption is not a valid System.IO.SearchOption value.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path encapsulated in the System.IO.FileInfo object is invalid (for example,
        /// it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public IEnumerable<System.IO.FileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return _directoryInfo.EnumerateFiles(searchPattern, searchOption);
        }

        /// <summary>
        /// Returns an enumerable collection of file system information in the current
        /// directory.
        /// </summary>
        /// <returns>
        /// An enumerable collection of file system information in the current directory.
        /// </returns>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path encapsulated in the System.IO.FileSystemInfo object is invalid (for
        /// example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos()
        {
            return _directoryInfo.EnumerateFileSystemInfos();
        }

        /// <summary>
        /// Returns an enumerable collection of file system information that matches
        /// a specified search pattern.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string. The default pattern is "*", which returns all files and
        /// directories.
        /// </param>
        /// <returns>
        /// An enumerable collection of file system information objects that matches
        /// searchPattern.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// searchPattern is null.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path encapsulated in the System.IO.FileSystemInfo object is invalid (for
        /// example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
        {
            return _directoryInfo.EnumerateFileSystemInfos(searchPattern);
        }

        /// <summary>
        /// Returns an enumerable collection of file system information that matches
        /// a specified search pattern and search subdirectory option.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string. The default pattern is "*", which returns all files or
        /// directories.
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or all subdirectories. The default
        /// value is System.IO.SearchOption.TopDirectoryOnly.
        /// </param>
        /// <returns>
        /// An enumerable collection of file system information objects that matches
        /// searchPattern and searchOption.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// searchPattern is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// searchOption is not a valid System.IO.SearchOption value.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path encapsulated in the System.IO.FileSystemInfo object is invalid (for
        /// example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return _directoryInfo.EnumerateFileSystemInfos(searchPattern, searchOption);
        }

        /// <summary>
        /// Gets a System.Security.AccessControl.DirectorySecurity object that encapsulates
        /// the access control list (ACL) entries for the directory described by the
        /// current System.IO.DirectoryInfo object.
        /// </summary>
        /// <returns>
        /// A System.Security.AccessControl.DirectorySecurity object that encapsulates
        /// the access control rules for the directory.
        /// </returns>
        /// <exception cref="System.SystemException">
        /// The directory could not be found or modified.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The current process does not have access to open the directory.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// An I/O error occurred while opening the directory.
        /// </exception>
        /// <exception cref="System.PlatformNotSupportedException">
        /// The current operating system is not Microsoft Windows 2000 or later.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The directory is read-only.-or- This operation is not supported on the current
        /// platform.-or- The caller does not have the required permission.
        /// </exception>
        public DirectorySecurity GetAccessControl()
        {
            return _directoryInfo.GetAccessControl();
        }

        /// <summary>
        /// Gets a System.Security.AccessControl.DirectorySecurity object that encapsulates
        /// the specified type of access control list (ACL) entries for the directory
        /// described by the current System.IO.DirectoryInfo object.
        /// </summary>
        /// <param name="includeSections">
        /// One of the System.Security.AccessControl.AccessControlSections values that
        /// specifies the type of access control list (ACL) information to receive.
        /// </param>
        /// <returns>
        /// A System.Security.AccessControl.DirectorySecurity object that encapsulates
        /// the access control rules for the file described by the path parameter.ExceptionsException
        /// typeConditionSystem.SystemExceptionThe directory could not be found or modified.System.UnauthorizedAccessExceptionThe
        /// current process does not have access to open the directory.System.IO.IOExceptionAn
        /// I/O error occurred while opening the directory.System.PlatformNotSupportedExceptionThe
        /// current operating system is not Microsoft Windows 2000 or later.System.UnauthorizedAccessExceptionThe
        /// directory is read-only.-or- This operation is not supported on the current
        /// platform.-or- The caller does not have the required permission.
        /// </returns>
        public DirectorySecurity GetAccessControl(AccessControlSections includeSections)
        {
            return _directoryInfo.GetAccessControl(includeSections);
        }

        /// <summary>
        /// Returns the subdirectories of the current directory.
        /// </summary>
        /// <returns>
        /// An array of System.IO.DirectoryInfo objects.
        /// </returns>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path encapsulated in the System.IO.DirectoryInfo object is invalid, such
        /// as being on an unmapped drive.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        public System.IO.DirectoryInfo[] GetDirectories()
        {
            return _directoryInfo.GetDirectories();
        }

        /// <summary>
        /// Returns an array of directories in the current System.IO.DirectoryInfo matching
        /// the given search criteria.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string. For example, "System*" can be used to search for all directories
        /// that begin with the word "System".
        /// </param>
        /// <returns>
        /// An array of type DirectoryInfo matching searchPattern.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// searchPattern contains one or more invalid characters defined by the System.IO.Path.GetInvalidPathChars()
        /// method.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// searchPattern is null.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path encapsulated in the DirectoryInfo object is invalid (for example,
        /// it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        public System.IO.DirectoryInfo[] GetDirectories(string searchPattern)
        {
            return _directoryInfo.GetDirectories(searchPattern);
        }

        /// <summary>
        /// Returns an array of directories in the current System.IO.DirectoryInfo matching
        /// the given search criteria and using a value to determine whether to search
        /// subdirectories.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string. For example, "System*" can be used to search for all directories
        /// that begin with the word "System".
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or all subdirectories.
        /// </param>
        /// <returns>
        /// An array of type DirectoryInfo matching searchPattern.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// searchPattern contains one or more invalid characters defined by the System.IO.Path.GetInvalidPathChars()
        /// method.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// searchPattern is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// searchOption is not a valid System.IO.SearchOption value.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path encapsulated in the DirectoryInfo object is invalid (for example,
        /// it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        public System.IO.DirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return _directoryInfo.GetDirectories(searchPattern, searchOption);
        }

        /// <summary>
        /// Returns a file list from the current directory.
        /// </summary>
        /// <returns>
        /// An array of type System.IO.FileInfo.
        /// </returns>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path is invalid, such as being on an unmapped drive.
        /// </exception>
        public System.IO.FileInfo[] GetFiles()
        {
            return _directoryInfo.GetFiles();
        }

        /// <summary>
        /// Returns a file list from the current directory matching the given search
        /// pattern.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string, such as "*.txt".
        /// </param>
        /// <returns>
        /// An array of type System.IO.FileInfo.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// searchPattern contains one or more invalid characters defined by the System.IO.Path.GetInvalidPathChars()
        /// method.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// searchPattern is null.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public System.IO.FileInfo[] GetFiles(string searchPattern)
        {
            return _directoryInfo.GetFiles(searchPattern);
        }

        /// <summary>
        /// Returns a file list from the current directory matching the given search
        /// pattern and using a value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string. For example, "System*" can be used to search for all directories
        /// that begin with the word "System".
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or all subdirectories.
        /// </param>
        /// <returns>
        /// An array of type System.IO.FileInfo.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// searchPattern contains one or more invalid characters defined by the System.IO.Path.GetInvalidPathChars()
        /// method.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// searchPattern is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// searchOption is not a valid System.IO.SearchOption value.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public System.IO.FileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return _directoryInfo.GetFiles(searchPattern, searchOption);
        }

        /// <summary>
        /// Returns an array of strongly typed System.IO.FileSystemInfo entries representing
        /// all the files and subdirectories in a directory.
        /// </summary>
        /// <returns>
        /// An array of strongly typed System.IO.FileSystemInfo entries.
        /// </returns>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        public FileSystemInfo[] GetFileSystemInfos()
        {
            return _directoryInfo.GetFileSystemInfos();
        }

        /// <summary>
        /// Retrieves an array of strongly typed System.IO.FileSystemInfo objects representing
        /// the files and subdirectories that match the specified search criteria.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string. For example, "System*" can be used to search for all directories
        /// that begin with the word "System".
        /// </param>
        /// <returns>
        /// An array of strongly typed FileSystemInfo objects matching the search criteria.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// searchPattern contains one or more invalid characters defined by the System.IO.Path.GetInvalidPathChars()
        /// method.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// searchPattern is null.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The specified path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public FileSystemInfo[] GetFileSystemInfos(string searchPattern)
        {
            return _directoryInfo.GetFileSystemInfos(searchPattern);
        }

        /// <summary>
        /// Retrieves an array of System.IO.FileSystemInfo objects that represent the
        /// files and subdirectories matching the specified search criteria.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string. The default pattern is "*", which returns all files and
        /// directories.
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or all subdirectories. The default
        /// value is System.IO.SearchOption.TopDirectoryOnly.
        /// </param>
        /// <returns>
        /// An array of file system entries that match the search criteria.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// searchPattern contains one or more invalid characters defined by the System.IO.Path.GetInvalidPathChars()
        /// method.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// searchPattern is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// searchOption is not a valid System.IO.SearchOption value.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The specified path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public FileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return _directoryInfo.GetFileSystemInfos(searchPattern, searchOption);
        }

        /// <summary>
        /// Moves a System.IO.DirectoryInfo instance and its contents to a new path.
        /// </summary>
        /// <param name="destDirName">
        /// The name and path to which to move this directory. The destination cannot
        /// be another disk volume or a directory with the identical name. It can be
        /// an existing directory to which you want to add this directory as a subdirectory.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// destDirName is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// destDirName is an empty string (''").
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// An attempt was made to move a directory to a different volume. -or-destDirName
        /// already exists.-or-You are not authorized to access this path.-or- The directory
        /// being moved and the destination directory have the same name.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref=" <exception cref="System.IO.DirectoryNotFoundException">">
        /// The destination directory cannot be found.
        /// </exception>
        [SecuritySafeCritical]
        public void MoveTo(string destDirName)
        {
            _directoryInfo.MoveTo(destDirName);
        }

        /// <summary>
        /// Applies access control list (ACL) entries described by a System.Security.AccessControl.DirectorySecurity
        /// object to the directory described by the current System.IO.DirectoryInfo
        /// object.
        /// </summary>
        /// <param name="directorySecurity">
        /// An object that describes an ACL entry to apply to the directory described
        /// by the path parameter.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// The directorySecurity parameter is null.
        /// </exception>
        /// <exception cref="System.SystemException">
        /// The file could not be found or modified.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The current process does not have access to open the file.
        /// </exception>
        /// <exception cref="System.PlatformNotSupportedException">
        /// The current operating system is not Microsoft Windows 2000 or later.
        /// </exception>
        public void SetAccessControl(DirectorySecurity directorySecurity)
        {
            _directoryInfo.SetAccessControl(directorySecurity);
        }

        /// <summary>
        /// Returns the original path that was passed by the user.
        /// </summary>
        /// <returns>
        /// Returns the original path that was passed by the user.
        /// </returns>
        public override string ToString()
        {
            return _directoryInfo.ToString();
        }
        #endregion
    }
}
