PROC IMPORT OUT= WORK.SAUCE
            DATAFILE= "X:\Archive\BANA\Production Files\28th due date issue\ACH for 28th not drawn--full list.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="A$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

Proc sql;
CREATE TABLE SOURCE AS
SELECT DISTINCT
	DF_SPE_ACC_ID
FROM SAUCE
;
QUIT;

DATA Duster.SOURCE;
set Work.SOURCE;
run;

RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
LIBNAME PROGREVW '/sas/whse/progrevw';

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
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
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE DEMO(compress=yes) AS
		SELECT
			LN90.BF_SSN
			,LN90.LN_SEQ
			,LN90.LN_FAT_SEQ
			,LN90.OLD_FAT_EFF
			,'02/28/2016' AS NEW_FAT_EFF
		FROM 
			SOURCE S
			JOIN OLWHRM1.PD10_PRS_NME PD10
				ON S.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			JOIN 
				(
					SELECT
						BF_SSN,
						LN_SEQ,
						LN_FAT_SEQ,
						LD_FAT_EFF AS OLD_FAT_EFF
					FROM
						OLWHRM1.LN90_FIN_ATY
					WHERE
						LC_STA_LON90 = 'A'
						AND PC_FAT_TYP = '10' 
						AND PC_FAT_SUB_TYP = '10'
						AND LD_FAT_EFF > MDY(02,28,2016)
				) LN90 ON PD10.DF_PRS_ID = LN90.BF_SSN
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;
PROC SORT DATA=DEMO;
	BY SSN;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\EXCEL OUTPUT.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
