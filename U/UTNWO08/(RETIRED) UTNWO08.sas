/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWO08.NWO08RZ";
FILENAME REPORT2 "&RPTLIB/UNWO08.NWO08R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

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
CONNECT TO DB2 (DATABASE=&DB);

CREATE TABLE INREPAYMENT_ALL AS
	SELECT	*
	FROM	CONNECTION TO DB2 (
				SELECT	COUNT(DISTINCT A.BF_SSN) AS BOR_CT
						,SUM(A.LA_CUR_PRI) AS LA_CUR_PRI
				FROM 	PKUB.LN10_LON A
						INNER JOIN PKUB.DW01_DW_CLC_CLU B
							ON A.BF_SSN = B.BF_SSN
							AND A.LN_SEQ = B.LN_SEQ
				WHERE	B.WC_DW_LON_STA IN ('03', '07', '13', '14')
						AND A.LA_CUR_PRI > 0
						AND A.LC_STA_LON10 = 'R' 
						AND B.WC_LON_STA_DFR = ''
						AND B.WC_LON_STA_FOR = ''
				FOR READ ONLY WITH UR
			);

CREATE TABLE INREPAYMENT_ACH AS
	SELECT	*
	FROM	CONNECTION TO DB2 (
				SELECT	COUNT(DISTINCT A.BF_SSN) AS BOR_CT_ACH
						,SUM(A.LA_CUR_PRI) AS LA_CUR_PRI_ACH
				FROM 	PKUB.LN10_LON A
						INNER JOIN PKUB.DW01_DW_CLC_CLU B
							ON A.BF_SSN = B.BF_SSN
							AND A.LN_SEQ = B.LN_SEQ
						INNER JOIN PKUB.BR30_BR_EFT C
							ON A.BF_SSN = C.BF_SSN
							AND C.BC_EFT_STA = 'A'
				WHERE	B.WC_DW_LON_STA IN ('03', '07', '13', '14')
						AND A.LA_CUR_PRI > 0
						AND A.LC_STA_LON10 = 'R' 
						AND B.WC_LON_STA_DFR = ''
						AND B.WC_LON_STA_FOR = ''
				FOR READ ONLY WITH UR
			);

/*DETAIL FILES*/
/*CREATE TABLE INREPAYMENT_ALL_DET AS*/
/*	SELECT	**/
/*	FROM	CONNECTION TO DB2 (*/
/*				SELECT	DISTINCT */
/*						A.BF_SSN*/
/*						,SUM(A.LA_CUR_PRI) AS LA_CUR_PRI*/
/*				FROM 	PKUB.LN10_LON A*/
/*						INNER JOIN PKUB.DW01_DW_CLC_CLU B*/
/*							ON A.BF_SSN = B.BF_SSN*/
/*							AND A.LN_SEQ = B.LN_SEQ*/
/*				WHERE	B.WC_DW_LON_STA IN ('03', '07', '13', '14')*/
/*						AND A.LA_CUR_PRI > 0*/
/*						AND A.LC_STA_LON10 = 'R' */
/*						AND B.WC_LON_STA_DFR = ''*/
/*						AND B.WC_LON_STA_FOR = ''*/
/*				GROUP BY A.BF_SSN*/
/*				FOR READ ONLY WITH UR*/
/*			);*/
/**/
/*CREATE TABLE INREPAYMENT_ACH_DET AS*/
/*	SELECT	**/
/*	FROM	CONNECTION TO DB2 (*/
/*				SELECT	DISTINCT*/
/*						A.BF_SSN*/
/*						,SUM(A.LA_CUR_PRI) AS LA_CUR_PRI_ACH*/
/*				FROM 	PKUB.LN10_LON A*/
/*						INNER JOIN PKUB.DW01_DW_CLC_CLU B*/
/*							ON A.BF_SSN = B.BF_SSN*/
/*							AND A.LN_SEQ = B.LN_SEQ*/
/*						INNER JOIN PKUB.BR30_BR_EFT C*/
/*							ON A.BF_SSN = C.BF_SSN*/
/*							AND C.BC_EFT_STA = 'A'*/
/*				WHERE	B.WC_DW_LON_STA IN ('03', '07', '13', '14')*/
/*						AND A.LA_CUR_PRI > 0*/
/*						AND A.LC_STA_LON10 = 'R' */
/*						AND B.WC_LON_STA_DFR = ''*/
/*						AND B.WC_LON_STA_FOR = ''*/
/*				GROUP BY A.BF_SSN*/
/*				FOR READ ONLY WITH UR*/
/*			);*/


DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
QUIT;

DATA ACH_OF_TOT_LCL;
	MERGE INREPAYMENT_ALL INREPAYMENT_ACH;
	FORMAT 	BOR_CT BOR_CT_ACH COMMA12.0 LA_CUR_PRI LA_CUR_PRI_ACH DOLLAR18.2;
	FORMAT 	CT_PCT BAL_PCT PERCENT8.2;
	LABEL 	BOR_CT='Total # of Borrowers'
			LA_CUR_PRI='Total Balance'
			BOR_CT_ACH='ACH Borrowers'
			LA_CUR_PRI_ACH='ACH Balance'
			CT_PCT='% of Borrowers on ACH'
			BAL_PCT='% of Total Balance on ACH';
	CT_PCT = BOR_CT_ACH / BOR_CT;
	BAL_PCT = LA_CUR_PRI_ACH / LA_CUR_PRI;
RUN;

ENDRSUBMIT;

DATA ACH_OF_TOT_LCL; SET LEGEND.ACH_OF_TOT_LCL; RUN;

PROC PRINTTO PRINT=REPORT2 NEW; RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127;
TITLE 		'Borrowers in Repayment on ACH';
TITLE2		"RUNDATE &SYSDATE9";
FOOTNOTE1   'JOB = UTNWO08  	 REPORT = UNWO08.NWO08R2';

PROC PRINT DATA=ACH_OF_TOT_LCL label noobs;
VAR		BOR_CT
		LA_CUR_PRI
		BOR_CT_ACH
		LA_CUR_PRI_ACH
		CT_PCT
		BAL_PCT;
RUN;

PROC PRINTTO;
RUN;

/*DETAIL FILES*/
/*DATA INREPAYMENT_ALL_DET; SET LEGEND.INREPAYMENT_ALL_DET; RUN;*/
/*DATA INREPAYMENT_ACH_DET; SET LEGEND.INREPAYMENT_ACH_DET; RUN;*/
/**/
/*/*export to Excel spreadsheet*/*/
/*PROC EXPORT DATA= WORK.INREPAYMENT_ALL_DET */
/*            OUTFILE= "T:\SAS\INREPAYMENT_ALL_DET.xlsx" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*/*export to Excel spreadsheet*/*/
/*PROC EXPORT DATA= WORK.INREPAYMENT_ACH_DET */
/*            OUTFILE= "T:\SAS\INREPAYMENT_ACH_DET.xlsx" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
