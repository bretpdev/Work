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
						LN40.LN_SEQ				AS Loan_Sequence,
						LN40.LA_SBM_CLM_PCL_PRI	AS Principal,
						LN40.LA_SBM_CLM_PCL_INT	AS Interest,
						LN40.LD_SBM_CLM_PCL		AS Date_Claim_Created,
						LN10.IF_GTR				AS Guarntor
					FROM	
						OLWHRM1.LN40_LON_CLM_PCL LN40
						LEFT JOIN OLWHRM1.LN10_LON_EOM LN10
							ON LN40.BF_SSN = LN10.BF_SSN
							AND LN40.LN_SEQ = LN10.LN_SEQ
					WHERE
						DAYS(LN40.LD_SBM_CLM_PCL) > DAYS('7/1/2015')
						AND DAYS(LN40.LD_SBM_CLM_PCL) < DAYS('7/31/2015')
						AND (LN40.LC_TYP_REC_CLP_LON = 1 OR LN40.LC_TYP_REC_CLP_LON  = 6)
						AND LN10.IF_GTR = '000749'

					FOR READ ONLY WITH UR
				)
	;
QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;

PROC SORT DATA=DEMO; BY SSN LOAN_SEQUENCE; RUN;

PROC EXPORT DATA = DEMO 
            OUTFILE = "T:\SAS\NH 24023.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
