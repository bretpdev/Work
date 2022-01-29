PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\August 27 rehab_dataDump.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Sheet1$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

DATA SOURCE2;
	SET SOURCE;
	DF_SPE_ACC_ID = PUT(ACCOUNT__,Z10.);
	FORMAT DF_SPE_ACC_ID $10.;
	KEEP DF_SPE_ACC_ID NAME;
	WHERE ACCOUNT__ IS NOT NULL;
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

DATA DUSTER.SOURCE2;
	SET SOURCE2;
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
	CREATE TABLE DEMO AS
		SELECT DISTINCT
			S.DF_SPE_ACC_ID
			,S.NAME
			,DC01.AF_APL_ID || DC01.AF_APL_ID_SFX AS UNIQUE_ID
			,DC01.LA_PAY_XPC AS INSTALL_AMT
			,DC01.LC_BIL_STA AS BILL_CODE
		FROM	
			SOURCE2 S
			LEFT JOIN OLWHRM1.PD01_PDM_INF PD01
				ON S.DF_SPE_ACC_ID = PD01.DF_SPE_ACC_ID
			LEFT JOIN OLWHRM1.DC01_LON_CLM_INF DC01
				ON PD01.DF_PRS_ID = DC01.BF_SSN 
/*				AND DC01.LC_STA_DC10 = '04'*/
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;
	
/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH 20746.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
