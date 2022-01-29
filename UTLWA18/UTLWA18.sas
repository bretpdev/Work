/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*%LET TBLLIB = /sas/whse/progrevw;*/
%LET RPTLIB = T:\SAS;
%LET TBLLIB = Q:\Process Automation\TabSAS;
FILENAME REPORT2 "&RPTLIB/ULWA18.LWA18R2";
FILENAME REPORT3 "&RPTLIB/ULWA18.LWA18R3";
FILENAME REPORT4 "&RPTLIB/ULWA18.LWA18R4";
FILENAME REPORT5 "&RPTLIB/ULWA18.LWA18R5";
FILENAME REPORTZ "&RPTLIB/ULWA18.LWA18RZ";
DATA _NULL_;
	INFILE "&TBLLIB/SWOP_VAR.txt" DLM=',' DSD MISSOVER LRECL=67000 FIRSTOBS=2;
	INFORMAT BOND $10. AMOUNT BEST12.;
	INPUT BOND $ AMOUNT;
	CALL SYMPUT('BOND',BOND);
	CALL SYMPUT('AMOUNT',AMOUNT);
RUN;
%LET MAX_LN = 25;
OPTIONS SYMBOLGEN NOCENTER NODATE NONUMBER LS=132;
%SYSLPUT AMOUNT = &AMOUNT;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
%MACRO SQLCHECK (SQLRPT= );
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
PROC SQL ;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE SWOPI AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,A.LN_SEQ
	,A.IC_LON_PGM
	,A.LC_STA_LON10
	,A.LF_LON_CUR_OWN
	,A.LA_CUR_PRI
	,A.LC_DFR_FOR_TYP
	,A.LD_LON_1_DSB
	,A.LD_TRM_BEG
	,A.LD_TRM_END
	,B.BOR_BAL
	,C.LN_BR_DSB_SEQ
	,C.LC_DSB_TYP
	,C.LC_STA_LON15
	,C.LD_DSB
	,COALESCE(C.LA_DSB,0) 			AS LA_DSB
	,COALESCE(C.LA_DSB_CAN,0) 		AS LA_DSB_CAN
	,D.WC_DW_LON_STA
	,E.IF_BND_ISS
	,COALESCE(A.LA_CUR_PRI,0) 		AS LA_CUR_PRI
	,COALESCE(A.LA_LTE_FEE_OTS,0) 	AS LA_LTE_FEE_OTS
	,COALESCE(D.LA_NSI_OTS,0) 		AS LA_NSI_OTS

	,COALESCE(A.LA_CUR_PRI,0) +
		COALESCE(A.LA_LTE_FEE_OTS,0) + 	
		COALESCE(D.LA_NSI_OTS,0) 		AS LN_BAL

	,PD10.DM_PRS_1
	,PD10.DM_PRS_LST
	,PD30.DX_STR_ADR_1
	,PD30.DX_STR_ADR_2
	,PD30.DM_CT
	,PD30.DC_DOM_ST
	,PD30.DF_ZIP_CDE
	,PD30.DM_FGN_CNY
	,PD10.DF_SPE_ACC_ID
	,PD30.DI_VLD_ADR
	,OL.OLIND
	,LN15.MAX_DISB_DT

FROM OLWHRM1.LN10_LON A
INNER JOIN (
	SELECT IJ1.BF_SSN
		,SUM(COALESCE(IJ1.LA_CUR_PRI,0)) +
		 SUM(COALESCE(IJ1.LA_LTE_FEE_OTS,0)) +
		 SUM(COALESCE(IJ2.LA_NSI_OTS,0)) AS BOR_BAL
	FROM OLWHRM1.LN10_LON IJ1
	LEFT OUTER JOIN OLWHRM1.DW01_DW_CLC_CLU IJ2
		ON IJ1.BF_SSN = IJ2.BF_SSN
		AND IJ1.LN_SEQ = IJ2.LN_SEQ
	WHERE IJ1.LC_STA_LON10 = 'R'
		AND IJ1.LD_LON_1_DSB < '07/01/2008'
	GROUP BY IJ1.BF_SSN
	) B
	ON A.BF_SSN = B.BF_SSN
INNER JOIN OLWHRM1.LN15_DSB C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ
LEFT OUTER JOIN OLWHRM1.DW01_DW_CLC_CLU D
	ON A.BF_SSN = D.BF_SSN
	AND A.LN_SEQ = D.LN_SEQ
