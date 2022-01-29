/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

PROC IMPORT OUT= WORK.xlsxfile 
            DATAFILE= "Y:\Development\SAS Test Files\Debbie\PLUS Indicator.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.XLSXFILE; SET XLSXFILE; RUN;

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
		SELECT
			D.BF_SSN
			,D.LF_FED_AWD
			,D.LN_FED_AWD_SEQ
			,D.LN_SEQ
			,D.LC_TLX_IBR_ELG
		FROM
			XLSXFILE X
			INNER JOIN CONNECTION TO DBX 
				(
					SELECT	
						DISTINCT
						FSXX.LF_FED_AWD
						,FSXX.LN_FED_AWD_SEQ
						,FSXX.BF_SSN
						,FSXX.LN_SEQ
						,LNXX.LC_TLX_IBR_ELG
					FROM
						PKUB.FSXX_DL_LON FSXX
						INNER JOIN PKUB.LNXX_LON LNXX
							 ON FSXX.BF_SSN = LNXX.BF_SSN
							 AND FSXX.LN_SEQ = LNXX.LN_SEQ

					FOR READ ONLY WITH UR
				) D
				ON X.BorrowerSSN = D.BF_SSN
				AND X.AwardID = D.LF_FED_AWD
				AND X.AwardIDSequence = D.LN_FED_AWD_SEQ
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO; SET LEGEND.DEMO; RUN;

PROC EXPORT DATA= WORK.DEMO 
            OUTFILE= "T:\SAS\PLUS Indicator with LN_SEQ.xlsx" 
            DBMS=EXCEL REPLACE;
     SHEET="A"; 
RUN;
