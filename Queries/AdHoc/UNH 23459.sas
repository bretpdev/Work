/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
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
						LN40.LN_SEQ				AS LOAN_SEQUENCE,
						LN10.IF_GTR				AS GUARANTOR 
					FROM	
						OLWHRM1.LN40_LON_CLM_PCL LN40
						LEFT JOIN OLWHRM1.LN10_LON_EOM LN10
							ON LN40.BF_SSN = LN10.BF_SSN
							AND LN40.LN_SEQ = LN10.LN_SEQ
					WHERE
						DAYS(LN40.LD_SBM_CLM_PCL) BETWEEN DAYS('07/01/2014') AND DAYS(CURRENT_DATE)
						AND (LN40.LC_TYP_REC_CLP_LON = 1 OR LN40.LC_TYP_REC_CLP_LON  = 6)
					ORDER BY
						LN10.IF_GTR
						
					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;

PROC SQL;
	CREATE TABLE COUNTS AS 
		SELECT
			'TOTAL NUMBER OF BORROWERS:' AS BWRS,
			COUNT(DISTINCT SSN) AS SSN_COUNT,
			'TOTAL NUMBER OF LOANS:' AS LOANS,
			COUNT(*) AS LOAN_COUNT
		FROM
			DEMO
;
QUIT;

PROC EXPORT DATA = WORK.COUNTS 
            OUTFILE = "T:\NH 23459.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="CLAIM_COUNTS"; 
RUN;

PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH 23459.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RAW_DATA"; 
RUN;
