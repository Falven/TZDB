USE World_Time

IF OBJECT_ID('timezones', 'U') IS NOT NULL
	DROP TABLE timezones;
IF OBJECT_ID('rules', 'U') IS NOT NULL
	DROP TABLE rules;
IF OBJECT_ID('leaps', 'U') IS NOT NULL
	DROP TABLE leaps;
IF OBJECT_ID('cities', 'U') IS NOT NULL
	DROP TABLE cities;
IF OBJECT_ID('timezone_rule', 'U') IS NOT NULL
	DROP TABLE timezone_rule;
            
CREATE TABLE timezones
(
	[id] INT NOT NULL UNIQUE PRIMARY KEY,
	[name] VARCHAR(30) NOT NULL,
	[bias] SMALLINT NOT NULL,
	[rule_name] VARCHAR(10),
	[tz_abreviation] VARCHAR(7) NOT NULL,
	[country_code] CHAR(2) NOT NULL,
	[country_name] VARCHAR(42) NOT NULL,
	[comments] VARCHAR(100),
	[coordinates] VARCHAR(20),
	[version] ROWVERSION
);

CREATE TABLE rules
(
	[id] INT NOT NULL UNIQUE IDENTITY PRIMARY KEY,
	[name] VARCHAR(10) NOT NULL,
	[bias] SMALLINT NOT NULL,
	[start_year] SMALLINT NOT NULL,
	[end_year] SMALLINT NOT NULL,
	[month] TINYINT NOT NULL,
	[date] VARCHAR(7) NOT NULL,
	[time] TIME(0) NOT NULL,
	[time_type] CHAR(1),
	[letter] CHAR(1),
	[version] ROWVERSION
);

CREATE TABLE leaps
(
	[id] INT NOT NULL UNIQUE IDENTITY PRIMARY KEY,
	[year] SMALLINT NOT NULL,
	[month] VARCHAR(3),
	[day] SMALLINT NOT NULL,
	[time] TIME(0) NOT NULL,
	[correction] VARCHAR(1) NOT NULL,
	[rs] VARCHAR(1) NOT NULL,
	[version] ROWVERSION
);

CREATE TABLE cities
(
	[id] INT NOT NULL UNIQUE IDENTITY PRIMARY KEY,
	[name] VARCHAR(200) NOT NULL,
	[ascii_name] VARCHAR(200),
	[alternate_names] VARCHAR(5000),
	[latitude] FLOAT(24),
	[longitude] FLOAT(24),
	[feature_class] CHAR(1),
	[feature_code] VARCHAR(10),
	[country_code] CHAR(2) NOT NULL,
	[country_code2] VARCHAR(60),
	[population] BIGINT,
	[elevation] INT,
	[modification_date] DATETIME NOT NULL,
	[admin1code] VARCHAR(20),
	[admin2code] VARCHAR(80),
	[admin3code] VARCHAR(20),
	[admin4code] VARCHAR(20),
	[gtopo30] INT,
	[timezone_id] INT NOT NULL,
	[timezone_name] VARCHAR(60) NOT NULL,
	[version] ROWVERSION
);