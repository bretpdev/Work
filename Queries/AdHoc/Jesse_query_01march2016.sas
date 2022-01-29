/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBDXXX SLIBREF=WORK ; /*test*/
RSUBMIT;

%LET DB = DLGSUTWH; /*live*/
/*%LET DB = DLGSWQUT; /*test*/

/*Report level variables*/
%LET BFSSN = 'XXXXXXXXX';

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
CONNECT TO DBX (DATABASE=&DB); 
CREATE TABLE QUERY AS
SELECT 
	*
FROM CONNECTION TO DBX 
	(
		SELECT DISTINCT
			LNXX.LD_NPD_PCV
		FROM
			OLWHRMX.LNXX_RPD_PIO_CVN LNXX
		WHERE
			LNXX.BF_SSN = &BFSSN
	)
;
DISCONNECT FROM DBX;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  
%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA QUERY;
	SET DUSTER.QUERY;
RUN;

PROC PRINT DATA=QUERY;
RUN;

/*PROC EXPORT*/
/*		DATA=QUERY*/
/*		OUTFILE="&RPTLIB\JESSE_QUERY.xlsx"*/
/*		DBMS = EXCEL*/
/*		REPLACE;*/
/*		SHEET="OUTPUT";*/
/*RUN;*/
