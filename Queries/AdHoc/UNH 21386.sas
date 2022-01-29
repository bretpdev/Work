PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\IBR ABEND Data Dump.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Sheet1$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

DATA DUSTER.SOURCE;
	SET SOURCE;
RUN;

RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

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

	CREATE TABLE LN65 AS
		SELECT 
			A.*
		FROM 
			SOURCE S
			INNER JOIN OLWHRM1.LN65_LON_RPS A
				ON S.SSN = A.BF_SSN
	;

	CREATE TABLE RS05 AS
		SELECT 
			A.*
		FROM 
			SOURCE S
			INNER JOIN OLWHRM1.RS05_IBR_RPS A
				ON S.SSN = A.BF_SSN
	;

	CREATE TABLE RS10 AS
		SELECT 
			A.*
		FROM 
			SOURCE S
			INNER JOIN OLWHRM1.RS10_BR_RPD A
				ON S.SSN = A.BF_SSN
	;

	CREATE TABLE RS20 AS
		SELECT 
			A.*
		FROM 
			SOURCE S
			INNER JOIN OLWHRM1.RS20_IBR_IRL_LON A
				ON S.SSN = A.BF_SSN
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

%MACRO PULL (TAB);
DATA &TAB;
	SET DUSTER.&TAB;
RUN;
%MEND PULL;
%PULL (LN65);
%PULL (RS05);
%PULL (RS10);
%PULL (RS20);

/*export to Excel spreadsheet*/
%MACRO EXP (TABLE);
PROC EXPORT DATA = WORK.&TABLE
            OUTFILE = "T:\SAS\EXCEL OUTPUT.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="&TABLE"; 
RUN;
%MEND EXP;
%EXP (LN65);
%EXP (RS05);
%EXP (RS10);
%EXP (RS20);
