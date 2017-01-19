USE master;

CREATE DATABASE World_Time;

CREATE LOGIN Falven
	WITH PASSWORD = '',
	DEFAULT_DATABASE = World_Time;

CREATE USER Falven;

SELECT * FROM sys.server_principals ORDER BY type_desc DESC;

SELECT * FROM sys.database_principals ORDER BY type_desc DESC;

SELECT * FROM sys.server_principals AS sp JOIN sys.database_principals AS dp ON sp.name = dp.name; 

SELECT * FROM sys.server_permissions;

SELECT * FROM sys.database_permissions;

SELECT @@SERVERNAME;

SELECT Serverproperty('Servername');

SELECT * FROM sys.databases;

SELECT * FROM sys.tables;

DROP DATABASE tempdb;

DROP USER Falven;

DROP LOGIN Falven;

ALTER LOGIN sa ENABLE ;
GO
ALTER LOGIN sa WITH PASSWORD = 'Franramak1!' ;
GO

ALTER LOGIN sa DISABLE;

CREATE LOGIN Falven WITH
	PASSWORD = '',
	DEFAULT_DATABASE = World_Time;
USE World_Time;
CREATE USER Falven
	FOR LOGIN Falven;
GO

USE World_Time;
EXEC sp_addrolemember 'db_owner', 'Falven';
EXEC sp_addrolemember 'db_datareader', 'Falven';
EXEC sp_addrolemember 'db_datawriter', 'Falven';
EXEC sp_addrolemember 'db_backupoperator', 'Falven';

select suser_sname(sid) from sys.database_principals where principal_id = user_id('dbo')
