PROC IMPORT OUT= WORK.DLO
            DATAFILE= "T:\MasterXXXXXXXX.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="DLO$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

DATA DLO;
	SET DLO;
	DROP FX FX FX FX FX FX FX;
RUN;

PROC IMPORT OUT= WORK.LNC
            DATAFILE= "T:\MasterXXXXXXXX.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="LNC$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

DATA LNC;
	SET LNC;
	DROP FX FX FX FX FX FX FX;
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.DLO; SET DLO; RUN;
DATA LEGEND.LNC; SET LNC; RUN;

RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
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

PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DLOX AS
		SELECT	
			D.*
			,LNXX.LD_LON_GTR
			,FSXX.LF_FED_AWD
			,FSXX.LN_FED_AWD_SEQ
		FROM	
			DLO D
			LEFT JOIN PKUB.LNXX_LON LNXX
				ON D.BF_SSN = LNXX.BF_SSN
				AND D.LN_SEQ = LNXX.LN_SEQ
			LEFT JOIN PKUB.FSXX_DL_LON FSXX
				ON D.BF_SSN = FSXX.BF_SSN
				AND D.LN_SEQ = FSXX.LN_SEQ
	;

	CREATE TABLE LNCX AS
		SELECT	
			L.*
			,LNXX.LD_LON_GTR
			,FSXX.LF_FED_AWD
			,FSXX.LN_FED_AWD_SEQ
		FROM	
			LNC L
			LEFT JOIN PKUB.LNXX_LON LNXX
				ON L.BF_SSN = LNXX.BF_SSN
				AND L.LN_SEQ = LNXX.LN_SEQ
			LEFT JOIN PKUB.FSXX_DL_LON FSXX
				ON L.BF_SSN = FSXX.BF_SSN
				AND L.LN_SEQ = FSXX.LN_SEQ
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DLOX; SET LEGEND.DLOX; RUN;
DATA LNCX; SET LEGEND.LNCX; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DLOX 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DLO"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.LNCX 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LNC"; 
RUN;
