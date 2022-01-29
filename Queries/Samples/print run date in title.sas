
data _null_;
      call symput("RUNDT",left(put("&sysdate"d,MMDDYY10.)));
run;

*************;

%macro fdate(fmt);
   %global fdate;
   data _null_;
      call symput("fdate",left(put("&sysdate"d,&fmt)));
   run;
%mend fdate;
%fdate(worddate.);

TITLE "Run Date:  &FDATE";