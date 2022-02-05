******************************************;
*UTLWQ10 SSN CHANGES BY UNAUTHORIZED USERS;
******************************************;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWQ10.LWQ10R2";
FILENAME REPORT3 "&RPTLIB/ULWQ10.LWQ10R3";
FILENAME REPORT4 "&RPTLIB/ULWQ10.LWQ10R4";
FILENAME REPORTZ "&RPTLIB/ULWQ10.LWQ10RZ";
/*--------------------------------------------*
| CREATE MACRO VARIABLE WITH AUTHORIZED USERS |
*--------------------------------------------*/
%LET USERS = 'UT00034','UT00186','UT00033','UT00026','UT00465','UT00045',
	'UT00466','UT00276','UT00380','UT00482','UT00324','UT00541';
%SYSLPUT USERS = &USERS;
/*--------------------------------------------*
| CREATE MACRO VARIABLE WITH THOSE AUTHORIZED FOR CHANGES BEFORE THE LOAN WAS CREATED |
*--------------------------------------------*/
%LET AUTHUSERS = 'UT00034','UT00186','UT00033','UT00026','UT00465',
'UT00045','UT00466','UT00068','UT00472','UT00288','UT00469','UT00270',
'UT00467','UT00276','UT00380','UT00482','UT00324','UT00541','UT00245',
'UT00474','UT00286','UT00476','UT00081','UT00468','UT00323','UT00470';
%SYSLPUT AUTHUSERS = &AUTHUSERS;
LIBNAME  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;

%MACRO SQLCHECK ;
  %IF &SQLXRC NE 0  %THEN  %DO  ;
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
         RUN;
  %END;
%MEND;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE COMPSN AS
SELECT *
FROM CONNECTION TO DB2 ( 
SELECT A.DF_PRS_ID_NEW  AS NSSN /*COMPASS QUERY*/
	,A.DF_PRS_ID_OLD  AS OSSN	
	,A.DF_USR_ID_CHG_REQ AS UID	
	,DATE(A.DF_LST_DTS_PI10) AS DATE
	,CASE 
	 	WHEN A.DF_USR_ID_CHG_REQ IS NOT NULL
	 	THEN 'COMPASS'
	 END AS SYSTEM
	,DATE(G.LD_LON_GTR) AS ESTDATE
FROM (OLWHRM1.PI10_PID_CHG_LOG A
	INNER JOIN OLWHRM1.LN10_LON G
		ON A.DF_PRS_ID_NEW = G.BF_SSN)
WHERE A.DF_USR_ID_CHG_REQ NOT IN (&USERS)
/*COMMENT LINE TO GENERATE HISTORICAL REPORT*/
AND DAYS(A.DF_LST_DTS_PI10) = DAYS(CURRENT DATE) - 1 
AND G.LC_STA_LON10 = 'R'

);

CREATE TABLE OLSN AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT B.DF_CRR_PRS AS NSSN /*ONELINK QUERY*/
	,B.DF_IRR_PRS AS OSSN
	,B.DF_USR_VER AS UID
	,DATE(B.DF_ACT_DTS_PI01) AS DATE
	,CASE 
		 WHEN B.DF_USR_VER IS NOT NULL
		 THEN 'ONELINK'
	 END AS SYSTEM
	,DATE(E.AD_PRC) AS ESTDATE
FROM (OLWHRM1.PI01_PID_CHG_INF B 
	INNER JOIN OLWHRM1.GA10_LON_APP	E
		ON B.AF_APL_ID = E.AF_APL_ID)
WHERE B.DF_USR_VER NOT IN (&USERS)
/*COMMENT LINE TO GENERATE HISTORICAL REPORT*/
AND DAYS(B.DF_ACT_DTS_PI01) = DAYS(CURRENT DATE) - 1 
);

CREATE TABLE NEWBD AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT	B.DF_PRS_ID AS SSN 
	,B.DD_BRT_HST AS OLD_BD
	,C.DD_BRT AS NEW_BD
	,DATE(B.DF_LST_DTS_PD05) AS DATE
	,B.DF_LST_USR_PD05 AS USERID
	,F.PF_USR
	,E.AD_PRC AS ESTDATE
