CREATE PROCEDURE [dbo].[LT_TS06BIDRPS_Fields]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


DECLARE @SQLStatement VARCHAR(MAX) =
	'
	SELECT
		CASE
			WHEN RD.LC_TYP_SCH_DIS IN(''CA'',''CP'') THEN ''Pay As You Earn Plan''
			WHEN RD.LC_TYP_SCH_DIS IN(''CQ'',''C1'',''C2'',''C3'') THEN ''Income Contingent Plan''
			WHEN RD.LC_TYP_SCH_DIS IN(''IB'',''IL'',''IP'',''I3'') THEN ''Income Based Plan''
			WHEN RD.LC_TYP_SCH_DIS IN(''I5'') THEN ''Revised Pay As You Earn Plan''
			ELSE ''See attached loan sheet.''
		END AS IDRScheduleType,
		CASE
			WHEN RD.LC_TYP_SCH_DIS IN(''CP'',''CQ'',''IL'',''IP'') THEN ''Permanent Standard''
			WHEN RD.LC_TYP_SCH_DIS IN(''IA'') THEN ''REPAYE Alternative Repayment''
			ELSE ''See attached loan sheet.''
		END AS AltFormatOrPermStnd,
		''$'' + CONVERT(VARCHAR, SUM(RD.LA_RPS_ISL),1) + '''' AS MonPmtAmnt
	FROM 
		OPENQUERY
		(	LEGEND,
			''SELECT
				ROW_NUMBER() OVER (PARTITION BY LN66.BF_SSN, LN66.LN_SEQ, LN66.LN_RPS_SEQ ORDER BY LN66.LN_GRD_RPS_SEQ) AS SEQ,
				LN10.BF_SSN,
				LN10.LN_SEQ,
				LN66.LN_GRD_RPS_SEQ,
				LN10.IC_LON_PGM,
				LN10.LD_LON_1_DSB,
				LN10.LA_LON_AMT_GTR,
				LN10.LA_CUR_PRI,
				LN72.LR_ITR,
				LN65.LC_TYP_SCH_DIS,
				LN65.LA_TOT_RPD_DIS,
				LN65.LA_ANT_CAP,
				RS10.LD_RPS_1_PAY_DU,
				LN66.LN_RPS_TRM,
				LN66.LA_RPS_ISL
			FROM
				PKUB.PD10_PRS_NME PD10
				INNER JOIN PKUB.LN10_LON LN10
					ON PD10.DF_PRS_ID = LN10.BF_SSN
				INNER JOIN
				(
					SELECT
						LN72.BF_SSN,
						LN72.LN_SEQ,
						LN72.LR_ITR,
						ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
					FROM	
						PKUB.LN72_INT_RTE_HST LN72
						INNER JOIN PKUB.PD10_PRS_NME PD10 ON PD10.DF_PRS_ID = LN72.BF_SSN
					WHERE
						LN72.LC_STA_LON72 = ''''A''''
						AND
						PD10.DF_SPE_ACC_ID = ''''' + @AccountNumber + '''''
				) LN72 ON LN10.BF_SSN = LN72.BF_SSN
						AND LN10.LN_SEQ = LN72.LN_SEQ
						AND LN72.SEQ = 1
				INNER JOIN PKUB.LN65_LON_RPS LN65 
					ON LN65.BF_SSN = LN10.BF_SSN
					AND LN65.LN_SEQ = LN10.LN_SEQ
				INNER JOIN PKUB.LN66_LON_RPS_SPF LN66
					ON LN66.BF_SSN = LN65.BF_SSN
					AND LN66.LN_SEQ = LN65.LN_SEQ
					AND LN66.LN_RPS_SEQ = LN65.LN_RPS_SEQ
				INNER JOIN PKUB.RS10_BR_RPD RS10
					ON RS10.BF_SSN = LN65.BF_SSN
					AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ
			WHERE 
				PD10.DF_SPE_ACC_ID = ''''' + @AccountNumber + '''''
			AND LN65.LC_STA_LON65 = ''''A''''
		''
		) RD  
		INNER JOIN CDW..FormatTranslation FMT ON RD.IC_LON_PGM = FMT.Start
	GROUP BY 
		RD.BF_SSN,
		CASE
			WHEN RD.LC_TYP_SCH_DIS IN(''CA'',''CP'') THEN ''Pay As You Earn Plan''
			WHEN RD.LC_TYP_SCH_DIS IN(''CQ'',''C1'',''C2'',''C3'') THEN ''Income Contingent Plan''
			WHEN RD.LC_TYP_SCH_DIS IN(''IB'',''IL'',''IP'',''I3'') THEN ''Income Based Plan''
			WHEN RD.LC_TYP_SCH_DIS IN(''I5'') THEN ''Revised Pay As You Earn Plan''
			ELSE ''See attached loan sheet.''
		END,
		CASE
			WHEN RD.LC_TYP_SCH_DIS IN(''CP'',''CQ'',''IL'',''IP'') THEN ''Permanent Standard''
			WHEN RD.LC_TYP_SCH_DIS IN(''IA'') THEN ''REPAYE Alternative Repayment''
			ELSE ''See attached loan sheet.''
		END
	'
 
-- execuate the dynamically created sql statement (inserting results into temp table)
EXEC (@SQLStatement)

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('[dbo].[LT_TS06BIDRPS_Fields] - No data returned for AccountNumber %s',11,2, @AccountNumber)
	END

END