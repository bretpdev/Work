/*THIS SPECIFICIES WHAT DIRECTORY TO READ REPORTS FROM AND WHICH DATABASE TO UPDATE.*/
/*LIVE*/
%INCLUDE "X:\Sessions\Local SAS Schedule\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas";
%LET REPORT = X:\PADD\FTP;

/*TEST*/
/*%INCLUDE "X:\PADU\SAS\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas" ;
%LET REPORT = T:\SAS;*/

%FILECHECK(30);

DATA LN56 (drop=trunc);
	INFILE REPORT30 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
	INPUT DF_SPE_ACC_ID :$10. LN_SEQ :6. LC_RPT_STA_CRB :$2. LD_RPT_CRB :$10. DT_ADJ :$10. ;
	RPT_STA_CRB = PUT(LC_RPT_STA_CRB,$STACRB.);
%TRUNCAT(30,DF_SPE_ACC_ID);
RUN;
%copyerror;

PROC SORT DATA=LN56 ; BY DF_SPE_ACC_ID LN_SEQ LD_RPT_CRB; RUN;
DATA LN56; SET LN56; BY DF_SPE_ACC_ID LN_SEQ LD_RPT_CRB; IF LAST.LD_RPT_CRB; RUN;

%ENCHILADA(LN56,LN56_CREDITREPORT,DF_SPE_ACC_ID LN_SEQ LD_RPT_CRB,LC_RPT_STA_CRB DT_ADJ RPT_STA_CRB,);

%GOODBYE_NULL(LN56_CREDITREPORT,LC_RPT_STA_CRB,'');
%GOODBYE_NULL(LN56_CREDITREPORT,DT_ADJ,'');
%GOODBYE_NULL(LN56_CREDITREPORT,RPT_STA_CRB,'');

DATA _NULL_;
	CALL SYMPUT('YR_AGO',PUT(INTNX('YEAR',TODAY(),-1,'S'),MMDDYY10.));
RUN;
%PUT &YR_AGO;

PROC SQL NOPRINT;
CONNECT TO ODBC AS MD (&MD);
SELECT COUNT(*) 
FROM CONNECTION TO MD (
      TRUNCATE TABLE ZDEL_LN56_CREDITREPORT;
      DELETE 
      FROM LN56_CREDITREPORT
	  WHERE CAST(LD_RPT_CRB AS DATETIME) < &YR_AGO

      );
QUIT;

%FINISH;
