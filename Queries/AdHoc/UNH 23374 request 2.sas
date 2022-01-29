/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

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

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						LN40.BF_SSN				AS SSN,
						LN40.LN_SEQ_CLM_PCL AS CLAIM_NUMBER,
						LN40.LD_SBM_CLM_PCL AS CLAIM_DATE,
						LN90.PMT_DATE AS PAID_DATE,
						LN10.IF_GTR,
						(LN90.BAL + LN90_A.BAL) AS AMT_PAID
					FROM	
						OLWHRM1.LN40_LON_CLM_PCL LN40
					LEFT JOIN OLWHRM1.LN10_LON_EOM LN10
							ON LN40.BF_SSN = LN10.BF_SSN
							AND LN40.LN_SEQ = LN10.LN_SEQ
					LEFT JOIN 
					(
						SELECT
							LN90.BF_SSN,
							LN10.IF_GTR,
							MAX(LD_FAT_EFF) AS PMT_DATE,
							SUM(COALESCE(LA_FAT_CUR_PRI,0) + COALESCE(LA_FAT_NSI, 0)) AS BAL
						FROM
							OLWHRM1.LN90_FIN_ATY LN90
						INNER JOIN OLWHRM1.LN10_LON LN10
							ON LN10.BF_SSN = LN90.BF_SSN
							AND LN10.LN_SEQ = LN90.LN_SEQ
						WHERE
							PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP = '30'
							AND LC_STA_LON90 = 'A'
							AND LC_FAT_REV_REA = ''
/*							AND DAYS(LD_FAT_EFF) BETWEEN DAYS('06/01/2015') AND DAYS('06/30/2015')*/
						GROUP BY
							LN90.BF_SSN,
							LN10.IF_GTR
							
					) LN90
						ON LN90.BF_SSN = LN40.BF_SSN
						AND LN90.IF_GTR = LN10.IF_GTR
					LEFT JOIN 
					(
						SELECT
							LN90.BF_SSN,
							LN10.IF_GTR,
							SUM(COALESCE(LA_FAT_CUR_PRI,0) + COALESCE(LA_FAT_NSI, 0)) AS BAL
						FROM
							OLWHRM1.LN90_FIN_ATY LN90
						INNER JOIN OLWHRM1.LN10_LON LN10
							ON LN10.BF_SSN = LN90.BF_SSN
							AND LN10.LN_SEQ = LN90.LN_SEQ
						WHERE
							PC_FAT_TYP = '34' AND PC_FAT_SUB_TYP = '01'
							AND LC_STA_LON90 = 'A'
							AND LC_FAT_REV_REA = ''
/*							AND DAYS(LD_FAT_EFF) BETWEEN DAYS('06/01/2015') AND DAYS('06/30/2015')*/
						GROUP BY
							LN90.BF_SSN,
							LN10.IF_GTR
							
					) LN90_A
						ON LN90_A.BF_SSN = LN40.BF_SSN
						AND LN90_A.IF_GTR = LN10.IF_GTR
					WHERE
						DAYS(LN90.PMT_DATE) BETWEEN DAYS('06/01/2015') AND DAYS('06/30/2015')
						AND LN40.LD_SBM_CLM_PCL IS NOT NULL
/*						AND LN90_A.BF_SSN IS NOT NULL AND LN90.BF_SSN IS NOT NULL */
						AND (LN40.LC_TYP_REC_CLP_LON = 1 OR LN40.LC_TYP_REC_CLP_LON  = 6)
						AND LN10.IF_GTR IN  ('000706','000708','000730','000731','000751','000755','000800','000927','000951')


					FOR READ ONLY WITH UR
				)
	;
QUIT;

proc sql;
	create table t as 
		select
			*
		from
			OLWHRM1.LN90_FIN_ATY
		where
			bf_ssn = '520760012'
			and ((PC_FAT_TYP = '34' AND PC_FAT_SUB_TYP = '01') OR (PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP = '30'))
			AND LC_STA_LON90 = 'A'
			AND LC_FAT_REV_REA = ''
			
;
quit;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;

DATA T;
SET DUSTER.T;
RUN;

proc sql;
	create table final as
		select distinct
			ssn,
			if_gtr,
			abs(amt_paid) as total_paid,
			PAID_DATE,
			CLAIM_DATE,
			count(claim_number) as claim_count
		from
			demo
		where
			abs(amt_paid) > 0
		group by
			ssn,
			if_gtr,
			amt_paid,
			PAID_DATE,
			CLAIM_DATE
		
;
quit;


PROC EXPORT DATA = final 
            OUTFILE = "T:\NH 23374_request_with claim date.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;

PROC EXPORT DATA = t 
            OUTFILE = "T:\NH 23374_request_DATA FOR 48 3418 8333.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
