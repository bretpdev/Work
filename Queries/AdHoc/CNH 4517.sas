%MACRO SOURCE (NAME,TAB);
PROC IMPORT OUT= WORK.&NAME
            DATAFILE= "T:\Cornerstone Transfers.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="&TAB$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;
%MEND SOURCE;
%SOURCE(SALLIE, To Sallie Mae);
%SOURCE(PHEAA, To Pheaa);
%SOURCE(NELNET, To Nelnet);
%SOURCE(GLAKES, To Great Lakes);

%MACRO ASSIGN (FILE,TIVA);
DATA &FILE;
	SET &FILE;
	TIVA = &TIVA;
RUN;
%MEND ASSIGN;
%ASSIGN(SALLIE, 'Sallie Mae');
%ASSIGN(PHEAA, 'Pheaa');
%ASSIGN(NELNET, 'Nelnet');
%ASSIGN(GLAKES, 'Great Lakes');

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.COMBO;
	SET GLAKES SALLIE PHEAA NELNET;
	BF_SSN = STRIP(COMPRESS(BORROWER_SSN,'-'));
	FORMAT BF_SSN $X.;
	KEEP BF_SSN TIVA;
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
						C.BF_SSN
						,C.TIVA
					FROM
						COMBO C
						INNER JOIN PKUB.LNXX_LON_RPS LNXX
							ON C.BF_SSN = LNXX.BF_SSN
					WHERE
						LNXX.LC_TYP_SCH_DIS = 'CQ'
/*						AND LNXX.LC_STA_LONXX = 'A'*/
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX - &SYSDATE..xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
