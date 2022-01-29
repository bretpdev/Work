PROC IMPORT OUT= WORK.SAUCE
            DATAFILE= "T:\Corner Stone REPAYE Email Campaign.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
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
	SET SAUCE;
	FORMAT BF_SSN $X.;
	BF_SSN = PUT(SSN, zX.);
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
		SELECT	
			S.BF_SSN
			,CASE
				WHEN AY.BF_SSN IS NOT NULL THEN 'X'
				ELSE 'X'
			 END AS CONTACT
		FROM SOURCE S
			LEFT JOIN 
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						AYXX.BF_SSN
					FROM
						PKUB.AYXX_BR_LON_ATY AYXX
					WHERE 
						AYXX.PF_REQ_ACT = 'PXXXC'
						AND DAYS(AYXX.LD_ATY_RSP) BETWEEN DAYS('XX-XX-XXXX') AND DAYS('XX-XX-XXXX')
						AND AYXX.PF_RSP_ACT = 'CNTCT'

					FOR READ ONLY WITH UR
				) AY
			ON S.BF_SSN = AY.BF_SSN
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\CNH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
