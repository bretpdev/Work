*------------------------------------------------------------*
| UTLWR09 - LARS COMPARISON TO FACT REPORT - INELIGIBLE LOAN |
*------------------------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWR09.LWR09RZ";
FILENAME REPORT2 "&RPTLIB/ULWR09.LWR09R2";
FILENAME REPORT3 "&RPTLIB/ULWR09.LWR09R3";
FILENAME REPORT4 "&RPTLIB/ULWR09.LWR09R4";
FILENAME REPORT5 "&RPTLIB/ULWR09.LWR09R5";
FILENAME REPORT6 "&RPTLIB/ULWR09.LWR09R6";
FILENAME REPORT7 "&RPTLIB/ULWR09.LWR09R7";
FILENAME REPORT8 "&RPTLIB/ULWR09.LWR09R8";
FILENAME REPORT9 "&RPTLIB/ULWR09.LWR09R9";
FILENAME REPORT10 "&RPTLIB/ULWR09.LWR09R10";
FILENAME REPORT11 "&RPTLIB/ULWR09.LWR09R11";
FILENAME REPORT12 "&RPTLIB/ULWR09.LWR09R12";
FILENAME REPORT13 "&RPTLIB/ULWR09.LWR09R13";
FILENAME REPORT14 "&RPTLIB/ULWR09.LWR09R14";
FILENAME REPORT15 "&RPTLIB/ULWR09.LWR09R15";
FILENAME REPORT16 "&RPTLIB/ULWR09.LWR09R16";
FILENAME REPORT17 "&RPTLIB/ULWR09.LWR09R17";
FILENAME REPORT18 "&RPTLIB/ULWR09.LWR09R18";
FILENAME REPORT19 "&RPTLIB/ULWR09.LWR09R19";
FILENAME REPORT20 "&RPTLIB/ULWR09.LWR09R20";
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
DATA _NULL_ ;
	CALL SYMPUT('MONTHNUM',MONTH(TODAY()));
