%LET RPTLIB = T:\SAS;
%LET RPTNAME = NH_28352_request_with_claim_date;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

%LET BEGINDATE = '07/01/2016';
%LET ENDDATE = '07/31/2016';

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

	CREATE TABLE CLAIMS AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
				SELECT DISTINCT
					LN40.BF_SSN			 AS SSN
					,LN40.LN_SEQ_CLM_PCL AS CLAIM_NUMBER
					,LN40.LD_SBM_CLM_PCL AS CLAIM_DATE
					,LN90.PMT_DATE 		 AS PAID_DATE
					,LN10.IF_GTR
					,(LN90.BAL + LN90_A.BAL) AS AMT_PAID
				FROM	
					OLWHRM1.LN40_LON_CLM_PCL LN40
				LEFT JOIN OLWHRM1.LN10_LON_EOM LN10
					ON LN40.BF_SSN = LN10.BF_SSN
					AND LN40.LN_SEQ = LN10.LN_SEQ
				LEFT JOIN 
					(
					SELECT
						LN90.BF_SSN
						,LN10.IF_GTR
						,MAX(LD_FAT_EFF) AS PMT_DATE
						,SUM(COALESCE(LA_FAT_CUR_PRI,0) + COALESCE(LA_FAT_NSI, 0)) AS BAL
					FROM
						OLWHRM1.LN90_FIN_ATY LN90
					INNER JOIN OLWHRM1.LN10_LON LN10
						ON LN10.BF_SSN = LN90.BF_SSN
						AND LN10.LN_SEQ = LN90.LN_SEQ
					WHERE
						PC_FAT_TYP = '10' 
						AND PC_FAT_SUB_TYP = '30'
						AND LC_STA_LON90 = 'A'
						AND LC_FAT_REV_REA = ''
/*						AND DAYS(LD_FAT_EFF) BETWEEN DAYS('06/01/2015') AND DAYS('06/30/2015')*/
					GROUP BY
						LN90.BF_SSN
						,LN10.IF_GTR
					) LN90
						ON LN90.BF_SSN = LN40.BF_SSN
						AND LN90.IF_GTR = LN10.IF_GTR
				LEFT JOIN 
					(
					SELECT
						LN90.BF_SSN
						,LN10.IF_GTR
						,SUM(COALESCE(LA_FAT_CUR_PRI,0) + COALESCE(LA_FAT_NSI, 0)) AS BAL
					FROM
						OLWHRM1.LN90_FIN_ATY LN90
					INNER JOIN OLWHRM1.LN10_LON LN10
						ON LN10.BF_SSN = LN90.BF_SSN
						AND LN10.LN_SEQ = LN90.LN_SEQ
					WHERE
						PC_FAT_TYP = '34' 
						AND PC_FAT_SUB_TYP = '01'
						AND LC_STA_LON90 = 'A'
						AND LC_FAT_REV_REA = ''
/*						AND DAYS(LD_FAT_EFF) BETWEEN DAYS('06/01/2015') AND DAYS('06/30/2015')*/
					GROUP BY
						LN90.BF_SSN
						,LN10.IF_GTR
					) LN90_A
						ON LN90_A.BF_SSN = LN40.BF_SSN
						AND LN90_A.IF_GTR = LN10.IF_GTR
				WHERE
					DAYS(LN90.PMT_DATE) BETWEEN DAYS(&BEGINDATE) AND DAYS(&ENDDATE)
					AND LN40.LD_SBM_CLM_PCL IS NOT NULL
/*					AND LN90_A.BF_SSN IS NOT NULL AND LN90.BF_SSN IS NOT NULL */
					AND LN40.LC_TYP_REC_CLP_LON IN (1,6)
					AND LN10.IF_GTR IN 
						(
						/*9 pre-BANA guarantors*/
						'000706','000708','000730',
						'000731','000751','000755',
						'000800','000927','000951',
						/*14 new BANA guarantors*/
						'000712','000717',
						'000723','000725',
						'000729','000733',
						'000734','000736',
						'000740','000742',
						'000744','000747',
						'000748','000753'
						)

					FOR READ ONLY WITH UR
				)
	;
QUIT;

ENDRSUBMIT;

DATA CLAIMS;
	SET DUSTER.CLAIMS;
RUN;

PROC SQL;
	CREATE TABLE FINAL AS
		SELECT DISTINCT
			SSN
			,IF_GTR
			,ABS(AMT_PAID) AS TOTAL_PAID
			,PAID_DATE
			,CLAIM_DATE
			,COUNT(CLAIM_NUMBER) AS CLAIM_COUNT
		FROM
			CLAIMS
		WHERE
			ABS(AMT_PAID) > 0
		GROUP BY
			SSN
			,IF_GTR
			,AMT_PAID
			,PAID_DATE
			,CLAIM_DATE
;
QUIT;

PROC EXPORT 
	DATA = FINAL 
    OUTFILE = "&RPTLIB\&RPTNAME..xlsx" 
    DBMS = EXCEL
	REPLACE;
    SHEET="Part_1"; 
RUN;
