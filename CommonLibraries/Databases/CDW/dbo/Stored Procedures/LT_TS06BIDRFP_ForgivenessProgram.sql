CREATE PROCEDURE [dbo].[LT_TS06BIDRFP_ForgivenessProgram]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT
		CASE
			WHEN LN65.LC_TYP_SCH_DIS IN('CA','CP') THEN 'Pay As You Earn Plan'
			WHEN LN65.LC_TYP_SCH_DIS IN('CQ','C1','C2','C3') THEN 'Income Contingent Plan'
			WHEN LN65.LC_TYP_SCH_DIS IN('IB','IL','IP','I3') THEN 'Income Based Plan'
			WHEN LN65.LC_TYP_SCH_DIS IN('I5') THEN 'Revised Pay As You Earn Plan'
			ELSE 'See attached loan sheet.'
		END AS LoanForgivenessProgramName
	FROM 
		CDW..PD10_Borrower PD10
		INNER JOIN CDW..LN10_LON LN10
			ON LN10.BF_SSN = PD10.BF_SSN
		INNER JOIN CDW..LN65_LON_RPS LN65
			ON LN65.BF_SSN = LN10.BF_SSN
			AND LN65.LN_SEQ = LN10.LN_SEQ
			AND LN65.LC_STA_LON65 = 'A'
	WHERE 
		PD10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('[dbo].[LT_TS06BIDRFP_ForgivenessProgram] - No data returned for AccountNumber %s',11,2, @AccountNumber)
	END