* This program creates monthly stats for queues by user and unit. mc	.;

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWQ03.LWQ03R2";
FILENAME REPORT3 "&RPTLIB/ULWQ03.LWQ03R3";
FILENAME REPORT4 "&RPTLIB/ULWQ03.LWQ03R4";
FILENAME REPORT5 "&RPTLIB/ULWQ03.LWQ03R5";
FILENAME REPORT6 "&RPTLIB/ULWQ03.LWQ03R6";
FILENAME REPORT7 "&RPTLIB/ULWQ03.LWQ03R7";
FILENAME REPORT8 "&RPTLIB/ULWQ03.LWQ03R8";
FILENAME REPORT9 "&RPTLIB/ULWQ03.LWQ03R9";
FILENAME REPORT10 "&RPTLIB/ULWQ03.LWQ03R10";
FILENAME REPORT11 "&RPTLIB/ULWQ03.LWQ03R11";
FILENAME REPORT12 "&RPTLIB/ULWQ03.LWQ03R12";
FILENAME REPORT13 "&RPTLIB/ULWQ03.LWQ03R13";
FILENAME REPORT14 "&RPTLIB/ULWQ03.LWQ03R14";
FILENAME REPORT15 "&RPTLIB/ULWQ03.LWQ03R15";
FILENAME REPORT16 "&RPTLIB/ULWQ03.LWQ03R16";
FILENAME REPORT17 "&RPTLIB/ULWQ03.LWQ03R17";*/

LIBNAME Q V8 '\\Ftp\PHEAA_FTP\LGSQ';

OPTIONS SYMBOLGEN NODATE CENTER LS=80;

data _null_  ;
   begin=intnx('month',today(),-1,'beginning')  ; 	*Resolves to 1st day of previous month;
   call symput  ( 'begin', "'"||put(begin,DATE9.)||"'" )   ;

   end=intnx('month',today(),-1,'end')  ;			*Resolves to last day of prev month;
   call symput  ( 'end', "'"||put(end,DATE9.)||"'" )   ;

   EFFMO = PUT(INTNX('MONTH',TODAY(),-1), MONNAME9.);
   EFFYR = PUT(INTNX('MONTH',TODAY(),-1), YEAR4.);
   CALL SYMPUT('EFFDATE',TRIM(LEFT(EFFMO||' '||EFFYR)));	*Resolves to Monthname,CCYY of prev mo;
run;

DATA WORK.QSTATLOCAL; *DO NOT TOUCH Q.QSTATS!;
SET Q.QSTATS;
RUN;

/*TESTING CODE BL0CK*/
/*DATA FAKEQ;*/
/*SET WORK.QSTATLOCAL;*/
/*RUN;*/
/*PROC SORT DATA=FAKEQ;*/
/*BY QUEUE;*/
/*RUN;*/
/*DATA FAKEQ;*/
/*SET FAKEQ;*/
/*IF QUEUE = 'URGEVRRQ' THEN QUEUE = 'QRHBFRCE';*/
/*RUN; */
/*DATA QTEMP;*/
/*SET FAKEQ;*/
/*IF MINS = 0 AND SECS > 0 THEN MINS = 1;*/
/*FORMAT DATE DATE9.;*/
/*IF NO = . THEN NO = 0;*/
/*IF MINS = . THEN MINS = 0; */
/*IF SECS = . THEN SECS = 0;*/
/*RUN;*/
/*END OF TESTING CODE BLOCK*/

/*COMMENT FOR TESTING*/
DATA QTEMP;
SET WORK.QSTATLOCAL;
IF MINS = 0 AND SECS > 0 THEN MINS = 1;
FORMAT DATE DATE9.;
IF NO = . THEN NO = 0;
IF MINS = . THEN MINS = 0; 
IF SECS = . THEN SECS = 0;
RUN;

