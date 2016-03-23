USE World_Time;

SELECT * FROM cities c
JOIN feature_codes fc ON c.feature_code = fc.code
JOIN timezones t ON c.timezone_id = t.id
JOIN rules r ON t.rule_name = r.name
WHERE c.name = 'Fort Collins';

SELECT * FROM cities c
JOIN feature_codes fc ON c.feature_code = fc.code
JOIN timezones t ON c.timezone_id = t.id
JOIN rules r ON t.rule_name = r.name
WHERE c.name = 'Tallahassee';

SELECT * FROM cities c
JOIN feature_codes fc ON c.feature_code = fc.code
JOIN timezones t ON c.timezone_id = t.id
JOIN rules r ON t.rule_name = r.name
WHERE c.name = 'Seattle';

SELECT * FROM cities c
JOIN feature_codes fc ON c.feature_code = fc.code
JOIN timezones t ON c.timezone_id = t.id
JOIN rules r ON t.rule_name = r.name
WHERE c.name = 'Fort Lauderdale';

SELECT * FROM cities c
JOIN feature_codes fc ON c.feature_code = fc.code
JOIN timezones t ON c.timezone_id = t.id
JOIN rules r ON t.rule_name = r.name
WHERE c.name = 'Key Largo';

SELECT * FROM cities c
JOIN feature_codes fc ON c.feature_code = fc.code
JOIN timezones t ON c.timezone_id = t.id
JOIN rules r ON t.rule_name = r.name
WHERE c.name = 'Hong Kong';

SELECT COUNT(*) FROM cities;
SELECT COUNT(*) FROM timezones;
SELECT COUNT(*) FROM rules;
SELECT COUNT(*) FROM feature_codes;