FROM OLWHRM1.PD05_PRS_NME_HST B
	INNER JOIN OLWHRM1.PD01_PDM_INF C
		ON B.DF_PRS_ID = C.DF_PRS_ID
	INNER JOIN OLWHRM1.GA01_APP D
		ON B.DF_PRS_ID = D.DF_PRS_ID_BR
	INNER JOIN OLWHRM1.GA10_LON_APP E
		ON E.AF_APL_ID = D.AF_APL_ID
		AND E.AC_PRC_STA = 'A'
	INNER JOIN OLWHRM1.US10_USR_PFL F
		ON B.DF_LST_USR_PD05 = F.PF_USR
		WHERE (B.DD_BRT_HST <> C.DD_BRT OR (B.DD_BRT_HST IS NULL AND C.DD_BRT IS NOT NULL))
		AND B.DF_LST_USR_PD05 NOT IN (&AUTHUSERS)
		/*COMMENT LINE TO GENERATE HISTORICAL REPORT*/
		AND DAYS(DATE(B.DF_LST_DTS_PD05)) = DAYS(CURRENT DATE) - 1 
		AND B.DF_LST_USR_PD05 NOT IN (
			SELECT PF_USR 
			FROM OLWHRM1.US10_USR_PFL
			WHERE PC_SER_DPT IN ('AUX','LOR')
			AND PD_USR_IN_DPT_END IS NULL
		)
);

CREATE TABLE NEWNM AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT B.DF_PRS_ID AS SSN 
	,RTRIM(B.DM_PRS_1_HST) || ' ' || RTRIM(B.DM_PRS_MID_HST) || ' ' || RTRIM(B.DM_PRS_LST_HST) AS OLD_NAME
	,RTRIM(C.DM_PRS_1) || ' ' || RTRIM(C.DM_PRS_MID) || ' ' || RTRIM(C.DM_PRS_LST) AS NEW_NAME
	,DATE(B.DF_LST_DTS_PD05) AS DATE
	,B.DF_LST_USR_PD05 AS USERID
	,F.PF_USR
	,E.AD_PRC AS ESTDATE
FROM OLWHRM1.PD05_PRS_NME_HST B
	INNER JOIN OLWHRM1.PD01_PDM_INF C
		ON B.DF_PRS_ID = C.DF_PRS_ID
	INNER JOIN OLWHRM1.GA01_APP D
		ON B.DF_PRS_ID = D.DF_PRS_ID_BR
	INNER JOIN OLWHRM1.GA10_LON_APP E
		ON E.AF_APL_ID = D.AF_APL_ID
		AND E.AC_PRC_STA = 'A'
	INNER JOIN OLWHRM1.US10_USR_PFL F
		ON B.DF_LST_USR_PD05 = F.PF_USR
WHERE B.DM_PRS_1_HST || ' ' || B.DM_PRS_MID_HST || ' ' || B.DM_PRS_LST_HST <> C.DM_PRS_1 || ' ' || C.DM_PRS_MID || ' '
/*COMMENT LINE TO GENERATE HISTORICAL REPORT*/
	AND DAYS(DATE(B.DF_LST_DTS_PD05)) = DAYS(CURRENT DATE) - 1 
	AND B.DF_LST_USR_PD05 NOT IN (&AUTHUSERS)
	AND B.DF_LST_USR_PD05 NOT IN (
		SELECT PF_USR 
		FROM OLWHRM1.US10_USR_PFL
		WHERE PC_SER_DPT IN ('AUX','LOR')
		AND PD_USR_IN_DPT_END IS NULL
	)
);
DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
/*QUIT;*/

