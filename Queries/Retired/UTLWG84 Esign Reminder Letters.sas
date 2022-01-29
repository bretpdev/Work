/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORTZ "&RPTLIB/ULWG84.LWG84RZ";*/
/*FILENAME REPORT2 "&RPTLIB/ULWG84.LWG84R2";*/
/*FILENAME REPORT3 "&RPTLIB/ULWG84.LWG84R3";*/
/*FILENAME REPORT4 "&RPTLIB/ULWG84.LWG84R4";*/
/*FILENAME REPORT5 "&RPTLIB/ULWG84.LWG84R5";*/
/*FILENAME REPORT6 "&RPTLIB/ULWG84.LWG84R6";*/
/*FILENAME REPORT7 "&RPTLIB/ULWG84.LWG84R7";*/
/*FILENAME REPORT8 "&RPTLIB/ULWG84.LWG84R8";*/
/*FILENAME REPORT9 "&RPTLIB/ULWG84.LWG84R9";*/

FILENAME REPORT2 "T:\SAS\ULWG84.LWG84R2.TEST.TXT";
FILENAME REPORT3 "T:\SAS\ULWG84.LWG84R3.TEST.TXT";
FILENAME REPORT4 "T:\SAS\ULWG84.LWG84R4.TEST.TXT";
FILENAME REPORT5 "T:\SAS\ULWG84.LWG84R5.TEST.TXT";
FILENAME REPORT6 "T:\SAS\ULWG84.LWG84R6.TEST.TXT";
FILENAME REPORT7 "T:\SAS\ULWG84.LWG84R7.TEST.TXT";
FILENAME REPORT8 "T:\SAS\ULWG84.LWG84R8.TEST.TXT";
FILENAME REPORT9 "T:\SAS\ULWG84.LWG84R9.TEST.TXT";

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
CREATE TABLE ESIGN AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		A.DF_PRS_ID	
		,CHAR(C.DF_SPE_ID)	AS DF_SPE_ID
		,B.DM_PRS_1
		,B.DM_PRS_LST
		,B.DX_STR_ADR_1
		,B.DX_STR_ADR_2
		,B.DM_CT
		,CASE WHEN B.DM_FGN_CNY = '' THEN B.DC_DOM_ST ELSE '' END	AS DC_DOM_ST
		,B.DM_FGN_CNY
		,B.DF_ZIP
		,CASE WHEN B.DC_DOM_ST = 'FC' THEN '' ELSE B.DC_DOM_ST END 	AS ST_CODE
		,'MA2330'			AS COST_CENTER_CODE
		,E.BD_ATY_PRF		AS SLBSL_BD_ATY_PRF
		,D.BD_ATY_PRF
		,D.PF_ACT
		,A.DI_VLD_ADR
FROM	OLWHRM1.PD03_PRS_ADR_PHN A
		INNER JOIN OLWHRM1.PD01_PDM_INF B ON
			A.DC_ADR = 'L'
			AND A.DF_PRS_ID = B.DF_PRS_ID
		INNER JOIN OLWHRM1.PH01_SUPER_ID C ON
			A.DF_PRS_ID = C.DF_PRS_ID
		INNER JOIN OLWHRM1.AY01_BR_ATY D ON
			A.DF_PRS_ID = D.DF_PRS_ID
		INNER JOIN (SELECT 	Y.DF_PRS_ID
							,Y.BD_ATY_PRF
					FROM	OLWHRM1.AY01_BR_ATY Y 
					WHERE 	Y.PF_ACT = 'SLBSL') E ON
			A.DF_PRS_ID = E.DF_PRS_ID
			AND D.BD_ATY_PRF > E.BD_ATY_PRF
		INNER JOIN OLWHRM1.GA01_APP F ON
			A.DF_PRS_ID = F.DF_PRS_ID_BR
		INNER JOIN OLWHRM1.GA10_LON_APP G ON
			F.AF_APL_ID = G.AF_APL_ID
			AND G.AC_PRC_STA = 'P'
WHERE	NOT EXISTS (SELECT 	*
					FROM 	OLWHRM1.AY01_BR_ATY X
					WHERE	X.PF_ACT = 'GDSST'
							AND DAYS(X.BD_ATY_PRF) > DAYS(E.BD_ATY_PRF)
							AND X.DF_PRS_ID = A.DF_PRS_ID)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

