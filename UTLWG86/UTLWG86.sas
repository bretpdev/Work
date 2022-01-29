*-------------------------------*
| CLAIM PAID MPNS TO BE EXPIRED |
*-------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWG86.LWG86R2";
FILENAME REPORTZ "&RPTLIB/ULWG86.LWG86RZ";

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
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE BASE AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.DF_PRS_ID_BR AS SSN
/*SERIAL LOAN INFO*/
	,B.AF_APL_ID||B.AF_APL_ID_SFX AS SLUID
	,A.AD_BR_SIG AS SER_SIGN_DT
	,B.AA_GTE_LON_AMT
	,D.LD_LDR_POF
	,B.AC_PRC_STA
	,B.AD_PRC
/*BASE MPN INFO IF SERIAL LOANS EXISTS*/
	,GA10_B.AF_APL_ID||GA10_B.AF_APL_ID_SFX  AS BLUID
	,GA01_B.AD_BR_SIG 		AS BAD_BR_SIG
	,GA10_B.AC_PRC_STA		AS BAC_PRC_STA
	,GA40.AC_MPN_STA		AS BAC_MPN_STA
	,DC01_B.LD_LDR_POF		AS BLD_LDR_POF
/*BASE MPN INFO IF NO SERIAL LOANS EXISTS*/
	,GA10_C.AF_APL_ID||GA10_C.AF_APL_ID_SFX  AS BLUID2
	,GA01_C.AD_BR_SIG		AS B2AD_BR_SIG
	,GA10_C.AC_PRC_STA		AS B2AC_PRC_STA
	,GA40_C.AC_MPN_STA		AS B2AC_MPN_STA
	,DC01_C.LD_LDR_POF		AS B2LD_LDR_POF

FROM OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.GA14_LON_STA C
	ON B.AF_APL_ID = C.AF_APL_ID
	AND B.AF_APL_ID_SFX = C.AF_APL_ID_SFX 
INNER JOIN OLWHRM1.DC01_LON_CLM_INF D
	ON B.AF_APL_ID = D.AF_APL_ID
	AND B.AF_APL_ID_SFX = D.AF_APL_ID_SFX

/*GET BASE MPN INFO IF IT EXISTS FOR A SERIAL LOAN*/
LEFT OUTER JOIN OLWHRM1.GA40_BS_MPN_CTL GA40
	ON A.AF_BS_MPN_APL_ID = GA40.AF_BS_MPN_APL_ID
	AND GA40.AC_MPN_STA = 'A'
LEFT OUTER JOIN OLWHRM1.GA01_APP GA01_B
	ON GA40.AF_BS_MPN_APL_ID = GA01_B.AF_APL_ID
LEFT OUTER JOIN OLWHRM1.GA10_LON_APP GA10_B
	ON GA01_B.AF_APL_ID = GA10_B.AF_APL_ID
	AND GA10_B.AC_PRC_STA = 'A'
LEFT OUTER JOIN OLWHRM1.DC01_LON_CLM_INF DC01_B
	ON GA10_B.AF_APL_ID = DC01_B.AF_APL_ID
	AND GA10_B.AF_APL_ID_SFX = DC01_B.AF_APL_ID_SFX
	AND DC01_B.LC_STA_DC10 = '03'

/*GET BASE MPN INFO IF THE BASE LOANS ARE THE ONLY LOANS ON THE MPN*/
LEFT OUTER JOIN OLWHRM1.GA40_BS_MPN_CTL GA40_C
	ON A.AF_APL_ID = GA40_C.AF_BS_MPN_APL_ID
	AND GA40_C.AC_MPN_STA = 'A'
LEFT OUTER JOIN OLWHRM1.GA01_APP GA01_C
	ON GA40_C.AF_BS_MPN_APL_ID = GA01_C.AF_APL_ID
LEFT OUTER JOIN OLWHRM1.GA10_LON_APP GA10_C
	ON GA01_C.AF_APL_ID = GA10_C.AF_APL_ID
	AND GA10_C.AC_PRC_STA = 'A'
LEFT OUTER JOIN OLWHRM1.DC01_LON_CLM_INF DC01_C
	ON GA10_C.AF_APL_ID = DC01_C.AF_APL_ID
	AND GA10_C.AF_APL_ID_SFX = DC01_C.AF_APL_ID_SFX
	AND DC01_C.LC_STA_DC10 = '03'