INNER JOIN OLWHRM1.PD10_PRS_NME PD10
	ON PD10.DF_PRS_ID = A.BF_SSN
INNER JOIN OLWHRM1.PD30_PRS_ADR PD30
	ON PD30.DF_PRS_ID = A.BF_SSN
	AND PD30.DC_ADR = 'L'
LEFT OUTER JOIN (
	SELECT IJ3.BF_SSN
		,IJ3.LN_SEQ
		,IJ3.IF_BND_ISS
	FROM OLWHRM1.LN35_LON_OWN IJ3
	INNER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,MAX(LD_OWN_EFF_SR) AS MXDT
		FROM OLWHRM1.LN35_LON_OWN
		GROUP BY BF_SSN
			,LN_SEQ
		) IJ4
		ON IJ3.BF_SSN = IJ4.BF_SSN
		AND IJ3.LN_SEQ = IJ4.LN_SEQ
		AND IJ3.LD_OWN_EFF_SR = IJ4.MXDT
	) E
	ON A.BF_SSN = E.BF_SSN
	AND A.LN_SEQ = E.LN_SEQ
LEFT OUTER JOIN (
	SELECT DISTINCT O1.DF_PRS_ID_BR
		,'Y' AS OLIND
	FROM OLWHRM1.GA01_APP O1
	INNER JOIN OLWHRM1.GA10_LON_APP O2
		ON O1.AF_APL_ID = O2.AF_APL_ID
	INNER JOIN OLWHRM1.GA14_LON_STA O3
		ON O2.AF_APL_ID = O3.AF_APL_ID
		AND O2.AF_APL_ID_SFX = O3.AF_APL_ID_SFX
	WHERE O2.AC_PRC_STA = 'A'
		AND O2.AF_CUR_LON_SER_AGY = '700121'
		AND O3.AC_STA_GA14 = 'A'
		AND O3.AC_LON_STA_TYP IN 
		(
			'ID','IG','DA','FB','RP','CR','IM'
		)
	) OL
	ON A.BF_SSN = OL.DF_PRS_ID_BR
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,MAX(LD_DSB) AS MAX_DISB_DT
	FROM OLWHRM1.LN15_DSB 
	WHERE LA_DSB <> COALESCE(LA_DSB_CAN,0)
		AND LC_STA_LON15 IN ('1','3')
		AND 
		(
			(
				LC_DSB_TYP = '2'			
			)
		OR 
			(
				LC_DSB_TYP = '1'
				AND LD_DSB_ROS_PRT IS NOT NULL	
			)
		)
	GROUP BY BF_SSN
		,LN_SEQ
	) LN15
	ON A.BF_SSN = LN15.BF_SSN
	AND A.LN_SEQ = LN15.LN_SEQ

WHERE B.BOR_BAL < &Amount
	AND B.BOR_BAL != 0 
	AND A.LC_STA_LON10 = 'R'
	AND A.LD_LON_1_DSB < '07/01/2008'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWA18.LWA18RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA SWOPI ;
	SET WORKLOCL.SWOPI;
RUN;
PROC SORT DATA = SWOPI NODUPKEY; 
	BY BF_SSN LN_SEQ LN_BR_DSB_SEQ; 
RUN;

PROC SQL;
CREATE TABLE FIN AS 
SELECT DISTINCT A.BF_SSN
	,A.LN_SEQ
	,A.IC_LON_PGM
	,A.LC_STA_LON10
	,A.WC_DW_LON_STA
	,A.LF_LON_CUR_OWN
	,A.IF_BND_ISS
	,A.BOR_BAL
	,A.LN_BAL
	,A.LA_CUR_PRI
	,A.LA_LTE_FEE_OTS
	,A.LA_NSI_OTS
	,A.DM_PRS_1
	,A.DM_PRS_LST
	,A.DX_STR_ADR_1
	,A.DX_STR_ADR_2
	,A.DM_CT
	,A.DC_DOM_ST
	,A.DF_ZIP_CDE
	,A.DM_FGN_CNY
	,A.DF_SPE_ACC_ID
	,A.DI_VLD_ADR
	,A.OLIND
	,A.LD_LON_1_DSB
	,A.LD_TRM_BEG
	,A.LD_TRM_END
	,A.MAX_DISB_DT
