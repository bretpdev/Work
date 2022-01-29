/*UTLWG81 LOANS DISBURSED NOT MET LATE DISBURSEMENT CRITERIA*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWG81.LWG81R2";
FILENAME REPORTZ "&RPTLIB/ULWG81.LWG81RZ";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DNMLDC AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN 
	,A.LN_SEQ
	,B.LN_BR_DSB_SEQ
	,A.LF_DOE_SCL_ORG
	,A.IF_DOE_LDR
	,A.LD_TRM_END
	,B.LD_DSB
	,B.LA_DSB
	,COALESCE(B.LA_DSB_CAN,0) AS LA_DSB_CAN
	,B.AF_APL_ID
	,D.LD_SCL_SPR
	,FD.FD_IND
	,AD.AD_IND
	,CASE 
		WHEN FD.FD_IND IS NOT NULL AND AD.AD_IND IS NULL THEN 'Y'
		ELSE 'N'
	 END AS FULLY_DISB
	,CASE 
		WHEN A.LD_LON_1_DSB < '07/01/2008' THEN 'Y'
		ELSE 'N'
	 END AS DSB1_LT_JUL1
	,E.DF_SPE_ACC_ID
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.LN15_DSB B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.LN13_LON_STU_OSD C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ
INNER JOIN OLWHRM1.SD10_STU_SPR D
	ON C.LF_STU_SSN = D.LF_STU_SSN
	AND C.LN_STU_SPR_SEQ = D.LN_STU_SPR_SEQ
INNER JOIN OLWHRM1.PD10_PRS_NME E
	ON A.BF_SSN = E.DF_PRS_ID
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,'Y' AS FD_IND 
	FROM OLWHRM1.LN15_DSB 
	WHERE LC_DSB_TYP = '2' 
		AND LC_STA_LON15 IN ('1','3')
		AND COALESCE(LA_DSB_CAN,0) <> LA_DSB
	) FD
	ON A.BF_SSN = FD.BF_SSN
	AND A.LN_SEQ = FD.LN_SEQ
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,'Y' AS AD_IND 
	FROM OLWHRM1.LN15_DSB 
	WHERE LC_DSB_TYP = '1' 
		AND LC_STA_LON15 IN ('1','3')
		AND COALESCE(LA_DSB_CAN,0) <> LA_DSB
	) AD
	ON A.BF_SSN = AD.BF_SSN
	AND A.LN_SEQ = AD.LN_SEQ

WHERE A.LC_STA_LON10 = 'R'
	AND A.LA_CUR_PRI > 0
	AND A.IC_LON_PGM IN ('STFFRD','UNSTFD','PLUS','PLUSGB')
	AND C.LC_STA_LON13 = 'A'
	AND D.LC_STA_STU10 = 'A'
	AND COALESCE(B.LA_DSB_CAN,0) <> B.LA_DSB

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWG81.LWG81RZ);*/
/*QUIT;*/
%MACRO G81_PROC(DS,SET_CRIT,NUM_OF_DAYS);
DATA &DS;
SET DNMLDC;
WHERE DSB1_LT_JUL1 = "&SET_CRIT";
IF FULLY_DISB = 'Y' 
	AND 
	(
		(
			LD_DSB > LD_TRM_END + &NUM_OF_DAYS
		) 
	OR 
		(
			LD_DSB > LD_SCL_SPR + &NUM_OF_DAYS
		)
	) THEN 
		OUTPUT;
ELSE IF FULLY_DISB = 'N' 
	AND 
	(
		(
			LD_DSB > LD_TRM_END + &NUM_OF_DAYS
		) 
	OR 
		(
			LD_DSB > LD_SCL_SPR + &NUM_OF_DAYS
		)
	) THEN
		OUTPUT;
RUN;
%MEND G81_PROC;
%G81_PROC(ASET,Y,120);
%G81_PROC(BSET,N,180);
DATA DNMLDC;
	SET ASET BSET;
RUN;
ENDRSUBMIT;
DATA DNMLDC ;
	SET WORKLOCL.DNMLDC;
RUN;
PROC SORT DATA=DNMLDC;
	BY IF_DOE_LDR LD_DSB;
RUN;
PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 CENTER DATE PAGENO=1;
TITLE 'DISBURSEMENTS NOT MEETING LATE DISBURSEMENT CRITERIA';
FOOTNOTE4   'JOB = UTLWG81  	 REPORT = ULWG81.LWG81R2';
PROC CONTENTS DATA=DNMLDC OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 126*'-';
	PUT      ////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT ////
		@57 '-- END OF REPORT --';
	PUT ////////////
		@46 "JOB = UTLWG81  	 REPORT = ULWG81.LWG81R2";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=DNMLDC WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT LD_DSB LD_SCL_SPR LD_TRM_END MMDDYY10. LA_DSB 12.2;
VAR DF_SPE_ACC_ID AF_APL_ID LN_SEQ LD_DSB LA_DSB LF_DOE_SCL_ORG IF_DOE_LDR LD_SCL_SPR LD_TRM_END;
LABEL DF_SPE_ACC_ID	= 'ACCOUNT/NUMBER'
	AF_APL_ID = 'APP ID'
	LN_SEQ = 'LOAN/SEQ'
	LD_DSB = 'DISB/DATE'
	LA_DSB = 'DISB/AMOUNT'
	LF_DOE_SCL_ORG = 'SCHOOL/CODE'
	IF_DOE_LDR = 'LENDER/CODE'
	LD_SCL_SPR = 'SEPARATION/DATE'
	LD_TRM_END= 'LOAN PERIOD/END DATE';
RUN;
PROC PRINTTO;
RUN;