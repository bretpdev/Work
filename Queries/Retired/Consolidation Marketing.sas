/*consolidation marketing*/
options symbolgen;

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;

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
CREATE TABLE MARKET AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT DISTINCT
	C.DF_PRS_ID_BR AS SSN
	,DL.AF_APL_ID
	,DL.AF_APL_ID_SFX
	,DL.AD_DSB_ADJ
	,DL.AC_DSB_ADJ
	,DL.AN_DSB_SEQ
	,CASE
		WHEN DL.AC_DSB_ADJ = 'U' THEN DL.AA_DSB_ADJ * (-1)
		WHEN DL.AC_DSB_ADJ = 'C' THEN DL.AA_DSB_ADJ * (-1)
		WHEN DL.AC_DSB_ADJ = 'P' THEN DL.AA_DSB_ADJ * (-1)
		ELSE DL.AA_DSB_ADJ
	END AS AA_DSB_ADJ
	,A.AC_LON_STA_TYP
	
	,D.DF_SPE_ACC_ID
	,D.DM_PRS_1
	,D.DM_PRS_MID
	,D.DM_PRS_LST
	,D.DX_STR_ADR_1
	,D.DX_STR_ADR_2
	,D.DM_CT
	,D.DC_DOM_ST
	,D.DM_FGN_CNY
	,D.DF_ZIP

	,D.DI_VLD_ADR

FROM OLWHRM1.GA14_LON_STA A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
	AND A.AF_APL_ID_SFX = B.AF_APL_ID_SFX
	AND B.AA_CUR_PRI > 0
	AND B.AF_CUR_LON_SER_AGY IN ('700121', '700126')
	AND B.AC_LON_TYP <> 'CL'
INNER JOIN OLWHRM1.GA01_APP C
	ON A.AF_APL_ID = C.AF_APL_ID
INNER JOIN OLWHRM1.PD01_PDM_INF D
	ON C.DF_PRS_ID_BR = D.DF_PRS_ID
	AND D.DC_ADR = 'L'
/***** DISBURSEMENT TABLES ******/
LEFT OUTER JOIN 
	(SELECT AF_APL_ID
		,AF_APL_ID_SFX
		,AD_DSB_ADJ
		,AC_DSB_ADJ
		,AA_DSB_ADJ
		,AN_DSB_SEQ
	FROM OLWHRM1.GA11_LON_DSB_ATY
	WHERE (AC_DSB_ADJ = 'A' OR AC_DSB_ADJ = 'U' OR AC_DSB_ADJ = 'C' OR AC_DSB_ADJ = 'P' 
		OR (AC_DSB_ADJ = 'E' AND DAYS(AD_DSB_ADJ) > DAYS(CURRENT DATE))
		)              
		AND AC_DSB_ADJ_STA = 'A' 
	) DL
	ON DL.AF_APL_ID = A.AF_APL_ID
	AND DL.AF_APL_ID_SFX = A.AF_APL_ID_SFX


WHERE A.AC_LON_STA_TYP IN ('CR', 'DA', 'FB', 'IA', 'ID', 'IG', 'IM', 'RF', 'RP')
AND A.AC_STA_GA14 = 'A'

FOR READ ONLY WITH UR

);



CREATE TABLE OPENLOANS AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT DISTINCT
	C.DF_PRS_ID_BR AS SSN

FROM OLWHRM1.GA14_LON_STA A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
	AND A.AF_APL_ID_SFX = B.AF_APL_ID_SFX
	AND B.AA_CUR_PRI > 0
	AND B.AF_CUR_LON_SER_AGY IN ('700121', '700126')
	AND B.AC_LON_TYP <> 'CL'
INNER JOIN OLWHRM1.GA01_APP C
	ON A.AF_APL_ID = C.AF_APL_ID
INNER JOIN OLWHRM1.PD01_PDM_INF D
	ON C.DF_PRS_ID_BR = D.DF_PRS_ID
	AND D.DC_ADR = 'L'
	AND D.DI_VLD_ADR = 'Y'

WHERE A.AC_LON_STA_TYP IN ('CR', 'DA', 'FB', 'IA', 'ID', 'IG', 'IM', 'RF', 'RP')
AND A.AC_STA_GA14 = 'A'
FOR READ ONLY WITH UR

);


DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/


PROC SQL;
CREATE TABLE MARKET2 AS
SELECT 
	SSN
	,AF_APL_ID
	,AF_APL_ID_SFX
	,AN_DSB_SEQ
	,AD_DSB_ADJ
	,SUM(AA_DSB_ADJ) AS AA_DSB_ADJ
	,DF_SPE_ACC_ID
	,DM_PRS_1
	,DM_PRS_MID
	,DM_PRS_LST
	,DX_STR_ADR_1
	,DX_STR_ADR_2
	,DM_CT
	,DC_DOM_ST
	,DM_FGN_CNY
	,DF_ZIP
	,DI_VLD_ADR

