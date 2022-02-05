/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS86.NWS86RZ";
FILENAME REPORT2 "&RPTLIB/UNWS86.NWS86R2";

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

	CREATE TABLE ECORR_ALL AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						LN10.BF_SSN,
						PH05.DI_VLD_CNC_EML_ADR,
						PH05.DI_CNC_ELT_OPI,
						PH05.DI_CNC_EBL_OPI,
						PH05.DI_CNC_TAX_OPI
					FROM
						PKUB.PD10_PRS_NME PD10
						JOIN PKUB.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						LEFT JOIN AES.PH05_CNC_EML PH05
							ON CAST(PD10.DF_SPE_ACC_ID AS DECIMAL(10,0)) = PH05.DF_SPE_ID
					WHERE
						LN10.LA_CUR_PRI ^= 0
						AND LN10.LC_STA_LON10 = 'R'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA ECORR_ALL; SET LEGEND.ECORR_ALL; RUN;

PROC SQL;
	CREATE TABLE CNT_ALL AS
		SELECT
			1 AS ID,
			COUNT(DISTINCT BF_SSN) AS CNT_ALL
		FROM
			ECORR_ALL
	;

	CREATE TABLE CNT_LETTER AS
		SELECT
			1 AS ID,
			COUNT(DISTINCT BF_SSN) AS LETTER
		FROM
			ECORR_ALL
		WHERE
			DI_VLD_CNC_EML_ADR = 'Y' AND DI_CNC_ELT_OPI = 'Y'
	;

	CREATE TABLE CNT_BILL AS
		SELECT
			1 AS ID,
			COUNT(DISTINCT BF_SSN) AS BILL
		FROM
			ECORR_ALL
		WHERE
			DI_VLD_CNC_EML_ADR = 'Y' AND DI_CNC_EBL_OPI = 'Y'
	;

	CREATE TABLE CNT_TAX AS
		SELECT
			1 AS ID,
			COUNT(DISTINCT BF_SSN) AS TAX
		FROM
			ECORR_ALL
		WHERE
			DI_VLD_CNC_EML_ADR = 'Y' AND DI_CNC_TAX_OPI = 'Y'
	;
QUIT;

PROC SQL;
	CREATE TABLE CNTS AS
		SELECT
			ALL.CNT_ALL,
			LTR.LETTER,
			COALESCE(LTR.LETTER,0)/ALL.CNT_ALL*100 AS PCT_LETTER,
			BIL.BILL,
			COALESCE(BIL.BILL,0)/ALL.CNT_ALL*100 AS PCT_BILL,
			TAX.TAX,
			COALESCE(TAX.TAX,0)/ALL.CNT_ALL*100 AS PCT_TAX
		FROM
			CNT_ALL ALL
			LEFT JOIN CNT_LETTER LTR
				ON ALL.ID = LTR.ID
			LEFT JOIN CNT_BILL BIL
				ON ALL.ID = BIL.ID
			LEFT JOIN CNT_TAX TAX
				ON ALL.ID = TAX.ID
	;
QUIT;

/*create printed report*/
PROC PRINTTO PRINT=REPORT2 NEW; RUN;

OPTIONS ORIENTATION=LANDSCAPE PS=39 LS=127;
TITLE 		'Ecorr Monthly Stats - FED';
TITLE2		;
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTNWS86  	 REPORT = UNWS86.NWS86R2';

PROC PRINT 
		NOOBS SPLIT = '/' 
		DATA = CNTS 
		WIDTH = UNIFORM 
		WIDTH = MIN 
		LABEL;

	FORMAT
		LETTER BILL TAX COMMA12.
		PCT_LETTER PCT_BILL PCT_TAX BEST5.2
	;

	VAR 
		LETTER
		PCT_LETTER
		BILL
		PCT_BILL
		TAX
		PCT_TAX
	;

	LABEL
		LETTER = 'Ecorr Letter Population'
		PCT_LETTER = 'Ecorr Letter Percentage'
		BILL = 'Ecorr Bill Population'
		PCT_BILL = 'Ecorr Bill Percentage'
		TAX = 'Ecorr Tax Population'
		PCT_TAX = 'Ecorr Tax Percentage'
	;
RUN;

PROC PRINTTO; RUN;

/*only needed for testing*/
/*PROC EXPORT*/
/*		DATA=ECORR_ALL*/
/*		OUTFILE='T:\SAS\S86_DETAIL.XLSX'*/
/*		REPLACE;*/
/*RUN;*/