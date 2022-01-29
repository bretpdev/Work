RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE SATIS AS
SELECT 
	*
FROM 
	CONNECTION TO DB2 (
SELECT 
	INPUT DISTINCT
	RTRIM(D.DM_PRS_1)								AS FIRST,
	RTRIM(D.dm_prs_lst)								AS LAST,
	RTRIM(D.dx_str_adr_1)||RTRIM(D.dx_str_adr_2)	AS ADDRESS,
	RTRIM(D.dm_ct)									AS CITY,
	D.dc_dom_st										AS ST,
	RTRIM(D.df_zip)									AS ZIP,
	A.df_prs_id_br									AS SSN,
	A.bf_leg_act_dkt								AS DOCKNO,
	RTRIM(F.bx_cmt)									AS JUDG,
    /*Court;County;Abstract*/
	E.bd_jdg_ent									AS JUDGDT		/*judgment date*/

FROM 	OLWHRM1.LA10_LEG_ACT A INNER JOIN
		OLWHRM1.LA12_LEG_ACT_LON B ON
			DATE(A.bf_crt_dts_la10) = DATE(B.bf_crt_dts_la10) AND
			A.df_prs_id_br = B.df_prs_id_br INNER JOIN
		OLWHRM1.DC01_LON_CLM_INF C ON
			B.af_apl_id = C.af_apl_id AND
			B.af_apl_id_sfx = C.af_apl_id_sfx INNER JOIN
		OLWHRM1.PD01_PDM_INF D ON
			A.df_prs_id_br = D.df_prs_id INNER JOIN
		OLWHRM1.LA11_LEG_ACT_ATY E ON
			DATE(A.bf_crt_dts_la10) = DATE(E.bf_crt_dts_la10) AND
			A.df_prs_id_br = E.df_prs_id_br AND
			E.bc_leg_act_aty_typ = 'JD' INNER JOIN
		OLWHRM1.AY01_BR_ATY F ON
			A.df_prs_id_br = F.df_prs_id AND
			F.pf_act = 'D632W'              /*'DJGNM'*/
		
WHERE	A.bc_ina_rea = '' AND
		C.lc_sta_dc10 = '04' AND
		A.bc_wdr_rea = ''
);

CREATE TABLE SATDT AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	G.bf_ssn				AS SSN,
	MAX(G.bd_trx_pst_hst)	AS SATDT
FROM  OLWHRM1.DC11_LON_FAT G
WHERE DAYS(G.bd_trx_pst_hst) > DAYS(CURRENT DATE) - 180
GROUP BY G.bf_ssn
);

DISCONNECT FROM DB2;
ENDRSUBMIT;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
PROC SQL;
CREATE TABLE WORKLOCL.SATISJUDG AS
SELECT	A.FIRST,
		A.LAST,
		A.ADDRESS,
		A.CITY,
		A.ST,
		A.ZIP,
		A.SSN,
		A.DOCKNO,
		A.JUDG,
    	/*Court;County;Abstract*/
		A.JUDGDT,
		B.SATDT
FROM	WORKLOCL.SATIS A INNER JOIN
		WORKLOCL.SATDT B ON
			A.SSN = B.SSN;
PROC SORT DATA=WORKLOCL.SATISJUDG;
BY SSN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=WORKLOCL.SATISJUDG;
BY SSN;
VAR FIRST
	LAST
	ADDRESS
	CITY
	ST
	ZIP
	DOCKNO
	JUDG
	JUDGDT
	SATDT;
TITLE 'SATISFACTION OF LEGAL ACTION';
RUN;
