/*TEST*/
/*%LET TESTMSG = - THIS IS A TEST;
%LET RPTLIB = T:\SAS;
%LET LOGLIB = Y:\Batch\Logs;
%LET CODELIB = Y:\Codebase\SAS\CDW;
%LET ODBCFILE = BSYS_TEST;
%LET DB = CDW_test;
%LET ODBCFILE_CSYS = CSYSTest;*/

/*LIVE*/
%LET TESTMSG = - THIS IS LIVE;
%LET RPTLIB = Z:\Batch\FTP;
%LET LOGLIB = Z:\Batch\Logs;
%LET CODELIB = Z:\Codebase\SAS\CDW;
%LET ODBCFILE = BSYS;
%LET DB = CDW;
%LET ODBCFILE_CSYS = CSYS;

%INCLUDE "&CODELIB\UTNWDW1 CDW DAILY UPDATE.FOLDER AND DATABASE.SAS";

%FILECHECK(20);

DATA LN16(drop=trunc);
	INFILE REPORT20 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
	INPUT DF_SPE_ACC_ID :$10. LN_SEQ :6. LD_DLQ_OCC :$10. LN_DLQ_MAX:4. 
	LD_DLQ_MAX:$10. ;
%TRUNCAT(20);
RUN;
%COPYERROR;
PROC SORT DATA=LN16 ; BY DF_SPE_ACC_ID LN_SEQ; RUN;
DATA LN16; SET LN16; BY DF_SPE_ACC_ID LN_SEQ; IF LAST.LN_SEQ; RUN;

%ENCHILADA(LN16,LN16_DELINQUENCY,DF_SPE_ACC_ID LN_SEQ,LD_DLQ_OCC LN_DLQ_MAX LD_DLQ_MAX,WHERE LN_DLQ_MAX > 0);

%GOODBYE_NULL(LN16_DELINQUENCY,LN_DLQ_MAX,0);
%GOODBYE_NULL(LN16_DELINQUENCY,LD_DLQ_OCC,'');
%GOODBYE_NULL(LN16_DELINQUENCY,LD_DLQ_MAX,'');

DATA _NULL_;
	CALL SYMPUT('THIS_YR',PUT(YEAR(TODAY()),4.));
	CALL SYMPUT('NEXT_YR',PUT(YEAR(TODAY())+1,4.));
RUN;

DATA HOLIDAYS (KEEP=HOL OBHOL) ;
FORMAT HOL OBHOL MMDDYY10.;
DO HOL="01JAN&THIS_YR"D TO "01JAN&NEXT_YR"d;
	MNTH = MONTH(HOL);
	WKDY = WEEKDAY(HOL);
	C = CEIL(DAY(HOL)/7);
	IF WKDY = 1 THEN OBHOL = HOL+1;
		ELSE IF WKDY = 7 THEN OBHOL = HOL-1;
		ELSE OBHOL = HOL;
	IF HOL IN ("01JAN&NEXT_YR"d,"01JAN&THIS_YR"d,"04JUL&THIS_YR"d,"24JUL&THIS_YR"d,
	"25DEC&THIS_YR"d,"11NOV&THIS_YR"D) THEN OUTPUT;
	ELSE IF MNTH = 1 AND WKDY = 2 AND C=3 THEN OUTPUT ; *Martin Luther Kings Birthday;
	ELSE IF MNTH = 2 AND WKDY = 2 AND C=3 THEN OUTPUT; *Washingtons Birthday;
	ELSE IF MNTH = 11 AND WKDY = 5 AND C=4 THEN OUTPUT; *Thanksgiving;
	ELSE IF MNTH = 5 AND WKDY = 2 AND DAY(HOL) > 24 THEN OUTPUT ; *Memorial Day;
	ELSE IF MNTH = 9 AND WKDY = 2 AND DAY(HOL) <= 7 THEN OUTPUT ; *Labor Day;
	ELSE IF MNTH = 11 AND WKDY = 6 AND 22 < DAY(HOL) < 30 THEN OUTPUT ; *Black Friday;
	ELSE IF MNTH = 10 AND WKDY = 2 AND C=2  THEN OUTPUT ; *Colombus Day;

END;
RUN;

DATA BUSDAY;
FORMAT BD MMDDYY10.;
	DO BD = TODAY()-1 TO TODAY()-7 by -1;
		IF WEEKDAY(BD) IN (2 3 4 5 6) THEN OUTPUT;
	END;
RUN;

PROC SQL noprint;
SELECT MAX(BD) INTO: LAST_BUS 
FROM BUSDAY A
LEFT OUTER JOIN HOLIDAYS B
	ON A.BD = B.OBHOL
WHERE B.OBHOL IS NULL;
QUIT;

PROC SQL;
CREATE TABLE BORR_dlq AS
	SELECT DISTINCT DF_SPE_ACC_ID
		,put(min(input(ld_dlq_occ,mmddyy10.)),mmddyy10.) as ld_dlq_occ
		,CASE
			WHEN max(INPUT(LD_DLQ_MAX,MMDDYY10.)) >= &LAST_BUS 
				THEN MAX(ln_dlq_max) + (TODAY() - max(INPUT(LD_DLQ_MAX,MMDDYY10.)))			
			WHEN max(INPUT(LD_DLQ_MAX,MMDDYY10.)) < &LAST_BUS 
				THEN MAX(LN_DLQ_MAX)
		END as cur_dlq
	FROM DB.LN16_DELINQUENCY
	GROUP BY DF_SPE_ACC_ID;
QUIT;
PROC SORT DATA=BORR_DLQ; BY DF_SPE_ACC_ID; RUN;

%ENCHILADA(BORR_dlq,BORR_DELINQUENCY,DF_SPE_ACC_ID,ld_dlq_occ cur_dlq,);

%CLEANUP_PARENT(BORR_DELINQUENCY,LN16_DELINQUENCY);

%FINISH(SOURCE=Delinquency);
