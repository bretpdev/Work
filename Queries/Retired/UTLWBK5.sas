/*	Select Chapter 7 and 13 bankruptcies if the account is in an "03" discharged
	status, with a Complaint Received and/or Complaint Answered date prior to the 
	discharge date.  Queue will be used by management to ensure that the 
	account was appropriately discharged. MC */

LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWBK5.LWBK5R2";

/*RSUBMIT ;*/
OPTIONS		NOCENTER DATE NUMBER LS=136  ;
PROC SQL  ;
CONNECT TO DB2 (DATABASE=dlgsutwh);
CREATE TABLE DEANE AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT		integer(a.BF_SSN)			AS SSN
			,b.LD_CPT_RCV				AS RCV
			,b.LD_CPT_ANS				AS ANS
			,b.LD_BKR_DCH				AS DCH
			,b.LC_BKR_CHP				AS CHAPCODE
			,b.LC_BKR_STA				AS STATUS
FROM	OLWHRM1.DC01_LON_CLM_INF 	a 
			inner join
		OLWHRM1.DC18_BKR			b
			on	 a.AF_APL_ID = b.AF_APL_ID			
			and  a.AF_APL_ID_SFX = b.AF_APL_ID_SFX
			and	 b.LC_BKR_STA = '03'			/*discharged bankruptcy*/
			and  b.LC_BKR_CHP in ('01','04')	/*chapter 13 and 7 bankruptcies*/
			AND ((b.LD_CPT_RCV < b.LD_BKR_DCH)
				or 	(b.LD_CPT_ANS < b.LD_BKR_DCH))
	/*Complaint recieved date and/or complaint answered date prior to discharge date */
		)  ;
DISCONNECT FROM DB2;
QUIT  ;

PROC SORT data=DEANE NODUPKEY;
BY SSN;
RUN;
DATA DEANE;
SET DEANE;
IF CHAPCODE = '01' THEN CHAPCODE = '07';
ELSE IF CHAPCODE = '04' THEN CHAPCODE = '13';
RUN;

/*endrsubmit  ;*/

/*libname  WORKMATT  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;*/
OPTIONS		NOCENTER NODATE NUMBER PAGENO=1 LS=80  ;

PROC PRINTTO PRINT=REPORT2;
RUN;

PROC PRINT SPLIT='/' DATA = DEANE n='Total number of accounts = ' NOOBS;
var SSN CHAPCODE RCV ANS DCH;
format  RCV MMDDYY10. ANS MMDDYY10. DCH MMDDYY10.  ;
title1 "Chapter 7 & 13 Bankruptcies in status '03' (Discharged) with a Complaint Received";
title2 'and/or Complaint Answered date prior to the Discharge Date.';
label	RCV = 'Complaint Received Date' ANS = 'Complaint Answered Date'
		DCH = 'Discharge Date' CHAPCODE 'Chapter';
where STATUS = '03' AND CHAPCODE IN ('07','13');
RUN;
