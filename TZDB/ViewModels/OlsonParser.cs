/* (c) Copyright Francisco Aguilera (Falven)
 * You are free to edit and distribute this
 * source so long as this statement remains
 * in place, here and in all other such files.
 */

using NUIClockUpdater.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NUIClockUpdater.ViewModels
{
    /// <summary>
    /// OlsonParser parses the Olson, or TZ Database from a specified directory into Rule,
    /// and TimeZone entities. This class raises it's EntryParsed event for each Rule or
    /// TimeZone parsed. This class also creates a map relationship from TZNames to TZid's, for easy
    /// integration with a GeonamesParser object.
    /// Instead of returning a Data Structure with all of the TimeZones and Rules, as this could take up
    /// too much memory, a user may subscribe to the EntryParsed event to retrieve the parsed TimeZone or Rule.
    /// See http://www.twinsun.com/tz/tz-link.htm for valid, complete TZ Database files.
    /// </summary>
    sealed class OlsonParser : ParserBase
    {
        #region Fields
        public const char COMMENT_CHAR = '#';

        /// <summary>
        /// Name of the file containing the TZName, Ccode and ID for a valid mapping.
        /// </summary>
        private const string ZONE_TAB_NAME = @"zone.tab";

        /// <summary>
        /// Name of the file containing the Ccode and Cname for Ccode to Cname mapping.
        /// </summary>
        private const string ISO_TAB_NAME = @"iso3166.tab";

        private int _curYear;

        /// <summary>
        /// The last assigned unique identifier.
        /// </summary>
        private int _lastUID;

        /// <summary>
        /// All of the parsed Links.
        /// </summary>
        private List<string[]> _links;

        /// <summary>
        /// Reference to the rows collection of the TimeZones DataTable.
        /// </summary>
        private System.Data.DataRowCollection _timeZoneRows;

        /// <summary>
        /// Reference to the rows collection of the Rules DataTable.
        /// </summary>
        private System.Data.DataRowCollection _ruleRows;

        /// <summary>
        /// Reference to the rows collection of the Leaps DataTable.
        /// </summary>
        private System.Data.DataRowCollection _leapRows;

        /// <summary>
        /// All of the files to be parsed by this olson parser.
        /// </summary>
        private BlockingCollection<OlsonFileInfo> _olsonFiles;

        /// <summary>
        /// The tab file containing the Ccode and Cname for Ccode to Cname mapping.
        /// </summary>
        private OlsonFileInfo _isoFile;

        /// <summary>
        /// The tab file containing the TZName, Ccode and ID for a valid mapping.
        /// </summary>
        private OlsonFileInfo _zoneFile;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new OlsonParser to parse the provided
        /// Olson/TZ Database directory into Rules
        /// and TimeZones. The directory provided must be a valid TZ Database. 
        /// </summary>
        /// <param name="timeZoneDataTable">The TimeZone DataTable containign the schema to retrieve new rows from.</param>
        /// <param name="ruleDataTable">The Rule DataTable containign the schema to retrieve new rows from.</param>
        /// <param name="leapDataTable">The Leap DataTable containign the schema to retrieve new rows from.</param>
        /// <param name="directoryPath">The valid directory to parse Rules and TimeZones from.</param>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="System.ArgumentException">directoryPath contains invalid characters such as ", <, >, or |. </exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// The specified path, file name, or both are too long.</exception>
        public OlsonParser(System.Data.DataTable timeZoneDataTable,
            System.Data.DataTable ruleDataTable,
            System.Data.DataTable leapDataTable,
            OlsonDirectoryInfo files)
        {
            Directory = files;

            _olsonFiles = files.ValidOlsonFiles;
            _isoFile = files.IsoFile;
            _zoneFile = files.ZoneFile;

            ParsedEntries = 0;

            TimeZones = timeZoneDataTable;
            _timeZoneRows = TimeZones.Rows;

            Rules = ruleDataTable;
            _ruleRows = Rules.Rows;

            Leaps = leapDataTable;
            _leapRows = Leaps.Rows;

            _links = new List<string[]>();
            TimeZoneLookupTable = new ConcurrentDictionary<string, System.Data.DataRow>(System.Environment.ProcessorCount, 450);

            _curYear = System.DateTime.Now.Year;

            // Pre-compiled regex for faster matching. Fields are separated from one another by any number of white space characters.
            _regex = new Regex(@"\s+", RegexOptions.Compiled);
        }
        #endregion

        #region Properties
        /// <summary>
        /// All of the parsed TimeZones.
        /// </summary>
        public System.Data.DataTable TimeZones { get; private set; }

        /// <summary>
        /// All of the parsed rules.
        /// </summary>
        public System.Data.DataTable Rules { get; private set; }

        /// <summary>
        /// All of the parsed Leaps.
        /// </summary>
        public System.Data.DataTable Leaps { get; private set; }

        /// <summary>
        /// All of the parsed TimeZones
        /// </summary>
        public ConcurrentDictionary<string, System.Data.DataRow> TimeZoneLookupTable { get; private set; }
        #endregion

        #region Members
        /// <summary>
        /// Reads and parses the .tab files in the provided TZ/Olson Database's driectory.
        /// The tab files are used to create the correct database mappings from TZName to TZId.
        /// </summary>
        /// <param name="directory">A directory containing valid Olson ".tab" files.</param>
        /// <exception cref="System.ArgumentNullException">fileName is null.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.ArgumentException">The file name is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="System.UnauthorizedAccessException">Access to fileName is denied.</exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="System.NotSupportedException">fileName contains a colon (:) in the middle of the string.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The filePath provided does not exist.</exception>
        /// <exception cref="Updater.Models.InvaliOlsonFileException">The file provided is not a valid Olson Database file.</exception>
        private IDictionary<string, string[]> ReadTabFiles()
        {
            // Map that will store Country Codes to Country Names from iso3166.
            var ccToCn = new Dictionary<string, string>(275);
            // Map that will store TZ names to id, country codes, country names, coordinates, and comments.
            var tzNameToInfo = new Dictionary<string, string[]>(450);

            RaiseFileParsing(_isoFile);
            using (var input = new StreamReader(_isoFile.FullName))
            {
                while (!input.EndOfStream)
                {
                    string line = input.ReadLine();

                    if (!isCommentNullOrWhiteSpace(line))
                    {
                        // Parse countryCode and countryName from each line.
                        string countryCode = line.Substring(0, 2);
                        string countryName = line.Substring(3, line.Length - 3);

                        ccToCn.Add(countryCode, countryName);
                    }
                }
            }
            RaiseFileParsed(_isoFile);

            RaiseFileParsing(_zoneFile);
            using (var input = new StreamReader(_zoneFile.FullName))
            {
                // Unique ID assigned to each TZName.
                _lastUID = 0;
                while (!input.EndOfStream)
                {
                    string line = input.ReadLine();
                    if (!isCommentNullOrWhiteSpace(line))
                    {
                        string[] fields = _regex.Split(line);

                        // Parsing fields.
                        string cCode = fields[0];
                        string coord = fields[1];
                        string tzName = fields[2];
                        string comments = null;
                        if (fields.Length > 3)
                        {
                            comments = fields[3];
                        }
                        string cName = "";
                        string uIdStr = (++_lastUID).ToString();
                        if (!ccToCn.TryGetValue(cCode, out cName))
                        {
                            App.ExceptionLogger.LogLow(new System.Exception("OlsonParser.cs Ln 169: Could not assign CountryName to TimeZone with ID: "
                                + uIdStr));
                        }

                        tzNameToInfo.Add(tzName, new string[] { uIdStr, cCode, cName, coord, comments });
                    }
                }
            }
            RaiseFileParsed(_zoneFile);

            return tzNameToInfo;
        }

        /// <summary>
        /// Parses TimeZones and Rules from the files in the provided
        /// Olson/TZ Database directory. The directory provided must be a valid TZ Database.
        /// </summary>
        /// <param name="directory">The valid directory to parse Rules and TimeZones from.</param>
        public void ReadTZDirectory()
        {
            try
            {
                // Mapping from TZname to: id, Ccode, Cname.
                var tzNameToInfo = ReadTabFiles();

                TimeZones.BeginLoadData();
                Rules.BeginLoadData();
                Leaps.BeginLoadData();

                foreach (OlsonFileInfo file in _olsonFiles.GetConsumingEnumerable())
                {
                    if (!file.IsTabFile)
                    {
                        RaiseFileParsing(file);
                        string[] lines = File.ReadAllLines(file.FullName);

                        // Process items.
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string line = lines[i].TrimStart();

                            if (!isCommentNullOrWhiteSpace(line))
                            {
                                string[] fields = ParseFields(line);

                                switch (fields[0])
                                {
                                    case Rule.RULE_NAME:
                                        {
                                            short from = Rule.ParseStartYear(fields[Rule.FromIndex]);
                                            short to = Rule.ParseEndYear(fields[Rule.ToIndex], from);

                                            if (from > _curYear || to < _curYear)
                                            {
                                                continue;
                                            }

                                            // No parsing necessary.
                                            string name = fields[Rule.NameIndex];

                                            var rule = Rules.NewRow();
                                            rule[1] = name;
                                            rule[2] = Rule.ParseBias(fields[Rule.SaveIndex]);
                                            rule[3] = from;
                                            rule[4] = to;
                                            rule[5] = Rule.ParseMonth(fields[Rule.InIndex]);
                                            rule[6] = fields[Rule.OnIndex];
                                            rule[7] = Rule.ParseTime(fields[Rule.AtIndex]);
                                            rule[8] = Rule.ParseTimeType(ref fields[Rule.AtIndex]);
                                            rule[9] = Rule.ParseAbrev(fields[Rule.LetterIndex]) ?? (object)System.DBNull.Value;

                                            _ruleRows.Add(rule);
                                            RaiseEntryParsed(null);
                                            break;
                                        }

                                    case TimeZone.ZONE_NAME:
                                        {
                                            // First TZ.
                                            int init = i;
                                            // Count to last
                                            do
                                            {
                                                i++;
                                            } while (i < lines.Length && TimeZone.IsContinuation(lines[i]));

                                            // If first was not the only...
                                            if (i - init != 1)
                                            {
                                                string[] continuation = ParseFields(lines[--i].Trim());

                                                // Continuation has a non-tab delimited date...
                                                if (continuation.Length > 4 && int.Parse(continuation[3]) < _curYear)
                                                {
                                                    continue;
                                                }
                                                System.Array.Copy(continuation, 0, fields, 2, continuation.Length);
                                            }
                                            else
                                            {
                                                if (fields.Length > 5 && int.Parse(fields[5]) < _curYear)
                                                {
                                                    continue;
                                                }
                                            }

                                            string name = TimeZone.ParseName(fields[TimeZone.NameIndex]);
                                            string countryCode = null;
                                            string countryName = null;
                                            string coord = null;
                                            string comments = null;
                                            int id = 0;
                                            string[] tokens;
                                            if (tzNameToInfo.TryGetValue(name, out tokens))
                                            {
                                                id = System.Convert.ToInt32(tokens[0]);
                                                countryCode = tokens[1];
                                                countryName = tokens[2];
                                                coord = tokens[3];
                                                comments = tokens[4];
                                            }
                                            else
                                            {
                                                App.ExceptionLogger.LogLow(new System.Exception("OlsonParser.cs Ln 390:"
                                                    + "Error attempting to get a TimeZone's iso/tab info."));
                                                continue;
                                            }
                                            short bias = TimeZone.ParseBias(fields[TimeZone.GMTOffsetIndex]);
                                            string ruleName = TimeZone.ParseRuleName(fields[TimeZone.RuleIndex]);
                                            string tzAbrev = TimeZone.ParseTzAbrev(fields[TimeZone.FormatIndex]);

                                            var timeZone = TimeZones.NewRow();
                                            timeZone[0] = id;
                                            timeZone[1] = name;
                                            timeZone[2] = bias;
                                            timeZone[3] = ruleName;
                                            timeZone[4] = tzAbrev;
                                            timeZone[5] = countryCode;
                                            timeZone[6] = countryName;
                                            timeZone[7] = comments;
                                            timeZone[8] = coord;

                                            TimeZoneLookupTable[name] = timeZone;
                                            _timeZoneRows.Add(timeZone);
                                            RaiseEntryParsed(null);
                                            break;
                                        }
                                    case Link.LINK_NAME:
                                        {
                                            var link = new string[] { fields[Link.FromZoneNameIndex], fields[Link.ToZoneNameIndex] };
                                            _links.Add(link);
                                            RaiseEntryParsed(null);
                                            break;
                                        }
                                    case Leap.LEAP_NAME:
                                        {
                                            var leap = Leaps.NewRow();
                                            leap[1] = Leap.ParseYear(fields[Leap.YearIndex]);
                                            leap[2] = fields[Leap.MonthIndex];
                                            leap[3] = Leap.ParseDay(fields[Leap.DayIndex]);
                                            leap[4] = Leap.ParseTime(fields[Leap.TimeIndex]);
                                            leap[5] = Leap.ParseCorrection(fields[Leap.CorrectionIndex]);
                                            leap[6] = Leap.ParseRs(fields[Leap.RsIndex]);

                                            _leapRows.Add(leap);
                                            RaiseEntryParsed(null);
                                            break;
                                        }
                                }
                                ParsedEntries++;
                            }
                        }
                        RaiseFileParsed(file);
                    }
                }
                // Adding all links to list of TimeZones.
                foreach (string[] link in _links)
                {
                    System.Data.DataRow from;
                    if (!TimeZoneLookupTable.TryGetValue(link[Link.FromZoneNameIndex], out from))
                    {
                        App.ExceptionLogger.LogLow(new System.Exception("OlsonParser.cs Ln 364: Error attempting to find the \"From\" TimeZone for the link:"
                            + link[0].ToString()));
                        continue;
                    }

                    string toName = link[Link.ToZoneNameIndex];
                    System.Data.DataRow to = TimeZones.NewRow();
                    to[0] = ++_lastUID;
                    to[1] = toName;
                    to[2] = from[2];
                    to[3] = from[3];
                    to[4] = from[4];
                    to[5] = from[5];
                    to[6] = from[6];
                    to[7] = from[7];
                    to[8] = from[8];

                    if (!TimeZoneLookupTable.TryAdd(toName, to))
                    {
                        App.ExceptionLogger.LogLow(new System.Exception("OlsonParser.cs Ln 373: Error attempting to add the TimeZone for the link:"
                            + link.ToString()));
                        continue;
                    }
                    try
                    {
                        _timeZoneRows.Add(to);
                        RaiseEntryParsed(null);
                        ParsedEntries++;
                    }
                    catch (System.Exception e)
                    {
                        App.ExceptionLogger.LogLow(e);
                        continue;
                    }
                }

                RaiseDirectoryParsed(Directory);
            }
            finally
            {
                TimeZones.EndLoadData();
                Rules.EndLoadData();
                Leaps.EndLoadData();
            }
        }

        /// <summary>
        /// Returns whether the provided line is a comment or empty line.
        /// </summary>
        /// <param name="line">The line to determine.</param>
        /// <returns>True if the provided line is a comment or empty, false otherwise.</returns>
        private bool isCommentNullOrWhiteSpace(string line)
        {
            return string.IsNullOrWhiteSpace(line) || line[0] == COMMENT_CHAR;
        }

        /// <summary>
        /// Parses the provided line into fields.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <returns>The fields parsed from the provided line.</returns>
        private string[] ParseFields(string line)
        {
            // Eliminating inline comments.
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == COMMENT_CHAR)
                {
                    break;
                }
                sb.Append(c);
            }
            return _regex.Split(sb.ToString());
        }
        #endregion
    }
}