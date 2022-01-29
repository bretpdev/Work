/*Daily ending balance for lenders*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:/SAS;*/
FILENAME REPORTZ "&RPTLIB/ULWL02.LWL02RZ";
FILENAME REPORT2 "&RPTLIB/ULWL02.LWL02R2";
FILENAME REPORT3 "&RPTLIB/ULWL02.LWL02R3";
FILENAME REPORT4 "&RPTLIB/ULWL02.LWL02R4";
FILENAME REPORT5 "&RPTLIB/ULWL02.LWL02R5";
FILENAME REPORT6 "&RPTLIB/ULWL02.LWL02R6";
FILENAME REPORT7 "&RPTLIB/ULWL02.LWL02R7";
FILENAME REPORT8 "&RPTLIB/ULWL02.LWL02R8";
FILENAME REPORT9 "&RPTLIB/ULWL02.LWL02R9";
FILENAME REPORT10 "&RPTLIB/ULWL02.LWL02R10";
FILENAME REPORT11 "&RPTLIB/ULWL02.LWL02R11";
FILENAME REPORT12 "&RPTLIB/ULWL02.LWL02R12";
FILENAME REPORT13 "&RPTLIB/ULWL02.LWL02R13";
FILENAME REPORT14 "&RPTLIB/ULWL02.LWL02R14";
FILENAME REPORT15 "&RPTLIB/ULWL02.LWL02R15";
FILENAME REPORT16 "&RPTLIB/ULWL02.LWL02R16";
FILENAME REPORT17 "&RPTLIB/ULWL02.LWL02R17";
FILENAME REPORT18 "&RPTLIB/ULWL02.LWL02R18";
FILENAME REPORT19 "&RPTLIB/ULWL02.LWL02R19";
FILENAME REPORT20 "&RPTLIB/ULWL02.LWL02R20";
FILENAME REPORT21 "&RPTLIB/ULWL02.LWL02R21";
FILENAME REPORT22 "&RPTLIB/ULWL02.LWL02R22";
FILENAME REPORT23 "&RPTLIB/ULWL02.LWL02R23";
FILENAME REPORT24 "&RPTLIB/ULWL02.LWL02R24";
FILENAME REPORT25 "&RPTLIB/ULWL02.LWL02R25";
FILENAME REPORT26 "&RPTLIB/ULWL02.LWL02R26";
FILENAME REPORT27 "&RPTLIB/ULWL02.LWL02R27";
FILENAME REPORT28 "&RPTLIB/ULWL02.LWL02R28";
FILENAME REPORT29 "&RPTLIB/ULWL02.LWL02R29";
FILENAME REPORT30 "&RPTLIB/ULWL02.LWL02R30";
FILENAME REPORT31 "&RPTLIB/ULWL02.LWL02R31";
FILENAME REPORT32 "&RPTLIB/ULWL02.LWL02R32";
FILENAME REPORT33 "&RPTLIB/ULWL02.LWL02R33";
FILENAME REPORT34 "&RPTLIB/ULWL02.LWL02R34";
FILENAME REPORT35 "&RPTLIB/ULWL02.LWL02R35";
FILENAME REPORT36 "&RPTLIB/ULWL02.LWL02R36";
FILENAME REPORT37 "&RPTLIB/ULWL02.LWL02R37";
FILENAME REPORT38 "&RPTLIB/ULWL02.LWL02R38";
FILENAME REPORT39 "&RPTLIB/ULWL02.LWL02R39";
FILENAME REPORT40 "&RPTLIB/ULWL02.LWL02R40";
FILENAME REPORT41 "&RPTLIB/ULWL02.LWL02R41";
FILENAME REPORT42 "&RPTLIB/ULWL02.LWL02R42";
FILENAME REPORT43 "&RPTLIB/ULWL02.LWL02R43";
FILENAME REPORT44 "&RPTLIB/ULWL02.LWL02R44";
FILENAME REPORT45 "&RPTLIB/ULWL02.LWL02R45";
FILENAME REPORT46 "&RPTLIB/ULWL02.LWL02R46";
FILENAME REPORT47 "&RPTLIB/ULWL02.LWL02R47";
FILENAME REPORT48 "&RPTLIB/ULWL02.LWL02R48";
FILENAME REPORT49 "&RPTLIB/ULWL02.LWL02R49";
FILENAME REPORT50 "&RPTLIB/ULWL02.LWL02R50";
FILENAME REPORT51 "&RPTLIB/ULWL02.LWL02R51";
FILENAME REPORT52 "&RPTLIB/ULWL02.LWL02R52";
FILENAME REPORT53 "&RPTLIB/ULWL02.LWL02R53";
FILENAME REPORT54 "&RPTLIB/ULWL02.LWL02R54";
FILENAME REPORT55 "&RPTLIB/ULWL02.LWL02R55";
FILENAME REPORT56 "&RPTLIB/ULWL02.LWL02R56";
FILENAME REPORT57 "&RPTLIB/ULWL02.LWL02R57";
FILENAME REPORT58 "&RPTLIB/ULWL02.LWL02R58";
FILENAME REPORT59 "&RPTLIB/ULWL02.LWL02R59";
FILENAME REPORT60 "&RPTLIB/ULWL02.LWL02R60";
FILENAME REPORT61 "&RPTLIB/ULWL02.LWL02R61";
FILENAME REPORT62 "&RPTLIB/ULWL02.LWL02R62";
FILENAME REPORT63 "&RPTLIB/ULWL02.LWL02R63";
FILENAME REPORT64 "&RPTLIB/ULWL02.LWL02R64";
FILENAME REPORT65 "&RPTLIB/ULWL02.LWL02R65";

DATA _NULL_;
	CALL SYMPUT('YDAY',TRIM(LEFT(UPCASE(PUT(INTNX('DAY',TODAY(),-1), WORDDATE.)))));
RUN;
/*LIBNAME  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=WORK  ;*/
/*RSUBMIT;*/
%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE EBI AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT LN10.BF_SSN
	,LN10.LN_SEQ 
	,LN10.IC_LON_PGM
	,LN10.LD_LON_1_DSB
	,LN10.LF_LON_CUR_OWN
	,LR10.IM_LDR_FUL
	,COALESCE(LN10.LA_CUR_PRI,0) AS PRIN_BAL
	,COALESCE(LN10.LA_NSI_OTS,0) AS INTR_BAL
	,LN10.IF_TIR_PCE
	,LN10.LF_RGL_CAT_LP20
	,LN10.IC_LON_PGM
	,CASE
		WHEN LN35.IF_BND_ISS IS NULL THEN 'BLANK' 
		ELSE LN35.IF_BND_ISS
	 END AS IF_BND_ISS 
	,CASE 
	 	WHEN LN10.LD_LON_1_DSB < '10/01/2007' THEN 'N'
		ELSE 'Y'
	 END AS OCT1_IND
	,CASE 
	 	WHEN LN10.LD_LON_1_DSB < '07/01/2008' THEN 'N'
		ELSE 'Y'
	 END AS JUL1_IND
	,LN10.LD_LON_GTR
	,LN10.LA_LON_AMT_GTR
	,SD10.LD_SCL_SPR
	,LN10.LD_TRM_BEG
	,LN10.LD_TRM_END
	,LN10.LC_ACA_GDE_LEV
	,LN10.LF_DOE_SCL_ORG
	,SD10.LF_DOE_SCL_ENR_CUR
	,SC10.IM_SCL_FUL
	,LN10.LR_ITR_ORG
	,LN10.LA_LTE_FEE_OTS
	,LN10.IF_DOE_LDR
	,LN72.LR_ITR
	,LNEC.LF_ECA_PGM_YR
	,DW01.WC_DW_LON_STA
	,DW01.WD_LON_RPD_SR
	,DW01.WD_XPC_POF_TS26

	,CASE
		WHEN CP.HAS_CON_PAY IS NULL THEN 'N'
		ELSE CP.HAS_CON_PAY
	 END AS HAS_CON_PAY

	,DAYS(CURRENT DATE) - DAYS(LD_DLQ_OCC) AS DAYS_DELQ
	,SC01.IC_SCL_TYP

	,LNEC.LC_ECA_PUT_STA
	,LNEC.LD_ECA_LON_PUT_DOE

	,LN10.LC_ELG_95_SPA_BIL
	,LN83.LR_EFT_RDC

	,LN84.LR_RDC

	,LNEC.LC_LON_CDU_STA
	,LNEC.LD_LON_CDU_BIL_SLE 
	,LNEC.LN_LON_CDU_DAY_DLQ
	,LNEC.LD_CDU_ANT_PUT


