/*
LOANS SELECTED FOR REHABILITATION SUPPORTING DOCUMENTS

This program selects legal loans selected for rehabilitation by the
recent rehab selection process and generates a promissory note pull
list, a file pull list, and end of job count report, and
acknowlegments.

The file room uses the lists to pull promissory notes and files so
collections can prepare documents to be mailed with the rehab 
selection letter.

*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWRH2.LWRH2R2";
FILENAME REPORT3 "&RPTLIB/ULWRH2.LWRH2R3";
FILENAME REPORT4 "&RPTLIB/ULWRH2.LWRH2R4";
FILENAME REPORT5 "&RPTLIB/ULWRH2.LWRH2R5";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE REHABLGL AS
SELECT NAME,
	DM_PRS_LST,
	DM_PRS_1,
	SSN,
	CLID,
	AA_GTE_LON_AMT,
	AD_BR_SIG,
	LA_CLM_BAL,
	LF_CLM_ID,
	AD_DSB_ADJ,
	PF_ACT,
	BD_ATY_PRF 	LENGTH=4
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	RTRIM(D.dm_prs_lst)||','||D.dm_prs_1 AS NAME,
	D.dm_prs_lst,
	D.dm_prs_1,
	D.df_prs_id						AS SSN,
	C.af_apl_id||C.af_apl_id_sfx	AS CLID,
	E.AA_GTE_LON_AMT,
	F.ad_br_sig,
	G.la_clm_bal,
	G.lf_clm_id,
	H.ad_dsb_adj,
	AY01.PF_ACT,
	AY01.BD_ATY_PRF
FROM 	OLWHRM1.DC01_LON_CLM_INF C INNER JOIN
		OLWHRM1.PD01_PDM_INF D ON
			C.bf_ssn = D.df_prs_id INNER JOIN
		OLWHRM1.GA10_LON_APP E ON
	 		C.af_apl_id = E.af_apl_id AND
			C.af_apl_id_sfx = E.af_apl_id_sfx INNER JOIN
		OLWHRM1.GA01_APP F ON
			C.af_apl_id = F.af_apl_id INNER JOIN
		OLWHRM1.DC02_BAL_INT G ON
			C.af_apl_id = G.af_apl_id AND
			C.af_apl_id_sfx = G.af_apl_id_sfx INNER JOIN
		OLWHRM1.GA11_LON_DSB_ATY H ON
			C.af_apl_id = H.af_apl_id AND
			C.af_apl_id_sfx = H.af_apl_id_sfx AND
			H.AC_DSB_ADJ = 'A' AND
			H.AN_DSB_SEQ = 0001 INNER JOIN 
		OLWHRM1.AY01_BR_ATY AY01 ON 
			C.BF_SSN = AY01.DF_PRS_ID
			AND AY01.PF_ACT = 'QRHBA'	
WHERE C.lc_grn IN ('06', '07') 		
);

CREATE TABLE AY01 AS
SELECT SSN
	,ACODE
	,AC_DT LENGTH=4
FROM CONNECTION TO DB2 (
SELECT DISTINCT  
	 DF_PRS_ID AS SSN
	,PF_ACT AS ACODE
	,BD_ATY_PRF AS AC_DT
FROM OLWHRM1.AY01_BR_ATY 		
WHERE PF_ACT = ('DSLT2')		
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA REHABLGL;SET WORKLOCL.REHABLGL;RUN;
DATA AY01;SET WORKLOCL.AY01;RUN;

PROC SORT DATA=REHABLGL;BY SSN;RUN;
PROC SORT DATA=AY01;BY SSN;RUN;

DATA REHABLGL;
MERGE REHABLGL (IN=A) AY01;
BY SSN;
IF A;
RUN;

DATA REHABLGL;
SET REHABLGL;
IF BD_ATY_PRF > AC_DT THEN OUTPUT;
RUN;

PROC SQL;
CREATE TABLE EOJ AS
SELECT	COUNT(DISTINCT SSN)	AS SSNS,
		COUNT(CLID)	AS CLIDS
FROM REHABLGL;
QUIT;

PROC SORT DATA=REHABLGL;
BY SSN NAME;
RUN;
/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS CENTER PAGENO=1 LS=80;
PROC PRINT NOOBS SPLIT='/' DATA=REHABLGL;
BY SSN NAME;
VAR CLID
	ad_dsb_adj
	AA_GTE_LON_AMT
	;
FORMAT 	ad_dsb_adj MMDDYY10.;
LABEL	CLID = 'UNIQUE ID'
		ad_dsb_adj = 'DISBURSEMENT DATE'
		AA_GTE_LON_AMT = 'LOAN AMOUNT'	
		;

TITLE1  'PROMISSORY NOTE PULL LIST';
TITLE2	'LEGAL LOANS SELECTED FOR REHABILITATION';
RUN;
PROC SORT DATA=REHABLGL;
BY NAME SSN;
RUN;
/*PROC PRINTTO PRINT=REPORT3;
RUN;*/
OPTIONS PAGENO=1;
PROC PRINT NOOBS SPLIT='/' DATA=REHABLGL;
BY NAME SSN;
VAR CLID
	ad_dsb_adj
	AA_GTE_LON_AMT
	;