FROM SWOPI A
/*EXCLUSION CATEGORIES*/
LEFT OUTER JOIN ( 
		SELECT DISTINCT A.BF_SSN /*LOANS NOT OWNED BY UHEAA*/
			,'X' AS EXCLD
		FROM SWOPI A
		WHERE EXISTS (
			SELECT *
			FROM SWOPI X
			WHERE X.BF_SSN = A.BF_SSN
				AND X.LN_SEQ = A.LN_SEQ
				AND X.LF_LON_CUR_OWN ^= '828476'
				AND X.LN_BAL > 0
			)
	UNION
		SELECT DISTINCT A.BF_SSN /*NOT FULLY DISBURSED*/
			,'X' AS EXCLD
		FROM SWOPI A
		WHERE NOT EXISTS (
			SELECT *
			FROM SWOPI X
			WHERE X.BF_SSN = A.BF_SSN
				AND X.LN_SEQ = A.LN_SEQ
				AND X.LC_STA_LON15 IN ('1','3')
				AND X.LC_DSB_TYP = '2'
			)
	UNION
		SELECT DISTINCT BF_SSN /*INTEREST ONLY BALANCE*/
			,'X' AS EXCLD
		FROM (
			SELECT DISTINCT A.BF_SSN
				,LN_SEQ
				,LA_CUR_PRI
				,SUM(LA_DSB) 
				,SUM(LA_DSB_CAN)
			FROM SWOPI A
			GROUP BY BF_SSN
				,LN_SEQ
				,LA_CUR_PRI
			HAVING SUM(LA_DSB) = SUM(LA_DSB_CAN) 
				AND LA_CUR_PRI > 0
			)
	UNION
		SELECT DISTINCT A.BF_SSN /*CREDIT BALANCE*/
			,'X' AS EXCLD
		FROM SWOPI A
		WHERE LA_CUR_PRI < 0
	UNION
		SELECT DISTINCT A.BF_SSN /*BOND ID*/
			,'X' AS EXCLD
		FROM SWOPI A
		WHERE IF_BND_ISS ^= "&Bond"
			AND LN_BAL > 0
	UNION
		SELECT DISTINCT A.BF_SSN /*LOAN STATUS*/
			,'X' AS EXCLD
		FROM SWOPI A
		WHERE WC_DW_LON_STA IN ('01','02')
	UNION
		SELECT DISTINCT A.BF_SSN  /*IN SCHOOL DEFERMENT*/
			,'X' AS EXCLD
		FROM SWOPI A
		WHERE WC_DW_LON_STA = '04' 
			AND LC_DFR_FOR_TYP IN ('15','16','18')
	) B
	ON A.BF_SSN = B.BF_SSN
WHERE B.EXCLD ^= 'X'
	AND LN_BAL > 0
;
QUIT;

PROC SORT DATA=FIN(WHERE=(WC_DW_LON_STA NOT IN ('08','12','17','19','21')))NODUPKEY;
	BY DC_DOM_ST BF_SSN LN_SEQ;
RUN;

DATA FIN ;
	SET FIN (DROP=WC_DW_LON_STA);
	IF OLIND = 'Y' THEN 
		DELETE;
	ELSE 
		OUTPUT FIN;
RUN;
/*************************************************************************************
* REMOVE ECASLA LOANS - NOTE THIS IS AT A LOAN LEVEL AND WASN'T INCLUDED IN THE 
* EXCLUSION PROCESSING ABOVE BECAUSE THE ECASLA EXCLUSION IS TAKING PLACE AT THE 
* LOAN LEVEL.
**************************************************************************************/
DATA FINX (DROP=E1_1ST_DSB_LB E1_1ST_DSB_UB E1_LN_TRM_DT  E1_MX_DSB_LB E1_MX_DSB_UB 
	E2_1ST_DSB_LB E2_LN_TRM_DT  E2_MX_DSB_LB  E2_MX_DSB_UB);
	SET FIN;
	/*ECASLA 1 DATE VARIABLE INITIALIZATION*/
	E1_1ST_DSB_LB = '01JUL2008'D;
	E1_1ST_DSB_UB = '01JUL2009'D;
	E1_LN_TRM_DT = '01JUL2008'D;
	E1_MX_DSB_LB = '01JUL2008'D;
	E1_MX_DSB_UB = '30SEP2009'D;
	/*ECASLA 2 DATE VARIABLE INITIALIZATION*/
	E2_1ST_DSB_LB = '01MAY2009'D;
	E2_LN_TRM_DT = '01JUL2009'D;
	E2_MX_DSB_LB = '01MAY2009'D;
	E2_MX_DSB_UB = '30SEP2010'D;
	/*DELETE IF ECASLA*/
	IF (E1_LN_TRM_DT <= LD_TRM_BEG OR LD_TRM_BEG <= E1_LN_TRM_DT <= LD_TRM_END) AND 
		E1_1ST_DSB_LB <= LD_LON_1_DSB < E1_1ST_DSB_UB AND	
		E1_MX_DSB_LB <= MAX_DISB_DT <= E1_MX_DSB_UB THEN 
			DELETE;
	ELSE IF (E2_LN_TRM_DT <= LD_TRM_BEG OR LD_TRM_BEG <= E2_LN_TRM_DT <= LD_TRM_END) AND
		E2_1ST_DSB_LB <= LD_LON_1_DSB AND
		E2_MX_DSB_LB <= MAX_DISB_DT <= E2_MX_DSB_UB THEN
			DELETE;
	ELSE 
		OUTPUT;
