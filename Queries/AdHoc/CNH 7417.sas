/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

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

PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE Def AS
		SELECT DISTINCT
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID,
						LNXX.LN_SEQ,
						DFXX.LC_DFR_TYP
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_LON LNXX
							ON LNXX.BF_SSN = PDXX.DF_PRS_ID
						INNER JOIN PKUB.DFXX_BR_DFR_REQ DFXX
							ON DFXX.BF_SSN = PDXX.DF_PRS_ID
					WHERE
						DFXX.LD_DFR_REQ_END = 'X/XX/XXXX'

					FOR READ ONLY WITH UR
				)
	;


	CREATE TABLE Forb AS
		SELECT DISTINCT
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID,
						LNXX.LN_SEQ,
						FBXX.LC_FOR_TYP
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_LON LNXX
							ON LNXX.BF_SSN = PDXX.DF_PRS_ID
						INNER JOIN PKUB.FBXX_BR_FOR_REQ FBXX
							ON FBXX.BF_SSN = PDXX.DF_PRS_ID
					WHERE
						FBXX.LD_FOR_REQ_END = 'X/XX/XXXX'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA Def; SET LEGEND.Def; RUN;
DATA Forb; SET LEGEND.Forb; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.Def 
            OUTFILE = "T:\SAS\NH XXXX.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Deferment";
RUN;


/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.Forb 
            OUTFILE = "T:\SAS\NH XXXX.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Forbearance";
RUN;
