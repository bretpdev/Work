/*UTLWO26 - CONSOL ADD-ON WITH INT RATE DISCREPANCIES*/

LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWO26.LWO26R2";
FILENAME REPORTZ "&RPTLIB/ULWO26.LWO26RZ";


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
CREATE TABLE LCOINT AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
		A.BF_SSN
		,D.DM_PRS_LST_opps
		,A.IC_LON_PGM
		,A.LN_SEQ
		,C1.LR_LCO_LN_INT
		,LN72.LR_INT_RDC_PGM_ORG
FROM	OLWHRM1.LN10_LON A
		INNER JOIN OLWHRM1.AY10_BR_LON_ATY B ON
			A.BF_SSN = B.BF_SSN
			AND A.IC_LON_PGM IN ('SUBCNS','UNCNS','SUBSPC','UNSPC')
			AND	A.LC_STA_LON10 = 'R'
			AND A.LA_CUR_PRI > 0
			AND B.LC_STA_ACTY10 = 'A'
			AND B.PF_REQ_ACT IN ('OC308','OC318')
		 INNER JOIN (SELECT X.BF_SSN
							,X.LN_SEQ
							,X.LR_INT_RDC_PGM_ORG
		 			 FROM   OLWHRM1.LN72_INT_RTE_HST X
		 			 WHERE  X.LC_STA_LON72 = 'A'
							AND X.LD_ITR_EFF_BEG <= CURRENT DATE
							AND X.LD_ITR_EFF_END >= CURRENT DATE
					)LN72 ON
			A.BF_SSN = LN72.BF_SSN
			AND A.LN_SEQ = LN72.LN_SEQ
/*		 INNER JOIN OLWHRM1.LC10_UND_LN_INF C ON*/
/*			A.BF_SSN = C.DF_LCO_PRS_SSN_BR*/
/*			AND A.LD_LON_1_DSB = C.LD_UND_LN_ORG_DSB*/
		INNER JOIN OLWHRM1.AP1A_LCO_APL C1 ON
			A.BF_SSN = C1.DF_LCO_PRS_SSN_BR
			AND A.LD_LON_1_DSB = C1.AD_LCO_APL_DSB
/*			AND C.AN_LCO_APL_SEQ = C1.AN_LCO_APL_SEQ*/
			AND LN72.LR_INT_RDC_PGM_ORG <> C1.LR_LCO_LN_INT
		INNER JOIN OLWHRM1.PD10_PRS_NME D ON
			A.BF_SSN = D.DF_PRS_ID
WHERE	(SELECT	COUNT(*)
		 FROM	OLWHRM1.LN90_FIN_ATY Z
		 WHERE	Z.BF_SSN = A.BF_SSN
		 		AND Z.LN_SEQ = A.LN_SEQ
				AND LN_FAT_SEQ_REV IS NULL
				AND Z.PC_FAT_TYP = '01'
				AND Z.PC_FAT_SUB_TYP = '01'
				AND Z.LC_CSH_ADV = 'A') > 1
ORDER BY D.DM_PRS_LST
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>>  ;  ** includes error messages to SAS log  ;
%sqlcheck;
quit;

/*ENDRSUBMIT;*/
/**/
/*DATA LCOINT;*/
/*SET WORKLOCL.LCOINT;*/
/*RUN;*/

PROC PRINTTO PRINT=REPORT2;
RUN;
OPTIONS NODATE CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;

PROC PRINT NOOBS SPLIT='/' DATA=LCOINT WIDTH=UNIFORM WIDTH=MIN;
VAR 	BF_SSN
		DM_PRS_LST
		IC_LON_PGM
		LN_SEQ
		LR_LCO_LN_INT
		LR_INT_RDC_PGM_ORG;
LABEL	BF_SSN = 'SSN'
		DM_PRS_LST = 'LAST NAME'
		IC_LON_PGM = 'LOAN TYPE'
		LN_SEQ = 'COMPASS LOAN SEQ #'
		LR_LCO_LN_INT = 'LCO NEW INTEREST RATE'
		LR_INT_RDC_PGM_ORG = 'COMPASS INTEREST RATE';
TITLE	'TWO STEP CONSOLIDATION LOANS';
FOOTNOTE  'JOB = UTLWO26     REPORT = ULWO26.LWO26R2';
RUN;