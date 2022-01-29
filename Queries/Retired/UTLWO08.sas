*****************************************;
*UTLWO08 ORIGINATION FEE BORROWER BENIFIT;
*****************************************;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORT2 "&RPTLIB/ULWO08.LWO08R2";
FILENAME REPORT3 "&RPTLIB/ULWO08.LWO08R3";
FILENAME REPORT4 "&RPTLIB/ULWO08.LWO08R4";
FILENAME REPORT5 "&RPTLIB/ULWO08.LWO08R5";
FILENAME REPORT6 "&RPTLIB/ULWO08.LWO08R6";
FILENAME REPORT7 "&RPTLIB/ULWO08.LWO08R7";
FILENAME REPORTZ "&RPTLIB/ULWO08.LWO08RZ";
DATA _NULL_;
	CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
	CALL SYMPUT('RUNTIME',PUT(TIME(), TIME.));
RUN;
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LENDER_ID)||"'"
		INTO :UHEAA_LIST SEPARATED BY ","
	FROM SAS_TAB.LDR_AFF
	WHERE AFFILIATION = 'UHEAA';
QUIT;
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
CREATE TABLE NOPRCTR_A AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,B.DM_PRS_LST
	,A.LN_SEQ
	,A.IC_LON_PGM
	,A.LD_LON_1_DSB
	,D.ID_LON_SLE
	,E.AMNT
    ,E.TRX
	,F.IF_SLL_OWN_SLD
	,F.IF_BUY_OWN_SLD
	,A.LF_LON_ALT
	,A.LN_LON_ALT_SEQ
	,ERR_TRX.EGT01_IND
	,A.LF_RGL_CAT_LP20
	,CASE
		WHEN A.IC_LON_PGM IN ('PLUS','PLUSGB') AND 
			DAYS(A.LD_LON_1_DSB) BETWEEN DAYS('07/01/2006') AND DAYS('08/07/2006')
		THEN 'X'
		ELSE ''
	 END AS DSBCHK

FROM OLWHRM1.LN10_LON A 
INNER JOIN OLWHRM1.PD10_PRS_NME B
	 ON A.BF_SSN = B.DF_PRS_ID 
INNER JOIN OLWHRM1.LN35_LON_OWN C
	 ON A.BF_SSN = C.BF_SSN
	 AND A.LN_SEQ = C.LN_SEQ
INNER JOIN OLWHRM1.OW30_LON_SLE_CTL D
	 ON C.IF_LON_SLE = D.IF_LON_SLE
INNER JOIN 
	(SELECT BF_SSN, 
	   LN_SEQ,
	   SUM(COALESCE(LA_FAT_CUR_PRI,0)) AS AMNT,
	   PC_FAT_TYP || PC_FAT_SUB_TYP AS TRX
	FROM OLWHRM1.LN90_FIN_ATY 
	WHERE (
			( 
				PC_FAT_TYP = '01'
			    AND PC_FAT_SUB_TYP = '01'
			)
			OR ( 
				PC_FAT_TYP = '06'
				AND PC_FAT_SUB_TYP = '85'
				)
			OR ( 
				PC_FAT_TYP = '14'
				AND PC_FAT_SUB_TYP = '01'
				)
			OR (
				PC_FAT_TYP = '02'
				AND PC_FAT_SUB_TYP = '91'
				)
			OR (
				PC_FAT_TYP = '14'
				AND PC_FAT_SUB_TYP IN ('86','48')
				)
			)
		AND		LC_FAT_REV_REA = ' '
		AND 	LC_STA_LON90 = 'A'
	 GROUP BY BF_SSN, LN_SEQ, PC_FAT_TYP || PC_FAT_SUB_TYP 
	 ) E
	ON A.BF_SSN = E.BF_SSN
	AND A.LN_SEQ = E.LN_SEQ
LEFT OUTER JOIN OLWHRM1.LN99_LON_SLE_FAT F
	ON C.BF_SSN = F.BF_SSN
	AND C.LN_SEQ = F.LN_SEQ
	AND C.IF_LON_SLE = F.IF_LON_SLE
LEFT OUTER JOIN 
	(SELECT BF_SSN
		,LN_SEQ
		,'Y' AS EGT01_IND
	 FROM OLWHRM1.LN90_FIN_ATY 
	 WHERE PC_FAT_TYP = '80'
	 AND PC_FAT_SUB_TYP = '01'
	 AND LC_FAT_REV_REA = ' '
	 AND LC_STA_LON90 = 'A'
	 ) ERR_TRX
	ON A.BF_SSN = ERR_TRX.BF_SSN
	AND A.LN_SEQ = ERR_TRX.LN_SEQ
