/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";

PROC IMPORT OUT = WORK.SOURCE
            DATAFILE = "T:NH 23374.xlsx" 
            DBMS = xlsx REPLACE;
   			SHEET = 'Raw Data'; 
RUN;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

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
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						LN40.BF_SSN				AS SSN,
						LN40.LN_SEQ				AS Loan_Sequence,
						LN10.IF_GTR				AS GUARANTOR,
						LN40.LN_SEQ_CLM_PCL 	AS CLAIM_NUMBER
					FROM	
						OLWHRM1.LN40_LON_CLM_PCL LN40
						LEFT JOIN OLWHRM1.LN10_LON_EOM LN10
							ON LN40.BF_SSN = LN10.BF_SSN
							AND LN40.LN_SEQ = LN10.LN_SEQ
/*						LEFT JOIN OLWHRM1.LN35_LON_OWN LN35*/
/*							ON LN40.BF_SSN = LN35.BF_SSN*/
/*							AND LN40.LN_SEQ = LN35.LN_SEQ*/
					WHERE
						DAYS(LN40.LD_SBM_CLM_PCL) BETWEEN DAYS('5/29/2014') AND DAYS('5/31/2015')
						AND (LN40.LC_TYP_REC_CLP_LON = 1 OR LN40.LC_TYP_REC_CLP_LON  = 6)
						AND LN10.IF_GTR IN  ('000706','000708','000730','000731','000751','000755','000800','000927','000951')
	

					FOR READ ONLY WITH UR
				)
	;
QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;

PROC SQL;
	CREATE TABLE TEMP AS 
		SELECT
			D.* 
		FROM
			DEMO D
		INNER JOIN SOURCE S
			ON D.SSN = S.SSN
			AND D.LOAN_SEQUENCE = S.LOAN_SEQUENCE
;
QUIT;

/*PROC SQL;*/
/*	CREATE TABLE COUNTS AS */
/*		SELECT DISTINCT*/
/*			SSN,*/
/*			LOAN_SEQUENCE,*/
/*			COUNT(CLAIM_NUMBER) AS NUMBER_OF_CLAIMS*/
/*		FROM*/
/*			TEMP*/
/*		GROUP BY*/
/*			SSN,*/
/*			LOAN_SEQUENCE*/
/*;*/
/*QUIT;*/
/**/
/*PROC EXPORT DATA = COUNTS */
/*            OUTFILE = "T:\SAS\NH 23374_CLAIM_COUNT.xlsx" */
/*            DBMS = EXCEL*/
/*			REPLACE;*/
/*     SHEET="CLAIM COUNTS"; */
/*RUN;*/


PROC EXPORT DATA = TEMP 
            OUTFILE = "T:\SAS\NH 23374_CLAIM_COUNT_WITH_GUARANTOR .xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RAW DATA"; 
RUN;