/*SPECIAL RULES APPLIED HERE*/
DATA QTEMP;
SET QTEMP;
*Report (only) to Operations;
IF QUEUE IN ('QGARNERR','TCHRAPPL','DRMVDPIF','CNDDISAB','ZZZZCOMP','QRHBFRCE') 
THEN RULE = 'OP';
*Report to Database Quality;
IF QUEUE IN ('REINSTAX','REMVTRAD','FMPIFRVS','DRMVDPIF','QGARNERR','QREINSTA')
THEN RULE = 'DQ';
*Report to Support Services;
IF QUEUE IN ('REPURCHS') 
THEN RULE = 'SS';
*Report to Post Claims;
IF QUEUE IN ('LBANKRUP','DSUSPENS','SKIPPRSN','DSPANISH')
THEN RULE = 'PC';
*Report to both Post Claims and Support Services;
IF QUEUE IN ('QADDFEE') 
THEN RULE = 'PCSS'; 
*Report to Claims;
IF QUEUE IN ('REVWDISB')
THEN RULE = 'CM';
*No Reporting for these queues;
IF QUEUE IN ('DALLAS')
THEN RULE = 'NR';
*Report to Loan Origination;
IF QUEUE IN ('SNEWAPSK','CANCRFND')
THEN RULE = 'LD';
*Report to Account Services;
IF QUEUE IN ('EDEFRVW')
THEN RULE = 'AS';
*Report to Default Prevention;
IF QUEUE IN ('PRISONRQ')
THEN RULE = 'DP';
/*REPORT TO AUX SERV*/
IF QUEUE IN ('DBANKRUP','MREFRADD')
THEN RULE = 'AU';
RUN;

%MACRO QMONTH(RECIP=,GRP=,SPEC=,REPNO1=,REPNO2=);
OPTIONS PAGENO=1;

/*CREATE QSTATS REPORTS*/

PROC SORT DATA = QTEMP; BY GROUP QUEUE DATE USER; RUN;

PROC SQL;
	CREATE TABLE QTEMP2 AS
	SELECT GROUP, QUEUE, SUM(NO) AS NO, SUM(MINS) AS MINS, RULE
	,SUM(NO)/SUM(MINS)*60 FORMAT=6.1 AS AVGHR
	FROM QTEMP
	WHERE DATE BETWEEN &BEGIN.D AND &END.d
	AND RULE NOT CONTAINS 'NR'
	GROUP BY GROUP, QUEUE, RULE;
QUIT;
RUN;

/*Process Rules*/
/*Select for Departments by Group if no rule, or by Rule if rule specifies them*/
%IF &SPEC NE 'OP'
%THEN %DO;
	DATA QTEMP3;
	SET QTEMP2;
	WHERE ((GROUP IN (&GRP,'ALL') AND RULE = ' ') 
		OR (RULE CONTAINS &SPEC))
		AND RULE NOT CONTAINS 'OP';
	RUN;
%END;
%ELSE %DO; /*Select all for Operations, unless rule forces no reporting for queue.*/
	DATA QTEMP3;
	SET QTEMP2;
	RUN;
%END;

/*PROC PRINTTO PRINT=REPORT&REPNO1.;
RUN;*/
PROC PRINTTO PRINT="C:\WINDOWS\TEMP\QSTAT Monthly - &RECIP..rtf" NEW;
RUN;

%macro qmonthprint1(dsname);
%let dsid=%sysfunc(open(&dsname));
%let hasobs=%sysfunc(attrn(&dsid,any));
%let rc=%sysfunc(close(&dsid));
%if &hasobs=1 %then
	%do;
PROC PRINT DATA = &dsname NOOBS LABEL;
	VAR QUEUE MINS NO AVGHR;
	LABEL MINS = 'Total Minutes in Queue' NO = 'Total Tasks Completed'
			AVGHR = 'Tasks per Hour' ;
	SUM NO MINS;
	BY GROUP;
	PAGEBY GROUP;
	TITLE "&RECIP Queue Stats Report";
	TITLE2 "Month of &EFFDATE";
	FOOTNOTE  "JOB = UTLWQ03     REPORT = ULWQ03.LWQ03R&REPNO1.";
RUN;
	%end;
%else %if &hasobs=0 %then
	%do;
    	data _null_;
            file print titles;
			put ' ';
            put @01 'No records found for this job.';
        run;
    %end;
%mend qmonthprint1;
%qmonthprint1(QTEMP3);


/*CREATE QUSER REPORTS*/

PROC SORT DATA = QTEMP; BY GROUP USER QUEUE DATE ; RUN;

PROC SQL DQUOTE=ANSI;
	CREATE TABLE QTEMP2 AS
	SELECT GROUP, "USER", QUEUE, SUM(NO) AS NO, SUM(MINS) AS MINS, RULE
	,SUM(NO)/SUM(MINS)*60 FORMAT=6.1 AS AVGHR
	FROM QTEMP
	WHERE DATE BETWEEN &BEGIN.D AND &END.d
	AND RULE NOT CONTAINS 'NR'
	GROUP BY GROUP, "USER", QUEUE, RULE
	ORDER BY "USER", QUEUE;
