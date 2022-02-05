/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS83.NWS83RZ";
FILENAME REPORT2 "&RPTLIB/UNWS83.NWS83R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%LET DB = DNFPUTDL;  *This is live;

DATA _NULL_;
	CALL SYMPUT('BEG',"'"||PUT(INTNX('MONTH',TODAY(),-1,'BEGINNING'),MMDDYY10.)||"'");
	CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'END'),MMDDYY10.)||"'");
RUN;

%PUT &BEG &END;

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

	CREATE TABLE S83_DET AS
		SELECT	
			BF_SSN,
			LN_SEQ,
			CAT(BF_SSN,PUT(LN_SEQ,Z3.)) AS LID, /*unique loan ID to be used later to count the number of loans*/
			LA_LON_AMT_GTR,
			LD_LON_ACL_ADD
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						LN10.BF_SSN,
						LN10.LN_SEQ,
						LN10.LA_LON_AMT_GTR,
						LN10.LD_LON_ACL_ADD
					FROM
						PKUB.LN10_LON LN10
					WHERE
						DAYS(LN10.LD_LON_ACL_ADD) BETWEEN DAYS(&BEG) AND DAYS(&END)

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA S83_DET; SET LEGEND.S83_DET; RUN;

/*create summary data for report*/
PROC SQL;
	CREATE TABLE S83_SUM AS
		SELECT
			COUNT(DISTINCT BF_SSN) AS BRWS,
			COUNT(DISTINCT LID) AS LNS,
			SUM(LA_LON_AMT_GTR) AS LN_AMT
		FROM
			S83_DET
	;
RUN;

/*create printed report*/
PROC PRINTTO PRINT=REPORT2 NEW; RUN;

OPTIONS ORIENTATION=LANDSCAPE PS=39 LS=127;
TITLE 		'COD Monthly Disbursement - FED';
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTNWS83  	 REPORT = UNWS83.NWS83R2';

PROC PRINT 
		NOOBS SPLIT = '/' 
		DATA = S83_SUM
		WIDTH = UNIFORM 
		WIDTH = MIN 
		LABEL;

	FORMAT 
		BRWS COMMA8.
		LNS COMMA8.
		LN_AMT DOLLAR18.2
	;

	VAR 
		BRWS
		LNS
		LN_AMT
	;

	LABEL
		BRWS = '# Of Borrowers'
		LNS = '# Of Loans'
		LN_AMT = 'Loan Amount (dollar amount of all guaranteed amounts)'
	;
RUN;

PROC PRINTTO; RUN;

/*create detail file for testing*/
/*PROC EXPORT*/
/*		DATA=S83_DET*/
/*		OUTFILE='T:\SAS\S83 DETAIL.XLSX'*/
/*		REPLACE;*/
/*RUN;*/