INNER JOIN (
	SELECT G1.BF_SSN
		,G2.LN_SEQ
		, SUM(G1.LA_DSB_FEE) AS LA_DSB_FEE 
	FROM OLWHRM1.LN18_DSB_FEE G1
	INNER JOIN OLWHRM1.LN15_DSB G2
		ON G1.BF_SSN = G2.BF_SSN
		AND G1.LN_BR_DSB_SEQ = G2.LN_BR_DSB_SEQ
		AND G2.LC_DSB_TYP = '2'
		AND (G2.LC_STA_LON15 = '1' OR G2.LC_STA_LON15 = '3')
		AND (G2.LA_DSB_CAN <> G2.LA_DSB OR G2.LA_DSB_CAN IS NULL)
	GROUP BY G1.BF_SSN, G2.LN_SEQ
	 ) G
	ON A.BF_SSN = G.BF_SSN
	AND A.LN_SEQ = G.LN_SEQ

WHERE A.LF_LON_CUR_OWN = '828476'
AND	A.LD_LON_GTR BETWEEN '05/01/2000' AND '06/30/2008'
AND	C.LC_STA_LON35 = 'A'
AND	C.IF_BND_ISS <> ' '
AND	D.IC_LON_SLE_STA = 'C'
AND DAYS(D.ID_LON_SLE) = DAYS(CURRENT DATE) - 1 
AND	D.IC_LON_SLE_TYP = 'T'
AND	D.IF_BUY_BND_ISS <> ' '
AND G.LA_DSB_FEE > 0

FOR READ ONLY WITH UR
);

CREATE TABLE PRCTR_A AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,B.DM_PRS_LST
	,A.LN_SEQ
	,A.IC_LON_PGM
	,A.LD_LON_1_DSB
	,D.ID_LON_SLE
	,E.AMNT
    ,E.TRX
	,E.LD_FAT_EFF
	,E.LD_FAT_APL
	,F.IF_SLL_OWN_SLD
	,F.IF_BUY_OWN_SLD
	,A.IF_TIR_PCE
	,A.LF_RGL_CAT_LP20
	,CASE
		WHEN A.IC_LON_PGM IN ('PLUS','PLUSGB') AND 
			DAYS(A.LD_LON_1_DSB) BETWEEN DAYS('07/01/2006') AND DAYS('08/07/2006')
		THEN 'X'
		ELSE ''
 	 END AS DSBCHK
FROM OLWHRM1.LN10_LON A 
INNER JOIN OLWHRM1.PD10_PRS_NME B
	 ON A.BF_SSN = B.DF_PRS_ID 
INNER JOIN OLWHRM1.LN35_LON_OWN C
	 ON A.BF_SSN = C.BF_SSN
	 AND A.LN_SEQ = C.LN_SEQ
INNER JOIN OLWHRM1.OW30_LON_SLE_CTL D
	 ON C.IF_LON_SLE = D.IF_LON_SLE
INNER JOIN 
	(SELECT BF_SSN, 
	   LN_SEQ,
	   LD_FAT_EFF,
	   LD_FAT_APL,
	   LA_FAT_CUR_PRI AS AMNT,
	   PC_FAT_TYP || PC_FAT_SUB_TYP AS TRX
	FROM OLWHRM1.LN90_FIN_ATY 
	WHERE (
			  ( PC_FAT_TYP = '03'
			    AND PC_FAT_SUB_TYP = '90')
		OR 	  ( PC_FAT_TYP = '70'
				AND PC_FAT_SUB_TYP = '01')
		OR 	  ( PC_FAT_TYP = '26'
				AND PC_FAT_SUB_TYP = '01')
		OR 	  ( PC_FAT_TYP = '10'
				AND PC_FAT_SUB_TYP IN ('10','11','12'))
		   )
		AND		LC_FAT_REV_REA = ' '
		AND 	LC_STA_LON90 = 'A'
	 ) E
	ON A.BF_SSN = E.BF_SSN
	AND A.LN_SEQ = E.LN_SEQ
LEFT OUTER JOIN OLWHRM1.LN99_LON_SLE_FAT F
	ON C.BF_SSN = F.BF_SSN
	AND C.LN_SEQ = F.LN_SEQ
	AND C.IF_LON_SLE = F.IF_LON_SLE
INNER JOIN (
	SELECT G1.BF_SSN
		,G2.LN_SEQ
		,SUM(G1.LA_DSB_FEE) AS LA_DSB_FEE 
	FROM OLWHRM1.LN18_DSB_FEE G1
	INNER JOIN OLWHRM1.LN15_DSB G2
		ON G1.BF_SSN = G2.BF_SSN
		AND G1.LN_BR_DSB_SEQ = G2.LN_BR_DSB_SEQ
		AND G2.LC_DSB_TYP = '2'
		AND (G2.LC_STA_LON15 = '1' OR G2.LC_STA_LON15 = '3')
		AND (G2.LA_DSB_CAN <> G2.LA_DSB OR G2.LA_DSB_CAN IS NULL)
	GROUP BY G1.BF_SSN, G2.LN_SEQ
	 ) G
	ON A.BF_SSN = G.BF_SSN
	AND A.LN_SEQ = G.LN_SEQ

WHERE A.LF_LON_CUR_OWN = '828476'
AND	A.LD_LON_GTR BETWEEN '05/01/2000' AND '06/30/2008'
AND	C.LC_STA_LON35 = 'A'
AND	C.IF_BND_ISS <> ' '
AND	D.IC_LON_SLE_STA = 'C'
AND DAYS(D.ID_LON_SLE) = DAYS(CURRENT DATE) - 1 
AND	D.IC_LON_SLE_TYP = 'T'
AND	D.IF_BUY_BND_ISS <> ' '
AND G.LA_DSB_FEE = 0

