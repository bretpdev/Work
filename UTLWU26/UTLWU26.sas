/*QC - invalid e-mail addresses*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWU26.LWU26RZ";
FILENAME REPORT2 "&RPTLIB/ULWU26.LWU26R2";
FILENAME REPORT3 "&RPTLIB/ULWU26.LWU26R3";
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
CREATE TABLE OL AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT D.DF_SPE_ACC_ID 
	,E.DX_EML_ADR AS EML_ADDSS	
	,E.DD_EML_ADR_EFF AS UPDATED
	,F.BF_LST_USR_AY01 AS USER_ID
	,'O' AS SYS
FROM OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.GA14_LON_STA C
	ON B.AF_APL_ID = C.AF_APL_ID
	AND B.AF_APL_ID_SFX = C.AF_APL_ID_SFX
INNER JOIN OLWHRM1.PD01_PDM_INF D
	ON A.DF_PRS_ID_BR = D.DF_PRS_ID
INNER JOIN OLWHRM1.PD03_PRS_ADR_PHN E
	ON D.DF_PRS_ID = E.DF_PRS_ID
LEFT JOIN OLWHRM1.AY01_BR_ATY F
	ON E.DF_PRS_ID = F.DF_PRS_ID
	AND F.BD_ATY_PRF = E.DD_EML_ADR_EFF 
	AND F.PF_ACT = 'MXADD'
WHERE E.DX_EML_ADR <> '' 
	AND E.DI_EML_ADR_VAL = 'Y'
	AND C.AC_STA_GA14 = 'A'
	AND C.AC_LON_STA_TYP IN 
	(
		'CP','CR','DA','DN','FB','IA','ID','IG',
		'IM','PC','RP','UA','UB','UC','UD'
	)
FOR READ ONLY WITH UR
);

CREATE TABLE CO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT B.DF_SPE_ACC_ID
	,C.DX_ADR_EML AS EML_ADDSS
	,DATE(C.DF_LST_DTS_PD32) AS UPDATED
	,C.DF_LST_USR_PD32 AS USER_ID
	,'C' AS SYS
FROM OLWHRM1.PD10_PRS_NME B
INNER JOIN OLWHRM1.PD32_PRS_ADR_EML C
	ON B.DF_PRS_ID = C.DF_PRS_ID
INNER JOIN (
	SELECT BF_SSN
		,SUM(LA_CUR_PRI) AS LA_CUR_PRI
	FROM OLWHRM1.LN10_LON 
	WHERE LC_STA_LON10 = 'R' 
	GROUP BY BF_SSN
	HAVING SUM(LA_CUR_PRI) > 50
	) A 
	ON A.BF_SSN = C.DF_PRS_ID
WHERE C.DX_ADR_EML != ''
	AND C.DI_VLD_ADR_EML = 'Y'
	AND C.DC_STA_PD32 = 'A'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/
DATA ASYS ;
	SET OL CO;
	WHERE INDEX(CATS(REVERSE(EML_ADDSS)) ,'.') > INDEX(CATS(REVERSE(EML_ADDSS)) ,'@') |
		INDEX(EML_ADDSS,'<') ^= 0 | INDEX(EML_ADDSS,'(') ^= 0 | INDEX(EML_ADDSS,')') ^= 0 | 
		INDEX(EML_ADDSS,'{') ^= 0 | INDEX(EML_ADDSS,'}') ^= 0 | INDEX(EML_ADDSS,'[') ^= 0 | 
		INDEX(EML_ADDSS,']') ^= 0 | INDEX(EML_ADDSS,',') ^= 0 | INDEX(EML_ADDSS,';') ^= 0 | 
		INDEX(EML_ADDSS,':') ^= 0 | INDEX(EML_ADDSS,'{') ^= 0 | INDEX(EML_ADDSS,'}') ^= 0 | 
		INDEX(EML_ADDSS,'>') ^= 0 | INDEX(TRIM(EML_ADDSS),' ') ^= 0 | INDEX(EML_ADDSS,'@') = 0 | 
		INDEX(EML_ADDSS,'%%') ^= 0 | INDEX(EML_ADDSS,'@@') ^= 0 | INDEX(EML_ADDSS,'..') ^= 0 
	;
RUN;
ENDRSUBMIT;
DATA ASYS;
	SET WORKLOCL.ASYS;
RUN;
%MACRO U26_REPS(SIND,RNO,NAME);
PROC SORT DATA=ASYS OUT=REPDS (WHERE=(SYS= "&SIND"));
	BY DF_SPE_ACC_ID;
RUN;

DATA _NULL_;
SET REPDS ;
LENGTH DESCRIPTION $600.;
USER = USER_ID;
ACT_DT = UPDATED;
DESCRIPTION = CATX(',',
	'BORROWER ACCOUNT #:' || DF_SPE_ACC_ID,
	"&NAME EMAIL ADDRESS:" || EML_ADDSS
);
FILE REPORT&RNO DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT USER $10. ;
FORMAT ACT_DT MMDDYY10. ;
FORMAT DESCRIPTION $600. ;
IF _N_ = 1 THEN DO;
	PUT "USER,ACT_DT,DESCRIPTION";
END;
DO;
   PUT USER $ @;
   PUT ACT_DT @;
   PUT DESCRIPTION $ ;
END;
RUN;

%MEND U26_REPS;
%U26_REPS(O,2,ONELINK);
%U26_REPS(C,3,COMPASS);
