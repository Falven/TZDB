/* (c) Copyright Francisco Aguilera (Falven)
 * You are free to edit and distribute this
 * source so long as this statement remains
 * in place, here and in all other such files.
 */

using NUIClockUpdater.Models;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NUIClockUpdater.ViewModels
{
    /// <summary>
    /// GeonamesParser parses Geonames City files from a provided directory into Falven.MetroClock.Cities,
    /// raising it's EntryParsed event for each city parsed, and optionally mapping their TimeZoneId
    /// string property to an int id using a provided IDictionary of TZ names to TimeZoneIds. Instead of
    /// returning a Data Structure with all of the Cities, as this would take up too much memory, a user
    /// may subscribe to the EntryParsed event to retrieve the parsed city.
    /// See http://www.geonames.org/ for a valid, complete directory of City files.
    /// </summary>
    sealed class GeonamesParser : ParserBase
    {
        #region Fields
        private readonly string FEATURECODE_FILE_PATH;

        /// <summary>
        /// The maximum number of threads to use for parsing.
        /// </summary>
        private int _maxParallelism;

        /// <summary>
        /// Pool that maintains string[] objects to be used when splitting.
        /// </summary>
        private ConcurrentPool<string[]> _bufferPool;

        /// <summary>
        /// DataTable that stores all of the featurecodes.
        /// </summary>
        private DataTable _featureCodesDataTable;

        /// <summary>
        /// All of the valid geonamesfiles to parse
        /// </summary>
        private BlockingCollection<GeonamesFileInfo> _geonamesFiles;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new GeonamesParser using the provided IDictionary to map each Cities'
        /// string TZ Name to int TimeZone ids.
        /// </summary>
        public GeonamesParser(DataTable featureCodesDataTable, GeonamesDirectoryInfo directory)
        {
            Directory = directory;

            _geonamesFiles = directory.ValidGeonamesFiles;

            FEATURECODE_FILE_PATH = Environment.CurrentDirectory + @"\Resources\geonames_feature_codes.txt";

            _regex = new Regex(@"\t", RegexOptions.Compiled);

            _maxParallelism = Environment.ProcessorCount;

            // Assign a reusable buffer for each thread.
            _bufferPool = new ConcurrentPool<string[]>(_maxParallelism);

            // Allocate buffer pool.
            for (int i = 0; i < _maxParallelism; i++)
            {
                // Adding string[] with max number of City fields for size.
                _bufferPool.Push(new string[City.NUM_FIELDS]);
            }

            ParsedEntries = 0;

            if (null != featureCodesDataTable)
            {
                _featureCodesDataTable = featureCodesDataTable;
            }
            else
            {
                throw new ArgumentNullException("featureCodesDataTable");
            }
        }
        #endregion

        #region Members
        public DataTable ReadFeatureCodesFile()
        {
            var featureCodesFile = new NUIClockUpdater.Models.FileInfo(FEATURECODE_FILE_PATH);
            RaiseFileParsing(featureCodesFile);
            if (featureCodesFile.Exists)
            {
                using (StreamReader input = new StreamReader(FEATURECODE_FILE_PATH))
                {
                    while (!input.EndOfStream)
                    {
                        string[] values = _regex.Split(input.ReadLine());
                        _featureCodesDataTable.Rows.Add(new object[]
                        {
                            DBNull.Value,
                            values[0],
                            values[1],
                            values[2]
                        });
                    }
                }
            }
            RaiseFileParsed(featureCodesFile);
            return _featureCodesDataTable;
        }

        /// <summary>
        /// Reads, parses, and inserts all of the Cities from the Geonames city files
        /// found on the provided directory. The provided directory must contain 
        /// Valid Geonames city files.
        /// </summary>
        public void ReadCityFiles()
        {
            Parallel.ForEach<GeonamesFileInfo>(
                _geonamesFiles.GetConsumingPartitioner<GeonamesFileInfo>(),
                new ParallelOptions { MaxDegreeOfParallelism = _maxParallelism },
                (inputFile, args) =>
                {
                    RaiseFileParsing(inputFile);
                    using (StreamReader input = new StreamReader(inputFile.FullName))
                    {
                        while (!input.EndOfStream)
                        {
                            RaiseEntryParsed(ParseCity(input.ReadLine()));
                            Interlocked.Increment(ref _parsedEntries);
                        }
                    }
                    RaiseFileParsed(inputFile);
                });
            RaiseDirectoryParsed(Directory);
        }

        /// <summary>
        /// Parses the provided line into a City.
        /// </summary>
        /// <param name="line">The line to parse into a city.</param>
        /// <returns>A city parsed from the provided line.</returns>
        private object[] ParseCity(string line)
        {
            // Fields corresponding to each City data type.
            string[] fields = SplitTabs(line);

            // Parse fields into each data type.
            object[] values = new object[]
            {
                (object)DBNull.Value,
                City.ParseName(fields[City.NameIndex]),
                City.ParseAsciiName(fields[City.AsciiNameIndex]),
                City.ParseAlternateNames(fields[City.AlternateNamesIndex]),
                City.ParseLatitude(fields[City.LatitudeIndex]),
                City.ParseLongitude(fields[City.LongitudeIndex]),
                City.ParseFeatureClass(fields[City.FeatureClassIndex]) ?? (object)DBNull.Value,
                City.ParseFeatureCode(fields[City.FeatureCodeIndex]),
                City.ParseCountryCode(fields[City.CountryCodeIndex]),
                City.ParseCountryCode2(fields[City.CountryCode2Index]),
                City.ParsePopulation(fields[City.PopulationIndex]),
                City.ParseElevation(fields[City.ElevationIndex]) ?? (object)DBNull.Value,
                City.ParseModificationDate(fields[City.ModificationDateIndex]),
                City.ParseAdmin1Code(fields[City.Admin1CodeIndex]),
                City.ParseAdmin2Code(fields[City.Admin2CodeIndex]),
                City.ParseAdmin3Code(fields[City.Admin3CodeIndex]),
                City.ParseAdmin4Code(fields[City.Admin4CodeIndex]),
                City.ParseGtopo30(fields[City.Gtopo30Index]),
                (object)DBNull.Value,
                fields[City.TimeZoneIdIndex],
                (object)DBNull.Value
            };

            _bufferPool.Push(fields);

            return values;
        }

        /// <summary>
        /// An optimized split implementation to split lines by tabs.
        /// </summary>
        /// <param name="line">Reference to the to split.</param>
        /// <returns>The split result of the provided line.</returns>
        private string[] SplitTabs(string line)
        {
            int bufferIndex = 0;
            string[] buffer = _bufferPool.Pop();
            StringBuilder bufferToken = new StringBuilder(line.Length / City.NUM_FIELDS);
            foreach (char c in line)
            {
                if (c == '\t')
                {
                    buffer[bufferIndex++] = bufferToken.ToString();
                    bufferToken.Clear();
                    continue;
                }
                bufferToken.Append(c);
            }
            buffer[bufferIndex] = bufferToken.ToString();
            return buffer;
        }
        #endregion
    }
}