FOR READ ONLY WITH UR
);

CREATE TABLE ZIONS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,E.LF_LON_ALT
	,E.LN_LON_ALT_SEQ
	,E.AMNT
	,E.TRX
	,O291.O291_IND
	,O101.O101_IND
	,ERR_TRX.EGT01_IND
	,'Y' AS ZB_IND

FROM OLWHRM1.LN10_LON A 
INNER JOIN 
	(SELECT V.BF_SSN, 
	   V.LF_LON_ALT,
	   V.LN_LON_ALT_SEQ,
	   SUM(COALESCE(Q.LA_FAT_CUR_PRI,0)) AS AMNT,
	   Q.PC_FAT_TYP || Q.PC_FAT_SUB_TYP AS TRX
	FROM OLWHRM1.LN90_FIN_ATY_ZB Q
	INNER JOIN OLWHRM1.LN10_LON_ZB V
		ON Q.BF_SSN = V.BF_SSN
		AND Q.LN_SEQ = V.LN_SEQ
	WHERE (
			( 
				Q.PC_FAT_TYP = '01'
			  	AND Q.PC_FAT_SUB_TYP = '01'
			)
			OR ( 
				Q.PC_FAT_TYP = '06'
				AND Q.PC_FAT_SUB_TYP = '85'
				)
			OR ( 
				Q.PC_FAT_TYP = '14'
				AND Q.PC_FAT_SUB_TYP = '01'
				)
			OR ( 
				Q.PC_FAT_TYP = '02'
				AND Q.PC_FAT_SUB_TYP = '91'
				)
			OR (
				Q.PC_FAT_TYP = '14'
				AND Q.PC_FAT_SUB_TYP IN ('86','48')
			   )
			)
		AND	Q.LC_FAT_REV_REA = ' '
		AND Q.LC_STA_LON90 = 'A'
	 GROUP BY V.BF_SSN,V.LF_LON_ALT,V.LN_LON_ALT_SEQ, Q.PC_FAT_TYP || Q.PC_FAT_SUB_TYP 
	 ) E
	ON A.BF_SSN = E.BF_SSN
	AND A.LF_LON_ALT = E.LF_LON_ALT
	AND A.LN_LON_ALT_SEQ = E.LN_LON_ALT_SEQ

LEFT OUTER JOIN 
	(SELECT V.BF_SSN
		,V.LF_LON_ALT
	   	,V.LN_LON_ALT_SEQ
		,'Y' AS O291_IND
	 FROM OLWHRM1.LN90_FIN_ATY_ZB Q
	 INNER JOIN OLWHRM1.LN10_LON_ZB V
		ON Q.BF_SSN = V.BF_SSN
		AND Q.LN_SEQ = V.LN_SEQ
	 WHERE PC_FAT_TYP = '02'
	 AND PC_FAT_SUB_TYP = '91'
	 AND LC_FAT_REV_REA = ' '
	 AND LC_STA_LON90 = 'A'
	 ) O291
	ON A.BF_SSN = O291.BF_SSN
	AND A.LF_LON_ALT = O291.LF_LON_ALT
	AND A.LN_LON_ALT_SEQ = O291.LN_LON_ALT_SEQ
LEFT OUTER JOIN 
	(SELECT V.BF_SSN
		,V.LF_LON_ALT
	   	,V.LN_LON_ALT_SEQ
		,'Y' AS O101_IND
	 FROM OLWHRM1.LN90_FIN_ATY_ZB Q
	 INNER JOIN OLWHRM1.LN10_LON_ZB V
		ON Q.BF_SSN = V.BF_SSN
		AND Q.LN_SEQ = V.LN_SEQ
	 WHERE PC_FAT_TYP = '01'
	 AND PC_FAT_SUB_TYP = '01'
	 AND LC_FAT_REV_REA = ' '
	 AND LC_STA_LON90 = 'A'
	 ) O101
	ON A.BF_SSN = O101.BF_SSN
	AND A.LF_LON_ALT = O101.LF_LON_ALT
	AND A.LN_LON_ALT_SEQ = O101.LN_LON_ALT_SEQ
LEFT OUTER JOIN 
	(SELECT V.BF_SSN
		,V.LF_LON_ALT
	   	,V.LN_LON_ALT_SEQ
		,'Y' AS EGT01_IND
	  FROM OLWHRM1.LN90_FIN_ATY_ZB Q
	  INNER JOIN OLWHRM1.LN10_LON_ZB V
		ON Q.BF_SSN = V.BF_SSN
		AND Q.LN_SEQ = V.LN_SEQ
	 WHERE PC_FAT_TYP = '80'
	 AND PC_FAT_SUB_TYP = '01'
	 AND LC_FAT_REV_REA = ' '
	 AND LC_STA_LON90 = 'A'
	 ) ERR_TRX
	ON A.BF_SSN = ERR_TRX.BF_SSN
	AND A.LF_LON_ALT = ERR_TRX.LF_LON_ALT
	AND A.LN_LON_ALT_SEQ = ERR_TRX.LN_LON_ALT_SEQ

