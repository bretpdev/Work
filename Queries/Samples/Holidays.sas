/*To calculate the previous business day use minus('-'), for future use ('+')*/
%let fut_past = +;

%let bus_day_ct = 1;

data _null_;
runday = today();
/*runday = '31jan2011'd;*/

bus_day = 0;

array all_hol{12} ;
do until (bus_day = &bus_day_ct);
	do until(weekday(runday) in (2,3,4,5,6));
		runday = runday &fut_past 1;
	end;

	all_hol(1) = holiday('christmas',year(runday)) ;
	all_hol(2) = holiday('thanksgiving',year(runday)) + 1 ;	*Black Friday;
	all_hol(3) = holiday('thanksgiving',year(runday)) ;
	all_hol(4) = holiday('labor',year(runday)) ;
	all_hol(5) = holiday('usindependence',year(runday)) + 20 ; *Pioneer Day;
	all_hol(6) = holiday('usindependence',year(runday)) ;
	all_hol(7) = holiday('memorial',year(runday)) ;
	all_hol(8) = holiday('mlk',year(runday)) ;
	all_hol(9) = holiday('uspresidents',year(runday)) ;
	all_hol(10) = holiday('newyear',year(runday)) ;
	all_hol(11) = holiday('columbus',year(runday)) ; *not UHEAA;
	all_hol(12) = holiday('veterans',year(runday)) ; *not UHEAA;

	do a = 1,2;
		do i = 1 to 12 ;
			if weekday(runday) = 2 and runday = all_hol(i) + 1 then runday = runday &fut_past 1;
			if runday = all_hol(i) then runday = runday &fut_past 1;
			if weekday(runday) = 6 and runday = all_hol(i) - 1 then runday = runday &fut_past 1;
		end;
	end;
	
	if weekday(runday) in (2,3,4,5,6) then bus_day + 1;
end;
call symput('bus_day',put(runday,mmddyy10.));
run;

%put &bus_day;

	
