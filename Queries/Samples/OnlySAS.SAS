LIBNAME BSYS ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn;" ;
LIBNAME OPT ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\OPT.dsn;" ;

PROC SQL;
CREATE TABLE SCKR_SAS AS
SELECT COURT 
	,DATEPART(STATUSDATE) AS UPDATED 
	,DATEPART(REQUESTED) AS REQUESTED
	,INPUT(PRIORITY,1.0) AS PRIORITY
	,TITLE AS SUBJECT
	,REQUEST AS TICKET
	,CurrentStatus as STATUS
	,REQUESTER
	,B.UNIT
FROM BSYS.SCKR_DAT_SASREQUESTS a
LEFT OUTER JOIN BSYS.SCKR_REF_UNITSAS b
	ON A.JOB = B.PROGRAM 
WHERE COURT 
/*	and requested*/
	and input(priority,2.) >6;
QUIT;

DATA SCKR_SAS(DROP=FACTOR DUEDATE);
SET SCKR_SAS ;
FORMAT DUEDATE UPDATED MMDDYY10.;
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
	PASTDUE = DATDIF(DUEDATE,TODAY(),'ACT/ACT');
	sortorder= pastdue / factor;
RUN;

PROC SORT DATA=SCKR_SAS;
BY DESCENDING sortorder DESCENDING priority UPDATED REQUESTED;
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
      bodysize = 50%
      contentscrollbar = auto
      contentsize = 50%
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
	body = 'BODY_OnlySAS.HTML'
	CONTENTS = 'OnlySASTOC.HTML'
	FRAME = 'OnlySAS.HTML'
	STYLE=styles.test
;
TITLE "LIST OF JOBS IN COURT - &LUPD";

PROC PRINT DATA=SCKR_SAS n='Number of requests in court: ' NOOBS LABEL width=UNIFORM ;
FORMAT  UPDATED REQUESTED MMDDYY10.;
/*id COURT;*/
VAR COURT REQUESTED PRIORITY SUBJECT TICKET STATUS PASTDUE REQUESTER UNIT;
LABEL OB = 'OBS'
	COURT = '00'X
	UNIT = 'CUSTOMER'
;
ods proclabel = 'Application Development';
RUN;
ODS HTML CLOSE;

ods html PATH = 'X:\REPORTS\LSS\OUTPUT'
	body = 'OnlySASTOC.HTML'
	STYLE=styles.test;

/*FOR PORTRAIT REPORTS;*/
TITLE 'SUMMARY BY PRIORITY';
FOOTNOTE ;
proc report data=SCKR_SAS nowd;
column COURT priority;
define COURT / group CENTER '00'X;
define priority / across CENTER;
quit;

PROC FORMAT LIB=work;
	VALUE color
		9 = 'red'
		7-8 = 'orange'
		4-6 = 'yellow'
		1-3 = 'green'
		other = 'blue';
RUN;
ODS HTML CLOSE;
/*X '"X:\REPORTS\LSS\OUTPUT\OnlySAS.html"';*/
