/*UTLWE14 - MONTHLY WELLS FARGO MPN DATA FILE*/

LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWE14.LWE14R2";

/*SET DATE RANGE:  IF EOM PROCESSING RUNS BEFORE LAST DAY OF MONTH, (3 DAY WINDOW)
REPORT FOR CURRENT MONTH, ELSE REPORT FOR THE PREVIOUS MONTH*/
DATA _NULL_;
     CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY()+3,-1,'beginning'), MMDDYYD10.)||"'");
     CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY()+3,-1,'end'), MMDDYYD10.)||"'");
     CALL SYMPUT('EFFDATE',TRIM(LEFT(UPCASE(
		PUT(INTNX('MONTH',TODAY()+3,-1), MONNAME9.)||' '||
		PUT(INTNX('MONTH',TODAY()+3,-1), YEAR4.)))));
RUN;

/*FILENAME REPORT2 'T:\SAS\ULWE14.LWE14R2';*/
/*%SYSLPUT BEGIN = &BEGIN;*/
/*%SYSLPUT END = &END;*/
/*libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;*/
/*RSUBMIT;*/

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE WFEOM AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT COUNT(B.AF_APL_ID||B.AF_APL_ID_SFX) AS LOANS
	,A.AF_APL_OPS_SCL
	,B.AC_LON_TYP
FROM OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
WHERE A.AF_CUR_APL_OPS_LDR = '813894'
AND B.AC_PRC_STA = 'A'
AND B.AD_PRC BETWEEN &BEGIN AND &END
AND A.AF_BS_MPN_APL_ID IS NULL
GROUP BY A.AF_APL_OPS_SCL, B.AC_LON_TYP
ORDER BY A.AF_APL_OPS_SCL, B.AC_LON_TYP
);
DISCONNECT FROM DB2;

/*ENDRSUBMIT;*/
/*DATA WFEOM; */
/*SET WORKLOCL.WFEOM; */
/*RUN;*/

DATA _NULL_;
SET  WORK.WFEOM;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT AF_APL_OPS_SCL $8. ;
FORMAT AC_LON_TYP $2. ;
FORMAT LOANS 11. ;
DO;
	PUT AF_APL_OPS_SCL $ @;
	PUT AC_LON_TYP $ @;
	PUT LOANS ;
	;
	END;
RUN;
