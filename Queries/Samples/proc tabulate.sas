/* Phone Contacts/Attempts in Default Prevention

Lists totals of contacts/attempts for default prevention by area and by user for the previous month.

*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWDPI.LWDPIR2";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
OPTIONS MPRINT SYMBOLGEN;
DATA _NULL_;
     BEGDATE = PUT(INTNX('MONTH',TODAY(),-1), YYMMDD10.);	/*RESOLVES TO 1ST OF PRIOR MONTH*/
     ENDDATE = PUT(INTNX('MONTH',TODAY(),0)-1 , YYMMDD10.);	/*RESOLVES TO END OF PRIOR MONTH*/
     CALL SYMPUT('BEGIN',"'"||BEGDATE||"'");	            /*CREATES MACRO VARIABLE WITH IN FORMAT  'YYYY/MM/DD'*/
     CALL SYMPUT('END',"'"||ENDDATE||"'");              	/* WILL BE USED AS REPLACEMENTS IN CODE*/
RUN;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DPPHN AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	bf_lst_usr_ay01		as userid,
	case 
		when pf_act in ('ADMXC', 'ADD14', 'ADD33', 'ADD36') then 'Autodialer Contacts'
		when pf_act in ('AAMXA', 'ABADP', 'ADD03', 'ADD08',
						'ADD51', 'ADD52', 'ADD56', 'ADD59', 'ADD89') then 'Autodialer Attempts'
		when pf_act in ('AD114') then 'Manual Contacts'
		when pf_act in ('AD102', 'AD103', 'AD108', 'AD151',
						'AD152', 'AD156', 'AD159', 'AD189') then 'Manual Attempts'
		when pf_act in ('ABNDP') then 'Non-DP Account'
		when pf_act in ('AFUWS') then 'School/Servicer Follow Up'
	end					as action
	
FROM	OLWHRM1.AY01_BR_ATY
WHERE	pf_act in ('ADMXC', 'ADD14', 'ADD33', 'ADD36', 'AAMXA', 'ABADP', 'ADD03', 'ADD08',
				   'ADD51', 'ADD52', 'ADD56', 'ADD59', 'ADD89', 'AD114', 'AD102', 'AD103',
				   'AD108', 'AD151', 'AD152', 'AD156', 'AD159', 'AD189', 'ABNDP', 'AFUWS') and
		bd_aty_prf between &begin and &end
);
DISCONNECT FROM DB2;
ENDRSUBMIT;
/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS PAGENO=1 LS=132;
proc tabulate data = worklocl.dpphn;
	class userid action;
	table userid ALL, action all;
	title 'Phone Contacts/Attempts in Default Prevention';
	FOOTNOTE  'JOB = UTLWDPI     REPORT = ULWDPI.LWDPIR2';
run;
