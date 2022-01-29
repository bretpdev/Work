%LET RPTLIB = T:\SAS;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;

RSUBMIT;

%LET DB = DNFPUTDL; /*live*/
/*%LET DB = DLGSWQUT; /*test*/
LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%LET SCHOOL = 'XXXXXXXX';

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
CREATE TABLE BORROWER AS
	SELECT *
	FROM CONNECTION TO DBX 
		(
		SELECT 
			LNXX.BF_SSN
			,LNXX.LN_SEQ
		FROM 
			PKUB.LNXX_LON LNXX
		WHERE 
			LNXX.LF_DOE_SCL_ORG = &SCHOOL
		)
;
DISCONNECT FROM DBX;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

ENDRSUBMIT;

DATA BORROWER;
	SET LEGEND.BORROWER;
RUN;

PROC EXPORT
		DATA=BORROWER
		OUTFILE="&RPTLIB\CNH XXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="Borrowers";
RUN;
