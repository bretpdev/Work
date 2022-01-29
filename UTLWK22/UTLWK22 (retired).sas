/*set job specific values*/
/*	%LET ARCTYPEID = 0;		*Atd22ByLoan - Add arc by sequence number;*/
/*	%LET ARCTYPEID = 1;		*Atd22AllLoans - Add arc to all loans;*/
	%LET ARCTYPEID = 2;		*Atd22ByBalance - Add arc for all loans with a balance;
/*	%LET ARCTYPEID = 3;		*Atd22ByLoanProgram - Add arc by loan program;*/
/*	%LET ARCTYPEID = 4;		*Atd22AllLoansRegards - Add arc to all loans with regards to information;*/
/*	%LET ARCTYPEID = 5;		*Atd22ByLoanRegards - Add arc by sequence number with regards to information;*/

	%LET ARC = 'K1APP';
	%LET COMMENT = 'Review borrower application for any possible new reference records including employer information';
	%LET SASID = 'UTLWK22';

/*set up library to SQL Server and include common code*/
/*	LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ULS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/

	LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ULS.dsn; update_lock_typ=nolock; bl_keepnulls=no";
	%INCLUDE "X:\Sessions\Local SAS Schedule\ArcAdd Common.SAS";


*--------------------------------------------*
| UTLWK22 - SKIP NEW LOAN APPLICATION REVIEW |
*--------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

/*TEST*/
/*%LET RPTLIB =T:\;*/
/*%LET RPTLIB2 =T:\SAS;*/

/*LIVE*/
%LET RPTLIB =X:\PADD\FTP;
%LET RPTLIB2 =X:\Archive\SAS;

FILENAME REPORT2 "&RPTLIB/ULWK22.LWK22R2.&SYSDATE";
FILENAME REPORT3 "&RPTLIB2/ULWK22.LWK22R2.&SYSDATE";
FILENAME REPORTZ "&RPTLIB/ULWK22.LWK22RZ.&SYSDATE";
LIBNAME  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
LIBNAME PROGREVW '/sas/whse/progrevw';

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
		CREATE TABLE SNLAR AS
			SELECT *
			FROM CONNECTION TO DB2 (
				SELECT DISTINCT 
					LN10.BF_SSN AS TARGET_ID 
					,'KLOANAPP' AS QUEUE_NAME 
					,'' AS INSTITUTION_ID 
					,'' AS INSTITUTION_TYPE
					,'' AS DATE_DUE 
					,'' AS TIME_DUE 
					,'Review borrower application for any possible new reference'||' '||
					 'records including employer information' AS COMMENT
					,LN10.LD_LON_1_DSB
					,AY01.BD_ATY_PRF
					,AY012.K1_IND
					,PD10.DF_SPE_ACC_ID
					,LN10.LF_LON_CUR_OWN
					,AY10.LD_ATY_REQ_RCV
					,CASE 
						WHEN PD01.DF_PRS_ID IS NULL THEN 1
						ELSE 0
					END AS COMPASS_ONLY
				FROM 
					OLWHRM1.LN10_LON LN10
					LEFT JOIN OLWHRM1.PD01_PDM_INF PD01
						ON PD01.DF_PRS_ID = LN10.BF_SSN
					LEFT OUTER JOIN 
					(
						SELECT 
							DF_PRS_ID
							,MAX(BD_ATY_PRF) AS BD_ATY_PRF 
						FROM 
							OLWHRM1.AY01_BR_ATY 
						WHERE 
							PF_ACT = 'K1APP'
						GROUP BY 
							DF_PRS_ID
					) AY01
						ON LN10.BF_SSN = AY01.DF_PRS_ID
					INNER JOIN (
							SELECT 
								DF_PRS_ID
							FROM 
								OLWHRM1.PD30_PRS_ADR 
							WHERE
								DC_ADR = 'L'
								AND DI_VLD_ADR = 'N'
						UNION 
							SELECT 
								DF_PRS_ID
							FROM 
								OLWHRM1.PD42_PRS_PHN
							WHERE 
								DC_PHN = 'H'
								AND DI_PHN_VLD = 'N'
						 ) PD3042
						ON LN10.BF_SSN = PD3042.DF_PRS_ID
					LEFT OUTER JOIN 
					(
						SELECT 
							DF_PRS_ID
							,'Y' AS K1_IND
						FROM 
							OLWHRM1.AY01_BR_ATY 
						WHERE 
							PF_ACT = 'K1APP'
							AND BD_ATY_PRF > '06/01/2001'
						GROUP BY 
							DF_PRS_ID
					) AY012
						ON LN10.BF_SSN = AY012.DF_PRS_ID
					JOIN OLWHRM1.PD10_PRS_NME PD10
						ON LN10.BF_SSN = PD10.DF_PRS_ID
					LEFT JOIN (
							SELECT
								AY10.BF_SSN
								,MX.LD_ATY_REQ_RCV
							FROM 
								OLWHRM1.AY10_BR_LON_ATY AY10
								JOIN (
										SELECT
											AY10.BF_SSN
											,MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
										FROM
											OLWHRM1.AY10_BR_LON_ATY AY10
										WHERE 
											AY10.PF_REQ_ACT = 'K1APP'
										GROUP BY 
											AY10.BF_SSN
										)MX
									ON AY10.BF_SSN = MX.BF_SSN
								) AY10 
						ON LN10.BF_SSN = AY10.BF_SSN
				WHERE LN10.LA_CUR_PRI > 0
					AND LN10.LC_STA_LON10 = 'R'
/*					AND LN10.IC_LON_PGM <> 'TILP'*/
/*					AND LN10.LF_LON_CUR_OWN NOT IN ('826717','830248')*/
					AND NOT EXISTS (
						SELECT *
						FROM OLWHRM1.CT30_CALL_QUE CT30
						WHERE CT30.DF_PRS_ID_BR = LN10.BF_SSN
						AND CT30.IF_WRK_GRP = 'KLOANAPP'
				) 
			FOR READ ONLY WITH UR
		);
	DISCONNECT FROM DB2;
	%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;
	%SQLCHECK (SQLRPT=ULWK22.LWK22RZ);

	CREATE TABLE SNLAR2 AS 
		SELECT
			S.*
			,LGL.LENDER_GROUP_ID
		FROM 
			SNLAR S
			JOIN PROGREVW.LENDER_GROUP_LENDERS LGL
				ON S.LF_LON_CUR_OWN = LGL.LENDER_ID
	;

