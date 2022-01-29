%LET TAB = To Sallie Mae;
%LET TAB = To Pheaa;
%LET TAB = To Nelnet;
%LET TAB = To Great Lakes;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

%LET NN = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\NelNetImport.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME NN ODBC &NN ;

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
						LNXX.BF_SSN
						,FSXX.LF_FED_AWD || FSXX.LN_FED_AWD_SEQ AS AWD_ID
					FROM
						PKUB.LNXX_LON LNXX
						LEFT JOIN (
								SELECT
									AYXX.BF_SSN
								FROM
									PKUB.AYXX_ATY_TXT AYXX
									INNER JOIN PKUB.AYXX_BR_LON_ATY AYXX
										ON AYXX.BF_SSN = AYXX.BF_SSN
										AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
								WHERE
									AYXX.PF_REQ_ACT = 'AXDCV'
									AND
									AYXX.LX_ATY = 'PSLF TRANSFER'
									) NEST
							ON LNXX.BF_SSN = NEST.BF_SSN
						INNER JOIN PKUB.FSXX_DL_LON FSXX
							ON LNXX.BF_SSN = FSXX.BF_SSN
							AND LNXX.LN_SEQ = FSXX.LN_SEQ
					WHERE
						LNXX.LC_FED_PGM_YR = 'LNC'
						AND
						LNXX.LC_SST_LONXX != 'X'
						AND
						NEST.BF_SSN IS NULL

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
