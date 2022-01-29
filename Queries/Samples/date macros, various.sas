*This code lets you manually enter a date range without quotes, so
you can use the dates in pass-through SQL;

%LET BEGIN 	= 07/01/2001;
%LET END 	= 06/30/2002;
%SYSLPUT BEGIN = %STR(%'&BEGIN%');
%SYSLPUT END = %STR(%'&END%');	


*&BEGIN = '01-01-2002';
*&END   = '01-31-2002';

DATA _NULL_;
     CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'beginning'), MMDDYYD10.)||"'");
     CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'end'), MMDDYYD10.)||"'");
     CALL SYMPUT('EFFDATE',TRIM(LEFT(UPCASE(
		PUT(INTNX('MONTH',TODAY(),-1), MONNAME9.)||' '||
		PUT(INTNX('MONTH',TODAY(),-1), YEAR4.)))));
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;

*EOM PROCESSING;
/*SET DATE RANGE:  IF EOM PROCESSING RUNS BEFORE LAST DAY OF MONTH, (3 DAY WINDOW)
REPORT FOR CURRENT MONTH, ELSE REPORT FOR THE PREVIOUS MONTH*/
DATA _NULL_;
     CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY()+3,-1,'beginning'), MMDDYYD10.)||"'");
     CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY()+3,-1,'end'), MMDDYYD10.)||"'");
     CALL SYMPUT('EFFDATE',TRIM(LEFT(UPCASE(
		PUT(INTNX('MONTH',TODAY()+3,-1), MONNAME9.)||' '||
		PUT(INTNX('MONTH',TODAY()+3,-1), YEAR4.)))));
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;


*&EFFDATE = September 2001;

DATA _NULL_;
     EFFMO = PUT(INTNX('MONTH',TODAY(),-1), MONNAME9.);
	 EFFYR = PUT(INTNX('MONTH',TODAY(),-1), YEAR4.);
     CALL SYMPUT('EFFDATE',TRIM(LEFT(UPCASE(EFFMO||' '||EFFYR))));
RUN;

DATA _NULL_;
PUT &EFFDATE;
RUN;

*these came from pheaa;
DATA _NULL_;
/*Date variable that displays the first of the previous month.*/
CALL SYMPUT('BEG',"'"||PUT(INTNX('MONTH',DATE(),-1), MMDDYY10.)||"'");
/*Date variable that displays the last date of the previous month.*/
CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',DATE(),0)-1, MMDDYY10.)||"'");
RUNDT = INTNX('DAY',INTNX('MONTH',DATE(),0),-1);
/*Month and year for title.*/
CALL SYMPUT('RUNDT',TRIM(LEFT(UPCASE(PUT(RUNDT,MONNAME.))))||' '||PUT(RUNDT,YEAR.));

*'02-05-2003';
DATA _NULL_;
CALL SYMPUT('RUNDATE',"'"||PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYYD10.)||"'");
RUN;
*02/05/2003;
DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;

/*business days*/
DATA _NULL_;
	CALL SYMPUT('BUSDATE',"'"||PUT(INTNX('WEEKDAY',TODAY(),-5,'beginning'), MMDDYYD10.)||"'");
RUN;

%put &busdate;

%macro fdate(fmt);
   %global fdate;
   data _null_;
      call symput("fdate",left(put("&sysdate"d,&fmt)));
   run;
%mend fdate;
%fdate(MMDDYY8.);


/*FISCAL YEAR*/
DATA _NULL_;
CALL SYMPUT('BEGIN',"'"||PUT(intnx('year.7',today(),0,'beginning'), MMDDYYD10.)||"'");
CALL SYMPUT('END',"'"||PUT(intnx('year.7',today(),0,'end'), MMDDYYD10.)||"'");
CALL SYMPUT('FY',PUT(YEAR(intnx('year.7',today(),0,'END')),4.));
CALL SYMPUT('EFFDATE',TRIM(LEFT(UPCASE(
	PUT(INTNX('MONTH',TODAY()+3,-1), MONNAME9.)||' '||
	PUT(INTNX('MONTH',TODAY()+3,-1), YEAR4.)))));
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;

*calculate the end of the current quarter;
DATA _NULL_;
	CALL SYMPUT('PREV_QTR_END',INTNX('QTR',TODAY(),-1,'E')); *use CALL SYMPUT to create a macro variable so it can be used outside the data step;
RUN;

/*write the contents of the macro variable to the log*/
/*a separate DATA step is needed because the macro variable isn't resolved until the end of the previous DATA step*/
DATA _NULL_;
/*	PUT &PREV_QTR_END :DATE9.; it seems this should work but it doesn't*/
	SHOW_DATE = &PREV_QTR_END;
	PUT SHOW_DATE :DATE9.; *formats the output;
RUN;

/*also writes contents of the macro variable to the log but without formatting (in -line formatting not available for %PUT)*/
%PUT &PREV_QTR_END;