WHERE A.IF_DOE_LDR = '817455'
AND A.LF_LON_CUR_OWN = '828476'
AND	A.LD_LON_GTR BETWEEN '05/01/2000' AND '06/30/2008'
AND EXISTS 
	(SELECT *
	 FROM OLWHRM1.LN90_FIN_ATY X
	 WHERE X.BF_SSN = A.BF_SSN
	 AND X.LN_SEQ = A.LN_SEQ
	 AND X.PC_FAT_TYP = '02'
	 AND X.PC_FAT_SUB_TYP = '91')

FOR READ ONLY WITH UR
);

CREATE TABLE BHVE AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,B.DM_PRS_LST
	,A.LN_SEQ
	,A.IC_LON_PGM
	,A.LD_LON_1_DSB
	,E.AMNT
    ,E.TRX
	,A.LF_LON_ALT
	,A.LN_LON_ALT_SEQ
	,A.LF_RGL_CAT_LP20
	,CASE
		WHEN A.IC_LON_PGM IN ('PLUS','PLUSGB') AND 
			DAYS(A.LD_LON_1_DSB) BETWEEN DAYS('07/01/2006') AND DAYS('08/07/2006')
		THEN 'X'
		ELSE ''
	 END AS DSBCHK
	,A.IF_TIR_PCE
FROM OLWHRM1.LN10_LON A 
INNER JOIN OLWHRM1.PD10_PRS_NME B
	 ON A.BF_SSN = B.DF_PRS_ID 
INNER JOIN OLWHRM1.LN15_DSB F
	ON A.BF_SSN = F.BF_SSN 
	AND A.LN_SEQ = F.LN_SEQ
INNER JOIN OLWHRM1.AP03_MASTER_APL G
	ON F.AF_APL_ID = G.AF_APL_ID	
LEFT OUTER JOIN 
	(SELECT BF_SSN, 
	   LN_SEQ,
	   SUM(COALESCE(LA_FAT_CUR_PRI,0)) AS AMNT,
	   PC_FAT_TYP || PC_FAT_SUB_TYP AS TRX
	FROM OLWHRM1.LN90_FIN_ATY 
	WHERE 
		(
			(PC_FAT_TYP = '01' AND PC_FAT_SUB_TYP = '01') OR 
			(PC_FAT_TYP = '06' AND PC_FAT_SUB_TYP = '85') OR 
			(PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '01') OR 
			(PC_FAT_TYP = '02' AND PC_FAT_SUB_TYP = '91') OR 
			(PC_FAT_TYP = '03' AND PC_FAT_SUB_TYP = '90') OR
			(PC_FAT_TYP = '70' AND PC_FAT_SUB_TYP = '01') OR
			(PC_FAT_TYP = '26' AND PC_FAT_SUB_TYP = '01') OR
			(PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP IN ('86','48')) OR 
			(PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP IN ('10','11','12'))
		)
		AND LC_FAT_REV_REA = ' '
		AND LC_STA_LON90 = 'A'
	 	GROUP BY BF_SSN, LN_SEQ, PC_FAT_TYP || PC_FAT_SUB_TYP 
	 ) E
	ON A.BF_SSN = E.BF_SSN
	AND A.LN_SEQ = E.LN_SEQ
INNER JOIN (
	SELECT X.BF_SSN
		,X.LN_SEQ
		,SUM(COALESCE(Y.LA_DSB_FEE,0)) AS OGFEE
	FROM OLWHRM1.LN15_DSB X
	INNER JOIN OLWHRM1.LN18_DSB_FEE Y
		ON X.BF_SSN = Y.BF_SSN
		AND X.LN_BR_DSB_SEQ = Y.LN_BR_DSB_SEQ
	GROUP BY X.BF_SSN
		,X.LN_SEQ
	) O
	ON A.BF_SSN = O.BF_SSN
	AND A.LN_SEQ = O.LN_SEQ

WHERE G.AF_LDR_ORG = '828476'
AND A.IC_LON_PGM IN ('PLUS','PLUSGB')
AND O.OGFEE > 0
AND F.LC_DSB_TYP = '2'
AND F.LC_STA_LON15 IN ('1','3')
AND F.LA_DSB <> COALESCE(F.LA_DSB_CAN,0)
FOR READ ONLY WITH UR
);