FROM OLWHRM1.LN10_LON LN10
INNER JOIN OLWHRM1.LR10_LDR_DMO LR10
	ON LR10.IF_DOE_LDR = LN10.LF_LON_CUR_OWN
LEFT OUTER JOIN OLWHRM1.LN35_LON_OWN LN35
	ON LN10.BF_SSN = LN35.BF_SSN
	AND LN10.LN_SEQ = LN35.LN_SEQ
	AND LN10.LF_LON_CUR_OWN = LN35.IF_OWN
	AND LN35.LC_STA_LON35 = 'A'
	AND LN35.LD_OWN_EFF_END IS NULL
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LF_STU_SSN
		,LN_SEQ
		,MAX(LN_STU_SPR_SEQ) AS LN_STU_SPR_SEQ
	FROM OLWHRM1.LN13_LON_STU_OSD 
	WHERE LC_STA_LON13 = 'A'
	GROUP BY BF_SSN
		,LF_STU_SSN
		,LN_SEQ
	) LN13
	ON LN10.BF_SSN = LN13.BF_SSN
	AND LN10.LN_SEQ = LN13.LN_SEQ
LEFT OUTER JOIN OLWHRM1.SD10_STU_SPR SD10
	ON LN13.LF_STU_SSN = SD10.LF_STU_SSN
	AND LN13.LN_STU_SPR_SEQ = SD10.LN_STU_SPR_SEQ
LEFT OUTER JOIN OLWHRM1.SC10_SCH_DMO SC10
	ON SD10.LF_DOE_SCL_ENR_CUR = SC10.IF_DOE_SCL
LEFT OUTER JOIN (
	SELECT LN72.BF_SSN
		,LN72.LN_SEQ
		,LN72.LR_ITR
	FROM OLWHRM1.LN72_INT_RTE_HST LN72
	WHERE LN72.LC_STA_LON72 = 'A'
		AND LN72.LD_ITR_EFF_BEG <= CURRENT DATE
		AND LN72.LD_ITR_EFF_END >= CURRENT DATE
	) LN72
	ON LN10.BF_SSN = LN72.BF_SSN
	AND LN10.LN_SEQ = LN72.LN_SEQ
LEFT OUTER JOIN OLWHRM1.LNEC_LON_ECA LNEC
	ON LN10.BF_SSN = LNEC.BF_SSN
	AND LN10.LN_SEQ = LNEC.LN_SEQ
	AND LNEC.LN_ECA_SEQ = 1
LEFT OUTER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
	ON LN10.BF_SSN = DW01.BF_SSN
	AND LN10.LN_SEQ = DW01.LN_SEQ
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,'Y' AS HAS_CON_PAY
	FROM OLWHRM1.LN90_FIN_ATY 
	WHERE LC_FAT_REV_REA = ' '
		AND LC_STA_LON90 = 'A'
		AND	PC_FAT_TYP = '10'
		AND PC_FAT_SUB_TYP IN ('70','80')
	) CP
	ON LN10.BF_SSN = CP.BF_SSN
	AND LN10.LN_SEQ = CP.LN_SEQ
LEFT OUTER JOIN OLWHRM1.LN16_LON_DLQ_HST LN16
	ON LN10.BF_SSN = LN16.BF_SSN
	AND LN10.LN_SEQ = LN16.LN_SEQ
	AND LN16.LC_STA_LON16 = '1'

LEFT OUTER JOIN (
	SELECT DISTINCT IF_IST
		,IC_SCL_TYP
	FROM OLWHRM1.SC01_LGS_SCL_INF 
	) SC01 
	ON LN10.LF_DOE_SCL_ORG = SC01.IF_IST	

LEFT OUTER JOIN OLWHRM1.LN83_EFT_TO_LON LN83
	ON LN10.BF_SSN = LN83.BF_SSN
	AND LN10.LN_SEQ = LN83.LN_SEQ
	AND LN83.LC_STA_LN83 = 'A'

LEFT OUTER JOIN OLWHRM1.LN84_LON_RTE_RDC LN84
	ON LN10.BF_SSN = LN84.BF_SSN
	AND LN10.LN_SEQ = LN84.LN_SEQ
	AND LN84.LC_STA_LON84 = 'A'	

WHERE LN10.LC_STA_LON10 IN ('R','L','D') 
	AND (LN10.LA_CUR_PRI <> 0 OR LN10.LA_NSI_OTS <> 0)
);

CREATE TABLE LN15 AS
SELECT B.*
FROM EBI A 
INNER JOIN CONNECTION TO DB2 (
	SELECT BF_SSN
		,LN_SEQ
		,LN_BR_DSB_SEQ
		,LC_STA_LON15 
		,LA_DSB - COALESCE(LA_DSB_CAN,0) AS LA_DSB
		,LD_DSB 
		,IC_LON_PGM 
		,LC_DSB_TYP
		,LD_DSB_ROS_PRT
	FROM OLWHRM1.LN15_DSB
	WHERE LA_DSB <> COALESCE(LA_DSB_CAN,0)
		AND LC_STA_LON15 IN ('1','3')
	) B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
;

CREATE TABLE LN18 AS
SELECT B.*
FROM EBI A 
INNER JOIN CONNECTION TO DB2 (
	SELECT A.BF_SSN
		,A.LN_SEQ
		,B.LC_DSB_FEE
		,SUM(LA_DSB_FEE) AS LA_DSB_FEE
	FROM OLWHRM1.LN15_DSB A
	INNER JOIN OLWHRM1.LN18_DSB_FEE B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_BR_DSB_SEQ = B.LN_BR_DSB_SEQ
	WHERE B.LC_DSB_FEE IN ('02','21')
	GROUP BY A.BF_SSN
		,A.LN_SEQ
		,B.LC_DSB_FEE
	) B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
;

CREATE TABLE OFCAL AS
SELECT B.*
FROM EBI A 
INNER JOIN CONNECTION TO DB2 (
	SELECT BF_SSN
		,LN_SEQ
		,LN_FAT_SEQ
		,LD_FAT_EFF
		,PC_FAT_TYP||PC_FAT_SUB_TYP AS TRX
		,COALESCE(LA_FAT_CUR_PRI,0) AS TRX_AMT
	FROM OLWHRM1.LN90_FIN_ATY 
	WHERE LC_FAT_REV_REA = ' '
		AND LC_STA_LON90 = 'A'
		AND
		(
			(
				PC_FAT_TYP = '10'
				AND PC_FAT_SUB_TYP IN 
				(
					'10','11','12','35','50','70',
					'80','38','20','21','36','37'
				)
			)
		OR	
			(
				PC_FAT_TYP = '70'
				AND PC_FAT_SUB_TYP = '01'
			)
		OR	
			(
				PC_FAT_TYP = '26'
				AND PC_FAT_SUB_TYP = '01'
			)
		)
	) B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ;
DISCONNECT FROM DB2;

PROC SQL;
CREATE TABLE EBI1 AS
SELECT DISTINCT A.*
	,PL_DISB.I_IND
	,CASE 
		WHEN FULLY_DISB.FDSB_IND IS NULL THEN 'Y'
		ELSE 'N'
	 END AS FDSB_IND
	,ACT.MAX_ACT_DISB_DT
	,ACT.MIN_ACT_DISB_DT
	,ANT.MAX_ANT_DISB_DT
	,ANT.ANT_DSB_AMT
	,COALESCE(ORG.OFEE,0) AS OFEE
	,COALESCE(GTY.GFEE,0) AS GFEE
	,CASE
		WHEN TBPI.BOR_TOT < 50 THEN 'Y'
		ELSE 'N'
	 END AS PRIN_BAL_U50