FORMAT 	ad_dsb_adj MMDDYY10.;
LABEL	CLID = 'UNIQUE ID'
		ad_dsb_adj = 'DISBURSEMENT DATE'
		AA_GTE_LON_AMT = 'LOAN AMOUNT'	
		;

TITLE1  'FILE PULL LIST';
TITLE2	'LEGAL LOANS SELECTED FOR REHABILITATION';
RUN;
/*PROC PRINTTO PRINT=REPORT4;
RUN;*/
OPTIONS PAGENO=1;
PROC PRINT NOOBS SPLIT='/' DATA=EOJ;
VAR	SSNS
	CLIDS;
LABEL	SSNS = 'BORROWERS'
		CLIDS = 'LOANS';
TITLE1  'END OF JOB COUNTS';
TITLE2	'LEGAL LOANS SELECTED FOR REHABILITATION';
RUN;
PROC SORT DATA=REHABLGL;
BY SSN ad_dsb_adj;
RUN;
/* Print acknowledgment to file 'REPORT5'. */
OPTIONS CENTER PAGENO=1 LS=80 PS=50;
DATA REHABLGL;
	SET REHABLGL;
	FILE 'C:\WINDOWS\TEMP\REPORT5';
	*FILE REPORT5 PRINT;
	TITLE;
	SSN2 = TRIM(SSN);
	SSN3 = INPUT(SSN2, 9.);

	PUT  '                  UTAH HIGHER EDUCATION ASSISTANCE AUTHORITY'
		/
		/'                    REHABILITATION BORROWER ACKNOWLEDGMENT'
		//
		/'================================================================================'
		//
		/'Borrower Name:'		@21 dm_prs_1 dm_prs_lst
		/'Borrower SSN:'		@21 SSN3 SSN11.
		/'Loan Unique ID:'		@21 CLID
		/'Disbursement Date:'	@21 ad_dsb_adj MMDDYY10.
		/'Claim Number:'	    @21 lf_clm_id
		//
		/'================================================================================'
		//
		/'I acknowledge that on _______________, I re-signed a copy of my original'
		/'promissory note, initially signed on ' ad_br_sig MMDDYY10. @48 ', thereby reaffirming my'
		/'obligation to repay my student loan, as evidenced by the promissory note, which'
		/'has been in default and upon which judgment has been entered.  The valuable'
		/'consideration for my promise to pay this loan is the agreement of Utah Higher'
		/'Education Assistance Authority (UHEAA) to satisfy the outstanding judgment'
		/'against me and rehabilitate my loan, which includes re-establishing my'
		/'eligibility for Title IV financial aid and reporting my loan to national credit'
		/'bureaus as no longer in default.  By signing a copy of my original promissory'
		/'note on this date, UHEAA and I have entered into a new agreement whereby I agree'
		/'to repay the note according to the terms and conditions thereof and acknowledge'
		/'that in the event of my subsequent default, the holder may bring suit against me'
		/'on the promissory note I originally executed on ' ad_br_sig MMDDYY10. @59 ' and re-executed on'
		/'_______________.  The current balance of the note is ' la_clm_bal DOLLAR13.2 @ 67 ' which'
		/'includes adjustments for payments made, interest accrued and other costs'
		/'assessed.'
		//
		/'================================================================================'
		//
		/'_______________________________________________________     ____________________'
		/@1 dm_prs_1 dm_prs_lst @61 'Date'
		/
		/'_______________________________________________________     ____________________'
		/'MARY GIBBS' @61 'Date'
		/'Utah Higher Education Assistance Authority Official'
		//
		/'================================================================================';
	PUT _PAGE_;
RUN;