CREATE TABLE XCLD AS
SELECT DISTINCT X.*
FROM CONNECTION TO DB2 (
	SELECT A.BF_SSN	
		,A.LN_SEQ
	FROM OLWHRM1.LN99_LON_SLE_FAT A
	INNER JOIN OLWHRM1.OW30_LON_SLE_CTL B
		 ON A.IF_LON_SLE = B.IF_LON_SLE
	WHERE DAYS(B.ID_LON_SLE) = DAYS(CURRENT DATE) - 1
		AND	
		(
			(A.IF_SLL_OWN_SLD = '813915' AND A.IF_BUY_OWN_SLD = '813894') OR
			(A.IF_SLL_OWN_SLD = '820043' AND A.IF_BUY_OWN_SLD = '829505') OR
			(A.IF_SLL_OWN_SLD = '813760' AND A.IF_BUY_OWN_SLD = '813760UT') OR
			(A.IF_SLL_OWN_SLD IN (&UHEAA_LIST))
		)
UNION
	SELECT BF_SSN
		,LN_SEQ
	FROM OLWHRM1.LN10_LON
	WHERE 
		(
			IC_LON_PGM IN ('PLUS','PLUSGB') AND 
			LD_LON_1_DSB >= '01/01/2008'
		)
	OR 
		(
			IC_LON_PGM IN ('STFFRD','UNSTFD') AND 
			LD_LON_1_DSB >= '07/01/2009'
		)
) X
;

DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;
%SQLCHECK (SQLRPT=ULWO08.LWO08RZ);
QUIT;

/*---------------- EXCLUDE LENDER MERGES ----------------*/
%MACRO EXCLUDE(NEW_TAB,OLD_TAB);
PROC SQL;
CREATE TABLE &NEW_TAB AS 
SELECT DISTINCT A.*
FROM &OLD_TAB A
WHERE NOT EXISTS (
	SELECT *
	FROM XCLD X
	WHERE X.BF_SSN = A.BF_SSN
	AND X.LN_SEQ = A.LN_SEQ
	)
;
QUIT;
%MEND EXCLUDE;
%EXCLUDE(NOPRCTR,NOPRCTR_A);
%EXCLUDE(PRCTR,PRCTR_A);

DATA BHVE_PRCTR BHVE_NOPRCTR;
SET BHVE;
IF IF_TIR_PCE = '' THEN OUTPUT BHVE_NOPRCTR;
ELSE OUTPUT BHVE_PRCTR;
RUN;
%MACRO DSCOMB(DS1,DS2);
PROC DATASETS FORCE;
APPEND OUT=&DS1 DATA=&DS2;
QUIT; 
%MEND DSCOMB;
%DSCOMB(NOPRCTR,BHVE_NOPRCTR);
%DSCOMB(PRCTR,BHVE_PRCTR);
/*ENDRSUBMIT;*/
/*DATA PRCTR;SET WORKLOCL.PRCTR;RUN;*/
/*DATA NOPRCTR;SET WORKLOCL.NOPRCTR;RUN;*/
/*DATA ZIONS;SET WORKLOCL.ZIONS;RUN;*/
/*========================================================================*/
/*MACRO DEFINITIONS*/
/*========================================================================*/
%MACRO STP1(DS);
PROC SORT DATA=&DS;
	BY BF_SSN DM_PRS_LST LN_SEQ ID_LON_SLE AMNT TRX IF_SLL_OWN_SLD IF_BUY_OWN_SLD;
RUN;
DATA &DS;
	SET &DS;
	WHERE IF_SLL_OWN_SLD NE IF_BUY_OWN_SLD AND IF_SLL_OWN_SLD NE '828476';
RUN;
%MEND STP1;
/*-------------------------------------------------------------------------*/
%MACRO STP2(DS,BYVAL1,BYVAL2);
PROC SORT DATA=&DS NODUPKEY;
	BY &BYVAL1 ;
RUN;
PROC TRANSPOSE DATA=&DS OUT=&DS (DROP=_NAME_ _LABEL_);
	VAR AMNT;
	BY &BYVAL2 ;
	ID TRX;
RUN;
%MEND STP2;
/*------------------------------------------------------------------------*/
%MACRO STP3(DS,TRX_TYP);
DATA &DS;
	SET &DS;
	FORMAT &TRX_TYP 10.2;
	IF &TRX_TYP = . THEN &TRX_TYP = 0;
	ELSE &TRX_TYP = ABS(&TRX_TYP);
RUN;
%MEND STP3;
/*------------------------------------------------------------------------*/
%MACRO STP4(DS,STMNT);
DATA &DS;
	SET &DS;
	&STMNT;
RUN;
%MEND STP4;
/*------------------------------------------------------------------------*/
%MACRO TBLCRT(DS1,DS2);
PROC SQL;
CREATE TABLE &DS1 AS 
SELECT DISTINCT TABLE1.*
FROM NOPRCTR TABLE1
INNER JOIN &DS2 TABLE2
	ON TABLE1.BF_SSN = TABLE2.BF_SSN
	AND TABLE1.LN_SEQ = TABLE2.LN_SEQ;
QUIT;
RUN;
%MEND TBLCRT;
/*------------------------------------------------------------------------*/
%MACRO GETRX(DS,CRIT);
DATA &DS;
	SET &DS;
	WHERE &CRIT ; 
RUN;
%MEND GETRX;
/*------------------------------------------------------------------------*/
%MACRO SCENUM(DS,CALVAR);
DATA &DS ;
	SET &DS;
	FRMNM = &CALVAR;
RUN;
%MEND SCENUM;
/*------------------------------------------------------------------------*/
%MACRO PAYCONSOL(DS);
PROC SORT DATA=&DS; 
	BY BF_SSN LN_SEQ TRX;
