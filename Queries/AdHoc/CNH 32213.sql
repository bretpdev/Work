USE [CDW]
GO
/****** Object:  StoredProcedure [dbo].[LT_TSXXRPYEXX_Deadline]    Script Date: X/XX/XXXX X:XX:XX AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[LT_TSXXRPYEXX_Deadline]
	@AccountNumber		char(XX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	
SELECT DISTINCT
	CONVERT(CHAR(XX),DATEADD(DAY, -XX, RSXX.BD_ANV_QLF_IBR),XXX) AS HardDeadline,
	PDXX.DM_PRS_X AS FirstName,
	LNXX.LA_RPS_ISL AS EstAltPayAmt
FROM 
	PDXX_PRS_NME PDXX
	INNER JOIN RSXX_IBR_RPS RSXX
		ON PDXX.DF_PRS_ID = RSXX.BF_SSN
	INNER JOIN LNXX_LON_RPS LNXX 
		ON LNXX.BF_SSN = RSXX.BF_SSN
		AND LNXX.LC_STA_LONXX = 'A'
	INNER JOIN 
		(
			SELECT
				BF_SSN,
				LN_SEQ,
				LN_RPS_SEQ,
				SUM(ISNULL(LA_RPS_ISL,X)) AS LA_RPS_ISL
			FROM
				[CDW].[dbo].[LNXX_LON_RPS_SPF]
			WHERE
				LN_GRD_RPS_SEQ = X
			GROUP BY
				BF_SSN,
				LN_SEQ,
				LN_RPS_SEQ
		)LNXX ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
WHERE 
	RSXX.BC_STA_RSXX = 'A' 
	AND (LNXX.LC_TYP_SCH_DIS LIKE '%IX%')
	AND PDXX.DF_SPE_ACC_ID = @AccountNumber
	
END

IF @@ROWCOUNT = X 
	BEGIN
		RAISERROR('No data returned.',XX,X)
	END
