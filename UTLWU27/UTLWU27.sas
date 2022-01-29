*-----------------------------------*
| UTLWU27 Duplicate Reference Added |
*-----------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWU27.LWU27R2";
FILENAME REPORTZ "&RPTLIB/ULWU27.LWU27RZ";
DATA _NULL_;
	CALL SYMPUT('DAYS_AGO_1',"'"||PUT(INTNX('DAY',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;
%SYSLPUT DAYS_AGO_1 = &DAYS_AGO_1;
LIBNAME WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
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
CREATE TABLE DREFA AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT AX.DF_PRS_ID_BR
	,AX.BF_LST_USR_BR03
	,DATE(AX.BF_LST_DTS_BR03) AS BF_LST_DTS_BR03
	,AX.DF_PRS_ID_RFR
	,AX.BM_RFR_1
	,AX.BM_RFR_LST
	,AX.BC_RFR_REL_BR
	,AX.BN_RFR_DOM_PHN
	,AX.BX_RFR_STR_ADR_1
	,AX.BX_RFR_STR_ADR_2
	,AX.BM_RFR_CT
	,AX.BC_RFR_ST
	,AX.BF_RFR_ZIP
FROM OLWHRM1.BR03_BR_REF AX
INNER JOIN (
	SELECT A.DF_PRS_ID_BR
		,A.BN_RFR_DOM_PHN
		,A.BM_RFR_1
		,A.BM_RFR_LST
		,A.BX_RFR_STR_ADR_1
		,A.BC_STA_BR03
	FROM OLWHRM1.BR03_BR_REF A
	INNER JOIN OLWHRM1.BR03_BR_REF B
		ON A.DF_PRS_ID_BR = B.DF_PRS_ID_BR
		AND A.BN_RFR_DOM_PHN = B.BN_RFR_DOM_PHN
		AND A.BC_STA_BR03 = B.BC_STA_BR03
	LEFT OUTER JOIN OLWHRM1.AY01_BR_ATY AY01A
		ON A.DF_PRS_ID_RFR = AY01A.DF_PRS_ID
		AND AY01A.PF_ACT = 'KUBA4'
	LEFT OUTER JOIN OLWHRM1.AY01_BR_ATY AY01B
		ON B.DF_PRS_ID_RFR = AY01B.DF_PRS_ID
		AND AY01B.PF_ACT = 'KUBA4'
	WHERE DATE(A.BF_LST_DTS_BR03) = &DAYS_AGO_1
		AND A.DF_PRS_ID_RFR != B.DF_PRS_ID_RFR
		AND A.BC_STA_BR03 = 'A'
		AND AY01A.PF_ACT IS NULL
		AND AY01B.PF_ACT IS NULL
	) BX
	ON AX.DF_PRS_ID_BR = BX.DF_PRS_ID_BR
	AND (
		AX.BN_RFR_DOM_PHN = BX.BN_RFR_DOM_PHN
		OR (AX.BM_RFR_1 = BX.BM_RFR_1 AND AX.BM_RFR_LST = BX.BM_RFR_LST)
		OR AX.BX_RFR_STR_ADR_1 = BX.BX_RFR_STR_ADR_1
	)
	AND AX.BC_STA_BR03 = BX.BC_STA_BR03
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWU27.LWU27RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA DREFA ;
SET WORKLOCL.DREFA;
RUN;
PROC FORMAT;
	VALUE $REL 'E' = 'Employer'
		'EM' = 'Employer'
		'F' = 'Friend'
		'FR' = 'Friend'
		'G' = 'Guardian'
		'GU' = 'Guardian'
		'M' = 'Spouse'
		'N' = 'Not available'
		'NE' = 'Neighbor'
		'O' = 'Other'
		'OT' = 'Other'
		'P' = 'Parent'
		'PA' = 'Parent'
		'R' = 'Relative'
		'RE' = 'Relative'
		'RM' = 'Roommate'
		'S' = 'Sibling'
		'SI' = 'Sibling'          
		'SP' = 'Spouse';
QUIT;
PROC SORT DATA=DREFA; 
	BY BN_RFR_DOM_PHN DESCENDING BF_LST_DTS_BR03;
RUN;
DATA _NULL_;
SET DREFA ;
LENGTH DESCRIPTION $600.;
USER = BF_LST_USR_BR03;
ACT_DT = BF_LST_DTS_BR03;
DESCRIPTION = CATX(',',
	'PERSON ID OF REFERENCE = '||DF_PRS_ID_RFR,
	'FIRST NAME = '||BM_RFR_1,
	'LAST NAME = '||BM_RFR_LST,   
	'RELATIONSHIP = '||PUT(BC_RFR_REL_BR,$REL.),
	'PHONE = '||BN_RFR_DOM_PHN,
	'STREET = '||BX_RFR_STR_ADR_1,
	'STREET2 = '||BX_RFR_STR_ADR_2,
	'CITY = '||BM_RFR_CT,
	'STATE = '||BC_RFR_ST,
	'ZIP = '||BF_RFR_ZIP
);
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
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