FROM EBI A
LEFT OUTER JOIN (
	SELECT DISTINCT BF_SSN
		,LN_SEQ
		,'X' AS I_IND
	FROM LN15
	WHERE LD_DSB < MDY(8,8,2006)
		AND IC_LON_PGM IN ('PLUS','PLUSGB')
	) PL_DISB
	ON A.BF_SSN = PL_DISB.BF_SSN
	AND A.LN_SEQ = PL_DISB.LN_SEQ
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,'N' AS FDSB_IND
	FROM LN15 
	WHERE LC_DSB_TYP = '1'
	) FULLY_DISB
	ON A.BF_SSN = FULLY_DISB.BF_SSN
	AND A.LN_SEQ = FULLY_DISB.LN_SEQ
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,MAX(LD_DSB) AS MAX_ACT_DISB_DT
		,MIN(LD_DSB) AS MIN_ACT_DISB_DT
	FROM LN15 
	WHERE LC_DSB_TYP = '2' 
	GROUP BY BF_SSN
		,LN_SEQ
	) ACT
	ON A.BF_SSN = ACT.BF_SSN
	AND A.LN_SEQ = ACT.LN_SEQ
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,MAX(LD_DSB) AS MAX_ANT_DISB_DT
		,SUM(LA_DSB) AS ANT_DSB_AMT
	FROM LN15 
	WHERE LC_DSB_TYP = '1'
		AND LD_DSB_ROS_PRT IS NULL  
	GROUP BY BF_SSN
		,LN_SEQ
	) ANT
	ON A.BF_SSN = ANT.BF_SSN
	AND A.LN_SEQ = ANT.LN_SEQ
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,LA_DSB_FEE AS OFEE
	FROM LN18 
	WHERE LC_DSB_FEE = '02'
	) ORG
	ON A.BF_SSN = ORG.BF_SSN
	AND A.LN_SEQ = ORG.LN_SEQ
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,LA_DSB_FEE AS GFEE
	FROM LN18 
	WHERE LC_DSB_FEE = '21'
	) GTY
	ON A.BF_SSN = GTY.BF_SSN
	AND A.LN_SEQ = GTY.LN_SEQ
LEFT OUTER JOIN (
	SELECT BF_SSN
		,SUM(PRIN_BAL) AS BOR_TOT
	FROM EBI 
	GROUP BY BF_SSN
	) TBPI
	ON A.BF_SSN = TBPI.BF_SSN
	

;
QUIT;

PROC DATASETS;
	DELETE EBI;
	CHANGE EBI1=EBI;
QUIT;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
/*QUIT;*/
/******************************************************************
* CALCULATE MOST OF THE VALUES THAT WILL BE USED IN PROCESSING
*******************************************************************/
DATA EBI ;
	SET EBI;
	LENGTH FY_FUL $ 75. FY_SRT $ 40. CAT $ 75. LOC1 $ 4. LOC2 $ 36. ECAT $ 10.;
/*MOST RECENT APPLICABLE DISBURSEMENT DATE FOR DATA FILE*/
	DO;
		IF FDSB_IND = 'Y' THEN
			MAX_DSB_DT = MAX_ACT_DISB_DT;
		ELSE 
			MAX_DSB_DT = MAX_ANT_DISB_DT;

		DF_IC_LON_PGM = IC_LON_PGM;

		IF IC_LON_PGM IN ('STFFRD','UNSTFD') THEN 
			IC_LON_PGM = 'STAFRD';	
		ELSE 
			IC_LON_PGM = IC_LON_PGM;
	END;	
/*LEGACY LENDER MERGES*/
	DO;
		IF LF_LON_CUR_OWN = '813760UT' THEN 
			LF_LON_CUR_OWN = '813760';
		ELSE IF LF_LON_CUR_OWN = '820043' THEN 
			LF_LON_CUR_OWN = '829505';
		ELSE 
			LF_LON_CUR_OWN = LF_LON_CUR_OWN ;
	END;
/*ORIGINATION FEE CALCS*/
	DO;
		IF I_IND = '' AND IC_LON_PGM IN ('PLUS','PLUSGB') THEN 
			LOC1 = '3%';
		ELSE IF I_IND = 'X' AND IC_LON_PGM IN ('PLUS','PLUSGB') THEN 
			LOC1 = '2%';
		ELSE IF LF_RGL_CAT_LP20 <= '1999030' AND IC_LON_PGM NOT IN ('PLUS','PLUSGB') THEN
			LOC1 = '3%';
		ELSE IF LF_RGL_CAT_LP20 = '2006020' AND IC_LON_PGM NOT IN ('PLUS','PLUSGB') THEN
			LOC1 = '2%';
		ELSE IF LF_RGL_CAT_LP20 = '2007020' AND IC_LON_PGM NOT IN ('PLUS','PLUSGB') THEN
			LOC1 = '1.5%';
		ELSE IF LF_RGL_CAT_LP20 = '2008020' AND IC_LON_PGM NOT IN ('PLUS','PLUSGB') THEN
			LOC1 = '1%';
		ELSE IF LF_RGL_CAT_LP20 = '2009020' AND IC_LON_PGM IN ('PLUS','PLUSGB') THEN
			LOC1 = '3%';
		ELSE IF LF_RGL_CAT_LP20 = '2009020' AND IC_LON_PGM NOT IN ('PLUS','PLUSGB') THEN
			LOC1 = '0.5%';

		IF IF_TIR_PCE ^= '' AND LF_RGL_CAT_LP20 < '2008020' THEN 
			LOC2 = 'LOAN WITH ZERO ORIGINATION FEE';
		ELSE IF IF_TIR_PCE ^= '' AND LF_RGL_CAT_LP20 = '2008020' THEN 
			LOC2 = 'UHEAA PAID ORIGINATION FEE LOANS';
		ELSE IF LF_RGL_CAT_LP20 = '2009020' THEN DO;
			IF IC_LON_PGM IN ('PLUS','PLUSGB') THEN DO;
				IF GFEE > 0 AND OFEE > 0 THEN
					LOC2 = 'LOANS WITH ORIG/GUAR FEE ED 3%/1%';
				ELSE 
					LOC2 = 'LOAN WITH ORIGINATION FEE 3%';
			END;
			ELSE IF IC_LON_PGM NOT IN ('PLUS','PLUSGB') THEN DO;
				IF GFEE > 0 AND OFEE > 0 THEN
					LOC2 = 'LOANS WITH ORIG/GUAR FEE ED 0.5%/1%';
				ELSE 
					LOC2 = 'LOAN WITH ORIGINATION FEE 0.5%';
			END;
		END;
		ELSE 
			LOC2 = 'LOAN WITH ORIGINATION FEE';
	END;
/*FISCAL YEAR DETERMINATION*/
	DO;
		IF MONTH(LD_LON_1_DSB) > 6 THEN DO;
			FY_SRT = CATX('-',YEAR(LD_LON_1_DSB),YEAR(LD_LON_1_DSB) + 1);
		END;
		ELSE DO; 
			FY_SRT = CATX('-',YEAR(LD_LON_1_DSB)-1,YEAR(LD_LON_1_DSB));
		END;
		FY_FUL = CATX(' ',IC_LON_PGM,'LOANS 1ST DISBURSED DURING FISCAL YEAR',FY_SRT);
	END;
/*CATEGORY ASSIGNMENT FOR PRINTED REPORTS*/
	IF IC_LON_PGM = 'STAFRD' THEN DO;
		IF LD_LON_1_DSB < '02MAY2006'D THEN DO;
			CAT = 'PREV_MAY52006';
			SCAT = 1;
		END;
		ELSE IF '02MAY2006'D <= LD_LON_1_DSB <= '30JUN2006'D  THEN DO;
			CAT = 'BTWN_MAY52006';
			SCAT = 2;
		END;
	END;
	DO;
		IF IC_LON_PGM IN ('STAFRD','PLUS','PLUSGB') AND CAT = '' THEN DO;
			CAT = FY_FUL;
			SCAT = 3;
		END;
	END;
/*ECASLA CATEGORY ASSGINMENT*/
	DO;
		IF LD_LON_1_DSB < MDY(7,1,2008) THEN
			ECAT = 'PRE ECASLA';
		ELSE IF LF_ECA_PGM_YR = '0809' THEN 
			ECAT = 'ECASLA I';
		ELSE IF LF_ECA_PGM_YR = '0910' THEN
			ECAT = 'ECASLA II';
		ELSE 
			ECAT = 'NON ECASLA';
	END;
