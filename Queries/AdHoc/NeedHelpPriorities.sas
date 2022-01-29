/*PROD*/
LIBNAME CSYS ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CSYS.dsn;" ;
LIBNAME NHU ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\NeedHelpUheaa.dsn;" ;
LIBNAME NHCS ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\NeedHelpCornerStone.dsn;" ;

DATA _NULL_;
	CALL SYMPUT('RUNDATE',LEFT(TRIM(PUT(TODAY(),MMDDYY10.))));
RUN;

DATA PERF_STNDRDS;
INFILE CARDS DSD DLM=',' MISSOVER;
INFORMAT PRIORITY 1. PRIORITY_CAT $15. PRIORITY_FREQ 1.;
INPUT PRIORITY PRIORITY_CAT $ PRIORITY_FREQ ;
CARDS ;
9,IMMEDIATE,2
8,HIGH,3
7,HIGH,3
6,NORMAL,10
5,NORMAL,10
4,NORMAL,10
3,LOW,20
2,LOW,20
1,LOW,20
;

%MACRO GET_NH_DATA(DSN,SYS);
	PROC SQL;
		CREATE TABLE HP_&SYS AS
			SELECT	
				A.Ticket
				,A.TicketCode
				,A.Priority
				,A.Subject
				,A.Issue
				,A.Status
				,CATX(' ',C.FirstName,C.LastName) AS Court
				,DATEPART(A.LastUpdated) AS LastUpdated
				,INTCK('WEEKDAY', DATEPART(A.LastUpdated), TODAY()) AS DAYS_SINCE_LAST_UPDATE
				,D.PRIORITY_FREQ
				,(CALCULATED DAYS_SINCE_LAST_UPDATE > D.PRIORITY_FREQ) AS IS_PAST_DUE
			FROM
				&DSN..DAT_Ticket A
				LEFT OUTER JOIN &DSN..DAT_TicketsAssociatedUserID B
					ON A.Ticket = B.Ticket
					AND B.Role = 'Court'
				LEFT OUTER JOIN CSYS.SYSA_DAT_Users C
					ON B.SqlUserId = C.SqlUserId
				INNER JOIN PERF_STNDRDS D
					ON A.Priority = D.Priority
			WHERE 
				A.TicketCode NOT IN ('FAC','FAR','FTRANS','IR','THR','FAC FED','FAR FED','FTRANS FED','IR FED','THR FED')
				AND A.Status NOT IN ('Resolved','Withdrawn','Verified','Complete And Verified','Complete','Completed','BS Approval')
			ORDER BY 
				CALCULATED DAYS_SINCE_LAST_UPDATE
		;
	QUIT;

	DATA HP_&SYS;
		SET HP_&SYS;
		IF IS_PAST_DUE THEN DAYS_BELOW_STANDARD = DAYS_SINCE_LAST_UPDATE - PRIORITY_FREQ;
		DAYS_BELOW_STANDARD = COALESCE(DAYS_BELOW_STANDARD,0);
	RUN;

	DATA ALL_LIST (KEEP=PRIORITY COURT IS_PAST_DUE);
		SET PERF_STNDRDS HP_&SYS;
	RUN;
	PROC FREQ DATA=ALL_LIST NOPRINT;
		WHERE IS_PAST_DUE IN (1,.);
		TABLE Court*Priority / SPARSE OUT=CP_LIST(KEEP=COURT PRIORITY);
	RUN;
	PROC SUMMARY DATA=HP_&SYS MEAN;
		WHERE IS_PAST_DUE AND COURT ^= '';
		VAR DAYS_BELOW_STANDARD;
		CLASS Court Priority PRIORITY_FREQ;
		TYPES Court*Priority*PRIORITY_FREQ;
		OUTPUT OUT=TEST2 MEAN=DAYS_BELOW_STANDARD;
	RUN;
	PROC SQL;
	CREATE TABLE HP_&SYS._summary as
		SELECT DISTINCT A.*
			,COALESCE(B._FREQ_,0) AS Count
			,ROUND(COALESCE(B.DAYS_BELOW_STANDARD,0),1) AS AVE_DAYS_BELOW_STANDARD
		FROM CP_LIST A
		LEFT OUTER JOIN TEST2 B
			ON A.Court = B.Court
			AND A.Priority = B.Priority
		INNER JOIN (
			SELECT COURT 
			FROM HP_&SYS
			WHERE IS_PAST_DUE
			) C
			ON A.Court = C.Court
		WHERE A.COURT ^= ''
		ORDER BY A.Court, A.Priority
		;
	QUIT;
	PROC TRANSPOSE DATA=HP_&SYS._summary OUT=HP_&SYS._summary2 (DROP=_NAME_ _LABEL_) PREFIX=Pri;
		VAR Count AVE_DAYS_BELOW_STANDARD;
		BY Court;
	RUN;

	DATA HP_&SYS._summary2;
	SET HP_&SYS._summary2;
	LENGTH lineDesc $50.;
	BY Court;
	IF FIRST.Court THEN do;
		lineDesc = 'Number of Tickets';
		ord = 1;
	end;
	IF LAST.Court THEN do;
		lineDesc = 'Ave # of Days Below Standard';
		ord = 2 ;
	end;
	RUN;

	proc sort data=HP_&SYS;
		by Court descending Priority DAYS_SINCE_LAST_UPDATE ;
	run;