RUN;
DATA &DS;
	SET &DS;
	BY BF_SSN LN_SEQ TRX;
	IF FIRST.TRX THEN X=0;
	X+AMNT;
	IF LAST.TRX THEN OUTPUT;
RUN;
DATA &DS (DROP=X);
	SET &DS;
	AMNT = X;
RUN;
%MEND PAYCONSOL;
/*------------------------------------------------------------------------*/
%MACRO OG_FEE(DS);
DATA &DS (DROP=LF_RGL_CAT_LP20 IC_LON_PGM LD_LON_1_DSB);
SET &DS;
IF LF_RGL_CAT_LP20 <= '1999030' OR (IC_LON_PGM IN ('PLUS','PLUSGB') AND LD_LON_1_DSB >= '08AUG2006'D) 
	THEN OFEE = .03;
ELSE IF LF_RGL_CAT_LP20 = '2006020' 
	THEN OFEE = .02 ;
ELSE IF LF_RGL_CAT_LP20 = '2007020' 
	THEN OFEE = .015 ;
ELSE IF LF_RGL_CAT_LP20 = '2008020' 
	THEN OFEE = .010 ;
ELSE IF LF_RGL_CAT_LP20 = '2009020' 
	THEN OFEE = .005 ;
RUN;
%MEND OG_FEE;
/*------------------------------------------------------------------------*/
%MACRO PLUS_DISB(DSB,DSN);
DATA &DSB &DSN;
SET &DSB;
IF DSBCHK = 'X' THEN OUTPUT &DSN;
ELSE OUTPUT &DSB;
RUN;
%MEND PLUS_DISB;
/*========================================================================*/
/*PRICE TIER PROCESSING*/
/*========================================================================*/
%PLUS_DISB(PRCTR,DSBSA);
%OG_FEE(PRCTR);
%STP1(PRCTR);

DATA PRCTR;
SET PRCTR;
IF TRX = '0390' AND LD_FAT_EFF NE ID_LON_SLE THEN DELETE;
ELSE OUTPUT;
RUN;

DATA DTCRIT (KEEP=BF_SSN LN_SEQ SALE_DT);
SET PRCTR (RENAME=(LD_FAT_EFF=SALE_DT));
WHERE TRX = '0390';
RUN;

PROC SORT DATA=DTCRIT NODUPKEY;BY BF_SSN LN_SEQ;RUN;

DATA PRCTR (DROP=LD_FAT_EFF);
MERGE PRCTR (IN=A) DTCRIT (IN=B);
BY BF_SSN LN_SEQ;
RUN;

DATA PRCTR;
SET PRCTR;
IF TRX = '0390' THEN OUTPUT;
ELSE IF LD_FAT_APL LT SALE_DT THEN OUTPUT;
ELSE DELETE;
RUN;

PROC SORT DATA=PRCTR; 
BY BF_SSN LN_SEQ TRX;
RUN;

DATA PRCTR;
SET PRCTR;
BY BF_SSN LN_SEQ TRX;
IF FIRST.TRX THEN X=0;
X+AMNT;
IF LAST.TRX THEN OUTPUT;
RUN;

DATA PRCTR (DROP=X);
SET PRCTR;
AMNT = X;
RUN;

%STP2(PRCTR,BF_SSN DM_PRS_LST LN_SEQ ID_LON_SLE AMNT TRX IF_SLL_OWN_SLD IF_BUY_OWN_SLD IF_TIR_PCE OFEE,
	BF_SSN DM_PRS_LST LN_SEQ ID_LON_SLE IF_TIR_PCE OFEE);

%STP3(PRCTR,_0390);
%STP3(PRCTR,_7001);
%STP3(PRCTR,_2601);
%STP3(PRCTR,_1010);
%STP3(PRCTR,_1011);
%STP3(PRCTR,_1012);

DATA PRCTR;
SET PRCTR; 
FORMAT CR_AMT 10.2;
CR_AMT = ROUND((_0390 + _1010 + _1011 + _1012 - _7001 - _2601) * OFEE,.01);
RUN;

%STP4(PRCTR,IF BF_SSN = ' ' THEN DELETE);
PROC SORT DATA=PRCTR NODUPKEY;BY BF_SSN LN_SEQ;RUN;

/*========================================================================*/
/*NO PRICE TIER PROCESSING*/
/*========================================================================*/
%PLUS_DISB(NOPRCTR,DSBSB);
%OG_FEE(NOPRCTR);

PROC SORT DATA=ZIONS NODUPRECS;
BY BF_SSN LF_LON_ALT LN_LON_ALT_SEQ;
RUN;

PROC SQL;
CREATE TABLE HKUP AS 
SELECT DISTINCT A.BF_SSN
	,A.DM_PRS_LST
	,A.LN_SEQ
	,A.ID_LON_SLE
	,A.IF_SLL_OWN_SLD
	,A.IF_BUY_OWN_SLD
	,A.EGT01_IND 
	,A.OFEE
	,B.AMNT
	,B.TRX
	,B.O291_IND
	,B.O101_IND
	,B.ZB_IND
	,B.EGT01_IND AS ZB_EX