ENDRSUBMIT;
DATA SSNCHNG;
SET WORKLOCL.COMPSN WORKLOCL.OLSN;
RUN;
DATA NEWBD;
SET WORKLOCL.NEWBD;
RUN;
DATA NEWNM;
SET WORKLOCL.NEWNM;
RUN;
DATA SSNCHNG(DROP=ESTDATE);
SET SSNCHNG;
IF UID = 'LPX2M' THEN DELETE;
IF ESTDATE > DATE THEN DELETE;
RUN;
DATA NEWBD(DROP=ESTDATE);
SET NEWBD;
IF ESTDATE > DATE THEN DELETE;
RUN;
DATA NEWNM(DROP=ESTDATE);
SET NEWNM;
IF ESTDATE > DATE THEN DELETE;
RUN;
PROC SORT DATA=SSNCHNG NODUPKEY;
BY NSSN ;
RUN;
PROC SORT DATA=NEWBD NODUPKEY;
BY SSN ;
RUN;
PROC SORT DATA=NEWNM NODUPKEY;
BY SSN ;
RUN;
PROC SORT DATA=SSNCHNG;
BY DATE SYSTEM NSSN ;
RUN;
PROC SORT DATA=NEWBD;
BY DATE USERID SSN ;
RUN;
PROC SORT DATA=NEWNM;
BY DATE USERID SSN ;
RUN;

DATA _NULL_;
SET SSNCHNG ;
LENGTH DESCRIPTION $600.;
USER = UID;
ACT_DT = DATE;
DESCRIPTION = CATX(',',
	'NEW SSN = '|| NSSN,
	'OLD SSN = '|| OSSN,
	'SYSTEM = '|| SYSTEM   
);
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT USER $10. ;
FORMAT ACT_DT MMDDYY10. ;
FORMAT DESCRIPTION $600. ;
FORMAT DATE MMDDYY10.;
IF _N_ = 1 THEN DO;
	PUT "USER,ACT_DT,DESCRIPTION";
END;
DO;
   PUT USER $ @;
   PUT ACT_DT @;
   PUT DESCRIPTION $ ;
END;
RUN;
PROC SQL;
CREATE TABLE NEWNM1 AS
SELECT A.*
FROM NEWNM A
WHERE A.SSN NOT IN (SELECT NSSN FROM SSNCHNG);
QUIT;
DATA _NULL_;
SET NEWNM1 ;
LENGTH DESCRIPTION $600.;
USER = USERID;
ACT_DT = DATE;
DESCRIPTION = CATX(',',
	'NEW NAME = '|| NEW_NAME,
	'OLD NAME = '|| OLD_NAME
);
FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT USER $10. ;
FORMAT ACT_DT MMDDYY10. ;
FORMAT DESCRIPTION $600. ;
IF _N_ = 1 THEN DO;
	PUT "USER,ACT_DT,DESCRIPTION";
END;
DO;
   PUT USER $ @;
   PUT ACT_DT @;
   PUT DESCRIPTION $ ;
END;
RUN;
PROC SQL;
CREATE TABLE NEWBD1 AS
SELECT A.*
FROM NEWBD A
WHERE A.SSN NOT IN (SELECT NSSN FROM SSNCHNG)
	AND A.SSN NOT IN (SELECT SSN FROM NEWNM);
QUIT;
DATA _NULL_;
SET NEWBD1 ;
LENGTH DESCRIPTION $600.;
USER = USERID;
ACT_DT = DATE;
DESCRIPTION = CATX(',',
	'NEW B-DAY = '|| NEW_BD ,
	'OLD B-DAY = '|| OLD_BD	
);
FILE REPORT4 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT USER $10. ;
FORMAT ACT_DT MMDDYY10. ;
FORMAT DESCRIPTION $600. ;
FORMAT NEW_BD MMDDYY10. ;
FORMAT OLD_BD MMDDYY10. ;
IF _N_ = 1 THEN DO;
	PUT "USER,ACT_DT,DESCRIPTION";
END;
DO;
   PUT USER $ @;
   PUT ACT_DT @;
   PUT DESCRIPTION $ ;
/*   PUT 'NEW B-DAY = ' NEW_BD @;*/
/*   PUT 'OLD B-DAY = ' OLD_BD $;*/
END;
RUN;