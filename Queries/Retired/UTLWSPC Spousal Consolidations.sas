/*  UTLWSPC.sas prints all spousal consolidation loans guaranteed during
	the preceding month.  This report runs on the 1st of every month.
	Output goes to DBQ (BD) on the 2nd of the month to make sure no
	spousal consolidations were guaranteed. 		mc 8/13/01

GA01_APP
ac_apl_typ   	Loan Type : "C" = consol
ac_eds_typ   	Endorser type code : 'S' - Spousal
af_apl_id 	(17) Application ID (Commonline Format)
af_cur_apl_ops_ldr (8) Current lender ID for application
df_prs_id_eds 	(9) Person ID of endorser. Alternate index.
df_prs_id_br 	M Ib1 X(9) Person ID of borrower. Alternate Index.
PD01_PDM_INF
df_prs_id 	(match to ga01_app.df_prs_id_eds)
dm_prs_mid 	MD X(1) Middle initial
dm_prs_1 	MD X(12) First name
dm_prs_lst 	MD X(35) Last name
GA10_LON_APP
ad_prc O Id2 X(10) Date processed.
*/

*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
*%LET RPTLIB = %SYSGET(reportdir);
*FILENAME REPORT2 "&RPTLIB/ULWSPC.LWSPCR2";

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
DATA _NULL_;
     BEGDATE = PUT(INTNX('MONTH',TODAY(),-1), YYMMDD10.);	/*RESOLVES TO 1ST OF PRIOR MONTH*/
     ENDDATE = PUT(INTNX('MONTH',TODAY(),0)-1 , YYMMDD10.);	/*RESOLVES TO END OF PRIOR MONTH*/
     CALL SYMPUT('BEGIN',"'"||BEGDATE||"'");	            /*CREATES MACRO VARIABLE WITH IN FORMAT  'YYYY/MM/DD'*/
     CALL SYMPUT('END',"'"||ENDDATE||"'");              	/* WILL BE USED AS REPLACEMENTS IN CODE*/
RUN;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE SPOU1 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT		A.af_cur_apl_ops_ldr	AS LENDER,
			A.af_apl_id				AS CLUID,			
			A.df_prs_id_eds			AS ESSN,		/*Endorser SSN*/
			B.dm_prs_lst		 	AS eLAST,		/*Endorser demo info*/
			B.dm_prs_1 				AS eFIRST,
			B.dm_prs_mid			AS eMI,
			C.ad_prc				AS PROCDT,   	/*Guarantee Date*/
			A.ax_br_sig_iad			AS BWRSGNDT
FROM	OLWHRM1.GA01_APP A JOIN OLWHRM1.PD01_PDM_INF B
ON A.df_prs_id_eds = B.df_prs_id
JOIN OLWHRM1.GA10_LON_APP C
ON A.af_apl_id = C.af_apl_id
WHERE A.ac_apl_typ = 'C'
AND A.ac_eds_typ = 'S'
AND C.ad_prc BETWEEN &BEGIN AND &END
);
DISCONNECT FROM DB2;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE SPOU2 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT		A.af_apl_id				AS CLUID,
			A.df_prs_id_br			AS BSSN,		/*Borrower SSN*/
			B.dm_prs_lst		 	AS bLAST,		/*Borrower demo info*/
			B.dm_prs_1 				AS bFIRST,
			B.dm_prs_mid			AS bMI
FROM	OLWHRM1.GA01_APP A JOIN OLWHRM1.PD01_PDM_INF B
ON A.df_prs_id_br = B.df_prs_id
JOIN OLWHRM1.GA10_LON_APP C
ON A.af_apl_id = C.af_apl_id
WHERE A.ac_apl_typ = 'C'
AND A.ac_eds_typ = 'S'
AND C.ad_prc BETWEEN &BEGIN AND &END
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA _NULL_;
     EFFMO = PUT(INTNX('MONTH',TODAY(),-1), MONNAME9.);
	 EFFYR = PUT(INTNX('MONTH',TODAY(),-1), YEAR4.);
     CALL SYMPUT('EFFDATE',EFFMO||' '||EFFYR);		/*&EFFDATE is previous month, year*/
RUN;

PROC SORT DATA=WORKLOCL.SPOU1;
BY CLUID;
RUN;

PROC SORT DATA=WORKLOCL.SPOU2;
BY CLUID;
RUN;

DATA SPOU;
MERGE WORKLOCL.SPOU1 WORKLOCL.SPOU2;
BY CLUID;
RUN;

DATA SPOU (DROP = BLAST BFIRST BMI ELAST EFIRST EMI);
SET SPOU;
BNAME = TRIM(BLAST)||', '||TRIM(BFIRST)||' '||BMI;
ENAME = TRIM(ELAST)||', '||TRIM(EFIRST)||' '||EMI;
RUN;

PROC SORT DATA = SPOU;
BY LENDER PROCDT BSSN;
RUN;
/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS PAGENO=1 LS=132;
PROC PRINT NOOBS SPLIT='/' DATA=SPOU ;
VAR LENDER CLUID BSSN BWRSGNDT PROCDT BNAME ESSN ENAME;
LABEL	LENDER = 'Lender ID'
		CLUID = 'Commonline Unique ID'
		PROCDT = 'Date App Processed'
		BSSN = "Borrower SSN"
		ESSN = "Endorser SSN"
		BNAME = 'Borrower'
		ENAME = 'Endorser'
		BWRSGNDT = 'Borrower Sign Date';
TITLE	"Spousal Consolidations, &EFFDATE";
RUN;
