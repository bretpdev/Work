/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/CNHXXXX.RZ";
FILENAME REPORTX "&RPTLIB/CNHXXXX.RX";

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
			PKUB.AYXX_BR_LON_ATY AYXX
		WHERE 
			AYXX.PF_REQ_ACT = 'APUPD'
			AND AYXX.PF_RSP_ACT = 'COMPL'
			AND DAYS(AYXX.LD_ATY_RSP) = DAYS('XX/XX/XXXX')
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
		SHEET="CNH_XXXX";
RUN;
