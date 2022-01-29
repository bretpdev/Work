/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/CHNXXXX.RZ";
FILENAME REPORTX "&RPTLIB/CHNXXXX.RX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;

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
CONNECT TO DBX (DATABASE=&DB); 
CREATE TABLE BORROWER AS
	SELECT *
	FROM CONNECTION TO DBX 
		(
		SELECT 
			*
		FROM 
			PKUB.LNXX_LON LNXX
		WHERE 
/*			LNXX.LF_DOE_SCL_ORG = 'XXXXXXXX'*/
/*			OR LNXX.LF_DOE_SCL_ORG LIKE '%XXXXX%'*/
			LNXX.LF_DOE_SCL_ORG = 'XXXXXXXX'/*correct code for Brensten Education*/
		)
;
DISCONNECT FROM DBX;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

ENDRSUBMIT;

DATA BORROWER;
	SET LEGEND.BORROWER;
RUN;

PROC PRINT DATA=BORROWER;
RUN;

PROC EXPORT
		DATA=BORROWER
		OUTFILE="&RPTLIB\CNH XXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="Brensten";
RUN;
