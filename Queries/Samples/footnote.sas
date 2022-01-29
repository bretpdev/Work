/* Fiscal Year Begin and End Dates

Calculates the begin date and the end date of the fiscal year
previous to the current date

*/


data test;
BDATE1=intnx('year.7',today(),-1);
EDATE1=intnx('year.7',today(),0) - 1;
run;
proc print data = test;
var bdate1 edate1;
format bdate1 mmddyy10. edate1 mmddyy10.;
footnote 'this is the footnote';
run;