/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/UNH 26447.RZ";
FILENAME REPORT2 "&RPTLIB/UNH 26447.R2";

PROC IMPORT DATAFILE="T:\SAS\UNH 25823.csv"
     OUT=Datafile
     DBMS=CSV
     REPLACE;
     GETNAMES=YES;
RUN;

DATA Datafile;
	SET Datafile;
	DF_SPE_ACC_ID = PUT(BORROWER_ACCT_NO, z10.);
RUN;

PROC SQL;
	CREATE TABLE Borrowers AS
		SELECT DISTINCT 
			DF_SPE_ACC_ID
		FROM 
			Datafile;
QUIT;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ;*/ /*test*/

DATA DUSTER.Borrowers; *Send data to Duster;
SET Borrowers;
RUN;

RSUBMIT;

%LET DB = DLGSUTWH; /*live*/
/*%LET DB = DLGSWQUT;*/ /*test*/
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1; /*needed for SQL queries, but not for DB2 queries*/

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
CREATE TABLE LoanDetail AS
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS Borrower_acct,
		LN10.BF_SSN,
		LN10.LN_SEQ AS Loan_seq,
		LN10.IC_LON_PGM AS Loan_type,
		LN15.LD_DSB AS Disbursement_Date,
		LN15.LA_DSB AS Total_Disb_Amount,
		/*LN72.PRIOR_LR_ITR AS Statutory_Int_Rate,*/
		CASE WHEN LN10.LC_STA_LON10 = 'D' THEN 'Loan Status: Transfered'
			 WHEN DW01.WC_DW_LON_STA = '01' THEN 'Loan Status: In Grace'
			 WHEN DW01.WC_DW_LON_STA = '02' THEN 'Loan Status: In School'
			 WHEN DW01.WC_DW_LON_STA = '03' THEN 'Loan Status: In Repayment'
			 WHEN DW01.WC_DW_LON_STA = '04' THEN 'Loan Status: In Deferment'
			 WHEN DW01.WC_DW_LON_STA = '05' THEN 'Loan Status: In Forbearance'
			 WHEN DW01.WC_DW_LON_STA = '06' THEN 'Loan Status: In Cure'
			 WHEN DW01.WC_DW_LON_STA = '07' THEN 'Loan Status: Claim Pending'
			 WHEN DW01.WC_DW_LON_STA = '08' THEN 'Loan Status: Claim Submitted'
			 WHEN DW01.WC_DW_LON_STA = '09' THEN 'Loan Status: Claim Cancelled'
			 WHEN DW01.WC_DW_LON_STA = '10' THEN 'Loan Status: Claim Reject'
			 WHEN DW01.WC_DW_LON_STA = '11' THEN 'Loan Status: Claim Returned'
			 WHEN DW01.WC_DW_LON_STA = '12' THEN 'Loan Status: Claim Paid'
			 WHEN DW01.WC_DW_LON_STA = '13' THEN 'Loan Status: Pre-claim Pending'
			 WHEN DW01.WC_DW_LON_STA = '14' THEN 'Loan Status: Preclaim submitted'
			 WHEN DW01.WC_DW_LON_STA = '15' THEN 'Loan Status: Pre-claim Cancelled'
			 WHEN DW01.WC_DW_LON_STA = '16' THEN 'Loan Status: Death Alleged'
			 WHEN DW01.WC_DW_LON_STA = '17' THEN 'Loan Status: Death Verified'
			 WHEN DW01.WC_DW_LON_STA = '18' THEN 'Loan Status: Disability Alleged' 
			 WHEN DW01.WC_DW_LON_STA = '19' THEN 'Loan Status: Disability Verified'
			 WHEN DW01.WC_DW_LON_STA = '20' THEN 'Loan Status: Bankruptcy Alleged'
			 WHEN DW01.WC_DW_LON_STA = '21' THEN 'Loan Status: Bankruptcy Verified'
			 WHEN DW01.WC_DW_LON_STA = '22' THEN 'Loan Status: Paid In Full'
			 WHEN DW01.WC_DW_LON_STA = '23' THEN 'Loan Status: Not Fully Originated'
			 WHEN DW01.WC_DW_LON_STA = '88' THEN 'Loan Status: Processing Error'
			 WHEN DW01.WC_DW_LON_STA = '98' THEN 'Loan Status: Unknown'
			 ELSE 'UNKNOWN' END AS Current_loan_status,
		'' AS Basis_For_Application,
		'Approved' AS Outcome_Of_Request,
		LN72.LD_ITR_APL AS Date_Of_Outcome,
		CASE WHEN LN10.LC_STA_LON10 = 'D' THEN LN10.LD_STA_LON10
			 WHEN DW01.WC_DW_LON_STA = '01' THEN intnx('day', SD10.LD_SCL_SPR, 1)
			 WHEN DW01.WC_DW_LON_STA = '02' THEN SD10.LD_ENR_STA_EFF_CAM
			 WHEN DW01.WC_DW_LON_STA = '03' THEN  
				CASE WHEN LN10.IC_LON_PGM IN ('CNSLDN','SUBCNS','SUBSPC','UNCNS','UNSPC','PLUS','PLUSGB') THEN intnx('day', LN15.LD_DSB , 1)
				     ELSE LN10.LD_END_GRC_PRD END
			 WHEN DW01.WC_DW_LON_STA = '04' THEN LN50.LD_DFR_BEG
			 WHEN DW01.WC_DW_LON_STA = '05' THEN LN60.LD_FOR_BEG
			 WHEN DW01.WC_DW_LON_STA = '06' THEN .
			 WHEN DW01.WC_DW_LON_STA = '07' THEN LN40.LD_CRT_CLM_PCL
			 WHEN DW01.WC_DW_LON_STA = '08' THEN LN40.LD_SBM_CLM_PCL
			 WHEN DW01.WC_DW_LON_STA = '09' THEN LN40.LD_CAN_CLM_PCL
			 WHEN DW01.WC_DW_LON_STA = '10' THEN LN40.LD_CLM_REJ_RTN_ACL
			 WHEN DW01.WC_DW_LON_STA = '11' THEN LN40.LD_CLM_REJ_RTN_ACL
			 WHEN DW01.WC_DW_LON_STA = '12' THEN LN90.LD_FAT_EFF
			 WHEN DW01.WC_DW_LON_STA = '13' THEN LN40.LD_CRT_CLM_PCL
			 WHEN DW01.WC_DW_LON_STA = '14' THEN LN40.LD_SBM_CLM_PCL
			 WHEN DW01.WC_DW_LON_STA = '15' THEN LN40.LD_CAN_CLM_PCL
			 WHEN DW01.WC_DW_LON_STA = '16' THEN PD20.DD_DTH_NTF
			 WHEN DW01.WC_DW_LON_STA = '17' THEN PD21.DD_DTH_VER
			 WHEN DW01.WC_DW_LON_STA = '18' THEN PD22.DD_DSA_RPT
			 WHEN DW01.WC_DW_LON_STA = '19' THEN PD23.DD_DSA_VER
			 WHEN DW01.WC_DW_LON_STA = '20' THEN PD24.DD_BKR_NTF
			 WHEN DW01.WC_DW_LON_STA = '21' THEN PD24.DD_BKR_VER
			 WHEN DW01.WC_DW_LON_STA = '22' THEN LN10.LD_PIF_RPT
			 WHEN DW01.WC_DW_LON_STA = '23' THEN LN15.LD_DSB
			 WHEN DW01.WC_DW_LON_STA = '88' THEN .
			 WHEN DW01.WC_DW_LON_STA = '98' THEN .
			 ELSE . END AS Current_status_date,
		LN72.LD_ITR_EFF_BEG AS Military_Interest_rate_start,
		LN72.LD_ITR_EFF_END AS Military_Interest_rate_end,
		LN72.PRIOR_END_DATE AS PRIOR_END_DATE,
		LN72.LR_ITR AS Adjusted_int_rate,
		LN72.LD_ITR_EFF_BEG AS Effective_date_of_adjusted,
		LN72.LD_ITR_EFF_END AS Effective_end_date_of_adjusted,
		LN72.NEXT_BEG_DATE AS NEXT_BEG_DATE
	FROM
		WORK.Borrowers BORR
		INNER JOIN OLWHRM1.PD10_PRS_NME PD10
			ON BORR.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		INNER JOIN OLWHRM1.LN10_LON LN10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
		INNER JOIN 
			(
				SELECT
					BF_SSN,	
					LN_SEQ,
					MIN(LD_DSB) AS LD_DSB,
					SUM(LA_DSB) AS LA_DSB 
				FROM 
					OLWHRM1.LN15_DSB
				GROUP BY
					BF_SSN,
					LN_SEQ
			) LN15
			ON LN15.BF_SSN = LN10.BF_SSN
			AND LN15.LN_SEQ = LN10.LN_SEQ
		LEFT OUTER JOIN 
			(
				SELECT
					DF_PRS_ID,
					MAX(DD_DTH_NTF) AS DD_DTH_NTF
				FROM
					OLWHRM1.PD20_PRS_DTH
				GROUP BY
					DF_PRS_ID
			)PD20
			ON PD20.DF_PRS_ID = PD10.DF_PRS_ID
		LEFT OUTER JOIN 
			(
				SELECT
					DF_PRS_ID,
					MAX(DD_DTH_VER) AS DD_DTH_VER
				FROM
					OLWHRM1.PD21_GTR_DTH
				GROUP BY
					DF_PRS_ID
			)PD21
			ON PD21.DF_PRS_ID = PD10.DF_PRS_ID
		LEFT OUTER JOIN 
			(
				SELECT
					DF_PRS_ID,
					MAX(DD_DSA_RPT) AS DD_DSA_RPT
				FROM
					OLWHRM1.PD22_PRS_DSA
				GROUP BY
					DF_PRS_ID
			)PD22 
			ON PD22.DF_PRS_ID = PD10.DF_PRS_ID
		LEFT OUTER JOIN 
			(
				SELECT
					DF_PRS_ID,
					MAX(DD_DSA_VER) AS DD_DSA_VER
				FROM
					OLWHRM1.PD23_GTR_DSA 
				GROUP BY
					DF_PRS_ID
			)PD23 
			ON PD23.DF_PRS_ID = PD10.DF_PRS_ID
		LEFT OUTER JOIN 
			(
				SELECT
					DF_PRS_ID,
					MAX(DD_BKR_NTF) AS DD_BKR_NTF,
					MAX(DD_BKR_VER) AS DD_BKR_VER
				FROM
					 OLWHRM1.PD24_PRS_BKR
				GROUP BY
					DF_PRS_ID
			)PD24
			ON PD24.DF_PRS_ID = PD10.DF_PRS_ID
		LEFT OUTER JOIN 
			(
				SELECT
					BF_SSN,
					LN_SEQ,
					MAX(LD_CRT_CLM_PCL) AS LD_CRT_CLM_PCL,
					MAX(LD_SBM_CLM_PCL) AS LD_SBM_CLM_PCL,
					MAX(LD_CAN_CLM_PCL) AS LD_CAN_CLM_PCL,
					MAX(LD_CLM_REJ_RTN_ACL) AS LD_CLM_REJ_RTN_ACL
				FROM
					OLWHRM1.LN40_LON_CLM_PCL
				GROUP BY
					BF_SSN,
					LN_SEQ
			)LN40
			ON LN40.BF_SSN = LN10.BF_SSN
			AND LN40.LN_SEQ = LN10.LN_SEQ
		LEFT OUTER JOIN
			(
				SELECT
					BF_SSN,
					LN_SEQ,
					MAX(LD_DFR_BEG) AS LD_DFR_BEG
				FROM 
					OLWHRM1.LN50_BR_DFR_APV 
				WHERE 
					LC_STA_LON50 = 'A'
				GROUP BY
					BF_SSN,
					LN_SEQ
			)LN50
			ON LN50.BF_SSN = LN10.BF_SSN
			AND LN50.LN_SEQ = LN10.LN_SEQ
		LEFT OUTER JOIN
			(
				SELECT
					BF_SSN,
					LN_SEQ,
					MAX(LD_FOR_BEG) AS LD_FOR_BEG
				FROM 
					OLWHRM1.LN60_BR_FOR_APV 
				WHERE 
					LC_STA_LON60 = 'A'
				GROUP BY
					BF_SSN,
					LN_SEQ
			)LN60
			ON LN60.BF_SSN = LN10.BF_SSN
			AND LN60.LN_SEQ = LN10.LN_SEQ
		LEFT OUTER JOIN 
			(
				SELECT
					BF_SSN,
					LN_SEQ,
					MAX(LD_FAT_EFF) AS LD_FAT_EFF
				FROM
					OLWHRM1.LN90_FIN_ATY
				WHERE
					PC_FAT_TYP = '10'
					AND PC_FAT_SUB_TYP = '30'
				GROUP BY
					BF_SSN,
					LN_SEQ
			)LN90
			ON LN90.BF_SSN = LN10.BF_SSN
			AND LN90.LN_SEQ = LN10.LN_SEQ
		LEFT OUTER JOIN 
			(
				SELECT
					LF_STU_SSN,
					MAX(LD_ENR_STA_EFF_CAM) AS LD_ENR_STA_EFF_CAM,
					MAX(LD_SCL_SPR) AS LD_SCL_SPR
				FROM
					OLWHRM1.SD10_STU_SPR
				GROUP BY 
					LF_STU_SSN
			)SD10
			ON SD10.LF_STU_SSN = LN10.BF_SSN	
		INNER JOIN 
			(
				SELECT
					LN72M.BF_SSN,
					LN72M.LN_SEQ,
					LN72M.LD_ITR_EFF_BEG,
					LN72M.LD_ITR_EFF_END,
					LN72M.LR_ITR,
					LN72M.LI_SPC_ITR,
					LN72M.LD_ITR_APL,
					MAX(CASE WHEN LN72Prior.LD_ITR_EFF_END < LN72M.LD_ITR_EFF_BEG THEN LN72Prior.LD_ITR_EFF_END END) AS PRIOR_END_DATE,
					MIN(CASE WHEN LN72Prior.LD_ITR_EFF_BEG > LN72M.LD_ITR_EFF_END THEN LN72Prior.LD_ITR_EFF_BEG END) AS NEXT_BEG_DATE
				FROM 
					OLWHRM1.LN72_INT_RTE_HST LN72M
					INNER JOIN 
						(
							SELECT
								LN72P.BF_SSN,
								LN72P.LN_SEQ,
								LN72P.LD_ITR_EFF_BEG,
								LN72P.LD_ITR_EFF_END,
								LN72P.LR_ITR
							FROM
								OLWHRM1.LN72_INT_RTE_HST LN72P
							WHERE
								LN72P.LC_STA_LON72 = 'A'
						)LN72Prior 
						ON LN72M.BF_SSN = LN72Prior.BF_SSN
						AND LN72M.LN_SEQ = LN72Prior.LN_SEQ
				WHERE
					LN72M.LC_INT_RDC_PGM = 'M'
					AND LN72M.LC_STA_LON72 = 'A'
				GROUP BY
					LN72M.BF_SSN,
					LN72M.LN_SEQ,
					LN72M.LD_ITR_EFF_BEG,
					LN72M.LD_ITR_EFF_END,
					LN72M.LR_ITR,
					LN72M.LI_SPC_ITR,
					LN72M.LD_ITR_APL
			) LN72 
			ON LN72.BF_SSN = LN10.BF_SSN
			AND LN72.LN_SEQ = LN10.LN_SEQ
	WHERE
		LN10.IC_LON_PGM not in ('TILP','COMPLT')
