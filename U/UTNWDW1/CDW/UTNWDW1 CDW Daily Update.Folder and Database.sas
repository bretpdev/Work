data _null_;
	call symput('start',put(time(),time9.));
run;

LIBNAME DB ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\&DB..dsn; update_lock_typ=nolock; bl_keepnulls=no";

/*	Gather the email addresses for error reports*/
PROC SQL NOPRINT;
CONNECT TO ODBC AS BSYS (REQUIRED="FILEDSN=X:\PADR\ODBC\&ODBCFILE..dsn;");
CREATE TABLE test AS
SELECT *
FROM CONNECTION TO BSYS (
SELECT a.WINUNAME 
	,A.TYPEKEY as type
FROM GENR_REF_MISCEMAILNOTIF a
where A.TYPEKEY in ('SAS Error','SAS Stats')
order by WINUNAME
);
DISCONNECT FROM BSYS;

SELECT TRIM(WINUNAME) || '@UTAHSBR.EDU' INTO :err SEPARATED BY '" "'
FROM test
WHERE TYPE = 'SAS Error';

SELECT TRIM(WINUNAME) || '@UTAHSBR.EDU' INTO :stat SEPARATED BY '" "'
FROM test
WHERE TYPE = 'SAS Stats';
;
QUIT;

FILENAME ERRMESS EMAIL to=("&err");
FILENAME STATMESS EMAIL to=("&err");

data _null_;
	call symput('start',put(time(),time9.));
	call symput('day_of_year',catt('',"#",(today() - intnx('year',today(),-1,'e')),"*"));
run;

%LET K=0;
%LET R=0;
%let error=0;

PROC FORMAT LIBRARY=WORK.FORMATS CNTLIN=DB.FORMATTRANSLATION; RUN;
%macro truncat1(r, column);
	retain trunc 0;
	if &column = '-Begin-' then do;
		if trunc = 0 then do;
			trunc = 1;
			delete;
		end;
		else do;
				call symput('ERROR',"&r begin");
		end;
		delete;
	end;
	else if &column = '-End-' then do;
		if trunc = 1 then trunc = 0;
		else do;
				call symput('ERROR',"&r end");
		end;
		delete;
	end;
if eof and &column ^= '-End-' then do;
		call symput('ERROR',"&r eof");
end;
%MEND;

%macro truncatnum(r, column, begin, end, fmt);
	retain trunc 0;
/*	DATA _NULL_;*/
/*		CALL SYMPUTX('begin',"'"||PUT(&begin,&fmt)||"'" );*/
/*		CALL SYMPUTX('end',"'"||PUT(&end,&fmt)||"'" );*/
/*	RUN;*/

	if &column = input(&begin, &fmt) then do;
		if trunc = 0 then do;
			trunc = 1;
			delete;
		end;
		else do;
				call symput('line123',"&r begin");
		end;
		delete;
	end;
	else if &column = input(&end, &fmt) then do;
		if trunc = 1 then trunc = 0;
		else do;
				call symput('line130',"&r end");
		end;
		delete;
	end;
if eof and &column ^= input(&end, &fmt) then do;
		call symput('line135',"&r eof");
end;
%MEND;

%macro truncat(r);
	retain trunc 0;
	if df_spe_acc_id = '-Begin-' then do;
		if trunc = 0 then do;
			trunc = 1;
			delete;
		end;
		else do;
				call symput('ERROR',"&r");
		end;
		delete;
	end;
	else if df_spe_acc_id = '-End-' then do;
		if trunc = 1 then trunc = 0;
		else do;
				call symput('ERROR',"&r");
		end;
		delete;
	end;
if eof and df_spe_acc_id ^= '-End-' then do;
		call symput('ERROR',"&r");
end;
%MEND;

%MACRO FILECHECK(i);
	FILENAME REPORT&I "&RPTLIB/UNWDW1.NWDW1R&I..*";
	%IF %SYSFUNC(FEXIST(REPORT&I)) = 0 %THEN %DO;
			%put "File &i was missing";
			%let k = &i;
	%END;
	%IF &k > 0 %THEN %DO;
		DATA _NULL_;
		FILE ERRMESS subject="CDW Error - Missing File&TESTMSG";
			PUT "The CDW Update Process was Aborted for Report &k!";
			PUT "The file was missing!";
		RUN;
/*		ENDSAS;*/
	%END;
%MEND;
%MACRO COPYERROR;
%IF &error > 0 %THEN %DO;
DATA _NULL_;
FILE ERRMESS SUBJECT= "CDW Error - Corrupted Data&TESTMSG";
	PUT "The CDW Update Process was Aborted:";
	PUT "File &error had corrupted (truncated) data." ;
