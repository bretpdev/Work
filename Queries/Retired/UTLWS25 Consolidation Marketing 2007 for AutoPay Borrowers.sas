/*UTLWS25*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWS25.LWS25R2";
FILENAME REPORT3 "&RPTLIB/ULWS25.LWS25R3";
FILENAME REPORTZ "&RPTLIB/ULWS25.LWS25RZ";

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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT DISTINCT CC.DF_SPE_ACC_ID AS ACCT 
	,COMP.BC_EFT_STA AS AUTO_PAY
	,AA.AF_CUR_LON_SER_AGY AS SERVICER
	,CC.DM_PRS_1
	,CC.DM_PRS_LST
	,CC.DX_STR_ADR_1
	,CC.DX_STR_ADR_2
	,CC.DM_CT
	,CC.DC_DOM_ST
	,CC.DM_FGN_CNY
	,CC.DF_ZIP
	,DD.LF_LON_CUR_OWN
	,CASE
	WHEN CC.DC_DOM_ST = 'FC'
	THEN ''
	WHEN CC.DC_DOM_ST <> 'FC'
	THEN CC.DC_DOM_ST
	END AS STATE_IND
	,CASE
	WHEN DD.LF_LON_CUR_OWN = '828476'
	THEN 'MA2324'
	WHEN DD.LF_LON_CUR_OWN <> '828476'
	THEN 'MA2327'
	END AS COST_CENTER_CODE
	,AY01.PF_ACT
	,BB.DF_PRS_ID_BR AS SSN
	,CC.DC_ADR

FROM (SELECT DISTINCT A.AF_APL_ID, A.AF_CUR_LON_SER_AGY 
FROM	OLWHRM1.GA10_LON_APP A
INNER JOIN (SELECT Z.AF_APL_ID
/*				,Z.AF_APL_ID_SFX*/
				,COUNT(*) AS CNT
			FROM	OLWHRM1.GA10_LON_APP Z
			INNER JOIN OLWHRM1.GA14_LON_STA Y
			ON Z.AF_APL_ID = Y.AF_APL_ID
			AND Z.AF_APL_ID_SFX = Y.AF_APL_ID_SFX
			AND Y.AC_STA_GA14 = 'A'
			WHERE Z.AA_CUR_PRI > 0
			AND Z.AC_LON_TYP <> 'CL'
			AND Y.AC_LON_STA_TYP IN ('CR', 'DA', 'FB', 'IG', 'OR', 'RP')
			GROUP BY Z.AF_APL_ID/*, Z.AF_APL_ID_SFX*/
			) B
	ON A.AF_APL_ID = B.AF_APL_ID
/*	AND A.AF_APL_ID_SFX = B.AF_APL_ID_SFX*/
INNER JOIN OLWHRM1.GA14_LON_STA C
	ON A.AF_APL_ID = C.AF_APL_ID
	AND A.AF_APL_ID_SFX = C.AF_APL_ID_SFX
	AND C.AC_STA_GA14 = 'A'
WHERE A.AA_CUR_PRI > 0
AND A.AC_LON_TYP <> 'CL'
AND C.AC_LON_STA_REA  NOT IN ('BC', 'BH', 'BO', 'CS', 'DE', 'DI', 'FC', 'IN') 
AND B.CNT > 1
UNION
SELECT DISTINCT A.AF_APL_ID, A.AF_CUR_LON_SER_AGY
FROM	OLWHRM1.GA10_LON_APP A
INNER JOIN (SELECT Z.AF_APL_ID
/*				,Z.AF_APL_ID_SFX*/
				,COUNT(*) AS CNT
			FROM	OLWHRM1.GA10_LON_APP Z
			INNER JOIN OLWHRM1.GA14_LON_STA Y
			ON Z.AF_APL_ID = Y.AF_APL_ID
			AND Z.AF_APL_ID_SFX = Y.AF_APL_ID_SFX
			AND Y.AC_STA_GA14 = 'A'
			WHERE Z.AA_CUR_PRI > 0
			AND Z.AC_LON_TYP <> 'CL'
			AND Y.AC_LON_STA_TYP IN ('CR', 'DA', 'FB', 'IG', 'OR', 'RP')
			GROUP BY Z.AF_APL_ID/*, Z.AF_APL_ID_SFX*/
			) B
	ON A.AF_APL_ID = B.AF_APL_ID
/*	AND A.AF_APL_ID_SFX = B.AF_APL_ID_SFX*/
INNER JOIN OLWHRM1.GA14_LON_STA C
	ON A.AF_APL_ID = C.AF_APL_ID
	AND A.AF_APL_ID_SFX = C.AF_APL_ID_SFX
	AND C.AC_STA_GA14 = 'A'
WHERE A.AA_CUR_PRI > 0
AND A.AC_LON_TYP = 'CL'
AND C.AC_LON_STA_REA  NOT IN ('BC', 'BH', 'BO', 'CS', 'DE', 'DI', 'FC', 'IN') 
) AA
INNER JOIN OLWHRM1.GA01_APP BB
ON AA.AF_APL_ID = BB.AF_APL_ID
INNER JOIN OLWHRM1.PD01_PDM_INF CC
ON BB.DF_PRS_ID_BR = CC.DF_PRS_ID
AND CC.DI_VLD_ADR = 'Y'
LEFT OUTER JOIN OLWHRM1.BR30_BR_EFT COMP
ON BB.DF_PRS_ID_BR = COMP.BF_SSN
INNER JOIN OLWHRM1.LN10_LON DD
ON BB.DF_PRS_ID_BR = DD.BF_SSN
LEFT OUTER JOIN (SELECT A1.DF_PRS_ID, MAX(A1.PF_ACT) AS PF_ACT 
				FROM OLWHRM1.AY01_BR_ATY A1 
				WHERE A1.PF_ACT LIKE ('MLCM%')
				GROUP BY A1.DF_PRS_ID
				) AY01
