/*
BORROWERS ELIGIBLE FOR JUDICIAL GARNISHMENT

This report lists legal borrowers with a current employer who are not currently
being garnished judicially and are eligible for garnishment.
Collections uses this report to initiate new judicial garnishments.

*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWJD1.LWJD1R2";
FILENAME REPORT3 "&RPTLIB/ULWJD1.LWJD1R3";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE NEWGARN AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.DF_PRS_ID											AS SSN,
	RTRIM(A.DM_PRS_1)									AS FIRST,
	RTRIM(A.DM_PRS_LST)									AS LAST,
	RTRIM(A.DX_STR_ADR_1)||' '||RTRIM(A.DX_STR_ADR_2)	AS ADDRESS,
	RTRIM(A.DM_CT)										AS CITY,
	RTRIM(A.DC_DOM_ST)									AS ST,
	RTRIM(A.DF_ZIP)										AS ZIP,
	A.BF_EMP_ID_1										AS EMP_ID,
	B.IM_IST_FUL										AS EMP_NAME,
	RTRIM(B.IX_GEN_STR_ADR_1)||' '||RTRIM(B.IX_GEN_STR_ADR_2)||' '||RTRIM(B.IX_GEN_STR_ADR_3)	AS EMP_ADD,
	RTRIM(B.IM_GEN_CT)									AS EMP_CITY,
	B.IC_GEN_ST											AS EMP_ST,
	RTRIM(B.IF_GEN_ZIP)									AS EMP_ZIP,
	SUBSTR(B.IF_GEN_ZIP,1,5)							AS ZIP5,
	B.IN_GEN_PHN										AS EMP_PHONE,
	C.BF_LEG_ACT_DKT									AS DOCKNO,
	SUM(E.LA_CLM_BAL)									AS BAL,
	F.BX_CMT											AS JUDG
/*FROM 	OLWHRM1.PD01_PDM_INF A INNER JOIN					<1>
		OLWHRM1.IN01_LGS_IDM_MST B ON
			A.BF_EMP_ID_1 = B.IF_IST AND
			A.BI_EMP_INF_OVR = 'Y' AND
			B.IC_GEN_ST = 'UT' INNER JOIN
		OLWHRM1.LA10_LEG_ACT C ON
			A.DF_PRS_ID = C.DF_PRS_ID_BR AND
			/*C.BC_WDR_REA = '' AND
			C.BC_INA_REA = '' AND*//*
			C.BC_LEG_ACT_REC_TYP = '2'INNER JOIN
		OLWHRM1.LA12_LEG_ACT_LON C1 ON
			A.DF_PRS_ID = C1.df_prs_id_br AND
			C.bf_crt_dts_la10 = C1.bf_crt_dts_la10 INNER JOIN
		OLWHRM1.DC01_LON_CLM_INF D ON
			C1.af_apl_id = D.af_apl_id AND
			C1.af_apl_id_sfx = D.af_apl_id_sfx AND
			D.lc_sta_dc10 = '03' AND
			D.lc_rea_clm_asn_doe = '' AND
			D.LC_GRN <> '07' AND
			DAYS(D.LD_LST_PAY) < DAYS(CURRENT DATE) - 60 AND
			D.lc_aux_sta = '' INNER JOIN 
		OLWHRM1.DC02_BAL_INT E ON
			D.AF_APL_ID = E.AF_APL_ID AND
			D.AF_APL_ID_SFX = E.AF_APL_ID_SFX AND
			E.LA_CLM_BAL > 100 FULL OUTER JOIN
		OLWHRM1.AY01_BR_ATY F ON
			A.DF_PRS_ID = F.DF_PRS_ID AND
			F.PF_ACT = 'DJGNM'
WHERE	C.BF_CRT_DTS_LA10 = (SELECT MIN(X.BF_CRT_DTS_LA10)
 							 FROM OLWHRM1.LA10_LEG_ACT X
							 WHERE	A.DF_PRS_ID = X.DF_PRS_ID_BR AND
											/*X.BC_WDR_REA = '' AND
											X.BC_INA_REA = '' AND*//*
											X.BC_LEG_ACT_REC_TYP = '2'
							 GROUP BY X.DF_PRS_ID_BR) AND (
		F.bd_aty_prf = (SELECT MAX(Y.bd_aty_prf)
						FROM OLWHRM1.AY01_BR_ATY Y
						WHERE   A.DF_PRS_ID = Y.DF_PRS_ID AND
								Y.PF_ACT = 'DJGNM'
						GROUP BY Y.DF_PRS_ID) OR
		NOT EXISTS (SELECT *
					FROM OLWHRM1.AY01_BR_ATY Y
					WHERE   A.DF_PRS_ID = Y.DF_PRS_ID AND
							Y.PF_ACT = 'DJGNM'))			</1>  */
