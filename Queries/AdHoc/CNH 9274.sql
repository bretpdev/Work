--DECLARE @Schools TABLE
--(
--	School VARCHAR(X)
--)

--INSERT INTO 
--	@Schools
--VALUES
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXXX'),
--	('XXXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXXX'),
--	('XXXXXX'),
--	('XXXXXXX'),
--	('XXXXXXX'),
--	('XXXXXX'),
--	('XXXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXXX'),
--	('XXXXXX'),
--	('XXXXXXX'),
--	('XXXXXX'),
--	('XXXXXXX'),
--	('XXXXXXX'),
--	('XXXXXXX'),
--	('XXXXXXX'),
--	('XXXXXXX'),
--	('XXXXXXX'),
--	('XXXXXX'),
--	('XXXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXX'),
--	('XXXXXXXX'),
--	('XXXXXXXX'),
--	('XXXXXXX'),
--	('XXXXXXX'),
--	('XXXXXXXX')


--UPDATE
--	@Schools
--SET
--	School = RIGHT('XXXXXXXX' + School, X)
	
--SELECT	
--	'''''' + School + ''''','
--FROM
--	@Schools

SELECT TOP X 
	*
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				*
			FROM
				PKUB.LNXX_LON
		'
	)
	
SELECT
	*
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT DISTINCT
				LNXX.BF_SSN,
				LNXX.LN_SEQ,
				LNXX.LF_DOE_SCL_ORG
			FROM
				PKUB.LNXX_LON LNXX
			WHERE
				LNXX.LF_DOE_SCL_ORG IN
				(
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX'',
					''XXXXXXXX''
				)
		'
	)