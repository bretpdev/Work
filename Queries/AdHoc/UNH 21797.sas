PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\CR20129.UTFILE.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="CR20129.UTFILE$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;


/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS46.NWS46RZ";
FILENAME REPORT2 "&RPTLIB/UNWS46.NWS46R2";

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=work  ;

DATA DUSTER.SOURCE;
	SET SOURCE;
RUN;

RSUBMIT DUSTER;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DLGSUTWH;  *This is live;

LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;

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
		SELECT	
			PD10.DF_SPE_ACC_ID
			,AD20.*
		FROM	
			SOURCE S
			INNER JOIN OLWHRM1.PD10_PRS_NME PD10
				ON S.DF_SPE_ACC_I = PD10.DF_SPE_ACC_ID
			LEFT JOIN OLWHRM1.AD20_PCV_ATY_ADJ AD20
				ON PD10.DF_PRS_ID = AD20.BF_SSN
				AND S.N_SEQ = AD20.LN_SEQ
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET DUSTER.DEMO; RUN;

/*create printed report*/
PROC PRINTTO PRINT=REPORT2 NEW; RUN;

OPTIONS ORIENTATION=LANDSCAPE PS=39 LS=127;
TITLE 		'Delinquency Rate - FED';
TITLE2		"RUNDATE &SYSDATE9";
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTNWS46  	 REPORT = UNWS46.NWS46R2';

PROC PRINT 
		NOOBS SPLIT = '/' 
		DATA = DEMO 
		WIDTH = UNIFORM 
		WIDTH = MIN 
		LABEL;

	FORMAT
		DF_SPE_ACC_ID $10.
		LN_SEQ BEST2.
		LD_LON_GTR MMDDYY10.
	;

	VAR 
		DM_PRS_1	
		DF_SPE_ACC_ID
		LN_SEQ
		LD_LON_GTR
	;

	LABEL
		DM_PRS_1 = 'First Name'
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
		LRECL = 32767
	;

	FORMAT
		DF_SPE_ACC_ID $10.
		LD_LON_GTR YYMMDD10.
	;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
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
            OUTFILE = "Y:\Development\SAS Test Files\Kathryn\SASR 3674\EXCEL OUTPUT.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

/*export to queue builder (fed) file*/
DATA _NULL_;
	SET DEMO ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
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
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;

	IF _N_ = 1 THEN
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
	FILE	REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
		DO;
			PUT	
				'DF_SPE_ACC_ID'
				','
				'DM_PRS_1'
				','
				'DM_PRS_LST'
				','
				'DX_ADR_EML'
			;
		END;

	/* write data*/	
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_ADR_EML $;
		;
	END;
RUN;
