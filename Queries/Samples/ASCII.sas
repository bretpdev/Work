*PRINTS THE ASCII COLLATE ORDER USED BY SAS;

data _NULL_;
DO N=1 TO 200 BY 1;
blah = byte(N);
PUT N BLAH;
END;
run;

DATA BLAH;
blah = byte(131);
put 'Windows code for dash: 131';
put 'Windows symbol for dash: ' blah;
blah = byte(196);
put 'AIX code for dash: 196';
put 'AIX symbol for dash: ' blah;
RUN;