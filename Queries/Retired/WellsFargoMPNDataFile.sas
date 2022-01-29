*--------------------------------------------*
| Wells Fargo MPN Data File					 |
*--------------------------------------------*;
DATA _NULL_;
	CALL SYMPUT('HEADER_DATE',PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYYS10.));
RUN;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE WFILE AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	D.DF_SPE_ACC_ID
	,A.DF_PRS_ID_BR
	,A.AF_APL_OPS_SCL
	,C.DM_PRS_1
	,C.DM_PRS_LST
	,C.DX_STR_ADR_1
	,C.DX_STR_ADR_2
	,C.DM_CT
	,C.DC_DOM_ST
	,SUBSTR(C.DF_ZIP,1,5) AS ZIP_CDE
	,SUBSTR(C.DF_ZIP,6,4) AS ZIP_SFX
	,C.DN_PHN
	,C.DC_ST_DRV_LIC
	,C.DF_DRV_LIC
	,C.DD_BRT 
	,A.AD_BR_SIG
	,C.DX_EML_ADR
	,C.DM_PRS_MID
	,C.DN_ALT_PHN
/*STUDENT INFO*/
	,D.DM_PRS_1 AS STU_DM_PRS_1
	,D.DM_PRS_LST AS STU_DM_PRS_LST
	,D.DM_PRS_MID AS STU_DM_PRS_MID
	,A.DF_PRS_ID_STU AS DF_PRS_ID_STU
	,D.DD_BRT AS STU_DD_BRT
/*REFERENCE 1 INFO*/
	,REF1.BM_RFR_1 AS REF1_BM_RFR_1
	,REF1.BM_RFR_LST AS REF1_BM_RFR_LST
	,REF1.BM_RFR_MID AS REF1_BM_RFR_MID
	,REF1.BX_RFR_STR_ADR_1 AS REF1_BX_RFR_STR_ADR_1
	,REF1.BX_RFR_STR_ADR_2 AS REF1_BX_RFR_STR_ADR_2
	,REF1.BM_RFR_CT AS REF1_BM_RFR_CT
	,REF1.BC_RFR_ST AS REF1_BC_RFR_ST
	,SUBSTR(REF1.BF_RFR_ZIP,1,5) AS REF1_BF_RFR_ZIP_CDE
	,SUBSTR(REF1.BF_RFR_ZIP,6,4) AS REF1_BF_RFR_ZIP_SFX
	,REF1.BN_RFR_DOM_PHN AS REF1_BN_RFR_DOM_PHN
/*REFERENCE 2 INFO*/
	,REF2.BM_RFR_1 AS REF2_BM_RFR_1
	,REF2.BM_RFR_LST AS REF2_BM_RFR_LST
	,REF2.BM_RFR_MID AS REF2_BM_RFR_MID
	,REF2.BX_RFR_STR_ADR_1 AS REF2_BX_RFR_STR_ADR_1
	,REF2.BX_RFR_STR_ADR_2 AS REF2_BX_RFR_STR_ADR_2
	,REF2.BM_RFR_CT AS REF2_BM_RFR_CT
	,REF2.BC_RFR_ST AS REF2_BC_RFR_ST
	,SUBSTR(REF2.BF_RFR_ZIP,1,5) AS REF2_BF_RFR_ZIP_CDE
	,SUBSTR(REF2.BF_RFR_ZIP,6,4) AS REF2_BF_RFR_ZIP_SFX
	,REF2.BN_RFR_DOM_PHN AS REF2_BN_RFR_DOM_PHN
/*OTHER MISCELLANEOUS INFO*/
	,C.DM_FGN_CNY
	,CASE 
		WHEN A.AC_APL_TYP = 'S' THEN 'M'
		WHEN A.AC_APL_TYP = 'P' THEN 'Q'
	 END AS PNOTE_TYPE
	,'G000749' AS ESIG_CODE
	,F.AD_DSB_ADJ
	,'749' AS GUAR_ID
	,'*' AS EOL
