/*
SATISFACTION OF LEGAL ACTION

This report lists accounts with closed loans and outstanding legal action.
Collections uses this report to identify and satisfy outstanding legal action
on closed loans.

*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
options symbolgen;
data _null_  ;
	if day(today()) < 16 then
		do;
	   		begin=intnx('month',today(),-1,'beginning')  ;
	   		call symput  ( 'begin', "'"||put(begin,yymmdd10.)||"'" )   ;
			end=intnx('month',today(),-1,'beginning')+14 ;
	   		call symput  ( 'end', "'"||put(end,yymmdd10.)||"'" )   ;
		end;
	else
		do;
			begin=intnx('month',today(),-1,'beginning') +15 ;
	   		call symput  ( 'begin', "'"||put(begin,yymmdd10.)||"'" )   ;
	   		end=intnx('month',today(),-1,'end')  ;
	   		call symput  ( 'end', "'"||put(end,yymmdd10.)||"'" )   ;
		end;
run;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE SATIS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	E.DM_PRS_1,
	E.dm_prs_lst,
	E.dx_str_adr_1,
	E.dx_str_adr_2,
	E.dm_ct,
	E.dc_dom_st,
	E.df_zip,
	B.df_prs_id_br,
	C.bf_leg_act_dkt,	/*docket number*/
	F.bx_cmt,			/*<1>judge's name*/
    /*Court;County;Abstract*/
	D.bd_jdg_ent,		/*judgment date*/
	A.ld_lst_pay

FROM	OLWHRM1.DC01_LON_CLM_INF A INNER JOIN
		OLWHRM1.LA12_LEG_ACT_LON B ON
			A.af_apl_id = B.af_apl_id AND
			A.af_apl_id_sfx = B.af_apl_id_sfx AND
			A.lc_sta_dc10 = '04' AND
			A.LD_STA_UPD_DC10 between &begin and &end /* AND <1>
			DAYS(A.ld_lst_pay) < DAYS(CURRENT DATE) - 15 */INNER JOIN
		OLWHRM1.LA10_LEG_ACT C ON
			DATE(B.bf_crt_dts_la10) = DATE(C.bf_crt_dts_la10) AND
			B.df_prs_id_br = C.df_prs_id_br AND/*<1>
			C.bc_ina_rea = '' AND
			C.bc_wdr_rea = '' AND*/
			C.bc_leg_act_rec_typ = '2' /*LEGAL ACTION RECORD TYPE 2 = SUMMONS & COMPLAINT*/ INNER JOIN
		OLWHRM1.LA11_LEG_ACT_ATY D ON
			DATE(C.bf_crt_dts_la10) = DATE(D.bf_crt_dts_la10) AND
			B.df_prs_id_br = D.df_prs_id_br AND
			D.bc_leg_act_aty_typ = 'JD' INNER JOIN
		OLWHRM1.PD01_PDM_INF E ON
			B.df_prs_id_br = E.df_prs_id left OUTER JOIN
		OLWHRM1.AY01_BR_ATY F ON				/*<1>*/
			A.BF_SSN = F.DF_PRS_ID AND			/*<1>*/
			F.PF_ACT = 'DJGNM'					/*<1>*/
WHERE 	not exists (select * 
					from OLWHRM1.DC01_LON_CLM_INF Z
					where	Z.lc_sta_dc10 = '03' AND
							Z.ld_clm_asn_doe IS NULL AND
							a.bf_ssn = z.bf_ssn) AND
		(F.bd_aty_prf = (SELECT MAX(Y.bd_aty_prf)					/*<1>*/
						FROM OLWHRM1.AY01_BR_ATY Y
						WHERE   A.BF_SSN = Y.DF_PRS_ID AND
								Y.PF_ACT = 'DJGNM'
						GROUP BY Y.DF_PRS_ID) OR
		NOT EXISTS (SELECT *
					FROM OLWHRM1.AY01_BR_ATY Y
					WHERE   A.BF_SSN = Y.DF_PRS_ID AND
							Y.PF_ACT = 'DJGNM'))				   /*</1>*/
);

DISCONNECT FROM DB2;
ENDRSUBMIT;
DATA WORKLOCL.SATIS (KEEP = SSN FIRST LAST ADDRESS CITY ST ZIP DOCKNO JUDG JUDGDT SATDT);
	SET WORKLOCL.SATIS;
		SSN = df_prs_id_br;
		FIRST = TRIM(dm_prs_1);
		LAST = TRIM(dm_prs_lst);
		ADDRESS = TRIM(dx_str_adr_1)||TRIM(dx_str_adr_2);
		CITY = TRIM(dm_ct);
		ST = dc_dom_st;
		ZIP = TRIM(df_zip);
		DOCKNO = bf_leg_act_dkt;
		JUDG = SUBSTR(bx_cmt,1,INDEX(bx_cmt, ' '));  /*<1>*/
    	/*Court;County;Abstract*/
		JUDGDT = bd_jdg_ent;
		SATDT = ld_lst_pay;		
RUN;
PROC SORT DATA=WORKLOCL.SATIS;
BY SSN;
RUN;
/*<1>PROC PRINTTO PRINT=REPORT2;
RUN;*/
/*OPTIONS CENTER PAGENO=1 LS=132;
PROC PRINT NOOBS SPLIT='/' DATA=WORKLOCL.SATIS;
VAR SSN
	FIRST
	LAST
	ADDRESS
	CITY
	ST
	ZIP
	DOCKNO
	/*JUDG*/
/*	JUDGDT
	SATDT;
FORMAT JUDGDT MMDDYY10. SATDT MMDDYY10.;
TITLE 'SATISFACTION OF LEGAL ACTION';
RUN;
PROC SORT DATA=WORKLOCL.SATIS;
BY LAST FIRST;
RUN;
/*PROC PRINTTO PRINT=REPORT3;
RUN;*/
/*OPTIONS PAGENO=1 LS=80;
PROC PRINT NOOBS SPLIT='/' DATA=WORKLOCL.SATIS;
VAR	LAST
	FIRST
	SSN;
TITLE1  'FILE PULL LIST';
TITLE2	'SATISFIED LEGAL LOANS';
RUN;*/
DATA _NULL_;
	 CALL SYMPUT('RUNDT',PUT(DATE(),MMDDYY10.));
RUN;

data _null_;
set  WORKLOCL.SATIS;
file 'T:\SAS\SATIS.txt' delimiter=',' DSD DROPOVER lrecl=32767;
/*file '/sas/whse/olr_xtrnl_files/ULWPF2.LWPF2R2' delimiter=',' DSD DROPOVER lrecl=32767;*/

   format SSN $9. ;
 if _n_ = 1 then do;     /* write column names */
    put "&RUNDT";
  end;
  do;
    put SSN $ ;
    ;
  end;
 run;


/*<1>  sr 132, jd, 07/26/02, 09/06/02