FROM NOPRCTR A
INNER JOIN ZIONS B
	ON A.LF_LON_ALT = B.LF_LON_ALT
	AND A.LN_LON_ALT_SEQ = B.LN_LON_ALT_SEQ
;
QUIT;
RUN;

DATA NOPRCTR (DROP=LF_LON_ALT LN_LON_ALT_SEQ);
SET NOPRCTR HKUP;
RUN;

%STP1(NOPRCTR);

/*============================================================*/
/*CREATE DATA SET FOR MANUAL REVIEW*/
/*============================================================*/
DATA _8001 ;
SET NOPRCTR;
WHERE EGT01_IND = 'Y' OR ZB_EX = 'Y';
RUN;

PROC SORT DATA=_8001;BY BF_SSN LN_SEQ;RUN;
PROC SORT DATA=NOPRCTR;BY BF_SSN LN_SEQ;RUN;

DATA _8001 (KEEP=BF_SSN LN_SEQ DM_PRS_LST) NOPRCTR;
MERGE _8001 (IN=A) NOPRCTR (IN=B);
BY BF_SSN LN_SEQ;
IF A THEN OUTPUT _8001;
ELSE OUTPUT NOPRCTR;
RUN;

PROC SORT DATA=_8001 NODUPKEY;BY BF_SSN LN_SEQ;RUN;
/*============================================================*/

DATA AX (KEEP=BF_SSN LN_SEQ) BX(KEEP=BF_SSN LN_SEQ) ;
SET NOPRCTR;
IF O291_IND = 'Y' THEN OUTPUT AX;
ELSE IF O101_IND = 'Y' AND O291_IND = '' THEN OUTPUT BX;
RUN;  

PROC SORT DATA=AX NODUPKEY;BY BF_SSN LN_SEQ;RUN;
PROC SORT DATA=BX NODUPKEY;BY BF_SSN LN_SEQ;RUN;

%TBLCRT(SEQ1,AX);
%TBLCRT(SEQ2,BX);

PROC SQL;
CREATE TABLE SEQ3 AS 
SELECT DISTINCT D.*
FROM NOPRCTR D
WHERE NOT EXISTS (
	SELECT *
	FROM AX A
	WHERE A.BF_SSN = D.BF_SSN
	AND A.LN_SEQ = D.LN_SEQ
	)
AND NOT EXISTS (
	SELECT *
	FROM BX B
	WHERE B.BF_SSN = D.BF_SSN
	AND B.LN_SEQ = D.LN_SEQ
	)
;
QUIT;

%GETRX(
	SEQ1,TRX EQ '0101' 
	OR TRX EQ '0685' 
	OR TRX EQ '1401'
	OR TRX EQ '1486'
	OR TRX EQ '1448' 
	OR TRX EQ '0291' AND ZB_IND EQ 'Y'
	);
%GETRX(
	SEQ2,TRX EQ '0101' 
	OR TRX EQ '0685'
	OR TRX EQ '1401'
	OR TRX EQ '1486'
	OR TRX EQ '1448' 
	);
%SCENUM(SEQ1,1);
%SCENUM(SEQ2,2);
%SCENUM(SEQ3,3);
DATA NOPRCTR (DROP=FRMNM) FORMULA(KEEP=BF_SSN LN_SEQ FRMNM);
SET SEQ1 SEQ2 SEQ3;
RUN;
%PAYCONSOL(NOPRCTR);
%STP2(NOPRCTR,
	BF_SSN DM_PRS_LST LN_SEQ ID_LON_SLE AMNT TRX IF_SLL_OWN_SLD IF_BUY_OWN_SLD OFEE,
	BF_SSN DM_PRS_LST LN_SEQ ID_LON_SLE OFEE);
%STP3(NOPRCTR,_0101);
%STP3(NOPRCTR,_0685);
%STP3(NOPRCTR,_1401);
%STP3(NOPRCTR,_0291);
%STP3(NOPRCTR,_1486);
%STP3(NOPRCTR,_1448);

PROC SORT DATA=FORMULA NODUPKEY;
BY BF_SSN LN_SEQ FRMNM;
RUN;

DATA NOPRCTR;
MERGE NOPRCTR FORMULA;
BY BF_SSN LN_SEQ;
RUN;

DATA NOPRCTR;
SET NOPRCTR; 
FORMAT CR_AMT 10.2 CR_EFF DATE9.;
CR_EFF = ID_LON_SLE + 1;
IF FRMNM IN (1,3) THEN 
	CR_AMT = ROUND(((_0101 + _0291 + _0685) * OFEE) - _1401 - _1486 - _1448,.01);
ELSE IF FRMNM = 2 THEN 
	CR_AMT = ROUND(((_0101 + _0685) * OFEE) - _1401 - _1486 - _1448,.01);
RUN;
/*========================================================================*/
/*GET TOTALS*/
/*========================================================================*/
PROC SQL;
CREATE TABLE TOTALS AS
SELECT COUNT(*) AS TOT_NUM
	,COALESCE(SUM(CR_AMT),0) AS TOT_AMT