RUN;
/*ENDSAS;*/
%END;
%MEND;

%MACRO FINISH(SOURCE=FILE);
DATA _NULL_;
	finish = put(time(),time9.);
	format total time9.;
	total = time() - "&start"t;
	PUT "&SOURCE";
	PUT "START = &START ";
	PUT "FINISH = " FINISH;
	put "TOTAL = " total; 
RUN;
%MEND;

%MACRO ENCHILADA(INSET,SQLTBL,PRIKEY,ATTR,FILTER);
data _null_;
i = 1;
do until (pri = '');
	pri = scan("&attr",i);
	if pri = '' then call symput('attr_ct',i-1);
	i + 1;
end;
i = 1;
do until (pri = '');
	pri = scan("&PRIKEY",i);
	if pri = '' then call symput('key_ct',i-1);
	i + 1;
end;  
run;
%PUT &ATTR_CT &KEY_CT;

PROC SQL;
CREATE TABLE NO_DELTA AS
SELECT DISTINCT A.%SCAN(&PRIKEY,1)
%if &key_ct > 1 %then %DO i = 2 %to &key_ct;
	,A.%SCAN(&PRIKEY,&I)
%end;
FROM &INSET A
INNER JOIN DB.&SQLTBL B
      ON A.%SCAN(&PRIKEY,1) = B.%SCAN(&PRIKEY,1)
%if &key_ct > 0 %then %DO i = 2 %to &key_ct;
	 AND A.%SCAN(&PRIKEY,&I) = B.%SCAN(&PRIKEY,&I)
%end;
%if &attr_ct > 0 %then %DO i = 1 %to &attr_ct;
	 AND A.%SCAN(&ATTR,&I) = B.%SCAN(&ATTR,&I)
%end;
;

CREATE TABLE DELTAS AS
SELECT DISTINCT A.*
FROM &INSET A
LEFT JOIN NO_DELTA B
	ON A.%SCAN(&PRIKEY,1) = B.%SCAN(&PRIKEY,1)
%if &key_ct > 0 %then %DO i = 2 %to &key_ct;
	 AND A.%SCAN(&PRIKEY,&I) = B.%SCAN(&PRIKEY,&I)
%end;
WHERE B.%SCAN(&PRIKEY,1) IS NULL;
QUIT;

PROC SORT DATA=DELTAS NODUPKEY OUT=DELTA_KEY(KEEP=&PRIKEY);
BY &PRIKEY;
RUN;
PROC SQL;
CREATE TABLE LOCL_DELTAS AS
      SELECT A.* 
      FROM DB.&SQLTBL A
      INNER JOIN DELTAS B
            ON A.%SCAN(&PRIKEY,1) = B.%SCAN(&PRIKEY,1)
		%if &key_ct > 0 %then %DO i = 2 %to &key_ct;
			 AND A.%SCAN(&PRIKEY,&I) = B.%SCAN(&PRIKEY,&I)
		%end;
;

INSERT INTO DB.ZDEL_&SQLTBL
	SELECT A.%SCAN(&PRIKEY,1)
%if &key_ct > 1 %then %DO i = 2 %to &key_ct;
	,A.%SCAN(&PRIKEY,&I)
%end;
      FROM LOCL_DELTAS A; 
QUIT;

PROC SORT DATA=LOCL_DELTAS; BY &PRIKEY; RUN;
PROC SORT DATA=DELTAS; BY &PRIKEY; RUN;

DATA LOCL_DELTAS;
	MERGE LOCL_DELTAS DELTAS;
	BY &PRIKEY;
RUN;

PROC SQL NOPRINT;
CONNECT TO ODBC AS MD ("FILEDSN=X:\PADR\ODBC\&DB..dsn; update_lock_typ=nolock; bl_keepnulls=no");
SELECT COUNT(*) 
FROM CONNECTION TO MD (
      IF EXISTS(SELECT * FROM &SQLTBL A INNER JOIN ZDEL_&SQLTBL B ON A.%SCAN(&PRIKEY,1) = B.%SCAN(&PRIKEY,1) %if &key_ct > 0 %then %DO i = 2 %to &key_ct; AND A.%SCAN(&PRIKEY,&I) = B.%SCAN(&PRIKEY,&I)	%end;)
	     BEGIN
			DELETE &SQLTBL
	      	FROM &SQLTBL A
	      	INNER JOIN ZDEL_&SQLTBL B
	            ON A.%SCAN(&PRIKEY,1) = B.%SCAN(&PRIKEY,1)
			%if &key_ct > 0 %then %DO i = 2 %to &key_ct;
				 AND A.%SCAN(&PRIKEY,&I) = B.%SCAN(&PRIKEY,&I)
			%end;
		END
);
DISCONNECT FROM MD;
QUIT;

