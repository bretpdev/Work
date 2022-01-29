/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/NHXXXX.RZ";
FILENAME REPORTX "&RPTLIB/NHXXXX.RX";

PROC IMPORT DATAFILE="&RPTLIB\NH XXXX.csv"
     OUT=Datafile
     DBMS=CSV
     REPLACE;
     GETNAMES=YES;
RUN;

DATA Datafile;
	SET Datafile;
	DF_SPE_ACC_IDX = PUT(DF_SPE_ACC_ID, XX.);
RUN;

PROC SQL;
	CREATE TABLE Borrowers AS
		SELECT DISTINCT 
			DF_SPE_ACC_IDX
		FROM 
			Datafile;
QUIT;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;

DATA LEGEND.Borrowers; *Send data to Legend;
SET Borrowers;
RUN;

RSUBMIT;

%LET DB = DNFPUTDL; /*live*/
/*%LET DB = DLGSWQUT; /*test*/
LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
CREATE TABLE LoanDetail AS
	SELECT DISTINCT
		PDXX.DF_SPE_ACC_ID AS Borrower_acct,
		LNXX.BF_SSN,
		LNXX.LN_SEQ AS Loan_seq,
		LNXX.IC_LON_PGM AS Loan_type,
		LNXX.LD_DSB AS Disbursement_Date,
		LNXX.LA_DSB AS Total_Disb_Amount,
