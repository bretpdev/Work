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

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID,
						LNXX.BF_SSN,
						LNXX.LN_SEQ,
						LNXX.LA_CUR_PRI,
						DWXX.WA_TOT_BRI_OTS,
						LNXX.IF_LON_SRV_DFL_LON,
						FSXX.LF_FED_AWD,
						FSXX.LN_FED_AWD_SEQ
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON LNXX.BF_SSN = DWXX.BF_SSN
							AND LNXX.LN_SEQ = DWXX.LN_SEQ
						JOIN PKUB.FSXX_DL_LON FSXX
							ON LNXX.BF_SSN = FSXX.BF_SSN
							AND LNXX.LN_SEQ = FSXX.LN_SEQ
						JOIN PKUB.LNXX_RPD_PIO_CVN LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ 
					WHERE
						LNXX.LC_STA_LONXX = 'L'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

DATA DEMO (DROP = LF_FED_AWD LN_FED_AWD_SEQ);
	SET LEGEND.DEMO;
	AWARD_ID = LF_FED_AWD||PUT(LN_FED_AWD_SEQ, ZX.);
RUN;

PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.xls" 
            DBMS = EXCEL
			REPLACE;
RUN;
