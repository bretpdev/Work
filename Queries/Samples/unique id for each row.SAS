*Adding a unique id to each row of a dataset;

data qqx;
n=0;
do obsnum=1 to last by 1; 
set q6 nobs=last point=obsnum;
n + 1;
output;
end; 
stop; 
run;