RUN;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE LCTFR AS
SELECT *
FROM CONNECTION TO DB2 (
	SELECT A.BF_SSN
		,A.LN_SEQ
		,A.IC_LON_PGM
		,TRIM(A.WM_BR_LST) AS WM_BR_LST
		,CASE
			WHEN A.WC_LON_STA IN ('06') THEN 'UNINSURED'
			ELSE 'INELIGIBLE'
		END AS STATUS
		,CASE /*THIS JOB WILL USUSALLY BE RUN THE FIRST MONTH AFTER END OF QUARTER */
			WHEN &MONTHNUM IN (1,4,7,10) THEN A.WA_CUR_PRI /*DURING FIRST MONTH AFTER END OF QUARTER*/
			WHEN &MONTHNUM IN (2,5,8,11) THEN A.WA_PRV_MTH_PRI /*ONLY WORKS 2ND MONTH AFTER EOQ */
			ELSE A.WA_CUR_PRI  /*JUST IN CASE IT IS EVER RUN WHEN IT SHOULDN'T BE*/
		END WA_PRV_MTH_PRI
		,B.BAL_MR84 AS PRI_BAL_799
		,A.WA_PRV_MTH_PRI - B.BAL_MR84 AS VARIANCE
				,C.LF_LON_CUR_OWN
				,D.DF_SPE_ACC_ID
				,E.IM_LDR_FUL
	FROM OLWHRM1.MR5A_MR_LON_MTH_01 A 
	INNER JOIN (SELECT MR84.BF_SSN AS SSN_MR84,
				MR84.LN_SEQ AS SEQ_MR84,
				MR84.IF_BND_ISS AS BOND_MR84,
				MR84.IF_OWN,
				SUM(MR84.WA_PRI_END_BAL) AS BAL_MR84
				FROM OLWHRM1.MR84_MR_799_CAL MR84
				GROUP BY MR84.BF_SSN
					,MR84.LN_SEQ
					,MR84.IF_BND_ISS
					,MR84.IF_OWN) B
		ON A.BF_SSN = B.SSN_MR84 
		AND A.LN_SEQ = B.SEQ_MR84 
		AND A.IF_BND_ISS = B.BOND_MR84
		AND A.IF_OWN = B.IF_OWN
	INNER JOIN OLWHRM1.LN10_LON C
		ON A.BF_SSN = C.BF_SSN
		AND A.LN_SEQ = C.LN_SEQ
	INNER JOIN OLWHRM1.PD10_PRS_NME D
		ON A.BF_SSN = D.DF_PRS_ID
	INNER JOIN OLWHRM1.LR10_LDR_DMO E
		ON C.LF_LON_CUR_OWN = E.IF_DOE_LDR
WHERE A.WA_PRV_MTH_PRI >= 0 
	AND (((A.WA_PRV_MTH_PRI != B.BAL_MR84) AND &MONTHNUM IN (2,5,8,11))
		OR (A.WA_CUR_PRI != B.BAL_MR84) AND &MONTHNUM NOT IN (2,5,8,11))
/*This job uses MR tables.  This value will should be blank in the first month after the quarter*/
/*for the rows we want.  Otherwise, do not use it as a filter*/
	AND (A.WC_REA_ZRO_BAL = '' OR &MONTHNUM NOT IN (1,4,7,10))
	AND A.BF_SSN IN ('086709157','458575213','525395850','528110688','528113840','528989037','529152560','529176422','529479226','529751046','539763580','554797662','560637260','646366453','182623938')
ORDER BY
	A.WC_LON_STA DESC,
	A.BF_SSN,
	A.LN_SEQ
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWR09.LWR09RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA LCTFR;
	SET WORKLOCL.LCTFR;
RUN;
PROC SORT DATA=LCTFR;
BY DF_SPE_ACC_ID LN_SEQ;
RUN;
/*/*/*/*FOR ALL REPORTS EXCEPT REPORT 5*/*/*/*/	;
%MACRO PRINT_IT(RNO,LID);
DATA PREP;
	SET LCTFR;
	WHERE LF_LON_CUR_OWN IN (&LID);
	VARIANCE = WA_PRV_MTH_PRI - PRI_BAL_799;
RUN;

PROC SORT DATA=PREP; 
	BY IC_LON_PGM BF_SSN LN_SEQ; 
RUN;

PROC PRINTTO PRINT=REPORT&RNO NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 CENTER DATE;
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	'Please take appropriate precautions to safeguard this information.';
FOOTNOTE3	;
FOOTNOTE4   "JOB = UTLWR09  	 REPORT = ULWR09.LWR09R&RNO";

PROC CONTENTS DATA=PREP OUT=EMPTYSET NOPRINT;RUN;
TITLE 'LARS COMPARISON FACT REPORT - INELIGIBLE LOANS';
TITLE2	"LENDER ID: &LID";
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 126*'-';
	PUT      //////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@57 '-- END OF REPORT --';
	PUT //////////////
		@46 "JOB = UTLWR09  	 REPORT = ULWR09.LWR09&RNO";
	END;
RETURN;
RUN;
DATA _NULL_;
	SET EMPTYSET;
	CALL SYMPUT('POBS',NOBS);
RUN;
%PUT &POBS;
%IF &POBS EQ 0 %THEN %DO;
	DATA _NULL_;
		CALL SYMPUT('LNM','');
	RUN;
%END;
%ELSE %DO;
	DATA _NULL_;
	SET PREP;
		CALL SYMPUT('LNM',TRIM(LEFT(IM_LDR_FUL)));
	RUN;
%END;
TITLE 'LARS COMPARISON FACT REPORT - INELIGIBLE LOANS';
TITLE2	"&LNM";
PROC PRINT NOOBS SPLIT='/' DATA=PREP WIDTH=UNIFORM WIDTH=MIN;
VAR DF_SPE_ACC_ID
	LN_SEQ
	WM_BR_LST
	IC_LON_PGM
	STATUS
	WA_PRV_MTH_PRI
	PRI_BAL_799
	VARIANCE
	LF_LON_CUR_OWN
	IM_LDR_FUL
	;
FORMAT WA_PRV_MTH_PRI PRI_BAL_799 VARIANCE COMMA10.2;

LABEL DF_SPE_ACC_ID = 'ACCOUNT NUMBER'
	LN_SEQ = 'LOAN SEQ'
	WM_BR_LST = 'LAST NAME'
	IC_LON_PGM = 'LOAN TYPE'
	WA_PRV_MTH_PRI = 'PCR BALANCE'
	PRI_BAL_799 = '799 PRINCIPAL BALANCE'
	VARIANCE = 'VARIANCE'
	LF_LON_CUR_OWN = 'LENDER ID'
	IM_LDR_FUL = 'LENDER NAME'
	;
RUN;

PROC PRINTTO;
RUN;
%MEND PRINT_IT;

/*%PRINT_IT(2,'817575');*/
/*%PRINT_IT(3,'822373');*/
/*%PRINT_IT(4,'835577');*/
/*%PRINT_IT(6,'833828'); */
/*%PRINT_IT(7,'817440');*/
/*%PRINT_IT(8,'817545');*/
/*%PRINT_IT(9,'830791');*/
/*%PRINT_IT(10,'817546');*/
/*%PRINT_IT(11,'820200'); */
/*%PRINT_IT(12,'830132'); */
/*%PRINT_IT(13,'829123'); */
/*%PRINT_IT(14,'811698'); */
/*%PRINT_IT(15,'830146'); */
/*%PRINT_IT(16,'829158'); */
/*%PRINT_IT(19,'834437');*/
/*%PRINT_IT(20,'834493');*/


/*/*/*REPORT 5 PROCESSING BELOW*/*/*/ ;
DATA PREP;
	SET LCTFR;
	WHERE LF_LON_CUR_OWN IN ('828476','82847601');
	VARIANCE = WA_PRV_MTH_PRI - PRI_BAL_799;
RUN;
PROC SORT DATA=PREP; 
	BY IC_LON_PGM BF_SSN LN_SEQ ; 
RUN;

PROC PRINTTO PRINT=REPORT5 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 CENTER DATE;
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	'Please take appropriate precautions to safeguard this information.';
FOOTNOTE3	;
FOOTNOTE4   "JOB = UTLWR09  	 REPORT = ULWR09.LWR09R5";
TITLE 'LARS COMPARISON FACT REPORT - INELIGIBLE LOANS';
TITLE2	"LENDER ID: 828476, 82847601";

PROC PRINT NOOBS SPLIT='/' DATA=PREP WIDTH=UNIFORM WIDTH=MIN;
VAR BF_SSN
	LN_SEQ
	WM_BR_LST
	IC_LON_PGM
	STATUS
	WA_PRV_MTH_PRI
	PRI_BAL_799
	VARIANCE
	LF_LON_CUR_OWN
	IM_LDR_FUL
	;
SUM VARIANCE;
FORMAT WA_PRV_MTH_PRI PRI_BAL_799 VARIANCE COMMA10.2;
LABEL BF_SSN = 'ACCOUNT NUMBER'
	LN_SEQ = 'LOAN SEQ'
	WM_BR_LST = 'LAST NAME'
	IC_LON_PGM = 'LOAN TYPE'
	WA_PRV_MTH_PRI = 'PCR BALANCE'
	PRI_BAL_799 = '799 PRINCIPAL BALANCE'
	VARIANCE = 'VARIANCE'
	LF_LON_CUR_OWN = 'LENDER ID'
	IM_LDR_FUL = 'LENDER NAME'
	;
RUN;

PROC PRINTTO;
RUN;