LIBNAME BSYS ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn;" ;
LIBNAME OPT ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\OPT.dsn;" ;

PROC SQL noprint;
select distinct "'" || programmer || "'" into: AD separated by ','
from BSYS.SCKR_REF_Programmer A
left outer join bsys.sckr_dat_sasrequests b
	on a.request = b.request
	and a.class = b.class
	and b.CurrentStatus not in ('Complete','Withdrawn')
left outer join bsys.sckr_dat_scriptrequests c
	on a.request = c.request
	and a.class = c.class
	and c.CurrentStatus not in ('Complete','Withdrawn')
where a.end = .
	and (b.request is not null or c.request is not null)
;
quit;
%put &AD;

proc sql;
CREATE TABLE LTDB AS
SELECT COURT 
	,DATEPART(STATUSDATE) AS UPDATED FORMAT=MMDDYY10.
	,DATEPART(REQUESTED) AS REQUESTED FORMAT=MMDDYY10.
	,INPUT(PRIORITY,1.0) AS PRIORITY
	,TITLE AS SUBJECT 
	,REQUEST AS TICKET
	,CurrentStatus as STATUS
	,'LETTER' AS SYSTEM
	,REQUESTER
	,B.UNIT
FROM BSYS.LTDB_DAT_REQUESTS	A
LEFT OUTER JOIN BSYS.LTDB_REF_UNIT B
	ON A.DOCNAME = B.DOCNAME
WHERE PROPCASE(COURT) IN (&AD)
	AND REQUESTED;

CREATE TABLE SCKR_SAS AS
SELECT COURT 
	,DATEPART(STATUSDATE) AS UPDATED FORMAT=MMDDYY10.
	,DATEPART(REQUESTED) AS REQUESTED FORMAT=MMDDYY10.
	,INPUT(PRIORITY,1.0) AS PRIORITY
	,TITLE AS SUBJECT
	,A.REQUEST AS TICKET
	,CurrentStatus as STATUS
	,'SAS' AS SYSTEM
	,REQUESTER
/*	,B.UNIT*/
FROM BSYS.SCKR_DAT_SASREQUESTS a
LEFT OUTER JOIN BSYS.SCKR_REF_Programmer C
	ON A.REQUEST = C.REQUEST
	AND A.CLASS = C.CLASS
/*	AND C.END = .*/
LEFT OUTER JOIN BSYS.SCKR_REF_UNITSAS b
	ON A.JOB = B.PROGRAM
WHERE A.COURT = C.PROGRAMMER
	OR CurrentStatus in ('Programmer Assignment','Programmer Queue');

CREATE TABLE SCKR_SCR AS
SELECT COURT 
	,DATEPART(STATUSDATE) AS UPDATED FORMAT=MMDDYY10.
	,DATEPART(REQUESTED) AS REQUESTED FORMAT=MMDDYY10.
	,INPUT(PRIORITY,1.0) AS PRIORITY
	,TITLE AS SUBJECT
	,A.REQUEST AS TICKET
	,CurrentStatus as STATUS
	,'SCRIPT' AS SYSTEM		
	,REQUESTER
/*	,B.UNIT*/
FROM BSYS.SCKR_DAT_SCRIPTREQUESTS A
LEFT OUTER JOIN BSYS.SCKR_REF_Programmer C
	ON A.REQUEST = C.REQUEST
	AND A.CLASS = C.CLASS
LEFT OUTER JOIN BSYS.SCKR_REF_UNIT b
	ON A.SCRIPT = B.PROGRAM
WHERE A.COURT = C.PROGRAMMER
	OR CurrentStatus in ('Programmer Assignment','Programmer Queue');

/*WHERE PROPCASE(COURT) IN (&AD)*/
/*	AND REQUESTED;*/
QUIT;


