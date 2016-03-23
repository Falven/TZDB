/* (c) Copyright Francisco Aguilera (Falven)
 * You are free to edit and distribute this
 * source so long as this statement remains
 * in place, here and in all other such files.
 */

using System;
using System.Data;
using System.Data.Linq;

namespace NUIClockUpdater.Models
{
    /// <summary>
    /// Static methods and constant integers representing various parsing operations
    /// to be done on a City to extract property information, and the field indexes
    /// of such properties, consequently.
    /// </summary>
    partial class City
    {
        public const string TABLE_SCHEMA = "dbo";
        public const string TABLE_NAME = "cities";
        public const string FULL_TABLE_NAME = TABLE_SCHEMA + "." + TABLE_NAME;

        public const int NUM_FIELDS = 19;
        public const int IdIndex = 0;
        public const int NameIndex = 1;
        public const int AsciiNameIndex = 2;
        public const int AlternateNamesIndex = 3;
        public const int LatitudeIndex = 4;
        public const int LongitudeIndex = 5;
        public const int FeatureClassIndex = 6;
        public const int FeatureCodeIndex = 7;
        public const int CountryCodeIndex = 8;
        public const int CountryCode2Index = 9;
        public const int Admin1CodeIndex = 10;
        public const int Admin2CodeIndex = 11;
        public const int Admin3CodeIndex = 12;
        public const int Admin4CodeIndex = 13;
        public const int PopulationIndex = 14;
        public const int ElevationIndex = 15;
        public const int Gtopo30Index = 16;
        public const int TimeZoneIdIndex = 17;
        public const int ModificationDateIndex = 18;

        /// <summary>
        /// Parses the name of the geographical point for a City.
        /// </summary>
        /// <param name="name">The string to parse into a name.</param>
        /// <returns>The name parsed from the provided string.</returns>
        public static string ParseName(string name)
        {
            return name.Equals(string.Empty) ? null : name;
        }

        /// <summary>
        /// Parses the asciiname of the geographical point,
        /// in plain ascii characters, for a City.
        /// </summary>
        /// <param name="name">The string to parse into an asciiname.</param>
        /// <returns>The asciiname parsed from the provided string.</returns>
        public static string ParseAsciiName(string asciiName)
        {
            return ParseName(asciiName);
        }

        /// <summary>
        /// Parses the name of the geographical point for a City.
        /// </summary>
        /// <param name="name">The string to parse into a name.</param>
        /// <returns>The name parsed from the provided string.</returns>
        public static string ParseAlternateNames(string alternateNames)
        {
            return ParseName(alternateNames);
        }

        /// <summary>
        /// Parses the Latitude for a City.
        /// </summary>
        /// <param name="name">The string to parse into a latitude.</param>
        /// <returns>The latitude parsed from the provided string.</returns>
        public static double ParseLatitude(string latitude)
        {
            return double.Parse(latitude);
        }

        /// <summary>
        /// Parses the Longitude for a City.
        /// </summary>
        /// <param name="name">The string to parse into a longitude.</param>
        /// <returns>The longitude parsed from the provided string.</returns>
        public static double ParseLongitude(string longitude)
        {
            return ParseLatitude(longitude);
        }

        /// <summary>
        /// Parses the FeatureClass identifier for a City.
        /// See http://www.geonames.org/export/codes.html
        /// </summary>
        /// <param name="name">The string to parse into a featureclass identifier.</param>
        /// <returns>The featureclass identifier parsed from the provided string.</returns>
        public static char? ParseFeatureClass(string featureClass)
        {
            return featureClass.Equals(string.Empty) ? null : (char?)(featureClass[0]);
        }

        /// <summary>
        /// Parses the FeatureCode identifier for a City.
        /// See http://www.geonames.org/export/codes.html
        /// </summary>
        /// <param name="name">The string to parse into a FeatureCode.</param>
        /// <returns>The FeatureCode parsed from the provided string.</returns>
        public static string ParseFeatureCode(string featureCode)
        {
            return ParseName(featureCode);
        }

