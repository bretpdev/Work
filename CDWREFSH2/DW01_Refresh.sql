USE CDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.DW01_DW_CLC_CLU
--FROM
--	OPENQUERY
--	(
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.DW01_DW_CLC_CLU DW01
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
					LEGEND,
					''
						SELECT
							DW01.*
						FROM
							PKUB.DW01_DW_CLC_CLU DW01
					''
				) 
		) L ON DW01.BF_SSN = L.BF_SSN AND DW01.LN_SEQ = L.LN_SEQ
	WHEN MATCHED THEN 
		UPDATE SET
			DW01.WD_CLC_THU = L.WD_CLC_THU,
			DW01.LA_NSI_OTS = L.LA_NSI_OTS,
			DW01.LA_NSI_ACR = L.LA_NSI_ACR,
			DW01.WA_TOT_BRI_OTS = L.WA_TOT_BRI_OTS,
			DW01.WC_DW_LON_STA = L.WC_DW_LON_STA,
			DW01.WD_LON_RPD_SR = L.WD_LON_RPD_SR,
			DW01.WD_XPC_POF_TS26 = L.WD_XPC_POF_TS26,
			DW01.WX_OVR_DW_LON_STA = L.WX_OVR_DW_LON_STA,
			DW01.WA_STD_STD_ISL = L.WA_STD_STD_ISL,
			DW01.WC_LON_STA_GRC = L.WC_LON_STA_GRC,
			DW01.WC_LON_STA_SCL = L.WC_LON_STA_SCL,
			DW01.WC_LON_STA_RPY = L.WC_LON_STA_RPY,
			DW01.WC_LON_STA_DFR = L.WC_LON_STA_DFR,
			DW01.WC_LON_STA_FOR = L.WC_LON_STA_FOR,
			DW01.WC_LON_STA_CUR = L.WC_LON_STA_CUR,
			DW01.WC_LON_STA_CLM = L.WC_LON_STA_CLM,
			DW01.WC_LON_STA_PCL = L.WC_LON_STA_PCL,
			DW01.WC_LON_STA_DTH = L.WC_LON_STA_DTH,
			DW01.WC_LON_STA_DSA = L.WC_LON_STA_DSA,
			DW01.WC_LON_STA_BKR = L.WC_LON_STA_BKR,
			DW01.WC_LON_STA_PIF = L.WC_LON_STA_PIF,
			DW01.WC_LON_STA_FUL_ORG = L.WC_LON_STA_FUL_ORG,
			DW01.WC_LON_DFR_FOR_TYP = L.WC_LON_DFR_FOR_TYP
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
			L.BF_SSN,
			L.LN_SEQ,
			L.WD_CLC_THU,
			L.LA_NSI_OTS,
			L.LA_NSI_ACR,
			L.WA_TOT_BRI_OTS,
			L.WC_DW_LON_STA,
			L.WD_LON_RPD_SR,
			L.WD_XPC_POF_TS26,
			L.WX_OVR_DW_LON_STA,
			L.WA_STD_STD_ISL,
			L.WC_LON_STA_GRC,
			L.WC_LON_STA_SCL,
			L.WC_LON_STA_RPY,
			L.WC_LON_STA_DFR,
			L.WC_LON_STA_FOR,
			L.WC_LON_STA_CUR,
			L.WC_LON_STA_CLM,
			L.WC_LON_STA_PCL,
			L.WC_LON_STA_DTH,
			L.WC_LON_STA_DSA,
			L.WC_LON_STA_BKR,
			L.WC_LON_STA_PIF,
			L.WC_LON_STA_FUL_ORG,
			L.WC_LON_DFR_FOR_TYP
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
				PKUB.DW01_DW_CLC_CLU DW01
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			CDW..DW01_DW_CLC_CLU DW01
	) L ON 1 = 1

IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('DW01_DW_CLC_CLU - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is requireL.', 16, 11, @CountDifference)
	END
ELSE IF @CountDifference != 0 AND @LoopCount = 0
	BEGIN

		SET @LoopCount = @LoopCount + 1
		
		-- Local and remote record counts did not match.  Run refresh again.
		GOTO RefreshStart;
	END