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

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE LNXX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT 
						PDXX.DF_SPE_ACC_ID,
						LNXX.*
					FROM
						PKUB.LNXX_INT_RTE_HST LNXX
						INNER JOIN PKUB.PDXX_PRS_NME PDXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
					WHERE
						PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX')
						
					FOR READ ONLY WITH UR
				)
	;
	CREATE TABLE ADXX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT 
						PDXX.DF_SPE_ACC_ID,
						ADXX.*
					FROM
						PKUB.ADXX_PCV_ATY_ADJ ADXX
						INNER JOIN PKUB.PDXX_PRS_NME PDXX
							ON PDXX.DF_PRS_ID = ADXX.BF_SSN
					WHERE
						PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX')

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA LNXX; SET LEGEND.LNXX; RUN;
DATA ADXX; SET LEGEND.ADXX; RUN;


/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.LNXX 
            OUTFILE = "T:\SAS\CNH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LNXX"; 
RUN;

PROC EXPORT DATA = WORK.ADXX 
            OUTFILE = "T:\SAS\CNH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="ADXX"; 
RUN;