/*UHEAA INFO*/
	,H.DI_VLD_ADR
	,G.DI_EML_ADR_VAL
	,C.DI_PHN_VLD 
	,C.DI_ALT_PHN_VLD
	,C.DF_ZIP
	,I.LOAN_IND
	,J.GUAR_IND
FROM OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.PD01_PDM_INF C
	ON A.DF_PRS_ID_BR = C.DF_PRS_ID
INNER JOIN OLWHRM1.PD01_PDM_INF D
	ON A.DF_PRS_ID_STU = D.DF_PRS_ID
INNER JOIN OLWHRM1.GA40_BS_MPN_CTL E
	ON A.AF_APL_ID = E.AF_BS_MPN_APL_ID
LEFT OUTER JOIN OLWHRM1.BR03_BR_REF REF1
	ON A.DF_PRS_ID_BR = REF1.DF_PRS_ID_BR
LEFT OUTER JOIN OLWHRM1.BR03_BR_REF REF2
	ON A.DF_PRS_ID_BR = REF2.DF_PRS_ID_BR
	AND REF1.DF_PRS_ID_RFR != REF2.DF_PRS_ID_RFR
LEFT OUTER JOIN (
	SELECT AF_APL_ID
		,MIN(AD_DSB_ADJ) AS AD_DSB_ADJ
	FROM OLWHRM1.GA11_LON_DSB_ATY
	WHERE AC_DSB_ADJ = 'A'
		AND AC_DSB_ADJ_STA = 'A'
	GROUP BY AF_APL_ID
	) F
	ON A.AF_APL_ID = F.AF_APL_ID
LEFT OUTER JOIN OLWHRM1.PD03_PRS_ADR_PHN G
	ON C.DF_PRS_ID = G.DF_PRS_ID
	AND C.DX_EML_ADR = G.DX_EML_ADR
LEFT OUTER JOIN OLWHRM1.PD03_PRS_ADR_PHN H
	ON C.DF_PRS_ID = H.DF_PRS_ID
	AND C.DC_ADR = H.DC_ADR
	AND H.DC_ADR = 'L'
LEFT OUTER JOIN (
	SELECT DISTINCT X.DF_PRS_ID_BR AS DF_PRS_ID
		,'X' AS LOAN_IND
	FROM OLWHRM1.GA01_APP X
	INNER JOIN OLWHRM1.GA10_LON_APP Y
		ON X.AF_APL_ID = Y.AF_APL_ID
	INNER JOIN OLWHRM1.GA14_LON_STA Z
		ON Y.AF_APL_ID = Z.AF_APL_ID
		AND Y.AF_APL_ID_SFX = Z.AF_APL_ID_SFX
	WHERE Z.AC_STA_GA14 = 'A'
		AND 
		(
			(
				Z.AC_LON_STA_TYP IN ('IA','IG','ID')
			)
		OR
			(
				Z.AC_LON_STA_TYP = 'DA' AND
				Z.AC_LON_STA_REA IN ('FT','HT')
			)
		)
	) I
	ON C.DF_PRS_ID = I.DF_PRS_ID
LEFT OUTER JOIN (
	SELECT DISTINCT X.DF_PRS_ID_BR AS DF_PRS_ID
		,'X' AS GUAR_IND
	FROM OLWHRM1.GA01_APP X
	INNER JOIN OLWHRM1.GA10_LON_APP Y
		ON X.AF_APL_ID = Y.AF_APL_ID
	WHERE Y.AD_PRC >= '07/01/2007'
		AND Y.AC_PRC_STA = 'A'
		AND Y.AC_LON_TYP IN 
		(
			'SF','SU','PL','GB'
		)
	) J
	ON C.DF_PRS_ID = J.DF_PRS_ID
