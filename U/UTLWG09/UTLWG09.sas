/*
UNAFFILIATED LOANS GUARANTEED

Lists loans guaranteed during the previous year for out of state lenders and unaffilated schools where
the borrower does not have a Utah address, driver's license, or SSN.
Used to monitor compliance with UHEAA guarantee eligibility policies.

*/


/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWG09.LWG09R2";
*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
OPTIONS MPRINT SYMBOLGEN;
DATA _NULL_;
     BEGDATE = PUT(INTNX('YEAR',TODAY(),-1), YYMMDD10.);	/*RESOLVES TO 1ST OF PRIOR YEAR*/
     ENDDATE = PUT(INTNX('YEAR',TODAY(),0)-1 , YYMMDD10.);	/*RESOLVES TO END OF PRIOR YEAR*/
     CALL SYMPUT('BEGIN',"'"||BEGDATE||"'");	            /*CREATES MACRO VARIABLE WITH IN FORMAT  'YYYY/MM/DD'*/
     CALL SYMPUT('END',"'"||ENDDATE||"'");              	/* WILL BE USED AS REPLACEMENTS IN CODE*/
RUN;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE NONUTGUARSLD AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.AF_APL_ID||A.AF_APL_ID_SFX							AS CLID,
	B.df_prs_id_br											AS SSN,
	B.af_cur_apl_ops_ldr									AS LENDER,
	D.im_ist_ful											AS LNAME,
	B.af_apl_ops_scl										AS SCHOOL,
	C.im_ist_ful											AS SNAME
FROM	OLWHRM1.GA10_LON_APP A INNER JOIN
		OLWHRM1.GA01_APP B ON
			A.af_apl_id = B.af_apl_id AND
			A.ac_prc_sta = 'A' AND
			A.ad_prc BETWEEN &BEGIN AND &END AND
			B.af_cur_apl_ops_ldr NOT IN ('822373', '817546', '817440', '833828', '817545',
										 '829123', '830791', '829158', '830146', '830132',
										 '817455', '826717', '829769', '830240', '820200') AND
			B.af_apl_ops_scl NOT IN ('02270800', '02270801', '02612200', '00367000', '00367600',
					 	 '00367100', '02573100', '02504000', '02556800', '02361000', '00367200',
						 '02298500', '03082100', '02360800', '00522000', '02078800', '00367600',
						 '00367900', '00367800', '02516100', '00367400', '00367404', '00367403',
						 '00367401', '00367500', '03030601', '03030602', '03030600', '00367101',
						 '00367700', '00402700', '02531800', '00368000', '00368100', '01147900',
						 '03108200', '02178500', '01017300', '02557700', '03110400', '00367102',
						 '02098805', '00160600', '00162500', '03030603', '03030604', '02098800',
						 '00522100', '00367901', '02572700', '02579000', '01116600', '88888800') AND
			SUBSTR(B.df_prs_id_br,1,3) NOT IN ('528', '529', '646', '647') AND
			SUBSTR(B.df_prs_id_eds,1,3) NOT IN ('528', '529', '646', '647') AND
			SUBSTR(B.df_prs_id_stu,1,3) NOT IN ('528', '529', '646', '647') FULL OUTER JOIN
		OLWHRM1.SC01_LGS_SCL_INF C ON
			B.af_apl_ops_scl = C.if_ist INNER JOIN
		OLWHRM1.LR01_LGS_LDR_INF D ON
			B.af_cur_apl_ops_ldr = D.if_ist INNER JOIN
		OLWHRM1.PD01_PDM_INF E ON
			(B.df_prs_id_br = E.df_prs_id OR
			 B.df_prs_id_eds = E.df_prs_id OR
			 B.df_prs_id_stu = E.df_prs_id) AND
			E.dc_dom_st <> 'UT' AND
			E.dc_st_drv_lic <> 'UT'

WHERE	NOT EXISTS (SELECT	*
					FROM 	OLWHRM1.GA10_LON_APP Z,
							OLWHRM1.GA01_APP Y
							WHERE
								Z.af_apl_id = Y.af_apl_id AND
								Z.ac_prc_sta = 'A' AND
								B.df_prs_id_br = Y.df_prs_id_br AND
								A.ad_prc > Z.ad_prc)
ORDER BY C.im_ist_ful, B.df_prs_id_br
);
DISCONNECT FROM DB2;

ENDRSUBMIT;

DATA _NULL_;
	 EFFYR = PUT(INTNX('YEAR',TODAY(),-1), YEAR4.);
     CALL SYMPUT('EFFDATE',EFFYR);
RUN;
/*PROC PRINTTO PRINT=REPORT2;
RUN;*/

%macro rptprnt(dsname);
%let dsid=%sysfunc(open(&dsname));
%let hasobs=%sysfunc(attrn(&dsid,any));
%let rc=%sysfunc(close(&dsid));
%if &hasobs=1 %then
	%do;
		PROC PRINT DATA = &dsname NOOBS SPLIT='/' WIDTH=MIN;
		BY SNAME;
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
		TITLE2	"UNAFFILIATED GUARANTEES FOR THE YEAR OF &EFFDATE";
		TITLE3	'WITH EDITS EXCLUDING UTAH LENDERS, ADDRESSES, LICENSES, AND SSNs';
		FOOTNOTE	'JOB = UTLWG09     REPORT = ULWG09.LWG09R2';
		RUN;
	%end;
%else %if &hasobs=0 %then
	%do;
    	data _null_;
            file print notitles;
            put @01 'No records found for ULWG09.LWG09R2 -- UNAFFILIATED GUARANTEES WITH EDITS FOR UTAH LENDER AND UTAH ADDRESS, LICENSE, OR SSN';
        run;
    %end;
%mend rptprnt;
%rptprnt(worklocl.nonutguarsld);
