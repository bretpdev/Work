CREATE PROCEDURE [dbo].[GetRepaymentData]
    @SSN varchar(9)
AS
    SELECT DISTINCT
        loan.FinancialDataId AS RepaymentDataId,
        'Y' AS RPST10_DAT_ENT_IND,
        'M' AS LC_FRQ_PAY,
        '1' AS LC_RPD_DIS,
        'A' AS LC_STA_RPST10,
        dbo.CONVERT_DATE(GRP.gr_1st_pmt_due) AS LD_RPS_1_PAY_DU,
        '' AS LD_RTN_RPD_DIS, 
        ISNULL(MAX_DISC.Max_Disc_Date, dbo.CONVERT_DATE(GRP.gr_disclosure_date)) AS LD_SNT_RPD_DIS,
        DATEADD(MONTH,1, ISNULL(MAX_DISC.Max_Disc_Date, dbo.CONVERT_DATE(GRP.gr_disclosure_date))) AS LD_STA_RPST10,
        'N' AS LI_SIG_RPD_DIS,
        '' AS LN_RPS_SEQ, 
        '' AS LC_RPY_FIX_TRM_AMT, 
        'N' AS LON06_DAT_ENT_IND,
        '' AS LA_CPI_RPD_DIS_AGG, 
        '' AS LA_IC_RPD_DIS_AGG, 
        '' AS LA_RPD_INT_DIS_AGG, 
        '' AS LA_TOT_RPD_DIS_AGG, 
        '' AS LC_STA_LON06, 
        '' AS LR_INT_RPD_DIS_AGG, 
        '' AS LC_TYP_SCH_DIS, 
        '' AS LA_ACR_INT_RPD_AGG, 
        '0' AS LN_LON07_DAT_OCC_CNT, 
        'Y' AS LON65_DAT_ENT_IND, 
        MAP.CommpassLoanSeq AS LN_SEQ, 
        LOAN.ln_curr_principal AS LA_CPI_RPD_DIS, 
        right(LOAN.ln_curr_interest, 8) AS LA_RPD_INT_DIS, 
        cast(cast((CAST(ln_curr_interest AS MONEY) / 100000) + (CAST(ln_curr_principal AS MONEY ) / 100) as decimal(10,2)) * 100 as int) AS LA_TOT_RPD_DIS, 
        'A' AS LC_STA_LON65,
        loan.ln_interest_rate AS LR_INT_RPD_DIS,
        '' AS LA_ANT_CAP, 
        CASE
			WHEN GRP.gr_pmt_schedule = 'E' THEN 'EL'  
			WHEN GRP.gr_pmt_schedule = 'G' THEN 'G'  
			WHEN GRP.gr_pmt_schedule = 'X' THEN 'EG'  
			WHEN GRP.gr_pmt_schedule = 'P' THEN 'IL'  
			WHEN GRP.gr_pmt_schedule = 'F' THEN 'IB'  
			WHEN GRP.gr_pmt_schedule = 'I' THEN 'IS'   
			ELSE 'L'
		END AS LC_TYP_SCH_DIS_1,
        '' AS LA_ACR_INT_RPD, 
        '' AS LD_RPD_MAX_TRM_SR, 
        '' AS LN_RPD_MAX_TRM_REQ, 
        '' AS LN_LON66_DAT_OCC_CNT, --This is set in code
        '0000' AS LN_LON04_DAT_OCC_CNT, 
        CASE
            WHEN IBR.br_ssn IS NOT NULL THEN 'Y'
            ELSE 'N'
        END AS RS05_DAT_ENT_IND,
        IBR.ibr_forgiveness_months_counter AS BD_CRT_RS05, 
        '' AS BN_IBR_SEQ, 
        '' AS BF_SSN_SPO, --BISPOS if provided ON THE IBR TABLE FILE IT SAYS NOT PROVIDED SO WE WILL SEE ON THIS ONE....
        '' AS BC_IBR_INF_SRC_VER, 
        'H' AS BC_IRS_TAX_FIL_STA, 
        IBR.agi AS BA_AGI, 
        IBR.family_size AS BN_MEM_HSE_HLD, 
        '' AS BI_JNT_BR_SPO_RPY, --Y IF SPOUSE SSN IS POPULATED (SPOUSE SSN NOT IN ALIGN VERSION)
		CASE
			WHEN IBR.pfh_amount IS NOT NULL THEN 'N'
			WHEN IBR.pfh_amount != '' THEN 'N'
			ELSE 'Y'
		END AS RS20_DAT_ENT_IND,
        IBR.pfh_amount AS LA_PFH_PAY, 
        IBR.perm_standard_amount AS LA_PMN_STD_PAY 
    FROM
        [dbo].[ITELSQLDF_Loan_NoZeroBalances] LOAN
        LEFT JOIN [dbo].[ITEKSQLDF_Group] GRP
            ON GRP.br_ssn = LOAN.br_ssn
            AND GRP.gr_id = LOAN.gr_id
        INNER JOIN[dbo].[CompassLoanMapping] MAP
            ON MAP.br_ssn = LOAN.br_ssn
            AND MAP.NelNetLoanSeq = LOAN.ln_num
        LEFT JOIN [dbo].[ITLSSQLDF_IBR] IBR
            ON IBR.br_ssn = LOAN.br_ssn
            AND IBR.ln_num = LOAN.ln_num
		LEFT JOIN
		(
			SELECT DISTINCT
				D.br_ssn,
				group_id,
				MAX(dbo.CONVERT_DEFFORB_DATE([disclosure_date])) AS Max_Disc_Date
			FROM
				[dbo].[ITLQSQLDF_Disclosure] D
			GROUP BY 
				D.br_ssn,
				group_id
		) MAX_DISC
			ON MAX_DISC.br_ssn = loan.br_ssn
			and MAX_DISC.group_id = loan.gr_id
		LEFT JOIN
		(
			SELECT DISTINCT
				D.br_ssn,
				group_id,
				MAX(dbo.CONVERT_DEFFORB_DATE([disclosure_date])) AS Newest_Disc_Date
			FROM
				[dbo].[ITLQSQLDF_Disclosure] D
			INNER JOIN [dbo].[ITLSSQLDF_IBR] I
				ON I.br_ssn = D.br_ssn
			GROUP BY 
				D.br_ssn,
				group_id
		) DISC_DATE
			ON DISC_DATE.br_ssn = loan.br_ssn
			and DISC_DATE.group_id = loan.gr_id
		WHERE
			LOAN.br_ssn = @SSN
RETURN 0
