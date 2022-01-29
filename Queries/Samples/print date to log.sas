%let today_ = put(today(), mmddyy10.);
%put &today_;

DATA _NULL_;
/*for previous month use -1, for current use 0, etc*/
	RUN_MON = -1;
	CALL SYMPUT('LN37DATE',PUT(INTNX('MONTH',TODAY(),RUN_MON,'E'),5.0));
	CALL SYMPUT('BEGIN',PUT(intnx('year.10',today(),0),5.0));
	CALL SYMPUT('FINISH',PUT(intnx('year.10',today(),1)-1,5.0));
	CALL SYMPUT('PREVMONTHEND',PUT(INTNX('MONTH',TODAY(),RUN_MON,'E'),5.0));

/*test date values*/
	date1 = PUT(&BEGIN, mmddyy10.);
	date2 = PUT(&FINISH, mmddyy10.);
	%put &BEGIN &FINISH; *requires "%" and "&" because variables are macro variables created using SYMPUT;
	put date1 date2; *normal variable;
RUN;

data _null_;
call symput('today_m',put(today(), date.));
%put &today_m;

today = put(today(), mmddyy10.);
put today;

run;