        /// <summary>
        /// Parses an ISO-3166 2-letter country code for a city.
        /// </summary>
        /// <param name="name">The string to parse into a country code.</param>
        /// <returns>The country code parsed from the provided string.</returns>
        public static string ParseCountryCode(string countryCode)
        {
            return ParseName(countryCode);
        }

        /// <summary>
        /// Parses alternate, comma separated ISO-3166 2-letter country codes for a City.
        /// </summary>
        /// <param name="name">The string to parse into a country code.</param>
        /// <returns>The country codes parsed from the provided string.</returns>
        public static string ParseCountryCode2(string countryCode2)
        {
            return ParseName(countryCode2);
        }

        /// <summary>
        /// Parses an admin1code for a City.
        /// </summary>
        /// <param name="name">The string to parse into a admin1code.</param>
        /// <returns>The admin1code parsed from the provided string.</returns>
        public static string ParseAdmin1Code(string admin1Code)
        {
            return ParseName(admin1Code);
        }

        /// <summary>
        /// Parses an admin2code for a City.
        /// </summary>
        /// <param name="name">The string to parse into an admin2code.</param>
        /// <returns>The admin2code parsed from the provided string.</returns>
        public static string ParseAdmin2Code(string admin2Code)
        {
            return ParseName(admin2Code);
        }

        /// <summary>
        /// Parses an admin3code for a City.
        /// </summary>
        /// <param name="name">The string to parse into an admin3code.</param>
        /// <returns>The admin3code parsed from the provided string.</returns>
        public static string ParseAdmin3Code(string admin3Code)
        {
            return ParseName(admin3Code);
        }

        /// <summary>
        /// Parses an admin4code for a City.
        /// </summary>
        /// <param name="name">The string to parse into an admin4code.</param>
        /// <returns>The admin4code parsed from the provided string.</returns>
        public static string ParseAdmin4Code(string admin4Code)
        {
            return ParseName(admin4Code);
        }

        /// <summary>
        /// Parses the population for a city.
        /// </summary>
        /// <param name="name">The string to parse into a population.</param>
        /// <returns>The population parsed from the provided string.</returns>
        public static long? ParsePopulation(string population)
        {
            return population.Equals(string.Empty) ? null : (long?)long.Parse(population);
        }

        /// <summary>
        /// Parses the elevation for a City.
        /// </summary>
        /// <param name="name">The string to parse into an elevation.</param>
        /// <returns>The elevation parsed from the provided string.</returns>
        public static int? ParseElevation(string elevation)
        {
            return elevation.Equals(string.Empty) ? null : (int?)int.Parse(elevation);
        }

        /// <summary>
        /// Parses the Gtopo30 for a City.
        /// </summary>
        /// <param name="name">The string to parse into a Gtopo30.</param>
        /// <returns>The Gtopo30 parsed from the provided string.</returns>
        public static int? ParseGtopo30(string gtopo30)
        {
            return ParseElevation(gtopo30);
        }

        /// <summary>
        /// Parses the date of last modification in yyyy-MM-dd format for a City.
        /// </summary>
        /// <param name="name">The string to parse into date.</param>
        /// <returns>The date parsed from the provided string.</returns>
        public static DateTime ParseModificationDate(string modificationDate)
        {
            #region OLDCODE
            //int year = 0;
            //int month = 0;
            //int day = 0;
            //StringBuilder sb = new StringBuilder(4);
            //for (int i = 0; i < modificationDate.Length; i++)
            //{
            //    char c = modificationDate[i];
            //    if (c == '-')
            //    {
            //        if (i == 4)
            //        {
            //            year = int.Parse(sb.ToString());
            //        }
            //        if (i == 7)
            //        {
            //            month = int.Parse(sb.ToString());
            //        }
            //        sb.Clear();
            //        continue;
            //    }
            //    sb.Append(c);
            //}
            //day = int.Parse(sb.ToString());
            //return new DateTime(year, month, day);
            #endregion
            DateTime newDate;
            if (!DateTime.TryParse(modificationDate, out newDate))
            {
                App.ExceptionLogger.LogLow(new Exception("CityOps.cs Ln 239: Could not parse a City's ModificationDate."));
                newDate = new DateTime();
            }
            return newDate;
        }
    }
}
