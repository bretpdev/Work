/*	This job provides the percentage breakout of all outstanding 
guarantees by school type, as of a given date specified on the first
line of code.  
Output is the Excel file "T:\SAS\School Type Percents.xls"
*/

%LET END = '2002-06-30';
%SYSLPUT END = &END;

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132 symbolgen;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE SCL AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	D.IC_SCL_TYP						AS SCHTYP,
		COUNT(b.af_apl_id||b.af_apl_id_sfx)	AS LOANCOUNT
FROM  OLWHRM1.GA01_APP A inner join OLWHRM1.GA10_LON_APP B
	on a.af_apl_id = b.af_apl_id
	and B.ac_prc_sta = 'A'				/*Approved loan*/
	and B.ad_prc <= &END
inner join OLWHRM1.GA14_LON_STA C
	on c.af_apl_id = b.af_apl_id
	and c.af_apl_id_sfx = b.af_apl_id_sfx
	AND C.AC_STA_GA14 = 'A'
inner join OLWHRM1.SC01_LGS_SCL_INF D
	on A.af_apl_ops_scl = D.if_ist
WHERE c.AC_LON_STA_TYP IN ('AL','CR','DA','FB','IA','ID','IG','IM','RP',
'RF','UA','UB','UI')
GROUP BY D.IC_SCL_TYP
);
DISCONNECT FROM DB2;
endrsubmit;

data scl;
set worklocl.scl;
LENGTH SCHOOL_TYPE $30.;
IF SCHTYP = '00' THEN SCHOOL_TYPE ='Degree, Private';
ELSE IF SCHTYP = '01' THEN SCHOOL_TYPE ='State';
ELSE IF SCHTYP = '02' THEN SCHOOL_TYPE ='State_related';
ELSE IF SCHTYP = '03' THEN SCHOOL_TYPE ='Junior';
ELSE IF SCHTYP = '04' THEN SCHOOL_TYPE ='Community (public 2 year)';
ELSE IF SCHTYP = '05' THEN SCHOOL_TYPE ='Nursing';
ELSE IF SCHTYP = '06' THEN SCHOOL_TYPE ='Vocational/Technical';
ELSE IF SCHTYP = '07' THEN SCHOOL_TYPE ='Business';
ELSE IF SCHTYP = '08' THEN SCHOOL_TYPE ='Trade';
ELSE IF SCHTYP = '09' THEN SCHOOL_TYPE ='School of Theology or Seminary';
run;

proc sql;
create table School_Types as
select 	a.SCHOOL_TYPE,
		loancount/(select sum(loancount)from scl)*100
			as PERCENT_OF_TOTAL format=8.2 LABEL='Percent of Total'
from scl a;
quit;

PROC EXPORT DATA= School_Types 
            OUTFILE= "T:\SAS\School Type Percents.xls" 
            DBMS=EXCEL2000 REPLACE;
RUN;

/* TESTFILE CREATION
libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132 symbolgen;
PROC SQL OUTOBS= 1000 feedback ;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE SCLTEST AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	A.DF_PRS_ID_BR,
		b.af_apl_id||b.af_apl_id_sfx		AS CLUID,
		A.af_apl_ops_scl,
		D.IC_SCL_TYP,
		B.ad_prc		
FROM  OLWHRM1.GA01_APP A inner join OLWHRM1.GA10_LON_APP B
	on a.af_apl_id = b.af_apl_id
	and B.ac_prc_sta = 'A'
	and B.ad_prc <= &END
inner join OLWHRM1.GA14_LON_STA C
	on c.af_apl_id = b.af_apl_id
	and c.af_apl_id_sfx = b.af_apl_id_sfx
	AND C.AC_STA_GA14 = 'A'
inner join OLWHRM1.SC01_LGS_SCL_INF D
	on A.af_apl_ops_scl = D.if_ist
WHERE c.AC_LON_STA_TYP IN ('AL','CR','DA','FB','IA','ID','IG','IM','RP',
'RF','UA','UB','UC','UD','UI')
);
DISCONNECT FROM DB2;
endrsubmit;

DATA SCLTEST;
SET WORKLOCL.SCLTEST;
RUN;

PROC EXPORT DATA= SCLTEST 
            OUTFILE= "T:\SAS\School Type Percents Testfile.xls" 
            DBMS=EXCEL2000 REPLACE;
RUN;*/