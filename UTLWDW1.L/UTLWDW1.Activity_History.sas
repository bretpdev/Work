/*THIS SPECIFICIES WHAT DIRECTORY TO READ REPORTS FROM AND WHICH DATABASE TO UPDATE.*/
/*LIVE*/
%INCLUDE "X:\Sessions\Local SAS Schedule\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas";
%LET REPORT = X:\PADD\FTP;

/*TEST*/
/*%INCLUDE "X:\PADU\SAS\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas" ;
%LET REPORT = T:\SAS;*/

%FILECHECK(28);

DATA ARCHIST(drop=trunc) ;
	INFILE REPORT28 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
	INPUT DF_SPE_ACC_ID :$10. LN_ATY_SEQ :6. LC_STA_ACTY10 :$1. PF_REQ_ACT :$5. PF_RSP_ACT :$5.
		PX_ACT_DSC_REQ :$20. LD_ATY_REQ_RCV :$10. LD_ATY_RSP :$10. LF_USR_REQ_ATY :$8. LT_ATY_RSP :$8.
LX_ATY :$360. ;
%TRUNCAT(28,DF_SPE_ACC_ID);
RUN;
%copyerror;

PROC SORT DATA=ARCHIST ; BY DF_SPE_ACC_ID LN_ATY_SEQ; RUN;
DATA ARCHIST; SET ARCHIST; BY DF_SPE_ACC_ID LN_ATY_SEQ; IF LAST.LN_ATY_SEQ; RUN;

%ENCHILADA(ARCHIST,AY10_HISTORY,DF_SPE_ACC_ID LN_ATY_SEQ
	,LC_STA_ACTY10 PF_REQ_ACT PF_RSP_ACT PX_ACT_DSC_REQ LD_ATY_REQ_RCV LD_ATY_RSP LF_USR_REQ_ATY LT_ATY_RSP LX_ATY
	,WHERE LC_STA_ACTY10 = 'A');
/*%MACRO ENCHILADA(INSET,SQLTBL,PRIKEY,ATTR,FILTER);*/

%GOODBYE_NULL(AY10_HISTORY,PF_REQ_ACT,'');
%GOODBYE_NULL(AY10_HISTORY,PF_RSP_ACT,'');
%GOODBYE_NULL(AY10_HISTORY,PX_ACT_DSC_REQ,'');
%GOODBYE_NULL(AY10_HISTORY,LD_ATY_REQ_RCV,'');
%GOODBYE_NULL(AY10_HISTORY,LD_ATY_RSP,'');
%GOODBYE_NULL(AY10_HISTORY,LT_ATY_RSP,'');
%GOODBYE_NULL(AY10_HISTORY,LX_ATY,'');


DATA _NULL_;
	CALL SYMPUT('MON_AGO',"'" || PUT(INTNX('MONTH',TODAY(),-1,'S'),DATE9.) || "'D");
RUN;
%PUT &MON_AGO;
PROC SQL;
CREATE TABLE EX_HIST AS
SELECT DF_SPE_ACC_ID
	,LN_ATY_SEQ
	,INPUT(LD_ATY_REQ_RCV,MMDDYY10.) AS DT
FROM UDW.AY10_HISTORY
GROUP BY DF_SPE_ACC_ID
HAVING COUNT(*) > 5
	AND MIN(INPUT(LD_ATY_REQ_RCV,MMDDYY10.)) < &MON_AGO;
QUIT;

PROC SORT DATA=EX_HIST; BY DF_SPE_ACC_ID DESCENDING LN_ATY_SEQ; RUN;
DATA EX_HIST(KEEP=DF_SPE_ACC_ID LN_ATY_SEQ);
SET EX_HIST;
BY DF_SPE_ACC_ID;
RETAIN A 0;
IF FIRST.DF_SPE_ACC_ID THEN A = 0;
A = A + 1;
IF A > 5 AND DT < &MON_AGO THEN OUTPUT;
RUN;

PROC SQL NOPRINT;
INSERT INTO UDW.ZDEL_AY10_HISTORY
      SELECT DF_SPE_ACC_ID
	  		,LN_ATY_SEQ
      FROM EX_HIST; 
QUIT;

PROC SQL NOPRINT;
CONNECT TO ODBC AS MD (&MD);
SELECT COUNT(*) 
FROM CONNECTION TO MD (
      DELETE AY10_HISTORY
      FROM AY10_HISTORY A
      INNER JOIN ZDEL_AY10_HISTORY B
            ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
			AND A.LN_ATY_SEQ = B.LN_ATY_SEQ;
);
DISCONNECT FROM MD;
QUIT;
PROC SQL NOPRINT;
CONNECT TO ODBC AS MD (&MD);
SELECT COUNT(*) 
FROM CONNECTION TO MD (
      TRUNCATE TABLE ZDEL_AY10_HISTORY 
);
DISCONNECT FROM MD;
QUIT;

%FINISH;
