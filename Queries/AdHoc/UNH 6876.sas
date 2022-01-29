/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS46.NWS46RZ";
FILENAME REPORT2 "&RPTLIB/UNWS46.NWS46R2";

PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\FHP 6864 non control Borrowers that resolved.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Sheet1$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;


LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.SOURCE;
	SET SOURCE;
	DF_SPE_ACC_ID = PUT(ACCOUNT_NUMBER, $z10.);
	KEEP DF_SPE_ACC_ID;
RUN;

RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUR DB2 DATABASE=&DB OWNER=PKUR;

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
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE DEMO AS

		SELECT DISTINCT
			S.DF_SPE_ACC_ID
			,LN16.LN_DLQ_MAX
		FROM
			SOURCE S
			INNER JOIN PKUB.PD10_PRS_NME PD10
				ON S.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT
						LN16.BF_SSN
						,MAX(LN16.LN_DLQ_MAX) AS LN_DLQ_MAX
					FROM 
						PKUB.LN16_LON_DLQ_HST LN16
					WHERE LN16.LC_STA_LON16 = '1'
					GROUP BY LN16.BF_SSN
						) LN16
				ON PD10.DF_PRS_ID = LN16.BF_SSN

	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH 6876.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
