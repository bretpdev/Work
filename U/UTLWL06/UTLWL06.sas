/*UTLWL06 - SMALL BALANCE WRITE OFF*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWL06.LWL06RZ";
FILENAME REPORT2 "&RPTLIB/ULWL06.LWL06R2";
FILENAME REPORT3 "&RPTLIB/ULWL06.LWL06R3";
FILENAME REPORT4 "&RPTLIB/ULWL06.LWL06R4";
DATA _NULL_;
	CALL SYMPUT('MNTH_6',"'"||PUT(INTNX('MONTH',TODAY(),-6,'S'), MMDDYYD10.)||"'");
	CALL SYMPUT('DAYS_54',INTNX('DAYS',TODAY(),-54,'B'));
	CALL SYMPUT('DAYS_20',INTNX('DAYS',TODAY(),-20,'B'));
	CALL SYMPUT('DAYS_7',INTNX('DAYS',TODAY(),-7,'B'));
RUN;
%SYSLPUT MNTH_6 = &MNTH_6;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
DATA LOAN_TYPES;
	FORMAT LN_TYP LN_PGM $50.;
	INFILE "/sas/whse/progrevw/GENR_REF_LoanTypes.txt" DLM=',' MISSOVER DSD;
	INFORMAT LN_TYP LN_PGM $50.;
	INPUT LN_TYP LN_PGM ;
	LN_PGM = UPCASE(LN_PGM);
RUN;
/*CREATE MACRO VARIALBE LISTS OF LOAN PROGRAMS(FFEL AND PRIVATE LOANS)*/
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LN_TYP)||"'" 
		INTO :FFELP_LIST SEPARATED BY "," /*FFEL LOAN LIST*/
	FROM LOAN_TYPES
	WHERE LN_PGM = 'FFEL';
QUIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
/************************************************
* LOAN SELECTION
*************************************************/
CREATE TABLE LNLEV AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,A.LN_SEQ
	,COALESCE(A.LA_CUR_PRI,0) AS LA_CUR_PRI
	,COALESCE(B.WA_TOT_BRI_OTS,0) AS WA_TOT_BRI_OTS
	,COALESCE(A.LA_CUR_PRI,0) + COALESCE(B.WA_TOT_BRI_OTS,0) AS TOT
	,A.IC_LON_PGM
	,A.LD_LON_1_DSB
	,C.DF_SPE_ACC_ID
	,D.IF_BND_ISS
	,DELQ.LD_DLQ_MAX 
	,CASE 
		WHEN LN90.CONSOL_IND = 'X' THEN 'CON'
		ELSE 'NON_CON'
	 END AS PIF_REA
	,CASE 
		WHEN A.IC_LON_PGM IN (&FFELP_LIST) THEN 'FFELP'
		ELSE 'NON FFELP'
	 END AS LOAN_CAT
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN (
	SELECT X.BF_SSN
	FROM OLWHRM1.LN10_LON X
	INNER JOIN OLWHRM1.DW01_DW_CLC_CLU Y
		ON X.BF_SSN = Y.BF_SSN
		AND X.LN_SEQ = Y.LN_SEQ
	WHERE COALESCE(X.LA_CUR_PRI,0) + COALESCE(Y.WA_TOT_BRI_OTS,0) BETWEEN .01 AND 49.99
	GROUP BY X.BF_SSN
	) BAL
	ON A.BF_SSN = BAL.BF_SSN
LEFT OUTER JOIN (
	SELECT BF_SSN
		,MAX(LD_DLQ_OCC) AS LD_DLQ_MAX 
	FROM OLWHRM1.LN16_LON_DLQ_HST
	WHERE LC_STA_LON16 = '1'
	GROUP BY BF_SSN
	) DELQ
	ON A.BF_SSN = DELQ.BF_SSN
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,'X' AS CONSOL_IND
	FROM OLWHRM1.LN90_FIN_ATY
	WHERE PC_FAT_TYP = '10'
	AND PC_FAT_SUB_TYP IN ('70','80')
	AND LC_STA_LON90 = 'A'
	AND LC_FAT_REV_REA = ''
	) LN90
	ON A.BF_SSN = LN90.BF_SSN
	AND A.LN_SEQ = LN90.LN_SEQ
INNER JOIN OLWHRM1.PD10_PRS_NME C
	ON A.BF_SSN = C.DF_PRS_ID
INNER JOIN OLWHRM1.LN35_LON_OWN D
	ON A.BF_SSN = D.BF_SSN
	AND A.LN_SEQ = D.LN_SEQ

