CREATE PROCEDURE [dbo].[GetLON04_DAT]
    @RepaymentDataId INT
AS
    SELECT DISTINCT
        dbo.CONVERT_DATE(GRP.gr_due_date) AS LD_PCB_NPD, 
        '' AS LI_PSB_TOT_RPD_AGG,
        '' AS LN_AGG_RPD_SEQ, 
        '' AS LC_TYP_PCV_BIL_DAT,
        '' AS LA_PT_PAY_PCV_AGG
	FROM ITEKSQLDF_Group GRP 
	INNER JOIN ITELSQLDF_Loan LOAN
		ON GRP.br_ssn = LOAN.br_ssn
		AND GRP.gr_id = LOAN.gr_id
	WHERE LOAN.FinancialDataId = @RepaymentDataId
		
RETURN 0
