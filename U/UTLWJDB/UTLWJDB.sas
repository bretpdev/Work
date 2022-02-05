/*
LEGAL BORROWERS WITH A NON UT EMPLOYER ELIGIBLE FOR GARNISHMENT

This job creates a text file which lists the SSN of legal borrowers with a non UT 
employer who are not currently being garnished judicially or through AWG
and are eligible for garnishment.
Collections uses this file with a script to build queue tasks to review the accounts.

*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWJDB.LWJDBR2";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE NEWGARN AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.DF_PRS_ID AS SSN
FROM OLWHRM1.PD01_PDM_INF A 
	INNER JOIN OLWHRM1.IN01_LGS_IDM_MST B 
		ON A.BF_EMP_ID_1 = B.IF_IST 
		AND	A.BI_EMP_INF_OVR = 'Y' 
		AND	B.IC_GEN_ST <> 'UT' 
	INNER JOIN OLWHRM1.LA10_LEG_ACT C 
		ON A.DF_PRS_ID = C.DF_PRS_ID_BR 
		AND	C.BC_WDR_REA = '' 
		AND	C.BC_INA_REA = '' 
		AND	(C.bd_hld < current date 
			OR C.bd_hld IS NULL) 
		AND	C.BC_LEG_ACT_REC_TYP = '2'
	INNER JOIN OLWHRM1.LA12_LEG_ACT_LON C1 
		ON A.DF_PRS_ID = C1.df_prs_id_br 
		AND	C.bf_crt_dts_la10 = C1.bf_crt_dts_la10 
	INNER JOIN OLWHRM1.DC01_LON_CLM_INF D 
		ON C1.af_apl_id = D.af_apl_id 
		AND	C1.af_apl_id_sfx = D.af_apl_id_sfx 
		AND	D.lc_sta_dc10 = '03' 
		AND	D.ld_clm_asn_doe IS NULL 
		AND	D.LC_GRN NOT IN ('02', '04', '05', '07') 
		AND	(DAYS(D.LD_LST_PAY) < DAYS(CURRENT DATE) - 60 
			OR D.LD_LST_PAY IS NULL) /*CONSIDERATION FOR NULLS ADDED 02/06/04*/
		AND	D.lc_aux_sta = '' 
	INNER JOIN OLWHRM1.DC02_BAL_INT E 
		ON D.AF_APL_ID = E.AF_APL_ID 
		AND	D.AF_APL_ID_SFX = E.AF_APL_ID_SFX 
		AND	E.LA_CLM_BAL > 100 
/*<1->*/
	INNER JOIN OLWHRM1.BR01_BR_CRF H
		ON	A.DF_PRS_ID = H.BF_SSN
		AND (H.BD_HLD IS NULL OR DAYS(H.BD_HLD) <= DAYS(CURRENT DATE))
/*</1>*/
	LEFT OUTER JOIN	OLWHRM1.AY01_BR_ATY F 
		ON A.DF_PRS_ID = F.DF_PRS_ID 
		AND	F.PF_ACT = 'DJGNM'
WHERE	C.BF_CRT_DTS_LA10 = 
	(SELECT MIN(X.BF_CRT_DTS_LA10)
	FROM OLWHRM1.LA10_LEG_ACT X
	WHERE	A.DF_PRS_ID = X.DF_PRS_ID_BR 
	AND X.BC_WDR_REA = '' 
	AND X.BC_INA_REA = '' 
	AND	(X.bd_hld < current date 
		OR X.bd_hld IS NULL) 
	AND	X.BC_LEG_ACT_REC_TYP = '2'
	GROUP BY X.DF_PRS_ID_BR
	) 
AND (F.bd_aty_prf = 
		(SELECT MAX(Y.bd_aty_prf)
		FROM OLWHRM1.AY01_BR_ATY Y
		WHERE A.DF_PRS_ID = Y.DF_PRS_ID 
		AND	Y.PF_ACT = 'DJGNM'
		GROUP BY Y.DF_PRS_ID
		) 
	OR NOT EXISTS 
		(SELECT *
		FROM OLWHRM1.AY01_BR_ATY Y
		WHERE A.DF_PRS_ID = Y.DF_PRS_ID
		AND	Y.PF_ACT = 'DJGNM'
		)
	)	
AND	NOT EXISTS 
	(SELECT *
	FROM OLWHRM1.LA11_LEG_ACT_ATY U 
	WHERE A.DF_PRS_ID = U.df_prs_id_br 
	AND	C.bf_crt_dts_la10 = U.bf_crt_dts_la10 
	AND	U.bc_leg_act_aty_typ = 'EX' 
	AND	U.bc_exe_typ IN ('04', '05'))
AND	NOT EXISTS 
	(SELECT *
	FROM OLWHRM1.LA10_LEG_ACT T
	WHERE A.DF_PRS_ID = T.DF_PRS_ID_BR 
	AND	T.BC_WDR_REA = '' 
	AND	T.BC_INA_REA = '' 
	AND	T.bc_hld_rea = '' 
	AND	T.BC_LEG_ACT_REC_TYP = '1')
GROUP BY A.DF_PRS_ID, RTRIM(A.DM_PRS_1), RTRIM(A.DM_PRS_LST),A.BF_EMP_ID_1, B.IM_IST_FUL, 
	RTRIM(B.IX_GEN_STR_ADR_1)||' '||RTRIM(B.IX_GEN_STR_ADR_2)||' '||RTRIM(B.IX_GEN_STR_ADR_3),
	RTRIM(B.IM_GEN_CT), B.IC_GEN_ST, RTRIM(B.IF_GEN_ZIP), SUBSTR(B.IF_GEN_ZIP,1,5),
	C.BF_LEG_ACT_DKT, F.BX_CMT
ORDER BY A.DF_PRS_ID
);
DISCONNECT FROM DB2;

ENDRSUBMIT;
DATA NEWGARN;
SET WORKLOCL.NEWGARN;
RUN;

data _null_;
set  NEWGARN                             end=EFIEOD;
%let _EFIERR_ = 0; /* set the ERROR detection macro variable */
%let _EFIREC_ = 0;     /* clear export record count macro variable */
file 'T:\SAS\NEWGARN.txt' delimiter=',' DSD DROPOVER lrecl=32767;
*file REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;
   format SSN $9. ;
if _n_ = 1 then        /* write column names */
 do;
   put
   'SSN'
   ;
 end;
 do;
   EFIOUT + 1;
   put SSN $ ;
   ;
 end;
if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */
If EFIEOD then
   call symput('_EFIREC_',EFIOUT);
run;

/*
<1> sr 350, jd, 08/21/03

*/
