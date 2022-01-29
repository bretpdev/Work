USE CDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.WK1J_LON_SLE_PLR
--FROM
--	OPENQUERY
--	(
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.WK1J_LON_SLE_PLR
--		'
--	) 


DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.WK1J_LON_SLE_PLR WK1J
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
							WK1J.*
						FROM
							PKUB.WK1J_LON_SLE_PLR WK1J
					''
				) 
		) L ON L.IF_LON_SLE = WK1J.IF_LON_SLE AND L.ID_LON_SLE_LST_PLR = WK1J.ID_LON_SLE_LST_PLR AND L.IT_SLE_LST_PLR = WK1J.IT_SLE_LST_PLR AND L.BF_SSN = WK1J.BF_SSN AND L.LN_SEQ = WK1J.LN_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			WK1J.LA_CUR_PRI = L.LA_CUR_PRI,
			WK1J.LA_NSI_OTS = L.LA_NSI_OTS,
			WK1J.LA_LTE_FEE_OTS = L.LA_LTE_FEE_OTS
	WHEN NOT MATCHED THEN
		INSERT 
		(
			IF_LON_SLE,
			ID_LON_SLE_LST_PLR,
			IT_SLE_LST_PLR,
			BF_SSN,
			LN_SEQ,
			LA_CUR_PRI,
			LA_NSI_OTS,
			LA_LTE_FEE_OTS
		)
		VALUES 
		(
			L.IF_LON_SLE,
			L.ID_LON_SLE_LST_PLR,
			L.IT_SLE_LST_PLR,
			L.BF_SSN,
			L.LN_SEQ,
			L.LA_CUR_PRI,
			L.LA_NSI_OTS,
			L.LA_LTE_FEE_OTS
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
				PKUB.WK1J_LON_SLE_PLR
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			WK1J_LON_SLE_PLR
	) L ON 1 = 1
	
IF @CountDifference != 0
BEGIN
	RAISERROR('WK1J_LON_SLE_PLR - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
END