*---------------------------------------------------------*
|UTLWG93 - Expire MPNs Ln Sold Prev Day Ineligible Lender |
*---------------------------------------------------------*;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORT2 "&RPTLIB/ULWG93.LWG93R2";
FILENAME REPORTZ "&RPTLIB/ULWG93.LWG93RZ";
DATA _NULL_;
     CALL SYMPUT('DAYS_1',"'"||PUT(INTNX('DAY',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
RUN;
/*%SYSLPUT DAYS_1 = &DAYS_1;*/
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
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
CREATE TABLE EMLSPDI AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT GA01.DF_PRS_ID_BR
	,GA10.AF_APL_ID||GA10.AF_APL_ID_SFX AS CLUID
FROM OLWHRM1.GA01_APP GA01
INNER JOIN OLWHRM1.GA10_LON_APP GA10
	ON GA01.AF_APL_ID = GA10.AF_APL_ID
INNER JOIN OLWHRM1.GA40_BS_MPN_CTL GA40
	ON GA10.AF_APL_ID = GA40.AF_BS_MPN_APL_ID
INNER JOIN (
	SELECT DISTINCT A.LF_LON_ALT AS AF_APL_ID
		,A.LN_LON_ALT_SEQ AS AF_APL_ID_SFX
		,A.LF_LON_CUR_OWN
		,C.IF_SLL_OWN_SLD
	FROM OLWHRM1.LN10_LON A
	INNER JOIN OLWHRM1.LN90_FIN_ATY B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
	INNER JOIN OLWHRM1.LN99_LON_SLE_FAT C
		ON B.BF_SSN = C.BF_SSN
		AND B.LN_SEQ = C.LN_SEQ
		AND B.LN_FAT_SEQ = C.LN_FAT_SEQ
	WHERE B.PC_FAT_TYP IN ('03','04')
	AND B.PC_FAT_SUB_TYP = '95'
	AND B.LC_STA_LON90 = 'A'
	AND B.LD_FAT_EFF = &DAYS_1  
	AND A.LF_LON_CUR_OWN = '828476'
	 ) COMPASS
	ON COMPASS.AF_APL_ID = GA10.AF_APL_ID 
	AND COMPASS.AF_APL_ID_SFX = INT(GA10.AF_APL_ID_SFX)  
INNER JOIN OLWHRM1.LR01_LGS_LDR_INF X
	ON COMPASS.IF_SLL_OWN_SLD = X.IF_IST
WHERE GA10.AC_PRC_STA = 'A'
AND GA10.AC_LON_TYP IN ('SF','SU','PL','GB')
AND GA40.AC_MPN_STA = 'A'
AND X.IC_DOE_LDR_STA = 'U'
FOR READ ONLY WITH UR
);

CREATE TABLE EXCLUDE AS
SELECT DISTINCT CLUID LENGTH=19
FROM CONNECTION TO DB2 (
	(
		SELECT C.LF_LON_ALT||'0'||LTRIM(CHAR(C.LN_LON_ALT_SEQ)) AS CLUID
		FROM OLWHRM1.LN99_LON_SLE_FAT A
		INNER JOIN OLWHRM1.OW30_LON_SLE_CTL B
			 ON A.IF_LON_SLE = B.IF_LON_SLE
		INNER JOIN OLWHRM1.LN10_LON C
			ON A.BF_SSN = C.BF_SSN
			AND A.LN_SEQ = C.LN_SEQ
		WHERE B.ID_LON_SLE = &DAYS_1
		AND	
		(
			(A.IF_SLL_OWN_SLD = '828476' AND A.IF_BUY_OWN_SLD = '828476')
		)
	)
	UNION
	(
		SELECT LON.LF_LON_ALT||'0'||LTRIM(CHAR(LON.LN_LON_ALT_SEQ)) AS CLUID
		FROM OLWHRM1.LN10_LON LON
		INNER JOIN OLWHRM1.LN90_FIN_ATY LN90
			ON LON.BF_SSN = LN90.BF_SSN
			AND LON.LN_SEQ = LN90.LN_SEQ 
		INNER JOIN OLWHRM1.LN99_LON_SLE_FAT LN99
			ON LN90.BF_SSN = LN99.BF_SSN
			AND LN90.LN_SEQ = LN99.LN_SEQ 
			AND LN90.LN_FAT_SEQ = LN99.LN_FAT_SEQ
		INNER JOIN OLWHRM1.LR10_LDR_DMO LR10
			ON LN99.IF_SLL_OWN_SLD = LR10.IF_DOE_LDR
			AND LN99.IF_BUY_OWN_SLD = LR10.IF_LDR_MRG_TO
		WHERE LN90.PC_FAT_TYP IN ('03','04')
			AND LN90.PC_FAT_SUB_TYP = '95' 
			AND LR10.IC_LDR_MRG_TYP IN ('F','1')
	)
);
DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;
%SQLCHECK (SQLRPT=ULWG93.LWG93RZ);
QUIT;
/*ENDRSUBMIT;*/
/*DATA EMLSPDI;SET WORKLOCL.EMLSPDI;RUN;*/
/*DATA EXCLUDE;SET WORKLOCL.EXCLUDE;RUN;*/
PROC SQL;
CREATE TABLE EMLSPDI2 AS 
SELECT DISTINCT A.*
FROM EMLSPDI A
WHERE NOT EXISTS (
	SELECT *
	FROM EXCLUDE X
	WHERE X.CLUID = A.CLUID
	)
;
QUIT;

DATA _NULL_;
SET  WORK.EMLSPDI2;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT DF_PRS_ID_BR $9. ;
FORMAT CLUID $19. ;
/*IF _N_ = 1 THEN    */
/* DO;*/
/*   PUT*/
/*   'DF_PRS_ID_BR'*/
/*   ','*/
/*   'CLUID'*/
/*   ;*/
/* END;*/
DO;
	PUT DF_PRS_ID_BR $ @;
	PUT CLUID $ ;
END;
RUN;
