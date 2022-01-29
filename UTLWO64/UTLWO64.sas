/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWO64.LWO64R2";
FILENAME REPORTZ "&RPTLIB/ULWO64.LWO64RZ";

DATA _NULL_;		
	EFFDT = TODAY() - 1; /*GETS THE PREVIOUS DAY*/
	CALL SYMPUT('RUNDATE',"'"||put(EFFDT,MMDDYY10.)||"'"); /*GETS THE PREVIOUS DAY*/
RUN;
%SYSLPUT RUNDATE = &RUNDATE;

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
SELECT DISTINCT A1.DF_SPE_ID
	,A2.DM_PRS_1 
	,A2.DM_PRS_LST
FROM OLWHRM1.PH01_SUPER_ID A1
INNER JOIN OLWHRM1.PD10_PRS_NME A2
ON A1.DF_PRS_ID = A2.DF_PRS_ID
INNER JOIN (SELECT A.BF_SSN AS SSN
			FROM	OLWHRM1.AY10_BR_LON_ATY A
			INNER JOIN OLWHRM1.LN10_LON B
			ON A.BF_SSN = B.BF_SSN
			AND B.LA_CUR_PRI > 0
			AND B.LC_STA_LON10 = 'R'
			AND B.IC_LON_PGM IN ('STFFRD', 'UNSTFD', 'PLUS', 'PLUSGB', 'SLS')
			INNER JOIN (SELECT BB.BF_SSN 
						,COUNT(*) AS CNT
						FROM OLWHRM1.LN10_LON BB
						INNER JOIN OLWHRM1.DW01_DW_CLC_CLU CC
						ON BB.BF_SSN = CC.BF_SSN
						and BB.LN_SEQ = CC.LN_SEQ
						AND CC.WC_DW_LON_STA IN ('01', '03', '04', '05', '13', '14', '15', '16', '20')
						WHERE BB.LA_CUR_PRI > 0
						AND BB.LC_STA_LON10 = 'R'
						AND BB.IC_LON_PGM IN ('STFFRD', 'UNSTFD', 'PLUS', 'PLUSGB', 'SLS')
						GROUP BY BB.BF_SSN) C
			ON A.BF_SSN = C.BF_SSN
			INNER JOIN (SELECT Q.BF_SSN FROM (SELECT BB.BF_SSN, SUM(BB.LA_CUR_PRI) AS LA_CUR_PRI
						FROM OLWHRM1.LN10_LON BB
						INNER JOIN OLWHRM1.DW01_DW_CLC_CLU CC
						ON BB.BF_SSN = CC.BF_SSN
						and BB.LN_SEQ = CC.LN_SEQ
						AND CC.WC_DW_LON_STA IN ('01', '03', '04', '05', '13', '14', '15', '16', '20')
						GROUP BY BB.BF_SSN) Q
						WHERE Q.LA_CUR_PRI > 1000
						) D
			ON A.BF_SSN = D.BF_SSN
/*			AND C.CNT > 1*/
			WHERE A.PF_REQ_ACT = 'H138Q'
			AND DAYS(A.LD_ATY_REQ_RCV) = DAYS(&RUNDATE) 
			UNION
			SELECT A.BF_SSN AS SSN
			FROM	OLWHRM1.AY10_BR_LON_ATY A
			INNER JOIN OLWHRM1.LN10_LON B
			ON A.BF_SSN = B.BF_SSN
			AND B.LA_CUR_PRI > 0
			AND B.LC_STA_LON10 = 'R'
			AND B.IC_LON_PGM IN ('CNSLDN', 'SUBCNS', 'UNCNS', 'SUBSPC', 'UNSPC')
			INNER JOIN (SELECT BB.BF_SSN 
						FROM OLWHRM1.LN10_LON BB
						INNER JOIN OLWHRM1.DW01_DW_CLC_CLU CC
						ON BB.BF_SSN = CC.BF_SSN
						and BB.LN_SEQ = CC.LN_SEQ
						AND CC.WC_DW_LON_STA IN ('01', '03', '04', '05', '13', '14', '15', '16', '20')
						WHERE BB.LA_CUR_PRI > 0
						AND BB.LC_STA_LON10 = 'R'
						AND BB.IC_LON_PGM IN ('STFFRD', 'UNSTFD', 'PLUS', 'PLUSGB', 'SLS')
						) C
			ON A.BF_SSN = C.BF_SSN
			INNER JOIN (SELECT Q.BF_SSN FROM (SELECT BB.BF_SSN, SUM(BB.LA_CUR_PRI) AS LA_CUR_PRI
						FROM OLWHRM1.LN10_LON BB
						INNER JOIN OLWHRM1.DW01_DW_CLC_CLU CC
						ON BB.BF_SSN = CC.BF_SSN
						and BB.LN_SEQ = CC.LN_SEQ
						AND CC.WC_DW_LON_STA IN ('01', '03', '04', '05', '13', '14', '15', '16', '20')
						GROUP BY BB.BF_SSN) Q
						WHERE Q.LA_CUR_PRI > 1000
						) D
			ON A.BF_SSN = D.BF_SSN
			WHERE A.PF_REQ_ACT = 'H138Q'
			AND DAYS(A.LD_ATY_REQ_RCV) = DAYS(&RUNDATE)
			) A3
ON A1.DF_PRS_ID = A3.SSN


FOR READ ONLY WITH UR

);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA DEMO; SET WORKLOCL.DEMO; RUN;

PROC SORT DATA=DEMO;
BY DF_SPE_ID;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

/*For portrait reports;*/
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS CENTER NODATE NUMBER PAGENO=1 PS=52 LS=96;
PROC CONTENTS DATA=DEMO OUT=EMPTYSET NOPRINT;
/*PORTRAIT*/
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 96*'-';
	PUT      ////////
		@35 '**** NO RECORDS FOUND ****';
	PUT ////////
		@38 '-- END OF REPORT --';
	PUT ////////////////
		@27 "JOB = UTLWO64     REPORT = ULWO64.LWO64R2";
	END;
RETURN;
TITLE "Deferment End Letter Produced";
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO;
VAR DF_SPE_ID DM_PRS_1 DM_PRS_LST;
LABEL	DF_SPE_ID = 'Account ID'
		DM_PRS_1 = 'First Name'
		DM_PRS_LST = 'Last Name';

TITLE 'Deferment End Letter Produced';
FOOTNOTE  'JOB = UTLWO64     REPORT = UTLWO64.LWO64R2';
RUN;

/*PROC EXPORT DATA=DEMO*/
/*            OUTFILE= "T:\SAS\DEMO.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/


