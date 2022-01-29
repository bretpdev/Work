/*	This query produces two reports from the previous night's batch processes.
	The first report shows loans rejected due to aggregate limits.
	The second report shows loans whose guaranteed amounts were reduced due to
	aggregate limits.										MC 9/27/01
*/
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE GUARS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	a.AF_APL_ID||a.AF_APL_ID_SFX							AS CLUID,
	a.ad_prc												AS PROC,
	a.af_lst_dts_ga10										AS TIME,
	a.ac_gte_rdc											AS REDUXCD,
	a.ac_apl_rej_rea_1										AS REJ1,
	a.ac_apl_rej_rea_2										AS REJ2,
	a.ac_apl_rej_rea_3										AS REJ3,
	a.ac_apl_rej_rea_4										AS REJ4,
	a.ac_apl_rej_rea_5										AS REJ5,
	a.AC_PRC_STA											AS STATUS,
	c.DF_PRS_ID												AS SSN1,
	RTRIM(c.dm_prs_1)||' '||RTRIM(c.dm_prs_lst)				AS NAME,
	c.df_zip												AS ZIP

FROM OLWHRM1.GA10_LON_APP a JOIN OLWHRM1.GA01_APP b
ON a.af_apl_id = b.af_apl_id
JOIN OLWHRM1.PD01_PDM_INF c
ON b.df_prs_id_br = c.DF_PRS_ID
WHERE	DAYS(a.ad_prc) >= DAYS(CURRENT DATE) - 1 
AND	((DAYS(a.af_lst_dts_ga10) = DAYS(CURRENT DATE) - 1 AND HOUR(a.af_lst_dts_ga10) > 22) 
OR DAYS(a.af_lst_dts_ga10) = DAYS(CURRENT DATE))
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA GUARS (DROP = SSN1);
SET WORKLOCL.GUARS;
SSN = INPUT(SSN1,9.);
FORMAT SSN SSN11.;
RUN;

PROC SORT DATA = GUARS;
BY SSN CLUID;
RUN;

OPTIONS NOCENTER PAGENO=1 LS=120;
PROC PRINT DATA = GUARS NOOBS N='Count:  ' LABEL WIDTH=MIN WIDTH=UNIFORM;
WHERE REDUXCD = '03';
VAR SSN NAME CLUID;
LABEL NAME = 'Name' CLUID = 'Commonline Unique ID';
TITLE 'Aggregate Limit Reduced Loans - Daily';
FOOTNOTE  'JOB = UTLWAGL     REPORT = ULWAGL.LWAGLR2';
RUN;

DATA GUARS (DROP = ZIP);
SET GUARS;
ZIP5 = SUBSTR(ZIP,1,5);
ZIP4 = SUBSTR(ZIP,6,4);
RUN;

PROC SORT DATA = GUARS;
BY ZIP5 ZIP4;
RUN;

DATA GUARS (DROP = ZIP5 ZIP4);
SET GUARS;
IF ZIP4 <> ' ' THEN ZIP = TRIM(ZIP5)||"-"||TRIM(ZIP4);
ELSE ZIP = ZIP5;
RUN;

OPTIONS NOCENTER PAGENO=1 LS=120;
PROC PRINT DATA = GUARS NOOBS N='Count:  ' LABEL WIDTH=MIN WIDTH=UNIFORM;
WHERE ((REJ1 = 'G') 
OR (REJ2 = 'G')
OR (REJ3 = 'G')
OR (REJ4 = 'G')
OR (REJ5 = 'G'))
AND STATUS IN ('R','X');
VAR ZIP SSN NAME CLUID;
LABEL NAME = 'Name' CLUID = 'Commonline Unique ID' ZIP = 'ZIP Code';
TITLE 'Aggregate Limit Rejects - Daily';
FOOTNOTE  'JOB = UTLWAGL     REPORT = ULWAGL.LWAGLR3';
RUN;