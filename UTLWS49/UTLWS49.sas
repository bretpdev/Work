/*A file is being imported from nightly processing, and then*/
/*is converted to a user friendly format for managers.*/
/*%LET INLIB = T:\SAS;*/
%LET INLIB = X:\PADD\FTP;
%LET OUTLIB = X:\REPORTS\LSS\OUTPUT;
FILENAME BUCKETS "&INLIB\ULWS49.LWS49R2.*";
FILENAME ERRMESS EMAIL "sshelp@utahsbr.edu";

/*This macro will check for the file and either process the file*/
/*or send a message reporting that the file was not there.*/
/*If the file is not there, the previous day's report will be untouched.*/
%MACRO FILECHECK;
%IF %SYSFUNC(FEXIST(BUCKETS)) = 0 %THEN %DO;
DATA _NULL_;
FILE ERRMESS;
	put '!em_cc! "nowens@utahsbr.edu"';
	PUT "The UTLWS49 file was not in x:\padd\ftp.";
	PUT "Please put the file in x:\padd\ftp and then rerun:";
	PUT "'X:\Sessions\Local SAS Schedule\UTLWS49 Borrower Delinquency Report.L.sas'";
RUN;
%END;
%ELSE %DO;
DATA BUCKETS;
CALL SYMPUT('TODAYS',PUT(TODAY(),WEEKDATE31.));
INFILE BUCKETS DLM = ',' DSD MISSOVER FIRSTOBS=2;
INPUT PERIOD :$10. TOT_NUM_LN :8.0 TOT_AMT :12.;
RUN;

ODS HTML BODY="&OUTLIB\DELQ_RPT.HTML";
TITLE 'BORROWER DELINQUENCY SUMMARY REPORT';
TITLE2	&TODAYS;

PROC PRINT NOOBS SPLIT='/' DATA=BUCKETS WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT TOT_NUM_LN COMMA8. TOT_AMT DOLLAR18.2;
VAR PERIOD TOT_NUM_LN TOT_AMT;
LABEL PERIOD = 'DaysS Delinquent'
	TOT_NUM_LN = 'Total Number of Loans'
	TOT_AMT = 'Total Amount of Loans';
RUN;

ODS _ALL_ CLOSE;
options noxwait; 
x 'del "X:\PADD\FTP\ULWS49.LWS49R2.*"';
%END;
%MEND;
%FILECHECK;
