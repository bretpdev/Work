LIBNAME BSYS ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn;" ;
/*proc sql;*/
/*CREATE TABLE SAS_PROGRAMMER AS*/
/*SELECT A.SEQUENCE*/
/*	,A.COURT FORMAT= $20.*/
/*	,A.CLASS*/
/*	,A.REQUEST*/
/*	,A.STATUS FORMAT= $25.	*/
/*	,DATEPART(A.BEGIN) AS BEGIN	FORMAT=MMDDYY10.*/
/*	,DATEPART(A.END) AS FINISH FORMAT=MMDDYY10.*/
/*	,COALESCE(C.HISTORY,B.HISTORY) AS HISTORY*/
/*FROM BSYS.SCKR_REF_STATUS A*/
/*LEFT OUTER JOIN BSYS.SCKR_DAT_SASREQUESTS B*/
/*	ON A.REQUEST = B.REQUEST*/
/*	AND A.CLASS = 'SAS'*/
/*LEFT OUTER JOIN BSYS.SCKR_DAT_SCRIPTREQUESTS C*/
/*	ON A.REQUEST = C.REQUEST*/
/*	AND A.CLASS = 'Scr'*/
/*WHERE COURT ^= ''*/
/*WHERE A.COURT IN ('Andy Adams','Bret Pehrson','Daren Beattie','Jay Davis','Nathan Owens')*/
/*	AND DATEPART(A.END) >= '1FEB2011'D */
/*and '31jan2011'd*/
/*where datepart(b.requested) >= '01jun2010'd*/
/*	AND A.COURT = 'Nathan Owens'*/
/*	and a.request = 3143*/
/*GROUP BY STATUS*/
/*;*/
/*QUIT;*/
proc sql;
CREATE TABLE SAS_PROGRAMMER AS
SELECT A.SEQUENCE
	,A.CLASS
	,A.REQUEST
	,a.begin format=datetime.
	,a.end format=datetime.
FROM BSYS.SCKR_REF_STATUS A
WHERE a.status in ('Testing')
	AND DATEPART(A.begin) >= '7feb2011'D 
	and datepart(a.end) >='01jan2011'd
	and a.end;
QUIT;
PROC SORT DATA=SAS_PROGRAMMER; BY class REQUEST SEQUENCE; RUN;

data sas_programmer(drop=begin end y z);
set sas_programmer;
if datepart(begin) = datepart(end) 
	then min_elapsed = INTCK('MINUTE',timepart(BEGin),timepart(end));
else do;
			Z = MAX(INTCK('MINUTE',timepart(BEGin),INPUT('17:00',TIME5.)),0);
			Y = MAX(INTCK('MINUTE',INPUT('08:00',TIME5.),timepart(end)),0);
			MIN_ELAPSED = (8 * 60 * (INTCK('WEEKDAY',datepart(BEGIN),datepart(end))-1))  + Z + Y ;
end;
run;

proc sql;
create table goin as
select class
	,count(*) as count
	,avg(total) as Avg_Minutes

from (select sum(min_elapsed) as total
	,class
	,request
	from sas_programmer
	group by class,request) a
group by class
;
quit;


/**/
/*PROC SQL;*/
/*CREATE TABLE SUMMARY_SCKR AS*/
/*SELECT COURT*/
/*	,STATUS*/
/*	,COUNT(*) AS OCCURRED*/
/*	,AVG(MIN_ELAPSED) AS AVG_MIN_ELAPSED FORMAT=10.2*/
/*FROM SAS_PROGRAMMER*/
/*GROUP BY COURT, STATUS;*/
/*QUIT;*/
/*FOR LANDSCAPE REPORTS:*/
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1;
title "Time taken in Testing";
title2 "For Requests that Concluded testing after 1/1/2011";
footnote1 "A Request is only counted once, even if it was tested repeatedly.";
PROC PRINTTO PRINT='T:\SAS\SACKER SUMMARY.TXT' NEW;
RUN;
proc print data=goin; 
run;
proc printto;
run;
/*PROC SQL;*/
/*TITLE 'SUMMARY BY STATUS';*/
/*SELECT STATUS*/
/*	,COUNT(*) AS OCCURRED*/
/*	,AVG(MIN_ELAPSED) AS AVG_MIN_ELAPSED FORMAT=10.2*/
/*FROM SAS_PROGRAMMER*/
/*GROUP BY STATUS;*/
/**/
/*TITLE 'SUMMARY OF WORK HISTORY IN SACKER BY COURT, STATUS';*/
/*FOOTNOTE 'THIS REPORT ONLY INCLUDES PROGRAMMERS';*/
/*FOOTNOTE2 'CAN BE EXPANDED TO INCLUDE OTHER BUSINESS UNITS';*/
/*FOOTNOTE3 '----------------------------------------------------------';*/
/*SELECT COURT*/
/*	,STATUS*/
/*	,COUNT(*) AS OCCURRED*/
/*	,AVG(MIN_ELAPSED) AS AVG_MIN_ELAPSED FORMAT=10.2*/
/*FROM SAS_PROGRAMMER*/
/*GROUP BY COURT, STATUS;*/
/**/
/*QUIT;*/
/*PROC PRINT DATA=SAS_PROGRAMMER; */
/*where beg and fin;*/
/*RUN;*/

/*PROC PRINTTO; */
/*RUN;*/
/*PROC EXPORT DATA= WORK.SAS_PROGRAMMER */
/*            OUTFILE= "T:\SAS\SACKER.CSV" */
/*            DBMS=CSV REPLACE;*/
/*     PUTNAMES=YES;*/
/*RUN;*/
