/*UTLWG57 - Non-Disbursed Consolidation Loan with no Matching Grace End Date*/
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORTZ "&RPTLIB/ULWG57.LWG57RZ";
FILENAME REPORT2 "&RPTLIB/ULWG57.LWG57R2";
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
%macro sqlcheck ;
  %if  &sqlxrc ne 0  %then  %do  ;
    data _null_  ;
            file reportz notitles  ;
            put @01 " ********************************************************************* "
              / @01 " ****  The SQL code above has experienced an error.               **** "
              / @01 " ****  The SAS should be reviewed.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  The SQL error code is  &sqlxrc  and the SQL error message  **** "
              / @01 " ****  &sqlxmsg   **** "
              / @01 " ********************************************************************* "
            ;
         run  ;
  %end  ;
%mend  ;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE BWRGRCEND AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		B.BF_SSN
		,A.DM_LCO_PRS_LST AS DM_PRS_LST
		,D.DF_LCO_PRS_SSN_CBR
		,E.DM_LCO_PRS_LST			AS DM_CPRS_LST
		,B.LN_SEQ
		,B.LD_END_GRC_PRD
		,D.BD_LCO_BR_GRC_END	AS LCO_GRC_END
		,'B'					AS PRS_TYP
		,D.AC_LCO_ACC_STA
		,A.DF_SPE_ACC_ID
FROM	OLWHRM1.PD6A_LCO_PRS_DMO A
		INNER JOIN OLWHRM1.LN10_LON B
			ON A.DF_LCO_PRS_SSN = B.BF_SSN
		INNER JOIN (
				SELECT	DF_LCO_PRS_SSN_BR
						,SUBSTR(LF_UND_LN_ACC_NUM,8,2) AS LF_UND_LN_ACC_NUM
						,AN_LCO_APL_SEQ
				FROM	OLWHRM1.LC10_UND_LN_INF 
				WHERE	SUBSTR(LF_UND_LN_ACC_NUM,1,4) = 'L0UT'
						AND LI_UND_LN_CON = 'Y'
						AND LC_UND_LN_PGM IN ('STFFRD','UNSTFD')) C 
			ON B.BF_SSN = C.DF_LCO_PRS_SSN_BR
			AND B.LN_SEQ = CAST(C.LF_UND_LN_ACC_NUM AS INTEGER)
		INNER JOIN OLWHRM1.AP1A_LCO_APL D
			ON B.BF_SSN = D.DF_LCO_PRS_SSN_BR
			AND C.AN_LCO_APL_SEQ = D.AN_LCO_APL_SEQ
			AND D.AD_LCO_APL_DSB IS NULL
			AND D.BD_LCO_BR_GRC_END IS NOT NULL
			AND (D.AC_LCO_ACC_STA IS NULL
			OR D.AC_LCO_ACC_STA = '')
		LEFT OUTER JOIN OLWHRM1.PD6A_LCO_PRS_DMO E
			ON D.DF_LCO_PRS_SSN_CBR = E.DF_LCO_PRS_SSN
FOR READ ONLY WITH UR
);

CREATE TABLE CBWRGRCEND AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		D.DF_LCO_PRS_SSN_BR		AS BF_SSN
		,E.DM_LCO_PRS_LST AS DM_PRS_LST
		,D.DF_LCO_PRS_SSN_CBR
		,A.DM_LCO_PRS_LST			AS DM_CPRS_LST
		,B.LN_SEQ
		,B.LD_END_GRC_PRD
		,D.BD_LCO_BR_GRC_END	AS LCO_GRC_END
		,'C'					AS PRS_TYP
		,D.AC_LCO_ACC_STA
FROM	OLWHRM1.PD6A_LCO_PRS_DMO A
		INNER JOIN OLWHRM1.LN10_LON B
			ON A.DF_LCO_PRS_SSN = B.BF_SSN
		INNER JOIN (
				SELECT	DF_LCO_PRS_SSN_CBR
						,SUBSTR(LF_UND_LN_ACC_NUM,8,2) AS LF_UND_LN_ACC_NUM
						,AN_LCO_APL_SEQ
				FROM	OLWHRM1.LC10_UND_LN_INF 
				WHERE	SUBSTR(LF_UND_LN_ACC_NUM,1,4) = 'L0UT'
						AND LI_UND_LN_CON = 'Y'
						AND LC_UND_LN_PGM IN ('STFFRD','UNSTFD')) C
			ON B.BF_SSN = C.DF_LCO_PRS_SSN_CBR
			AND B.LN_SEQ = CAST(C.LF_UND_LN_ACC_NUM AS INTEGER)
		INNER JOIN OLWHRM1.AP1A_LCO_APL D
			ON B.BF_SSN = D.DF_LCO_PRS_SSN_CBR
			AND C.AN_LCO_APL_SEQ = D.AN_LCO_APL_SEQ
			AND D.AD_LCO_APL_DSB IS NULL
			AND D.BD_LCO_BR_GRC_END IS NOT NULL
			AND (D.AC_LCO_ACC_STA IS NULL
			OR D.AC_LCO_ACC_STA = '')
		LEFT OUTER JOIN OLWHRM1.PD6A_LCO_PRS_DMO E
			ON D.DF_LCO_PRS_SSN_BR = E.DF_LCO_PRS_SSN
FOR READ ONLY WITH UR
);

DISCONNECT FROM DB2;
/*ENDRSUBMIT;*/
/*DATA BWRGRCEND; SET WORKLOCL.BWRGRCEND; RUN;*/
/*DATA CBWRGRCEND; SET WORKLOCL.CBWRGRCEND; RUN;*/

DATA ALLGRCEND;
SET BWRGRCEND CBWRGRCEND;
RUN;

PROC SORT DATA=ALLGRCEND;
BY BF_SSN;
RUN;

PROC SQL;
CREATE TABLE GRCEND AS
SELECT	*
FROM 	ALLGRCEND A
WHERE	A.LCO_GRC_END <> (SELECT	MAX(B.LD_END_GRC_PRD)
						  FROM		ALLGRCEND B
						  WHERE		A.BF_SSN = B.BF_SSN)
;

%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;
%sqlcheck;
quit;

/*ENDRSUBMIT;*/
/*DATA GRCEND; SET WORKLOCL.GRCEND; RUN;*/

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;

PROC PRINT NOOBS SPLIT='/' DATA=GRCEND WIDTH=UNIFORM WIDTH=MIN;
VAR		DF_SPE_ACC_ID
		DM_PRS_LST
		DF_LCO_PRS_SSN_CBR
		DM_CPRS_LST
		LN_SEQ
		LD_END_GRC_PRD
		LCO_GRC_END
		PRS_TYP;
LABEL	DF_SPE_ACC_ID = 'ACCT #'
		DM_PRS_LST = 'BORROWER LAST NAME'
		DF_LCO_PRS_SSN_CBR = 'COBORROWER SSN'
		DM_CPRS_LST = 'COBORROWER LAST NAME'
		LN_SEQ = 'LOAN SEQ NO'
		LD_END_GRC_PRD = 'COMPASS GRACE END'
		LCO_GRC_END = 'LCO GRACE END'
		PRS_TYP = 'PERSON TYPE';
FORMAT 	LD_END_GRC_PRD MMDDYY10.
		LCO_GRC_END MMDDYY10.;
TITLE	'NON-DISB CONSOL LOAN WITH NO MATCHING GRACE END DATE';
FOOTNOTE  'JOB = UTLWG57     REPORT = ULWG57.LWG57R2';
RUN;
PROC PRINTTO;
RUN;
