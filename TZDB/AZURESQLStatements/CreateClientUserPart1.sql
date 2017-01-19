--Create Client User with only select priviledges.
USE master;
CREATE LOGIN Timekeeper
	WITH PASSWORD = '';
	--DEFAULT_DATABASE = World_Time; NOT SUPPORTED IN AZURE
GO
--