PROC SQL;
CONNECT TO ODBC AS OPT (REQUIRED="FILEDSN=X:\PADR\ODBC\OPT.DSN;");
CREATE TABLE NDHP AS
SELECT *
FROM CONNECTION TO OPT (
SELECT DISTINCT C.LEGALNAME AS COURT
	,B.LASTUPDATED AS UPDATED
	,B.REQUESTED
	,B.PRIORITY AS PRIOR
	,B.SUBJECT AS SUBJECT 
	,B.TICKET AS TICKET
	,B.STATUS AS STATUS
	,'NEEDHELP' AS SYSTEM
	,E.LEGALNAME AS REQUESTER
/*	,B.UNIT	*/
FROM NDHP_DAT_TICKETS B
LEFT OUTER JOIN NDHP_REF_STATUSES A
	ON B.TICKET = A.TICKET
	AND A.ENDDATE = 0
LEFT OUTER JOIN SYSA_DAT_ACCESSANDNOTIFICATIONUSERS C
	ON A.COURT = C.ACTIVEDIRECTORYSID
LEFT OUTER JOIN NDHP_DAT_TICKETSASSOCIATEDUSERIDS D
	ON B.TICKET = D.TICKET
	AND D.ROLE = 'Requester'
LEFT OUTER JOIN SYSA_DAT_ACCESSANDNOTIFICATIONUSERS e
	ON d.userid = e.ACTIVEDIRECTORYSID
WHERE C.LEGALNAME IN (&AD)
	and b.status not in ('Resolved'));
DISCONNECT FROM OPT;

quit;

DATA NDHP(DROP=PRIOR);
SET NDHP;
FORMAT UPDATED REQUESTED MMDDYY10.;
	UPDATED = DATEPART(UPDATED);
	REQUESTED = DATEPART(REQUESTED);
	PRIORITY = INPUT(PRIOR,15.0);
RUN;


DATA NDSKLT(DROP=FACTOR DUEDATE);
length COURT $40.;
SET NDHP LTDB SCKR_SAS SCKR_SCR ;
FORMAT DUEDATE UPDATED MMDDYY10.;
	UPDATED = COALESCE(UPDATED,REQUESTED);
	IF COURT= '' THEN COURT = '- UNASSIGNED -';

	IF PRIORITY = 9 THEN DO;
		DUEDATE = UPDATED;
		FACTOR = 1;
		END;
	ELSE IF PRIORITY >= 7 THEN DO;
		DUEDATE = intnx('weekday',UPDATED,2,'b');
		FACTOR = 2;
		END;
	ELSE IF PRIORITY >= 4 THEN DO;
		DUEDATE = UPDATED + 7;
		FACTOR = 7;
		END;
	ELSE IF PRIORITY >= 1 THEN DO;
		DUEDATE = UPDATED + 14;
		FACTOR = 14;
		END;
	ELSE DO;
		DUEDATE = UPDATED + 28;
		FACTOR = 28;
		END;
	COURT = PROPCASE(COURT);
	PASTDUE = DATDIF(DUEDATE,TODAY(),'ACT/ACT');
RUN;

PROC SORT DATA=NDSKLT noduprec;
BY COURT DESCENDING priority DESCENDING UPDATED DESCENDING REQUESTED;
RUN;

DATA NDSKLT;
SET NDSKLT;
BY COURT;
RETAIN OB;
IF FIRST.COURT THEN OB = 1;
ELSE OB = OB + 1;
RUN;

DATA _NULL_;
CALL SYMPUT('LUPD',PUT(TODAY(),MMDDYY10.) || ' ' || PUT(TIME(),TIME10.));
RUN;

proc template;
  define style styles.test;
  parent=styles.Sansprinter;
   style ContentItem from IndexItem
      "Controls the leafnode item in the Contents file." /
      marginright = 6 pt
      marginleft = 6 pt
      textalign = left;
   style ContentFolder from IndexItem
      "Controls the generic folder definition in the Contents file." /
      marginright = 6 pt
      marginleft = 6 pt
      textalign = left
      listentryanchor = off
      color = colors('confolderfg');
   style ContentProcName from IndexProcName
      "Controls the proc name in the Contents file." /
      marginright = _undef_
      marginleft = _undef_
      textalign = left;
   style frame from Frame /
      contentposition = left
      bodyscrollbar = auto
      bodysize = 70%
      contentscrollbar = auto
      contentsize = 30%
      framespacing = 1
      frameborderwidth = 1
      frameborder = on;


