/*UTLWD18 DYPMTRVW QUE*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORT2 "&RPTLIB/ULWD18.LWD18R2";*/
DATA _NULL_;
	CALL SYMPUT('DAYS_AGO_30',"'"||PUT(INTNX('DAY',TODAY(),-30,'BEGINNING'), MMDDYYD10.)||"'");
RUN;
FILENAME REPORT2 'T:\SAS\ULWD18.LWD18R2';
%SYSLPUT DAYS_AGO_30 = &DAYS_AGO_30;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DPYMTRVW AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT B.BF_SSN
	,'' AS V
FROM OLWHRM1.DC01_LON_CLM_INF B
INNER JOIN OLWHRM1.AY01_BR_ATY C
	 ON B.BF_SSN = C.DF_PRS_ID
WHERE B.LC_STA_DC10 = '03'
	AND B.LC_AUX_STA = ''
	AND B.LD_CLM_ASN_DOE IS NULL
	AND B.LC_PCL_REA IN 
	(
		'DF','DQ','DB'
	)
	AND B.LC_BIL_STA IN 
	(
		'11'
	)
	AND C.PF_ACT = 'DD136'
	AND C.BD_ATY_PRF < &DAYS_AGO_30
	AND NOT EXISTS
	(
		SELECT 1
		FROM OLWHRM1.AY01_BR_ATY X
		WHERE X.DF_PRS_ID = B.BF_SSN
			AND X.PF_ACT = 'DD137'
	)
ORDER BY B.BF_SSN
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA DPYMTRVW;
	SET WORKLOCL.DPYMTRVW;
RUN;

DATA _NULL_;
SET  WORK.DPYMTRVW;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT BF_SSN $9. ;
/*IF _N_ = 1 THEN DO;*/
/*	PUT 'SSN,QNAME,INDT_ID,INDT_TYPE,DUE_DATE,DUE_TIME,TEXT';*/
/*END;*/
DO;
	PUT BF_SSN $ ;
/*	PUT "DYPMTRVW" V @;*/
/*	PUT V $ @;*/
/*	PUT V $ @;*/
/*	PUT V $ @;*/
/*	PUT V $ @;*/
/*	PUT V $ @;*/
END;
RUN;
