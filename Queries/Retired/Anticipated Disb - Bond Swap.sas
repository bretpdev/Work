*------------------------------*
| Anticipated Disb - Bond Swap |
*------------------------------*;
%LET FINANCE_BOND = 00000UBS;
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/Anticipated Disb - Bond Swap.txt";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
%SYSLPUT FINANCE_BOND = &FINANCE_BOND;
RSUBMIT;
DATA LOAN_TYPES; /*INPUT ALL LOAN TYPES FROM STORED LIST ON DUSTER*/
	FORMAT LN_TYP LN_PGM $50.;
	INFILE "/sas/whse/progrevw/GENR_REF_LoanTypes.txt" DLM=',' MISSOVER DSD;
	INFORMAT LN_TYP LN_PGM $50.;
	INPUT LN_TYP LN_PGM ;
	LN_PGM = UPCASE(LN_PGM);
RUN;
PROC SQL NOPRINT;/*CREATE LIST FOR FFEL*/
	SELECT "'"||TRIM(LN_TYP)||"'" 
		INTO :FFELP_LIST SEPARATED BY "," 
	FROM LOAN_TYPES
	WHERE LN_PGM = 'FFEL';
QUIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE ADBS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.BF_SSN
	,A.LN_BR_DSB_SEQ
	,A.LN_SEQ
	,A.AF_APL_ID
	,B.AF_CNL||B.AF_CNL_SFX AS CLUID
	,B.AF_LDR_BND_ISS
	,C.IF_BND_ISS
	,D.DF_SPE_ACC_ID

FROM OLWHRM1.LN15_DSB A
INNER JOIN OLWHRM1.AP03_MASTER_APL B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.LN35_LON_OWN C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ
INNER JOIN OLWHRM1.PD10_PRS_NME D
	ON C.BF_SSN = D.DF_PRS_ID
WHERE A.LC_DSB_TYP = '1'
	AND A.LC_STA_LON15 IN ('1','3')
	AND A.LA_DSB != COALESCE(A.LA_DSB_CAN,0)
	AND A.LD_DSB_ROS_PRT IS NULL
	AND A.IC_LON_PGM IN (&FFELP_LIST)
	AND C.LC_STA_LON35 = 'A'
	AND C.LD_OWN_EFF_END IS NOT NULL
	AND B.AF_LDR_BND_ISS != C.IF_BND_ISS
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
PROC SORT DATA=ADBS(WHERE=(INDEX(CLUID,'REALLO') = 0 AND IF_BND_ISS = "&FINANCE_BOND")) NODUPKEY;
	BY DF_SPE_ACC_ID LN_SEQ;
RUN;
ENDRSUBMIT;
DATA ADBS;
	SET WORKLOCL.ADBS;
RUN;
DATA _NULL_;
	SET WORK.ADBS;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	   FORMAT DF_SPE_ACC_ID $10. ;
	   FORMAT LN_SEQ 6. ;
	   FORMAT AF_APL_ID $9. ;
	   FORMAT IF_BND_ISS $8. ;
	   FORMAT DISB_AMT DOLLAR8.2 ;
	   FORMAT LD_DSB MMDDYY10. ;
	IF _N_ = 1 THEN DO;
	   PUT "DF_SPE_ACC_ID,AF_APL_ID,LN_SEQ,IF_BND_ISS";
	END;
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT AF_APL_ID $ @;
		PUT LN_SEQ @;
		PUT IF_BND_ISS @;
	END;
RUN;
