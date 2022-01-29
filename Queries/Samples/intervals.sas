data shift;
today = '01jun2001'd;
date = intnx('year.7',today,-1);
put date mmddyy10.;
run;
proc print data=shift;
var date;
format date mmddyy10.;
run;
