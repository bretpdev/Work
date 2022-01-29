/*%LET RPTLIB = %SYSGET(reportdir);*/
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';
%LET RPTLIB = T:\SAS;
/*LIBNAME SAS_TAB V8 'Y:\Development\SAS Test Files\progrevw';*/
FILENAME REPORTZ "&RPTLIB/UNWF09.NWF09RZ";
FILENAME REPORT2 "&RPTLIB/UNWF09.NWF09R2";
FILENAME REPORT3 "&RPTLIB/UNWF09.NWF09R3";
FILENAME REPORT4 "&RPTLIB/UNWF09.NWF09R4";
FILENAME REPORT5 "&RPTLIB/UNWF09.NWF09R5";

/*DATA _NULL_; */
/*	SET SAS_TAB.LASTRUN_JOBS;*/
/**/
/*	IF JOB = 'UTNWF09' THEN DO;*/
/*		CALL SYMPUT('LAST_RUN',"'"||TRIM(LEFT(PUT(LAST_RUN,YYMMDD10.)))||"'");*/
/*	END;*/
/*RUN;*/

/*used to set last run date manually, comment out for production*/
DATA _NULL_;
	%LET LAST_RUN = '2017-04-01';
RUN;
%PUT &LAST_RUN;	*writes date to the log, only needed for verifying the date being used while testing but won't hurt anything in prod;

%SYSLPUT LAST_RUN = &LAST_RUN;
/**/
/**/
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

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

	CREATE TABLE F09 AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID,
						PD10.DM_PRS_LST,
						LN10.LN_SEQ,
						COALESCE(LN10.LA_CUR_PRI,0) + COALESCE(LN10.LA_NSI_OTS,0) + COALESCE(DW01.LA_NSI_ACR,0) AS LOAN_BAL,
						LN09.IF_LON_SRV_DFL_LON,
						LN10.LC_SST_LON10,
						LN10.LC_STA_LON10,
						LN10.LD_STA_LON10,
						CASE
							WHEN LN09.IF_LON_SRV_DFL_LON = '' AND LN10.LC_SST_LON10 = '5' THEN 1
							WHEN LN09.IF_LON_SRV_DFL_LON = '' AND LN10.LC_SST_LON10 = '8' THEN 2
							WHEN LN09.IF_LON_SRV_DFL_LON <> '' AND CL40.LC_LIT_STA = 'V' AND DAYS(COALESCE(CL40.LD_LIT_STA,CURRENT_DATE)) < DAYS(CURRENT_DATE) - 15 THEN 3
							WHEN LN09.IF_LON_SRV_DFL_LON <> '' AND LN10.LC_SST_LON10 = '8' THEN 4
							WHEN LN09.IF_LON_SRV_DFL_LON <> '' AND CL40.LC_LIT_STA = 'V' AND CL40.LD_LIT_STA BETWEEN &LAST_RUN AND CURRENT_DATE THEN 5
							ELSE 0
						END AS SECTION,
						LN10.IC_LON_PGM
					FROM
						PKUB.PD10_PRS_NME PD10
						JOIN PKUB.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						JOIN PKUB.LN16_LON_DLQ_HST LN16
							ON LN10.BF_SSN = LN16.BF_SSN
							AND LN10.LN_SEQ = LN16.LN_SEQ
							AND LN16.LC_STA_LON16 = '1'
						JOIN PKUB.LN09_RPD_PIO_CVN LN09
							ON LN10.BF_SSN = LN09.BF_SSN
							AND LN10.LN_SEQ = LN09.LN_SEQ
						LEFT JOIN PKUB.CL40_LIT_HST CL40
							ON LN10.BF_SSN = CL40.BF_SSN
							AND LN10.LN_SEQ = CL40.LN_SEQ
						JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
					WHERE
						LN10.LA_CUR_PRI > 0
						AND LN16.LN_DLQ_MAX >= 360
					ORDER BY 
						SECTION,
						DF_SPE_ACC_ID,
						LN_SEQ

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

/*	%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*	%SQLCHECK;*/

QUIT;

ENDRSUBMIT;

DATA F09; SET LEGEND.F09; RUN;

DATA TITLES;
	FORMAT TITLE $60.;
	SECTION = 1;
	TITLE = 'First Time Defaults (never been submitted for assignment)';
	OUTPUT;

	SECTION = 2;
	TITLE = 'First Time Defaults (errored out after submission)';
	OUTPUT;

	SECTION = 3;
	TITLE = 'Un-Processable Redefaults (awaiting activation on DMCS)';
	OUTPUT;

	SECTION = 4;
	TITLE = 'Processable Redefaults (errored out after submission)';
	OUTPUT;

	SECTION = 5;
	TITLE = 'Redefaults Not Inventoried, Not Submitted';
	OUTPUT;
RUN;

%MACRO PRINT_REPORTS(LOAN_TYPE);
	DATA _NULL_;
		%IF &LOAN_TYPE = DIRECT %THEN
			%DO;
				%LET WHERE_CLAUSE = (IC_LON_PGM LIKE ('DL%') OR IC_LON_PGM = 'TEACH');
