/*
UNIV of UTAH CU LOANS GUARANTEED

Lists loans guaranteed during the previous week for the UofU CU.


*/


LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWG06.LWG06R2";


/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
OPTIONS MPRINT SYMBOLGEN;
DATA _NULL_;
     BEGDATE = PUT(INTNX('WEEK',TODAY()+3,-1,'BEGINNING'), YYMMDD10.);
     ENDDATE = PUT(INTNX('WEEK',TODAY()+3,-1, 'END'), YYMMDD10.);
     CALL SYMPUT('BEGIN',"'"||BEGDATE||"'");
     CALL SYMPUT('END',"'"||ENDDATE||"'");
RUN;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE UCUGUARS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.AF_APL_ID||A.AF_APL_ID_SFX							AS CLID,
	B.af_apl_ops_scl										AS SCHOOL,
	C.im_ist_ful											AS SNAME,
	A.aa_gte_lon_amt										AS GROSS,
	A.aa_tot_rfd											AS REFUND,
	A.aa_tot_can											AS CANCEL,
	A.ac_lon_typ											AS TYPE
FROM	OLWHRM1.GA10_LON_APP A INNER JOIN
		OLWHRM1.GA01_APP B ON
			A.af_apl_id = B.af_apl_id AND
			A.ac_prc_sta = 'A' INNER JOIN
		OLWHRM1.SC01_LGS_SCL_INF C ON
			B.af_apl_ops_scl = C.if_ist
WHERE	B.af_cur_apl_ops_ldr = '830132' AND
		A.ad_prc BETWEEN &BEGIN AND &END AND
		(A.aa_tot_can IS NULL AND A.aa_gte_lon_amt - A.aa_tot_rfd > 0 OR
		 A.aa_tot_can IS NOT NULL AND A.aa_gte_lon_amt - A.aa_tot_rfd - A.aa_tot_can > 0)
);

CREATE TABLE LNAME AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.IM_IST_FUL
FROM	OLWHRM1.IN01_LGS_IDM_MST A 
WHERE	A.IF_IST = '830132'
);
DISCONNECT FROM DB2;
/*ENDRSUBMIT;*/

/*DATA WORK.UCUGUARS;*/
/*	SET WORKLOCL.UCUGUARS;*/
/*RUN;*/

/*DATA WORK.LNAME;*/
/*	SET WORKLOCL.LNAME;*/
/*RUN;*/

DATA UCUGUARS;
	SET UCUGUARS;
	IF CANCEL = . THEN CANCEL = 0;
	NETAMT = GROSS - REFUND - CANCEL;
RUN;

DATA _NULL_;
	SET LNAME;
    CALL SYMPUT('LENDERNAME',IM_IST_FUL);
RUN;

PROC SQL;
CREATE TABLE UCUBYSCH AS
SELECT	SNAME,
		SCHOOL,
		TYPE,
		SUM(NETAMT)	AS AMOUNT,
		COUNT(CLID)	AS NUMBER
FROM	UCUGUARS
GROUP BY SCHOOL, SNAME, TYPE;
QUIT;
DATA _NULL_;
     BEGDATE = PUT(INTNX('WEEK',TODAY()+3,-1,'BEGINNING'), MMDDYY10.);
     ENDDATE = PUT(INTNX('WEEK',TODAY()+3,-1, 'END'), MMDDYY10.);
     CALL SYMPUT('BEGIN',BEGDATE);
     CALL SYMPUT('END',ENDDATE);
RUN;
PROC SORT DATA = UCUBYSCH;
BY SCHOOL SNAME;
RUN;
OPTIONS  PAGENO=1 LS=80;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

PROC PRINT DATA = UCUBYSCH NOOBS SPLIT='/';
BY SCHOOL SNAME;
VAR TYPE
	NUMBER
	AMOUNT;
SUMBY 	SCHOOL;
FORMAT	AMOUNT DOLLAR13.
		NUMBER COMMA8.;
LABEL	SNAME = 'SCHOOL NAME'
		SCHOOL = 'SCHOOL ID';
TITLE1	'UTAH HIGHER EDUCATION ASSISTANCE AUTHORITY';
TITLE2	"NET GUARANTEES FOR THE WEEK OF &BEGIN TO &END";
TITLE3	"&LENDERNAME";
FOOTNOTE	'JOB = UTLWG06     REPORT = ULWG06.LWG06R2';
RUN;

PROC PRINTTO;
RUN;