WHERE E.AC_MPN_STA = 'A'
	AND A.AF_ORG_APL_OPS_LDR IN ('813894','813915')
	AND A.AC_APL_TYP IN ('S','P')
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*GET RID OF EXTRA REFERENCE ROWS AND NON FIRST DISBURSEMENT ROWS*/
PROC SORT DATA=WFILE ;BY DF_PRS_ID_BR DESCENDING AD_DSB_ADJ;RUN;
DATA WFILE;
	SET WFILE;
	BY DF_PRS_ID_BR;
	IF FIRST.DF_PRS_ID_BR THEN OUTPUT;
RUN;
DATA WFILE;
	LENGTH NAME $100.;
	SET WFILE;
	NAME = CATX(' ',DM_PRS_1,DM_PRS_LST);
	CCC = 'MA2330';
	IF DC_DOM_ST IN ('FC','') THEN 
		SVAR = 1;
	ELSE 
		SVAR = 2;
RUN;
%MACRO KILL_QUOTES(VAR);
	DATA WFILE;
		SET WFILE;
		&VAR = TRANWRD(&VAR,'"',' ');
	RUN;
%MEND;
%KILL_QUOTES(REF1_BX_RFR_STR_ADR_1);  
%KILL_QUOTES(REF1_BX_RFR_STR_ADR_2);  
%KILL_QUOTES(REF2_BX_RFR_STR_ADR_1);  
%KILL_QUOTES(REF2_BX_RFR_STR_ADR_2);  
/*CREATE UHEAA FILES*/
PROC SORT DATA=WFILE OUT=R3 (
	WHERE = (DI_VLD_ADR='Y' AND LOAN_IND ='X')
	KEEP = DI_VLD_ADR LOAN_IND DF_PRS_ID_BR NAME DX_STR_ADR_1 DX_STR_ADR_2 DM_CT DC_DOM_ST 
		DF_ZIP DM_FGN_CNY DF_SPE_ACC_ID DC_DOM_ST CCC
	);
	BY SVAR DC_DOM_ST;
RUN;
PROC SORT DATA=WFILE OUT=R4 (
	WHERE = (DI_EML_ADR_VAL='Y' AND LOAN_IND ='X')
	KEEP = DI_EML_ADR_VAL LOAN_IND DF_PRS_ID_BR NAME DX_EML_ADR DF_SPE_ACC_ID 
	);
	BY DF_PRS_ID_BR;
RUN;
PROC SORT DATA=WFILE OUT=R5 (
	WHERE = (GUAR_IND ='X' AND (DI_PHN_VLD = 'Y' OR DI_ALT_PHN_VLD= 'Y'))
	KEEP = DF_PRS_ID_BR GUAR_IND DI_PHN_VLD DI_ALT_PHN_VLD
	);
	BY DF_PRS_ID_BR;
RUN;
ENDRSUBMIT;
/*WELLS FILE*/
DATA WFILE;SET WORKLOCL.WFILE;RUN;
/*UHEAA FILES*/
DATA R3;SET WORKLOCL.R3;RUN;
DATA R4;SET WORKLOCL.R4;RUN;
DATA R5;SET WORKLOCL.R5;RUN;
DATA _NULL_;
	SET WFILE;
	CALL SYMPUT('RECS',TRIM(LEFT(_N_)));
