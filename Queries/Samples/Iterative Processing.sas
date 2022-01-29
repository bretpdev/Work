data INHOUSE;
n=0;
do obsnum=1 to LAST by 1; 
	set INHOUSE nobs=last point=obsnum;
	IF NEW_SSN_IND = 1 THEN n + 1;
	output;
end; 
stop;
RUN;