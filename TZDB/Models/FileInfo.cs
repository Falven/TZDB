using System;

namespace NUIClockUpdater.Models
{
    /// <summary>
    /// Provides all of the properties and methods of the wrapped FileInfo class for other classes to inherit.
    /// </summary>
    [System.Serializable]
    public class FileInfo
    {
        #region Fields
        /// <summary>
        /// The FileInfo object this base class serves as a wrapper for.
        /// </summary>
        protected System.IO.FileInfo _fileInfo;
        #endregion

        #region Constructors
        public FileInfo(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            _fileInfo = new System.IO.FileInfo(fileName);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the attributes for the current file or directory(Inherited from FileSystemInfo.)
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">The specified file does not exist.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid; for example, it is on an unmapped drive.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.ArgumentException">
        /// The caller attempts to set an invalid file attribute.
        /// -or-
        /// The user attempts to set an attribute value but does not have write permission.
        /// </exception>
        /// <exception cref="System.IO.IOException">Refresh cannot initialize the data.</exception>
        public System.IO.FileAttributes Attributes
        {
            get { return _fileInfo.Attributes; }
            set { _fileInfo.Attributes = value; }
        }

        /// <summary>
        /// Gets or sets the creation time of the current file or directory.
        /// </summary>
        /// <exception cref="System.IO.IOException">Refresh cannot initialize the data.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid; for example, it is on an unmapped drive.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The caller attempts to set an invalid access time.</exception>
        public System.DateTime CreationTime
        {
            get { return _fileInfo.CreationTime; }
            set { _fileInfo.CreationTime = value; }
        }

        /// <summary>
        /// Gets or sets the creation time, in coordinated universal time (UTC),
        /// of the current file or directory.(Inherited from FileSystemInfo.)
        /// </summary>
        /// <exception cref="System.IO.IOException">Refresh cannot initialize the data.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid; for example, it is on an unmapped drive.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The caller attempts to set an invalid access time.</exception>
        public System.DateTime CreationTimeUtc
        {
            get { return _fileInfo.CreationTimeUtc; }
            set { _fileInfo.CreationTimeUtc = value; }
        }

        /// <summary>
        /// Gets an instance of the parent directory.
        /// </summary>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        public System.IO.DirectoryInfo Directory { get { return _fileInfo.Directory; } }

        /// <summary>
        /// Gets a string representing the directory's full path.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">null was passed in for the directory name.</exception>
        /// <exception cref="System.IO.PathTooLongException">The fully qualified path is 260 or more characters.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        public string DirectoryName { get { return _fileInfo.DirectoryName; } }

        /// <summary>
        /// Gets a value indicating whether a file exists.(Overrides FileSystemInfo.Exists.)
        /// </summary>
        public bool Exists
        {
            get { return _fileInfo.Exists; }
        }

        /// <summary>
        /// Gets the string representing the extension part of the file.(Inherited from FileSystemInfo.)
        /// </summary>
        public string Extension { get { return _fileInfo.Extension; } }

        /// <summary>
        /// Gets the full path of the directory or file.(Inherited from FileSystemInfo.)
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">The fully qualified path and file name is 260 or more characters.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        public string FullName { get { return _fileInfo.FullName; } }

        /// <summary>
        /// Gets or sets a value that determines if the current file is read only.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">The file described by the current FileInfo object could not be found.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// This operation is not supported on the current platform.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.ArgumentException">The user does not have write permission, but attempted to set this property to false.</exception>
        public bool IsReadOnly
        {
            get { return _fileInfo.IsReadOnly; }
            set { _fileInfo.IsReadOnly = value; }
        }

        /// <summary>
        /// Gets or sets the time the current file or directory was last accessed.(Inherited from FileSystemInfo.)
        /// </summary>
        /// <exception cref="System.IO.IOException">Refresh cannot initialize the data.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The caller attempts to set an invalid access time.</exception>
        public System.DateTime LastAccessTime
        {
            get { return _fileInfo.LastAccessTime; }
            set { _fileInfo.LastAccessTime = value; }
        }

        /// <summary>
        /// Gets or sets the time, in coordinated universal time (UTC),
        /// that the current file or directory was last accessed.(Inherited from FileSystemInfo.)
        /// </summary>
        /// <exception cref="System.IO.IOException">Refresh cannot initialize the data.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The caller attempts to set an invalid access time.</exception>
        public System.DateTime LastAccessTimeUtc
        {
            get { return _fileInfo.LastAccessTimeUtc; }
            set { _fileInfo.LastAccessTimeUtc = value; }
        }

        /// <summary>
        /// Gets or sets the time when the current file or directory was last written to.(Inherited from FileSystemInfo.)
        /// </summary>
        /// <exception cref="System.IO.IOException">Refresh cannot initialize the data.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The caller attempts to set an invalid write time.</exception>
        public System.DateTime LastWriteTime
        {
            get { return _fileInfo.LastWriteTime; }
            set { _fileInfo.LastWriteTime = value; }
        }

        /// <summary>
        /// Gets or sets the time, in coordinated universal time (UTC), when the current file or directory was last written to.(Inherited from FileSystemInfo.)
        /// </summary>
        /// <exception cref="System.IO.IOException">Refresh cannot initialize the data.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The caller attempts to set an invalid write time.</exception>
        public System.DateTime LastWriteTimeUtc
        {
            get { return _fileInfo.LastWriteTimeUtc; }
            set { _fileInfo.LastWriteTimeUtc = value; }
        }

        /// <summary>
        /// Gets the size, in bytes, of the current file.
        /// </summary>
        /// <exception cref="System.IO.IOException">Refresh cannot initialize the data.</exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// The file does not exist.
        /// -or-
        /// The Length property is called for a directory.
        /// </exception>
        public long Length { get { return _fileInfo.Length; } }

        /// <summary>
        /// Gets the name of the file.(Overrides FileSystemInfo.Name.)
        /// </summary>
        public string Name { get { return _fileInfo.Name; } }
        #endregion

        #region Members
        /// <summary>
        /// Creates a StreamWriter that appends text to the file represented by this instance of the FileInfo.
        /// </summary>
        /// <returns></returns>
        public System.IO.StreamWriter AppendText()
        {
            return _fileInfo.AppendText();
        }

        /// <summary>
        /// Copies an existing file to a new file, disallowing the overwriting of an existing file.
        /// </summary>
        /// <param name="destFileName">The name of the new file to copy to.</param>
        /// <returns>A new file with a fully qualified path.</returns>
        /// <exception cref="System.ArgumentException">destFileName is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="System.IO.IOException">An error occurs, or the destination file already exists.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission </exception>
        /// <exception cref="System.ArgumentNullException">destFileName is null. </exception>
        /// <exception cref="System.UnauthorizedAccessException">A directory path is passed in, or the file is being moved to a different drive.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The directory specified in destFileName does not exist.</exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="System.NotSupportedException">destFileName contains a colon (:) within the string but does not specify the volume.</exception>
        public System.IO.FileInfo CopyTo(string destFileName)
        {
            return _fileInfo.CopyTo(destFileName);
        }

        /// <summary>
        /// Copies an existing file to a new file, allowing the overwriting of an existing file.
        /// </summary>
        /// <param name="destFileName">The name of the new file to copy to. </param>
        /// <param name="overwrite">true to allow an existing file to be overwritten; otherwise, false. </param>
        /// <returns>A new file, or an overwrite of an existing file if overwrite is true. If the file exists and overwrite is false, an IOException is thrown.</returns>
        /// <exception cref="System.ArgumentException">destFileName is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="System.IO.IOException">An error occurs, or the destination file already exists and overwrite is false.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.ArgumentNullException">destFileName is null.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The directory specified in destFileName does not exist.</exception>
        /// <exception cref="System.UnauthorizedAccessException">A directory path is passed in, or the file is being moved to a different drive.</exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="System.NotSupportedException">destFileName contains a colon (:) within the string but does not specify the volume.</exception>
        public System.IO.FileInfo CopyTo(string destFileName, bool overwrite)
        {
            return _fileInfo.CopyTo(destFileName, overwrite);
        }

        /// <summary>
        /// Creates a file.
        /// </summary>
        /// <returns>A new file.</returns>
        public System.IO.FileStream Create()
        {
            return _fileInfo.Create();
        }

        /// <summary>
        /// Creates a StreamWriter that writes a new text file.
        /// </summary>
        /// <returns>A new StreamWriter.</returns>
        /// <exception cref="System.UnauthorizedAccessException">The file name is a directory.</exception>
        /// <exception cref="System.IO.IOException">The disk is read-only.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        public System.IO.StreamWriter CreateText()
        {
            return _fileInfo.CreateText();
        }

        /// <summary>
        /// Decrypts a file that was encrypted by the current account using the Encrypt method.
        /// </summary>
        /// <exception cref="System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The file described by the current System.IO.FileInfo object could not be found.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="System.NotSupportedException">The file system is not NTFS.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file described by the current System.IO.FileInfo object is read-only.
        /// -or-
        /// This operation is not supported on the current platform.-or- The caller does not have the required permission.</exception>
        public void Decrypt()
        {
            _fileInfo.Decrypt();
        }

        /// <summary>
        /// Permanently deletes a file.(Overrides FileSystemInfo.Delete().)
        /// </summary>
        /// <exception cref="System.IO.IOException">The target file is open or memory-mapped on a computer running Microsoft 
        /// Windows NT.-or-There is an open handle on the file, and the operating system
        /// is Windows XP or earlier. This open handle can result from enumerating directories
        /// and files. For more information, see How to: Enumerate Directories and Files.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The path is a directory.</exception>
        public void Delete()
        {
            _fileInfo.Delete();
        }

        /// <summary>
        /// Encrypts a file so that only the account used to encrypt the file can decrypt it.
        /// </summary>
        /// <exception cref="System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The file described by the current System.IO.FileInfo object could not be found.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="System.NotSupportedException">The file system is not NTFS.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file described by the current System.IO.FileInfo object is read-only.
        /// -or-
        /// This operation is not supported on the current platform.-or- The caller does not have the required permission.</exception>
        public void Encrypt()
        {
            _fileInfo.Encrypt();
        }

        /// <summary>
        /// Gets a FileSecurity object that encapsulates the access control list (ACL) entries for the file described by the current FileInfo object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The current operating system is not Microsoft Windows 2000 or later.</exception>
        /// <exception cref="System.Security.AccessControl.PrivilegeNotHeldException">The current system account does not have administrative privileges.</exception>
        /// <exception cref="System.SystemException">The file could not be found.</exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// This operation is not supported on the current platform.
        /// -or-
        /// The caller does not have the required permission.</exception>
        public System.Security.AccessControl.FileSecurity GetAccessControl()
        {
            return _fileInfo.GetAccessControl();
        }

        /// <summary>
        /// Gets a FileSecurity object that encapsulates the specified type of access control list (ACL) entries for the file described by the current FileInfo object.
        /// </summary>
        /// <param name="includeSections"></param>
        /// <returns></returns>
        /// <exception cref="System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The current operating system is not Microsoft Windows 2000 or later.</exception>
        /// <exception cref="System.Security.AccessControl.PrivilegeNotHeldException">The current system account does not have administrative privileges.</exception>
        /// <exception cref="System.SystemException">The file could not be found.</exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// This operation is not supported on the current platform.
        /// -or-
        /// The caller does not have the required permission.</exception>
        public System.Security.AccessControl.FileSecurity GetAccessControl(System.Security.AccessControl.AccessControlSections includeSections)
        {
            return _fileInfo.GetAccessControl(includeSections);
        }

        /// <summary>
        /// Moves a specified file to a new location, providing the option to specify a new file name.
        /// </summary>
        /// <param name="destFileName"></param>
        /// <exception cref="System.IO.IOException">An I/O error occurs, such as the destination file already exists or the destination device is not ready.</exception>
        /// <exception cref="System.ArgumentNullException">destFileName is null.</exception>
        /// <exception cref="System.ArgumentException">destFileName is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.UnauthorizedAccessException">destFileName is read-only or is a directory.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The file is not found.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="System.NotSupportedException">destFileName contains a colon (:) in the middle of the string.</exception>
        public void MoveTo(string destFileName)
        {
            _fileInfo.MoveTo(destFileName);
        }

        /// <summary>
        /// Opens a file in the specified mode.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        /// <exception cref="System.IO.FileNotFoundException">The file is not found.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The file is read-only or is a directory.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="System.IO.IOException">The file is already open.</exception>
        public System.IO.FileStream Open(System.IO.FileMode mode)
        {
            return _fileInfo.Open(mode);
        }

        /// <summary>
        /// Opens a file in the specified mode with read, write, or read/write access.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.ArgumentException">path is empty or contains only white spaces.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The file is not found.</exception>
        /// <exception cref="System.ArgumentNullException">One or more arguments is null.</exception>
        /// <exception cref="System.UnauthorizedAccessException">path is read-only or is a directory.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="System.IO.IOException">The file is already open.</exception>
        public System.IO.FileStream Open(System.IO.FileMode mode, System.IO.FileAccess access)
        {
            return _fileInfo.Open(mode, access);
        }

        /// <summary>
        /// Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="access"></param>
        /// <param name="share"></param>
        /// <returns></returns>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.ArgumentException">path is empty or contains only white spaces.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The file is not found.</exception>
        /// <exception cref="System.ArgumentNullException">One or more arguments is null.</exception>
        /// <exception cref="System.UnauthorizedAccessException">path is read-only or is a directory.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="System.IO.IOException">The file is already open.</exception>
        public System.IO.FileStream Open(System.IO.FileMode mode, System.IO.FileAccess access, System.IO.FileShare share)
        {
            return _fileInfo.Open(mode, access, share);
        }

        /// <summary>
        /// Creates a read-only FileStream.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.UnauthorizedAccessException">path is read-only or is a directory.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="System.IO.IOException">The file is already open.</exception>
        public System.IO.FileStream OpenRead()
        {
            return _fileInfo.OpenRead();
        }

        /// <summary>
        /// Creates a StreamReader with UTF8 encoding that reads from an existing text file.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The file is not found.</exception>
        /// <exception cref="System.UnauthorizedAccessException">path is read-only or is a directory.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        public System.IO.StreamReader OpenText()
        {
            return _fileInfo.OpenText();
        }

        /// <summary>
        /// Creates a write-only FileStream.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.UnauthorizedAccessException">The path specified when creating an instance of the System.IO.FileInfo object is read-only or is a directory.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The path specified when creating an instance of the System.IO.FileInfo object is invalid, such as being on an unmapped drive.</exception>
        public System.IO.FileStream OpenWrite()
        {
            return _fileInfo.OpenWrite();
        }

        /// <summary>
        /// Replaces the contents of a specified file with the file described by the current FileInfo object, deleting the original file, and creating a backup of the replaced file.
        /// </summary>
        /// <param name="destinationFileName"></param>
        /// <param name="destinationBackupFileName"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// The path described by the destFileName parameter was not of a legal form.
        /// -or-
        /// The path described by the destBackupFileName parameter was not of a legal form.</exception>
        /// <exception cref="System.ArgumentNullException">The destFileName parameter is null.</exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// The file described by the current System.IO.FileInfo object could not be found.
        /// -or-
        /// The file described by the destinationFileName parameter could be found.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        public System.IO.FileInfo Replace(string destinationFileName, string destinationBackupFileName)
        {
            return _fileInfo.Replace(destinationFileName, destinationBackupFileName);
        }

        /// <summary>
        /// Replaces the contents of a specified file with the file described by the current FileInfo object, deleting the original file, and creating a backup of the replaced file. Also specifies whether to ignore merge errors.
        /// </summary>
        /// <param name="destinationFileName"></param>
        /// <param name="destinationBackupFileName"></param>
        /// <param name="ignoreMetadataErrors"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// The path described by the destFileName parameter was not of a legal form.
        /// -or-
        /// The path described by the destBackupFileName parameter was not of a legal form.</exception>
        /// <exception cref="System.ArgumentNullException">The destFileName parameter is null.</exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// The file described by the current System.IO.FileInfo object could not befound.
        /// -or-
        /// The file described by the destinationFileName parameter could notbe found.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        public System.IO.FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            return _fileInfo.Replace(destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
        }

        /// <summary>
        ///  Applies access control list (ACL) entries described by a FileSecurity object to the file described by the current FileInfo object.
        /// </summary>
        /// <param name="fileSecurity"></param>
        /// <exception cref="System.ArgumentNullException">The fileSecurity parameter is null.</exception>
        /// <exception cref="System.SystemException">The file could not be found or modified.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The current process does not have access to open the file.</exception>
        /// <exception cref="PlatformNotSupportedException">The current operating system is not Microsoft Windows 2000 or later.</exception>
        public void SetAccessControl(System.Security.AccessControl.FileSecurity fileSecurity)
        {
            _fileInfo.SetAccessControl(fileSecurity);
        }

        /// <summary>
        /// Returns the path as a string. (Overrides Object.ToString().)
        /// </summary>
        /// <returns>A string representing the path.</returns>
        public override string ToString()
        {
            return _fileInfo.ToString();
        }
        #endregion
    }
}