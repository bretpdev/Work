%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME AES DBX DATABASE=&DB OWNER=AES;

/*%MACRO SQLCHECK ;
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
%MEND  ;*/

PROC SQL;

	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE TEST AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
				SELECT 
					*
				FROM
					AES.PHXX_CNC_EML PHXX
				WHERE
					PHXX.DF_LST_DTS_PHXX >= 'XX/XX/XXXX'
					FOR READ ONLY WITH UR
				);
   CREATE TABLE TESTX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
				SELECT 
					*
				FROM
					AES.PHXX_CNC_EML_HST PHXX
				WHERE
					PHXX.DF_CRT_DTS_PHXX >= 'XX/XX/XXXX'
					FOR READ ONLY WITH UR
				);
	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;
	%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA TEST; SET LEGEND.TEST; RUN;
DATA TESTX; SET LEGEND.TESTX; RUN;
/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.TEST 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;

PROC EXPORT DATA = WORK.TESTX 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