/*SORT VARS FOR DATA FINAL SORTS*/
	DO;
		IF IC_LON_PGM IN ('PLUS','PLUSGB') THEN
			MS_SRT = 1;
		ELSE IF IC_LON_PGM = 'STAFRD' THEN 
			MS_SRT = 2;
		ELSE 
			MS_SRT = 3;	
	END;
RUN;
/*******************************************************************************
* CALCULATIONS FOR THE DATA FILE
********************************************************************************/
PROC SQL;
CREATE TABLE OFD AS 
SELECT B.*
	,INPUT(A.LOC1,PERCENT3.) AS OFEE
	,A.PRIN_BAL AS FAT_CUR_PRI
	,A.JUL1_IND
	,A.LF_RGL_CAT_LP20
	,A.LOC2
	,A.LD_LON_1_DSB
FROM EBI A 
INNER JOIN OFCAL B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
ORDER BY BF_SSN 
	,LN_SEQ 
	,TRX
;
QUIT;
DATA OFD;
	SET OFD;
	BY BF_SSN LN_SEQ TRX;
	IF FIRST.TRX THEN 
		X=0;
	X+TRX_AMT;
	IF LAST.TRX THEN 
		OUTPUT;
RUN;
DATA OFD (DROP=X);
	SET OFD;
	TRX_AMT = X;
RUN;
PROC TRANSPOSE DATA=OFD OUT=OFDX (DROP=_NAME_ _LABEL_);
	VAR TRX_AMT;
	BY BF_SSN LN_SEQ;
	ID TRX;
RUN;
PROC SORT DATA=OFD NODUPKEY;
	BY BF_SSN LN_SEQ;
RUN;
PROC SORT DATA=OFDX;
	BY BF_SSN LN_SEQ;
RUN;
DATA OFD;
	MERGE OFD (IN=A) OFDX (IN=B);
	BY BF_SSN LN_SEQ;
	IF A;
RUN;
%MACRO CALCB(DS,TRX_TYP);
	DATA &DS;
	SET &DS;
	FORMAT &TRX_TYP 10.2;
	IF &TRX_TYP = . THEN &TRX_TYP = 0;
		ELSE &TRX_TYP = ABS(&TRX_TYP);
	RUN;
%MEND CALCB;
%CALCB(OFD,_1010);
%CALCB(OFD,_1011);
%CALCB(OFD,_1012);
%CALCB(OFD,_1020);
%CALCB(OFD,_1021);
%CALCB(OFD,_1035);
%CALCB(OFD,_1036);
%CALCB(OFD,_1038);
%CALCB(OFD,_1037);
%CALCB(OFD,_1050);
%CALCB(OFD,_1070);
%CALCB(OFD,_1080);
%CALCB(OFD,_7001);
%CALCB(OFD,_2601);
DATA OFD (KEEP=BF_SSN LN_SEQ PAROD CLOF LFPD LFEE);
	SET OFD; 
	FORMAT PAROD CLOF 10.2 ;
	tPAROD = ROUND(FAT_CUR_PRI + (
			_1010 + _1011 + _1012 + _1020 + _1021 + _1035 + 
			_1036 + _1038 + _1037 + _1050 + _1070 + _1080) - 
			_7001 - _2601,.01);
	tCLOF = tPAROD * OFEE;
	IF JUL1_IND = 'Y' AND LD_LON_1_DSB < MDY(7,1,2009) THEN DO;
		LFPD = ROUND(tPAROD,.01);
		LFEE = ROUND(tPAROD * .01,.01);
		PAROD = 0;
		CLOF = 0;
	END;
	ELSE IF	INPUT(LF_RGL_CAT_LP20,BEST12.) < 2008020 
		AND LOC2 = 'LOAN WITH ZERO ORIGINATION FEE' THEN DO;
			PAROD = ROUND(tPAROD,.01);
			CLOF = ROUND(tCLOF,.01);
			LFPD=0;
			LFEE = 0;
	END;
	ELSE DO;
		PAROD = 0;
		CLOF = 0;
		LFPD = 0;
		LFEE = 0;
	END;
RUN;
PROC SORT DATA=EBI NODUPKEY;
	BY BF_SSN LN_SEQ;
RUN;
PROC SORT DATA=OFD NODUPKEY;
	BY BF_SSN LN_SEQ;
RUN;
DATA EBI;
	MERGE EBI (IN=A) OFD (IN=B);
	BY BF_SSN LN_SEQ;
	IF A;
	DO;
		PAROD = COALESCE(PAROD,0);
		CLOF = COALESCE(CLOF,0);
		LFPD = COALESCE(LFPD,0);
		LFEE = COALESCE(LFEE,0);
	END;
RUN;
/*ENDRSUBMIT;*/

/*libname files "C:\SERF_File\";*/
/*DATA EBI;*/
/*	SET files.EBI;*/
/*RUN;*/


/*DATA EBI;
	SET WORKLOCL.EBI;
RUN;*/

/*******************************************************************************
* GET DATA FOR REPORTS
********************************************************************************/
%MACRO DS_DSQL(DS,SEL_STR,WHR_STR,GRP_STR,ORD_STR);
PROC SQL;
	CREATE TABLE &DS AS 
		SELECT &SEL_STR
		FROM EBI
		WHERE &WHR_STR
		GROUP BY &GRP_STR
		ORDER BY &ORD_STR
	;
QUIT;
%MEND DS_DSQL;
/******************************************************************************
* DATA FOR REPORTS - SDAT5 WILL ONLY BE USED FOR UHEAA REPORTS
*******************************************************************************/
%DS_DSQL(EB,/*DATA SET FOR REPORT SECTION 1 - LENDER, OF%*/
	%STR(LF_LON_CUR_OWN,SUM(PRIN_BAL) AS PRIN_BAL,IM_LDR_FUL,LOC1,LOC2),
	%STR(LF_LON_CUR_OWN ^= ''),
	%STR(LF_LON_CUR_OWN,IM_LDR_FUL,LOC1,LOC2),
	%STR(LF_LON_CUR_OWN,IM_LDR_FUL,LOC1,LOC2)
	);
%DS_DSQL(EBB,/*DATA SET FOR REPORT SECTION 2 - LENDER, FULLY DISB, OF%*/
	%STR(FDSB_IND,LF_LON_CUR_OWN,SUM(PRIN_BAL) AS PRIN_BAL,IM_LDR_FUL,LOC1,LOC2),
	%STR(LF_LON_CUR_OWN ^= ''),
	%STR(FDSB_IND,LF_LON_CUR_OWN,IM_LDR_FUL,LOC1,LOC2),
	%STR(FDSB_IND,LF_LON_CUR_OWN,IM_LDR_FUL,LOC1,LOC2)
	);
%DS_DSQL(SDAT1,/*DATA SET FOR REPORT SECTION 3 - LENDER, OCT1 DISB*/
	%STR(LF_LON_CUR_OWN,IC_LON_PGM,OCT1_IND,0 AS SCAT,COUNT(DISTINCT BF_SSN) AS BOR_COUNT,
		 COUNT(*) AS LON_COUNT,SUM(PRIN_BAL) AS PRIN_BAL,SUM(INTR_BAL) AS INTR_BAL,
		 SUM(PRIN_BAL) + SUM(INTR_BAL) AS TOT_BAL),
	%STR(IC_LON_PGM IN ('STAFRD','PLUS','PLUSGB')),
	%STR(LF_LON_CUR_OWN,IC_LON_PGM,OCT1_IND),
	%STR(LF_LON_CUR_OWN,IC_LON_PGM,OCT1_IND)
	);
%DS_DSQL(SDAT2,/*DATA SET FOR REPORT SECTION 3 - LENDER, STAFFORD DISB*/
	%STR(LF_LON_CUR_OWN,IC_LON_PGM,CAT,SCAT,COUNT(DISTINCT BF_SSN) AS BOR_COUNT,COUNT(*) AS LON_COUNT,
		 SUM(PRIN_BAL) AS PRIN_BAL,SUM(INTR_BAL) AS INTR_BAL,SUM(PRIN_BAL) + SUM(INTR_BAL) AS TOT_BAL),
	%STR(CAT ^= ''),
	%STR(LF_LON_CUR_OWN,IC_LON_PGM,CAT,SCAT),
	%STR(LF_LON_CUR_OWN,IC_LON_PGM,SCAT)
	);
