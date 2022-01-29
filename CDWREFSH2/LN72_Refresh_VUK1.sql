USE CDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.LN72_INT_RTE_HST
--FROM
--	OPENQUERY
--	(
--		LEGEND_TEST_VUK1,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.LN72_INT_RTE_HST
--		'
--	) 

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(LN72.LF_LST_DTS_LN72), '1-1-1900 00:00:00'), 21) FROM LN72_INT_RTE_HST LN72)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.LN72_INT_RTE_HST LN72
	USING
		(
			SELECT
				*
			FROM
				OPENQUERY
				(
					LEGEND_TEST_VUK1,
					''
						SELECT
							LN72.*
						FROM
							PKUB.LN72_INT_RTE_HST LN72
						-- comment WHERE clause for full table refresh
						--WHERE
							--LN72.LF_LST_DTS_LN72 > ''''' + @LastRefresh + '''''
					''
				) 
		) L ON L.BF_SSN = LN72.BF_SSN AND L.LN_SEQ = LN72.LN_SEQ AND L.LC_ITR_TYP = LN72.LC_ITR_TYP AND L.LN_ITR_SEQ = LN72.LN_ITR_SEQ
	WHEN MATCHED THEN 
		UPDATE SET
			LN72.LD_ITR_EFF_BEG = L.LD_ITR_EFF_BEG,
			LN72.LC_ELG_SIN = L.LC_ELG_SIN,
			LN72.LC_STA_LON72 = L.LC_STA_LON72,
			LN72.LD_CRT_LON72 = L.LD_CRT_LON72,
			LN72.LD_ITR_APL = L.LD_ITR_APL,
			LN72.LD_STA_LON72 = L.LD_STA_LON72,
			LN72.LI_SPC_ITR = L.LI_SPC_ITR,
			LN72.LD_ITR_EFF_END = L.LD_ITR_EFF_END,
			LN72.LR_ITR = L.LR_ITR,
			LN72.LF_LST_DTS_LN72 = L.LF_LST_DTS_LN72,
			LN72.LC_ROW_SRC_TP_MNL = L.LC_ROW_SRC_TP_MNL,
			LN72.LC_INT_RDC_PGM = L.LC_INT_RDC_PGM,
			LN72.LR_INT_RDC_PGM_ORG = L.LR_INT_RDC_PGM_ORG,
			LN72.LR_SCRA_CAP = L.LR_SCRA_CAP
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_SEQ,
			LC_ITR_TYP,
			LN_ITR_SEQ,
			LD_ITR_EFF_BEG,
			LC_ELG_SIN,
			LC_STA_LON72,
			LD_CRT_LON72,
			LD_ITR_APL,
			LD_STA_LON72,
			LI_SPC_ITR,
			LD_ITR_EFF_END,
			LR_ITR,
			LF_LST_DTS_LN72,
			LC_ROW_SRC_TP_MNL,
			LC_INT_RDC_PGM,
			LR_INT_RDC_PGM_ORG,
			LR_SCRA_CAP
		)
		VALUES 
		(
			L.BF_SSN,
			L.LN_SEQ,
			L.LC_ITR_TYP,
			L.LN_ITR_SEQ,
			L.LD_ITR_EFF_BEG,
			L.LC_ELG_SIN,
			L.LC_STA_LON72,
			L.LD_CRT_LON72,
			L.LD_ITR_APL,
			L.LD_STA_LON72,
			L.LI_SPC_ITR,
			L.LD_ITR_EFF_END,
			L.LR_ITR,
			L.LF_LST_DTS_LN72,
			L.LC_ROW_SRC_TP_MNL,
			L.LC_INT_RDC_PGM,
			L.LR_INT_RDC_PGM_ORG,
			L.LR_SCRA_CAP
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
		LEGEND_TEST_VUK1,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				PKUB.LN72_INT_RTE_HST LN72
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			LN72_INT_RTE_HST LN72
	) L ON 1 = 1
	
IF @CountDifference != 0
BEGIN
	RAISERROR('LN72_INT_RTE_HST - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
END
