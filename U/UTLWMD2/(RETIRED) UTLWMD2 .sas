/*%LET RPTLIB = %SYSGET(reportdir);*/
/*%LET PROGREVW = /sas/whse/progrevw;*/
%LET RPTLIB = T:\SAS;
%LET PROGREVW = Q:\Support Services\Test Files\SAS\PROGREVW;
FILENAME REPORTZ "&RPTLIB/ULWMD2.LWMD2RZ";
FILENAME REPORT2 "&RPTLIB/ULWMD2.LWMD2R2";
FILENAME REPORT3 "&RPTLIB/ULWMD2.LWMD2R3";
FILENAME REPORT4 "&RPTLIB/ULWMD2.LWMD2R4";
FILENAME REPORT5 "&RPTLIB/ULWMD2.LWMD2R5";
FILENAME REPORT6 "&RPTLIB/ULWMD2.LWMD2R6";

LIBNAME  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

LIBNAME PROGREVW "&PROGREVW";

PROC SQL NOPRINT;
	SELECT 
		"'"||STRIP(LENDER_ID)||"'" INTO :EXCL_LIST SEPARATED BY ","
	FROM 
		PROGREVW.LENDER_GROUP_LENDERS
	WHERE 
		LENDER_GROUP_ID = 1
	;
QUIT;
%PUT &EXCL_LIST;

%SYSLPUT EXCL_LIST = &EXCL_LIST;

RSUBMIT;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';

DATA _NULL_;
SET SAS_TAB.LASTRUN_JOBS;
IF JOB = 'UTLWMD2' THEN CALL SYMPUT('LAST_RUN',"'"||TRIM(LEFT(PUT(LAST_RUN,DATE10.)))|| "'D");
/*	CALL SYMPUT('LAST_RUN',"'"||TRIM(LEFT(PUT(TODAY()-10,DATE10.)))|| "'D");*/
RUN;

PROC SQL;
CREATE TABLE ADDRESS_UPDATE AS
SELECT B.DF_PRS_ID
	,B.DF_LST_DTS_PD30 AS TIMESTA FORMAT=DATETIME.
		,b.dd_ver_adr
	,B.DI_VLD_ADR
	,B.DX_STR_ADR_1	AS ADDRESS1
	,B.DX_STR_ADR_2	AS ADDRESS2
	,DM_CT AS CITY
	,DC_DOM_ST AS STATE
	,DF_ZIP_CDE	AS ZIP
	,DM_FGN_CNY AS COUNTRY
FROM DLGSUTWH.PD30_PRS_ADR B
INNER JOIN (SELECT DF_PRS_ID
			,MAX(DN_ADR_SEQ) AS DN_ADR_SEQ
		FROM DLGSUTWH.PD31_PRS_INA A
		GROUP BY DF_PRS_ID) A
	ON B.DF_PRS_ID = A.DF_PRS_ID
INNER JOIN DLGSUTWH.PD31_PRS_INA C
	ON A.DF_PRS_ID = C.DF_PRS_ID
	AND A.DN_ADR_SEQ = C.DN_ADR_SEQ
JOIN DLGSUTWH.LN10_LON LN10
	ON B.DF_PRS_ID = LN10.BF_SSN
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.LF_LON_CUR_OWN NOT IN (&EXCL_LIST)
INNER JOIN DLGSUTWH.PD04_ADR_PHN_HST PD04
	ON PD04.DF_PRS_ID = LN10.BF_SSN
WHERE b.dd_sta_pdem30 >= &last_run
	AND DATEPART(B.DF_LST_DTS_PD30) >= &LAST_RUN
	AND SUBSTR(B.DF_PRS_ID,1,1) ^= 'P'
	AND C.DD_CRT_PD31 = B.DD_STA_PDEM30
;

CREATE TABLE PHONE_H AS
SELECT B.DF_PRS_ID
	,DF_LST_DTS_PD42 AS TIMESTA FORMAT=DATETIME.
		,dd_phn_ver
	,DI_PHN_VLD
	,CASE
			WHEN DC_ALW_ADL_PHN = 'Q' THEN 'L'
			WHEN DC_ALW_ADL_PHN IN ('U','X') THEN 'T'
			ELSE DC_ALW_ADL_PHN
		END AS DC_ALW_ADL_PHN
	,trim(DN_DOM_PHN_ARA)	|| trim(DN_DOM_PHN_XCH) || trim(DN_DOM_PHN_LCL)
		|| trim(DN_PHN_XTN) AS PHONE
	,trim(DN_FGN_PHN_INL) || TRIM(DN_FGN_PHN_CNY) || TRIM(DN_FGN_PHN_CT)
		|| TRIM(DN_FGN_PHN_LCL) AS FOR_PHONE
FROM DLGSUTWH.PD42_PRS_PHN B
INNER JOIN (SELECT DF_PRS_ID
			,MAX(DN_PHN_SEQ) AS DN_PHN_SEQ
		FROM DLGSUTWH.PD41_PHN_HST A
		GROUP BY DF_PRS_ID) A
	ON B.DF_PRS_ID = A.DF_PRS_ID
