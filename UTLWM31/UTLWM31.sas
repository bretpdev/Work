/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWM31.LWM31R2";
FILENAME REPORT3 "&RPTLIB/ULWM31.LWM31R3";
FILENAME REPORT4 "&RPTLIB/ULWM31.LWM31R4";
FILENAME REPORT5 "&RPTLIB/ULWM31.LWM31R5";
FILENAME REPORT6 "&RPTLIB/ULWM31.LWM31R6";
FILENAME REPORT7 "&RPTLIB/ULWM31.LWM31R7";
FILENAME REPORTZ "&RPTLIB/ULWM31.LWM31RZ";


OPTIONS SYMBOLGEN NOCENTER NODATE NONUMBER LS=132;

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

SELECT DISTINCT 
	DC01.BF_SSN
	,PD01.DF_SPE_ACC_ID
	,PD01.DM_PRS_1
	,PD01.DM_PRS_LST
	,PD01.DX_STR_ADR_1
	,PD01.DX_STR_ADR_2
	,PD01.DM_CT
	,PD01.DC_DOM_ST
	,PD01.DF_ZIP
	,PD01.DM_FGN_CNY
	,DAYS(CURRENT DATE) - DAYS(DC01.LD_DCO) AS DAYS_DLQ
	,AY01_A.ALTS1
	,AY01_B.ALTS2
	,AY01_C.ALTT1
	,AY01_D.ALTT2
	,AY01_E.ALTV1
	,AY01_F.ALTV2
	,PD01.DC_ADR
	,CASE
	WHEN PD01.DM_FGN_CNY <> ''
		THEN ''
	WHEN PD01.DM_FGN_CNY = ''
		THEN PD01.DC_DOM_ST
	END AS STATE_IND
	,'MA2329' AS CCC

FROM	OLWHRM1.DC01_LON_CLM_INF DC01
INNER JOIN OLWHRM1.GA15_NDS_ID GA15
	ON DC01.BF_SSN = GA15.DF_PRS_ID_STU_NDS
	AND DC01.AF_APL_ID = GA15.AF_APL_ID
	AND DC01.AF_APL_ID_SFX = GA15.AF_APL_ID_SFX
	AND GA15.AD_NDS_CLC_ENT_RPD < CURRENT DATE - 2 YEARS
INNER JOIN OLWHRM1.PD01_PDM_INF PD01
	ON PD01.DF_PRS_ID = DC01.BF_SSN
	AND PD01.DI_PHN_VLD = 'N'
	AND PD01.DI_VLD_ADR = 'Y'
	AND PD01.DC_ADR = 'L'
LEFT OUTER JOIN (SELECT A.DF_PRS_ID, 'X' AS ALTS1 
				FROM OLWHRM1.AY01_BR_ATY A 
				WHERE A.PF_ACT = 'ALTS1'
				AND A.BD_ATY_PRF >= CURRENT DATE - 14 DAYS
				) AY01_A
	ON DC01.BF_SSN = AY01_A.DF_PRS_ID
LEFT OUTER JOIN (SELECT A.DF_PRS_ID, 'X' AS ALTS2
				FROM OLWHRM1.AY01_BR_ATY A 
				WHERE A.PF_ACT = 'ALTS2'
				AND A.BD_ATY_PRF >= CURRENT DATE - 14 DAYS
				) AY01_B
	ON DC01.BF_SSN = AY01_B.DF_PRS_ID
LEFT OUTER JOIN (SELECT A.DF_PRS_ID, 'X' AS ALTT1 
				FROM OLWHRM1.AY01_BR_ATY A 
				WHERE A.PF_ACT = 'ALTT1'
				AND A.BD_ATY_PRF >= CURRENT DATE - 14 DAYS
				) AY01_C
	ON DC01.BF_SSN = AY01_C.DF_PRS_ID
LEFT OUTER JOIN (SELECT A.DF_PRS_ID, 'X' AS ALTT2 
				FROM OLWHRM1.AY01_BR_ATY A 
				WHERE A.PF_ACT = 'ALTT2'
				AND A.BD_ATY_PRF >= CURRENT DATE - 14 DAYS
				) AY01_D
	ON DC01.BF_SSN = AY01_D.DF_PRS_ID
LEFT OUTER JOIN (SELECT A.DF_PRS_ID, 'X' AS ALTV1 
				FROM OLWHRM1.AY01_BR_ATY A 
				WHERE A.PF_ACT = 'ALTV1'
				AND A.BD_ATY_PRF >= CURRENT DATE - 14 DAYS
				) AY01_E
	ON DC01.BF_SSN = AY01_E.DF_PRS_ID
LEFT OUTER JOIN (SELECT A.DF_PRS_ID, 'X' AS ALTV2 
				FROM OLWHRM1.AY01_BR_ATY A 
				WHERE A.PF_ACT = 'ALTV2'
				AND A.BD_ATY_PRF >= CURRENT DATE - 14 DAYS
				) AY01_F
	ON DC01.BF_SSN = AY01_F.DF_PRS_ID