%DS_DSQL(SDAT3,/*DATA SET FOR REPORT SECTION 3 - LENDER, TOTAL OCT1 DISB*/
	%STR(LF_LON_CUR_OWN,OCT1_IND,'ALL' AS IC_LON_PGM,4 AS SCAT, COUNT(DISTINCT BF_SSN) AS BOR_COUNT,
		 COUNT(*) AS LON_COUNT,SUM(PRIN_BAL) AS PRIN_BAL,SUM(INTR_BAL) AS INTR_BAL,
		 SUM(PRIN_BAL) + SUM(INTR_BAL) AS TOT_BAL),
	%STR(IC_LON_PGM IN ('STAFRD','PLUS','PLUSGB')),
	%STR(LF_LON_CUR_OWN,OCT1_IND),
	%STR(LF_LON_CUR_OWN,OCT1_IND)
	);
%DS_DSQL(SDAT3B,/*DATA SET FOR REPORT SECTION 3 - LENDER, TOTAL OCT1 DISB*/
	%STR(DISTINCT LF_LON_CUR_OWN,FY_SRT,'LOANS 1ST DISBURSED DURING FISCAL YEAR '||' '||FY_SRT AS IC_LON_PGM
		 ,5 AS SCAT,COUNT(DISTINCT BF_SSN) AS BOR_COUNT,COUNT(*) AS LON_COUNT,SUM(PRIN_BAL) AS PRIN_BAL
		 ,SUM(INTR_BAL) AS INTR_BAL,SUM(PRIN_BAL) + SUM(INTR_BAL) AS TOT_BAL),
	%STR(IC_LON_PGM IN ('STAFRD','PLUS','PLUSGB') AND LD_LON_1_DSB >= '01JUL2008'D),
	%STR(LF_LON_CUR_OWN,FY_SRT),
	%STR(LF_LON_CUR_OWN,FY_SRT)
	);
%DS_DSQL(SDAT4,/*DATA SET FOR REPORT SECTION 4 - FISCAL YEAR, FULLY DISB, LOAN TYPE*/
	%STR(FY_SRT,FDSB_IND,IC_LON_PGM,LF_LON_CUR_OWN,IM_LDR_FUL,COUNT(DISTINCT BF_SSN) AS BOR_COUNT
		,COUNT(*) AS LON_COUNT,SUM(PRIN_BAL) AS PRIN_BAL,SUM(INTR_BAL) AS INTR_BAL
		,SUM(PRIN_BAL) + SUM(INTR_BAL) AS TOT_BAL),
	%STR(JUL1_IND = 'Y'),
	%STR(FY_SRT,FDSB_IND,IC_LON_PGM,LF_LON_CUR_OWN,IM_LDR_FUL),
	%STR(FY_SRT,FDSB_IND,IC_LON_PGM,LF_LON_CUR_OWN,IM_LDR_FUL)
	);
%DS_DSQL(SDAT5,/*DATA SET FOR REPORT 20 & 26 SECTION 5 - BOND, LENDER, TOTAL OCT1 DISB*/
	%STR(FY_SRT,IF_BND_ISS,FDSB_IND,IC_LON_PGM,LF_LON_CUR_OWN,IM_LDR_FUL,COUNT(DISTINCT BF_SSN) AS BOR_COUNT
		,COUNT(*) AS LON_COUNT,SUM(PRIN_BAL) AS PRIN_BAL,SUM(INTR_BAL) AS INTR_BAL
		,SUM(PRIN_BAL) + SUM(INTR_BAL) AS TOT_BAL),
	%STR(JUL1_IND = 'Y'),
	%STR(FY_SRT,IF_BND_ISS,FDSB_IND,IC_LON_PGM,LF_LON_CUR_OWN,IM_LDR_FUL),
	%STR(FY_SRT,IF_BND_ISS,FDSB_IND,IC_LON_PGM,LF_LON_CUR_OWN,IM_LDR_FUL)
	);
/*REPORT 3 PROCESSING*/
DATA SDAT;
	LENGTH IC_LON_PGM $ 100.;
	FORMAT IC_LON_PGM $100.;
	SET SDAT1 SDAT2 SDAT3 SDAT3B;
RUN;
DATA SDAT (DROP=OCT1_IND IC_LON_PGM);
	SET SDAT;
	IF FY_SRT ^= '' THEN DO;
		CAT = IC_LON_PGM;
	END;
	ELSE DO;
		IF CAT = '' THEN DO;
			IF OCT1_IND = 'N' THEN 
				CAT = CATx('_','PREV_OCT1',IC_LON_PGM);
			ELSE 
				CAT = CATx('_','ONAFTR_OCT1',IC_LON_PGM);
		END;
	END;
RUN;
/*CUSTOME FORMATS FOR DATA LEVELS*/
PROC FORMAT;
	VALUE $CAFRMT	
		'PREV_OCT1_PLUS' 	 =  'PLUS LOANS 1ST DISBURSED PRIOR TO 10/1/07  '
		'ONAFTR_OCT1_PLUS' 	 =  'PLUS LOANS 1ST DISBURSED ON/AFTER 10/1/07  '
		'PREV_OCT1_PLUSGB' 	 =  'PLUSGB LOANS 1ST DISBURSED PRIOR TO 10/1/07'
		'ONAFTR_OCT1_PLUSGB' =  'PLUSGB LOANS 1ST DISBURSED ON/AFTER 10/1/07'
		'PREV_OCT1_STAFRD' 	 =  'STAFFORD LOANS 1ST DISBURSED PRIOR TO 10/1/07'
		'ONAFTR_OCT1_STAFRD' =  'STAFFORD LOANS 1ST DISBURSED ON/AFTER 10/1/07'
		'PREV_MAY52006' 	 =  'STAFFORD LOANS 1ST DISBURSED PRIOR TO 5/2/06'
		'BTWN_MAY52006' 	 =  'STAFFORD LOANS 1ST DISBURSED BETWEEN 5/2/06 AND 6/30/06  '
		'PREV_OCT1_ALL' 	 =  'TOTALS FOR LOANS DISBURSED PRIOR TO 10/1/07'
		'ONAFTR_OCT1_ALL' 	 =  'TOTALS FOR LOANS DISBURSED ON/AFTER TO 10/1/07';
	VALUE $FDISB	
		'Y' = 'FULLY DISBURSED'
		'N' = 'NOT FULLY DISBURSED';
	VALUE $HBND
		'BLANK' = '          ';
	VALUE $LNSTA
		'01' = 'GRACE'
		'02' = 'IN SCHOOL'
		'03' = 'REPAYMENT'
		'04' = 'DEFERMENT'
		'05' = 'FORBEARANCE'
		'06' = 'CURE'
		'07' = 'CLAIM PENDING'
		'08' = 'CLAIM SUBMITTED'
		'09' = 'CLAIM CANCELED'
		'10' = 'CLAIM REJECTED'
		'11' = 'CLAIM RETURNED'
		'12' = 'CLAIM PAID'
		'13' = 'PRECLAIM PENDING'
		'14' = 'PRECLAIM SUBMITTED'
		'15' = 'PRELCIAM CANCELED'
		'16' = 'ALLEGED DEATH'
		'17' = 'VERIFIED DEATH'
		'18' = 'ALLEGED DISABILITY'
		'19' = 'VERIFIED DISABILITY'
		'20' = 'ALLEGED BANKRUPTCY'
		'21' = 'VERIFIED BANKRUPTCY'
		'22' = 'PAID IN FULL'
		'23' = 'PARTIAL DISBURSED';
QUIT;
/**********************************************************************
* REPORT MACRO
***********************************************************************/
%MACRO ENDBAL(LENDER=,REPNO=,SEC5=); 
%LET TTL = ;
DATA RSEC1;
	SET EB;
	WHERE LF_LON_CUR_OWN = "&LENDER";
RUN;
DATA RSEC2;
	SET EBB;
	WHERE LF_LON_CUR_OWN = "&LENDER";
