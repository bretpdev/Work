/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORT2 "&RPTLIB/ULWO33.LWO33R2";*/
/*FILENAME REPORTZ "&RPTLIB/ULWO33.LWO33RZ";*/


options symbolgen;

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWO33.LWO33R2.txt";
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

SELECT A.BF_SSN AS SSN
		,B.DM_PRS_LST
		,A.LN_SEQ
		,A.LA_FAT_CUR_PRI
		,A.LD_FAT_EFF
		,B.DF_SPE_ACC_ID AS ACCTNUM
FROM	OLWHRM1.LN90_FIN_ATY A
INNER JOIN OLWHRM1.PD10_PRS_NME B
ON A.BF_SSN = B.DF_PRS_ID
WHERE PC_FAT_TYP = '10'
AND PC_FAT_SUB_TYP = '27'
AND LC_STA_LON90 = 'A'
AND (LC_FAT_REV_REA = '' OR LC_FAT_REV_REA = ' ' OR LC_FAT_REV_REA IS NULL)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA DEMO; SET WORKLOCL.DEMO; RUN;
PROC SQL;
CREATE TABLE MODANONE AS
SELECT SSN, LN_SEQ
,COUNT(*) AS CNT
FROM DEMO
GROUP BY SSN, LN_SEQ;
QUIT;

PROC SQL;
CREATE TABLE DEMO2 AS
SELECT A.ACCTNUM
		,A.DM_PRS_LST
		,A.LN_SEQ
		,A.LA_FAT_CUR_PRI
		,A.LD_FAT_EFF
		,MAX(A.LD_FAT_EFF) AS DT
FROM DEMO A
INNER JOIN (SELECT V.SSN,V.LN_SEQ
				FROM MODANONE V
				WHERE V.CNT > 1
			) B
ON B.SSN = A.SSN
AND B.LN_SEQ = A.LN_SEQ
GROUP BY A.ACCTNUM,DM_PRS_LST,A.LN_SEQ,LA_FAT_CUR_PRI,LD_FAT_EFF ;
QUIT;

PROC SORT DATA=DEMO2;
BY ACCTNUM;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

/*For portrait reports;*/
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS CENTER DATE NUMBER PAGENO=1 PS=52 LS=96;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO2;
FORMAT LA_FAT_CUR_PRI DOLLAR10.2
			LD_FAT_EFF MMDDYY10.;

VAR ACCTNUM DM_PRS_LST LN_SEQ LA_FAT_CUR_PRI LD_FAT_EFF;
LABEL	DM_PRS_LST = 'LAST NAME'
		LN_SEQ = 'Loan Sequence Number '
		LA_FAT_CUR_PRI = 'Most Recent 1027 Amount '
		LD_FAT_EFF = 'Most Recent 1027 Effective Date ';

TITLE 'Borrowers With More Than One Origination Fee Credit Transaction';
FOOTNOTE  'JOB = UTLWO33     REPORT = UTLWO33.LWO33R2';
RUN;

PROC PRINTTO;
RUN;