FROM 	OLWHRM1.PD01_PDM_INF A INNER JOIN
		OLWHRM1.IN01_LGS_IDM_MST B ON
			A.BF_EMP_ID_1 = B.IF_IST AND
			A.BI_EMP_INF_OVR = 'Y' AND
			B.IC_GEN_ST = 'UT' INNER JOIN
		OLWHRM1.LA10_LEG_ACT C ON
			A.DF_PRS_ID = C.DF_PRS_ID_BR AND
			C.BC_WDR_REA = '' AND
			C.BC_INA_REA = '' AND
			(C.bd_hld < current date OR
			 C.bd_hld IS NULL) AND
			C.BC_LEG_ACT_REC_TYP = '2'INNER JOIN
		OLWHRM1.LA12_LEG_ACT_LON C1 ON
			A.DF_PRS_ID = C1.df_prs_id_br AND
			C.bf_crt_dts_la10 = C1.bf_crt_dts_la10 INNER JOIN 
		OLWHRM1.DC01_LON_CLM_INF D ON
			C1.af_apl_id = D.af_apl_id AND
			C1.af_apl_id_sfx = D.af_apl_id_sfx AND
			D.lc_sta_dc10 = '03' AND
			D.ld_clm_asn_doe IS NULL AND
			D.LC_GRN IN ('06', '07') AND
			(DAYS(D.LD_LST_PAY) < DAYS(CURRENT DATE) - 60 
			 OR D.LD_LST_PAY IS NULL) AND /*CONSIDERATION FOR NULL ADDED 02/05/04*/
			D.lc_aux_sta = '' /*<5->*/ INNER JOIN
		OLWHRM1.BR01_BR_CRF H ON
			A.DF_PRS_ID = H.BF_SSN AND
			(H.BD_HLD IS NULL OR DAYS(H.BD_HLD) <= DAYS(CURRENT DATE)) /*</5>*/INNER JOIN 
		OLWHRM1.DC02_BAL_INT E ON
			D.AF_APL_ID = E.AF_APL_ID AND
			D.AF_APL_ID_SFX = E.AF_APL_ID_SFX AND
			E.LA_CLM_BAL > 100 LEFT OUTER JOIN
		OLWHRM1.AY01_BR_ATY F ON
			A.DF_PRS_ID = F.DF_PRS_ID AND
			F.PF_ACT = 'DJGNM'
WHERE	C.BF_CRT_DTS_LA10 = 
	(SELECT MIN(X.BF_CRT_DTS_LA10)
	FROM OLWHRM1.LA10_LEG_ACT X
	WHERE	A.DF_PRS_ID = X.DF_PRS_ID_BR 
	AND	X.BC_WDR_REA = '' 
	AND	X.BC_INA_REA = '' 
	AND	(X.bd_hld < current date 
		OR X.bd_hld IS NULL) 
	AND	X.BC_LEG_ACT_REC_TYP = '2'
	GROUP BY X.DF_PRS_ID_BR) 
AND (F.bd_aty_prf = 
		(SELECT MAX(Y.bd_aty_prf)
		FROM OLWHRM1.AY01_BR_ATY Y
		WHERE A.DF_PRS_ID = Y.DF_PRS_ID 
		AND Y.PF_ACT = 'DJGNM'
		GROUP BY Y.DF_PRS_ID) 
	OR
	NOT EXISTS 
		(SELECT *
		FROM OLWHRM1.AY01_BR_ATY Y
		WHERE A.DF_PRS_ID = Y.DF_PRS_ID 
		AND Y.PF_ACT = 'DJGNM')
	)	
	AND NOT EXISTS 
		(SELECT *
		FROM OLWHRM1.LA11_LEG_ACT_ATY U 
		WHERE A.DF_PRS_ID = U.df_prs_id_br 
		AND C.bf_crt_dts_la10 = U.bf_crt_dts_la10 
		AND U.bc_leg_act_aty_typ = 'EX' 
		AND U.bc_exe_typ IN ('04', '05')
		)
