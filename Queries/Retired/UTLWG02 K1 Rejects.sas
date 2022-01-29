/*

K1 REJECTS

This report lists rejected applications with a reject reason of 'K1' procesed the previous
day.

*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWG02.LWG02R2";*/


LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE K1S AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT		
	A.df_zip									AS ZIP,
	A.df_prs_id									AS SSN,
	RTRIM(A.dm_prs_1)||' '||RTRIM(A.dm_prs_lst) AS NAME
FROM 	OLWHRM1.PD01_PDM_INF A INNER JOIN
		OLWHRM1.GA01_APP B ON
			A.df_prs_id = B.df_prs_id_br INNER JOIN
		OLWHRM1.GA10_LON_APP C ON
			B.af_apl_id = C.af_apl_id AND
			DAYS(C.ad_prc) = DAYS(CURRENT DATE) - 1
WHERE	C.ac_apl_rej_rea_1 = 'K1' OR
		C.ac_apl_rej_rea_2 = 'K1' OR
		C.ac_apl_rej_rea_3 = 'K1' OR
		C.ac_apl_rej_rea_4 = 'K1' OR
		C.ac_apl_rej_rea_5 = 'K1'
);
DISCONNECT FROM DB2;
ENDRSUBMIT;
PROC SORT DATA=WORKLOCL.K1S;
BY ZIP;
RUN;
/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS PAGENO=1 LS=80;
PROC PRINT NOOBS SPLIT='/' DATA=WORKLOCL.K1S;
VAR ZIP
	SSN
	NAME;
TITLE 'K1 REJECTS';
RUN;