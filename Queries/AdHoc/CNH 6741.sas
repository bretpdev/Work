/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						lnXX.*
					FROM
						 PKUB.LNXX_LON LNXX
					left join PKUB.LNXX_INT_RTE_HST lnXX
						ON lnXX.BF_SSN = LNXX.BF_SSN
						AND lnXX.LN_SEQ = LNXX.LN_SEQ
					WHERE
						LNXX.IC_LON_PGM = 'DLPLUS'
						AND LNXX.LD_LON_ACL_ADD BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
					ORDER BY LNXX.LD_LON_ACL_ADD


					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO;
SET LEGEND.DEMO;
RUN;

proc sql;
	create table old as 
		select distinct
			BF_SSN,
			LN_SEQ,
			LR_ITR,
			MAX(LD_STA_LONXX) format mmddyyXX.
		from
			demo
		where
			LC_STA_LONXX = 'I'
			and LR_ITR = X.XXX
		group by 
			BF_SSN,
			LN_SEQ,
			LR_ITR
;
quit;

proc sql;
	create table new as 
		select distinct
			d.BF_SSN,
			d.LN_SEQ,
			LR_ITR,
			MAX(LD_STA_LONXX) format mmddyyXX. as EFF_DATE
		from
			demo d
		where
			LR_ITR ^= X.XXX
			and LC_STA_LONXX = 'A'
		group by 
			d.BF_SSN,
			d.LN_SEQ,
			LR_ITR
		order by 
			d.BF_SSN,
			d.LN_SEQ
;
quit;

proc sql;
	create table final as
		select distinct
			n.bf_ssn AS SSN,
			n.ln_seq AS LOAN_SEQ,
			o.LR_ITR as OLD_RATE,
			n.LR_ITR as NEW_RATE,
			n.EFF_DATE
		from
			new n
		inner join old o
			on n.bf_ssn = o.bf_ssn
			and n.ln_seq = o.ln_seq
;
quit;

PROC EXPORT DATA = WORK.final 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
