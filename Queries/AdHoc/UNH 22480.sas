LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;
LIBNAME XL 'T:\HP 21824-LN56 DCR.XLSX';

DATA INSET;
	SET XL.'LN56$'N;
RUN;

DATA DUSTER.INSET; SET INSET; RUN;

RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE LN56TS AS
		SELECT DISTINCT
			INSET.BF_SSN,
			INSET.LN_SEQ,
			LN56.LF_CRT_DTS_LN56,
			INSET.LC_RPT_STA_CRB_Current,
			INSET.LC_RPT_STA_CRB_Updated
		FROM
			INSET
			JOIN OLWHRM1.LN56_LON_CRB_RPT LN56
				ON INSET.BF_SSN = LN56.BF_SSN
				AND INSET.LN_SEQ = LN56.LN_SEQ
				AND DATEPART(INSET.LF_CRT_DTS_LN56*86400) = DATEPART(LN56.LF_CRT_DTS_LN56)
				AND INSET.LC_RPT_STA_CRB_Current = LN56.LC_RPT_STA_CRB
		ORDER BY
			INSET.BF_SSN,
			INSET.LN_SEQ
	;
QUIT;
ENDRSUBMIT;

DATA LN56TS; SET DUSTER.LN56TS; RUN;

DATA LN56TSF;
	SET LN56TS;
	date = put(datepart(LF_CRT_DTS_LN56), yymmdd10.);
	hour = hour(LF_CRT_DTS_LN56);
	if hour < 10 then
		hour1= cats('0',compress(hour));
	if hour1 = . then
		hour1 = compress(hour);


	minute = minute(LF_CRT_DTS_LN56);

	if minute < 10 then
		minute1= cats('0',compress(minute));
	if minute1 = . then
		minute1 = compress(minute);

	sec = round(second(LF_CRT_DTS_LN56), .0000005);

	trailzero= substr(compress(sec)||"000000000",1,9);

	val = cats(date, "-", hour1,".",minute1,".", trailzero);


RUN;

PROC SQL;
	CREATE TABLE LN56F AS
		SELECT
			BF_SSN,
			LN_SEQ,
			val as LF_CRT_DTS_LN56,
			LC_RPT_STA_CRB_Current,
			LC_RPT_STA_CRB_Updated
		FROM
			LN56TSF
	;
quit;

/*Export to CSV and paste in excel for all columns as text*/
PROC EXPORT DATA = WORK.LN56F 
            OUTFILE = "T:\SAS\LN56 DCR WITH TIMESTAMP.CSV" 
            DBMS = CSV 
			REPLACE;
     PUTNAMES = YES;
RUN;
