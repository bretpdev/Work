data r2;
	input bf_ssn $ 1-9;
	datalines;
1234567890
;
run;

/*raw data*/
/*proc sql;*/
/*	select bf_ssn into :avar from r2;*/
/*quit;*/

/*char data with quotes ready for use (variable needs to include quotes if used like WHERE LN10.BF_SSSN = &avar)*/
proc sql;
	select "'"||TRIM(bf_ssn)||"'" into :avar from r2;
quit;


%put &avar;

PROC SQL NOPRINT;
	SELECT "'"||TRIM(LENDER_ID)||"'"
		INTO :UHEAA_LIST SEPARATED BY ","
	FROM SAS_TAB.LDR_AFF
	WHERE AFFILIATION = 'UHEAA';
QUIT;