INNER JOIN DLGSUTWH.PD41_PHN_HST C
	ON A.DF_PRS_ID = C.DF_PRS_ID
	AND A.DN_PHN_SEQ = C.DN_PHN_SEQ
LEFT OUTER JOIN DLGSUTWH.AY10_BR_LON_ATY D
	ON A.DF_PRS_ID = D.BF_SSN
	AND D.PF_REQ_ACT IN ('TCPAB','APRLB')
	AND D.LD_ATY_REQ_RCV >= &LAST_RUN
JOIN DLGSUTWH.LN10_LON LN10
	ON B.DF_PRS_ID = LN10.BF_SSN
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.LF_LON_CUR_OWN NOT IN (&EXCL_LIST)
INNER JOIN DLGSUTWH.PD04_ADR_PHN_HST PD04
	ON PD04.DF_PRS_ID = LN10.BF_SSN
WHERE B.DC_PHN = 'H'
	AND SUBSTR(B.DF_PRS_ID,1,1) ^= 'P'
	AND DATEPART(B.DF_LST_DTS_PD42) >= &LAST_RUN
	AND C.DD_CRT_41 = B.DD_CRT_PD42
	AND B.DD_CRT_PD42 >= &LAST_RUN
	AND D.BF_SSN IS NULL
;

CREATE TABLE PHONE_A AS
SELECT B.DF_PRS_ID
	,DF_LST_DTS_PD42 AS TIMESTA FORMAT=DATETIME.
		,DD_PHN_VER
	,DI_PHN_VLD
	,CASE
			WHEN DC_ALW_ADL_PHN = 'Q' THEN 'L'
			WHEN DC_ALW_ADL_PHN IN ('U','X') THEN 'T'
			ELSE DC_ALW_ADL_PHN
		END AS DC_ALW_ADL_PHN
	,TRIM(DN_DOM_PHN_ARA)	|| TRIM(DN_DOM_PHN_XCH) || TRIM(DN_DOM_PHN_LCL)
		|| TRIM(DN_PHN_XTN) AS ALTPHONE
	,TRIM(DN_FGN_PHN_INL) || TRIM(DN_FGN_PHN_CNY) || TRIM(DN_FGN_PHN_CT)
		|| TRIM(DN_FGN_PHN_LCL) AS FOR_ALTPHONE
FROM DLGSUTWH.PD42_PRS_PHN B
INNER JOIN (SELECT DF_PRS_ID
			,MAX(DN_PHN_SEQ) AS DN_PHN_SEQ
		FROM DLGSUTWH.PD41_PHN_HST A
		GROUP BY DF_PRS_ID) A
	ON B.DF_PRS_ID = A.DF_PRS_ID
INNER JOIN DLGSUTWH.PD41_PHN_HST C
	ON A.DF_PRS_ID = C.DF_PRS_ID
	AND A.DN_PHN_SEQ = C.DN_PHN_SEQ
LEFT OUTER JOIN DLGSUTWH.AY10_BR_LON_ATY D
	ON A.DF_PRS_ID = D.BF_SSN
	AND D.PF_REQ_ACT IN ('TCPAB','APRLB')
	AND D.LD_ATY_REQ_RCV >= &LAST_RUN
JOIN DLGSUTWH.LN10_LON LN10
	ON B.DF_PRS_ID = LN10.BF_SSN
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.LF_LON_CUR_OWN NOT IN (&EXCL_LIST)
INNER JOIN DLGSUTWH.PD04_ADR_PHN_HST PD04
	ON PD04.DF_PRS_ID = LN10.BF_SSN
WHERE B.DC_PHN = 'A'
	AND SUBSTR(B.DF_PRS_ID,1,1) ^= 'P'
	AND DATEPART(B.DF_LST_DTS_PD42) >= &LAST_RUN
	AND C.DD_CRT_41 = B.DD_CRT_PD42
	AND B.DD_CRT_PD42 >= &LAST_RUN
	AND D.BF_SSN IS NULL
;

CREATE TABLE EML_UPDATE AS
SELECT B.DF_PRS_ID
	,DF_LST_DTS_PD32 AS TIMESTA FORMAT=DATETIME.
		,dd_ver_adr_eml
	,DC_ADR_EML
	,DI_VLD_ADR_EML
	,DX_ADR_EML
	,dc_sta_pd32
FROM DLGSUTWH.PD32_PRS_ADR_EML B
JOIN DLGSUTWH.LN10_LON LN10
	ON B.DF_PRS_ID = LN10.BF_SSN
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.LF_LON_CUR_OWN NOT IN (&EXCL_LIST)
INNER JOIN DLGSUTWH.PD04_ADR_PHN_HST PD04
	ON PD04.DF_PRS_ID = LN10.BF_SSN
WHERE DATEPART(B.DF_LST_DTS_PD32) >= &LAST_RUN
	AND SUBSTR(B.DF_PRS_ID,1,1) ^= 'P'

