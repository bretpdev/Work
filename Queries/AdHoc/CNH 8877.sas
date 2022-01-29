/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

PROC IMPORT DATAFILE="T:\January XXXX Closures.xlsx"
     OUT=JANXXXX
     DBMS=EXCEL
     REPLACE;
     GETNAMES=YES;
RUN;

PROC IMPORT DATAFILE="T:\December XXXX Closures.xlsx"
     OUT=DECXXXX
     DBMS=EXCEL
     REPLACE;
     GETNAMES=YES;
RUN;

PROC IMPORT DATAFILE="T:\November XXXX Closures.xlsx"
     OUT=NOVXXXX
     DBMS=EXCEL
     REPLACE;
     GETNAMES=YES;
RUN;

PROC IMPORT DATAFILE="T:\October XXXX Closures.xlsx"
     OUT=OCTXXXX
     DBMS=EXCEL
     REPLACE;
     GETNAMES=YES;
RUN;

DATA WORK.CONCAT;
	SET JANXXXX DECXXXX NOVXXXX OCTXXXX;
	IF OPE_ID ='' THEN DELETE;
	IF SUBSTR(OPE_ID,X,X)= '`' THEN OPE_ID = SUBSTR(OPE_ID,X);
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;

DATA LEGEND.CONCAT; *Send data to Legend;
SET CONCAT;
RUN;

RSUBMIT;

%LET DB = DNFPUTDL; /*live*/
/*%LET DB = DLGSWQUT; /*test*/
LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

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
CREATE TABLE BORROWERS AS
	SELECT DISTINCT
		LNXX.BF_SSN
		,LNXX.LN_SEQ
		,LNXX.LF_DOE_SCL_ORG
/*		,C.OPE_ID*/
/*		,C.NAME*/
/*		,CLOSURE_DATE*/
	FROM
		WORK.CONCAT C
		INNER JOIN PKUB.LNXX_LON LNXX
			ON INPUT(C.OPE_ID,X.) = INPUT(LNXX.LF_DOE_SCL_ORG,X.)
;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

ENDRSUBMIT;

DATA BORROWERS;
	SET LEGEND.BORROWERS;
RUN;

PROC EXPORT
		DATA=BORROWERS
		OUTFILE="&RPTLIB\UNH XXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
RUN;
