%let rptlib = t:\sas;
filename report2 "&rptlib\STUDENTS WHO MET PRIORITY DEADLINE.TXT";
LIBNAME REGENTS ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\REGENTS.DSN;" ;

proc sql noprint;
select "'" || max(YEAR) || "'" into: app_year
from REGENTS.PROGRAMOPERATIONYEARS;

select "'" || put(max(datepart(finaldeadline)),date9.) || "'d" into: max_dead_line
from REGENTS.PROGRAMOPERATIONYEARS;

select "'" || put(max(datepart(PriorityDeadline)),date9.) || "'d" into: dead_line
from REGENTS.PROGRAMOPERATIONYEARS;

select case
		when max(finaldeadline) < today() then put(max(datepart(PriorityDeadline)),mmddyy10.)
		else put(today(),mmddyy10.)
	end into: rpt_dt
from REGENTS.PROGRAMOPERATIONYEARS;
quit;


PROC SQL;
CREATE TABLE REGENTS_DATA AS
SELECT O.STATESTUDENTID
	,PROPCASE(TRIM(A.FIRSTNAME) || ' ' || TRIM(A.MIDDLENAME) || ' ' ||  A.LASTNAME) AS NAME
	,DATEPART(MAX(B.STATUSDATE,C.STATUSDATE,E.STATUSDATE)) AS LAST_DT FORMAT=MMDDYY10.
	,D.NAME AS HIGHSCHOOL
	,case
		when calculated last_dt <= &dead_line then 1
		else 0
	end as bypriority_dt

FROM REGENTS.SCHOLARSHIPAPPLICATION O
INNER JOIN REGENTS.STUDENT A
	ON A.STATESTUDENTID = O.STATESTUDENTID
LEFT OUTER JOIN REGENTS.SCHOOLLOOKUP D
	ON O.HIGHSCHOOLCEEBCODE = D.CEEBCODE
INNER JOIN REGENTS.DOCUMENT B
	ON O.STATESTUDENTID = B.STATESTUDENTID
INNER JOIN REGENTS.DOCUMENT C
	ON O.STATESTUDENTID = C.STATESTUDENTID
INNER JOIN REGENTS.DOCUMENT E
	ON O.STATESTUDENTID = E.STATESTUDENTID
WHERE B.TYPECODE = 1
	AND C.TYPECODE = 2
	AND E.TYPECODE = 15
	AND B.STATUSDATE <= &max_dead_line
	AND C.STATUSDATE <= &max_dead_line
	AND E.STATUSDATE <= &max_dead_line
	AND O.APPLICATIONYEAR = &APP_YEAR;
QUIT;

PROC SORT DATA=REGENTS_DATA nodupkey; BY LAST_DT STATESTUDENTID; RUN; 

proc sql noprint;
select count(*) into: by_prior
from REGENTS_DATA
where bypriority_dt = 1;

select count(*) into: by_dead
from REGENTS_DATA;

quit;
%let by_prior = &by_prior;
%let by_dead = &by_dead;

PROC EXPORT DATA= WORK.REGENTS_DATA 
            OUTFILE= "t:\sas\STUDENTS WHO MET PRIORITY DEADLINE.xls" 
            DBMS=EXCEL REPLACE;
     RANGE="PRIORITY DEADLINE"; 
     NEWFILE=YES;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW; RUN;
title 'STUDENTS WHO MET PRIORITY DEADLINE';
footnote1 "Total - Priority Deadline Met: &by_prior";
footnote2 "Total Meeting Requirements as of &rpt_dt: &by_dead";
footnote;
/*FOR LANDSCAPE REPORTS:*/
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 nonumber nodate;
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS PS=52 LS=96;

PROC PRINT DATA=REGENTS_DATA 
	n="Total Meeting Requirements as of &rpt_dt: &by_dead    Total - Priority Deadline Met: " 
NOOBS LABEL width=UNIFORM ;
where LAST_DT <= &dead_line;
FORMAT  LAST_DT MMDDYY10.;
FORMAT  HIGHSCHOOL $35.;
FORMAT  NAME $30.;
VAR  STATESTUDENTID NAME LAST_DT HIGHSCHOOL ;
LABEL STATESTUDENTID = 'SSID'
	LAST_DT = 'DATE LAST REQUIRED DOC RECEIVED'
	HIGHSCHOOL = 'HIGH SCHOOL'
;
RUN;

PROC PRINTTO; RUN;