/*    style body from body /*/
/*          prehtml='<SCRIPT LANGUAGE=JAVASCRIPT*/
/*          TYPE="text/javascript" SRC="password.js" ></script>';*/
  end;
run;

OPTIONS NODATE;
ODS LISTING CLOSE;
ods html PATH = 'X:\REPORTS\LSS\OUTPUT'
/*ods html PATH = 't:\'*/
	body = 'BODY_CourtList.HTML'
	CONTENTS = 'CourtListTOC.HTML'
	FRAME = 'CourtList.HTML'
	STYLE=styles.test
;
TITLE "LIST OF JOBS IN COURT - &LUPD";

PROC PRINT DATA=NDSKLT n='Number of requests in court: ' NOOBS LABEL width=UNIFORM contents='00'X;
BY COURT;
pageBY COURT;
FORMAT  UPDATED REQUESTED MMDDYY10.;
id COURT;
VAR  REQUESTED PRIORITY SUBJECT SYSTEM TICKET STATUS PASTDUE REQUESTER;
/*UNIT;*/
LABEL OB = 'OBS'
	COURT = '00'X
/*	UNIT = 'CUSTOMER'*/
;
ods proclabel = 'Application Development';
RUN;
ODS HTML CLOSE;




ods html PATH = 'X:\REPORTS\LSS\OUTPUT'
	body = 'CourtListTOC.HTML'
	STYLE=styles.test
;

/*FOR PORTRAIT REPORTS;*/
TITLE 'SUMMARY BY PRIORITY';
FOOTNOTE ;
proc report data=NDSKLT nowd;
column COURT priority;
/*SYSTEM;*/
define COURT / group CENTER '00'X;
define priority / across CENTER;
/*define SYSTEM / across CENTER 'TRACKING SYSTEM';*/
quit;
TITLE 'SUMMARY BY SYSTEM';
proc report data=NDSKLT nowd ;
column COURT SYSTEM ;
define COURT / group CENTER 'Programmer' ;
/*define priority / across CENTER;*/
define SYSTEM / across CENTER 'Tracking System' ;
quit;

PROC FORMAT LIB=work;
	VALUE color
		9 = 'red'
		7-8 = 'orange'
		4-6 = 'yellow'
		1-3 = 'green'
		other = 'blue';
RUN;
/*legend1 cborder=black*/
/*        label=('System:')*/
/*        across=15;*/

/*title '';*/
/*ods listing;*/
/*goptions hpos=0 vpos=0 ;*/
/*goptions hpos=0 vpos=0 htext=0;*/
/**/
/*proc gchart data=NDSKLT  ;*/
/*title ;*/
/*FORMAT PRIORITY COLCODE.;*/
/*label court='00'x priority='00'x;*/
/*vbar system / group=court */
/*	midpoints= 'LETTER' 'NEEDHELP' 'SCRIPT' 'SAS'*/
/*	subgroup=priority*/
/*	             legend=legend1*/
/*                coutline=black*/
/*                caxis=black	*/
/*				width=2*/
/*;*/
/**/
/*run;*/
/*quit;*/

/*proc sort data=court_totals out=test(keep=COURT businessunit) nodupkey; */
/*by COURT businessunit; run;*/
/**/
/*TITLE 'SUMMARY OF JOBS IN COURT';*/
/*PROC PRINT DATA=court_count LABEL NOOBS;*/
/*BY businessunit ;*/
/*VAR COURT;*/
/*SUM ltdb scr sas ndhp TOTAL;*/
/*LABEL ltdb = 'LET'*/
/*	sas = 'SAS'*/
/*	scr = 'SCR'*/
/*	ndhp = 'HELP'*/
/*	COURT = 'COURT'*/
/*	BUSINESSUNIT = 'UNIT'*/
/*;*/
/*RUN;*/
ODS HTML CLOSE;

/*X '"X:\REPORTS\LSS\OUTPUT\CourtList.html"';*/
