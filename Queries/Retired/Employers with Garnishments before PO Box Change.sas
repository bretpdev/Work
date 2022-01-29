/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = C:\WINDOWS\TEMP;
FILENAME REPORT2 "&RPTLIB/EMPLWITHGARN.TXT";

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
SELECT DISTINCT ALL.*
FROM	(
		SELECT	DISTINCT
				F.IM_IST_FUL
				,F.IX_GEN_STR_ADR_1
				,F.IX_GEN_STR_ADR_2
				,F.IM_GEN_CT
				,F.IC_GEN_ST
				,F.IF_GEN_ZIP 
				,CASE WHEN F.IC_GEN_ST = 'FC' THEN '' ELSE F.IC_GEN_ST END AS STATE_IND
				,'MA2329' AS COST_CENTER_CODE
/*				,B.DF_PRS_ID_BR*/
/*				,A.LC_GRN*/
		FROM	OLWHRM1.DC01_LON_CLM_INF A
				INNER JOIN OLWHRM1.LA12_LEG_ACT_LON B
					ON A.AF_APL_ID = B.AF_APL_ID
					AND A.AF_APL_ID_SFX = B.AF_APL_ID_SFX
					AND A.LC_STA_DC10 = '03'
					AND A.LC_AUX_STA = ''
					AND A.LD_CLM_ASN_DOE IS NULL
					AND A.LC_GRN IN ('07')
				INNER JOIN OLWHRM1.LA10_LEG_ACT C
					ON B.DF_PRS_ID_BR = C.DF_PRS_ID_BR
					AND B.BF_CRT_DTS_LA10 = C.BF_CRT_DTS_LA10
					AND C.BC_LEG_ACT_REC_TYP = '2'
					AND C.BD_WDR IS NULL
					AND C.BD_INA IS NULL
				INNER JOIN OLWHRM1.LA11_LEG_ACT_ATY D
					ON C.DF_PRS_ID_BR = D.DF_PRS_ID_BR
					AND C.BF_CRT_DTS_LA10 = D.BF_CRT_DTS_LA10
					AND D.BC_EXE_TYP IN ('04','05')
				INNER JOIN OLWHRM1.PD01_PDM_INF E
					ON E.DF_PRS_ID = C.DF_PRS_ID_BR
					AND E.BI_EMP_INF_OVR = 'Y'
				INNER JOIN OLWHRM1.IN01_LGS_IDM_MST F
					ON E.BF_EMP_ID_1 = F.IF_IST
					AND F.IC_IST_TYP = '006'

		UNION

		SELECT	DISTINCT
				F.IM_IST_FUL
				,F.IX_GEN_STR_ADR_1
				,F.IX_GEN_STR_ADR_2
				,F.IM_GEN_CT
				,F.IC_GEN_ST
				,F.IF_GEN_ZIP 
				,CASE WHEN F.IC_GEN_ST = 'FC' THEN '' ELSE F.IC_GEN_ST END AS STATE_IND
				,'MA2329' AS COST_CENTER_CODE
/*				,B.DF_PRS_ID_BR*/
/*				,A.LC_GRN*/
		FROM	OLWHRM1.DC01_LON_CLM_INF A
				INNER JOIN OLWHRM1.LA12_LEG_ACT_LON B
					ON A.AF_APL_ID = B.AF_APL_ID
					AND A.AF_APL_ID_SFX = B.AF_APL_ID_SFX
					AND A.LC_STA_DC10 = '03'
					AND A.LC_AUX_STA = ''
					AND A.LD_CLM_ASN_DOE IS NULL
					AND A.LC_GRN IN ('02')
				INNER JOIN OLWHRM1.LA10_LEG_ACT C
					ON B.DF_PRS_ID_BR = C.DF_PRS_ID_BR
					AND B.BF_CRT_DTS_LA10 = C.BF_CRT_DTS_LA10
					AND C.BC_LEG_ACT_REC_TYP = '1'
					AND C.BD_WDR IS NULL
					AND C.BD_INA IS NULL
				INNER JOIN OLWHRM1.PD01_PDM_INF E
					ON E.DF_PRS_ID = C.DF_PRS_ID_BR
					AND E.BI_EMP_INF_OVR = 'Y'
				INNER JOIN OLWHRM1.IN01_LGS_IDM_MST F
					ON E.BF_EMP_ID_1 = F.IF_IST
					AND F.IC_IST_TYP = '006'
		) ALL
ORDER BY ALL.STATE_IND
			
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA DEMO;
SET WORKLOCL.DEMO;
RUN;

data _null_;
set  DEMO;
file REPORT2  delimiter=',' DSD DROPOVER lrecl=32767;
	format IM_IST_FUL $35. ;
	format IX_GEN_STR_ADR_1 $35. ;
	format IX_GEN_STR_ADR_2 $35. ;
	format IM_GEN_CT $30. ;
	format IC_GEN_ST $2. ;
	format IF_GEN_ZIP $14. ; 
	format STATE_IND $2. ;
	format COST_CENTER_CODE $6. ;
if _n_ = 1 then        /* write column names */
 do;
   put
   'IM_IST_FUL'
   ','
   'IX_GEN_STR_ADR_1'
   ','
   'IX_GEN_STR_ADR_2'
   ','
   'IM_GEN_CT'
   ','
   'IC_GEN_ST'
   ','
   'IF_GEN_ZIP'
   ','
   'STATE_IND'
   ','
   'COST_CENTER_CODE'
   ;
 end;
 do;
   EFIOUT + 1;
   put IM_IST_FUL $ @;
   put IX_GEN_STR_ADR_1 $ @;
   put IX_GEN_STR_ADR_2 $ @;
   put IM_GEN_CT $ @;
   put IC_GEN_ST $ @;
   put IF_GEN_ZIP $ @;
   put STATE_IND $ @;
   put COST_CENTER_CODE $ ;
   ;
 end;
run;
