/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
%let DB = DNFPRQUT;  *This is test;
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
/*%let DB = DNFPUTDL;  *This is live;*/

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

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN
						,LNXX.LN_SEQ
						,FRXX.LD_INT_RPT_EOM_PRC
						,FRXX.LN_INT_EOM_PRC_SEQ
						,FRXX.LA_FMS_MTH_INT_ACR
						,FRXX.LA_FMS_MTH_INT_ADJ
						,FRXX.LC_MTH_INT_RPT_STA
						,FRXX.LF_LST_DTS_FRXX
						,FRXX.WD_FMS_RPT
						,FRXX.WN_FMS_RPT_SEQ
						,FRXX.WD_FMS_RPT_ADJ
						,FRXX.WN_FMS_RPT_SEQ_ADJ
						,LNXX.LF_FED_CLC_RSK
					FROM
						PKUB.LNXX_LON LNXX
						LEFT JOIN PKUR.FRXX_MTH_INT_RPT FRXX
							ON LNXX.BF_SSN = FRXX.BF_SSN
							AND LNXX.LN_SEQ = FRXX.LN_SEQ
					WHERE
						(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = X)
						OR 
						(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = X)
						OR
						(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = X)
						OR
						(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = X)
						OR
						(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = X)

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