FROM NOPRCTR
WHERE CR_AMT <> 0;
QUIT;
%STP4(NOPRCTR,IF BF_SSN = ' ' THEN DELETE);
%STP4(TOTALS,IF TOT_AMT = 0 THEN DELETE);
PROC SORT DATA=NOPRCTR NODUPKEY;BY BF_SSN LN_SEQ;RUN;
/*========================================================================*/
/*PLUS/PLUSGB PROCESSING*/
/*========================================================================*/
DATA DSBS (KEEP=BF_SSN DM_PRS_LST LN_SEQ);
SET DSBSB DSBSA;
RUN;
PROC SORT DATA=DSBS NODUPKEY;
BY BF_SSN LN_SEQ;
RUN;
/*========================================================================*/
/*CREATE FILES*/
/*========================================================================*/
%MACRO NOOBS_REP(DS,RNO);
PROC CONTENTS DATA=&DS OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 126*'-';
	PUT      /////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
		@57 '-- END OF REPORT --';
	PUT ////////////
		@46 "JOB = UTLWO08     REPORT = ULWO08.LWO08R&RNO";
	END;
RETURN;
RUN;
%MEND NOOBS_REP;
DATA _NULL_;
SET NOPRCTR;
WHERE CR_AMT NE 0;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT CR_EFF MMDDYY10. CR_AMT 10.2 ;
PUT BF_SSN $ @;
PUT	DM_PRS_LST $ @; 
PUT	LN_SEQ @;
PUT	CR_EFF @;
PUT	CR_AMT;
RUN;

DATA _NULL_;
SET PRCTR;
FILE REPORT4 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT CR_AMT 10.2 ;
PUT BF_SSN $ @;
PUT	LN_SEQ @;
PUT	CR_AMT @;
PUT	IF_TIR_PCE $ @;
PUT OFEE ;
RUN;

PROC PRINTTO PRINT=REPORT3 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 NODATE CENTER PAGENO=1;
TITLE 'ORIGINATION FEE CREDIT POSTING REPORT';
TITLE2 "&RUNDATE - &RUNTIME";
FOOTNOTE 'JOB = UTLWO08     REPORT = ULWO08.LWO08R3';
%NOOBS_REP(TOTALS,3);
PROC PRINT NOOBS SPLIT='/' DATA=TOTALS;
FORMAT TOT_AMT DOLLAR14.2;
VAR TOT_NUM
	TOT_AMT;
LABEL	TOT_NUM = 'TOTAL NUMBER OF LOANS'
		TOT_AMT = 'TOTAL AMOUNT';
RUN;

PROC PRINTTO PRINT=REPORT5 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 NODATE CENTER PAGENO=1;
TITLE '8001 PRECONVERSION FINANCIAL TRANSACTION';
TITLE2 'MANUAL REVIEW';
TITLE3 "&RUNDATE - &RUNTIME";
FOOTNOTE 'JOB = UTLWO08     REPORT = ULWO08.LWO08R5';
%NOOBS_REP(_8001,5);
PROC PRINT NOOBS SPLIT='/' DATA=_8001;
VAR BF_SSN DM_PRS_LST LN_SEQ;
LABEL BF_SSN = 'SSN' DM_PRS_LST = 'LAST NAME' LN_SEQ = 'LOAN SEQ';
RUN;

PROC PRINTTO PRINT=REPORT6 NEW;
RUN;
DATA NOPRCTR_ZERO;
SET NOPRCTR;
WHERE CR_AMT = 0;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 NODATE CENTER PAGENO=1;
TITLE 'ORIGINATION FEE CREDIT - ZERO AMOUNT';
TITLE3 "&RUNDATE - &RUNTIME";
FOOTNOTE 'JOB = UTLWO08     REPORT = ULWO08.LWO08R6';
%NOOBS_REP(NOPRCTR_ZERO,6);
PROC PRINT NOOBS SPLIT='/' DATA=NOPRCTR_ZERO;
WHERE CR_AMT = 0;
VAR BF_SSN DM_PRS_LST LN_SEQ;
LABEL BF_SSN = 'SSN' DM_PRS_LST = 'LAST NAME' LN_SEQ = 'LOAN SEQ';
RUN;

PROC PRINTTO PRINT=REPORT7 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 NODATE CENTER PAGENO=1;
TITLE 'ORIGINATION FEE CREDIT';
TITLE2 'PLUS-PLUSGB LOANS 1ST DISBURSEMENT BETWEEN 07/01/2006 AND 08/07/2006';
TITLE3 "&RUNDATE - &RUNTIME";
FOOTNOTE 'JOB = UTLWO08     REPORT = ULWO08.LWO08R7';
%NOOBS_REP(DSBS,7);
PROC PRINT NOOBS SPLIT='/' DATA=DSBS;
VAR BF_SSN DM_PRS_LST LN_SEQ;
LABEL BF_SSN = 'SSN' DM_PRS_LST = 'LAST NAME' LN_SEQ = 'LOAN SEQ';
RUN;

PROC PRINTTO;
RUN;