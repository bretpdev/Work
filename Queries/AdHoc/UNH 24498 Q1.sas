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
						LN40.LN_SEQ_CLM_PCL 	AS CLAIM_NUMBER,
						LN40.BF_SSN				AS SSN,
						LN40.LN_SEQ				AS Loan_Sequence,
						LN40.LA_SBM_CLM_PCL_PRI	AS Principal,
						LN40.LA_SBM_CLM_PCL_INT	AS Interest,
						LN40.LD_SBM_CLM_PCL		AS Date_Claim_Created,
						LN10.IF_GTR				AS Guarntor
/*						LN35.IF_BND_ISS			AS Bond_Id*/
					FROM	
						OLWHRM1.LN40_LON_CLM_PCL LN40
						LEFT JOIN OLWHRM1.LN10_LON_EOM LN10
							ON LN40.BF_SSN = LN10.BF_SSN
							AND LN40.LN_SEQ = LN10.LN_SEQ
/*						LEFT JOIN OLWHRM1.LN35_LON_OWN LN35*/
/*							ON LN40.BF_SSN = LN35.BF_SSN*/
/*							AND LN40.LN_SEQ = LN35.LN_SEQ*/
					WHERE
						DAYS(LN40.LD_SBM_CLM_PCL) BETWEEN DAYS('09/01/2015') AND DAYS('09/30/2015')
						AND (LN40.LC_TYP_REC_CLP_LON = 1 OR LN40.LC_TYP_REC_CLP_LON  = 6)
						AND LN10.IF_GTR IN  ('000706','000708','000730','000731','000751','000755','000800','000927','000951')
/*						AND LN40.LC_TYP_REJ_RTN = ' '*/

					FOR READ ONLY WITH UR
				)
	;
QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;

PROC SORT DATA=DEMO; BY SSN LOAN_SEQUENCE; RUN;

DATA DEMO2;
	SET DEMO;
	MTH = MONTH(DATE_CLAIM_CREATED);
	YR = YEAR(DATE_CLAIM_CREATED);
RUN;

PROC EXPORT DATA = DEMO2 
            OUTFILE = "T:\SAS\NH 24498_09-01-2015_09-30-2015.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
