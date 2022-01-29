/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

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

	CREATE TABLE CURE AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						LN10.BF_SSN
/*						,LN10.LN_SEQ*/
						,LN33.LC_CU_REA
						,LN33.LD_VIO_LON_CU
					FROM	
						OLWHRM1.LN10_LON LN10
						JOIN OLWHRM1.LN33_LON_CU_INF LN33
							ON LN10.BF_SSN = LN33.BF_SSN
							AND LN10.LN_SEQ = LN33.LN_SEQ
					WHERE	
						LN10.LA_CUR_PRI > 0
						AND 
						LN10.LC_STA_LON10 = 'R'
						AND 
						LN33.LD_CU_END IS NULL

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA CURE;
	SET DUSTER.CURE;
RUN;

DATA R2 R3 R4;
	SET CURE;
	IF LC_CU_REA = '05' THEN OUTPUT R2;
		ELSE IF LC_CU_REA = '19' THEN OUTPUT R3;
		ELSE OUTPUT R4;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.R2
            OUTFILE = "T:\SAS\NH 24065.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="R2"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.R3
            OUTFILE = "T:\SAS\NH 24065.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="R3"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.R4
            OUTFILE = "T:\SAS\NH 24065.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="R4"; 
RUN;
