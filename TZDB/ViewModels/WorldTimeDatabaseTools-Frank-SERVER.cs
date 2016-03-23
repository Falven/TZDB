/* (c) Copyright Francisco Aguilera (Falven)
 * You are free to edit and distribute this
 * source so long as this statement remains
 * in place, here and in all other such files.
 */

using System;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Text;

namespace NUIClockUpdater.ViewModels
{
    public class WorlTimeDatabaseTools
    {
        private const string TABLE_SCHEMA = "dbo";
        private const string TIMEZONE_TABLE_NAME = "timezones";
        private const string RULE_TABLE_NAME = "rules";
        private const string CITY_TABLE_NAME = "cities";
        private const string FEATURECODE_TABLE_NAME = "feature_codes";

        private SqlConnectionStringBuilder _connectionString;

        /// <summary>
        /// Creates a new WorldTimeDatabaseTools instance for the Database given by the provided connectionString.
        /// </summary>
        /// <param name="connectionString">A valid connectionString to the Database.</param>
        public WorlTimeDatabaseTools(SqlConnectionStringBuilder connectionString = null)
        {
            if (null != connectionString)
            {
                _connectionString = connectionString;
            }
        }

        /// <summary>
        /// The ConnectionString for the Database to be managed by these WorldTimeDatabaseTools.
        /// </summary>
        public SqlConnectionStringBuilder ConnectionString
        {
            get { return _connectionString; }
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("ConnectionString");
                }
                _connectionString = value;
            }
        }

        /// <summary>
        /// Creates and returns a new TimeZone DataTable.
        /// </summary>
        /// <returns>A new TimeZone DataTable.</returns>
        public DataTable GetTimeZoneDataTable()
        {
            DataTable timeZoneDataTable = new DataTable(TIMEZONE_TABLE_NAME, TABLE_SCHEMA);
            timeZoneDataTable.Columns.Add("id", typeof(int));
            timeZoneDataTable.Columns.Add("name", typeof(string));
            timeZoneDataTable.Columns.Add("bias", typeof(short));
            timeZoneDataTable.Columns.Add("rule_name", typeof(string));
            timeZoneDataTable.Columns.Add("tz_abreviation", typeof(string));
            timeZoneDataTable.Columns.Add("country_code", typeof(string));
            timeZoneDataTable.Columns.Add("country_name", typeof(string));
            timeZoneDataTable.Columns.Add("comments", typeof(string));
            timeZoneDataTable.Columns.Add("coordinates", typeof(string));
            timeZoneDataTable.Columns.Add("version", typeof(Binary));
            return timeZoneDataTable;
        }

        /// <summary>
        /// Creates and returns a new Rule DataTable.
        /// </summary>
        /// <returns>A new Rule DataTable.</returns>
        public DataTable GetRuleDataTable()
        {
            DataTable ruleDataTable = new DataTable(RULE_TABLE_NAME, TABLE_SCHEMA);
            ruleDataTable.Columns.Add("id", typeof(int));
            ruleDataTable.Columns.Add("name", typeof(string));
            ruleDataTable.Columns.Add("bias", typeof(short));
            ruleDataTable.Columns.Add("start_year", typeof(short));
            ruleDataTable.Columns.Add("end_year", typeof(short));
            ruleDataTable.Columns.Add("month", typeof(byte));
            ruleDataTable.Columns.Add("date", typeof(string));
            ruleDataTable.Columns.Add("time", typeof(System.TimeSpan));
            ruleDataTable.Columns.Add("time_type", typeof(char));
            ruleDataTable.Columns.Add("letter", typeof(char));
            ruleDataTable.Columns.Add("version", typeof(Binary));
            return ruleDataTable;
        }

        /// <summary>
        /// Creates and returns a new City DataTable.
        /// </summary>
        /// <returns>A new City DataTable.</returns>
        public DataTable GetCityDataTable()
        {
            DataTable cityDataTable = new DataTable(CITY_TABLE_NAME, TABLE_SCHEMA);
            cityDataTable.Columns.Add("id", typeof(int));
            cityDataTable.Columns.Add("name", typeof(string));
            cityDataTable.Columns.Add("ascii_name", typeof(string));
            cityDataTable.Columns.Add("alternate_names", typeof(string));
            cityDataTable.Columns.Add("latitude", typeof(double));
            cityDataTable.Columns.Add("longitude", typeof(double));
            cityDataTable.Columns.Add("feature_class", typeof(char));
            cityDataTable.Columns.Add("feature_code", typeof(string));
            cityDataTable.Columns.Add("country_code", typeof(string));
            cityDataTable.Columns.Add("country_code2", typeof(string));
            cityDataTable.Columns.Add("population", typeof(long));
            cityDataTable.Columns.Add("elevation", typeof(int));
            cityDataTable.Columns.Add("modification_date", typeof(System.DateTime));
            cityDataTable.Columns.Add("admin1code", typeof(string));
            cityDataTable.Columns.Add("admin2code", typeof(string));
            cityDataTable.Columns.Add("admin3code", typeof(string));
            cityDataTable.Columns.Add("admin4code", typeof(string));
            cityDataTable.Columns.Add("gtopo30", typeof(int));
            cityDataTable.Columns.Add("timezone_id", typeof(int));
            cityDataTable.Columns.Add("timezone_name", typeof(string));
            cityDataTable.Columns.Add("version", typeof(Binary));
            return cityDataTable;
        }

        /// <summary>
        /// Creates and returns a new FeatureCodedataTable.
        /// </summary>
        /// <returns>A new featurecode datatable.</returns>
        public DataTable GetFeatureCodeDataTable()
        {
            DataTable featureCodeDataTable = new DataTable(FEATURECODE_TABLE_NAME, TABLE_SCHEMA);
            featureCodeDataTable.Columns.Add("id", typeof(int));
            featureCodeDataTable.Columns.Add("code", typeof(string));
            featureCodeDataTable.Columns.Add("name", typeof(string));
            featureCodeDataTable.Columns.Add("description", typeof(string));
            return featureCodeDataTable;
        }

        /// <summary>
        /// Creates the appropriate TimeZone, Rule, and City Database tables on the Database.
        /// </summary>
        public void CreateOlsonDatabaseTables()
        {
            StringBuilder commandBuilder = new StringBuilder("USE ");
            commandBuilder.Append(_connectionString.InitialCatalog);
            commandBuilder.Append(";\n");
            // Drop tables if they exist
            commandBuilder.Append("\nIF OBJECT_ID('");
            commandBuilder.Append(TIMEZONE_TABLE_NAME);
            commandBuilder.Append("', 'U') IS NOT NULL\n");
            commandBuilder.Append("\tDROP TABLE ");
            commandBuilder.Append(TIMEZONE_TABLE_NAME);
            commandBuilder.Append(";\n");
            commandBuilder.Append("\nIF OBJECT_ID('");
            commandBuilder.Append(RULE_TABLE_NAME);
            commandBuilder.Append("', 'U') IS NOT NULL\n");
            commandBuilder.Append("\tDROP TABLE ");
            commandBuilder.Append(RULE_TABLE_NAME);
            commandBuilder.Append(";\n");
            // Create tables
            commandBuilder.Append("CREATE TABLE ");
            commandBuilder.Append(TIMEZONE_TABLE_NAME);
            commandBuilder.Append("\n(\n");
            commandBuilder.Append("\t[id] INT NOT NULL PRIMARY KEY,\n");
            commandBuilder.Append("\t[name] VARCHAR(30) NOT NULL,\n");
            commandBuilder.Append("\t[bias] SMALLINT NOT NULL,\n");
            commandBuilder.Append("\t[rule_name] VARCHAR(10),\n");
            commandBuilder.Append("\t[tz_abreviation] VARCHAR(7) NOT NULL,\n");
            commandBuilder.Append("\t[country_code] CHAR(2) NOT NULL,\n");
            commandBuilder.Append("\t[country_name] VARCHAR(42) NOT NULL,\n");
            commandBuilder.Append("\t[comments] VARCHAR(100),\n");
            commandBuilder.Append("\t[coordinates] VARCHAR(20),\n");
            commandBuilder.Append("\t[version] BINARY(8)\n);\n");
            commandBuilder.Append("CREATE TABLE ");
            commandBuilder.Append(RULE_TABLE_NAME);
            commandBuilder.Append("\n(\n");
            commandBuilder.Append("\t[id] INT NOT NULL IDENTITY PRIMARY KEY,\n");
            commandBuilder.Append("\t[name] VARCHAR(10) NOT NULL,\n");
            commandBuilder.Append("\t[bias] SMALLINT NOT NULL,\n");
            commandBuilder.Append("\t[start_year] SMALLINT NOT NULL,\n");
            commandBuilder.Append("\t[end_year] SMALLINT NOT NULL,\n");
            commandBuilder.Append("\t[month] TINYINT NOT NULL,\n");
            commandBuilder.Append("\t[date] VARCHAR(7) NOT NULL,\n");
            commandBuilder.Append("\t[time] TIME(0) NOT NULL,\n");
            commandBuilder.Append("\t[time_type] CHAR(1),\n");
            commandBuilder.Append("\t[letter] CHAR(1),\n");
            commandBuilder.Append("\t[version] BINARY(8)\n);\n");
            ExecuteStatement(commandBuilder.ToString());
        }

        public void CreateGeonamesDatabaseTables()
        {
            StringBuilder commandBuilder = new StringBuilder("USE ");
            commandBuilder.Append(_connectionString.InitialCatalog);
            commandBuilder.Append(";\n");
            // Drop table if it exists
            commandBuilder.Append("\nIF OBJECT_ID('");
            commandBuilder.Append(CITY_TABLE_NAME);
            commandBuilder.Append("', 'U') IS NOT NULL\n");
            commandBuilder.Append("\tDROP TABLE ");
            commandBuilder.Append(CITY_TABLE_NAME);
            commandBuilder.Append(";\n");
            commandBuilder.Append("\nIF OBJECT_ID('");
            commandBuilder.Append(FEATURECODE_TABLE_NAME);
            commandBuilder.Append("', 'U') IS NOT NULL\n");
            commandBuilder.Append("\tDROP TABLE ");
            commandBuilder.Append(FEATURECODE_TABLE_NAME);
            commandBuilder.Append(";\n");
            // cities table
            commandBuilder.Append("CREATE TABLE ");
            commandBuilder.Append(CITY_TABLE_NAME);
            commandBuilder.Append("\n(\n");
            commandBuilder.Append("\t[id] INT NOT NULL IDENTITY PRIMARY KEY,\n");
            commandBuilder.Append("\t[name] VARCHAR(200) NOT NULL,\n");
            commandBuilder.Append("\t[ascii_name] VARCHAR(200),\n");
            commandBuilder.Append("\t[alternate_names] VARCHAR(5500),\n");
            commandBuilder.Append("\t[latitude] FLOAT(24),\n");
            commandBuilder.Append("\t[longitude] FLOAT(24),\n");
            commandBuilder.Append("\t[feature_class] CHAR(1),\n");
            commandBuilder.Append("\t[feature_code] VARCHAR(10),\n");
            commandBuilder.Append("\t[country_code] CHAR(2) NOT NULL,\n");
            commandBuilder.Append("\t[country_code2] VARCHAR(60),\n");
            commandBuilder.Append("\t[population] BIGINT,\n");
            commandBuilder.Append("\t[elevation] INT,\n");
            commandBuilder.Append("\t[modification_date] DATETIME NOT NULL,\n");
            commandBuilder.Append("\t[admin1code] VARCHAR(20),\n");
            commandBuilder.Append("\t[admin2code] VARCHAR(80),\n");
            commandBuilder.Append("\t[admin3code] VARCHAR(20),\n");
            commandBuilder.Append("\t[admin4code] VARCHAR(20),\n");
            commandBuilder.Append("\t[gtopo30] INT,\n");
            commandBuilder.Append("\t[timezone_id] INT NOT NULL,\n");
            commandBuilder.Append("\t[timezone_name] VARCHAR(60) NOT NULL,\n");
            commandBuilder.Append("\t[version] BINARY(8)\n);\n");
            // feature_codes table.
            commandBuilder.Append("CREATE TABLE ");
            commandBuilder.Append(FEATURECODE_TABLE_NAME);
            commandBuilder.Append("\n(\n");
            commandBuilder.Append("\t[id] INT NOT NULL IDENTITY PRIMARY KEY,\n");
            commandBuilder.Append("\t[code] VARCHAR(5) NOT NULL,\n");
            commandBuilder.Append("\t[name] VARCHAR(46) NOT NULL,\n");
            commandBuilder.Append("\t[description] VARCHAR(233),\n");
            commandBuilder.Append("\t[version] BINARY(8)\n);\n");

            ExecuteStatement(commandBuilder.ToString());
        }

        /// <summary>
        /// Executes the provided statement on the SqlDatabase determined by this tool's ConnectionString.
        /// </summary>
        /// <param name="query">The string query to execute on the Database.</param>
        private void ExecuteStatement(string statement)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.ToString()))
            {
                SqlCommand command = new SqlCommand(
                    statement,
                    connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