QUIT;

%IF &SPEC NE 'OP' %THEN 
%DO;
	DATA PRN1;
	SET QTEMP2;
	WHERE (GROUP IN (&GRP) AND RULE = ' ')
	OR (RULE CONTAINS &SPEC);
	RUN;

	DATA PRN2;
	SET QTEMP2;
	WHERE GROUP NOT IN (&GRP) AND RULE NOT CONTAINS &SPEC;
	RUN;

	PROC SQL DQUOTE=ANSI;
	CREATE TABLE PRN2X AS
	SELECT * 
	FROM PRN2
	WHERE "USER" IN
		(SELECT "USER" 
		FROM PRN1)
	;
	QUIT;

	DATA PRN3;
	SET PRN1 PRN2X;
	BY USER QUEUE;
	WHERE RULE NOT CONTAINS 'OP';
	RUN;
%END;
%ELSE %DO;
	DATA PRN3;
	SET QTEMP2;
	RUN;
%END;


OPTIONS PAGENO=1;

/*PROC PRINTTO PRINT=REPORT&REPNO2.;
RUN;*/
PROC PRINTTO PRINT="C:\WINDOWS\TEMP\QUSER Monthly - &RECIP..rtf" NEW;
RUN;

%macro qmonthprint2(dsname);
%let dsid=%sysfunc(open(&dsname));
%let hasobs=%sysfunc(attrn(&dsid,any));
%let rc=%sysfunc(close(&dsid));
%if &hasobs=1 %then
	%do;
PROC PRINT DATA = &dsname NOOBS LABEL;
	VAR QUEUE MINS NO AVGHR;
	LABEL MINS = 'Total Minutes in Queue' NO = 'Total Tasks Completed'
			AVGHR = 'Tasks per Hour' ;
	SUM NO MINS;
	BY USER;
	TITLE "&RECIP Queue Users Report";
	TITLE2 "Month of &EFFDATE";
	FOOTNOTE  "JOB = UTLWQ03     REPORT = ULWQ03.LWQ03R&REPNO2.";
RUN;
	%end;
%else %if &hasobs=0 %then
	%do;
    	data _null_;
            file print titles;
			put ' ';
            put @01 'No records found for this job.';
        run;
    %end;
%mend qmonthprint2;
%qmonthprint2(PRN3);

PROC PRINTTO;
RUN;
%MEND QMONTH;

%QMONTH(RECIP=Loan Management Services, GRP=%STR('DFT','SKP'), SPEC='PC', REPNO1=2, REPNO2=3)
/*MAILTO=("bhill@utahsbr.edu" "dbriggs@utahsbr.edu" "sdennis@utahsbr.edu"))*/
%QMONTH(RECIP=Loan Delivery, GRP=%STR('APP','LMN','SCR'), SPEC='LD', REPNO1=4, REPNO2=5)
/*, MAILTO="tvig@utahsbr.edu")*/
%QMONTH(RECIP=Support Services, GRP=%STR('BKP'), SPEC='SS', REPNO1=6, REPNO2=7)
/*, MAILTO="dcox@utahsbr.edu")*/
%QMONTH(RECIP=Default Prevention, GRP=%STR('PRE'), SPEC='DP', REPNO1=8, REPNO2=9)
/*, MAILTO="lolney@utahsbr.edu")*/
%QMONTH(RECIP=Database Quality, GRP=%STR(' '), SPEC='DQ', REPNO1=10, REPNO2=11)
/*, MAILTO="manderson@utahsbr.edu")*/
%QMONTH(RECIP=Operations, GRP=%STR('ALL','APP','BKP','DFT','LMN','PRE','SCR','SKP','RST'), 
SPEC='OP', REPNO1=12, REPNO2=13)
/*, MAILTO=("jginos@utahsbr.edu" "bcox@utahsbr.edu" "kpage@utahsbr.edu"))*/
%QMONTH(RECIP=Claims, GRP=%STR(' '), SPEC='CM', REPNO1=14, REPNO2=15)
/*, MAILTO="mhansen@utahsbr.edu")*/
%QMONTH(RECIP=Account Services, GRP=%STR(' '), SPEC='AS', REPNO1=16, REPNO2=17)
/*, MAILTO="rbales@utahsbr.edu")*/
%QMONTH(RECIP=Auxiliary Services, GRP=%STR(' '), SPEC='AU', REPNO1=18, REPNO2=19)
;