RUN;
/*WELLS FILE*/
DATA _NULL_;
SET  WORK.WFILE;
FILE "T:\SAS\SchoolMPN_00367400.txt" DELIMITER='|' DSD DROPOVER LRECL=32767;
	FORMAT DF_PRS_ID_BR $9. ;
	FORMAT AF_APL_OPS_SCL $8. ;
	FORMAT DM_PRS_1 $12. ;
	FORMAT DM_PRS_LST $35. ;
	FORMAT DX_STR_ADR_1 $35. ;
	FORMAT DX_STR_ADR_2 $35. ;
	FORMAT DM_CT $30. ;
	FORMAT DC_DOM_ST $2. ;
	FORMAT ZIP_CDE $5. ;
	FORMAT ZIP_SFX $4. ;
	FORMAT DN_PHN $17. ;
	FORMAT DC_ST_DRV_LIC $2. ;
	FORMAT DF_DRV_LIC $20. ;
	FORMAT DD_BRT MMDDYY10. ;
	FORMAT AD_BR_SIG MMDDYY10. ;
	FORMAT DX_EML_ADR $56. ;
	FORMAT DM_PRS_MID $1. ;
	FORMAT DN_ALT_PHN $17. ;
	FORMAT STU_DM_PRS_1 $12. ;
	FORMAT STU_DM_PRS_LST $35. ;
	FORMAT STU_DM_PRS_MID $1. ;
	FORMAT DF_PRS_ID_STU $9. ;
	FORMAT STU_DD_BRT MMDDYY10. ;
	FORMAT REF1_BM_RFR_1 $12. ;
	FORMAT REF1_BM_RFR_LST $35. ;
	FORMAT REF1_BM_RFR_MID $1. ;
	FORMAT REF1_BX_RFR_STR_ADR_1 $35. ;
	FORMAT REF1_BX_RFR_STR_ADR_2 $35. ;
	FORMAT REF1_BM_RFR_CT $30. ;
	FORMAT REF1_BC_RFR_ST $2. ;
	FORMAT REF1_BF_RFR_ZIP_CDE $5. ;
	FORMAT REF1_BF_RFR_ZIP_SFX $4. ;
	FORMAT REF1_BN_RFR_DOM_PHN $10. ;
	FORMAT REF2_BM_RFR_1 $12. ;
	FORMAT REF2_BM_RFR_LST $35. ;
	FORMAT REF2_BM_RFR_MID $1. ;
	FORMAT REF2_BX_RFR_STR_ADR_1 $35. ;
	FORMAT REF2_BX_RFR_STR_ADR_2 $35. ;
	FORMAT REF2_BM_RFR_CT $30. ;
	FORMAT REF2_BC_RFR_ST $2. ;
	FORMAT REF2_BF_RFR_ZIP_CDE $5. ;
	FORMAT REF2_BF_RFR_ZIP_SFX $4. ;
	FORMAT REF2_BN_RFR_DOM_PHN $10. ;
	FORMAT DM_FGN_CNY $25. ;
	FORMAT PNOTE_TYPE $1. ;
	FORMAT ESIG_CODE $7. ;
	FORMAT AD_DSB_ADJ MMDDYY10. ;
	FORMAT GUAR_ID $3. ;
	FORMAT EOL $1. ;
IF _N_ = 1 THEN DO; *CREATE HEADER ROW;
	PUT "SchoolMPN_00367400.txt|&RECS|&HEADER_DATE|&HEADER_DATE|N";