ON AY01.DF_PRS_ID = BB.DF_PRS_ID_BR

WHERE BB.DF_PRS_ID_BR NOT IN (SELECT A1.DF_PRS_ID 
							FROM OLWHRM1.AY01_BR_ATY A1
							WHERE (PF_ACT IN ('MLCM1','MLCM2')
							AND DAYS(A1.BD_ATY_PRF) > DAYS(CURRENT DATE) - 60)
							OR A1.PF_ACT = 'MLCM3'
							)

FOR READ ONLY WITH UR

);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA DEMO; SET WORKLOCL.DEMO; RUN;

DATA NELNET AUTO;
SET DEMO;
IF SERVICER = '700121' THEN OUTPUT NELNET;
IF AUTO_PAY = 'A' THEN OUTPUT AUTO;
ELSE DELETE;
RUN;

%MACRO KEY_CLC(TBL);
*CALCULATE KEYLINE;
DATA &TBL (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET &TBL;
KEYSSN = TRANSLATE(SSN,'MYLAUGHTER','0987654321');
MODAY = PUT(DATE(),MMDDYYN4.);
KEYLINE = "P"||KEYSSN||MODAY||DC_ADR;
CHKDIG = 0;
LENGTH DIG $2.;
DO I = 1 TO LENGTH(KEYLINE);
	IF I/2 NE ROUND(I/2,1) 
		THEN DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4) * 2, 2.);
	ELSE DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4), 2.);
	IF SUBSTR(DIG,1,1) = " " 
		THEN CHKDIG = CHKDIG + INPUT(SUBSTR(DIG,2,1),1.);
		ELSE DO;
			CHK1 = INPUT(SUBSTR(DIG,1,1),1.);
			CHK2 = INPUT(SUBSTR(DIG,2,1),1.);
			IF CHK1 + CHK2 >= 10
				THEN DO;
					CHK3 = PUT(CHK1 + CHK2,2.);
					CHK1 = INPUT(SUBSTR(CHK3,1,1),1.);
					CHK2 = INPUT(SUBSTR(CHK3,2,1),1.);
				END;
			CHKDIG = CHKDIG + CHK1 + CHK2;
		END;
END;
CHKDIGIT = 10 - INPUT(SUBSTR((RIGHT(PUT(CHKDIG,3.))),3,1),3.);
IF CHKDIGIT = 10 THEN CHKDIGIT = 0;
CHECK = PUT(CHKDIGIT,1.);
ACSKEY = "#"||KEYLINE||CHECK||"#";
RUN;
%MEND KEY_CLC;
%KEY_CLC(NELNET);
%KEY_CLC(AUTO);


PROC SQL;
CREATE TABLE AUTO2 AS
SELECT *
FROM AUTO A
WHERE A.ACCT NOT IN (SELECT B.ACCT FROM NELNET B)
;
QUIT;

%MACRO toFile(TB=,REPNO=);

PROC SORT DATA=&TB NODUPKEY;
BY ACCT;
RUN;

DATA &TB;
SET &TB;
IF INPUT(SUBSTR(DF_ZIP,6,1),BEST12.) >= 0 
	THEN DF_ZIP = LEFT(TRIM(SUBSTR(DF_ZIP,1,5)))||'-'||LEFT(TRIM(SUBSTR(DF_ZIP,6,4)));
ELSE DF_ZIP = DF_ZIP;
RUN;

PROC SORT DATA=&TB;
BY COST_CENTER_CODE STATE_IND;
RUN;

data _null_;
set  &TB;
file REPORT&REPNO  delimiter=',' DSD DROPOVER lrecl=32767;
   format ACCT $10. ;
   format AUTO_PAY $1. ;
   format SERVICER $8. ;
   format DM_PRS_1 $12. ;
   format DM_PRS_LST $35. ;
   format DX_STR_ADR_1 $35. ;
   format DX_STR_ADR_2 $35. ;
   format DM_CT $30. ;
   format DC_DOM_ST $2. ;
   format DM_FGN_CNY $25. ;
   format DF_ZIP $14. ;
   format LF_LON_CUR_OWN $8. ;
   FORMAT PF_ACT $5. ;
   FORMAT SSN $9. ;
   FORMAT ACSKEY $20.
   format STATE_IND $2. ;
   format COST_CENTER_CODE $6. ;
   
if _n_ = 1 then        /* write column names */
 do;
   put
   'ACCT'
   ','
   'DM_PRS_1'
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
   'LF_LON_CUR_OWN'
   ','
   'PF_ACT'
   ','
   'SSN'
   ','
   'ACSKEY'
   ','
   'STATE_IND'
   ','
   'COST_CENTER_CODE'
   ;
 end;
 do;
   EFIOUT + 1;
   put ACCT $ @;
   put DM_PRS_1 $ @;
   put DM_PRS_LST $ @;
   put DX_STR_ADR_1 $ @;
   put DX_STR_ADR_2 $ @;
   put DM_CT $ @;
   put DC_DOM_ST $ @;
   put DM_FGN_CNY $ @;
   put DF_ZIP $ @;
   put LF_LON_CUR_OWN $ @;
   put PF_ACT $ @;
   put SSN $ @;
   PUT ACSKEY $ @;
   put STATE_IND $ @;
   put COST_CENTER_CODE $ ;
   ;
 end;
run;

%MEND ;
%toFile(TB=NELNET,REPNO=2);
%toFile(TB=AUTO2,REPNO=3);
