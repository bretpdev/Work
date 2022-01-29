USE UDW
GO

SELECT TOP 1
	*
INTO 
	dbo.LN20_EDS
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				*
			FROM
				OLWHRM1.LN20_EDS
		'
	) 

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(LN20.LF_LST_DTS_LN20), '1-1-1900 00:00:00'), 21) FROM LN20_EDS LN20)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.LN20_EDS LN20
	USING
		(
			SELECT
				*
			FROM
				OPENQUERY
				(
					DUSTER,
					''
						SELECT
							LN20.*
						FROM
							OLWHRM1.LN20_EDS LN20
						-- comment WHERE clause for full table refresh
						WHERE
							LN20.LF_LST_DTS_LN20 > ''''' + @LastRefresh + '''''
					''
				) 
		) D ON D.BF_SSN = LN20.BF_SSN AND D.LN_EDS_SEQ = LN20.LN_EDS_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			LN20.LC_STA_LON20 = D.LC_STA_LON20,
			LN20.LC_REL_TO_BR = D.LC_REL_TO_BR,
			LN20.LC_EDS_TYP = D.LC_EDS_TYP,
			LN20.LF_EDS = D.LF_EDS,
			LN20.LF_LST_DTS_LN20 = D.LF_LST_DTS_LN20,
			LN20.LN_SEQ = D.LN_SEQ,
			LN20.AN_SEQ = D.AN_SEQ,
			LN20.IC_LON_PGM = D.IC_LON_PGM,
			LN20.AN_SEQ_WK79 = D.AN_SEQ_WK79,
			LN20.LD_EFF_EDS_HST = D.LD_EFF_EDS_HST,
			LN20.LC_REA_EDS_HST = D.LC_REA_EDS_HST,
			LN20.LF_LST_USR_HST_EDS = D.LF_LST_USR_HST_EDS,
			LN20.AF_APL_ID = D.AF_APL_ID,
			LN20.AN_LC_APL_SEQ = D.AN_LC_APL_SEQ
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_EDS_SEQ,
			LC_STA_LON20,
			LC_REL_TO_BR,
			LC_EDS_TYP,
			LF_EDS,
			LF_LST_DTS_LN20,
			LN_SEQ,
			AN_SEQ,
			IC_LON_PGM,
			AN_SEQ_WK79,
			LD_EFF_EDS_HST,
			LC_REA_EDS_HST,
			LF_LST_USR_HST_EDS,
			AF_APL_ID,
			AN_LC_APL_SEQ
		)
		VALUES 
		(
			D.BF_SSN,
			D.LN_EDS_SEQ,
			D.LC_STA_LON20,
			D.LC_REL_TO_BR,
			D.LC_EDS_TYP,
			D.LF_EDS,
			D.LF_LST_DTS_LN20,
			D.LN_SEQ,
			D.AN_SEQ,
			D.IC_LON_PGM,
			D.AN_SEQ_WK79,
			D.LD_EFF_EDS_HST,
			D.LC_REA_EDS_HST,
			D.LF_LST_USR_HST_EDS,
			D.AF_APL_ID,
			D.AN_LC_APL_SEQ
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'

PRINT @SQLStatement
EXEC (@SQLStatement)

-- ###### VALIDATION
DECLARE 
	@CountDifference INT

SELECT
	@CountDifference = L.LocalCount - R.RemoteCount
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				OLWHRM1.LN20_EDS
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			LN20_EDS
	) L ON 1 = 1
	
IF @CountDifference != 0
BEGIN
	RAISERROR('LN20_EDS - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
END
