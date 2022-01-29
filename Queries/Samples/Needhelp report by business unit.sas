LIBNAME OPT ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\OPT.dsn;" ;
filename report2 "X:\REPORTS\LSS\OUTPUT\NDHP_MONTH_REPORT.txt";
ods listing;
data _null_;
call symput('beg_month',intnx('month',today(),0,'b'));
call symput('beg_month_tit',left(put(intnx('month',today(),0,'b'),worddate21.)));
run;

PROC SQL;
CREATE TABLE NDHP_DETAIL AS
SELECT B.UNIT FORMAT=$22.
	,case when status in ('Resolved','Verified','Withdrawn','Complete','Complete And Verified') then '_X_'
		ELSE b.ticketcode
	END AS ticketcode length=4
	,b.ticket
	,B.SUBJECT
	,b.status
	,DATEPART(B.LASTUPDATED) AS UPDATED	FORMAT=MMDDYY10.
FROM OPT.NDHP_DAT_TICKETS B
WHERE DATEPART(B.LASTUPDATED) >= &beg_month
	or b.status not in ('Resolved','Verified','Withdrawn','Complete','Complete And Verified')
ORDER BY 1,2;

CREATE TABLE TICKETCODE AS
SELECT FLOWID format=$8.
	,DESCRIPTION format=$50.
FROM OPT.FLOW_LST_FLOWS
;
quit;

OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS NODATE PS=39 LS=154 pageno=1;
TITLE "Needhelp Requests";
TITLE2 "Currently Open or";
title3 "Closed since" &beg_month_tit;
FOOTNOTE "JOB = Needhelp Requests By Business Unit";

proc printto print=report2 new;
run;
proc report data=ndhp_detail nowd ;
column unit ticketcode ;
define unit / group 'Business Unit';
define ticketcode / across CENTER 'Types';
quit;

title "Code Translation";
proc print data=ticketcode;
run;

TITLE "Detail Listing for Testing";

proc sort data=ndhp_detail; by unit ticketcode ticket; run;
PROC PRINT DATA=NDHP_DETAIL n='Number of tickets counted: ' NOOBS LABEL width=UNIFORM ;
by unit;
FORMAT STATUS $15. TICKET 6.;
VAR TICKETCODE TICKET SUBJECT STATUS UPDATED;
RUN;
proc printto; 
run;
