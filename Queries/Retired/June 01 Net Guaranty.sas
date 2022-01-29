/*	This one-time query prints the net guaranty amount for June 2001 -
	Sasha requested UTLWG03 be run to show cumulative net guarantees
	as of the end of several months.

*/
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;

%MACRO MYSQL(BEGD, ENDD);
%SYSLPUT BEGIN = &BEGD;
%SYSLPUT END = &ENDD;

RSUBMIT;
OPTIONS MPRINT SYMBOLGEN;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE MOGUARS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.AF_APL_ID||A.AF_APL_ID_SFX	AS CLID,
	B.df_prs_id_br					AS SSN,
	B.af_cur_apl_ops_ldr			AS LENDER,
	D.im_ist_ful					AS LNAME,
	B.af_apl_ops_scl				AS SCHOOL,
	C.im_ist_ful					AS SNAME,
	ROUND(A.aa_gte_lon_amt,0)		AS GROSS,
	ROUND(A.aa_tot_rfd,0)			AS REFUND,
	ROUND(A.aa_tot_can,0)			AS CANCEL,
	A.ac_lon_typ					AS TYPE
FROM	OLWHRM1.GA10_LON_APP A INNER JOIN
		OLWHRM1.GA01_APP B ON
			A.af_apl_id = B.af_apl_id AND
			A.ac_prc_sta = 'A' INNER JOIN
		OLWHRM1.SC01_LGS_SCL_INF C ON
			B.af_apl_ops_scl = C.if_ist INNER JOIN
		OLWHRM1.LR01_LGS_LDR_INF D ON
			B.af_cur_apl_ops_ldr = D.if_ist

WHERE	A.ac_lon_typ <> 'CL'
		AND A.ad_prc BETWEEN &BEGIN AND &END AND
		(A.aa_tot_can IS NULL AND A.aa_gte_lon_amt - A.aa_tot_rfd > 0 OR
		 A.aa_tot_can IS NOT NULL AND A.aa_gte_lon_amt - A.aa_tot_rfd - A.aa_tot_can > 0)
);
DISCONNECT FROM DB2;

DATA MOGUARS;
	SET MOGUARS;
	IF CANCEL = . THEN CANCEL = 0;
	NETAMT = GROSS - REFUND - CANCEL;
RUN;

PROC SQL;
CREATE TABLE LENDER AS
SELECT	LNAME,
		LENDER,
		SUM(NETAMT)	AS AMOUNT,
		COUNT(CLID)	AS NUMBER
FROM	MOGUARS
GROUP BY LENDER, LNAME;
QUIT;
PROC SORT DATA = LENDER;
BY LNAME;
RUN;

ENDRSUBMIT;
DATA LENDER;
	SET WORKLOCL.LENDER;
RUN;

OPTIONS NODATE CENTER PAGENO=1 LS=80;
PROC PRINT DATA = LENDER NOOBS SPLIT='/';
VAR LNAME
	LENDER
	NUMBER
	AMOUNT;
SUM 	NUMBER
		AMOUNT;
FORMAT	AMOUNT DOLLAR13.
		NUMBER COMMA8.;
LABEL	LNAME = 'LENDER NAME'
		LENDER = 'LENDER ID';
TITLE1	'UTAH HIGHER EDUCATION ASSISTANCE AUTHORITY';
TITLE2	"NET GUARANTEES FOR THE MONTH ENDING &ENDD";
TITLE3	'BY LENDER';
FOOTNOTE	'JOB = QUERY     REPORT = JUNE 01 NET GTY';
RUN;
%MEND(MYSQL);

%MYSQL('07-01-2001','07-31-2001')
%MYSQL('08-01-2001','08-31-2001')
%MYSQL('09-01-2001','09-30-2001')
%MYSQL('10-01-2001','10-31-2001')