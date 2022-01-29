/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
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

PROC SQL inobs = XXXX;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID
						,LNXX.LN_SEQ
/*						,DFR.LN_SEQ AS DFR*/
/*						,FRB.LN_SEQ AS FRB*/
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						LEFT JOIN (
									SELECT
										A.BF_SSN
										,A.LN_SEQ
									FROM 
										PKUB.LNXX_BR_DFR_APV A
									WHERE 
										A.LC_STA_LONXX = 'A'
										AND A.LD_DFR_END = 'XX/XX/XXXX'
									) DFR
							ON LNXX.BF_SSN = DFR.BF_SSN
							AND LNXX.LN_SEQ = DFR.LN_SEQ
						LEFT JOIN (
									SELECT
										A.BF_SSN
										,A.LN_SEQ
									FROM 
										PKUB.LNXX_BR_FOR_APV A
									WHERE 
										A.LC_STA_LONXX = 'A'
										AND A.LD_FOR_END = 'XX/XX/XXXX'
									) FRB
							ON LNXX.BF_SSN = FRB.BF_SSN
							AND LNXX.LN_SEQ = FRB.LN_SEQ
					WHERE
						DFR.BF_SSN IS NOT NULL OR FRB.BF_SSN IS NOT NULL

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