WHERE A.LC_STA_LON10 = 'R'
AND COALESCE(A.LA_CUR_PRI,0) + COALESCE(B.WA_TOT_BRI_OTS,0) > 0
AND D.LD_OWN_EFF_END IS NULL
AND D.LC_STA_LON35 = 'A'
/*AND A.LA_CUR_PRI <> A.LA_LON_AMT_GTR*/
FOR READ ONLY WITH UR
);
/****************************************
* ADD FINANCIAL TRANSACTION INFO
******************************************/
CREATE TABLE LN90 AS
SELECT DISTINCT LNLEV.*
	,FIN.LN_FAT_SEQ
	,FIN.LD_FAT_EFF
	,FIN.PC_FAT_TYP
	,FIN.PC_FAT_SUB_TYP
	,FIN.LA_FAT_CUR_PRI
	,FIN.LC_FAT_REV_REA
	,FIN.LC_STA_LON90
FROM LNLEV 
LEFT OUTER JOIN CONNECTION TO DB2 (
	SELECT X.BF_SSN
		,X.LN_SEQ
		,X.LN_FAT_SEQ
		,X.LD_FAT_EFF
		,X.PC_FAT_TYP
		,X.PC_FAT_SUB_TYP
		,X.LA_FAT_CUR_PRI
		,X.LC_FAT_REV_REA
		,X.LC_STA_LON90
	FROM OLWHRM1.LN90_FIN_ATY X
	INNER JOIN (
		SELECT BF_SSN
			,MAX(LD_FAT_EFF) AS LD_FAT_EFF
		FROM OLWHRM1.LN90_FIN_ATY 
		WHERE LC_FAT_REV_REA = ''
		AND LC_STA_LON90 = 'A'	
		GROUP BY BF_SSN
		) Y
		ON X.BF_SSN = Y.BF_SSN	
		AND X.LD_FAT_EFF = Y.LD_FAT_EFF
		WHERE X.LC_FAT_REV_REA = ''
		AND X.LC_STA_LON90 = 'A'
	FOR READ ONLY WITH UR
	) FIN
	ON FIN.BF_SSN = LNLEV.BF_SSN
	AND FIN.LN_SEQ = LNLEV.LN_SEQ
