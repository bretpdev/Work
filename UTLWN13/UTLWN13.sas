*------------------------------------------------------*
| UTLWN13 - Loans Guaranteed, Disbursed and Sold Report|
*------------------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWN13.LWN13R2";
FILENAME REPORTZ "&RPTLIB/ULWN13.LWN13RZ";
DATA _NULL_;
	CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'END'), MMDDYYD10.)||"'");
	CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;
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
CREATE TABLE LGDSR AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,A.LN_SEQ
	,A.IC_LON_PGM
	,A.LD_LON_1_DSB
	,A.IF_DOE_LDR
	,A.LF_LON_CUR_OWN 
	,A.IF_TIR_PCE
	,INT(A.LF_RGL_CAT_LP20) AS LF_RGL_CAT_LP20
	,B.SALE_DT
	,B.AMT_SLD
	,F.AA_GTR_LON
	,F.AD_GTR_ORG
	,F.AF_DOE_LDR
	,E.LD_DSB
	,E.LC_DSB_TYP
	,G.DISB_CAN_DT
	,COALESCE(SUM(PMNT.LA_FAT_CUR_PRI),0) AS BOR_PMT
	,COALESCE(SUM(OTHR.LA_FAT_CUR_PRI),0) AS BOR_OTH
FROM OLWHRM1.LN10_LON A
INNER JOIN (
	SELECT L15A.AF_APL_ID
		,L15A.BF_SSN
		,L15A.LN_SEQ
		,L15A.LN_BR_DSB_SEQ
		,L15A.LC_DSB_TYP
		,L15A.LD_DSB
	FROM OLWHRM1.LN15_DSB L15A
	INNER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,MAX(LD_DSB) AS LD_DSB
		FROM OLWHRM1.LN15_DSB
		WHERE LC_STA_LON15 IN ('1','3')
		GROUP BY BF_SSN
			,LN_SEQ
		) L15B
		ON L15A.BF_SSN = L15B.BF_SSN
		AND L15A.LN_SEQ = L15B.LN_SEQ
		AND L15A.LD_DSB = L15B.LD_DSB
	WHERE L15A.LC_STA_LON15 IN ('1','3')
	) E
	ON A.BF_SSN = E.BF_SSN
	AND A.LN_SEQ = E.LN_SEQ
INNER JOIN OLWHRM1.AP03_MASTER_APL F
	ON E.AF_APL_ID = F.AF_APL_ID
INNER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,LN_FAT_SEQ
		,LD_FAT_EFF AS SALE_DT
		,ABS(SUM(LA_FAT_CUR_PRI)) AS AMT_SLD
	FROM OLWHRM1.LN90_FIN_ATY 
	WHERE PC_FAT_TYP IN ('03','04')
		AND PC_FAT_SUB_TYP = '95'
		AND LD_FAT_EFF BETWEEN &BEGIN AND &END
	GROUP BY BF_SSN
		,LN_SEQ
		,LN_FAT_SEQ
		,LD_FAT_EFF 
	) B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.LN99_LON_SLE_FAT C
	ON B.BF_SSN = C.BF_SSN
	AND B.LN_SEQ = C.LN_SEQ
	AND B.LN_FAT_SEQ = C.LN_FAT_SEQ
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,LD_FAT_PST
		,ABS(LA_FAT_CUR_PRI) AS LA_FAT_CUR_PRI
	FROM OLWHRM1.LN90_FIN_ATY 
	WHERE PC_FAT_TYP = '10' 
		AND PC_FAT_SUB_TYP IN 
		(
			'10','11','12','20',
			'21','35','36','37',
			'38','50','70','80'
		)
	) PMNT
	ON A.BF_SSN = PMNT.BF_SSN
	AND A.LN_SEQ = PMNT.LN_SEQ
	AND PMNT.LD_FAT_PST < B.SALE_DT
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,LD_FAT_PST
		,ABS(LA_FAT_CUR_PRI) AS LA_FAT_CUR_PRI
	FROM OLWHRM1.LN90_FIN_ATY 
	WHERE PC_FAT_SUB_TYP ='01'
		AND PC_FAT_TYP IN 
		(		
			'26','70'
		)
	) OTHR
	ON A.BF_SSN = OTHR.BF_SSN
	AND A.LN_SEQ = OTHR.LN_SEQ
	AND OTHR.LD_FAT_PST < B.SALE_DT
LEFT OUTER JOIN (
		SELECT A.BF_SSN
			,A.LN_SEQ
			,A.LN_BR_DSB_SEQ
			,A.LD_DSB_CAN AS DISB_CAN_DT
		FROM OLWHRM1.LN15_DSB A
		INNER JOIN OLWHRM1.LN93_DSB_FIN_TRX B
			ON A.BF_SSN = B.BF_SSN
			AND A.LN_BR_DSB_SEQ = B.LN_BR_DSB_SEQ
		INNER JOIN OLWHRM1.LN90_FIN_ATY C
			ON B.BF_SSN = C.BF_SSN
			AND B.LN_SEQ = C.LN_SEQ
			AND B.LN_FAT_SEQ = C.LN_FAT_SEQ
		WHERE A.LA_DSB = A.LA_DSB_CAN 
			AND C.PC_FAT_TYP = '10' 
			AND A.LC_DSB_TYP = '2' 
			AND 
			(
				(
					A.LD_DSB_CAN = C.LD_FAT_EFF AND 
					C.PC_FAT_SUB_TYP IN ('40','41')
				)
			OR 
				(
					A.LD_DSB_CAN = C.LD_FAT_PST AND 
					C.PC_FAT_SUB_TYP = '45'
				)
			)
	UNION
		SELECT BF_SSN
			,LN_SEQ
			,LN_BR_DSB_SEQ
			,LD_DSB_CAN AS DISB_CAN_DT
		FROM OLWHRM1.LN15_DSB 
		WHERE LC_DSB_TYP = '2'
			AND LD_DSB_CAN IS NOT NULL
	) G
	ON E.BF_SSN = G.BF_SSN
	AND E.LN_SEQ = G.LN_SEQ
	AND E.LN_BR_DSB_SEQ = G.LN_BR_DSB_SEQ
	AND G.DISB_CAN_DT < B.SALE_DT

