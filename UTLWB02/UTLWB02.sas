/* UTLWB02 - PENDING BANKRUPTCY HARDSHIP CLAIMS */

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWB02.LWB02R2";*/

OPTIONS SYMBOLGEN;

/*SET DATE RANGE:  IF TODAY IS IN THE LAST 3 DAYS OF THE MONTH, REPORT FOR CURRENT MONTH*/
/*ELSE, REPORT FOR THE PREVIOUS MONTH*/
DATA _NULL_;
NOW = DATE();
IF NOW = INTNX('MONTH',TODAY(),0,'end') 
OR NOW = INTNX('MONTH',TODAY(),0,'end') - 1
OR NOW = INTNX('MONTH',TODAY(),0,'end') - 2
THEN DO;
     CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),0,'beginning'), MMDDYYD10.)||"'");
     CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),0,'end'), MMDDYYD10.)||"'");
     CALL SYMPUT('EFFDATE',TRIM(LEFT(UPCASE(
		PUT(INTNX('MONTH',TODAY(),0), MONNAME9.)||' '||
		PUT(INTNX('MONTH',TODAY(),0), YEAR4.)))));
END;
ELSE DO;
	 CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'beginning'), MMDDYYD10.)||"'");
     CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'end'), MMDDYYD10.)||"'");
     CALL SYMPUT('EFFDATE',TRIM(LEFT(UPCASE(
		PUT(INTNX('MONTH',TODAY(),-1), MONNAME9.)||' '||
		PUT(INTNX('MONTH',TODAY(),-1), YEAR4.)))));
END;
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132 SYMBOLGEN;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE PENDBKR AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	integer(A.bf_ssn)				as SSN,
		A.LC_PCL_REA,
		A.LC_STA_DC10,
		A.LF_CLM_ID,
		A.af_apl_id||A.af_apl_id_sfx	as CLUID,
		LD_LDR_POF,
		A.LA_TOT_ITL_CLM_PD				as CLMPDAMT,
		C.LC_BKR_STA,
		C.LD_BKR_DSM,
		C.LD_BKR_DCH,
		B.LD_AGY_BIL_DOE				as RQSTDT
		,C.LD_BKR_FIL
FROM  OLWHRM1.DC01_LON_CLM_INF A left outer join OLWHRM1.DC19_DOE_RIN B
	on a.LF_CRT_DTS_DC10 = b.LF_CRT_DTS_DC10
INNER JOIN OLWHRM1.DC18_BKR C
	on a.af_apl_id = C.af_apl_id
	and a.af_apl_id_sfx = C.af_apl_id_sfx
INNER JOIN (SELECT AF_APL_ID
				,AF_APL_ID_SFX
				,MAX(LD_PCL_RCV) AS LD_PCL_RCV
			FROM OLWHRM1.DC01_LON_CLM_INF
			GROUP BY AF_APL_ID, AF_APL_ID_SFX) D
	ON A.LD_PCL_RCV = D.LD_PCL_RCV
	AND D.AF_APL_ID = A.AF_APL_ID
	AND D.AF_APL_ID_SFX = A.AF_APL_ID_SFX
INNER JOIN (SELECT AF_APL_ID
				,AF_APL_ID_SFX
				,MAX(LD_STA_UPD_DC10) AS LD_STA_UPD_DC10
			FROM OLWHRM1.DC01_LON_CLM_INF
			GROUP BY AF_APL_ID, AF_APL_ID_SFX) E
	ON A.LD_STA_UPD_DC10 = E.LD_STA_UPD_DC10
	AND E.AF_APL_ID = A.AF_APL_ID
	AND E.AF_APL_ID_SFX = A.AF_APL_ID_SFX
INNER JOIN (SELECT AF_APL_ID
				,AF_APL_ID_SFX
				,MAX(LD_BKR_FIL) AS LD_BKR_FIL
			FROM OLWHRM1.DC18_BKR
			GROUP BY AF_APL_ID, AF_APL_ID_SFX) F
	ON C.LD_BKR_FIL = F.LD_BKR_FIL
	AND F.AF_APL_ID = A.AF_APL_ID
	AND F.AF_APL_ID_SFX = A.AF_APL_ID_SFX

WHERE A.LC_PCL_REA = 'BH'
AND A.LC_STA_DC10 IN ('03','04')
AND (B.LD_AGY_BIL_DOE IS NULL
	OR B.LD_AGY_BIL_DOE BETWEEN &BEGIN AND &END
	OR B.LD_AGY_BIL_DOE < 
		(SELECT MAX(Z.LD_LDR_POF)
		FROM OLWHRM1.DC01_LON_CLM_INF Z
		WHERE Z.LC_STA_DC10 IN ('03','04')
		AND Z.LF_CRT_DTS_DC10 = A.LF_CRT_DTS_DC10
		)
	)
ORDER BY B.LD_AGY_BIL_DOE, A.bf_ssn, A.LF_CLM_ID
);
DISCONNECT FROM DB2;
endrsubmit  ;

DATA PENDBKR; 
SET WORKLOCL.PENDBKR; 
RUN;

OPTIONS CENTER NODATE NUMBER LS=/*133*/126 PAGENO=1;

/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
PROC CONTENTS DATA=PENDBKR OUT=EMPTYSET NOPRINT;
    DATA _NULL_;
       SET EMPTYSET;
       FILE PRINT;
       IF  NOBS=0 AND _N_ =1 THEN DO;
           PUT // /*132*/126*'-';
           PUT      ////////
               @51 '**** NO OBSERVATIONS FOUND ****';
           PUT ////////
               @57 '-- END OF REPORT --';
           END;
    RETURN;
TITLE 'PENDING BANKRUPTCY HARDSHIP CLAIMS';
FOOTNOTE  'JOB = UTLWB02     REPORT = ULWB02.LWB02R2';
run;

PROC PRINT NOOBS SPLIT='/' DATA=PENDBKR WIDTH=UNIFORM WIDTH=MIN;
VAR SSN LC_PCL_REA LC_STA_DC10 LF_CLM_ID CLUID LD_LDR_POF CLMPDAMT
LC_BKR_STA LD_BKR_DSM LD_BKR_DCH RQSTDT;
SUM CLMPDAMT;
LABEL LC_PCL_REA='Preclaim Reason' LC_STA_DC10='Claim Status'
LF_CLM_ID='Claim ID' CLUID='Commonline Unique ID' 
LD_LDR_POF='Lender Payoff Date' CLMPDAMT='Claim Paid Amount'
LC_BKR_STA='Bankruptcy Status' LD_BKR_DSM='Bankruptcy Date Dismissed'
LD_BKR_DCH='Bankruptcy Date of Discharge' 
RQSTDT='Reinsurance Billing Date Requested';
FORMAT SSN SSN11. LD_LDR_POF MMDDYY10. CLMPDAMT DOLLAR13.2
LD_BKR_DSM MMDDYY10. LD_BKR_DCH MMDDYY10. RQSTDT MMDDYY10.;
TITLE 'PENDING BANKRUPTCY HARDSHIP CLAIMS';
FOOTNOTE  'JOB = UTLWB02     REPORT = ULWB02.LWB02R2';
RUN;