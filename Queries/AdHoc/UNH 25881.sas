/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/NH25729.RZ";
FILENAME REPORT2 "&RPTLIB/NH25729.R2";

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/

RSUBMIT;
%LET DB = DLGSUTWH; /*live*/
/*%LET DB = DLGSWQUT; /*test*/
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
CONNECT TO DB2 (DATABASE=&DB); 
CREATE TABLE BORROWER AS
	SELECT *
	FROM CONNECTION TO DB2 
		(
		SELECT 
			*
		FROM 
			OLWHRM1.BR30_BR_EFT BR30
		WHERE 
			BF_EFT_ABA = '054000030'
			AND BF_EFT_ACC = '5501510853'
/*			AND BC_EFT_STA IN ('A','I','D')*/
		)
;
DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

ENDRSUBMIT;

DATA BORROWER;
	SET DUSTER.BORROWER;
RUN;

PROC EXPORT
		DATA=BORROWER
		OUTFILE="&RPTLIB\UNH 25881.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="_BR30";
RUN;
