/*	Select Chapter 13 bankruptcies if the account is in an 01 pending bankruptcy
	status, if no trustee payment "TP" has been posted within the last 90 days,
	no action code of DBKRW or BXMIS exists indicating a review in the 
	last 30 days, and the filing date is > 180 days prior to current.
	Queue will be used to review for discharge or dismissal.  MC  */

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWBK3.LWBK3R2";
FILENAME REPORTZ "&RPTLIB/ULWBK3.LWBK3RZ";

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT ;
%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;
PROC SQL STIMER ;
CONNECT TO DB2 (DATABASE=dlgsutwh);
CREATE TABLE DEANC1 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT		a.BF_SSN	AS SSN				
			,b.LC_BKR_STA				
			,CASE 
				WHEN B.LC_BKR_CHP = '01' THEN '07'
				WHEN B.LC_BKR_CHP = '14' THEN '13'
				WHEN B.LC_BKR_CHP = '02' THEN '11'
				WHEN B.LC_BKR_CHP = '03' THEN '12'
				ELSE B.LC_BKR_CHP
	 		 END AS LC_BKR_CHP				
			,b.LD_BKR_FIL				
			,B.LF_BKR_DKT

FROM  		OLWHRM1.DC01_LON_CLM_INF 	a 
				inner join
			OLWHRM1.DC18_BKR			b
				on	 a.AF_APL_ID = b.AF_APL_ID
				and  a.AF_APL_ID_SFX = b.AF_APL_ID_SFX
				and	 b.LC_BKR_STA = '01'		/*PENDING (OPEN) ACCTS ONLY*/
				and  b.LC_BKR_CHP = '04'		/*CHAPTER 13 (04) ACCTS ONLY*/
				and  days(current date) - days(b.LD_BKR_FIL) > 180
WHERE not exists 
	(select *					/*No review in last 30 days*/
	from OLWHRM1.AY01_BR_ATY		c
	where c.DF_PRS_ID = a.BF_SSN
	and	c.PF_ACT in ('DBKRW','BXMIS')
	and	days(current date) - days(c.BD_ATY_PRF) < 31
	)
and not exists 
	(select *					/*No Trustee Pmt in last 90 days*/
	from OLWHRM1.DC11_LON_FAT		d
	where d.BF_SSN = a.BF_SSN
	and	d.LC_TRX_TYP = 'TP'
	and	days(current date) - days(d.BD_TRX_PST_HST) < 91
	)
AND A.LC_STA_DC10 = '03'
AND NOT EXISTS (
	SELECT *
	FROM OLWHRM1.CT30_CALL_QUE X
	WHERE X.DF_PRS_ID_BR = a.BF_SSN
	AND X.IF_WRK_GRP = 'BKCP13RV'
	)
);

CREATE TABLE DEANC2 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	 max(BD_ATY_PRF)		AS MAXREVIEW
	,DF_PRS_ID			AS SSN 			 		
from 	OLWHRM1.AY01_BR_ATY 	 
where 	PF_ACT in ('DBKRW','BXMIS')
group by	DF_PRS_ID 
)  ;

CREATE TABLE DEANC3 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	 max(BD_TRX_PST_HST)	AS MAXTP
	,BF_SSN		AS SSN 			 		
from OLWHRM1.DC11_LON_FAT
where LC_TRX_TYP = 'TP'
group by BF_SSN
)  ;

DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWBK3.LWBK3RZ);*/
/*QUIT;*/
PROC SORT data=DEANC1 NODUPKEY;
BY SSN;
PROC SORT data=DEANC2 NODUPKEY;
BY SSN;
PROC SORT data=DEANC3 NODUPKEY;
BY SSN;
DATA DEANC;
MERGE DEANC1 (IN=A) DEANC2 DEANC3;
BY SSN;
IF A;
RUN;

endrsubmit  ;
DATA DEANC;
SET WORKLOCL.DEANC;
RUN;

