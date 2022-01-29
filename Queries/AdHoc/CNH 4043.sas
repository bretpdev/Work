
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT distinct
						LNXX.BF_SSN,
						AYXX.PF_REQ_ACT,
						AYXX.LD_ATY_REQ_RCV
					FROM
						PKUB.LNXX_LON LNXX
					INNER JOIN PKUB.AYXX_BR_LON_ATY AYXX
						ON AYXX.BF_SSN = LNXX.BF_SSN
					WHERE
						AYXX.PF_REQ_ACT IN ('DITLF' , 'DIPSF', 'DIFRB', 'DIDFR','DICSK', 'DIUPR','DIFCR')
						AND AYXX.LC_STA_ACTYXX = 'A'
						AND DAYS(AYXX.LD_ATY_REQ_RCV) BETWEEN DAYS('XX/XX/XXXX') AND DAYS('XX/XX/XXXX') 

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA TLFPSF;
SET LEGEND.DEMO;
WHERE PF_REQ_ACT IN ('DITLF', 'DIPSF');
RUN;

DATA TLFPSF_dedup(drop=PF_REQ_ACT LD_ATY_REQ_RCV);
set TLFPSF;
run;

proc sql;
create table TLFPSF_final as 
	select distinct bf_ssn from TLFPSF_dedup
	;
quit;



DATA DIFRB;
SET LEGEND.DEMO;
WHERE PF_REQ_ACT = 'DIFRB';
RUN;

DATA DIUPR;
SET LEGEND.DEMO;
WHERE PF_REQ_ACT = 'DIUPR';
RUN;

DATA DICSK;
SET LEGEND.DEMO;
WHERE PF_REQ_ACT = 'DICSK';
RUN;

DATA DIFCR;
SET LEGEND.DEMO;
WHERE PF_REQ_ACT = 'DIFCR';
RUN;

DATA DIDFR;
SET LEGEND.DEMO;
WHERE PF_REQ_ACT = 'DIDFR';
RUN;

proc sql;
create table DICSK_count as 
SELECT COUNT(*) AS DICSK_Count
	FROM DICSK
	;
QUIT;	

proc sql;
create table DIUPR_count as 
SELECT COUNT(*) AS DIUPR_Count
	FROM DIUPR
	;
QUIT;

proc sql;
create table DIFCR_count as 
SELECT COUNT(*) AS DIFCR_Count
	FROM DIFCR
	;
QUIT;


PROC SQL;
CREATE TABLE TLF_PSF_COUNT AS
	SELECT COUNT(*) AS DITLF_DIPSF_COUNT
	FROM TLFPSF_final
	;
QUIT;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.DIFRB; *Send data to Legend;
SET DIFRB;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.DIDFR; *Send data to Legend;
SET DIDFR;
RUN;

RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE FORB_DATA AS
		SELECT distinct
			FORB.BF_SSN,
			ALL_FORB.LD_CRT_REQ_FOR,
			ALL_FORB.LC_FOR_TYP,
			FORB.LD_ATY_REQ_RCV,
			forb.PF_REQ_ACT

		FROM
			 DIFRB FORB
		LEFT JOIN 
		(
			SELECT 
				FBXX.BF_SSN,
				FBXX.LD_CRT_REQ_FOR,
				FBXX.LC_FOR_TYP
			FROM 
				PKUB.FBXX_BR_FOR_REQ FBXX
			inner join DIFRB f
				ON F.BF_SSN  = FBXX.BF_SSN
			WHERE 
				LD_CRT_REQ_FOR >= f.LD_ATY_REQ_RCV
		) ALL_FORB
			ON ALL_FORB.BF_SSN = FORB.BF_SSN
		WHERE 
			ALL_FORB.LC_FOR_TYP IN ('XX','XX','XX','XX','XX','XX')
;
QUIT;
ENDRSUBMIT;


RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE DEF_DATA AS
		SELECT distinct
			DEF.BF_SSN,
			ALL_FORB.LD_CRT_REQ_DFR,
			ALL_FORB.LC_DFR_TYP,
			DEF.LD_ATY_REQ_RCV,
			DEF.PF_REQ_ACT

		FROM
			 DIDFR DEF
		LEFT JOIN 
		(
			SELECT 
				FBXX.BF_SSN,
				FBXX.LD_CRT_REQ_DFR,
				FBXX.LC_DFR_TYP
			FROM 
				PKUB.DFXX_BR_DFR_REQ FBXX
			inner join DIDFR f
				ON F.BF_SSN  = FBXX.BF_SSN
			WHERE 
				LD_CRT_REQ_DFR >= f.LD_ATY_REQ_RCV
		) ALL_FORB
			ON ALL_FORB.BF_SSN = DEF.BF_SSN
		
		WHERE 
			ALL_FORB.LC_DFR_TYP IN ('XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX')
		
