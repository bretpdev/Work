/*	Select Chapter 13 bankruptcies if the account is in an 01 pending bankruptcy
	status, if the account has a filing date that is greater than 180 days, and
	if no action code of DBKRW exists indicating a review in the last 30 days.  
	Queue will be used to review for discharge or dismissal.  MC  */

LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWBK1.LWBK1R2";

/*RSUBMIT ;*/
OPTIONS		NOCENTER DATE NUMBER LS=136  ;
PROC SQL  ;
CONNECT TO DB2 (DATABASE=dlgsutwh);
CREATE TABLE DEANA1 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	 a.BF_SSN				AS SSN
		,b.LD_BKR_FIL			AS FILEDT
		,b.LC_BKR_STA			AS STATUS
		,b.LC_BKR_CHP			AS CHAPCODE
FROM  	OLWHRM1.DC01_LON_CLM_INF 	a 
	inner join
	OLWHRM1.DC18_BKR		b
	on	 a.AF_APL_ID = b.AF_APL_ID
	     and  a.AF_APL_ID_SFX = b.AF_APL_ID_SFX
	     and  b.LC_BKR_STA = '01'
	     and  b.LC_BKR_CHP = '04'
	     and  days(current date) - days(b.LD_BKR_FIL) > 180
WHERE	not exists 
	(select 	*
	 from 	OLWHRM1.AY01_BR_ATY		c
	 where 	c.DF_PRS_ID = a.BF_SSN
	     and	c.PF_ACT= 'DBKRW'
	     and	days(current date) - days(c.BD_ATY_PRF) < 31
	)
)  ;
/******************************************************************************************/
/*** Read AY01 to id most recent instance of activity code DBKRW  any application       ***/
/******************************************************************************************/
CREATE TABLE DEANA2 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	 max(BD_ATY_PRF)		AS MAXREVIEW
		,DF_PRS_ID				AS SSN 			 		
from 	OLWHRM1.AY01_BR_ATY 	 
where 	PF_ACT= 'DBKRW'
group by	DF_PRS_ID 
)  ;
DISCONNECT FROM DB2;
QUIT  ;
PROC SORT data=DEANA1 NODUPKEY;
BY SSN;
PROC SORT data=DEANA2 NODUPKEY;
BY SSN;
DATA DEANA;
MERGE DEANA1 DEANA2;
BY SSN;
RUN;
/*endrsubmit  ;*/

/*libname  WORKMATT  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;*/
OPTIONS		NOCENTER NODATE NUMBER PAGENO=1 LS=80  ;

PROC PRINTTO PRINT=REPORT2;
RUN;

PROC PRINT SPLIT='/' DATA = DEANA n='Total number of accounts = ' NOOBS;
var SSN FILEDT MAXREVIEW;
format  FILEDT MMDDYY10. MAXREVIEW MMDDYY10.;
title1 'Open Chapter 13 Bankruptcies older than 6 months';
title2 'with no review (DBKRW) in the last 30 days.';
label	FILEDT = 'Filing Date' 
		MAXREVIEW = 'Last Review Date';
where STATUS = '01' AND CHAPCODE = '04';
RUN;
