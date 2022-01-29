/*%let rptlib = Q:\Support Services\CCB Report ;*/
%let rptlib = T:\SAS ;
%LET OUTFILE = "&RPTLIB\PROMOTIONS_&sysdate9..xls";

LIBNAME BSYS ODBC USER="SQLSERVER;" PASSWORD="PROCAUTO;" REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.DSN;" ;
data _null_;
from_date = intnx('week',today(),-1,'b')+3;
call symput('WEEK_AGO',from_date);
RUN;

%PUT &WEEK_AGO;

PROC SQL;
CREATE TABLE PROMOTED AS
SELECT A.SEQUENCE
	,A.CLASS
	,A.REQUEST
	,COALESCE(B.TITLE,C.TITLE) AS JOB
	,COALESCE(E.DESCRIPTION,F.DESCRIPTION) AS DESCRIPTION
	,DATEPART(A.BEGIN) AS BEGIN_DATE FORMAT=MMDDYY10.
	,DATEPART(A.END) AS END_DATE FORMAT=MMDDYY10.
	,A.STATUS
	,COALESCE(B.PRIORITY,C.PRIORITY) AS PRIORITY
	,COALESCE(B.SUMMARY,C.SUMMARY) AS SUMMARY
	,D.ORDER 
FROM BSYS.SCKR_REF_STATUS A
LEFT OUTER JOIN BSYS.SCKR_DAT_SASREQUESTS B
	ON A.REQUEST = B.REQUEST
	AND A.CLASS = B.CLASS
LEFT OUTER JOIN BSYS.SCKR_DAT_SAS E
	ON E.JOB = B.JOB
LEFT OUTER JOIN BSYS.SCKR_DAT_SCRIPTREQUESTS C
	ON A.REQUEST = C.REQUEST
	AND A.CLASS = C.CLASS
LEFT OUTER JOIN BSYS.SCKR_DAT_SCRIPTS F
	ON C.SCRIPT = F.SCRIPT

LEFT OUTER JOIN BSYS.SCKR_LST_STATUSES D
	ON A.STATUS = D.STATUS
where DATEPART(A.BEGIN) >= &WEEK_AGO
	or DATEPART(A.END) >= &WEEK_AGO
;
QUIT;

proc sort data=promoted; by class request descending sequence; run;
data promoted(KEEP=num job description priority summary report) ;
format num $4.;
set promoted;
LENGTH REPORT $ 24;
LABEL num ="Request";
LABEL JOB ="Job Name";
label description ="Job Description";
label priority ="Priority";
label summary ="Request Summary";
label report ="Request Status";
by class request;
num = put(request,4.0);
retain a 0;
if first.request then do; 
	if substr(status,1,4) = 'Hold' 
		and end_date = . 
		and last.request = 0 then a = 1;
	else do;
		a = 0;
		if status = 'Complete' and begin_date >= &WEEK_AGO then b=1;
		else if order in (17,18,19,20,21,22,23,24) and begin_date >= &WEEK_AGO then b=1;
	end;
end;
else if a = 1 then do;
	if substr(status,1,4) = 'Hold' then a = 1;
		else a = 0; 
	if order in (17,18,19,20,21,22,23,24) and begin_date >= &WEEK_AGO then b=1;
end;
if b=1 then do;
	if class = 'SAS' THEN DO;
		IF ORDER = 25 THEN REPORT = 'COMPLETE SAS';
		ELSE REPORT = 'PENDING PROMOTION SAS';
	END;
	ELSE IF ORDER = 25 THEN REPORT = 'COMPLETE SCRIPT';
	ELSE REPORT = 'PENDING PROMOTION SCRIPT';
	summary = tranwrd(summary,'0A'x,'20'x);	*ELIMINATES NEW LINES;
	summary = tranwrd(summary,'0D'x,'20'x);	*ELIMINATES HARD RETURNS;
	description = tranwrd(description,'0A'x,'20'x);	*ELIMINATES NEW LINES;
	description = tranwrd(description,'0D'x,'20'x);	*ELIMINATES HARD RETURNS;
	output;
end;
run;

PROC SORT DATA=PROMOTED; BY REPORT descending priority; RUN;

proc template;
	define style Styles.Test;
     parent=styles.default;
        class color_list /
/*grid lines for table*/
			'fgA1' = cx000000 
/*background color for table*/
           'bgA1' = cxffffff
/*text in the table*/
           'fgA' = cx000000
/*background color for footer*/
           'bgA' = cxffffff;
        class fonts /
          'headingFont' = ("Arial",10pt,Bold)
          'docFont' = ("Arial",10pt);
    end;
RUN;

title;
footnote;

ods html file=&OUTFILE RS=none style=test  ;
proc print data=promoted noobs label;
var num job description priority summary report;
run;
ods html close;