FROM MARKET
GROUP BY SSN, AF_APL_ID, AF_APL_ID_SFX, AN_DSB_SEQ, AD_DSB_ADJ
	,DF_SPE_ACC_ID
	,DM_PRS_1
	,DM_PRS_MID
	,DM_PRS_LST
	,DX_STR_ADR_1
	,DX_STR_ADR_2
	,DM_CT
	,DC_DOM_ST
	,DM_FGN_CNY
	,DF_ZIP
	,DI_VLD_ADR
;
QUIT;

PROC SQL;
CREATE TABLE MARKET3 AS
SELECT DISTINCT
	SSN
	,DF_SPE_ACC_ID
	,DM_PRS_1
	,DM_PRS_MID
	,DM_PRS_LST
	,DX_STR_ADR_1
	,DX_STR_ADR_2
	,DM_CT
	,DC_DOM_ST
	,DM_FGN_CNY
	,DF_ZIP
	,DI_VLD_ADR
	,CASE 
		WHEN DC_DOM_ST = 'FC' THEN ''
		ELSE DC_DOM_ST
	END AS STATE_IND
	,'MA4117' AS COST_CENTER_CODE
FROM MARKET2
WHERE AA_DSB_ADJ > 0
;
QUIT;

DATA MARKET4 INVALIDADDR;
SET MARKET3;
IF DI_VLD_ADR = 'Y' THEN OUTPUT MARKET4;
ELSE OUTPUT INVALIDADDR;
RUN;

PROC SORT DATA=MARKET4;
BY STATE_IND DM_PRS_LST DM_PRS_1 DM_PRS_MID;
RUN;

DATA MARKET4 (DROP=DI_VLD_ADR);
SET MARKET4;
RUN;

ENDRSUBMIT;

DATA MARKET4; SET WORKLOCL.MARKET4; RUN;
DATA OPENLOANS; SET WORKLOCL.OPENLOANS; RUN;
DATA INVALIDADDR; SET WORKLOCL.INVALIDADDR; RUN;

data _null_;
set  WORK.Market4;

file 'T:\SAS\ConsolidationMarketing.txt' delimiter=',' DSD DROPOVER lrecl=32767;
   format SSN $9. ;
   format DF_SPE_ACC_ID $10. ;
   format DM_PRS_1 $12. ;
   format DM_PRS_MID $1. ;
   format DM_PRS_LST $35. ;
   format DX_STR_ADR_1 $35. ;
   format DX_STR_ADR_2 $35. ;
   format DM_CT $30. ;
   format DC_DOM_ST $2. ;
   format DM_FGN_CNY $25. ;
   format DF_ZIP $14. ;
   format STATE_IND $2. ;
   format COST_CENTER_CODE $6. ;
if _n_ = 1 then        /* write column names */
 do;
   put
   'SSN'
   ','
   'DF_SPE_ACC_ID'
   ','
   'DM_PRS_1'
   ','
   'DM_PRS_MID'
   ','
   'DM_PRS_LST'
   ','
   'DX_STR_ADR_1'
   ','
   'DX_STR_ADR_2'
   ','
   'DM_CT'
   ','
   'DC_DOM_ST'
   ','
   'DM_FGN_CNY'
   ','
   'DF_ZIP'
   ','
   'STATE_IND'
   ','
   'COST_CENTER_CODE'
   ;
 end;
 do;
   EFIOUT + 1;
   put SSN $ @;
   put DF_SPE_ACC_ID $ @;
   put DM_PRS_1 $ @;
   put DM_PRS_MID $ @;
   put DM_PRS_LST $ @;
   put DX_STR_ADR_1 $ @;
   put DX_STR_ADR_2 $ @;
   put DM_CT $ @;
   put DC_DOM_ST $ @;
   put DM_FGN_CNY $ @;
   put DF_ZIP $ @;
   put STATE_IND $ @;
   put COST_CENTER_CODE $ ;
   ;
 end;
run;

PROC SQL;
CREATE TABLE OPENLOANS_NOTINCLUDED AS
SELECT DISTINCT
	A.SSN
FROM OPENLOANS A
WHERE A.SSN NOT IN (SELECT B.SSN FROM MARKET4 B)
;
QUIT;


PROC EXPORT DATA=OPENLOANS_NOTINCLUDED
            OUTFILE= "T:\SAS\OPENLOANS_NOT_INCLUDED_WITH_VALID_ADDRESS.xls" 
            DBMS=EXCEL2000 REPLACE;
RUN;


PROC EXPORT DATA=INVALIDADDR
            OUTFILE= "T:\SAS\INVALID_ADDR.xls" 
            DBMS=EXCEL2000 REPLACE;
RUN;