RUN;
PROC SORT DATA=SDAT OUT=RSEC3 (WHERE=(LF_LON_CUR_OWN = "&LENDER"));
	BY SCAT;
RUN;
DATA RSEC4;
	SET SDAT4;
	WHERE LF_LON_CUR_OWN = "&LENDER";
RUN;
/*NOTE: THIS DATA SET WILL ONLY BE USED IN THE UHEAA REPORTS */
DATA RSEC5;
	SET SDAT5;
	WHERE LF_LON_CUR_OWN = "&LENDER";
RUN;
PROC PRINTTO PRINT=REPORT&REPNO NEW;
RUN;
PROC CONTENTS DATA=RSEC1 OUT=EMPTYSET NOPRINT;RUN;
DATA _NULL_;
	SET EMPTYSET;
	CALL SYMPUT('OBS',NOBS);
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 CENTER NODATE NONUMBER PAGENO=1;
%PUT OBS=&OBS;
%IF &OBS = 0 %THEN %DO;
    DATA _NULL_;
       SET EMPTYSET;
       FILE PRINT;
       IF  NOBS=0 AND _N_ =1 THEN DO;
           PUT // 126*'-';
           PUT      //////
               @51 '**** NO OBSERVATIONS FOUND ****';
           PUT ////////
               @57 '-- END OF REPORT --';
		   PUT ///////////
		   		@46 "JOB = UTLWL02     REPORT = ULWL02.LWL02R&REPNO";
           END;
    RETURN;
	TITLE "UTAH HIGHER EDUCATION ASSISTANCE AUTHORITY";
	TITLE2 "DAILY ENDING BALANCE REPORT";
	TITLE3 "OWNER: &LENDER";
	TITLE4 "&YDAY";
	RUN;
%END;
%ELSE %DO;
	DATA _NULL_;
	SET RSEC1;
		CALL SYMPUT('TTL',TRIM(IM_LDR_FUL));
	RUN;
	PROC REPORT DATA=RSEC1 NOWD HEADSKIP SPLIT='/' SPACING=3 ;
		TITLE "UTAH HIGHER EDUCATION ASSISTANCE AUTHORITY";
		TITLE2 "DAILY ENDING BALANCE REPORT";
		TITLE3 "OWNER: &LENDER - &TTL";
		TITLE4 "&YDAY";
		FOOTNOTE  "JOB = UTLWL02     REPORT = ULWL02.LWL02R&REPNO";
		COLUMN LOC2 LOC1 PRIN_BAL;
		DEFINE LOC2 / GROUP ORDER WIDTH=31 'CATEGORY ' ;
		DEFINE LOC1 / GROUP ORDER WIDTH=20 'ED ORIGINATION FEE %' ;
		DEFINE PRIN_BAL / ANALYSIS FORMAT=DOLLAR20.2 'PRINCIPAL BALANCE';
		COMPUTE AFTER LOC2;
			LINE @1 @26 30*'�' @85 18*'�';
			LINE @1 @26 'ENDING BALANCE: ' @83 PRIN_BAL.SUM DOLLAR20.2 ;
			LINE ' ';
			LINE ' ';
		ENDCOMP;
		COMPUTE AFTER;
			LINE ' ';
			LINE @29 'TOTAL ENDING BALANCE' @83 PRIN_BAL.SUM DOLLAR20.2;
		ENDCOMP;
	RUN;
	PROC REPORT DATA=RSEC2 NOWD HEADSKIP SPLIT='/' SPACING=3 ;
		COLUMN FDSB_IND LOC2 LOC1 PRIN_BAL;
		DEFINE FDSB_IND / GROUP 'CATEGORY' WIDTH=10 FORMAT=$FDISB. FLOW;
		DEFINE LOC2 / GROUP ORDER WIDTH=31 '/' ;
		DEFINE LOC1 / GROUP ORDER WIDTH=20 'ED ORIGINATION FEE %' ;
		DEFINE PRIN_BAL / ANALYSIS FORMAT=DOLLAR20.2 'PRINCIPAL BALANCE';
		COMPUTE AFTER LOC2;
			LINE @1 @32 31*'�' @91 18*'�';
			LINE @1 @32 'ENDING BALANCE: ' @89 PRIN_BAL.SUM DOLLAR20.2 ;
			LINE ' ';
			LINE ' ';
		ENDCOMP;
		COMPUTE AFTER;
			LINE ' ';
			LINE @35 'TOTAL ENDING BALANCE' @89 PRIN_BAL.SUM DOLLAR20.2;
		ENDCOMP;
	RUN;
	PROC PRINT NOOBS SPLIT='/' DATA=RSEC3 WIDTH=UNIFORM WIDTH=MIN LABEL;
		FORMAT CAT $CAFRMT. BOR_COUNT LON_COUNT COMMA8. PRIN_BAL INTR_BAL TOT_BAL DOLLAR20.2;
		VAR CAT BOR_COUNT LON_COUNT PRIN_BAL INTR_BAL TOT_BAL;
		LABEL CAT = '/'
		BOR_COUNT = '#/BORROWERS'
		LON_COUNT = '#/LOANS'
		PRIN_BAL = 'PRINCIPAL'
		INTR_BAL = 'INTEREST'
		TOT_BAL = 'PRINCIPAL &/INTEREST';
	RUN;
	PROC REPORT DATA=RSEC4 NOWD HEADSKIP SPLIT='/' SPACING=3 ;
		COLUMN FY_SRT FDSB_IND IC_LON_PGM BOR_COUNT LON_COUNT PRIN_BAL INTR_BAL TOT_BAL;
		DEFINE FY_SRT / GROUP WIDTH=11 "FISCAL/YEAR" ; 
		DEFINE FDSB_IND / GROUP "FULLY/DISB" WIDTH=5;  
		DEFINE IC_LON_PGM / GROUP "LOAN/TYPE" WIDTH=8;  
		DEFINE BOR_COUNT / ANALYSIS "TOTAL/BORROWERS" WIDTH=9 FORMAT=COMMA8.; 
		DEFINE LON_COUNT / ANALYSIS "TOTAL/LOANS" WIDTH=9 FORMAT=COMMA8.; 
		DEFINE PRIN_BAL / ANALYSIS "TOTAL/PRINCIPAL" WIDTH=18 FORMAT=DOLLAR18.2; 
		DEFINE INTR_BAL / ANALYSIS "TOTAL/INTEREST" WIDTH=18 FORMAT=DOLLAR12.2; 
		DEFINE TOT_BAL / ANALYSIS "TOTAL/PRINCIPAL AND/ INTEREST" WIDTH=18 FORMAT=DOLLAR18.2;  
		BREAK AFTER FDSB_IND / SUPPRESS SUMMARIZE SKIP OL;
	RUN;
	%IF &SEC5 %THEN %DO;
		PROC REPORT DATA=RSEC5 NOWD HEADSKIP SPLIT='/' SPACING=2 ;
			COLUMN FY_SRT FDSB_IND IF_BND_ISS IC_LON_PGM BOR_COUNT LON_COUNT PRIN_BAL INTR_BAL TOT_BAL;
			DEFINE FY_SRT / GROUP WIDTH=11 "FISCAL/YEAR" ; 
			DEFINE FDSB_IND / GROUP "FULLY/DISB" WIDTH=5;  
			DEFINE IF_BND_ISS / GROUP "BOND" WIDTH=10;
			DEFINE IC_LON_PGM / GROUP "LOAN/TYPE" WIDTH=8;  
			DEFINE BOR_COUNT / ANALYSIS "TOTAL/BORROWERS" WIDTH=9 FORMAT=COMMA8.; 
			DEFINE LON_COUNT / ANALYSIS "TOTAL/LOANS" WIDTH=9 FORMAT=COMMA8.; 
			DEFINE PRIN_BAL / ANALYSIS "TOTAL/PRINCIPAL" WIDTH=18 FORMAT=DOLLAR18.2; 
			DEFINE INTR_BAL / ANALYSIS "TOTAL/INTEREST" WIDTH=18 FORMAT=DOLLAR12.2; 
			DEFINE TOT_BAL / ANALYSIS "TOTAL/PRINCIPAL AND/ INTEREST" WIDTH=18 FORMAT=DOLLAR18.2;  
			BREAK AFTER FDSB_IND / SUPPRESS SUMMARIZE SKIP OL;
		RUN;
	%END;