PROC SQL ;
INSERT INTO DB.&SQLTBL
(%SCAN(&PRIKEY,1)
%if &key_ct > 0 %then %DO i = 2 %to &key_ct;
	 	,%SCAN(&PRIKEY,&I)
%end;
%if &attr_ct > 0 %then %DO i = 1 %to &attr_ct;
	 	,%SCAN(&ATTR,&I)
%end;
)

      SELECT %SCAN(&PRIKEY,1)
%if &key_ct > 0 %then %DO i = 2 %to &key_ct;
	 	,%SCAN(&PRIKEY,&I)
%end;
%if &attr_ct > 0 %then %DO i = 1 %to &attr_ct;
	 	,%SCAN(&ATTR,&I)
%end;
      FROM LOCL_DELTAS
	  &FILTER
; 
QUIT;
PROC SQL NOPRINT;
CONNECT TO ODBC AS MD ("FILEDSN=X:\PADR\ODBC\&DB..dsn; update_lock_typ=nolock; bl_keepnulls=no");
SELECT COUNT(*) 
FROM CONNECTION TO MD (
      truncate table ZDEL_&SQLTBL 
      );
QUIT;
%MEND; 

%MACRO UPDATE_DATA(BASE_DS,UPDATE_DS,PRI_KEY);
PROC SORT DATA=&BASE_DS; BY &PRI_KEY; RUN;
PROC SORT DATA=&UPDATE_DS; BY &PRI_KEY; RUN;
DATA &BASE_DS;
MERGE &BASE_DS &UPDATE_DS;
BY &PRI_KEY;
RUN;
%MEND UPDATE_DATA;
%MACRO GOODBYE_NULL(TABLE,COLUMN,BLANK);
PROC SQL NOPRINT;
CONNECT TO ODBC AS DB (REQUIRED="FILEDSN=X:\PADR\ODBC\&DB..dsn; update_lock_typ=nolock; bl_keepnulls=no");
SELECT COUNT(*) 
FROM CONNECTION TO DB (
	IF EXISTS(SELECT * FROM &TABLE WHERE &COLUMN IS NULL)
    	UPDATE 
			&TABLE
	 	SET 
			&COLUMN = &BLANK
		WHERE 
			&COLUMN IS NULL
		);
DISCONNECT FROM DB;
QUIT;
OPTIONS OBS=MAX;
%MEND;
%MACRO CLEANUP_LOAN(TAB);
PROC SQL NOPRINT;
CONNECT TO ODBC AS DB (REQUIRED="FILEDSN=X:\PADR\ODBC\&DB..dsn; update_lock_typ=nolock; bl_keepnulls=no");
SELECT COUNT(*) 
FROM CONNECTION TO DB (
	IF EXISTS(SELECT * FROM &TAB A INNER JOIN LN10_LOAN B ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID AND A.LN_SEQ = B.LN_SEQ	WHERE B.LA_CUR_PRI = 0)
		BEGIN
	      DELETE &TAB
	      FROM &TAB A
	      INNER JOIN LN10_LOAN B
	            ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
				AND A.LN_SEQ = B.LN_SEQ
		  WHERE B.LA_CUR_PRI = 0
		END
);
DISCONNECT FROM DB;
QUIT;
%MEND;
%MACRO CLEANUP_PARENT(TAB,PAR);
PROC SQL NOPRINT;
CONNECT TO ODBC AS DB (REQUIRED="FILEDSN=X:\PADR\ODBC\&DB..dsn; update_lock_typ=nolock; bl_keepnulls=no");
SELECT COUNT(*) 
FROM CONNECTION TO DB (
	IF EXISTS(SELECT * FROM &TAB A LEFT OUTER JOIN &PAR B ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID WHERE B.DF_SPE_ACC_ID IS NULL)
		BEGIN
			DELETE &TAB
		    FROM &TAB A
		    LEFT OUTER JOIN &PAR B
		   		ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
			WHERE B.DF_SPE_ACC_ID IS NULL
		END
);
DISCONNECT FROM DB;
QUIT;
%MEND;

OPTIONS NOTES SOURCE MPRINT NOSYMBOLGEN NOXWAIT NOXSYNC;