;
/***************************************
* CREATE EXCLUSION DATA SET
****************************************/
CREATE TABLE BRXOUT AS
SELECT *
FROM CONNECTION TO DB2 (
	SELECT DISTINCT BF_SSN
	FROM OLWHRM1.DW01_DW_CLC_CLU 
	WHERE WC_DW_LON_STA = '23'
UNION
	SELECT DISTINCT DF_PRS_ID_BR AS BF_SSN
	FROM OLWHRM1.CT30_CALL_QUE
	WHERE IF_WRK_GRP = 'CNSLNRVW'
UNION 
	SELECT DISTINCT BF_SSN
	FROM OLWHRM1.AY10_BR_LON_ATY
	WHERE PF_REQ_ACT = 'OC312'
	AND LD_ATY_REQ_RCV >= &MNTH_6
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
PROC SQL;
CREATE TABLE LOANS AS 
SELECT DISTINCT A.*
FROM LNLEV A
WHERE A.BF_SSN NOT IN (
	SELECT DISTINCT BF_SSN
	FROM BRXOUT 
	);
QUIT;
ENDRSUBMIT;
DATA LOANS;SET WORKLOCL.LOANS;RUN;
DATA LN90;SET WORKLOCL.LN90;RUN;

/***************************************
* CREATE INDICATORS FOR FILE PROCESSING
****************************************/
PROC SQL;
CREATE TABLE PCRT AS 
SELECT DISTINCT A.*
	,B.BOR_TOT
	,C._1040
	,D._20FIN
	,E._54FIN
	,F._53DEL
	,G._07FIN
	,COALESCE(H.BOR_TOT_NOCON,0) AS BOR_TOT_NOCON
	,I.FFELP_TOT
FROM LOANS A
LEFT OUTER JOIN (
	SELECT BF_SSN
		,SUM(TOT) AS BOR_TOT
	FROM LOANS
	GROUP BY BF_SSN
	) B
	ON A.BF_SSN = B.BF_SSN
LEFT OUTER JOIN (
	SELECT BF_SSN
		,'X' AS _1040
	FROM LN90
	WHERE PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP = '40'
	) C
	ON A.BF_SSN = C.BF_SSN
LEFT OUTER JOIN (
	SELECT BF_SSN
		,'X' AS _20FIN
	FROM LN90
	WHERE LD_FAT_EFF >= &DAYS_20
	) D
	ON A.BF_SSN = D.BF_SSN
LEFT OUTER JOIN (
	SELECT BF_SSN
		,'X' AS _54FIN
	FROM LN90
	WHERE LD_FAT_EFF >= &DAYS_54 
	) E
	ON A.BF_SSN = E.BF_SSN
LEFT OUTER JOIN (
	SELECT BF_SSN
		,'X' AS _53DEL
	FROM LOANS
	WHERE LD_DLQ_MAX > &DAYS_54 
	) F
	ON A.BF_SSN = F.BF_SSN
LEFT OUTER JOIN (
	SELECT BF_SSN
		,'X' AS _07FIN
	FROM LN90
	WHERE LD_FAT_EFF > &DAYS_7 
	) G
	ON A.BF_SSN = G.BF_SSN
LEFT OUTER JOIN (
	SELECT BF_SSN
		,SUM(TOT) AS BOR_TOT_NOCON
	FROM LOANS
	WHERE PIF_REA = 'CON'
	AND NOT 
		(
			IC_LON_PGM IN ('SUBSPC','UNSPC','SUBCNS','UNCNS','CNSLDN') 
			AND TOT > 10
		)
	GROUP BY BF_SSN	
	) H
	ON A.BF_SSN = H.BF_SSN
LEFT OUTER JOIN (
	SELECT BF_SSN
		,SUM(TOT) AS FFELP_TOT
	FROM LOANS
	WHERE LOAN_CAT = 'FFELP'
	GROUP BY BF_SSN
	) I
	ON A.BF_SSN = I.BF_SSN
;
QUIT;
/*******************************************
* PROCESS FILES AND DETERMINE FILE OUTPUT
*********************************************/
DATA R2 R3 R4;
SET PCRT;
IF PIF_REA = 'NON_CON' AND 
	(
			(
				BOR_TOT > 50 
			) 
		OR
			(
				10 < FFELP_TOT < 50 
				AND				
				( 
					_53DEL = 'X' OR _54FIN = 'X'
				)
			) 
		OR
			(
				BOR_TOT <= 10 AND
				_20FIN = 'X'
			) 
	) THEN DELETE;

ELSE IF PIF_REA = 'CON' AND 
	(
		BOR_TOT_NOCON > 10 OR
		BOR_TOT_NOCON = 0 OR  
		_07FIN = 'X'			
	) THEN DELETE;
ELSE IF _1040 = 'X' THEN OUTPUT R3;
ELSE IF PIF_REA = 'NON_CON' THEN OUTPUT R2;
ELSE IF PIF_REA = 'CON' THEN OUTPUT R4;
RUN;
/*********************************
* REPORT AND FILE CREATION
**********************************/
DATA _NULL_;
SET R2;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT BF_SSN $9. ;
   FORMAT LN_SEQ 6. ;
   FORMAT LD_LON_1_DSB MMDDYY10. ;
DO;
   PUT BF_SSN $ @;
   PUT LN_SEQ @;
   PUT LD_LON_1_DSB @;
END;
RUN;

PROC PRINTTO PRINT=REPORT3 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 CENTER;
TITLE 'SMALL BALANCE W/ REFUND TRANS';
FOOTNOTE 'JOB = UTLWL06  	 REPORT = ULWL06.LWL06R3';
PROC CONTENTS DATA=R3 OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 127*'-';
	PUT      ////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@57 '-- END OF REPORT --';
	PUT //////////
		@46 "JOB = UTLWL06  	 REPORT = ULWL06.LWL06R3";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=R3 WIDTH=UNIFORM WIDTH=MIN;
VAR BF_SSN LN_SEQ IC_LON_PGM LD_LON_1_DSB;
FORMAT LD_LON_1_DSB MMDDYY10.;
LABEL BF_SSN = 'SSN' 
	LN_SEQ = 'LOAN SEQ'
	IC_LON_PGM = 'LOAN TYPE'
	LD_LON_1_DSB = '1ST DISB DATE';
RUN;
PROC PRINTTO;
RUN;
DATA _NULL_;
SET  R4;
FILE REPORT4 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT BF_SSN $9. ;
   FORMAT LN_SEQ 6. ;
   FORMAT LD_LON_1_DSB MMDDYY10. ;
DO;
   PUT BF_SSN $ @;
   PUT LN_SEQ @;
   PUT LD_LON_1_DSB @;
END;
RUN;