/*		LNXX.PRIOR_LR_ITR AS Statutory_Int_Rate,*/
		CASE WHEN LNXX.LC_STA_LONXX = 'D' THEN 'Loan Status: Transfered'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: In Grace'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: In School'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: In Repayment'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: In Deferment'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: In Forbearance'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: In Cure'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Claim Pending'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Claim Submitted'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Claim Cancelled'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Claim Reject'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Claim Returned'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Claim Paid'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Pre-claim Pending'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Preclaim submitted'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Pre-claim Cancelled'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Death Alleged'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Death Verified'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Disability Alleged' 
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Disability Verified'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Bankruptcy Alleged'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Bankruptcy Verified'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Paid In Full'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Not Fully Originated'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Processing Error'
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN 'Loan Status: Unknown'
			 ELSE 'UNKNOWN' END AS Current_loan_status,
		'' AS Basis_For_Application,
		'Approved' AS Outcome_Of_Request,
		LNXX.LD_ITR_APL AS Date_Of_Outcome,
		CASE WHEN LNXX.LC_STA_LONXX = 'D' THEN LNXX.LD_STA_LONXX
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN intnx('day', SDXX.LD_SCL_SPR, X)
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN SDXX.LD_ENR_STA_EFF_CAM
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN  
				CASE WHEN LNXX.IC_LON_PGM IN ('CNSLDN','SUBCNS','SUBSPC','UNCNS','UNSPC','PLUS','PLUSGB') 
					 THEN intnx('day', LNXX.LD_DSB , X)
				     ELSE LNXX.LD_END_GRC_PRD END
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_DFR_BEG
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_FOR_BEG
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN .
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_CRT_CLM_PCL
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_SBM_CLM_PCL
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_CAN_CLM_PCL
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_CLM_REJ_RTN_ACL
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_CLM_REJ_RTN_ACL
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_FAT_EFF
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_CRT_CLM_PCL
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_SBM_CLM_PCL
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_CAN_CLM_PCL
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN PDXX.DD_DTH_NTF
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN PDXX.DD_DTH_VER
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN PDXX.DD_DSA_RPT
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN PDXX.DD_DSA_VER
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN PDXX.DD_BKR_NTF
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN PDXX.DD_BKR_VER
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_PIF_RPT
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN LNXX.LD_DSB
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN .
			 WHEN DWXX.WC_DW_LON_STA = 'XX' THEN .
			 ELSE . END AS Current_status_date,
		LNXX.LD_ITR_EFF_BEG AS Military_Interest_rate_start,
		LNXX.LD_ITR_EFF_END AS Military_Interest_rate_end,
		LNXX.PRIOR_END_DATE AS PRIOR_END_DATE,
		LNXX.LR_ITR AS Adjusted_int_rate,
		LNXX.LD_ITR_EFF_BEG AS Effective_date_of_adjusted,
		LNXX.LD_ITR_EFF_END AS Effective_end_date_of_adjusted,
		LNXX.NEXT_BEG_DATE AS NEXT_BEG_DATE
	FROM
		WORK.Borrowers BORR
		INNER JOIN PKUB.PDXX_PRS_NME PDXX
			ON BORR.DF_SPE_ACC_IDX = PDXX.DF_SPE_ACC_ID
		INNER JOIN PKUB.LNXX_LON LNXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
			ON DWXX.BF_SSN = LNXX.BF_SSN
			AND DWXX.LN_SEQ = LNXX.LN_SEQ
		INNER JOIN 
			(
				SELECT
					BF_SSN,	
					LN_SEQ,
					MIN(LD_DSB) AS LD_DSB,
					SUM(LA_DSB) AS LA_DSB 
				FROM 
					PKUB.LNXX_DSB
				GROUP BY
					BF_SSN,
					LN_SEQ
			) LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		LEFT OUTER JOIN 
			(
				SELECT
					DF_PRS_ID,
					MAX(DD_DTH_NTF) AS DD_DTH_NTF
				FROM
					PKUB.PDXX_PRS_DTH
				GROUP BY
					DF_PRS_ID
			)PDXX
			ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		LEFT OUTER JOIN 
			(
				SELECT
					DF_PRS_ID,
					MAX(DD_DTH_VER) AS DD_DTH_VER
				FROM
					PKUB.PDXX_GTR_DTH
				GROUP BY
					DF_PRS_ID
			)PDXX
			ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		LEFT OUTER JOIN 
			(
				SELECT
					DF_PRS_ID,
					MAX(DD_DSA_RPT) AS DD_DSA_RPT
				FROM
					PKUB.PDXX_PRS_DSA
				GROUP BY
					DF_PRS_ID
			)PDXX 
			ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		LEFT OUTER JOIN 
			(
				SELECT
					DF_PRS_ID,
					MAX(DD_DSA_VER) AS DD_DSA_VER
				FROM
					PKUB.PDXX_GTR_DSA 
				GROUP BY
					DF_PRS_ID
			)PDXX 
			ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		LEFT OUTER JOIN 
			(
				SELECT
					DF_PRS_ID,
					MAX(DD_BKR_NTF) AS DD_BKR_NTF,
					MAX(DD_BKR_VER) AS DD_BKR_VER
				FROM
					 PKUB.PDXX_PRS_BKR
				GROUP BY
					DF_PRS_ID
			)PDXX
			ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
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
					PKUB.LNXX_LON_CLM_PCL
				GROUP BY
					BF_SSN,
					LN_SEQ
			)LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		LEFT OUTER JOIN
			(
				SELECT
					BF_SSN,
					LN_SEQ,
					MAX(LD_DFR_BEG) AS LD_DFR_BEG
				FROM 
					PKUB.LNXX_BR_DFR_APV 
				WHERE 
					LC_STA_LONXX = 'A'
				GROUP BY
					BF_SSN,
					LN_SEQ
			)LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		LEFT OUTER JOIN
			(
				SELECT
					BF_SSN,
					LN_SEQ,
					MAX(LD_FOR_BEG) AS LD_FOR_BEG
				FROM 
					PKUB.LNXX_BR_FOR_APV 
				WHERE 
					LC_STA_LONXX = 'A'
				GROUP BY
					BF_SSN,
					LN_SEQ
			)LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		LEFT OUTER JOIN 
			(
				SELECT
					BF_SSN,
					LN_SEQ,
					MAX(LD_FAT_EFF) AS LD_FAT_EFF
				FROM
					PKUB.LNXX_FIN_ATY
				WHERE
					PC_FAT_TYP = 'XX'
					AND PC_FAT_SUB_TYP = 'XX'
				GROUP BY
					BF_SSN,
					LN_SEQ
			)LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		LEFT OUTER JOIN 
			(
				SELECT
					LF_STU_SSN,
					MAX(LD_ENR_STA_EFF_CAM) AS LD_ENR_STA_EFF_CAM,
					MAX(LD_SCL_SPR) AS LD_SCL_SPR
				FROM
					PKUB.SDXX_STU_SPR
				GROUP BY 
					LF_STU_SSN
			)SDXX
			ON SDXX.LF_STU_SSN = LNXX.BF_SSN	
		LEFT OUTER JOIN 
			(
				SELECT
					LNXXM.BF_SSN,
					LNXXM.LN_SEQ,
					LNXXM.LD_ITR_EFF_BEG,
					LNXXM.LD_ITR_EFF_END,
					LNXXM.LR_ITR,
					LNXXM.LI_SPC_ITR,
					LNXXM.LD_ITR_APL,
					MAX(CASE WHEN LNXXPrior.LD_ITR_EFF_END < LNXXM.LD_ITR_EFF_BEG THEN LNXXPrior.LD_ITR_EFF_END END) AS PRIOR_END_DATE,
					MIN(CASE WHEN LNXXPrior.LD_ITR_EFF_BEG > LNXXM.LD_ITR_EFF_END THEN LNXXPrior.LD_ITR_EFF_BEG END) AS NEXT_BEG_DATE
				FROM 
					PKUB.LNXX_INT_RTE_HST LNXXM
					INNER JOIN 
						(
							SELECT
								LNXXP.BF_SSN,
								LNXXP.LN_SEQ,
								LNXXP.LD_ITR_EFF_BEG,
								LNXXP.LD_ITR_EFF_END,
								LNXXP.LR_ITR
							FROM
								PKUB.LNXX_INT_RTE_HST LNXXP
							WHERE
								LNXXP.LC_STA_LONXX = 'A'
						)LNXXPrior 
						ON LNXXM.BF_SSN = LNXXPrior.BF_SSN
						AND LNXXM.LN_SEQ = LNXXPrior.LN_SEQ
				WHERE
					LNXXM.LC_INT_RDC_PGM = 'M'
					AND LNXXM.LC_STA_LONXX = 'A'
				GROUP BY
					LNXXM.BF_SSN,
					LNXXM.LN_SEQ,
					LNXXM.LD_ITR_EFF_BEG,
					LNXXM.LD_ITR_EFF_END,
					LNXXM.LR_ITR,
					LNXXM.LI_SPC_ITR,
					LNXXM.LD_ITR_APL
			) LNXX 
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

