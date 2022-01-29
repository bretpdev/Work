/*%LET TAB = To Sallie Mae;*/
/*%LET TAB = To Pheaa;*/
/*%LET TAB = To Nelnet;*/
%LET TAB = To Great Lakes;

PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\Cornerstone Transfers.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="&TAB$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.SOURCE;
	SET SOURCE;
	BF_SSN = STRIP(COMPRESS(BORROWER_SSN,'-'));
	FORMAT BF_SSN $X.;
	KEEP BF_SSN;
RUN;

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

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
					SELECT DISTINCT
						S.BF_SSN
/*						,FSXX.LF_FED_AWD || FSXX.LN_FED_AWD_SEQ AS AWD_ID*/
/*						,LNXX.LD_LON_GTR*/
/*						,LNXO.LC_FED_PGM_YR*/
						,LON.AWD_ID
						,LON.LD_LON_GTR
						,LON.LC_FED_PGM_YR
						,PDXX.DM_PRS_LST
						,PDXX.DM_PRS_X
					FROM
						SOURCE S
						LEFT JOIN (
								SELECT
									A.BF_SSN
									,B.LF_FED_AWD || PUT(B.LN_FED_AWD_SEQ, ZX.) AS AWD_ID
									,A.LD_LON_GTR
									,A.LC_FED_PGM_YR
								FROM
									PKUB.LNXX_LON A
									INNER JOIN PKUB.FSXX_DL_LON B
										ON A.BF_SSN = B.BF_SSN
										AND A.LN_SEQ = B.LN_SEQ
									) LON
							ON S.BF_SSN = LON.BF_SSN
/*						LEFT JOIN PKUB.LNXX_LON LNXX*/
/*							ON S.BF_SSN = LNXX.BF_SSN*/
/*						LEFT JOIN PKUB.FSXX_DL_LON FSXX*/
/*							ON S.BF_SSN = FSXX.BF_SSN*/
						LEFT JOIN PKUB.PDXX_PRS_NME PDXX
							ON S.BF_SSN = PDXX.DF_PRS_ID

	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX - &TAB..xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
