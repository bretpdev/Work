/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
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
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT	
						DISTINCT
						A.DF_SPE_ACC_ID
/*						,LNXX.LN_SEQ*/
/*						,LNXX.PC_FAT_TYP*/
/*						,LNXX.PC_FAT_SUB_TYP */
/*						,DAYS(CURRENT DATE) - DAYS(LNXX.LD_DLQ_OCC) AS DELQ*/
					FROM
						PKUB.PDXX_PRS_NME A
						INNER JOIN PKUB.LNXX_FIN_ATY LNXX
							ON A.DF_PRS_ID = LNXX.BF_SSN
							AND LNXX.PC_FAT_TYP = 'XX' 
							AND LNXX.PC_FAT_SUB_TYP = 'XX'
						INNER JOIN PKUB.LNXX_LON_DLQ_HST LNXX
							ON A.DF_PRS_ID = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LC_STA_LONXX = 'X'
							AND DAYS(CURRENT DATE) - DAYS(LNXX.LD_DLQ_OCC) >= XXX 

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
PROC EXPORT DATA= WORK.DEMO 
            OUTFILE= "T:\SAS\NH XXXX.xls" 
            DBMS=EXCEL REPLACE;
     SHEET="A"; 
RUN;