%MEND GET_NH_DATA;
%GET_NH_DATA(NHU,UHEAA);
%GET_NH_DATA(NHCS,CORNERSTONE);

ODS PDF file='T:\SAS\NeedHelpPerformanceStandardsUHEAA.pdf' CONTENTS=NO STYLE=htmlblue NOTOC;
OPTIONS PS=39 LS=127;
OPTIONS ORIENTATION = LANDSCAPE nodate;
TITLE 'Need Help UHEAA - Tickets Not Meeting Performance Standards';
TITLE2 "&RunDate";
FOOTNOTE 'Need Help Region=UHEAA';
PROC REPORT DATA=HP_UHEAA HEADSKIP NOWD SPLIT='~';
WHERE IS_PAST_DUE = 1;
by Court;
COLUMN Ticket TicketCode Priority Issue Status Court LastUpdated DAYS_SINCE_LAST_UPDATE;
DEFINE Ticket / DISPLAY 'Ticket~Number' WIDTH=6;
DEFINE TicketCode/DISPLAY 'Ticket~Type' WIDTH=6;
DEFINE Priority/DISPLAY WIDTH=8;
DEFINE Issue/DISPLAY WIDTH=50 FLOW;
DEFINE Status/ DISPLAY ;
DEFINE Court/DISPLAY WIDTH=20 ;
DEFINE LastUpdated/DISPLAY 'Last Updated' FORMAT=MMDDYY10.;
DEFINE DAYS_SINCE_LAST_UPDATE/DISPLAY 'Days Since~Last Update';
RUN;
ODS PDF CLOSE;
ODS PDF file='T:\SAS\NeedHelpPerformanceStandardsCornerStone.pdf' CONTENTS=NO STYLE=htmlblue NOTOC;
TITLE 'Need Help CornerStone - Tickets Not Meeting Performance Standards';
TITLE2 "&RunDate";
FOOTNOTE 'Need Help Region=CornerStone';
PROC REPORT DATA=HP_CORNERSTONE HEADSKIP NOWD SPLIT='~';
WHERE IS_PAST_DUE = 1;
by Court;
COLUMN Ticket TicketCode Priority Issue Status Court LastUpdated DAYS_SINCE_LAST_UPDATE;
DEFINE Ticket / DISPLAY 'Ticket~Number' WIDTH=6;
DEFINE TicketCode/DISPLAY 'Ticket~Type' WIDTH=6;
DEFINE Priority/DISPLAY WIDTH=8;
DEFINE Issue/DISPLAY WIDTH=50 FLOW;
DEFINE Status/ DISPLAY ;
DEFINE Court/DISPLAY WIDTH=20 ;
DEFINE LastUpdated/DISPLAY 'Last Updated' FORMAT=MMDDYY10.;
DEFINE DAYS_SINCE_LAST_UPDATE/DISPLAY 'Days Since~Last Update';
RUN;
ODS PDF CLOSE;
/*Summary Information*/
ODS PDF file='T:\SAS\NeedHelpPerformanceStandardsSummaryUHEAA.pdf' CONTENTS=NO STYLE=sasweb NOTOC STARTPAGE=NO;
OPTIONS PS=39 LS=127;
OPTIONS ORIENTATION = LANDSCAPE nodate;
TITLE 'Need Help UHEAA - Summary For Tickets Not Meeting Performance Standards';
TITLE2 "&RunDate";
FOOTNOTE 'Need Help Summary Region=UHEAA';