/*-----------------------------------------------------------------------------*
| THIS MACRO WILL CHANGE DATES INTO STIRNGS SO THEY CAN BE CONCATINATED IN THE |
| COMMENTS VARIABLE                                                            |
*-----------------------------------------------------------------------------*/
%MACRO DATE_2_CHAR(DS,CDATE,DAYPART);
DATA &DS;
SET &DS;
LENGTH &DAYPART $ 10. ;
IF &CDATE = . THEN DO;
	&DAYPART = '';
	END;
ELSE IF DAY(&CDATE) < 10 AND MONTH(&CDATE) < 10 THEN DO;
	&DAYPART = '0'||TRIM(LEFT(MONTH(&CDATE)))||'/'||'0'||TRIM(LEFT(DAY(&CDATE)))||'/'||TRIM(LEFT(YEAR(&CDATE)));
	END;
ELSE IF DAY(&CDATE) < 10 AND MONTH(&CDATE) >= 10 THEN DO;
	&DAYPART = TRIM(LEFT(MONTH(&CDATE)))||'/'||'0'||TRIM(LEFT(DAY(&CDATE)))||'/'||TRIM(LEFT(YEAR(&CDATE)));
	END;
ELSE IF DAY(&CDATE) >= 10 AND MONTH(&CDATE) < 10 THEN DO;
	&DAYPART ='0'||TRIM(LEFT(MONTH(&CDATE)))||'/'||TRIM(LEFT(DAY(&CDATE)))||'/'||TRIM(LEFT(YEAR(&CDATE)));
	END;
ELSE DO;
	&DAYPART = TRIM(LEFT(MONTH(&CDATE)))||'/'||TRIM(LEFT(DAY(&CDATE)))||'/'||TRIM(LEFT(YEAR(&CDATE)));
	END;
RUN;
%MEND DATE_2_CHAR;
%DATE_2_CHAR(DEANC,LD_BKR_FIL,CHAR_LD_BKR_FIL);
%DATE_2_CHAR(DEANC,MAXREVIEW,CHAR_MAXREVIEW);
/*------------------ VARIABLE INITIALIZATION ------------------*/
DATA DEANC (KEEP=TARGET_ID QUEUE_NAME INSTITUTION_ID INSTITUTION_TYPE DATE_DUE TIME_DUE LN_SEQ COMMENTS);
SET DEANC ;
LENGTH COMMENTS $ 600.;
TARGET_ID = SSN;
QUEUE_NAME = 'BKCP13RV';
INSTITUTION_ID = '';
INSTITUTION_TYPE = '';
DATE_DUE = '';
TIME_DUE = '';
COMMENTS = 'OneLINK BK Filed Date: '||TRIM(LEFT(CHAR_LD_BKR_FIL))||','||
	'BK Status: '||TRIM(LEFT(LC_BKR_STA))||','||
	'BK Chapter: '||TRIM(LEFT(LC_BKR_CHP))||','||
	'BK Doc: '||TRIM(LEFT(LF_BKR_DKT))||','||
	'Last RVW: '||TRIM(LEFT(CHAR_MAXREVIEW));
RUN;

DATA _NULL_;
SET  WORK.DEANC; 
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT COMMENTS $600. ;
   FORMAT TARGET_ID $9. ;
   FORMAT QUEUE_NAME $8. ;
   FORMAT INSTITUTION_ID $1. ;
   FORMAT INSTITUTION_TYPE $1. ;
   FORMAT DATE_DUE $1. ;
   FORMAT TIME_DUE $1. ;
DO;
	PUT TARGET_ID $ @;
	PUT QUEUE_NAME $ @;
	PUT INSTITUTION_ID $ @;
	PUT INSTITUTION_TYPE $ @;
	PUT DATE_DUE $ @;
	PUT TIME_DUE $ @;
	PUT COMMENTS $ ;
END;
RUN;