PROC SQL;
CREATE TABLE LoanDetailX AS
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
		LNXXPre.LR_ITR AS Int_Rate_before_military_rate,
		LD.Adjusted_int_rate,
		LD.Effective_date_of_adjusted,
		LD.Effective_end_date_of_adjusted,
		LNXXPost.LR_ITR AS Post_adjustment_int_rate
	FROM LoanDetail LD
	INNER JOIN PKUB.LNXX_INT_RTE_HST LNXXPre
		ON LD.PRIOR_END_DATE = LNXXPre.LD_ITR_EFF_END
		AND LD.BF_SSN = LNXXPre.BF_SSN
		AND LD.Loan_seq = LNXXPre.LN_SEQ
		AND LNXXPre.LC_STA_LONXX = 'A'
	INNER JOIN PKUB.LNXX_INT_RTE_HST LNXXPost
		ON LD.NEXT_BEG_DATE = LNXXPost.LD_ITR_EFF_BEG
		AND LD.BF_SSN = LNXXPost.BF_SSN
		AND LD.Loan_seq = LNXXPost.LN_SEQ
		AND LNXXPost.LC_STA_LONXX = 'A'
;
Quit;

ENDRSUBMIT;

DATA LoanDetailX;
	SET LEGEND.LoanDetailX;
	FORMAT Current_status_date MMDDYYXX.;
	FORMAT Disbursement_Date MMDDYYXX.;
	FORMAT Military_Interest_rate_start MMDDYYXX.;
	FORMAT Military_Interest_rate_end MMDDYYXX.;
	FORMAT Effective_date_of_adjusted MMDDYYXX.;
	FORMAT Effective_end_date_of_adjusted MMDDYYXX.;
	FORMAT Date_Of_Outcome MMDDYYXX.;
RUN;

PROC EXPORT
		DATA=LoanDetailX
		OUTFILE="&RPTLIB\NH XXXX.csv"
		DBMS = CSV
		REPLACE;
RUN;