WHERE DC01.LC_STA_DC10 = '01'
AND LC_PCL_REA IN ('DF','RS','DB','DQ')


FOR READ ONLY WITH UR

);
DISCONNECT FROM DB2;

%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;
%sqlcheck;
quit;

ENDRSUBMIT;

DATA DEMO; SET WORKLOCL.DEMO; RUN;

PROC SQL;
CREATE TABLE DEMO2 AS
SELECT BF_SSN
	,DF_SPE_ACC_ID
	,DM_PRS_1
	,DM_PRS_LST
	,DX_STR_ADR_1
	,DX_STR_ADR_2
	,DM_CT
	,DC_DOM_ST
	,DF_ZIP
	,DM_FGN_CNY
	,MAX(DAYS_DLQ) AS DAYS_DLQ
	,ALTS1
	,ALTS2
	,ALTT1
	,ALTT2
	,ALTV1
	,ALTV2
	,DC_ADR
	,STATE_IND
	,CCC
FROM DEMO
GROUP BY BF_SSN
	,DF_SPE_ACC_ID
	,DM_PRS_1
	,DM_PRS_LST
	,DX_STR_ADR_1
	,DX_STR_ADR_2
	,DM_CT
	,DC_DOM_ST
	,DF_ZIP
	,DM_FGN_CNY
	,ALTS1
	,ALTS2
	,ALTT1
	,ALTT2
	,ALTV1
	,ALTV2
	,DC_ADR
	,STATE_IND
	,CCC
;
QUIT;

%MACRO KEY_CLC(TBL);
*CALCULATE KEYLINE;
DATA &TBL (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET &TBL;
KEYSSN = TRANSLATE(BF_SSN,'MYLAUGHTER','0987654321');
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
%KEY_CLC(DEMO2);

DATA R2;
SET DEMO2;
WHERE DAYS_DLQ = 157 AND DAYS_DLQ < 171 AND ALTS1 IS NULL;
RUN;

DATA R3;
SET DEMO2;
WHERE DAYS_DLQ >= 172 AND DAYS_DLQ < 186 AND ALTS2 IS NULL;
RUN;

DATA R4;
SET DEMO2;
WHERE DAYS_DLQ >= 187 AND DAYS_DLQ < 201 AND ALTT1 IS NULL;
RUN;

DATA R5;
SET DEMO2;
WHERE DAYS_DLQ >= 202 AND DAYS_DLQ < 216 AND ALTT2 IS NULL;
RUN;

DATA R6;
SET DEMO2;
WHERE DAYS_DLQ >= 217 AND DAYS_DLQ < 231 AND ALTV1 IS NULL;
RUN;

DATA R7;
SET DEMO2;
WHERE DAYS_DLQ >= 232 AND DAYS_DLQ < 330 AND ALTV2 IS NULL;
RUN;

%MACRO TEXT_FILE(REPNO);

PROC SORT DATA=R&REPNO;
BY STATE_IND;
RUN;

data _null_;
set  R&REPNO;
file REPORT&REPNO delimiter=',' DSD DROPOVER lrecl=32767;
   format BF_SSN $9. ;
   format DF_SPE_ACC_ID $10. ;
   format DM_PRS_1 $12. ;
   format DM_PRS_LST $35. ;
   format DX_STR_ADR_1 $35. ;
   format DX_STR_ADR_2 $35. ;
   format DM_CT $30. ;
   format DC_DOM_ST $2. ;
   format DF_ZIP $14. ;
   format DM_FGN_CNY $25. ;
   format DAYS_DLQ 11. ;
   format ACSKEY $18. ;
   format STATE_IND $2. ;
   format CCC $6. ;
if _n_ = 1 then        /* write column names */
 do;
   put
   'BF_SSN'
   ','
   'DF_SPE_ACC_ID'
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
   'DF_ZIP'
   ','
   'DM_FGN_CNY'
   ','
   'DAYS_DLQ'
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
   put BF_SSN $ @;
   put DF_SPE_ACC_ID $ @;
   put DM_PRS_1 $ @;
   put DM_PRS_LST $ @;
   put DX_STR_ADR_1 $ @;
   put DX_STR_ADR_2 $ @;
   put DM_CT $ @;
   put DC_DOM_ST $ @;
   put DF_ZIP $ @;
   put DM_FGN_CNY $ @;
   put DAYS_DLQ @;
   put ACSKEY $ @;
   put STATE_IND $ @;
   put CCC $ ;
   ;
 end;
run;
%MEND TEXT_FILE;

%TEXT_FILE(2);
%TEXT_FILE(3);
%TEXT_FILE(4);
%TEXT_FILE(5);
%TEXT_FILE(6);
%TEXT_FILE(7);