RUN;

*CALCULATE KEYLINE;
%MACRO KEY_CLC(TBL);
DATA &TBL (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET &TBL;
KEYSSN = TRANSLATE(BF_SSN,'MYLAUGHTER','0987654321');
MODAY = PUT(DATE(),MMDDYYN4.);
KEYLINE = "P"||KEYSSN||MODAY||'L';
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
%KEY_CLC(FIN);

DATA FIN ;
SET FIN;
FORMAT COST_CENTER_CODE $6.;
COST_CENTER_CODE = 'MA4119';
RUN;

DATA FIN2(KEEP=BF_SSN LN_SEQ LA_CUR_PRI LA_NSI_OTS) FIN4 ;
SET FIN;
RUN;

PROC SORT DATA=FIN4 NODUPKEY;
BY BF_SSN LN_SEQ;
RUN;

/********************************************************************************
* CREATE BORROWER LEVEL DATASET CONTAINING LOAN INFO
*********************************************************************************/
%MACRO ROLLUP(NEWDS,NEWCOL);
PROC TRANSPOSE DATA=FIN4 OUT=FIN4&NEWDS (DROP=_NAME_) PREFIX=&NEWCOL;
VAR &NEWCOL;
BY BF_SSN;
RUN;
%MEND ROLLUP;
%ROLLUP(1,LN_SEQ);
%ROLLUP(2,IC_LON_PGM);
%ROLLUP(3,LA_CUR_PRI);
%ROLLUP(4,LA_NSI_OTS);
%ROLLUP(5,LA_LTE_FEE_OTS);
DATA FIN_1LN (DROP=_LABEL_);
MERGE FIN4 (DROP=LN_SEQ IC_LON_PGM LA_CUR_PRI LA_NSI_OTS LA_LTE_FEE_OTS) FIN41 FIN42 FIN43 FIN44 FIN45;
BY BF_SSN;
RUN;
PROC SORT DATA=FIN_1LN NODUPKEY;
BY BF_SSN;
RUN;

DATA _NULL_;
SET  WORK.FIN2;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT BF_SSN $9. ;
   FORMAT LN_SEQ 6. ;
   FORMAT LA_CUR_PRI 10.2;
   FORMAT LA_NSI_OTS 15.2;
IF _N_ = 1 THEN      
DO;
   PUT 'BF_SSN,LN_SEQ,LA_CUR_PRI,LA_NSI_OTS';
END;
DO;
	PUT BF_SSN $ @;
	PUT LN_SEQ @;
	PUT LA_CUR_PRI @;
	PUT LA_NSI_OTS ;
END;
RUN;

%MACRO CF;
PROC SORT DATA=FIN_1LN ;
BY DC_DOM_ST COST_CENTER_CODE;
RUN;
DATA _NULL_;
SET WORK.FIN_1LN;
FILE REPORT4 DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT BF_SSN $9. ;
	FORMAT DM_PRS_1 $13. ;
	FORMAT DM_PRS_LST $23. ;
	FORMAT DX_STR_ADR_1 $30. ;
	FORMAT DX_STR_ADR_2 $30. ;
	FORMAT DM_CT $20. ;
	FORMAT DC_DOM_ST $2. ;
	FORMAT DF_ZIP_CDE $17. ;
	FORMAT DM_FGN_CNY $25. ;
	FORMAT DF_SPE_ACC_ID $10. ;
	FORMAT DI_VLD_ADR $1. ;
	FORMAT ACSKEY $18. ;
	FORMAT LN_SEQ1-LN_SEQ&MAX_LN 6. ;
	FORMAT IC_LON_PGM1-IC_LON_PGM&MAX_LN $6. ;
	FORMAT LA_CUR_PRI1-LA_CUR_PRI&MAX_LN Dollar10.2 ;
	FORMAT LA_NSI_OTS1-LA_NSI_OTS&MAX_LN Dollar10.2 ;
IF _N_ = 1 THEN /*FILE HEADER*/       
DO;
   PUT
   'BF_SSN'
   ','
   'DM_PRS_1'
   ','
   'DM_PRS_LST'
   ','
   'DX_STR_ADR_1'
   ','
   'DX_STR_ADR_2'
   ','
   'DM_CT'
   ','
   'DC_DOM_ST'
   ','
   'DF_ZIP_CDE'
   ','
   'DM_FGN_CNY'
   ','
   'DF_SPE_ACC_ID'
   ','
   'DI_VLD_ADR'
   ','
   'ACSKEY'
   ',' @;
	%DO I=1 %TO &MAX_LN;
		PUT "LOAN&I LOAN SEQUENCE" @;
		PUT "," @;
		PUT "LOAN&I LOAN TYPE" @;
		PUT "," @;
		PUT "LOAN&I PRINCIPAL AMOUNT" @;
		PUT "," @;
		PUT "LOAN&I INTEREST AMOUNT" @;
		PUT "," @;
	%END;
   PUT 
   'STATE_IND'
   ','
   'COST_CENTER_CODE';
END;
DO;
   PUT BF_SSN $ @;
   PUT DM_PRS_1 $ @;
   PUT DM_PRS_LST $ @;
   PUT DX_STR_ADR_1 $ @;
   PUT DX_STR_ADR_2 $ @;
   PUT DM_CT $ @;
   PUT DC_DOM_ST $ @;
   PUT DF_ZIP_CDE $ @;
   PUT DM_FGN_CNY $ @;
   PUT DF_SPE_ACC_ID $ @;
   PUT DI_VLD_ADR $ @;
   PUT ACSKEY $ @;
   %DO I=1 %TO &MAX_LN;
		PUT LN_SEQ&I @;
		PUT IC_LON_PGM&I $ @;
		PUT LA_CUR_PRI&I @;
		PUT LA_NSI_OTS&I @;
   %END;
   PUT DC_DOM_ST $ @;
   PUT COST_CENTER_CODE $;
END;
RUN;
%MEND CF;
%CF;

PROC SQL;
CREATE TABLE TOT_BAL AS
SELECT SUM(LA_CUR_PRI) +
	 SUM(LA_NSI_OTS) AS TOTBAL
	,COUNT(*) AS TOTLOAN
	,COUNT(DISTINCT BF_SSN) AS TOTBORR
FROM FIN
;
QUIT;

PROC PRINTTO PRINT=REPORT3 NEW;
RUN;
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS CENTER NODATE NUMBER PAGENO=1 PS=52 LS=96;
PROC PRINT NOOBS SPLIT='/' DATA=TOT_BAL;
FORMAT TOTBAL DOLLAR18.2;
VAR TOTBORR TOTLOAN TOTBAL;
LABEL	TOTBORR = 'TOTAL BORROWERS'
		TOTLOAN = 'TOTAL LOANS'
		TOTBAL = 'TOTAL OUTSTANDING BALANCE'
		;
TITLE 'Special Write-off Program';
TITLE2 'Final Summary';
FOOTNOTE  'JOB = UTLWA18     REPORT = UTLWA18.LWA18R3';
RUN;
PROC PRINTTO;
RUN;

PROC SQL;
	CREATE TABLE R5 AS
		SELECT BF_SSN
			,SUM(LA_LTE_FEE_OTS) AS LA_LTE_FEE_OTS1
			FROM FIN
			GROUP BY BF_SSN
			HAVING SUM(LA_LTE_FEE_OTS) > 0;
QUIT;

DATA _NULL_;
SET WORK.R5;
FILE REPORT5 DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT BF_SSN $9. ;
	FORMAT LA_LTE_FEE_OTS1 DOLLAR10.2 ;
DO;
   PUT BF_SSN $ @;
   PUT LA_LTE_FEE_OTS1;
END;
RUN;