/*				%LET WHERE_CLAUSE = (IC_LON_PGM = 'DLSTFD'); *test code to verify split works;*/
				%LET SUM_REPORT = REPORT2;
				%LET DET_REPORT = REPORT3;
				%LET RNO = 2;
				%LET RTITLE = DCMS Backlog Summary Direct Loans - FED;
			%END;
		%ELSE
			%DO;
				%LET WHERE_CLAUSE = NOT (IC_LON_PGM LIKE ('DL%') OR IC_LON_PGM = 'TEACH');
/*				%LET WHERE_CLAUSE = NOT (IC_LON_PGM = 'DLSTFD'); *test code to verify split works;*/
				%LET SUM_REPORT = REPORT4;
				%LET DET_REPORT = REPORT5;
				%LET RNO = 4;
				%LET RTITLE = DCMS Backlog Summary FFELP Loans - FED;
			%END;
	RUN;

	%PUT LOAN_TYPE = &LOAN_TYPE;
	%PUT WHERE_CLAUSE = &WHERE_CLAUSE;
	%PUT SUM_REPORT = &SUM_REPORT;
	%PUT DET_REPORT = &DET_REPORT;
	%PUT RNO = &RNO;
	%PUT RTITLE = &RTITLE;

	%MACRO CREATE_SUMMARIES(SEC);
		PROC SQL;
			CREATE TABLE SUM_SEC&SEC AS
				SELECT
					&SEC AS SECTION,
					T.TITLE,
					F09.DEBTS,
					F09.BORROWERS,
					F09.TOTAL_BALANCE
				FROM
					(
						SELECT
							COUNT(LN_SEQ) AS DEBTS,
							COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWERS,
							CASE
								WHEN SUM(LOAN_BAL)IS NULL THEN 0
								ELSE SUM(LOAN_BAL)
							END AS TOTAL_BALANCE
						FROM
							F09
						WHERE
							SECTION = &SEC
							AND &WHERE_CLAUSE 
					) F09
					LEFT JOIN TITLES T
						ON SECTION = &SEC
			;
		QUIT;
	%MEND;

	%CREATE_SUMMARIES(1);
	%CREATE_SUMMARIES(2);
	%CREATE_SUMMARIES(3);
	%CREATE_SUMMARIES(4);
	%CREATE_SUMMARIES(5);

	DATA R2;
		SET SUM_SEC1 SUM_SEC2 SUM_SEC3 SUM_SEC4 SUM_SEC5;
	RUN;

	/*create printed report*/
	PROC PRINTTO PRINT=&SUM_REPORT NEW; RUN;

	OPTIONS ORIENTATION=LANDSCAPE PS=39 LS=127;
	TITLE 		&RTITLE;
	FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
	FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
	FOOTNOTE3	;
	FOOTNOTE4   "JOB = UTNWF09  	 REPORT = UNWF09.NWF09R&RNO";

	PROC PRINT 
			NOOBS SPLIT = '/' 
			DATA = R2 
			WIDTH = UNIFORM 
			WIDTH = MIN 
			LABEL;

		FORMAT
			DEBTS COMMA8.
			BORROWERS COMMA8.
			TOTAL_BALANCE DOLLAR18.2
		;

		VAR 
			SECTION
			TITLE
			DEBTS
			BORROWERS
			TOTAL_BALANCE
		;

		LABEL
			SECTION = 'Section'
			TITLE = 'Section Title'
			DEBTS = 'Debts'
			BORROWERS = 'Borrowers'
			TOTAL_BALANCE = 'Principal & Interest'
		;
	RUN;

	PROC PRINTTO; RUN;

	/*write to comma delimited file*/
	DATA _NULL_;
		SET	WORK.F09;
		WHERE &WHERE_CLAUSE;
		FILE
			&DET_REPORT
			DELIMITER = ','
			DSD
			DROPOVER
			LRECL = 32767
		;

		FORMAT
			DF_SPE_ACC_ID $10.
			LOAN_BAL DOLLAR18.2
			LD_STA_LON10 MMDDYY10.
		;

		/* write column names */
		IF _N_ = 1 THEN
			DO;
				PUT	'SECTION,DF_SPE_ACC_ID,DM_PRS_LST,IC_LON_PGM,LN_SEQ,LOAN_BAL,IF_LON_SRV_DFL_LON,LC_SST_LON10,LC_STA_LON10,LD_STA_LON10';
			END;

		/* write data*/	
		DO;
			PUT SECTION @;
			PUT DF_SPE_ACC_ID $ @;
			PUT DM_PRS_LST $ @;
			PUT IC_LON_PGM $ @;
			PUT LN_SEQ @;
			PUT LOAN_BAL @;
			PUT IF_LON_SRV_DFL_LON $ @;
			PUT LC_SST_LON10 $ @;
			PUT LC_STA_LON10 $ @;
			PUT LD_STA_LON10;
			;
		END;
	RUN;

%MEND;

%PRINT_REPORTS(DIRECT);
%PRINT_REPORTS(FFELP);

/*updates last run date in data set to today*/
/*DATA SAS_TAB.LASTRUN_JOBS;*/
/*	SET SAS_TAB.LASTRUN_JOBS;*/
/*	IF JOB = 'UTNWF09' THEN LAST_RUN = TODAY();*/
/*RUN;*/
