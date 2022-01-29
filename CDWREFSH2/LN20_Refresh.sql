USE CDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.LN20_EDS
--FROM
--	OPENQUERY
--	(
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.LN20_EDS
--		'
--	) 

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
					LEGEND,
					''
						SELECT
							LN20.*
						FROM
							PKUB.LN20_EDS LN20
						-- comment WHERE clause for full table refresh
						WHERE
							LN20.LF_LST_DTS_LN20 > ''''' + @LastRefresh + '''''
					''
				) 
		) L ON L.BF_SSN = LN20.BF_SSN AND L.LN_EDS_SEQ = LN20.LN_EDS_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			LN20.LC_STA_LON20 = L.LC_STA_LON20,
			LN20.LC_REL_TO_BR = L.LC_REL_TO_BR,
			LN20.LC_EDS_TYP = L.LC_EDS_TYP,
			LN20.LF_EDS = L.LF_EDS,
			LN20.LF_LST_DTS_LN20 = L.LF_LST_DTS_LN20,
			LN20.LN_SEQ = L.LN_SEQ,
			LN20.AN_SEQ = L.AN_SEQ,
			LN20.IC_LON_PGM = L.IC_LON_PGM,
			LN20.AN_SEQ_WK79 = L.AN_SEQ_WK79,
			LN20.LD_EFF_EDS_HST = L.LD_EFF_EDS_HST,
			LN20.LC_REA_EDS_HST = L.LC_REA_EDS_HST,
			LN20.LF_LST_USR_HST_EDS = L.LF_LST_USR_HST_EDS,
			LN20.AF_APL_ID = L.AF_APL_ID,
			LN20.AN_LC_APL_SEQ = L.AN_LC_APL_SEQ
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
			L.BF_SSN,
			L.LN_EDS_SEQ,
			L.LC_STA_LON20,
			L.LC_REL_TO_BR,
			L.LC_EDS_TYP,
			L.LF_EDS,
			L.LF_LST_DTS_LN20,
			L.LN_SEQ,
			L.AN_SEQ,
			L.IC_LON_PGM,
			L.AN_SEQ_WK79,
			L.LD_EFF_EDS_HST,
			L.LC_REA_EDS_HST,
			L.LF_LST_USR_HST_EDS,
			L.AF_APL_ID,
			L.AN_LC_APL_SEQ
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	--    DELETE
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
		LEGEND,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				PKUB.LN20_EDS
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