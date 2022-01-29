/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/NH25823.RZ";
FILENAME REPORT2 "&RPTLIB/NH25823.R2";

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ; /*test*/
RSUBMIT;

%LET DB = DLGSUTWH; /*live*/
/*%LET DB = DLGSWQUT; /*test*/
/*LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1; needed for SQL queries, but not for DB2 queries*/

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
CREATE TABLE GAO_COM AS
	SELECT 
		*
	FROM CONNECTION TO DB2 
		(
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID AS Borrower_Acct_No,
			LN72.LD_ITR_EFF_BEG AS Military_Interest_Rate_Start,
			LN72.LD_ITR_EFF_END AS Military_Interest_Rate_End
		FROM
			OLWHRM1.PD10_PRS_NME PD10
			INNER JOIN OLWHRM1.LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			INNER JOIN OLWHRM1.LN72_INT_RTE_HST LN72
				ON LN72.BF_SSN = LN10.BF_SSN
				AND LN72.LN_SEQ = LN10.LN_SEQ
		WHERE
			LN72.LC_INT_RDC_PGM = 'M' /*military interest rate reduction program*/
			AND LN72.LC_STA_LON72 = 'A' /*active rows*/
			AND (DAYS(LN72.LD_ITR_EFF_BEG) BETWEEN DAYS('10-01-2008') AND DAYS('12-31-2015')
				OR DAYS(LN72.LD_ITR_EFF_END) BETWEEN DAYS('10-01-2008') AND DAYS('12-31-2015'))
/*			LN72.LD_ITR_EFF_BEG >= '10/01/2008'*/
/*			AND LN72.LD_ITR_EFF_BEG <= '12/31/2015'*/
		ORDER BY
			LN72.LD_ITR_EFF_BEG,
			LN72.LD_ITR_EFF_END
		)
;
DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  
%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA GAO_COM;
	SET DUSTER.GAO_COM;
RUN;

PROC EXPORT
		DATA=GAO_COM
		OUTFILE="&RPTLIB\UNH 25823.csv"
		DBMS = CSV
		REPLACE;
RUN;
