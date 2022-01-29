
PROC CONTENTS DATA=DEMO OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT /*REPORT2*/;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 132 /*126*/ *'-';
	PUT      ////////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
		@57 '-- END OF REPORT --';
	PUT ////////////////
		@46 "JOB = UTLW___     REPORT = ULW___.LW___R_";
	END;
RETURN;
TITLE 'PENDING BANKRUPTCY HARDSHIP CLAIMS';
run;

*FOR PORTRAIT;
OPTIONS CENTER NODATE NUMBER PAGENO=1 LS=90;
PROC CONTENTS DATA=DADISBLDR OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT /*REPORT2*/;
IF  NOBS=0 AND _N_ =1 THEN DO;
   PUT // 90*'-';
   PUT      ////////
       @31 '**** NO DISBURSEMENTS FOUND ****';
   PUT ////////
       @37 '-- END OF REPORT --';
   PUT /////////////////////////////
   		@26 "JOB = UTLWL01     REPORT = ULWL01.LWL01R&REPNO";
   END;
RETURN;
TITLE 'DAILY DISBURSEMENT REPORT';
TITLE2 "LENDER ID # &LENDER";
TITLE3 "DISBURSEMENT DATE : &DSBDAT";
FOOTNOTE "JOB = UTLWL01     REPORT = ULWL01.LWL01R&REPNO";
run;

*OR: ;

%macro rptprnt(dsname);
%let dsid=%sysfunc(open(&dsname));
%let hasobs=%sysfunc(attrn(&dsid,any));
%let rc=%sysfunc(close(&dsid));
%if &hasobs=1 %then
	%do;
		PROC PRINT DATA = &dsname NOOBS SPLIT='/' WIDTH=MIN;
		VAR SSN
			CLID
			LENDER
			LNAME
			SCHOOL
			SNAME;
		LABEL	CLID = 'UNIQUE ID'
				LENDER = 'LENDER ID'
				LNAME = 'LENDER NAME'
				SCHOOL = 'SCHOOL ID'
				SNAME = 'SCHOOL NAME';
		TITLE1	'UTAH HIGHER EDUCATION ASSISTANCE AUTHORITY';
		TITLE2	"UNAFFILIATED GUARANTEES FOR THE MONTH OF &EFFDATE";
		FOOTNOTE	'JOB = UTLWG07     REPORT = ULWG07.LWG07R2';
		RUN;
	%end;
%else %if &hasobs=0 %then
	%do;
    	data _null_;
            file print notitles;
            put @01 'No records found for ULWG07.LWG07R2';
        run;
    %end;
%mend rptprnt;
%rptprnt(worklocl.nonutguars);