%END;
PROC PRINTTO;
RUN;
%MEND ENDBAL;
/**********************************************************************
* FILE MACRO
***********************************************************************/
%MACRO LDR_DT_FILE(LDRID,RNO);
DATA L02_REP_DS;
	SET EBI;
	%IF &RNO NE 49 %THEN %DO;
		WHERE LF_LON_CUR_OWN = "&LDRID";
	%END;
	%IF &RNO EQ 50 %THEN %DO;
		IF IC_LON_PGM = 'TILP' THEN 
			OUTPUT;
	%END;
RUN;

DATA L02_REP_DS (DROP=SCHL_CAT SCL_DESC);
	SET L02_REP_DS;
	LENGTH SCHL_CAT SCL_DESC SCL_TYP $100.;
		IF IC_SCL_TYP IN ('00','01','02') THEN 
			SCHL_CAT = '4 YEAR SCHOOL';
		ELSE IF IC_SCL_TYP IN ('03','04') THEN 
			SCHL_CAT = '2 YEAR SCHOOL';
		ELSE IF IC_SCL_TYP IN ('05','06','07','08','09','10','AN','AP') THEN 
			SCHL_CAT = 'PROPRIETARY';
		ELSE 
			SCHL_CAT = 'UNKNOWN';

		IF IC_SCL_TYP = '00' 
			THEN SCL_DESC = 'DEGREE, PRIVATE';

		ELSE IF IC_SCL_TYP = '01'
			THEN SCL_DESC = 'STATE';

		ELSE IF IC_SCL_TYP = '02'           
			THEN SCL_DESC = 'STATE-RELATED';
		ELSE IF IC_SCL_TYP = '03'           
			THEN SCL_DESC = 'JUNIOR';
		ELSE IF IC_SCL_TYP = '04'           
			THEN SCL_DESC = 'COMMUNITY (PUBLIC, 2-YEAR)';
		ELSE IF IC_SCL_TYP = '05'           
			THEN SCL_DESC = 'NURSING';
		ELSE IF IC_SCL_TYP = '06'           
			THEN SCL_DESC = 'VOCATIONAL/TECHNICAL';
		ELSE IF IC_SCL_TYP = '07'           
			THEN SCL_DESC = 'BUSINESS';
		ELSE IF IC_SCL_TYP = '08'           
			THEN SCL_DESC = 'TRADE';
		ELSE IF IC_SCL_TYP = '09'           
			THEN SCL_DESC = 'SCHOOL OF THEOLOGY OR SEMINARY';
		ELSE            
			SCL_DESC = '';

		SCL_TYP = CATX(' : ', SCHL_CAT, SCL_DESC);
RUN;
PROC SORT DATA=L02_REP_DS;
	BY LF_LON_CUR_OWN MS_SRT LD_SCL_SPR BF_SSN LN_SEQ;
RUN;
DATA _NULL_;
SET  L02_REP_DS;
FILE REPORT&RNO DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT BF_SSN $9. ;
   FORMAT LN_SEQ 6. ;
   FORMAT DF_IC_LON_PGM $6. ;
   FORMAT LD_LON_1_DSB MMDDYY10. ;
   FORMAT LF_LON_CUR_OWN $8. ;
   FORMAT IM_LDR_FUL $40. ;
   FORMAT PRIN_BAL 15.2 ;
   FORMAT INTR_BAL 15.2 ;
   FORMAT IF_TIR_PCE $3. ;
   FORMAT LF_RGL_CAT_LP20 $7. ;
   FORMAT I_IND $1. ;
   FORMAT IC_LON_PGM0 $6. ;
   FORMAT IF_BND_ISS $8. ;
   FORMAT FDSB_IND $1. ;
   FORMAT OCT1_IND $1. ;
   FORMAT JUL1_IND $1. ;
   FORMAT LD_LON_GTR MMDDYY10. ;
   FORMAT LA_LON_AMT_GTR 10.2 ;
   FORMAT LD_SCL_SPR MMDDYY10. ;
   FORMAT LD_TRM_BEG MMDDYY10. ;
   FORMAT LD_TRM_END MMDDYY10. ;
   FORMAT LC_ACA_GDE_LEV $2. ;
   FORMAT LF_DOE_SCL_ORG $8. ;
   FORMAT LF_DOE_SCL_ENR_CUR $8. ;
   FORMAT IM_SCL_FUL $40. ;
   FORMAT LR_ITR_ORG 7.3 ;
   FORMAT FY_FUL $40. ;
   FORMAT FY_SRT $40. ;
   FORMAT CAT $40. ;
   FORMAT LOC1 $4. ;
   FORMAT LOC2 $31. ;
   FORMAT SCAT BEST12. ;
   FORMAT PAROD 10.2 ;
   FORMAT CLOF 10.2 ;
   FORMAT LFPD 10.2 ;
   FORMAT LFEE 10.2 ;
   FORMAT MAX_ACT_DISB_DT MMDDYY10.;
   FORMAT MAX_ANT_DISB_DT MMDDYY10.;
   FORMAT IF_DOE_LDR $8.;
   FORMAT LR_ITR 7.3;
   FORMAT WC_DW_LON_STA $LNSTA.;
   FORMAT HAS_CON_PAY $1.;
   FORMAT PRIN_BAL_U50 $1.;
   FORMAT WC_DW_LON_STA $2. ;
   FORMAT WD_LON_RPD_SR MMDDYY10. ;
   FORMAT WD_XPC_POF_TS26 MMDDYY10. ;
   FORMAT DAYS_DELQ 11. ;
   FORMAT LC_ECA_PUT_STA $2. ;
   FORMAT LD_ECA_LON_PUT_DOE MMDDYY10. ;
   FORMAT LC_ELG_95_SPA_BIL $1. ;
   FORMAT LR_EFT_RDC 7.3 ;
   FORMAT LR_RDC 7.3 ;
   FORMAT LC_LON_CDU_STA $2. ;
   FORMAT LD_LON_CDU_BIL_SLE MMDDYY10. ;
   FORMAT LN_LON_CDU_DAY_DLQ 5. ;
   FORMAT LD_CDU_ANT_PUT MMDDYY10. ;
   FORMAT MIN_ACT_DISB_DT BEST12. ;
   FORMAT SCL_TYP $100. ;
IF _N_ = 1 THEN DO;
   PUT 'LENDER ID'
		','
		'LENDER NAME'
		','
		'ORIGINAL LENDER'
		','
		'SSN' 
		','
		'LOAN SEQ #'
		','
		'LOAN TYPE' 
		','
		'PRINCIPAL BALANCE' 
		','
		'INTEREST' 
		','
		'INTEREST RATE'
		','
		'ORIGINAL INTEREST RATE' 
		','
		'LOAN TERM BEGIN'
		','
		'LOAN TERM END' 
		','
		'GUARANTEED AMOUNT' 
		','
		'GUARANTEED DATE' 
		','
		'1ST DISB DATE' 
		','
		'GRADE LEVEL' 
		','
		'CURRENT SCHOOL CODE' 
		','
		'ORIGINAL SCHOOL CODE' 
		','
		'SCHOOL NAME' 
		','
		'SEPARATION DATE' 
		','
		'ORIGINATION FEE %' 
		',' 
		'FEE CATEGORY' 
		','
		'LOAN CATEGORY' 
		','
		'BOND ID' 
		','
		'LATE FEES' 
		','
		'MAXIMUM ACTUAL DISB DATE' 
		','
		'MAXIMUM ANTICIPATED DISB DATE' 
		','
		'CALCULATED OF AMOUNT' 
		','
		'CALCULATED LF AMOUNT' 
		','
		'CALCULATED PRINCIPAL LENDER FEE' 
		','
		"WD_LON_RPD_SR"
		','
		"WD_XPC_POF_TS26"
		','
		"DAYS_DELQ"
		','
		"CALC_1ST_DISB_DT"
		','
		"SCL_TYP"
		','
		"LC_ECA_PUT_STA"
		','
		"LD_ECA_LON_PUT_DOE"
		','
		"LC_ELG_95_SPA_BIL"
		','
		"LR_EFT_RDC"
		','
		"LR_RDC"
		','
		"LC_LON_CDU_STA"
		','
		"LD_LON_CDU_BIL_SLE"
		','
		"LN_LON_CDU_DAY_DLQ"
		','
		"LD_CDU_ANT_PUT"
		','
		'CALCULATED PRINCIPAL ORIGINATION FEE' 
		%IF &RNO NE 50 %THEN %DO;
			',' 
			'ANTICIPATED DISBURSEMENT AMOUNT'
			','
			'ECASLA CATEGORY'
			','
			'LOAN STATUS'
			','
			'CONAOLIDATION PAYMENT INDICATOR'
			','
			'UNDER $50 PRINCIPAL BALANCE INDICATOR'
		%END;
		
	;
