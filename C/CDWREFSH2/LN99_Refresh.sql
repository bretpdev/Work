USE CDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.LN99_LON_SLE_FAT
--FROM
--	OPENQUERY
--	(
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.LN99_LON_SLE_FAT
--		'
--	) 

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(LN99.LF_LST_DTS_LN99), '1-1-1900 00:00:00'), 21) FROM LN99_LON_SLE_FAT LN99)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.LN99_LON_SLE_FAT LN99
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
							LN99.*
						FROM
							PKUB.LN99_LON_SLE_FAT LN99
					''
				) 
		) L ON L.BF_SSN = LN99.BF_SSN AND L.LN_SEQ = LN99.LN_SEQ AND L.LN_FAT_SEQ = LN99.LN_FAT_SEQ AND L.IF_LON_SLE = LN99.IF_LON_SLE
	WHEN MATCHED THEN 
		UPDATE SET 
			LN99.LF_LST_DTS_LN99 = L.LF_LST_DTS_LN99,
			LN99.IF_SLL_OWN_SLD = L.IF_SLL_OWN_SLD,
			LN99.IF_BUY_OWN_SLD = L.IF_BUY_OWN_SLD,
			LN99.LA_STD_STD_ISL_DCV = L.LA_STD_STD_ISL_DCV
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_SEQ,
			LN_FAT_SEQ,
			IF_LON_SLE,
			LF_LST_DTS_LN99,
			IF_SLL_OWN_SLD,
			IF_BUY_OWN_SLD,
			LA_STD_STD_ISL_DCV
		)
		VALUES 
		(
			L.BF_SSN,
			L.LN_SEQ,
			L.LN_FAT_SEQ,
			L.IF_LON_SLE,
			L.LF_LST_DTS_LN99,
			L.IF_SLL_OWN_SLD,
			L.IF_BUY_OWN_SLD,
			L.LA_STD_STD_ISL_DCV
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
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
				PKUB.LN99_LON_SLE_FAT
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			LN99_LON_SLE_FAT
	) L ON 1 = 1
	
IF @CountDifference != 0
BEGIN
	RAISERROR('LN99_LON_SLE_FAT - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
END