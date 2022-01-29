/*%LET RPTLIB = T:\SAS;*/
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ULS_TEST.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= dbo;
%INCLUDE "Y:\Codebase\SAS\ArcAdd Common.SAS";*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ULS.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= dbo;
%INCLUDE "Z:\Codebase\SAS\ArcAdd Common.SAS";
%LET RPTLIB = X:\PADD\FTP;
FILENAME REPORTZ "&RPTLIB/ULWS67.LWS67RZ";
FILENAME REPORT2 "&RPTLIB/ULWS67.LWS67R2";


/*TEST*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK;*/

/*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

RSUBMIT;

/*%let DB = DLGSWQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;

DATA _NULL_;
	last_run= today()-7;
	CALL SYMPUTX('LASTRUN',"'"||PUT(LAST_RUN,MMDDYY10.)||"'" );
	CALL SYMPUTX('YESTERDAY',"'"||PUT(TODAY()-1,MMDDYY10.)||"'" );
RUN;
%PUT LASTRUN=&LASTRUN;
%PUT YESTERDAY=&YESTERDAY;

LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;

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
%MEND SQLCHECK;


PROC SQL;
CONNECT TO DB2 (DATABASE=&DB);

/*GET GENERAL FORBEARNCE INFORMATION FOR ALL RECORDS*/
CREATE TABLE TotalForbTime AS
	SELECT *
	FROM CONNECTION TO DB2 
	(
		SELECT DISTINCT
			LN60.BF_SSN,
			LN60.LN_SEQ,
			LAST7.DF_SPE_ACC_ID,
			LAST7.LF_USR_CRT_REQ_FOR,
			MAX(LAST7.LD_FOR_APL) AS LD_FOR_APL,
			MAX(LN60.LD_FOR_END) AS LD_FOR_END,
			SUM((DAYS(LN60.LD_FOR_END) - DAYS(LN60.LD_FOR_BEG))/365.00*12.00) AS MONTHS_USED 
		FROM
			OLWHRM1.FB10_BR_FOR_REQ FB10
		 	INNER JOIN OLWHRM1.LN60_BR_FOR_APV LN60
				ON FB10.BF_SSN = LN60.BF_SSN
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
				AND FB10.LC_FOR_TYP = '05'
				AND LN60.LC_STA_LON60 = 'A'
				AND FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
			INNER JOIN 
			(
				SELECT DISTINCT
					FB10.BF_SSN,
					PD10.DF_SPE_ACC_ID,
					FB10.LF_USR_CRT_REQ_FOR,
					LN60.LD_FOR_APL
				FROM 
					OLWHRM1.FB10_BR_FOR_REQ FB10
					INNER JOIN OLWHRM1.LN60_BR_FOR_APV LN60
						ON FB10.BF_SSN = LN60.BF_SSN
						AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
						AND FB10.LC_FOR_TYP = '05'
						AND LN60.LC_STA_LON60 = 'A'
						AND FB10.LC_FOR_STA = 'A'
						AND FB10.LC_STA_FOR10 = 'A'
						AND LN60.LD_FOR_APL BETWEEN &LASTRUN AND &YESTERDAY
					INNER JOIN OLWHRM1.PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = FB10.BF_SSN
			) LAST7 
				ON LAST7.BF_SSN = LN60.BF_SSN
			LEFT OUTER JOIN
			(
				SELECT DISTINCT
					AY10.BF_SSN,
					AY10.PF_REQ_ACT,
					AY10.LD_ATY_REQ_RCV
				FROM
					OLWHRM1.AY10_BR_LON_ATY AY10
				WHERE
					AY10.PF_REQ_ACT = 'FBEMA'
			) AY10Ex 
				ON AY10Ex.BF_SSN = LN60.BF_SSN
				AND DAYS(AY10Ex.LD_ATY_REQ_RCV) BETWEEN DAYS(LN60.LD_FOR_APL)-14 AND DAYS(LN60.LD_FOR_APL)
			LEFT OUTER JOIN /*Exclude borrowers with an open G303M arc task*/
			(
				SELECT
					WQ20.BF_SSN
				FROM
					OLWHRM1.WQ20_TSK_QUE WQ20
				WHERE
					WQ20.PF_REQ_ACT = 'G303M'
					AND WQ20.WC_STA_WQUE20 NOT IN ('X','C')
			) G303M 
				ON G303M.BF_SSN = LN60.BF_SSN
		WHERE
			AY10Ex.BF_SSN IS NULL
			AND G303M.BF_SSN IS NULL
		GROUP BY
			LN60.BF_SSN,
			LN60.LN_SEQ,
			LAST7.DF_SPE_ACC_ID,
			LAST7.LF_USR_CRT_REQ_FOR
		HAVING
			SUM((DAYS(LN60.LD_FOR_END) - DAYS(LN60.LD_FOR_BEG))/365.00*12.00) > 36 /*36 months of forb used on the loan*/
			
	FOR READ ONLY WITH UR
);

DISCONNECT FROM DB2;

/*GET FORBEARANCE INFO FOR BORROWERS WHERE DURATION OF FORBEARANCES > 36 MONTHS*/
CREATE TABLE FORB_36 AS
SELECT DISTINCT
	TFT.DF_SPE_ACC_ID,
	TFT.LF_USR_CRT_REQ_FOR,
	TFT.LD_FOR_APL,
	'' AS RECIPIENT_ID,
	CAT('Forbearance processed by ', TFT.LF_USR_CRT_REQ_FOR, ' on ', PUT(TFT.LD_FOR_APL,mmddyy10.)) AS COMMENT,
	0 AS IS_REFERENCE,
	0 AS IS_ENDORSER,
	. AS PROCESS_FROM,
	. AS PROCESS_TO,
	. AS NEEDED_BY,
	'' AS REGARDS_TO,
	'' AS REGARDS_CODE,
	'UTLWS67' AS CREATED_BY
FROM
	TotalForbTime TFT
;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA FORB_36; SET DUSTER.FORB_36; RUN;

/*write to arc add*/
%GENERAL_ARC_ADD_PROCESSING(WORK.FORB_36, DF_SPE_ACC_ID, COMMENT, RECIPIENT_ID, IS_REFERENCE,  IS_ENDORSER, PROCESS_FROM, PROCESS_TO, NEEDED_BY, REGARDS_TO, REGARDS_CODE, CREATED_BY, 1, 'G303M', 'UTLWS67');