END;
DO;
	PUT DF_PRS_ID_BR $ @;
	PUT AF_APL_OPS_SCL $ @;
	PUT DM_PRS_1 $ @;
	PUT DM_PRS_LST $ @;
	PUT DX_STR_ADR_1 $ @;
	PUT DX_STR_ADR_2 $ @;
	PUT DM_CT $ @;
	PUT DC_DOM_ST $ @;
	PUT ZIP_CDE $ @;
	PUT ZIP_SFX $ @;
	PUT DN_PHN $ @;
	PUT DC_ST_DRV_LIC $ @;
	PUT DF_DRV_LIC $ @;
	PUT DD_BRT @;
	PUT AD_BR_SIG @;
	PUT DX_EML_ADR $ @;
	PUT DM_PRS_MID $ @;
	PUT DN_ALT_PHN $ @;
	PUT STU_DM_PRS_1 $ @;
	PUT STU_DM_PRS_LST $ @;
	PUT STU_DM_PRS_MID $ @;
	PUT DF_PRS_ID_STU $ @;
	PUT STU_DD_BRT @;
	PUT REF1_BM_RFR_1 $ @;
	PUT REF1_BM_RFR_LST $ @;
	PUT REF1_BM_RFR_MID $ @;
	PUT REF1_BX_RFR_STR_ADR_1 $ @;
	PUT REF1_BX_RFR_STR_ADR_2 $ @;
	PUT REF1_BM_RFR_CT $ @;
	PUT REF1_BC_RFR_ST $ @;
	PUT REF1_BF_RFR_ZIP_CDE $ @;
	PUT REF1_BF_RFR_ZIP_SFX $ @;
	PUT REF1_BN_RFR_DOM_PHN $ @;
	PUT REF2_BM_RFR_1 $ @;
	PUT REF2_BM_RFR_LST $ @;
	PUT REF2_BM_RFR_MID $ @;
	PUT REF2_BX_RFR_STR_ADR_1 $ @;
	PUT REF2_BX_RFR_STR_ADR_2 $ @;
	PUT REF2_BM_RFR_CT $ @;
	PUT REF2_BC_RFR_ST $ @;
	PUT REF2_BF_RFR_ZIP_CDE $ @;
	PUT REF2_BF_RFR_ZIP_SFX $ @;
	PUT REF2_BN_RFR_DOM_PHN $ @;
	PUT DM_FGN_CNY $ @;
	PUT PNOTE_TYPE $ @;
	PUT ESIG_CODE $ @;
	PUT AD_DSB_ADJ @;
	PUT GUAR_ID $ @;
	PUT EOL $ ;
END;
RUN;
/*R3 REPORT*/
DATA _NULL_;
SET  R3;
FILE "T:\SAS\wellsdata.r3" DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT DF_PRS_ID_BR $9. ;
	FORMAT NAME $100. ;
	FORMAT DX_STR_ADR_1 $35. ;
	FORMAT DX_STR_ADR_2 $35. ;
	FORMAT DM_CT $30. ;
	FORMAT DC_DOM_ST $2. ;
	FORMAT DF_ZIP $13. ;
	FORMAT DM_FGN_CNY $25. ;
	FORMAT DF_SPE_ACC_ID $10. ;
	FORMAT CCC $6. ;
IF _N_ = 1 THEN DO; 
	PUT "SSN,NAME,ADDR1,ADDR2,CITY,STATE,ZIP,COUNTRY,ACCOUNT_NUMBER,STATE_IND,COST_CENTER_CODE";
END;
DO;
	PUT DF_PRS_ID_BR $ @ ;
	PUT NAME $ @ ;
	PUT DX_STR_ADR_1 $ @ ;
	PUT DX_STR_ADR_2 $ @ ;
	PUT DM_CT $ @ ;
	PUT DC_DOM_ST $ @ ;
	PUT DF_ZIP $ @ ;
	PUT DM_FGN_CNY $ @ ;
	PUT DF_SPE_ACC_ID $ @;
	PUT DC_DOM_ST $ @ ;
	PUT CCC $ ;
END;
RUN;
/*R4 REPORT*/
DATA _NULL_;
SET  R4;
FILE "T:\SAS\wellsdata.r4" DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT DF_PRS_ID_BR $9. ;
	FORMAT NAME $100. ;
	FORMAT DX_EML_ADR $56. ;
	FORMAT DF_SPE_ACC_ID $10. ;
IF _N_ = 1 THEN DO; 
	PUT "SSN,NAME,EMAIL,ACCOUNT_NUMBER";
END;
DO;
	PUT DF_PRS_ID_BR $ @ ;
	PUT NAME $ @ ;
	PUT DX_EML_ADR $ @;
	PUT DF_SPE_ACC_ID $ ;
END;
RUN;
/*R5 REPORT*/
DATA _NULL_;
SET  R5;
FILE "T:\SAS\wellsdata.r5" DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT DF_PRS_ID_BR $9. ;
IF _N_ = 1 THEN DO; 
	PUT "SSN";
END;
DO;
	PUT DF_PRS_ID_BR $  ;
END;
RUN;
