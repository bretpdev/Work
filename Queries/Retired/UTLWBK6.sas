/*	Select Chapter 7 and 13 bankruptices if the account is in an 01 pending bankruptcy
	status, if the Complaint Received date is greater than 180 days, and
	if no action code of DBKRW exists indicating a review in the last 30 days.  
	Queue will be used to review for discharge or dismissal of adversary proceedings. MC */

LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWBK6.LWBK6R2";

/*RSUBMIT ;*/
OPTIONS		NOCENTER DATE NUMBER LS=136  ;
PROC SQL  ;
CONNECT TO DB2 (DATABASE=dlgsutwh);
CREATE TABLE DEANF1 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT		integer(a.BF_SSN)			AS SSN
			,b.LD_CPT_RCV				AS RCV
			,b.LC_BKR_STA				AS STATUS
			,b.LC_BKR_CHP				AS CHAPCODE
FROM	OLWHRM1.DC01_LON_CLM_INF 	a 
			inner join
		OLWHRM1.DC18_BKR			b
			on	 a.AF_APL_ID = b.AF_APL_ID			
			and  a.AF_APL_ID_SFX = b.AF_APL_ID_SFX
			and	 b.LC_BKR_STA = '01'			/*pending (open) bankruptcy*/
			and  b.LC_BKR_CHP in ('01','04')	/*chapter 13 and 7 bankruptcies*/
			AND (days(current date) - days(b.LD_CPT_RCV)) > 180	
			/*Complaint date older than 6 mos - NULL more than 99% of the time */
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
CREATE TABLE DEANF2 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	 max(BD_ATY_PRF)		AS MAXREVIEW
	,integer(DF_PRS_ID)			AS SSN 			 		
from 	OLWHRM1.AY01_BR_ATY 	 
where 	PF_ACT= 'DBKRW'
group by	DF_PRS_ID 
)  ;

DISCONNECT FROM DB2;
QUIT  ;

PROC SORT data=DEANF1 NODUPKEY;
BY SSN;
PROC SORT data=DEANF2 NODUPKEY;
BY SSN;
DATA DEANF;
MERGE DEANF1 DEANF2;
BY SSN;
RUN;
DATA DEANF;
SET DEANF;
IF CHAPCODE = '01' THEN CHAPCODE = '07';
ELSE IF CHAPCODE = '04' THEN CHAPCODE = '13';
RUN;

/*endrsubmit  ;*/

/*libname  WORKMATT  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;*/
OPTIONS		NOCENTER NODATE NUMBER PAGENO=1 LS=80  ;

PROC PRINTTO PRINT=REPORT2;
RUN;

PROC PRINT SPLIT='/' DATA = DEANF n='Total number of accounts = ' NOOBS;
var SSN CHAPCODE RCV MAXREVIEW;
format  RCV MMDDYY10. MAXREVIEW MMDDYY10.;
title1 "Open Chapter 7 & 13 Bankruptcies with a Complaint Received date";
title2 'more than 6 months old and no review (DBKRW) in the last 30 days.';
label	RCV = 'Complaint Received Date' MAXREVIEW = 'Last Review Date'
		CHAPCODE = 'Chapter';
where STATUS IN ('01') AND CHAPCODE IN ('07','13');
RUN;