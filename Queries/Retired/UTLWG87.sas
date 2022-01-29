LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);

FILENAME REPORT2 "&RPTLIB/ULWG87.LWG87R2";
FILENAME REPORT3 "&RPTLIB/ULWG87.LWG87R3";
FILENAME REPORT4 "&RPTLIB/ULWG87.LWG87R4";
FILENAME REPORTZ "&RPTLIB/ULWG87.LWG87RZ";

/*FILENAME REPORT2 "T:\SAS\ULWG87.LWG87R2";*/
/*FILENAME REPORT3 "T:\SAS\ULWG87.LWG87R3";*/
/*FILENAME REPORT4 "T:\SAS\ULWG87.LWG87R4";*/
/**/
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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT DISTINCT
A.AF_APL_ID
,B.AF_APL_ID AS AF_APL_ID2
	,B.DF_PRS_ID_BR AS SSN
	,D.DM_PRS_1
	,D.DM_PRS_LST
	,C.BM_RFR_LST 
	,C.BM_RFR_1
	,C.BM_RFR_MID
	,C.BX_RFR_STR_ADR_1 
	,C.BX_RFR_STR_ADR_2
	,C.BM_RFR_CT
	,C.BC_RFR_ST 
	,C.BM_RFR_FGN_CNY
	,C.BF_RFR_ZIP 
	,C.BI_VLD_ADR
	,C.BN_RFR_DOM_PHN
	,C.BI_DOM_PHN_VLD
	,C.BN_RFR_ALT_PHN
	,C.DF_PRS_ID_RFR
	,CASE 
	 WHEN C.BC_RFR_REL_BR = 'OT'
	 THEN '01'
	 WHEN C.BC_RFR_REL_BR = 'O'
	 THEN '01'
	 WHEN C.BC_RFR_REL_BR = 'PA'
	 THEN '02'
	 WHEN C.BC_RFR_REL_BR = 'P'
	 THEN '02'	
	 WHEN C.BC_RFR_REL_BR = 'RE'
	 THEN '03'
	 WHEN C.BC_RFR_REL_BR = 'R'
	 THEN '03'
	 WHEN C.BC_RFR_REL_BR = 'SP'
	 THEN '06'
	 WHEN C.BC_RFR_REL_BR = 'M'
	 THEN '06'
	 WHEN C.BC_RFR_REL_BR = 'SI'
	 THEN '07'
	 WHEN C.BC_RFR_REL_BR = 'S'
	 THEN '07'
	 WHEN C.BC_RFR_REL_BR = 'RM'
	 THEN '08'
	 WHEN C.BC_RFR_REL_BR = 'NE'
	 THEN '09'
	 WHEN C.BC_RFR_REL_BR = 'N'
	 THEN '01'
	 WHEN C.BC_RFR_REL_BR = 'FR'
	 THEN '11'
	 WHEN C.BC_RFR_REL_BR = 'F'
	 THEN '11'
	 WHEN C.BC_RFR_REL_BR = 'GU'
	 THEN '12'
	 WHEN C.BC_RFR_REL_BR = 'G'
	 THEN '12'
	 WHEN C.BC_RFR_REL_BR = 'EM'
	 THEN '19'
	 WHEN C.BC_RFR_REL_BR = 'E'
	 THEN '19'
	 END AS BC_RFR_REL_BR
	
	,D.DX_STR_ADR_1

	
	
	
	
	

FROM	OLWHRM1.GA10_LON_APP A
INNER JOIN OLWHRM1.GA01_APP BA
ON A.AF_APL_ID = BA.AF_APL_ID
INNER JOIN OLWHRM1.BR04_BR_REF_APL B
ON A.AF_APL_ID = B.AF_APL_ID
OR BA.AF_BS_MPN_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.BR03_BR_REF C
ON B.DF_PRS_ID_BR = C.DF_PRS_ID_BR
AND B.DF_PRS_ID_RFR = C.DF_PRS_ID_RFR
AND C.BC_STA_BR03 = 'A'


INNER JOIN OLWHRM1.PD01_PDM_INF D
ON B.DF_PRS_ID_BR = D.DF_PRS_ID


WHERE AC_PRC_STA = 'A' 
AND DAYS(AD_PRC) = DAYS(CURRENT DATE) - 1 
AND AA_GTE_LON_AMT > 0


FOR READ ONLY WITH UR

);
DISCONNECT FROM DB2;

%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;
%sqlcheck;
quit;

/*ENDRSUBMIT;*/
/**/
/*DATA DEMO; SET WORKLOCL.DEMO; RUN;*/

PROC SORT DATA=DEMO;
BY AF_APL_ID;
RUN;

PROC SQL;
CREATE TABLE NoRef AS
SELECT DISTINCT AF_APL_ID
FROM DEMO
WHERE AF_APL_ID NOT IN (SELECT DISTINCT AF_APL_ID2 FROM DEMO)
;
QUIT;

PROC SQL;
CREATE TABLE UseBaseRef AS
SELECT DISTINCT *
FROM DEMO
WHERE AF_APL_ID IN (SELECT AF_APL_ID FROM NoRef)
;
QUIT;