PROC SQL;
CREATE TABLE SLBR1 AS
SELECT 	DISTINCT
		A.DF_PRS_ID	
		,A.DF_SPE_ID	
		,A.DM_PRS_1
		,A.DM_PRS_LST
		,A.DX_STR_ADR_1
		,A.DX_STR_ADR_2
		,A.DM_CT
		,A.DC_DOM_ST
		,A.DM_FGN_CNY
		,A.DF_ZIP
		,A.ST_CODE
		,A.COST_CENTER_CODE
		,A.DI_VLD_ADR
FROM	ESIGN A
WHERE	TODAY() - A.SLBSL_BD_ATY_PRF BETWEEN 14 AND 27	/* >=14 AND < 28 */
		AND NOT EXISTS (SELECT  *
						FROM 	ESIGN B
						WHERE	B.PF_ACT = 'SLBR1'
								AND B.BD_ATY_PRF > A.SLBSL_BD_ATY_PRF
								AND A.DF_PRS_ID	= B.DF_PRS_ID)
ORDER BY A.ST_CODE;
QUIT;

PROC SQL;
CREATE TABLE SLBR2 AS
SELECT 	DISTINCT
		A.DF_PRS_ID	
		,A.DF_SPE_ID	
		,A.DM_PRS_1
		,A.DM_PRS_LST
		,A.DX_STR_ADR_1
		,A.DX_STR_ADR_2
		,A.DM_CT
		,A.DC_DOM_ST
		,A.DM_FGN_CNY
		,A.DF_ZIP
		,A.ST_CODE
		,A.COST_CENTER_CODE
		,A.DI_VLD_ADR
FROM	ESIGN A
WHERE	TODAY() - A.SLBSL_BD_ATY_PRF BETWEEN 28 AND 55	/* >=28 AND < 56 */
		AND NOT EXISTS (SELECT  *
						FROM 	ESIGN B
						WHERE	B.PF_ACT = 'SLBR2'
								AND B.BD_ATY_PRF > A.SLBSL_BD_ATY_PRF
								AND A.DF_PRS_ID	= B.DF_PRS_ID)
ORDER BY A.ST_CODE;
QUIT;

PROC SQL;
CREATE TABLE SLBR3 AS
SELECT 	DISTINCT
		A.DF_PRS_ID	
		,A.DF_SPE_ID	
		,A.DM_PRS_1
		,A.DM_PRS_LST
		,A.DX_STR_ADR_1
		,A.DX_STR_ADR_2
		,A.DM_CT
		,A.DC_DOM_ST
		,A.DM_FGN_CNY
		,A.DF_ZIP
		,A.ST_CODE
		,A.COST_CENTER_CODE
		,A.DI_VLD_ADR
FROM	ESIGN A
WHERE	TODAY() - A.SLBSL_BD_ATY_PRF BETWEEN 56 AND 69	/* >=56 AND < 70 */
		AND NOT EXISTS (SELECT  *
						FROM 	ESIGN B
						WHERE	B.PF_ACT = 'SLBR3'
								AND B.BD_ATY_PRF > A.SLBSL_BD_ATY_PRF
								AND A.DF_PRS_ID	= B.DF_PRS_ID)
ORDER BY A.ST_CODE;
QUIT;

PROC SQL;
CREATE TABLE SLBR4 AS
SELECT 	DISTINCT
		A.DF_PRS_ID	
		,A.DF_SPE_ID
		,A.DM_PRS_1
		,A.DM_PRS_LST
		,A.DX_STR_ADR_1
		,A.DX_STR_ADR_2
		,A.DM_CT
		,A.DC_DOM_ST
		,A.DM_FGN_CNY
		,A.DF_ZIP
		,A.ST_CODE
		,A.COST_CENTER_CODE
		,A.DI_VLD_ADR
FROM	ESIGN A
WHERE	TODAY() - A.SLBSL_BD_ATY_PRF >= 70
		AND NOT EXISTS (SELECT  *
						FROM 	ESIGN B
						WHERE	B.PF_ACT = 'SLBR4'
								AND B.BD_ATY_PRF > A.SLBSL_BD_ATY_PRF
								AND A.DF_PRS_ID	= B.DF_PRS_ID)
