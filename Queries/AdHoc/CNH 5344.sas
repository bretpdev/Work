PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\NH_XXXX.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="A$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.SOURCE;
	SET SOURCE;
RUN;

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

/*%MACRO TBLS (TBL,NME);*/
PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE RSXX AS
		SELECT DISTINCT
			RSXX.*
		FROM SOURCE S
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON S.DF_SPE_ACC_ID = PDXX.DF_SPE_ACC_ID
			LEFT JOIN PKUB.RSXX_IBR_RPS RSXX
				ON PDXX.DF_PRS_ID = RSXX.BF_SSN


;
QUIT;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE RSXX AS
		SELECT DISTINCT
			RSXX.*
		FROM SOURCE S
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON S.DF_SPE_ACC_ID = PDXX.DF_SPE_ACC_ID
			LEFT JOIN PKUB.RSXX_BR_RPD RSXX
				ON PDXX.DF_PRS_ID = RSXX.BF_SSN


;
QUIT;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE RSXX AS
		SELECT DISTINCT
			RSXX.*
		FROM SOURCE S
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON S.DF_SPE_ACC_ID = PDXX.DF_SPE_ACC_ID
			LEFT JOIN PKUB.RSXX_IBR_EXT_LON RSXX
				ON PDXX.DF_PRS_ID = RSXX.BF_SSN


;
QUIT;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE RSXX AS
		SELECT DISTINCT
			RSXX.*
		FROM SOURCE S
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON S.DF_SPE_ACC_ID = PDXX.DF_SPE_ACC_ID
			LEFT JOIN PKUB.RSXX_IBR_IRL_LON RSXX
				ON PDXX.DF_PRS_ID = RSXX.BF_SSN


;
QUIT;
/*%MEND TBLS;*/
/*%TBLS (RSXX_IBR_RPS, RSXX);*/
/*%TBLS (RSXX_BR_RPD, RSXX);*/
/*%TBLS (RSXX_IBR_EXT_LON, RSXX);*/
/*%TBLS (RSXX_IBR_IRL_LON, RSXX);*/

ENDRSUBMIT;

DATA RSXX; SET LEGEND.RSXX; RUN;
DATA RSXX; SET LEGEND.RSXX; RUN;
DATA RSXX; SET LEGEND.RSXX; RUN;
DATA RSXX; SET LEGEND.RSXX; RUN;

/*create printed report*/
PROC PRINTTO PRINT=REPORTX NEW; RUN;

OPTIONS ORIENTATION=LANDSCAPE PS=XX LS=XXX;
TITLE 		'Delinquency Rate - FED';
TITLEX		"RUNDATE &SYSDATEX";
FOOTNOTEX  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTEX	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTEX	;
FOOTNOTEX   'JOB = UTNWSXX  	 REPORT = UNWSXX.NWSXXRX';

PROC PRINT 
		NOOBS SPLIT = '/' 
		DATA = DEMO 
		WIDTH = UNIFORM 
		WIDTH = MIN 
		LABEL;

	FORMAT
		DF_SPE_ACC_ID $XX.
		LN_SEQ BESTX.
		LD_LON_GTR MMDDYYXX.
	;

	VAR 
		DM_PRS_X	
		DF_SPE_ACC_ID
		LN_SEQ
		LD_LON_GTR
	;

	LABEL
		DM_PRS_X = 'First Name'
		DF_SPE_ACC_ID = 'Account Number'
		LN_SEQ = 'Loan Sequence Number'
		LD_LON_GTR = 'Guarantee Date'
	;
RUN;

PROC PRINTTO; RUN;

/*write to comma delimited file*/
DATA _NULL_;
	SET		WORK.DEMO;
	FILE
		'T:\SAS\CSV OUTPUT.txt'
		DELIMITER = ','
		DSD
		DROPOVER
		LRECL = XXXXX
	;

	FORMAT
		DF_SPE_ACC_ID $XX.
		LD_LON_GTR YYMMDDXX.
	;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = X THEN
		DO;
			PUT	
				'DF_SPE_ACC_ID'
				','
				'LN_SEQ'
				','
				'LD_LON_GTR'
			;
		END;

	/* write data*/	
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT LN_SEQ $ @;
		PUT LD_LON_GTR;
		;
	END;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\EXCEL OUTPUT.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

/*export to queue builder (fed) file*/
DATA _NULL_;
	SET DEMO ;
	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	PUT DF_SPE_ACC_ID 'AMFDD,,,,,,,ALL,Borrower has multiple due dates please review' ;
RUN;

/*export to comma delimited file*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\CSV OUTPUT.CSV" 
            DBMS = CSV 
			REPLACE;
     PUTNAMES = YES;
RUN;

/*export to comman delimited file for the Special E-mail Campaign - FED script*/
DATA _NULL_;
	SET DEMO ;
	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;

	IF _N_ = X THEN
		DO;
			PUT "ACCOUNT_NUMBER,BORROWER_NAME,EMAIL_ADDRESS";
		END;

	DO;
	   PUT DF_SPE_ACC_ID @;
	   PUT BRW_NAME @;
	   PUT DX_ADR_EML $ ;
	END;
RUN;


/*write to comma delimited file for the Email Batch Script - FED script*/
DATA _NULL_;
	SET		WORK.SKIP;
	FILE	REPORTX delimiter=',' DSD DROPOVER lrecl=XXXXX;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = X THEN
		DO;
			PUT	
				'DF_SPE_ACC_ID'
				','
				'DM_PRS_X'
				','
				'DM_PRS_LST'
				','
				'DX_ADR_EML'
			;
		END;

	/* write data*/	
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_X $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_ADR_EML $;
		;
	END;
RUN;