PROC REPORT DATA=hp_uheaa_summary2 SPANROWS NOWD SPLIT='~'; 
	COLUMN ("Court"(Court ord lineDesc)) ("Priority"(Pri1-Pri9)) ;
	DEFINE Court / "~" GROUP  ORDER=Internal ;
	define ord / group order=internal noprint;
	DEFINE lineDesc / GROUP "~" ORDER=Internal ;
	DEFINE Pri1 / "1"  style(column)=[cellwidth=45pt ];
	DEFINE Pri2 / "2"  style(column)=[cellwidth=45pt ];
	DEFINE Pri3 / "3"  style(column)=[cellwidth=45pt ];
	DEFINE Pri4 / "4"  style(column)=[cellwidth=45pt ];
	DEFINE Pri5 / "5"  style(column)=[cellwidth=45pt ];
	DEFINE Pri6 / "6"  style(column)=[cellwidth=45pt ];
	DEFINE Pri7 / "7"  style(column)=[cellwidth=45pt ];
	DEFINE Pri8 / "8"  style(column)=[cellwidth=45pt ];
	DEFINE Pri9 / "9"  style(column)=[cellwidth=45pt ];
RUN;
ods pdf close;

ODS PDF file='T:\SAS\NeedHelpPerformanceStandardsSummaryCornerStone.pdf' CONTENTS=NO STYLE=sasweb NOTOC STARTPAGE=NO;
OPTIONS PS=39 LS=127;
OPTIONS ORIENTATION = LANDSCAPE nodate;
TITLE 'Need Help CornerStone - Summary For Tickets Not Meeting Performance Standards';
TITLE2 "&RunDate";
FOOTNOTE 'Need Help Summary Region=CornerStone';

PROC REPORT DATA=hp_CORNERSTONE_summary2 SPANROWS NOWD SPLIT='~'; 
	COLUMN ("Court"(Court ord lineDesc)) ("Priority"(Pri1-Pri9)) ;
	DEFINE Court / "~" GROUP  ORDER=Internal ;
	define ord / group order=internal noprint;
	DEFINE lineDesc / GROUP "~" ORDER=Internal ;
	DEFINE Pri1 / "1"  style(column)=[cellwidth=45pt ];
	DEFINE Pri2 / "2"  style(column)=[cellwidth=45pt ];
	DEFINE Pri3 / "3"  style(column)=[cellwidth=45pt ];
	DEFINE Pri4 / "4"  style(column)=[cellwidth=45pt ];
	DEFINE Pri5 / "5"  style(column)=[cellwidth=45pt ];
	DEFINE Pri6 / "6"  style(column)=[cellwidth=45pt ];
	DEFINE Pri7 / "7"  style(column)=[cellwidth=45pt ];
	DEFINE Pri8 / "8"  style(column)=[cellwidth=45pt ];
	DEFINE Pri9 / "9"  style(column)=[cellwidth=45pt ];
RUN;
ods pdf close;


options emailsys=smtp emailhost=mail.utahsbr.edu emailport=25;

 FILENAME OUTBOX EMAIL
FROM = ("SSHelp@utahsbr.edu")
TO = ("jryan@utahsbr.edu")
ATTACH=("T:\SAS\NeedHelpPerformanceStandardsCornerStone.pdf" "T:\SAS\NeedHelpPerformanceStandardsSummaryCornerStone.pdf" "T:\SAS\NeedHelpPerformanceStandardsSummaryUHEAA.pdf" "T:\SAS\NeedHelpPerformanceStandardsUHEAA.pdf")
 REPLYTO = ("SSHelp@utahsbr.edu")
 SUBJECT = ("Test")
 ;
 DATA _NULL_;
 FILE OUTBOX;
/* PUT "Hello,";*/
/* PUT ;*/
/* PUT %SYSFUNC(COMPBL(*/
/* "This is an example email."));*/
/* PUT ;*/
/* PUT %SYSFUNC(COMPBL(*/
/*"By using COMPBL we remove extra blanks from our text."));*/
/* PUT %SYSFUNC(COMPBL(*/
/*"There is no separation with this email line."));*/
 RUN;
 FILENAME OUTBOX CLEAR;


