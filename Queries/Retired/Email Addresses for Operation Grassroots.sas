/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
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

CREATE TABLE NOCOMP AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_PRS_ID
FROM	(SELECT	DF_PRS_ID
		 FROM	OLWHRM1.PD21_GTR_DTH
		 WHERE	DC_DTH_STA = '02'
		 UNION
		 SELECT	DF_PRS_ID
		 FROM	OLWHRM1.PD23_GTR_DSA
		 WHERE	DC_DSA_STA = '09'
		 UNION
		 SELECT	DF_PRS_ID
		 FROM	OLWHRM1.PD24_PRS_BKR
		 WHERE	DC_BKR_STA = '06') A
FOR READ ONLY WITH UR
);

CREATE TABLE ALLALL AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		A.DF_PRS_ID_EDS
		,A.DF_PRS_ID_STU
		,A.DF_PRS_ID_BR
FROM	OLWHRM1.GA01_APP A
		INNER JOIN OLWHRM1.GA10_LON_APP B
			ON A.AF_APL_ID = B.AF_APL_ID
			AND B.AC_PRC_STA = 'A'

WHERE	NOT EXISTS (SELECT  C.AF_APL_ID
				    FROM	OLWHRM1.GA14_LON_STA C
				    WHERE	B.AF_APL_ID = C.AF_APL_ID
							AND B.AF_APL_ID_SFX = C.AF_APL_ID_SFX
							AND C.AC_STA_GA14 = 'A'
							AND C.AC_LON_STA_TYP = 'CP')

FOR READ ONLY WITH UR
);


CREATE TABLE ALL AS
SELECT 	*
FROM	ALLALL A
WHERE	NOT EXISTS (SELECT 	D.DF_PRS_ID
				    FROM	NOCOMP D
				    WHERE	A.DF_PRS_ID_BR = D.DF_PRS_ID)
;

CREATE TABLE OL_EMAIL AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		A1.DF_PRS_ID
		,RTRIM(A1.DM_PRS_1) AS NAME
		,A1.DF_SPE_ACC_ID
		,UCASE(A2.DX_EML_ADR) AS DX_EML_ADR
FROM	OLWHRM1.PD01_PDM_INF A1
  		INNER JOIN OLWHRM1.PD03_PRS_ADR_PHN A2
			ON A1.DF_PRS_ID = A2.DF_PRS_ID
			AND A2.DI_EML_ADR_VAL = 'Y'
);

CREATE TABLE CO_EMAIL AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		A1.DF_PRS_ID
		,RTRIM(A1.DM_PRS_1) AS NAME
		,A1.DF_SPE_ACC_ID
		,UCASE(A2.DX_ADR_EML) AS DX_EML_ADR
FROM	OLWHRM1.PD10_PRS_NME A1
  		INNER JOIN OLWHRM1.PD32_PRS_ADR_EML A2
			ON A1.DF_PRS_ID = A2.DF_PRS_ID
			AND A2.DI_VLD_ADR_EML = 'Y'
			AND A2.DC_STA_PD32 = 'A'
);

CREATE TABLE EMAIL AS
SELECT  DISTINCT *
FROM	(SELECT *
		 FROM OL_EMAIL
		 UNION
		 SELECT *
		 FROM CO_EMAIL)
;	

CREATE TABLE R2 AS
SELECT  DISTINCT
		C.SSN
		,C.NAME
		,C.DF_SPE_ACC_ID
		,C.DX_EML_ADR
FROM 	(SELECT DISTINCT 
				A.DF_PRS_ID_BR AS SSN
				,B.NAME
				,B.DF_SPE_ACC_ID
				,B.DX_EML_ADR
		 FROM 	ALL A
				INNER JOIN EMAIL B
					ON A.DF_PRS_ID_BR = B.DF_PRS_ID
		 UNION
		 SELECT DISTINCT 
				A.DF_PRS_ID_STU AS SSN
				,B.NAME
				,B.DF_SPE_ACC_ID
				,B.DX_EML_ADR
		 FROM 	ALL A
				INNER JOIN EMAIL B
					ON A.DF_PRS_ID_STU = B.DF_PRS_ID
		 UNION
		 SELECT DISTINCT 
				A.DF_PRS_ID_EDS AS SSN
				,B.NAME
				,B.DF_SPE_ACC_ID
				,B.DX_EML_ADR
		 FROM 	ALL A
				INNER JOIN EMAIL B
					ON A.DF_PRS_ID_EDS = B.DF_PRS_ID) C
;

CREATE TABLE R3 AS
SELECT	DISTINCT A.*
FROM 	R2 A
		INNER JOIN CONNECTION TO DB2 (SELECT	*
									  FROM		OLWHRM1.AY10_BR_LON_ATY
									  WHERE		PF_REQ_ACT = 'SPEWO') B ON
			A.SSN = B.BF_SSN
;

DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;


PROC EXPORT DATA= WORKLOCL.R2
            OUTFILE= "T:\SAS\Grassroots.R2.txt" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;

PROC EXPORT DATA= WORKLOCL.R3
            OUTFILE= "T:\SAS\Grassroots.R3.txt" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;
