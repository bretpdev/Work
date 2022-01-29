LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
proc contents data=DLGSUTWH.RS05_IBR_RPS out=testquery noprint;
run;

ENDRSUBMIT;
DATA TESTQUERY;
	SET WORKLOCL.TESTQUERY;
RUN;
proc sort data=testquery; by varnum; run;
proc printto print='t:\sas\rs05.txt' new;
run;
title 'CONTENTS OF RS05_IBR_RPS TABLE';
proc print data=testquery;
run;
proc printto;
run;