QUIT;
ENDRSUBMIT;

DATA SNLAR;
SET WORKLOCL.SNLAR2;
RUN;

PROC SQL;
	CREATE TABLE REMOTE_DATA AS
		SELECT DISTINCT
			S.DF_SPE_ACC_ID
		FROM 
			SNLAR S
			LEFT JOIN 
			(
				SELECT
					S.DF_SPE_ACC_ID
				FROM 
					SNLAR S
				WHERE 
					LENDER_GROUP_ID = 3
			) TRES
				ON S.DF_SPE_ACC_ID = TRES.DF_SPE_ACC_ID
		WHERE 
			TRES.DF_SPE_ACC_ID IS NULL
			AND 
			(S.LD_LON_1_DSB > S.LD_ATY_REQ_RCV OR S.LD_ATY_REQ_RCV IS NULL)
	;

	CREATE TABLE SNLAR3 AS
		SELECT
			S.*
		FROM 
			SNLAR S
			LEFT JOIN REMOTE_DATA RD
				ON S.DF_SPE_ACC_ID = RD.DF_SPE_ACC_ID
		WHERE 
			RD.DF_SPE_ACC_ID IS NULL
	;
QUIT;

DATA SNLAR (DROP= LD_LON_1_DSB BD_ATY_PRF K1_IND DF_SPE_ACC_ID LF_LON_CUR_OWN LENDER_GROUP_ID);
SET SNLAR3;
IF (LD_LON_1_DSB > BD_ATY_PRF AND BD_ATY_PRF ^= .) OR K1_IND ^= 'Y' 
	THEN OUTPUT SNLAR;
ELSE DELETE ;
RUN;

PROC SORT DATA=SNLAR NODUPKEY;
BY TARGET_ID;
RUN;

DATA _NULL_;
SET  WORK.SNLAR; 
WHERE COMPASS_ONLY = 0;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT COMMENTS $600. ;
   FORMAT TARGET_ID $9. ;
   FORMAT QUEUE_NAME $8. ;
   FORMAT INSTITUTION_ID $1. ;
   FORMAT INSTITUTION_TYPE $1. ;
   FORMAT DATE_DUE $1. ;
   FORMAT TIME_DUE $1. ;
DO;
	PUT TARGET_ID $ @;
	PUT QUEUE_NAME $ @;
	PUT INSTITUTION_ID $ @;
	PUT INSTITUTION_TYPE $ @;
	PUT DATE_DUE $ @;
	PUT TIME_DUE $ @;
	PUT COMMENT $ ;
END;
RUN;

/*When moved to live, this will send an additional, identical report to X:\Archive\SAS*/
DATA _NULL_;
SET  WORK.SNLAR; 
WHERE COMPASS_ONLY = 0;
FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT COMMENTS $600. ;
   FORMAT TARGET_ID $9. ;
   FORMAT QUEUE_NAME $8. ;
   FORMAT INSTITUTION_ID $1. ;
   FORMAT INSTITUTION_TYPE $1. ;
   FORMAT DATE_DUE $1. ;
   FORMAT TIME_DUE $1. ;
DO;
	PUT TARGET_ID $ @;
	PUT QUEUE_NAME $ @;
	PUT INSTITUTION_ID $ @;
	PUT INSTITUTION_TYPE $ @;
	PUT DATE_DUE $ @;
	PUT TIME_DUE $ @;
	PUT COMMENT $ ;
END;
RUN;

/*call macro or put data step here to add job specific data to LEGEND data*/
%CREATE_GENERIC_ARCADD_DATA;
/*end job specific code*/

/*call ARC add common processing*/
%ARC_ADD_PROCESSING;
