
proc gchart data=disb;
  vbar bf_ssn / sumvar=la_dsb type=mean;
  title 'Average Daily Receipts';
run;
quit;