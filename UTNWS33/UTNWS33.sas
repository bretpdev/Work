/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS33.NWS33RZ";
FILENAME REPORT2 "&RPTLIB/UNWS33.NWS33R2";
FILENAME REPORT3 "&RPTLIB/UNWS33.NWS33R3";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;
LIBNAME PROGREVW '/sas/whse/progrevw';

DATA _NULL_;
	SET PROGREVW.lastrun_jobs;
	WHERE JOB = 'UTNWS33';
/*Easy way to change date for dev/test purposes*/
/*	last_run= today()-7;*/
	CALL SYMPUTX('LASTRUN',"'"||PUT(LAST_RUN,MMDDYY10.)||"'" );
	CALL SYMPUTX('YESTERDAY',"'"||PUT(TODAY()-1,MMDDYY10.)||"'" );
RUN;
%PUT >>>LASTRUN=&LASTRUN;
%PUT >>>YESTERDAY=&YESTERDAY;

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

	CREATE TABLE DEMO AS
		SELECT DISTINCT
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT	
						DISTINCT
						PD10.DF_SPE_ACC_ID
						,CASE 
							WHEN BL10.LC_BIL_TYP = 'P' THEN 'Installment'
							WHEN BL10.LC_BIL_TYP = 'C' THEN 'Interest Cap'
							WHEN BL10.LC_BIL_TYP = 'I' THEN 'Interest'
							ELSE ' '
						END AS LC_BIL_TYP
						,BL10.LD_BIL_CRT
						,LN80.LN_SEQ
						,LN80.BF_SSN
					FROM
						PKUB.BL10_BR_BIL BL10
						INNER JOIN PKUB.PD10_PRS_NME PD10
							ON BL10.BF_SSN = PD10.DF_PRS_ID 
						INNER JOIN PKUB.LN80_LON_BIL_CRF LN80
							ON BL10.BF_SSN = LN80.BF_SSN
							AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
							AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
					WHERE
						BL10.LC_STA_BIL10 = 'A'
						AND BL10.LD_BIL_CRT BETWEEN &LASTRUN AND &YESTERDAY
						AND BL10.LC_BIL_MTD IN ('2','E')
						AND BL10.LC_IND_BIL_SNT IN ('G','R','0','1','2','7')
					ORDER BY PD10.DF_SPE_ACC_ID
					FOR READ ONLY WITH UR
				)
	;

CREATE TABLE DLQ AS
		SELECT DISTINCT DEMO.DF_SPE_ACC_ID
						,MAX(DLQ.LN_DLQ_MAX) + 1 AS LN_DLQ_MAX
		FROM DEMO 
			INNER JOIN	
			CONNECTION TO DB2 (
				SELECT DISTINCT
					LN16.BF_SSN
					,LN16.LN_SEQ
					,MAX(LN16.LN_DLQ_MAX) AS LN_DLQ_MAX
				FROM	PKUB.LN16_LON_DLQ_HST LN16
				WHERE	LN16.LC_STA_LON16 = '1'
					AND LN16.LC_DLQ_TYP = 'P'
					AND LN16.LD_DLQ_MAX >= &YESTERDAY
				GROUP BY LN16.BF_SSN
						,LN16.LN_SEQ
			) DLQ
				ON DEMO.BF_SSN = DLQ.BF_SSN
				AND DEMO.LN_SEQ = DLQ.LN_SEQ
		GROUP BY DEMO.DF_SPE_ACC_ID		
;


	DISCONNECT FROM DB2;
;
	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

DATA PROGREVW.LASTRUN_JOBS;
SET PROGREVW.LASTRUN_JOBS;
IF JOB = 'UTNWS33' THEN LAST_RUN = TODAY();
RUN;

DATA DEMO2;
	LENGTH LOANS $100.;
   	DO UNTIL (LAST.DF_SPE_ACC_ID);
      SET DEMO;
        BY DF_SPE_ACC_ID;
      LOANS=CATX('_',LOANS,LN_SEQ);
   	END;
   	DROP LN_SEQ;
RUN;

