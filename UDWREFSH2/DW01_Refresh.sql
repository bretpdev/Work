USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.DW01_DW_CLC_CLU
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.DW01_DW_CLC_CLU DW01
--		'
--	) 

DECLARE @LoopCount TINYINT = 0

RefreshStart:

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.DW01_DW_CLC_CLU DW01
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
							DW01.*
						FROM
							OLWHRM1.DW01_DW_CLC_CLU DW01
					''
				) 
		) D ON DW01.BF_SSN = D.BF_SSN AND DW01.LN_SEQ = D.LN_SEQ
	WHEN MATCHED THEN 
		UPDATE SET
			DW01.WD_CLC_THU = D.WD_CLC_THU,
			DW01.LA_NSI_OTS = D.LA_NSI_OTS,
			DW01.LA_NSI_ACR = D.LA_NSI_ACR,
			DW01.WA_TOT_BRI_OTS = D.WA_TOT_BRI_OTS,
			DW01.WC_DW_LON_STA = D.WC_DW_LON_STA,
			DW01.WD_LON_RPD_SR = D.WD_LON_RPD_SR,
			DW01.WD_XPC_POF_TS26 = D.WD_XPC_POF_TS26,
			DW01.WX_OVR_DW_LON_STA = D.WX_OVR_DW_LON_STA,
			DW01.WA_STD_STD_ISL = D.WA_STD_STD_ISL,
			DW01.WC_LON_STA_GRC = D.WC_LON_STA_GRC,
			DW01.WC_LON_STA_SCL = D.WC_LON_STA_SCL,
			DW01.WC_LON_STA_RPY = D.WC_LON_STA_RPY,
			DW01.WC_LON_STA_DFR = D.WC_LON_STA_DFR,
			DW01.WC_LON_STA_FOR = D.WC_LON_STA_FOR,
			DW01.WC_LON_STA_CUR = D.WC_LON_STA_CUR,
			DW01.WC_LON_STA_CLM = D.WC_LON_STA_CLM,
			DW01.WC_LON_STA_PCL = D.WC_LON_STA_PCL,
			DW01.WC_LON_STA_DTH = D.WC_LON_STA_DTH,
			DW01.WC_LON_STA_DSA = D.WC_LON_STA_DSA,
			DW01.WC_LON_STA_BKR = D.WC_LON_STA_BKR,
			DW01.WC_LON_STA_PIF = D.WC_LON_STA_PIF,
			DW01.WC_LON_STA_FUL_ORG = D.WC_LON_STA_FUL_ORG,
			DW01.WC_LON_DFR_FOR_TYP = D.WC_LON_DFR_FOR_TYP
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_SEQ,
			WD_CLC_THU,
			LA_NSI_OTS,
			LA_NSI_ACR,
			WA_TOT_BRI_OTS,
			WC_DW_LON_STA,
			WD_LON_RPD_SR,
			WD_XPC_POF_TS26,
			WX_OVR_DW_LON_STA,
			WA_STD_STD_ISL,
			WC_LON_STA_GRC,
			WC_LON_STA_SCL,
			WC_LON_STA_RPY,
			WC_LON_STA_DFR,
			WC_LON_STA_FOR,
			WC_LON_STA_CUR,
			WC_LON_STA_CLM,
			WC_LON_STA_PCL,
			WC_LON_STA_DTH,
			WC_LON_STA_DSA,
			WC_LON_STA_BKR,
			WC_LON_STA_PIF,
			WC_LON_STA_FUL_ORG,
			WC_LON_DFR_FOR_TYP
		)
		VALUES 
		(
			D.BF_SSN,
			D.LN_SEQ,
			D.WD_CLC_THU,
			D.LA_NSI_OTS,
			D.LA_NSI_ACR,
			D.WA_TOT_BRI_OTS,
			D.WC_DW_LON_STA,
			D.WD_LON_RPD_SR,
			D.WD_XPC_POF_TS26,
			D.WX_OVR_DW_LON_STA,
			D.WA_STD_STD_ISL,
			D.WC_LON_STA_GRC,
			D.WC_LON_STA_SCL,
			D.WC_LON_STA_RPY,
			D.WC_LON_STA_DFR,
			D.WC_LON_STA_FOR,
			D.WC_LON_STA_CUR,
			D.WC_LON_STA_CLM,
			D.WC_LON_STA_PCL,
			D.WC_LON_STA_DTH,
			D.WC_LON_STA_DSA,
			D.WC_LON_STA_BKR,
			D.WC_LON_STA_PIF,
			D.WC_LON_STA_FUL_ORG,
			D.WC_LON_DFR_FOR_TYP
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
		DUSTER,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				OLWHRM1.DW01_DW_CLC_CLU DW01
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			UDW..DW01_DW_CLC_CLU DW01
	) L ON 1 = 1

IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('DW01_DW_CLC_CLU - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
	END
ELSE IF @CountDifference != 0 AND @LoopCount = 0
	BEGIN

		SET @LoopCount = @LoopCount + 1
		
		-- Local and remote record counts did not match.  Run refresh again.
		GOTO RefreshStart;
	END
