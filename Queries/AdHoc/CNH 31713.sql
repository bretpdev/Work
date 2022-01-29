USE [CDW]
GO
/****** Object:  StoredProcedure [dbo].[LT_TSXXBRPYXP_IDR]    Script Date: X/XX/XXXX X:XX:XX PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[LT_TSXXBRPYXP_IDR]
	@AccountNumber		char(XX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


SELECT DISTINCT
	CONVERT(CHAR(XX),DATEADD(DAY, -XX, RSXX.BD_ANV_QLF_IBR),XXX) AS SoftDeadline,
	PDXX.DM_PRS_X AS FirstName,
	CASE
		WHEN LNXX.LA_RPS_ISL < XX THEN XX
		ELSE LNXX.LA_RPS_ISL
	END AS EstAltPayAmt
FROM 
	PDXX_PRS_NME PDXX
	INNER JOIN RSXX_IBR_RPS RSXX
		ON PDXX.DF_PRS_ID = RSXX.BF_SSN
	INNER JOIN 
		(
			SELECT
				LNXX.BF_SSN,
				SUM(ISNULL(LNXX.LA_RPS_ISL,X)) AS LA_RPS_ISL
			FROM
				LNXX_LON_RPS LNXX
				INNER JOIN [CDW].[dbo].[LNXX_LON_RPS_SPF] LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
			WHERE
				LN_GRD_RPS_SEQ = X
				AND LNXX.LC_STA_LONXX = 'A'
				AND LNXX.LC_TYP_SCH_DIS = 'IX'
			GROUP BY
				LNXX.BF_SSN
		)LNXX ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	
WHERE
	RSXX.BC_STA_RSXX = 'A'
	AND PDXX.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = X 
	BEGIN
		RAISERROR('No data returned.',XX,X)
	END