END;
DO;
	PUT LF_LON_CUR_OWN $ @;
	PUT IM_LDR_FUL $ @;
	PUT IF_DOE_LDR $ @;
	PUT BF_SSN $ @;
	PUT LN_SEQ @;
	PUT DF_IC_LON_PGM $ @;
	PUT PRIN_BAL @;
	PUT INTR_BAL @;
	PUT LR_ITR @;
	PUT LR_ITR_ORG @;
	PUT LD_TRM_BEG @;
	PUT LD_TRM_END @;
	PUT LA_LON_AMT_GTR @;
	PUT LD_LON_GTR @;
	PUT LD_LON_1_DSB @;
	PUT LC_ACA_GDE_LEV $ @;
	PUT LF_DOE_SCL_ENR_CUR $ @;
	PUT LF_DOE_SCL_ORG $ @;
	PUT IM_SCL_FUL $ @;
	PUT LD_SCL_SPR @;
	PUT LOC1 $ @;
	PUT LOC2 $ @;
	PUT FDSB_IND $ @;
	PUT IF_BND_ISS $ @;
	PUT LA_LTE_FEE_OTS @;
	PUT MAX_ACT_DISB_DT @;
	PUT MAX_ANT_DISB_DT @;
	PUT CLOF @;
	PUT LFEE @;
	PUT LFPD @;
	PUT WD_LON_RPD_SR @;
    PUT WD_XPC_POF_TS26 @;
    PUT DAYS_DELQ @;
    PUT MIN_ACT_DISB_DT @;
    PUT SCL_TYP $ @;
    PUT LC_ECA_PUT_STA $ @;
    PUT LD_ECA_LON_PUT_DOE @;
    PUT LC_ELG_95_SPA_BIL $ @;
    PUT LR_EFT_RDC @;
    PUT LR_RDC @;
    PUT LC_LON_CDU_STA $ @;
    PUT LD_LON_CDU_BIL_SLE @;
    PUT LN_LON_CDU_DAY_DLQ @;
    PUT LD_CDU_ANT_PUT @;
	%IF &RNO NE 50 %THEN %DO;
		PUT PAROD @;
		PUT ANT_DSB_AMT @;
		PUT ECAT $ @;
		PUT WC_DW_LON_STA $ @;
		PUT HAS_CON_PAY $ @;
		PUT PRIN_BAL_U50 $ ;
	%END;
	%ELSE %DO;
		PUT PAROD ;
	%END;
END;
RUN;
%MEND LDR_DT_FILE;
/*PRINTED REPORTS*/
%ENDBAL(LENDER=811698,REPNO=2,SEC5=0);
%ENDBAL(LENDER=813760,REPNO=3,SEC5=0);
%ENDBAL(LENDER=813894,REPNO=4,SEC5=0);
%ENDBAL(LENDER=817440,REPNO=5,SEC5=0);
%ENDBAL(LENDER=817545,REPNO=6,SEC5=0);
%ENDBAL(LENDER=817546,REPNO=7,SEC5=0);
%ENDBAL(LENDER=819628,REPNO=8,SEC5=0);
/*%ENDBAL(LENDER=829505,REPNO=9,SEC5=0);*/
%ENDBAL(LENDER=820200,REPNO=10,SEC5=0);
%ENDBAL(LENDER=822373,REPNO=11,SEC5=0);
%ENDBAL(LENDER=829123,REPNO=12,SEC5=0);
%ENDBAL(LENDER=829158,REPNO=13,SEC5=0);
%ENDBAL(LENDER=830132,REPNO=14,SEC5=0);
%ENDBAL(LENDER=830146,REPNO=15,SEC5=0);
%ENDBAL(LENDER=830791,REPNO=16,SEC5=0);
/*%ENDBAL(LENDER=832241,REPNO=17,SEC5=0);*/
%ENDBAL(LENDER=833828,REPNO=18,SEC5=0);
%ENDBAL(LENDER=817455,REPNO=19,SEC5=0);
%ENDBAL(LENDER=828476,REPNO=20,SEC5=1);
%ENDBAL(LENDER=817575,REPNO=21,SEC5=0);
%ENDBAL(LENDER=834122,REPNO=22,SEC5=0);
%ENDBAL(LENDER=833577,REPNO=23,SEC5=0);
%ENDBAL(LENDER=834265,REPNO=24,SEC5=0);
%ENDBAL(LENDER=834396,REPNO=25,SEC5=0);
%ENDBAL(LENDER=834437,REPNO=26,SEC5=1);
%ENDBAL(LENDER=82847601,REPNO=51,SEC5=1);
%ENDBAL(LENDER=834529,REPNO=53,SEC5=1);
%ENDBAL(LENDER=826717,REPNO=54,SEC5=0);
%ENDBAL(LENDER=830248,REPNO=55,SEC5=0);
%ENDBAL(LENDER=900749,REPNO=56,SEC5=0);
%ENDBAL(LENDER=829769,REPNO=57,SEC5=0);
%ENDBAL(LENDER=82976901,REPNO=58,SEC5=0);
%ENDBAL(LENDER=82976902,REPNO=59,SEC5=0);
%ENDBAL(LENDER=82976903,REPNO=60,SEC5=0);
%ENDBAL(LENDER=82976904,REPNO=61,SEC5=0);
%ENDBAL(LENDER=82976905,REPNO=62,SEC5=0);
%ENDBAL(LENDER=82976906,REPNO=63,SEC5=0);
%ENDBAL(LENDER=82976907,REPNO=64,SEC5=0);
%ENDBAL(LENDER=82976908,REPNO=65,SEC5=0);


/*DATA FILES*/
%LDR_DT_FILE(811698,27);	
%LDR_DT_FILE(813760,28);	
%LDR_DT_FILE(813894,29);	
%LDR_DT_FILE(817440,30);	
%LDR_DT_FILE(817545,31);	
%LDR_DT_FILE(817546,32);	
%LDR_DT_FILE(819628,33);	
%LDR_DT_FILE(820200,34);	
%LDR_DT_FILE(822373,35);	
%LDR_DT_FILE(829158,37);	
%LDR_DT_FILE(830146,39);	
%LDR_DT_FILE(833828,41);	
%LDR_DT_FILE(817455,42);	
%LDR_DT_FILE(828476,43);	
%LDR_DT_FILE(817575,44);	
%LDR_DT_FILE(834122,45);	
%LDR_DT_FILE(833577,46);	
%LDR_DT_FILE(834265,47);	
%LDR_DT_FILE(834437,48);	
%LDR_DT_FILE(829123,36);	
%LDR_DT_FILE(830132,38);	
%LDR_DT_FILE(830791,40);
%LDR_DT_FILE(000000,49);
%LDR_DT_FILE(971357,50);
%LDR_DT_FILE(82847601,51);	
%LDR_DT_FILE(834493,52);
%LDR_DT_FILE(834529,53);		
%LDR_DT_FILE(826717,54);	
%LDR_DT_FILE(830248,55);	
%LDR_DT_FILE(900749,56);	
%LDR_DT_FILE(829769,57);
%LDR_DT_FILE(82976901,58);
%LDR_DT_FILE(82976902,59);
%LDR_DT_FILE(82976903,60);
%LDR_DT_FILE(82976904,61);
%LDR_DT_FILE(82976905,62);
%LDR_DT_FILE(82976906,63);
%LDR_DT_FILE(82976907,64);
%LDR_DT_FILE(82976908,65);
