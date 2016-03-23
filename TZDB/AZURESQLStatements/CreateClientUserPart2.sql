USE World_Time;

--CREATE USER for this database.
CREATE USER Timekeeper
	FOR LOGIN Timekeeper;
GO
--

-- GRANT Proviledges
GRANT SELECT ON cities TO Timekeeper;
GRANT SELECT ON feature_codes TO Timekeeper;
GRANT SELECT ON timezones TO Timekeeper;
GRANT SELECT ON rules TO Timekeeper;
GO
--