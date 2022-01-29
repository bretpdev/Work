/*Privacy Disclosures fo Wells Fargo*/
/*UTLWE23*/
/*PLEASE NOTE: THIS DIRECTORY HAS BEEN CHANGED TO PROGREVW BECAUSE OF THE SIZE OF THE OUTPUT FILE*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = /sas/whse/progrevw;*/
/*FILENAME REPORT2 "&RPTLIB/ULWE23.LWE23R2";*/
/*FILENAME REPORT3 "&RPTLIB/ULWE23.LWE23R3";*/

FILENAME REPORT2 "T:\SAS\ULWE23.LWE23R2";
FILENAME REPORT3 "T:\SAS\ULWE23.LWE23R3";

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;

OPTIONS SYMBOLGEN;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE PIVY AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
A.BF_SSN AS SSN
,CASE 
	WHEN RTRIM(C.DM_PRS_MID) <> ' ' 
		THEN RTRIM(C.DM_PRS_1)||' '||RTRIM(C.DM_PRS_MID)||' '||RTRIM(C.DM_PRS_LST)
	WHEN RTRIM(C.DM_PRS_MID) = ' ' 
		THEN RTRIM(C.DM_PRS_1)||' '||RTRIM(C.DM_PRS_LST)
END AS NAME
,D.DX_STR_ADR_1 AS STR_1
,D.DX_STR_ADR_2 AS STR_2
,D.DX_STR_ADR_3 AS STR_3
,D.DM_CT AS CITY
,CASE 
	WHEN D.DC_DOM_ST  = ''
		THEN D.DM_FGN_ST
	WHEN D.DC_DOM_ST <> ''
		THEN D.DC_DOM_ST
 END AS STATE
,D.DF_ZIP_CDE AS ZIP
,D.DM_FGN_ST AS FGN_STATE
,D.DM_FGN_CNY AS FGN_COUNTRY
,D.DI_VLD_ADR
,D.DC_ADR

FROM  OLWHRM1.LN10_LON A 
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.PD10_PRS_NME C
	ON A.BF_SSN = C.DF_PRS_ID
INNER JOIN OLWHRM1.PD30_PRS_ADR D
	on A.BF_SSN = D.DF_PRS_ID
	AND D.DC_ADR = 'L'
WHERE A.LF_LON_CUR_OWN = '813894'
AND A.LC_STA_LON10 = 'R'
AND A.LA_CUR_PRI > 0
ORDER BY A.BF_SSN
);
DISCONNECT FROM DB2;
endrsubmit  ;

DATA PIVY; 
SET WORKLOCL.PIVY; 
RUN;

*CALCULATE KEYLINE;
DATA PIVY (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
ACSID = '#BWNHDFH#';
LENGTH ACSKEY $ 18;
SET PIVY;
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

DATA PIVY_V (DROP=DI_VLD_ADR DC_ADR)
PIVY_I (DROP=DI_VLD_ADR DC_ADR);
SET PIVY;
IF DI_VLD_ADR = 'Y' THEN OUTPUT PIVY_V;
ELSE OUTPUT PIVY_I;
RUN;


DATA _NULL_;
SET  PIVY_V;
FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;
PUT ACSID $ @ ;
PUT ACSKEY $ @;
PUT NAME $ @;
PUT STR_1 $ @;
PUT STR_2 $ @;
PUT STR_3 $ @;
PUT CITY $ @;
PUT STATE $ @;
PUT ZIP $ @;
PUT FGN_COUNTRY $; 

RUN;

DATA _NULL_;
SET  PIVY_I;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
PUT SSN $ @;
PUT NAME $ @;
PUT STR_1 $ @;
PUT STR_2 $ @;
PUT STR_3 $ @;
PUT CITY $ @;
PUT STATE $ @;
PUT ZIP $ @;
PUT FGN_COUNTRY $; 

RUN;

/*PROC EXPORT DATA= PIVY */
/*            OUTFILE= "T:\SAS\DEMO.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/


/*PROC EXPORT DATA= PIVY_V */
/*            OUTFILE= "T:\SAS\&LID.valid.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
/*PROC EXPORT DATA= PIVY_I*/
/*            OUTFILE= "T:\SAS\&LID.invalid.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/