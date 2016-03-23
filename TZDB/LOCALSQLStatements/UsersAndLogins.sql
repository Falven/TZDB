USE master;

-- How to create the database
CREATE DATABASE World_Time;
--

-- viewing the different logins (server principals) and users (database principals).
SELECT * FROM sys.server_principals ORDER BY type_desc DESC;
SELECT * FROM sys.database_principals ORDER BY type_desc DESC;
SELECT * FROM sys.server_principals AS sp
JOIN sys.database_principals AS dp
ON sp.name = dp.name; 
--

-- Viewing the different permissions for logins (server) and users (database).
SELECT * FROM sys.server_permissions;
SELECT * FROM sys.database_permissions;
--

-- How to get the servername
SELECT @@SERVERNAME;
SELECT Serverproperty('Servername');
--

-- How to view all of the databases and tables in a certain database (USE DATABASE).
SELECT * FROM sys.databases;
SELECT * FROM sys.tables;
--

-- How to delete login and user.
DROP USER Falven;
DROP LOGIN Falven;
--

-- Trick to allow sql authentication?
ALTER LOGIN sa ENABLE ;
GO
ALTER LOGIN sa WITH PASSWORD = 'Franramak1!';
GO
ALTER LOGIN sa DISABLE;
GO
--

-- Creating Falven login for general purpose sql authenticated admin loggin.
USE World_Time;
CREATE LOGIN Falven WITH
	PASSWORD = 'Franramak1!',
	DEFAULT_DATABASE = World_Time;
GO
CREATE USER Falven
	FOR LOGIN Falven;
GO
EXEC sp_addrolemember 'db_owner', 'Falven';
EXEC sp_addrolemember 'db_datareader', 'Falven';
EXEC sp_addrolemember 'db_datawriter', 'Falven';
EXEC sp_addrolemember 'db_backupoperator', 'Falven';
GO
select suser_sname(sid) from sys.database_principals where principal_id = user_id('dbo');
GO
--

--Create Client User with only select priviledges.
USE World_Time;
CREATE LOGIN Timekeeper
	WITH PASSWORD = 'keeperofthetime',
	DEFAULT_DATABASE = World_Time;
GO
CREATE USER Timekeeper
	FOR LOGIN Timekeeper;
GO
USE World_Time;
GRANT SELECT ON cities TO Timekeeper;
GRANT SELECT ON feature_codes TO Timekeeper;
GRANT SELECT ON timezones TO Timekeeper;
GRANT SELECT ON rules TO Timekeeper;
GO
--