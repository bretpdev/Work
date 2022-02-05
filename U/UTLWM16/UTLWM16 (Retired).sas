*------------------------------*
| UTLWM16 DEFAULTS WITH MEMPLS |
*------------------------------*;
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWM16.LWM16R2";
FILENAME REPORTZ "&RPTLIB/ULWM16.LWM16RZ";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT DUSTER;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SORT DATA=DLGSUTWH.AY01_BR_ATY OUT=SEL(KEEP=DF_PRS_ID BD_ATY_PRF PF_ACT BX_CMT); 
BY DF_PRS_ID BD_ATY_PRF PF_ACT;
WHERE PF_ACT IN ('MEMPL','MEMP0','DEMP1','DEMPL');
RUN;
DATA SEL(DROP=A PF_ACT);
SET SEL;
BY DF_PRS_ID BD_ATY_PRF PF_ACT;
RETAIN A;
IF FIRST.DF_PRS_ID THEN A = 0;
IF PF_ACT = 'DEMPL' AND BD_ATY_PRF >= TODAY() - 10 THEN A = 1;

IF LAST.DF_PRS_ID AND PF_ACT = 'MEMPL' AND A=0 THEN DO;
if BX_CMT in ('# (LPXG0   )',
	'N/A (LPXG0   )',
	'NA (LPXG0   )',
	'EMPLOYER (LPXG0   )',
	'UNKNOWN (LPXG0   )',
	'UKNOWN (LPXG0   )',
	'YNKNOWN (LPXG0   )',
	'* (LPXG0   )',
	'OTHER (LPXG0   )',
	'EMP (LPXG0   )',
	'1 (LPXG0   )',
	'## (LPXG0   )',
	'. (LPXG0   )',
	'E (LPXG0   )',
	'***** (LPXG0   )',
	'- (LPXG0   )',
	' # (LPXG0   )',
	' , (LPXG0   )',
	'/ (LPXG0   )',
	'/* (LPXG0   )',
	'0 (LPXG0   )',
	'1 (LPXG0   )',
	'@ (LPXG0   )',
	'A (LPXG0   )',
	'? (LPXG0   )',
	'¬ (LPXG0   )',
	'WRK (LPXG0   )',
	'WRK # (LPXG0   )',
	'WK (LPXG0   )',
	'WORK (LPXG0   )',
	'WORK # (LPXG0   )',
	'XXX (LPXG0   )',
	'XX (LPXG0   )',
	'X (LPXG0   )',
	'### (LPXG0 )') then delete;
ELSE OUTPUT;
end;
RUN;

DATA SEL(KEEP=DF_PRS_ID BD_ATY_PRF NEW_STRING );
SET SEL; 
LENGTH NEW_STRING $ 250;
length x $ 600;
i = 1;
do until (scan(bx_cmt,i,' ') = '');
	if i = 1 then x = scan(bx_cmt,1,' ');
	else x = trim(x) || ' ' || scan(bx_cmt,i,' ');
	i = i + 1;
end;

new_string = substr(x,1,250);
output;
if length(x) > 250 then do;
	new_string = substr(x,251,250);
	output;
end;
if length(x) > 500 then do;
	new_string = substr(x,501,100);
	output;
end;
run;

PROC SQL;
CREATE TABLE DEFWME AS
SELECT DISTINCT A.BF_SSN
	,B.BD_ATY_PRF
	,B.NEW_STRING
FROM DLGSUTWH.DC01_LON_CLM_INF A
INNER JOIN SEL B
	ON A.BF_SSN = b.df_prs_id
LEFT OUTER JOIN DLGSUTWH.CT30_CALL_QUE Y
	ON Y.DF_PRS_ID_BR = A.BF_SSN
	AND Y.IF_WRK_GRP = 'DEMPCALL'

WHERE A.LC_STA_DC10 = '03'
	AND A.LD_CLM_ASN_DOE = .
	AND A.LC_AUX_STA = ''
	and y.df_prs_id_br is null
order by bf_ssn ,bd_aty_prf;
quit;
ENDRSUBMIT;

DATA DEFWME;
SET DUSTER.DEFWME;
RUN;

DATA _NULL_;
SET DEFWME; 
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT COMMENTS $600. BD_ATY_PRF MMDDYY10.;
	INSTITUTION_ID = '';
	INSTITUTION_TYPE = '';
	DATE_DUE = '';
	TIME_DUE = '';
	QUEUE_NAME = 'DEMPCALL';
	COMMENTS = PUT(BD_ATY_PRF,MMDDYY10.)||','|| TRIM(NEW_STRING);
DO;
	PUT BF_SSN $ @;
	PUT QUEUE_NAME $ @;
	PUT INSTITUTION_ID $ @;
	PUT INSTITUTION_TYPE $ @;
	PUT DATE_DUE $ @;
	PUT TIME_DUE $ @;
	PUT COMMENTS $ ;
END;
RUN;
