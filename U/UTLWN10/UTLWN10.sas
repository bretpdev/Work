/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWN10.LWN10R2";
FILENAME REPORT3 "&RPTLIB/ULWN10.LWN10R3";
FILENAME REPORTZ "&RPTLIB/ULWN10.LWN10RZ";


OPTIONS SYMBOLGEN NOCENTER NODATE NONUMBER LS=132;
DATA _NULL_;		
	CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'"); /*beginning of previous month*/
	CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'END'), MMDDYYD10.)||"'"); /*end of previous month*/
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;


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
RTRIM(PD01.DM_PRS_LST) || ', ' || RTRIM(PD01.DM_PRS_1)  || ' ' || RTRIM(PD01.DM_PRS_MID) AS NAME
,RTRIM(PD01.DX_STR_ADR_1) || ' ' || RTRIM(PD01.DX_STR_ADR_2) AS ADDR
,RTRIM(PD01.DM_CT) || ', ' || PD01.DC_DOM_ST || ' ' || PD01.DM_FGN_CNY AS CITYST
,PD01.DF_ZIP
,PD01.DN_PHN
,PD01.DX_EML_ADR
,GA01.AF_ORG_APL_OPS_LDR


FROM OLWHRM1.GA10_LON_APP GA10
INNER JOIN OLWHRM1.GA01_APP GA01
	ON GA10.AF_APL_ID = GA01.AF_APL_ID
	AND GA01.AF_CUR_APL_OPS_LDR = '828476'
	AND GA01.AF_ORG_APL_OPS_LDR <> '828476'
INNER JOIN OLWHRM1.PD01_PDM_INF PD01
	ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID

WHERE GA10.AC_PRC_STA = 'A'
AND GA10.AD_PRC BETWEEN &BEGIN AND &END

FOR READ ONLY WITH UR

);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA DEMO; SET WORKLOCL.DEMO; RUN;


%MACRO GUAR(LID=,LNAME=,REPNO=);

DATA DEMO1;
SET DEMO;
WHERE AF_ORG_APL_OPS_LDR = "&LID";
RUN;

PROC PRINTTO PRINT=REPORT&REPNO NEW;
RUN;
/*PROC PRINTTO;*/
/*RUN;*/

/*For landscape reports:*/
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS CENTER NODATE NUMBER PAGENO=1 PS=39 LS=127;

PROC CONTENTS DATA=DEMO1 OUT=EMPTYSET NOPRINT;

/*LANDSCAPE*/
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 132*'-';
	PUT      ////////
		@51 '**** NO Loans Guaranteed in Previous Month ****';
	PUT ////////
		@57 '-- END OF REPORT --';
	PUT ////////////////
		@46 "JOB = UTLWN10     REPORT = ULWN10.LWN10R&REPNO";
	END;
RETURN;
TITLE "Referral Lender - Loans Guaranteed in Previous Month";
TITLE2 "&LNAME";
RUN;

PROC REPORT DATA = DEMO1 NOWD HEADLINE HEADSKIP SPLIT = '|' SPACING=2;
TITLE "Referral Lender - Loans Guaranteed in Previous Month";
TITLE2 "&LNAME";
FOOTNOTE  'JOB = UTLWN10     REPORT = UTLWN10.LWN10R&REPNO';

COLUMN NAME ADDR CITYST DF_ZIP DN_PHN DX_EML_ADR;
DEFINE 	NAME / 'BORROWER NAME' WIDTH = 20 FLOW;
DEFINE	ADDR / 'ADDRESS' WIDTH = 25 FLOW;
DEFINE  CITYST / 'CITY/STATE/COUNTRY' WIDTH = 20 FLOW;
DEFINE	DF_ZIP / 'ZIP' WIDTH = 9;
DEFINE	DN_PHN / 'PHONE' WIDTH = 10;
DEFINE	DX_EML_ADR / 'E-MAIL' WIDTH = 25 FLOW;

RUN;

PROC PRINTTO;
RUN;

%MEND GUAR;
%GUAR(LID=832382,LNAME=BEEHIVE CREDIT UNION,REPNO=2);
%GUAR(LID=817476,LNAME=GOLDENWEST CREDIT UNION,REPNO=3);