WHERE C.IF_SLL_OWN_SLD = '819628'
	AND A.IC_LON_PGM IN 
	(
		'STFFRD','PLUS','PLUSGB','UNSTFD'
	)
GROUP BY A.BF_SSN
	,A.LN_SEQ
	,A.IC_LON_PGM
	,A.LD_LON_1_DSB
	,A.LF_LON_CUR_OWN 
	,A.IF_TIR_PCE
	,A.IF_DOE_LDR
	,INT(A.LF_RGL_CAT_LP20) 
	,B.SALE_DT
	,B.AMT_SLD
	,F.AA_GTR_LON
	,F.AD_GTR_ORG
	,F.AF_DOE_LDR
	,E.LD_DSB
	,E.LC_DSB_TYP
	,G.DISB_CAN_DT
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWN13.LWN13RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA LGDSR;
	SET WORKLOCL.LGDSR;
RUN;
/*CALCULATE ORIGINATION FEE %*/
DATA LGDSR;
	SET LGDSR;
	LENGTH OFD $ 5.;
	IF LF_RGL_CAT_LP20 <= 1999030 
		THEN DO;
			OFP = .03;
			OFD = '3%';
		END;
	ELSE IF LF_RGL_CAT_LP20 = 2006020 
		THEN DO;
			IF IC_LON_PGM IN ('PLUS','PLUSGB') AND '01JUL2006'D <= LD_LON_1_DSB <='07AUG2006'D 
				THEN DO;
					OFP = .02;
					OFD = '2%';
				END;
			ELSE DO;
				OFP = .03;
				OFD = '3%';
			END;
		END;
	ELSE IF LF_RGL_CAT_LP20 = 2007020 
		THEN DO;
			OFP = .015;
			OFD = '1.5%';
		END;
RUN;
PROC SORT DATA=LGDSR NODUPKEY;
	BY BF_SSN LN_SEQ ;
RUN;
/*CALULATE NECESSARY VALUES & CATEGORIES*/
DATA LGDSR;
	SET LGDSR;
	PRIN_AMT = AMT_SLD + BOR_PMT - BOR_OTH;
	OFEE_AMT = ROUND(PRIN_AMT*OFP,.01);
	SELECT (IF_TIR_PCE);
		WHEN ('')LCAT = 'LOAN WITH ZERO ORIGINATION FEES';
		OTHERWISE LCAT = 'LOAN WITH ORIGINATION FEES';
	END;
RUN;
/*PRINT REPORT*/
PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 DATE PAGENO=1 CENTER;
TITLE 'CHASE LOANS SOLD WITHIN PREVIOUS MONTH';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	'Please take appropriate precautions to safeguard this information.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTLWN13  	 REPORT = ULWN13.LWN13R2';
PROC REPORT DATA=LGDSR NOWD SPLIT='/' HEADSKIP SPACING=1;
	COLUMN BF_SSN IF_DOE_LDR IC_LON_PGM AA_GTR_LON AD_GTR_ORG LD_DSB LC_DSB_TYP 
		SALE_DT DISB_CAN_DT PRIN_AMT AMT_SLD OFD OFEE_AMT;
	DEFINE BF_SSN / DISPLAY 'SSN' WIDTH=9 CENTER;
	DEFINE IF_DOE_LDR  / DISPLAY 'LENDER' WIDTH=6 CENTER;
	DEFINE IC_LON_PGM  / DISPLAY 'LOAN/TYPE' WIDTH=6 CENTER;
	DEFINE AA_GTR_LON  / DISPLAY 'LOAN/AMOUNT' WIDTH=10 FORMAT=8.2 CENTER;
	DEFINE AD_GTR_ORG  / DISPLAY 'GUARANTEE/DATE' WIDTH=10 FORMAT=MMDDYY10. CENTER;
	DEFINE LD_DSB  / DISPLAY 'LAST/DISB/DATE'  WIDTH=10 FORMAT=MMDDYY10. CENTER;
	DEFINE LC_DSB_TYP  / DISPLAY 'DISB/STATUS/OF/LAST/DISB' WIDTH=6 CENTER;
	DEFINE SALE_DT  / DISPLAY 'SALE DATE' WIDTH=10 FORMAT=MMDDYY10. CENTER;
	DEFINE DISB_CAN_DT  / DISPLAY 'CANCELLATION/DATE/OF/LAST/DISB/' WIDTH=12 FORMAT=MMDDYY10. CENTER;
	DEFINE PRIN_AMT  / DISPLAY 'PRINCIPAL/AMOUNT/ELIGIBLE/FOR/OF' WIDTH=10 FORMAT=10.2 CENTER;
	DEFINE AMT_SLD  / DISPLAY 'PRINCIPAL/AMOUNT/SOLD' WIDTH=10 FORMAT=10.2 CENTER;
	DEFINE OFD  / DISPLAY 'ORIG/FEE' WIDTH=4 CENTER;
	DEFINE OFEE_AMT / DISPLAY 'ORIG/FEE/AMOUNT' WIDTH=10 FORMAT=10.2 CENTER;
RUN;
PROC PRINTTO;
RUN;