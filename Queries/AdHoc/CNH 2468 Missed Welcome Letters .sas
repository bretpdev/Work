/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

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
CREATE TABLE DEMO AS
	SELECT	*
	FROM	CONNECTION TO DBX (
				SELECT 	DISTINCT
						A.BF_SSN,
						A.LD_LON_ACL_ADD
				FROM	PKUB.LNXX_LON A
						INNER JOIN PKUB.AYXX_BR_LON_ATY C
							ON A.BF_SSN = C.BF_SSN
				WHERE  C.PF_REQ_ACT IN( 'WELCC', 'WELCF','WELCA','WELCO','WELCX','WELCD')
						AND A.LD_LON_ACL_ADD = 'XX/XX/XXXX' 
				FOR READ ONLY WITH UR
			);
			DISCONNECT FROM DBX;


PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE DEMOX AS
	SELECT	*
	FROM	CONNECTION TO DBX (
				SELECT 	DISTINCT
						A.BF_SSN,
						A.LD_LON_ACL_ADD
				FROM	PKUB.LNXX_LON A	
				WHERE  A.LD_LON_ACL_ADD = 'XX/XX/XXXX' 
				FOR READ ONLY WITH UR
			);

DISCONNECT FROM DBX;


/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO; SET LEGEND.DEMO; RUN;
DATA DEMOX; SET LEGEND.DEMOX;RUN;

PROC SQL;
CREATE TABLE DEMOX AS 
	SELECT
		A.BF_SSN,
		A.LD_LON_ACL_ADD
	FROM DEMOX A
	WHERE A.BF_SSN NOT IN(SELECT BF_SSN FROM DEMO);


/*export to Excel spreadsheet*/
PROC EXPORT DATA= WORK.DEMOX 
            OUTFILE= "T:\SAS\EXCEL OUTPUT.xls" 
            DBMS=EXCEL REPLACE;
     SHEET="A"; 
RUN;

