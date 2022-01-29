/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS89.NWS89RZ";
FILENAME REPORT2 "&RPTLIB/UNWS89.NWS89R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PROGREVW '/sas/whse/progrevw';

DATA _NULL_;
/*	SET PROGREVW.lastrun_jobs;*/
/*	WHERE JOB = 'UTNWS89';*/
/*Easy way to change date for dev/test purposes*/
	LAST_RUN= today() - 7;
	CALL SYMPUTX('LASTRUN',"'"||PUT(LAST_RUN,MMDDYY10.)||"'" );
RUN;
%PUT >>>LASTRUN=&LASTRUN;
%LET ARC = 'EASYP';

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE POP AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						COALESCE(ARC_USER.LF_USR_REQ_ATY, 'NO AGENT ID FOUND') AS AgentID,
						PD10.DF_SPE_ACC_ID AS AccountNumber,
						ARC_ADD.ARC_DATE AS ARCAddDate,
						BR30.BD_EFT_STA AS ACHAddDate
					FROM
						PKUB.LN10_LON LN10
						INNER JOIN PKUB.PD10_PRS_NME PD10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						INNER JOIN PKUB.BR30_BR_EFT BR30
							ON LN10.BF_SSN = BR30.BF_SSN
						LEFT JOIN /*GETS THE LAST TIME AN ARC WAS DROPED*/
						(
							SELECT DISTINCT
								AY10.BF_SSN,
								MAX(AY10.LD_ATY_REQ_RCV) AS ARC_DATE
							FROM
								PKUB.AY10_BR_LON_ATY AY10
							WHERE
								AY10.PF_REQ_ACT = &ARC
							GROUP BY 
								AY10.BF_SSN
						)ARC_ADD
							ON ARC_ADD.BF_SSN = LN10.BF_SSN
						LEFT JOIN /*GETS THE LAST USER TO DROP A SPECIFIC ARC*/
						(
							SELECT DISTINCT
								AY10.BF_SSN,
								AY10.LF_USR_REQ_ATY,
								MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
							FROM
								PKUB.AY10_BR_LON_ATY AY10
							WHERE
								AY10.PF_REQ_ACT = &ARC
							GROUP BY 
								AY10.BF_SSN,
								AY10.LF_USR_REQ_ATY
								
						)ARC_USER
							ON ARC_USER.BF_SSN = LN10.BF_SSN
							AND ARC_USER.LD_ATY_REQ_RCV <= BR30.BD_EFT_STA
					WHERE
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'
						AND BR30.BC_EFT_STA = 'A'
						AND BR30.BD_EFT_STA >= &LASTRUN

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

/*DATA PROGREVW.LASTRUN_JOBS;*/
/*SET PROGREVW.LASTRUN_JOBS;*/
/*IF JOB = 'UTNWS89' THEN LAST_RUN = TODAY();*/
/*RUN;*/

ENDRSUBMIT;

DATA POP; SET LEGEND.POP; RUN;

DATA _NULL_;
	SET		WORK.POP;
	FILE	REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;
	format ACHAddDate mmddyy10.;
	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
		DO;
			PUT	
				'AgentID'
				','
				'Account#'
				','
				'ARCAddDate'
				','
				'ACHAddDate'
			;
		END;

	/* write data*/	
	DO;
		PUT AgentID $ @;
		PUT AccountNumber $ @;
		PUT ARCAddDate $ @;
		PUT ACHAddDate $;
		;
	END;
RUN;