/*FILTER OUT BORROWERS WITH XPIREMPN QUEUE TASKS*/
LEFT OUTER JOIN OLWHRM1.CT30_CALL_QUE QUE
	ON A.DF_PRS_ID_BR = QUE.DF_PRS_ID_BR
	AND QUE.IF_WRK_GRP = 'XPIREMPN'

WHERE C.AC_STA_GA14 = 'A'
AND C.AC_LON_STA_TYP = 'CP'
AND C.AC_LON_STA_REA IN ('DF','DI','DE')
AND D.LC_STA_DC10 = '03'
AND QUE.IF_WRK_GRP IS NULL

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWG86.LWG86RZ);*/
/*QUIT;*/
ENDRSUBMIT;

DATA BASE;SET WORKLOCL.BASE;RUN;

DATA BASE2 (DROP=BLUID BAD_BR_SIG BAC_PRC_STA BAC_MPN_STA BLD_LDR_POF BLUID2 
	B2AD_BR_SIG	B2AC_PRC_STA B2AC_MPN_STA B2LD_LDR_POF);
SET BASE;
/*GET THE BASE MPN AND BASE MPN LOANS*/
IF BLUID NE '' THEN DO;
	BASE_ID = SUBSTR(BLUID,1,17);
	BASE_LOANS = BLUID;
	BASE_SIG_DT = BAD_BR_SIG;
	BASE_PRC_STA = BAC_PRC_STA;
	BASE_PRC_STA =  BAC_PRC_STA;
	BASE_MPN_STA = BAC_MPN_STA;
	BASE_LDR_POF = BLD_LDR_POF;
	OUTPUT;
	END;
ELSE IF BLUID2 NE '' THEN DO;
	BASE_ID = SUBSTR(BLUID2,1,17);
	BASE_LOANS = BLUID2;
	BASE_SIG_DT = B2AD_BR_SIG;
	BASE_PRC_STA = B2AC_PRC_STA;
	BASE_PRC_STA = B2AC_PRC_STA;
	BASE_MPN_STA = B2AC_MPN_STA;
	BASE_LDR_POF = B2LD_LDR_POF;
	OUTPUT;
	END;
ELSE DELETE;
RUN;

PROC SORT DATA=BASE2 NODUPKEY;
BY SSN BASE_LOANS;
RUN;

DATA _NULL_;
SET  WORK.BASE2;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT SSN $9. ;
FORMAT BASE_LOANS $19. ;
/*IF _N_ = 1 THEN        */
/* DO;*/
/*   PUT*/
/*   'SSN'*/
/*   ','*/
/*   'BASE_ID'*/
/*   ;*/
/* END;*/
DO;
PUT SSN $ @;
PUT BASE_LOANS $ ;
;
END;
RUN;

/*-------------------*
| EXCLUSION DATA SET |
*-------------------*/
/*PROC SQL;*/
/*CREATE TABLE EXCLUID AS */
/*SELECT DISTINCT A.SSN*/
/*	,A.BASE_ID*/
/*	,A.BASE_LOANS*/
/*FROM BASE2 A*/
/*WHERE A.BASE_SIG_DT = A.SER_SIGN_DT*/
/*AND A.AA_GTE_LON_AMT > 0*/
/*AND A.SER_SIGN_DT < A.LD_LDR_POF*/
/*AND (*/
/*	A.AD_PRC > (*/
/*		SELECT MAX(B.BASE_LDR_POF)*/
/*		FROM BASE2 B*/
/*		WHERE B.BASE_ID = A.BASE_ID*/
/*		)*/
/*	AND A.BASE_LDR_POF <> .*/
/*	OR */
/*		A.AD_PRC > (*/
/*		SELECT MAX(B.LD_LDR_POF)*/
/*		FROM BASE2 B*/
/*		WHERE B.BASE_ID = A.BASE_ID*/
/*		)*/
/*	AND A.LD_LDR_POF <> .*/
/*	)*/
/*;*/
/*QUIT;*/
/**/
/*PROC SQL;*/
/*CREATE TABLE OTPT AS */
/*SELECT DISTINCT A.SSN*/
/*	,A.BASE_LOANS*/
/*FROM BASE2 A*/
/*WHERE A.BASE_ID NOT IN (*/
/*	SELECT DISTINCT BASE_ID*/
/*	FROM EXCLUID*/
/*	)*/
/*ORDER BY A.SSN*/
/*	,A.BASE_LOANS*/
/*;*/
/*QUIT;*/