;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

PROC SQL;
CREATE TABLE LoanDetail2 AS
	SELECT
		LD.Borrower_acct,
		LD.Loan_seq,
		LD.Loan_type,
		LD.Disbursement_Date,
		LD.Total_Disb_Amount,
		LD.Current_loan_status,
		LD.Current_status_date,
		LD.Basis_For_Application,
		LD.Outcome_Of_Request,
		LD.Date_Of_Outcome,
		LD.Military_Interest_rate_start,
		LD.Military_Interest_rate_end,
		LN72Pre.LR_ITR AS Int_Rate_before_military_rate,
		LD.Adjusted_int_rate,
		LD.Effective_date_of_adjusted,
		LD.Effective_end_date_of_adjusted,
		LN72Post.LR_ITR AS Post_adjustment_int_rate
	FROM LoanDetail LD
	INNER JOIN OLWHRM1.LN72_INT_RTE_HST LN72Pre
		ON LD.PRIOR_END_DATE = LN72Pre.LD_ITR_EFF_END
		AND LD.BF_SSN = LN72Pre.BF_SSN
		AND LD.Loan_seq = LN72Pre.LN_SEQ
		AND LN72Pre.LC_STA_LON72 = 'A'
	INNER JOIN OLWHRM1.LN72_INT_RTE_HST LN72Post
		ON LD.NEXT_BEG_DATE = LN72Post.LD_ITR_EFF_BEG
		AND LD.BF_SSN = LN72Post.BF_SSN
		AND LD.Loan_seq = LN72Post.LN_SEQ
		AND LN72Post.LC_STA_LON72 = 'A'
;
Quit;
ENDRSUBMIT;

DATA LoanDetail2;
	SET DUSTER.LoanDetail2;
	FORMAT Current_status_date MMDDYY10.;
	FORMAT Disbursement_Date MMDDYY10.;
	FORMAT Military_Interest_rate_start MMDDYY10.;
	FORMAT Military_Interest_rate_end MMDDYY10.;
	FORMAT Effective_date_of_adjusted MMDDYY10.;
	FORMAT Effective_end_date_of_adjusted MMDDYY10.;
	FORMAT Date_Of_Outcome MMDDYY10.;
RUN;

PROC EXPORT
		DATA=LoanDetail2
		OUTFILE="&RPTLIB\UNH 26447.csv"
		DBMS = CSV
		REPLACE;
		
RUN;