GROUP BY A.DF_PRS_ID, RTRIM(A.DM_PRS_1),	RTRIM(A.DM_PRS_LST),
	RTRIM(A.DX_STR_ADR_1)||' '||RTRIM(A.DX_STR_ADR_2),
	RTRIM(A.DM_CT), RTRIM(A.DC_DOM_ST), RTRIM(A.DF_ZIP), A.BF_EMP_ID_1, B.IM_IST_FUL,
	RTRIM(B.IX_GEN_STR_ADR_1)||' '||RTRIM(B.IX_GEN_STR_ADR_2)||' '||RTRIM(B.IX_GEN_STR_ADR_3),
	RTRIM(B.IM_GEN_CT), B.IC_GEN_ST, RTRIM(B.IF_GEN_ZIP), SUBSTR(B.IF_GEN_ZIP,1,5),
	B.IN_GEN_PHN, C.BF_LEG_ACT_DKT, F.BX_CMT
ORDER BY A.DF_PRS_ID
);
DISCONNECT FROM DB2;
ENDRSUBMIT;
/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS PAGENO=1 LS=132;
PROC PRINT SPLIT='/' DATA=WORKLOCL.NEWGARN WIDTH=MIN;
WHERE	ZIP5 IN ('84017', '84020', '84024', '84033', '84036', '84044', '84047', '84055',
				 '84060', '84061', '84065', '84070', '84084', '84088', '84092', '84093',
				 '84094', '84095', '84098', '84101', '84102', '84103', '84104', '84105',
				 '84106', '84107', '84108', '84109', '84111', '84112', '84113', '84114', 
				 '84115', '84116', '84117', '84118', '84119', '84120', '84121', '84123', 
				 '84124', '84125', '84128', '84133', '84157',
			'84006','84090','84091','84110','84122','84126','84127','84130','84131',
			'84132','84141','84142','84143','84144','84145','84147','84148','84150',
			'84134','84135','84136','84139','84140','84151','84152','84153','84158',
			'84165','84170','84171','84180','84184','84189','84190','84193','84194',
			'84195','84199'
); /*<2>*//*<3>*//*<4>*/
VAR SSN
	FIRST
	LAST
	ADDRESS
	CITY
	ST
	ZIP
	EMP_ID
	EMP_NAME
	EMP_ADD
	EMP_CITY
	EMP_ST
	EMP_ZIP
	EMP_PHONE
	DOCKNO
	JUDG
	BAL;
TITLE1 'BORROWERS ELIGIBLE FOR JUDICAL GARNISHMENT';
TITLE2 'NO CURRENT GARNISHMENT';
TITLE3 'SERVE BY CONSTABLE';
FOOTNOTE 'JOB = UTLWJD1     REPORT = ULWJD1.LWJD1R2';
RUN;
/*PROC PRINTTO PRINT=REPORT3;
RUN;*/
OPTIONS PAGENO=1 LS=132;
PROC PRINT SPLIT='/' DATA=WORKLOCL.NEWGARN WIDTH=MIN;
WHERE	ZIP5 NOT IN ('84017', '84020', '84024', '84033', '84036', '84044', '84047', '84055',
				 	 '84060', '84061', '84065', '84070', '84084', '84088', '84092', '84093',
				 	 '84094', '84095', '84098', '84101', '84102', '84103', '84104', '84105',
				 	 '84106', '84107', '84108', '84109', '84111', '84112', '84113', '84114', 
				 	 '84115', '84116', '84117', '84118', '84119', '84120', '84121', '84123', 
				 	 '84124', '84125', '84128', '84133', '84157',
			'84006','84090','84091','84110','84122','84126','84127','84130','84131',
			'84132','84141','84142','84143','84144','84145','84147','84148','84150',
			'84134','84135','84136','84139','84140','84151','84152','84153','84158',
			'84165','84170','84171','84180','84184','84189','84190','84193','84194',
			'84195','84199'
); /*<2>*//*<3>*//*<4>*/
VAR SSN
	FIRST
	LAST
	ADDRESS
	CITY
	ST
	ZIP
	EMP_ID
	EMP_NAME
	EMP_ADD
	EMP_CITY
	EMP_ST
	EMP_ZIP
	EMP_PHONE
	DOCKNO
	JUDG
	BAL;
TITLE1 'BORROWERS ELIGIBLE FOR JUDICAL GARNISHMENT';
TITLE2 'NO CURRENT GARNISHMENT';
TITLE3 'SERVE BY SHERIFF';
FOOTNOTE 'JOB = UTLWJD1     REPORT = ULWJD1.LWJD1R3';
RUN;

/* <1> sr82, jd, 5/1/02
   <2> sr106, jd, 6/25/02, added 84125 and 84157
   <3> sr 138, jd, 8/1/02, added 84133
   <4> sr180, mc, 10/29/02, added several
   <5> sr352, jd, 08/21/03

   */