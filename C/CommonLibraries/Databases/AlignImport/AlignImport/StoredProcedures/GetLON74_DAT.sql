CREATE PROCEDURE [dbo].[GetLON74_DAT]
	@FinancialDataId int
AS
	SELECT DISTINCT
        CASE
            WHEN loan.ln_loan_type IN ('CONS' ,'CONU') THEN ISNULL(grp.gr_interest_rate, '')
            ELSE ''
        END  AS [LR_LON_WIR],
        CASE
            WHEN loan.ln_loan_type IN ('CONS' ,'CONU') THEN ISNULL(dbo.CONVERT_DATE(loan.ln_1st_disb_date), '')
            ELSE ''
        END  AS [LD_LON_WIR_EFF_BEG],
		'' AS [LD_LON_WIR_EFF_END],
		'' AS [LI_LON_WIR_OVR],
		'' AS [LR_LON_WIR_PRV]
	FROM
		[ITELSQLDF_Loan_NoZeroBalances] loan
		LEFT JOIN ITEKSQLDF_Group grp
			ON loan.gr_id = grp.gr_id
			AND loan.br_ssn = grp.br_ssn
	WHERE
		loan.FinancialDataId = @FinancialDataId
RETURN 0