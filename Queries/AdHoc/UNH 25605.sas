PROC IMPORT OUT= WORK.Sauce
            DATAFILE= "T:/Borrowers with OID in EA27.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Sheet1$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

DATA SOURCE;
	SET SAUCE;
	SSN = PUT(BORROWERSSN, z9.);
	KEEP SSN;
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWS65.LWS65RZ";
FILENAME REPORT2 "&RPTLIB/ULWS65.LWS65R2";

LIBNAME  QADBD004  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;

DATA QADBD004.SOURCE;
	SET SOURCE;
	BF_SSN = INPUT(SSN, $9.);
	KEEP BF_SSN;
RUN;

RSUBMIT QADBD004;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSWQUT OWNER=OLWHRM1;

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
	CONNECT TO DB2 (DATABASE=DLGSWQUT);

	CREATE TABLE LN09 AS
		SELECT 
			LN09.*
		FROM SOURCE S
			JOIN OLWHRM1.LN09_RPD_PIO_CVN LN09
				ON S.BF_SSN = LN09.BF_SSN
	;

	CREATE TABLE MR64 AS
		SELECT 
			MR64.*
		FROM SOURCE S
			JOIN OLWHRM1.MR64_BR_TAX MR64 
				ON S.BF_SSN = MR64.BF_SSN
	;

	CREATE TABLE MR68 AS
		SELECT 
			MR68.*
		FROM SOURCE S
			JOIN OLWHRM1.MR68_TAX_RPT_NOT MR68
				ON S.BF_SSN = MR68.BF_SSN
	;

	DISCONNECT FROM DB2;

QUIT;

ENDRSUBMIT;

DATA LN09;
	SET QADBD004.LN09;
RUN;

DATA MR64;
	SET QADBD004.MR64;
RUN;

DATA MR68;
	SET QADBD004.MR68;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.LN09 
            OUTFILE = "T:\SAS\NH 25605 v2.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN09"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.MR64 
            OUTFILE = "T:\SAS\NH 25605 v2.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="MR64"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.MR68 
            OUTFILE = "T:\SAS\NH 25605 v2.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="MR68"; 
RUN;
