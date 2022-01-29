*--------------------------------------------*
| UVU SUMMER 2010 							 |
*--------------------------------------------*;
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/UVUSummer2010.R2";
FILENAME REPORT3 "&RPTLIB/UVUSummer2010.R3";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE US2010 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_PRS_ID_BR AS DF_PRS_ID
	,D.DF_SPE_ACC_ID
	,A.AF_CUR_APL_OPS_LDR
	,C.DI_VLD_ADR 
	,C.DI_EML_ADR_VAL 
	,C.DC_ADR 
	,C.DX_EML_ADR
	,D.DM_PRS_1
	,D.DM_PRS_LST
	,C.DX_STR_ADR_1
	,C.DX_STR_ADR_2
	,C.DM_CT
	,C.DC_DOM_ST
	,C.DF_ZIP
	,C.DM_FGN_CNY
FROM OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.PD03_PRS_ADR_PHN C
	ON A.DF_PRS_ID_BR = C.DF_PRS_ID
INNER JOIN OLWHRM1.PD01_PDM_INF D
	ON C.DF_PRS_ID = D.DF_PRS_ID
WHERE A.AF_APL_OPS_SCL = '00402700'
	AND B.AC_PRC_STA = 'A'
	AND B.AD_PRC >= '07/01/2009'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
ENDRSUBMIT;
DATA US2010;
	SET WORKLOCL.US2010;
RUN;
/*SPLIT DATA*/
DATA LTR(DROP=DX_EML_ADR DI_EML_ADR_VAL) EML(KEEP= AF_CUR_APL_OPS_LDR DC_ADR DF_SPE_ACC_ID DM_PRS_1
	DM_PRS_LST DX_EML_ADR DI_EML_ADR_VAL);
	SET US2010;
RUN;
/*R2 PROCESSING*/
PROC SORT DATA=LTR (WHERE=(DC_ADR IN ('L','A') AND AF_CUR_APL_OPS_LDR ^= '828476' AND DI_VLD_ADR = 'Y'));
	BY DF_SPE_ACC_ID DC_ADR;
RUN;
DATA LTR;
	SET LTR;
	BY DF_SPE_ACC_ID DC_ADR;
	IF LAST.DF_SPE_ACC_ID THEN 
		OUTPUT;
RUN;
%MACRO KEY_CLC(TBL);
DATA &TBL (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET &TBL;
KEYSSN = TRANSLATE(DF_PRS_ID,'MYLAUGHTER','0987654321');
MODAY = PUT(DATE(),MMDDYYN4.);
KEYLINE = "P"||KEYSSN||MODAY||DC_ADR;
CHKDIG = 0;
LENGTH DIG $2.;
DO I = 1 TO LENGTH(KEYLINE);
	IF I/2 NE ROUND(I/2,1) 
		THEN DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4) * 2, 2.);
	ELSE DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4), 2.);
	IF SUBSTR(DIG,1,1) = " " 
		THEN CHKDIG = CHKDIG + INPUT(SUBSTR(DIG,2,1),1.);
		ELSE DO;
			CHK1 = INPUT(SUBSTR(DIG,1,1),1.);
			CHK2 = INPUT(SUBSTR(DIG,2,1),1.);
			IF CHK1 + CHK2 >= 10
				THEN DO;
					CHK3 = PUT(CHK1 + CHK2,2.);
					CHK1 = INPUT(SUBSTR(CHK3,1,1),1.);
					CHK2 = INPUT(SUBSTR(CHK3,2,1),1.);
				END;
			CHKDIG = CHKDIG + CHK1 + CHK2;
		END;
END;
CHKDIGIT = 10 - INPUT(SUBSTR((RIGHT(PUT(CHKDIG,3.))),3,1),3.);
IF CHKDIGIT = 10 THEN CHKDIGIT = 0;
CHECK = PUT(CHKDIGIT,1.);
ACSKEY = "#"||KEYLINE||CHECK||"#";
RUN;
%MEND KEY_CLC;
%KEY_CLC(LTR);
PROC SORT DATA=LTR;
	BY DC_DOM_ST DF_SPE_ACC_ID;
RUN;
/*R3 PROCESSING*/
PROC SORT DATA=EML (WHERE=(DC_ADR IN ('L','A') AND AF_CUR_APL_OPS_LDR = '828476' AND DI_EML_ADR_VAL = 'Y'));
	BY DF_SPE_ACC_ID DC_ADR;
RUN;
DATA EML;
	SET EML;
	BY DF_SPE_ACC_ID DC_ADR;
	NAME = CATX(' ',DM_PRS_1,DM_PRS_LST);
	IF LAST.DF_SPE_ACC_ID THEN 
		OUTPUT;
RUN;
/*R2 FILE*/
DATA _NULL_;
SET LTR;
CCC = '50005';
GENX = '';
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT DF_PRS_ID $9. ;
   FORMAT DF_SPE_ACC_ID $10. ;
   FORMAT AF_CUR_APL_OPS_LDR $8. ;
   FORMAT DI_VLD_ADR $1. ;
   FORMAT DC_ADR $1. ;
   FORMAT DM_PRS_1 $12. ;
   FORMAT DM_PRS_LST $35. ;
   FORMAT DX_STR_ADR_1 $35. ;
   FORMAT DX_STR_ADR_2 $35. ;
   FORMAT DM_CT $30. ;
   FORMAT DC_DOM_ST $2. ;
   FORMAT DF_ZIP $14. ;
   FORMAT DM_FGN_CNY $25. ;
   FORMAT ACSKEY $18. ;
   FORMAT CCC $5.;
IF _N_ = 1 THEN DO;
   PUT "ACS_KEYLINE"
	','
	"FIRST_NAME"
	','
	"LAST_NAME"
	','
	"ADDRESS_1"
	','
	"ADDRESS_2"
	','
	"CITY"
	','
	"STATE"
	','
	"ZIP" 
	','
	"COUNTRY"
	','
	"ACCOUNT_NUMBER"
	','
	"CCC" 
	','
	"GEN_1"
	','
	"GEN_2"
	','
	"GEN_3";
END;
DO;
	PUT ACSKEY $ @;
	PUT DM_PRS_1 $ @;
	PUT DM_PRS_LST $ @;
	PUT DX_STR_ADR_1 $ @;
	PUT DX_STR_ADR_2 $ @;
	PUT DM_CT $ @;
	PUT DC_DOM_ST $ @;
	PUT DF_ZIP $ @;
	PUT DM_FGN_CNY $ @;
	PUT DF_SPE_ACC_ID $ @;
	PUT CCC $ @;
	PUT GENX $ @;
	PUT GENX $ @;
	PUT GENX $ ;   
END;
RUN;
/*R3 FILE*/
DATA _NULL_;
	SET EML;
	FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;
	   FORMAT DF_SPE_ACC_ID $10. ;
	   FORMAT DX_EML_ADR $56. ;
	IF _N_ = 1 THEN DO;
	   PUT "ACCOUNT_NUMBER,NAME,EMAIL";
	END;
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT NAME $ @;
		PUT DX_EML_ADR $;   
	END;
RUN;