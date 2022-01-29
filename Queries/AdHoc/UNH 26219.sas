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

/*cumulative defaults on all loans entering repayment subsequent to the start date 
divided by principal amount of all loan entering repayment subsequent to start date.*/

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						LN10.BF_SSN, 
						CAST(SUM(CASE WHEN LN40.BF_SSN IS NULL OR LN90.BF_SSN IS NULL THEN 0 ELSE LN10.LA_LON_AMT_GTR END) AS DOUBLE) DefaultedPrincipal,
						CAST(SUM(LN10.LA_LON_AMT_GTR) AS DOUBLE) TotalPrincipal,
						CAST(SUM(CASE WHEN LN40.BF_SSN IS NULL OR LN90.BF_SSN IS NULL THEN 0 ELSE LN10.LA_LON_AMT_GTR END) AS DOUBLE) / CAST(SUM(LN10.LA_LON_AMT_GTR) AS DOUBLE) AS PercentageOfDefault
					FROM 
						OLWHRM1.LN10_LON LN10
						LEFT OUTER JOIN 
							(
								SELECT DISTINCT 
									BF_SSN,
									LN_SEQ
								FROM
									OLWHRM1.LN40_LON_CLM_PCL 
								WHERE
									LC_REA_CLP_LON = '06'
									AND LC_REA_CAN_CLM_PCL = ''
									AND LC_TYP_REJ_RTN = ''
							)LN40
							ON LN40.BF_SSN = LN10.BF_SSN
							AND LN40.LN_SEQ = LN10.LN_SEQ
						LEFT OUTER JOIN
							(
								SELECT DISTINCT
									BF_SSN,
									LN_SEQ
								FROM 
									OLWHRM1.LN90_FIN_ATY
								WHERE
									PC_FAT_TYP = '10'
									AND PC_FAT_SUB_TYP = '30'
									AND LC_STA_LON90 = 'A'
									AND LC_FAT_REV_REA = ''
							) LN90 ON LN90.BF_SSN = LN10.BF_SSN
							AND LN90.LN_SEQ = LN10.LN_SEQ
						INNER JOIN 
							(
								SELECT DISTINCT 
									BF_SSN,
									LN_SEQ
								FROM 
									OLWHRM1.LN65_LON_RPS
								WHERE
									LC_STA_LON65 = 'A'
							) LN65 ON LN65.BF_SSN = LN10.BF_SSN
							AND LN65.LN_SEQ = LN10.LN_SEQ
						LEFT OUTER JOIN
							(
								SELECT
									BF_SSN,
									LN_SEQ,
									CONCAT(LF_LON_ALT,LPAD(LN_LON_ALT_SEQ,2,'0')) AS CLUID,
									LF_GTR_RFR_XTN
								FROM 
									OLWHRM1.LN10_LON
							) CLUID ON CLUID.BF_SSN = LN10.BF_SSN
							AND CLUID.LN_SEQ <> LN10.LN_SEQ
							AND (CLUID.CLUID = CONCAT(LN10.LF_LON_ALT,LPAD(LN10.LN_LON_ALT_SEQ,2,'0'))
								OR CLUID.LF_GTR_RFR_XTN = LN10.LF_GTR_RFR_XTN)
					WHERE CLUID.BF_SSN IS NULL
					AND LN10.IC_LON_PGM NOT IN ('COMPLT','TILP')
					GROUP BY
						LN10.BF_SSN
					FOR READ ONLY WITH UR
				)
	;
QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;

PROC EXPORT DATA = DEMO 
            OUTFILE = "T:\SAS\Claims.xlsx" 
            DBMS = EXCEL
			REPLACE;
RUN;