;
QUIT;ENDRSUBMIT;

DATA DEF_MIN (DROP=LD_ATY_REQ_RCV LC_DFR_TYP LD_ATY_REQ_RCV PF_REQ_ACT);
SET legend.DEF_DATA;
MINDATE = MIN(LD_CRT_REQ_DFR, MINDATE);
MINDATEX = MIN(LD_ATY_REQ_RCV, MINDATEX);
FORMAT MINDATE MMDDYYXX.;
FORMAT MINDATEX MMDDYYXX.;
RUN;

proc sql;
create table def_min_X as 
	select distinct
		bf_ssn,
		min(mindate)format mmddyyXX. as dte,
		min(mindateX)format mmddyyXX. as dteX

	from
		def_min
	group by
		bf_ssn
	;
quit;

/*LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;*/
DATA LEGEND.def_min_X; *Send data to Legend;
SET def_min_X;
RUN;


proc sql;
create table final_def as 
	select
		dd.bf_ssn,
		LD_CRT_REQ_DFR,
		LC_DFR_TYP,
		LD_ATY_REQ_RCV,
		PF_REQ_ACT
	from
		legend.DEF_DATA dd
	inner join legend.def_min_X d
		on d.bf_ssn = dd.bf_ssn
		and d.dte = dd.LD_CRT_REQ_DFR
		and d.dteX = dd.LD_ATY_REQ_RCV
		;
quit;

proc sql;
create table DIFRB_count as 
	select count(*) as DIFRB_count
	from legend.FORB_DATA
	;
	quit;

proc sql;
create table DIDFR_count as 
	select count(*) as DIDFR_count
	from final_def
	;
	quit;


PROC EXPORT DATA = WORK.DICSK_count 
            OUTFILE = "T:\SAS\NH XXXX COUNTS.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DICSK"; 
RUN;

PROC EXPORT DATA = WORK.DIUPR_count 
            OUTFILE = "T:\SAS\NH XXXX COUNTS.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DIUPR"; 
RUN;

PROC EXPORT DATA = WORK.DIFCR_count 
            OUTFILE = "T:\SAS\NH XXXX COUNTS.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DIFCR"; 
RUN;

PROC EXPORT DATA = WORK.Tlf_psf_count 
            OUTFILE = "T:\SAS\NH XXXX COUNTS.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DITLF and DIPSF"; 
RUN;

PROC EXPORT DATA = WORK.DIDFR_count 
            OUTFILE = "T:\SAS\NH XXXX COUNTS.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DIDFR"; 
RUN;

PROC EXPORT DATA = WORK.DIFRB_count 
            OUTFILE = "T:\SAS\NH XXXX COUNTS.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DIFRB"; 
RUN;
/**/







PROC EXPORT DATA = WORK.DICSK;
            OUTFILE = "T:\SAS\NH XXXX details.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DICSK"; 
RUN;

PROC EXPORT DATA = WORK.DIUPR
            OUTFILE = "T:\SAS\NH XXXX details.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DIUPR"; 
RUN;

PROC EXPORT DATA = WORK.DIFCR
            OUTFILE = "T:\SAS\NH XXXX details.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DIFCR"; 
RUN;

PROC EXPORT DATA = WORK.tlfpsf 
            OUTFILE = "T:\SAS\NH XXXX details.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DITLF and DIPSF"; 
RUN;

PROC EXPORT DATA = WORK.final_def 
            OUTFILE = "T:\SAS\NH XXXX details.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DIDFR"; 
RUN;

PROC EXPORT DATA = legend.forb_data 
            OUTFILE = "T:\SAS\NH XXXX details.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DIFRB"; 
RUN;








/*export to Excel spreadsheet*/
/*PROC EXPORT DATA = WORK.DEMO */
/*            OUTFILE = "T:\SAS\EXCEL OUTPUT.xls" */
/*            DBMS = EXCEL*/
/*			REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/

