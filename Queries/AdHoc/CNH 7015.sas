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

	CREATE TABLE LTXX_AYXX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT
						LTXX.RM_APL_PGM_QUE,
						LTXX.RT_RUN_SRT_DTS_QUE,
						LTXX.RN_SEQ_LTR_CRT_QUE,
						LTXX.RN_SEQ_REC_QUE,
						LTXX.RM_DSC_LTR_QUE,
						LTXX.RI_LTR_REQ_DEL_QUE,
						AYXX.BF_SSN,
						AYXX.LN_ATY_SEQ,
						AYXX.LC_STA_ACTYXX,
						AYXX.LD_ATY_RSP,
						AYXX.PF_RSP_ACT
					FROM
						PKUB.LTXX_LTR_REQ_QUE LTXX
						JOIN PKUB.AYXX_BR_LON_ATY AYXX
							ON LTXX.RF_SBJ_QUE = AYXX.BF_SSN 
                            AND LTXX.RN_ATY_SEQ_QUE = AYXX.LN_ATY_SEQ
					WHERE
						LTXX.RM_APL_PGM_QUE = 'TLXIO'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA LTXX_AYXX; SET LEGEND.LTXX_AYXX; RUN;

DATA LTXX_AYXX_DATE;
	SET LTXX_AYXX;
	date = put(datepart(RT_RUN_SRT_DTS_QUE), yymmddXX.);
	hour = hour(RT_RUN_SRT_DTS_QUE);
	if hour < XX then
		hourX= cats('X',compress(hour));
	if hourX = . then
		hourX = compress(hour);

	minute = minute(RT_RUN_SRT_DTS_QUE);

	if minute < XX then
		minuteX= cats('X',compress(minute));
	if minuteX = . then
		minuteX = compress(minute);

	sec = round(second(RT_RUN_SRT_DTS_QUE), .XXXXXXX);

	trailzero= substr(compress(sec)||"XXXXXXXXX",X,X);

	val = cats(date, "-", hourX,".",minuteX,".", trailzero);
RUN;

PROC SQL;
	CREATE TABLE LTXX AS
		SELECT
			RM_APL_PGM_QUE,
			val,
			RN_SEQ_LTR_CRT_QUE,
			RN_SEQ_REC_QUE,
			RM_DSC_LTR_QUE,
			RI_LTR_REQ_DEL_QUE,
			BF_SSN,
			LN_ATY_SEQ,
			LC_STA_ACTYXX,
			LD_ATY_RSP,
			PF_RSP_ACT
		FROM
			LTXX_AYXX_DATE
	;
quit;


/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.LTXX
            OUTFILE = "T:\SAS\LTXX_AYXX_DUMP.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
