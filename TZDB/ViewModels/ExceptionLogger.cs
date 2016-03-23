/* (c) Copyright Francisco Aguilera (Falven)
 * You are free to edit and distribute this
 * source so long as this statement remains
 * in place, here and in all other such files.
 */

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace NUIClockUpdater.ViewModels
{
    /// <summary>
    /// Basic thread-safe class to log and serialize program exceptions.
    /// Types of Exceptions:
    /// Low - Writes the exception to the Debug command line, and adds the exception to be serialized to the log.
    /// Medium - Writes the exception to the Debug command line, displays a messagebox to the user containing the exception,
    /// and adds the exception to be serialized to the log.
    /// Critical - Writes the exception to the Debug command line, displays a messagebox to the user containing the exception,
    /// adds the exception to be serialized to the log, serializes the log, and exits the program.
    /// TODO serialize as the errors happen
    /// TODO concurrent access to the file.
    /// </summary>
    public class ExceptionLogger
    {
        /// <summary>
        /// Default name of the output file.
        /// </summary>
        private const string DEFAULT_OUTPUTFILE_NAME = @"NUIClockUpdaterExceptions.txt";

        /// <summary>
        /// The maximum exceptions ot buffer before writing to file.
        /// </summary>
        private const int MAX_SIZE = 10000;

        /// <summary>
        /// The file to serialize Exceptions to.
        /// </summary>
        private string _outputFile;

        /// <summary>
        /// The timestamp for the current session.
        /// </summary>
        private string _timeStamp;

        /// <summary>
        /// Whether this is a new session or not.
        /// </summary>
        private bool _isTimeStampSet;

        /// <summary>
        /// Contains all of the exceptions to be serialized.
        /// </summary>
        private ConcurrentQueue<Exception> _exceptionBuffer;

        /// <summary>
        /// Object lock for writing to file.
        /// </summary>
        private object _fileHandleLock;

        /// <summary>
        /// Instantiates a new ExceptionLogger object with the provided optional outputFile.
        /// Defaults to a file in the current directory labeled exceptions.txt
        /// </summary>
        public ExceptionLogger(string outputFile = null)
        {
            _fileHandleLock = new object();

            _exceptionBuffer = new ConcurrentQueue<Exception>();

            _timeStamp = "Session ID: " + DateTime.Now.Day + "\t" + DateTime.Now.Date + "\t" + DateTime.Now.Year;
            _isTimeStampSet = false;

            string directory = Environment.CurrentDirectory;
            _outputFile = outputFile ?? directory + DEFAULT_OUTPUTFILE_NAME;
        }

        /// <summary>
        /// Writes the exception to the Debug command line, and adds the exception to be serialized to the output file.
        /// </summary>
        /// <param name="e">The Exception to log.</param>
        public void LogLow(Exception e)
        {
            _exceptionBuffer.Enqueue(e);
            if (_exceptionBuffer.Count == MAX_SIZE)
            {
                SerializeExceptions();
            }
        }

        /// <summary>
        /// Writes the Exception to the Debug command line, displays a messagebox to the user containing the Exception,
        /// and adds the Exception to be serialized to the log.
        /// </summary>
        /// <param name="e">The Exception to log.</param>
        public void LogMedium(Exception e)
        {
            LogLow(e);
            MessageBoxResult result = System.Windows.MessageBox.Show(
                e.Message,
                "A " + e.GetType().ToString() + " has occured.",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        /// <summary>
        /// Writes the Exception to the Debug command line, displays a messagebox to the user containing the Exception,
        /// adds the Exception to be serialized to the log, serializes the log, and exits the program.
        /// </summary>
        /// <param name="e">The Exception to log.</param>
        public void LogCritical(Exception e)
        {
            LogMedium(e);
            SerializeExceptions();
            Environment.Exit(0);
        }

        /// <summary>
        /// Writes all of the buffered Exceptions to the provided/default ErrorFile.
        /// </summary>
        public void SerializeExceptions()
        {
            lock (_fileHandleLock)
            {
                using (StreamWriter output = new StreamWriter(_outputFile, true))
                {
                    if (!_isTimeStampSet)
                    {
                        output.WriteLine(_timeStamp);
                        output.WriteLine("There were " + _exceptionBuffer.Count + " total Exceptions.\n");
                        _isTimeStampSet = true;
                    }

                    while (!_exceptionBuffer.IsEmpty)
                    {
                        Exception e;
                        if (_exceptionBuffer.TryDequeue(out e))
                        {
                            output.WriteLine(e.GetType().ToString() + ": " + e.Message);
                        }
                        else
                        {
                            _exceptionBuffer.Enqueue(new InvalidOperationException("ExceptionLogger.cs ln 139: Could not dequeue an exception for writing."));
                        }
                    }
                }
            }
        }
    }
}