;
QUIT;
PROC SORT DATA=ADDRESS_UPDATE; BY DF_PRS_ID TIMESTA; RUN;
PROC SORT DATA=PHONE_H; BY DF_PRS_ID TIMESTA; RUN;
PROC SORT DATA=PHONE_A; BY DF_PRS_ID TIMESTA; RUN;
PROC SORT DATA=EML_UPDATE; BY DF_PRS_ID DC_STA_PD32; RUN;



/*DATA SAS_TAB.LASTRUN_JOBS;*/
/*	SET SAS_TAB.LASTRUN_JOBS;*/
/*	IF JOB = 'UTLWMD2' THEN LAST_RUN = TODAY();*/
/*RUN;*/

ENDRSUBMIT;
DATA ADDRESS_UPDATE; SET WORKLOCL.ADDRESS_UPDATE; RUN;
DATA PHONE_H; SET WORKLOCL.PHONE_H; RUN;
DATA PHONE_A; SET WORKLOCL.PHONE_A; RUN;
DATA EML_UPDATE; SET WORKLOCL.EML_UPDATE; RUN;

DATA ADDRESS_UPDATE foreign(drop=PHONE ALTPHONE EMAIL);
SET ADDRESS_UPDATE;
BY DF_PRS_ID ;
IF LAST.DF_PRS_ID;
CALL MISSING(PHONE,ALTPHONE,EMAIL);
	ADDRESS1 = TRANWRD(ADDRESS1,',',' ');
	ADDRESS2 = TRANWRD(ADDRESS2,',',' ');
if COUNTRY ^= '' then output foreign;
else output  ADDRESS_UPDATE;
RUN;

DATA PHONE_H addh(drop=Address1 Address2 City State Zip Country AltPhone Email);
SET PHONE_H;  
BY DF_PRS_ID ;
IF LAST.DF_PRS_ID;
CALL MISSING(ADDRESS1,ADDRESS2,CITY,STATE,ZIP,COUNTRY,EMAIL);
	if phone = '' then Phone = 'No Phone';
if FOR_PHONE ^= '' then output addh;
else output PHONE_H;
RUN;

DATA PHONE_A adda(drop=Address1 Address2 City State Zip Country Phone Email);
SET PHONE_A;
BY DF_PRS_ID  ;
IF LAST.DF_PRS_ID;
CALL MISSING(ADDRESS1,ADDRESS2,CITY,STATE,ZIP,COUNTRY,EMAIL);
	if AltPhone = '' then AltPhone = 'No Phone';
if FOR_AltPHONE ^= '' then output adda;
else output PHONE_A;
RUN;
DATA foreign(drop=phone altphone email DI_PHN_VLD);
merge foreign addh adda;
BY DF_PRS_ID ;
if DI_VLD_ADR = '' then di_vld_adr = di_phn_vld;
run;

DATA EML_UPDATE;
SET EML_UPDATE;
BY DF_PRS_ID ;
IF first.DF_PRS_ID;
CALL MISSING(ADDRESS1,ADDRESS2,CITY,STATE,ZIP,COUNTRY,PHONE,ALTPHONE);
RUN;

%MACRO REPORTS(REP,SET,QUEUE,COMMENT);
DATA _NULL_;
SET &SET ;
TARGET_ID = DF_PRS_ID;
QUEUE_NAME = &QUEUE;
INSTITUTION_ID = '';
INSTITUTION_TYPE = '';
DATE_DUE = '';
TIME_DUE = '';
COMMENTS = &COMMENT;
FILE REPORT&REP DELIMITER=',' DSD DROPOVER LRECL=32767;
DO;
   PUT TARGET_ID @;
   PUT QUEUE_NAME @;
   PUT INSTITUTION_ID @;
   PUT INSTITUTION_TYPE @;
   PUT DATE_DUE @;
   PUT TIME_DUE @;
   PUT COMMENTS $;
END;
RUN;
%MEND;
%REPORTS(2,ADDRESS_UPDATE,'XDEMOG',%STR(TRIM(Address1)||','||TRIM(Address2)||','||TRIM(City)||','
	||TRIM(State)||','||TRIM(Zip)||','||TRIM(Country)||',,,,'||TRIM(DI_VLD_ADR)));
%REPORTS(3,PHONE_H,'XDEMOG',%STR(',,,,,,'||TRIM(Phone)||',,,'||TRIM(DI_PHN_VLD)||','||TRIM(DC_ALW_ADL_PHN)));
%REPORTS(4,PHONE_A,'XDEMOG',%STR(',,,,,,,'||TRIM(ALTPhone)||',,'||TRIM(DI_PHN_VLD)||','||TRIM(DC_ALW_ADL_PHN)));
%REPORTS(5,EML_UPDATE,'XDEMOE',%STR(',,,,,,,,'||TRIM(DX_ADR_EML)||','||TRIM(DI_VLD_ADR_EML)));
%REPORTS(6,FOREIGN,'FRGNDEMO',%STR(TRIM(ADDRESS1)||','||TRIM(ADDRESS2)||','||TRIM(CITY)||','
	||',,'||TRIM(ZIP)||','||TRIM(COUNTRY)||','||TRIM(FOR_PHONE)||','||TRIM(FOR_ALTPHONE)||',,'
	||TRIM(DI_VLD_ADR)));