ORDER BY A.ST_CODE;
QUIT;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

/*DATA ESIGN; SET WORKLOCL.ESIGN; RUN;*/
DATA SLBR1; SET WORKLOCL.SLBR1; RUN;
DATA SLBR2; SET WORKLOCL.SLBR2; RUN;
DATA SLBR3; SET WORKLOCL.SLBR3; RUN;
DATA SLBR4; SET WORKLOCL.SLBR4; RUN;

*CALCULATE KEYLINE;
%MACRO KEYLINE(FILENO=);
DATA &FILENO (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET &FILENO;
KEYSSN = TRANSLATE(DF_PRS_ID,'MYLAUGHTER','0987654321');
MODAY = PUT(DATE(),MMDDYYN4.);
KEYLINE = "P"||KEYSSN||MODAY||"L";
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
%MEND ;

%KEYLINE(FILENO = SLBR1);
%KEYLINE(FILENO = SLBR2);
%KEYLINE(FILENO = SLBR3);
%KEYLINE(FILENO = SLBR4);


%MACRO FULTR(FILENO=,IND=,REPNO=);
DATA ESLTR;
SET &FILENO;
WHERE DI_VLD_ADR = &IND;
RUN;

DATA _NULL_;
SET ESLTR;
FILE REPORT&REPNO DELIMITER=',' DSD DROPOVER LRECL=32767;

FORMAT DF_PRS_ID $9. ;
FORMAT DM_PRS_1 $12. ;
FORMAT DM_PRS_LST $35. ;
FORMAT DX_STR_ADR_1 $35. ;
FORMAT DX_STR_ADR_2 $35. ;
FORMAT DM_CT $30. ;
FORMAT DC_DOM_ST $2. ;
FORMAT DF_ZIP $14. ;
FORMAT DM_FGN_CNY $25. ;
FORMAT ACSKEY $18. ;
FORMAT DF_SPE_ID $10.;

IF _N_ = 1 THEN DO;       /* WRITE COLUMN NAMES */
	PUT
	'DF_PRS_ID'			','
	'DF_SPE_ID'			','
	'DM_PRS_1'			','
	'DM_PRS_LST'		','
	'DX_STR_ADR_1'		','
	'DX_STR_ADR_2'		','
	'DM_CT'				','
	'DC_DOM_ST'			','
	'DM_FGN_CNY'		','
	'DF_ZIP'			','
	'ACSKEY'			','
	'STATE_IND'			','
	'COST_CENTER_CODE'
	;
	END;

DO;
	PUT DF_PRS_ID $ @;
	PUT DF_SPE_ID $ @;
	PUT DM_PRS_1 $ @;
	PUT DM_PRS_LST $ @;
	PUT DX_STR_ADR_1 $ @;
	PUT DX_STR_ADR_2 $ @;
	PUT DM_CT $ @;
	PUT DC_DOM_ST $ @;
	PUT DM_FGN_CNY $ @;
	PUT DF_ZIP $ @;
	PUT ACSKEY $ @;
	PUT ST_CODE $ @;
	PUT COST_CENTER_CODE $ ;
	END;
RUN;
%MEND ;

%FULTR(FILENO=SLBR1,IND='Y',REPNO=2);
%FULTR(FILENO=SLBR2,IND='Y',REPNO=3);
%FULTR(FILENO=SLBR3,IND='Y',REPNO=4);
%FULTR(FILENO=SLBR4,IND='Y',REPNO=5);

%MACRO ACTREC(FILENO=,IND=,REPNO=);
DATA ESLTR;
SET &FILENO;
WHERE DI_VLD_ADR = &IND;
RUN;

DATA _NULL_;
SET ESLTR;
FILE REPORT&REPNO DELIMITER=',' DSD DROPOVER LRECL=32767;

FORMAT DF_PRS_ID $9. ;

DO;
	PUT DF_PRS_ID $ ;
	END;
RUN;
%MEND ;

%ACTREC(FILENO=SLBR1,IND='N',REPNO=6);
%ACTREC(FILENO=SLBR2,IND='N',REPNO=7);
%ACTREC(FILENO=SLBR3,IND='N',REPNO=8);
%ACTREC(FILENO=SLBR4,IND='N',REPNO=9);