PROC SQL;
CREATE TABLE UseRef AS
SELECT DISTINCT *
FROM DEMO
WHERE AF_APL_ID = AF_APL_ID2
;
QUIT;

DATA DEMO (DROP=AF_APL_ID AF_APL_ID2);
SET UseRef UseBaseRef;
RUN;

PROC SORT DATA=DEMO NODUPKEY;
BY SSN DM_PRS_1 DM_PRS_LST BM_RFR_LST BM_RFR_1 BM_RFR_MID DF_PRS_ID_RFR;
RUN;


DATA DEMO2 MISADDR;
SET DEMO;
FORMAT DOOP $1.;
IF SUBSTR(BM_RFR_LST,1,4) = SUBSTR(DM_PRS_LST,1,4) 
	AND SUBSTR(BM_RFR_1,1,4) = SUBSTR(DM_PRS_1,1,4)
	AND SUBSTR(BX_RFR_STR_ADR_1,1,4) = SUBSTR(DX_STR_ADR_1,1,4)
THEN DOOP = 'Y';
ELSE DOOP = 'N';

IF BX_RFR_STR_ADR_1 = ''  OR BX_RFR_STR_ADR_1 = 'N/A' 
OR BM_RFR_CT = '' OR BM_RFR_CT = 'N/A' 
OR BC_RFR_ST = '' OR BC_RFR_ST = 'N/A' 
OR BF_RFR_ZIP = '' OR BF_RFR_ZIP = 'N/A' THEN OUTPUT MISADDR;
ELSE OUTPUT DEMO2;
RUN;

DATA ADDREF (DROP=DOOP DX_STR_ADR_1) DUPS (DROP=DOOP DX_STR_ADR_1);
SET DEMO2;
IF DOOP = 'Y' THEN OUTPUT DUPS;
ELSE OUTPUT ADDREF;
RUN;

DATA MISADDR (DROP=DOOP DX_STR_ADR_1);
SET MISADDR;
RUN;

%MACRO TOFILE(TB, RPT);
data _null_;
set  &TB;

file &RPT delimiter=',' DSD DROPOVER lrecl=32767;
    format SSN $9. ;
    format DM_PRS_1 $12. ;
    format DM_PRS_LST $35. ;
    format BM_RFR_LST $35. ;
    format BM_RFR_1 $12. ;
    format BM_RFR_MID $1. ;
    format BX_RFR_STR_ADR_1 $35. ;
    format BX_RFR_STR_ADR_2 $35. ;
    format BM_RFR_CT $30. ;
    format BC_RFR_ST $2. ;
    format BM_RFR_FGN_CNY $25. ;
    format BF_RFR_ZIP $14. ;
    format BI_VLD_ADR $1. ;
    format BN_RFR_DOM_PHN $10. ;
    format BI_DOM_PHN_VLD $1. ;
	format BN_RFR_ALT_PHN $10. ;
	format DF_PRS_ID_RFR $9. ;
    format BC_RFR_REL_BR $2. ;

if _n_ = 1 then        /* write column names */
 do;
   put
    'SSN'
    ','
    'DM_PRS_1'
    ','
    'DM_PRS_LST'
    ','
    'BM_RFR_LST'
    ','
    'BM_RFR_1'
    ','
    'BM_RFR_MID'
    ','
    'BX_RFR_STR_ADR_1'
    ','
    'BX_RFR_STR_ADR_2'
    ','
    'BM_RFR_CT'
    ','
    'BC_RFR_ST'
    ','
    'BM_RFR_FGN_CNY'
    ','
    'BF_RFR_ZIP'
    ','
    'BI_VLD_ADR'
    ','
    'BN_RFR_DOM_PHN'
    ','
    'BI_DOM_PHN_VLD'
    ','
	'BN_RFR_ALT_PHN'
	','
	'DF_PRS_ID_RFR'
	','
    'BC_RFR_REL_BR'
   ;
 end;
 do;

 put SSN $ @;
 put DM_PRS_1 $ @;
 put DM_PRS_LST $ @;
 put BM_RFR_LST $ @;
 put BM_RFR_1 $ @;
 put BM_RFR_MID $ @;
 put BX_RFR_STR_ADR_1 $ @;
 put BX_RFR_STR_ADR_2 $ @;
 put BM_RFR_CT $ @;
 put BC_RFR_ST $ @;
 put BM_RFR_FGN_CNY $ @;
 put BF_RFR_ZIP $ @;
 put BI_VLD_ADR $ @;
 put BN_RFR_DOM_PHN $ @;
 put BI_DOM_PHN_VLD $ @;
 put BN_RFR_ALT_PHN $ @;
 put DF_PRS_ID_RFR $ @;
 put BC_RFR_REL_BR $ ;

   ;
 end;

run;
%MEND TOFILE;

%TOFILE(ADDREF, REPORT2);
%TOFILE(DUPS, REPORT3);
%TOFILE(MISADDR, REPORT4);