PROC SQL;
	CREATE TABLE FINAL AS
		SELECT DISTINCT
			DEMO2.*
			,DLQ.LN_DLQ_MAX
		FROM DEMO2
			LEFT JOIN DLQ
				ON DEMO2.DF_SPE_ACC_ID = DLQ.DF_SPE_ACC_ID
		ORDER BY DEMO2.DF_SPE_ACC_ID
	;
QUIT;

ENDRSUBMIT;

DATA FINAL;
SET LEGEND.FINAL;
RUN;

/*set up library to SQL Server and include common code*/
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn;';*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no";
%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS.dsn;';

/*TODO change this to use SAS Common code when it is complete*/
%MACRO PROCESS_VARIABLE_COMMENT(DATA,ACCOUNT_NUMBER,COMMENT,RECIPIENT_ID,IS_REFERENCE,IS_ENDORSER,PROCESS_FROM,PROCESS_TO,NEEDED_BY,REGARDS_TO,REGARDS_CODE,ARC_TYPE,ARC,SAS_ID);
	/*update SQL Server CLS.ArcAddProcessing*/
	PROC SQL;
		INSERT INTO
			SQL.ARCADDPROCESSING
				(
					ARCTYPEID
					,ACCOUNTNUMBER
					,RECIPIENTID
					,ARC
					,SCRIPTID
					,PROCESSON
					,COMMENT
					,ISREFERENCE
					,ISENDORSER
					,PROCESSFROM
					,PROCESSTO
					,NEEDEDBY
					,REGARDSTO
					,REGARDSCODE
					,CREATEDAT
					,CREATEDBY
				)
		SELECT
			&ARC_TYPE,
			D.&ACCOUNT_NUMBER,
			D.&RECIPIENT_ID,
			&ARC,
			&SAS_ID,
			TODAY() * 86400, 
			D.&COMMENT,
			D.&IS_REFERENCE,
			D.&IS_ENDORSER,
			D.&PROCESS_FROM,
			D.&PROCESS_TO,
			D.&NEEDED_BY,
			D.&REGARDS_TO,
			D.&REGARDS_CODE,
			TODAY() * 86400,
			&SAS_ID
			
		FROM
			&DATA D
		;
	QUIT;
%MEND PROCESS_VARIABLE_COMMENT;

DATA R2;
	SET FINAL ;
	RECIPIENT_ID = '';
	IS_REFERENCE = 0;
	IS_ENDORSER = 0;
	PROCESS_FROM = .;
	PROCESS_TO = .;
	NEEDED_BY = .;
	REGARDS_TO = '';
	REGARDS_CODE = '';
	COMMENT = LC_BIL_TYP || ' E-Bill sent to borrower ' || PUT(LD_BIL_CRT, MMDDYY10.)  ||' in regards to loan sequence(s) ' || LOANS;
	WHERE LN_DLQ_MAX IS NULL;	
RUN;

/*we are not actually passing the variabes to the marco rather just the names they will just used in a query*/
%PROCESS_VARIABLE_COMMENT(R2, DF_SPE_ACC_ID, COMMENT, RECIPIENT_ID, IS_REFERENCE, IS_ENDORSER, PROCESS_FROM, PROCESS_TO, NEEDED_BY, REGARDS_TO, REGARDS_CODE, 1, 'EBILL','UNWS33.R2');

DATA R3;
	SET FINAL ;
	RECIPIENT_ID = '';
	IS_REFERENCE = 0;
	IS_ENDORSER = 0;
	PROCESS_FROM = .;
	PROCESS_TO = .;
	NEEDED_BY = .;
	REGARDS_TO = '';
	REGARDS_CODE = '';
	COMMENT ='Delinquent E-Bill sent to borrower. Currently' || PUT(LN_DLQ_MAX, BEST12.) || 'days delinquent';
	WHERE LN_DLQ_MAX IS NOT NULL;
RUN;

/*we are not actually passing the variabes to the marco rather just the names they will just used in a query*/
%PROCESS_VARIABLE_COMMENT(R3, DF_SPE_ACC_ID, COMMENT, RECIPIENT_ID, IS_REFERENCE, IS_ENDORSER, PROCESS_FROM, PROCESS_TO, NEEDED_BY, REGARDS_TO, REGARDS_CODE, 1, 'EBILL','UNWS33.R3');


