USE CDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.BL10_BR_BIL
--FROM
--	OPENQUERY
--	(
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.BL10_BR_BIL
--		'
--	)

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(BL10.LF_LST_DTS_BL10), '1-1-1900 00:00:00'), 21) FROM BL10_BR_BIL BL10)
PRINT 'Last Refreshed at: ' + @LastRefresh


DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.BL10_BR_BIL BL10
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
							*
						FROM
							PKUB.BL10_BR_BIL BL10
						-- comment WHERE clause for full table refresh
						WHERE
							BL10.LF_LST_DTS_BL10 > ''''' + @LastRefresh + '''''
					''
				) 
		) L ON L.BF_SSN = BL10.BF_SSN AND L.LD_BIL_CRT = BL10.LD_BIL_CRT AND L.LN_SEQ_BIL_WI_DTE = BL10.LN_SEQ_BIL_WI_DTE
	WHEN MATCHED THEN 
		UPDATE SET 
			BL10.LN_BIL_SRL = L.LN_BIL_SRL,
			BL10.LC_BIL_MTD = L.LC_BIL_MTD,
			BL10.LC_BIL_TYP = L.LC_BIL_TYP,
			BL10.LD_RBL_LST = L.LD_RBL_LST,
			BL10.LC_IND_BIL_SNT = L.LC_IND_BIL_SNT,
			BL10.LD_BIL_DU = L.LD_BIL_DU,
			BL10.LD_STA_BIL10 = L.LD_STA_BIL10,
			BL10.LC_STA_BIL10 = L.LC_STA_BIL10,
			BL10.LF_LST_DTS_BL10 = L.LF_LST_DTS_BL10,
			BL10.LC_EFT_EXT = L.LC_EFT_EXT,
			BL10.BN_EFT_SEQ = L.BN_EFT_SEQ,
			BL10.LC_BR_BIL_SNT_REA = L.LC_BR_BIL_SNT_REA,
			BL10.LA_INT_PD_LST_STM = L.LA_INT_PD_LST_STM,
			BL10.LA_FEE_PD_LST_STM = L.LA_FEE_PD_LST_STM,
			BL10.LA_PRI_PD_LST_STM = L.LA_PRI_PD_LST_STM,
			BL10.LD_LST_PAY_LST_STM = L.LD_LST_PAY_LST_STM,
			BL10.LC_PCV_BIL = L.LC_PCV_BIL
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LD_BIL_CRT,
			LN_SEQ_BIL_WI_DTE,
			LN_BIL_SRL,
			LC_BIL_MTD,
			LC_BIL_TYP,
			LD_RBL_LST,
			LC_IND_BIL_SNT,
			LD_BIL_DU,
			LD_STA_BIL10,
			LC_STA_BIL10,
			LF_LST_DTS_BL10,
			LC_EFT_EXT,
			BN_EFT_SEQ,
			LC_BR_BIL_SNT_REA,
			LA_INT_PD_LST_STM,
			LA_FEE_PD_LST_STM,
			LA_PRI_PD_LST_STM,
			LD_LST_PAY_LST_STM,
			LC_PCV_BIL
		)
		VALUES 
		(
			L.BF_SSN,
			L.LD_BIL_CRT,
			L.LN_SEQ_BIL_WI_DTE,
			L.LN_BIL_SRL,
			L.LC_BIL_MTD,
			L.LC_BIL_TYP,
			L.LD_RBL_LST,
			L.LC_IND_BIL_SNT,
			L.LD_BIL_DU,
			L.LD_STA_BIL10,
			L.LC_STA_BIL10,
			L.LF_LST_DTS_BL10,
			L.LC_EFT_EXT,
			L.BN_EFT_SEQ,
			L.LC_BR_BIL_SNT_REA,
			L.LA_INT_PD_LST_STM,
			L.LA_FEE_PD_LST_STM,
			L.LA_PRI_PD_LST_STM,
			L.LD_LST_PAY_LST_STM,
			L.LC_PCV_BIL
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
				PKUB.BL10_BR_BIL
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			BL10_BR_BIL
	) L ON 1 = 1
	
IF @CountDifference != 0
BEGIN
	RAISERROR('BL10_BR_BIL - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
END