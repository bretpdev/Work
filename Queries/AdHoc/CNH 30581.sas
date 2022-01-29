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
					LNXX.BF_SSN
					,LNXX.LD_CRT_LONXX
					,AYXX.LD_ATY_REQ_RCV
				FROM
					PKUB.LNXX_LON LNXX
					INNER JOIN 
					(
						SELECT
							LNXX.BF_SSN
							,MAX(LNXX.LD_CRT_LONXX) AS LD_CRT_LONXX
						FROM
							PKUB.LNXX_LON_RPS LNXX
						WHERE
							LNXX.LC_TYP_SCH_DIS = 'IL'
							AND LNXX.LC_STA_LONXX = 'A'
							AND DAYS(LNXX.LD_CRT_LONXX) BETWEEN DAYS('XX/XX/XXXX') AND DAYS('XX/XX/XXXX')
						GROUP BY 
							LNXX.BF_SSN
					) LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
					LEFT JOIN  
					(
						SELECT
							AYXX.BF_SSN
							,MAX(AYXX.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
						FROM 
							PKUB.AYXX_BR_LON_ATY AYXX
						WHERE 
							AYXX.PF_REQ_ACT IN ('IDRPR','CODPA','IBRDF','CODCA') 
						GROUP BY 
							AYXX.BF_SSN
					) AYXX
						ON LNXX.BF_SSN = AYXX.BF_SSN
				WHERE
					LNXX.LA_CUR_PRI > X
					AND LNXX.LC_STA_LONXX = 'R'

				FOR READ ONLY WITH UR
			)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; 
	SET LEGEND.DEMO; 
RUN;

DATA RETURN NO_RETURN;
	SET DEMO;
	IF LD_ATY_REQ_RCV > LD_CRT_LONXX THEN OUTPUT RETURN;
	ELSE OUTPUT NO_RETURN;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.NO_RETURN
            OUTFILE = "T:\SAS\CNH XXXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="NO_RETURN"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.RETURN
            OUTFILE = "T:\SAS\CNH XXXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RETURN"; 
RUN;
