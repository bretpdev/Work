*--------------------------------------------*
| UTLWA24 Fully Disbursed Loans after 7-1-08 |
*--------------------------------------------*;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORT2 "&RPTLIB/ULWA24.LWA24R2";
FILENAME REPORT3 "&RPTLIB/ULWA24.LWA24R3";
FILENAME REPORT4 "&RPTLIB/ULWA24.LWA24R4";
FILENAME REPORTZ "&RPTLIB/ULWA24.LWA24RZ";
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
CREATE TABLE FDLASOE AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT C.DF_SPE_ACC_ID,
	A.BF_SSN,
	A.LN_SEQ,
	A.LF_LON_CUR_OWN,
	A.LA_CUR_PRI,
	A.LC_STA_LON10,
	A.LD_LON_1_DSB,
	A.IC_LON_PGM,
	A.LF_DOE_SCL_ORG,
	B.WC_DW_LON_STA,
	B.WA_TOT_BRI_OTS, 
	D.IM_SCL_SHO,
	E.LD_DSB
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.PD10_PRS_NME C 
	ON A.BF_SSN = C.DF_PRS_ID
INNER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,MAX(LD_DSB) AS LD_DSB
	FROM OLWHRM1.LN15_DSB
	WHERE LC_STA_LON15 IN ('1','3')
		AND LC_DSB_TYP = '2'
	GROUP BY BF_SSN
		,LN_SEQ
	) E
	ON A.BF_SSN = E.BF_SSN
	AND A.LN_SEQ = E.LN_SEQ
LEFT OUTER JOIN OLWHRM1.DW01_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
LEFT OUTER JOIN OLWHRM1.SC10_SCH_DMO D
	ON A.LF_DOE_SCL_ORG = D.IF_DOE_SCL
WHERE A.LC_STA_LON10 = 'R'
	AND A.LA_CUR_PRI > 0
	AND A.LD_LON_1_DSB > '2008-07-01'
	AND	A.IC_LON_PGM IN ('STFFRD','UNSTFD','PLUS','PLUSGB')	
	AND NOT EXISTS 
	(
		SELECT 1
		FROM OLWHRM1.LN15_DSB Y
		WHERE A.BF_SSN = Y.BF_SSN
		AND A.LN_SEQ = Y.LN_SEQ
		AND Y.LC_STA_LON15 IN ('1','3')
		AND Y.LC_DSB_TYP = '1'
		AND (Y.LA_DSB_CAN IS NULL
		    OR Y.LA_DSB_CAN <> Y.LA_DSB)
	)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;
%sqlcheck;
quit;
/*ENDRSUBMIT;*/
/*DATA FDLASOE; */
/*	SET WORKLOCL.FDLASOE; */
/*RUN;*/
PROC SORT DATA=FDLASOE;
	BY LF_DOE_SCL_ORG DF_SPE_ACC_ID LN_SEQ;
RUN;
PROC FORMAT;
VALUE $LNSTA 
	'01' = 'IN GRACE'
	'02' = 'IN SCHOOL' 
	'03' = 'IN REPAYMENT'
	'04' = 'IN DEFERMENT' 
	'05' = 'IN FORBEARANCE' 
	'06' = 'IN CURE' 
	'07' = 'CLAIM PENDING' 
	'08' = 'CLAIM SUBMITTED' 
	'09' = 'CLAIM CANCLED'
	'10' = 'CLAIM REJECT' 
	'11' = 'CLAIM RETURNED' 
	'12' = 'CLAIM PAID' 
	'13' = 'PRE-CLAIM PENDING' 
	'14' = 'PRE-CLAIM SUBMITTED' 
	'15' = 'PRE-CLAIM CANCELLED' 
	'16' = 'DEATH ALLEGED' 
	'17' = 'DEATH VERIFIED' 
	'18' = 'DISABILITY ALLEGED' 
	'19' = 'DISABILITY VERIFIED' 
	'20' = 'BANKRUPTCY ALLEGED' 
	'21' = 'BANKRUPTCY VERIFIED' 
	'22' = 'PAID IN FULL' 
	'23' = 'NOT FULLY ORIGINATED' 
	'88' = 'PROCESSING ERROR';
QUIT;
/*PRINTED REPORTS*/
%MACRO UTLWA24_REPS(RVAR,RNO,TVAR);
PROC SQL;
CREATE TABLE REPIT AS
SELECT &RVAR
	,COUNT(*) AS LOAN_CT
	,SUM(LA_CUR_PRI) AS PRIN_TOTAL
FROM FDLASOE
GROUP BY &RVAR;
QUIT;
PROC PRINTTO PRINT=REPORT&RNO NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE1	'FULLY DISBURSED LOANS AFTER 07/01/2008';
TITLE2	"BY &TVAR";
FOOTNOTE  "JOB = UTLWA24		        REPORT = ULWA24.L2A24R&RNO";
PROC PRINT NOOBS SPLIT='/' DATA=REPIT WIDTH=UNIFORM WIDTH=MIN;	
	FORMAT LOAN_CT COMMA6. PRIN_TOTAL DOLLAR12.;
	VAR &RVAR LOAN_CT PRIN_TOTAL;
	LABEL &RVAR = "&TVAR/ID" LOAN_CT = 'LOAN/COUNT' PRIN_TOTAL = 'TOTAL/PRINCIPAL';
RUN;
PROC PRINTTO;
RUN;
%MEND UTLWA24_REPS;
%UTLWA24_REPS(LF_DOE_SCL_ORG,2,SCHOOL); 
%UTLWA24_REPS(LF_LON_CUR_OWN,4,LENDER); 
/*TEXT FILE*/
DATA _NULL_;
SET FDLASOE ;
FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT DF_SPE_ACC_ID $10. ;
   FORMAT LN_SEQ 6. ;
   FORMAT LF_LON_CUR_OWN $8. ;
   FORMAT LA_CUR_PRI 10.2 ;
   FORMAT WA_TOT_BRI_OTS 9.2 ;
   FORMAT IC_LON_PGM $6. ;
   FORMAT LF_DOE_SCL_ORG $8. ;
   FORMAT WC_DW_LON_STA $LNSTA. ;
   FORMAT IM_SCL_SHO $20. ;
   FORMAT LD_DSB MMDDYY10. ;
IF _N_ = 1 THEN DO;
	PUT
		'DF_SPE_ACC_ID'
		','
		'LN_SEQ'
		','
		'LF_LON_CUR_OWN'
		','
		'LA_CUR_PRI'
		','
		'IC_LON_PGM'
		','
		'LF_DOE_SCL_ORG'
		','
		'WC_DW_LON_STA'
		','
		'IM_SCL_SHO'
		','
		'WA_TOT_BRI_OTS'
		','
		'LD_DSB';
END;
DO;
   PUT DF_SPE_ACC_ID $ @;
   PUT LN_SEQ @;
   PUT LF_LON_CUR_OWN $ @;
   PUT LA_CUR_PRI @;
   PUT IC_LON_PGM $ @;
   PUT LF_DOE_SCL_ORG $ @;
   PUT WC_DW_LON_STA $ @;
   PUT IM_SCL_SHO $ @;
   PUT WA_TOT_BRI_OTS @;
   PUT LD_DSB ;
END;
RUN;
