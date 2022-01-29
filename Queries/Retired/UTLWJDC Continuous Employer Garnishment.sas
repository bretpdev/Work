/*
JUDICIAL GARNISHMENT WILL EXPIRE (continuous garnishment)

This report lists legal borrowers with a current employer who are currently
being garnished judicially where the garnishment is going to expire and writes the
information to a text file for a script which produces the garnishment documents.

*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORT2 "&RPTLIB/ULWJDC.LWJDCR2";*/

FILENAME REPORT2 "C:\Windows\Temp\ULWJDC.LWJDCR2.test.txt";

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE RNEWGARN AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.DF_PRS_ID													AS SSN,
	RTRIM(A.DM_PRS_1)||' '||RTRIM(A.DM_PRS_LST)					AS Borrower,
	SUM(E.LA_CLM_BAL)											AS Balance,
	C.bd_leg_act_fil											AS PADF,
	C.BF_LEG_ACT_DKT											AS DockNo,
	G.BX_CMT													AS Judge,
	B.IM_IST_FUL												AS Garnishee,
	A.BF_EMP_ID_1												AS EmpID,
	RTRIM(B.IX_GEN_STR_ADR_1)									AS Address1,
	RTRIM(B.IX_GEN_STR_ADR_2)									AS Address2,
	RTRIM(B.IX_GEN_STR_ADR_3)									AS Address3,
	CASE
		WHEN SUBSTR(B.IF_GEN_ZIP,6,4) = '    '
		THEN RTRIM(B.IM_GEN_CT)||' '||B.IC_GEN_ST||'  '||RTRIM(B.IF_GEN_ZIP)
		ELSE RTRIM(B.IM_GEN_CT)||' '||B.IC_GEN_ST||'  '||SUBSTR(B.IF_GEN_ZIP,1,5)
		||'-'||SUBSTR(B.IF_GEN_ZIP,6,4)
	END															AS City,
	CASE
		WHEN SUBSTR(B.IF_GEN_ZIP,1,5) IN 	
			('84017', '84020', '84024', '84033', '84036', '84044', '84047', '84055',
			 '84060', '84061', '84065', '84070', '84084', '84088', '84092', '84093',
			 '84094', '84095', '84098', '84101', '84102', '84103', '84104', '84105',
			 '84106', '84107', '84108', '84109', '84111', '84112', '84113', '84114', 
			 '84115', '84116', '84117', '84118', '84119', '84120', '84121', '84123', 
			 '84124', '84125', '84128', '84133', '84157',
			'84006','84090','84091','84110','84122','84126','84127','84130','84131',
			'84132','84141','84142','84143','84144','84145','84147','84148','84150',
			'84134','84135','84136','84139','84140','84151','84152','84153','84158',
			'84165','84170','84171','84180','84184','84189','84190','84193','84194',
			'84195','84199')
		THEN 'constable'
		ELSE 'sheriff'
	END															AS Server,
	F.BD_LEG_ACT_ATY_FIL										AS Filed,
	SUBSTR(I.BX_CMT,1,8) 										AS Servd,
	RTRIM(SUBSTR(I.BX_CMT,9,8))									AS EmpSrvd,
	SUBSTR(I.BX_CMT,17,2) 										AS WOG
FROM 	OLWHRM1.PD01_PDM_INF A INNER JOIN
		OLWHRM1.IN01_LGS_IDM_MST B ON
			A.BF_EMP_ID_1 = B.IF_IST AND
			A.BI_EMP_INF_OVR = 'Y' INNER JOIN
		OLWHRM1.LA10_LEG_ACT C ON
			A.DF_PRS_ID = C.DF_PRS_ID_BR AND
			C.BC_WDR_REA = '' AND
			C.BC_INA_REA = '' AND
			(C.bd_hld < current date OR
			 C.bd_hld IS NULL) AND
			C.BC_LEG_ACT_REC_TYP = '2' INNER JOIN
		OLWHRM1.DC01_LON_CLM_INF D ON
			A.DF_PRS_ID = D.BF_SSN AND 
			D.LC_GRN = '07' INNER JOIN
		OLWHRM1.DC02_BAL_INT E ON
			D.AF_APL_ID = E.AF_APL_ID AND
			D.AF_APL_ID_SFX = E.AF_APL_ID_SFX AND
			E.LA_CLM_BAL > 100 INNER JOIN
		OLWHRM1.LA11_LEG_ACT_ATY F ON
			A.DF_PRS_ID = F.DF_PRS_ID_BR AND
			C.BF_CRT_DTS_LA10 = F.BF_CRT_DTS_LA10 AND
			F.BC_EXE_TYP = '04' AND
			DAYS(F.BD_LEG_ACT_ATY_FIL) < DAYS(CURRENT DATE)-90 LEFT OUTER JOIN
		OLWHRM1.AY01_BR_ATY G ON
			A.DF_PRS_ID = G.DF_PRS_ID AND
			G.PF_ACT = 'DJGNM' INNER JOIN
		OLWHRM1.BR01_BR_CRF H ON
			A.DF_PRS_ID = H.BF_SSN AND
			(H.BD_HLD IS NULL OR DAYS(H.BD_HLD) <= DAYS(CURRENT DATE)) LEFT OUTER JOIN
		OLWHRM1.AY01_BR_ATY I ON
			A.DF_PRS_ID = I.DF_PRS_ID AND
			I.PF_ACT = 'LRTRN'

WHERE	C.BF_CRT_DTS_LA10 = (SELECT MIN(X.BF_CRT_DTS_LA10)
							 FROM OLWHRM1.LA10_LEG_ACT X
							 WHERE	A.DF_PRS_ID = X.DF_PRS_ID_BR AND
									X.BC_WDR_REA = '' AND
									X.BC_INA_REA = '' AND
									X.BC_LEG_ACT_REC_TYP = '2'
							 GROUP BY X.DF_PRS_ID_BR)
AND (G.bd_aty_prf = 
		(SELECT MAX(Y.bd_aty_prf)
		FROM OLWHRM1.AY01_BR_ATY Y
		WHERE   A.DF_PRS_ID = Y.DF_PRS_ID AND
				Y.PF_ACT = 'DJGNM'
		GROUP BY Y.DF_PRS_ID
		) 
	OR NOT EXISTS 
		(SELECT *
		FROM OLWHRM1.AY01_BR_ATY Y
		WHERE A.DF_PRS_ID = Y.DF_PRS_ID
		AND Y.PF_ACT = 'DJGNM'
		)
	)	
AND (I.bd_aty_prf = 
		(SELECT MAX(Y.bd_aty_prf)
		FROM OLWHRM1.AY01_BR_ATY Y
		WHERE   A.DF_PRS_ID = Y.DF_PRS_ID AND
				Y.PF_ACT = 'LRTRN'
		GROUP BY Y.DF_PRS_ID
		) 
	OR NOT EXISTS 
		(SELECT *
		FROM OLWHRM1.AY01_BR_ATY Y
		WHERE A.DF_PRS_ID = Y.DF_PRS_ID
		AND Y.PF_ACT = 'DJGNM'
		)
	)	
 
GROUP BY	A.DF_PRS_ID, RTRIM(A.DM_PRS_1)||' '||RTRIM(A.DM_PRS_LST), C.BF_LEG_ACT_DKT, C.bd_leg_act_fil,
			G.BX_CMT, B.IM_IST_FUL, A.BF_EMP_ID_1, F.BD_LEG_ACT_ATY_FIL,
			RTRIM(B.IX_GEN_STR_ADR_1), RTRIM(B.IX_GEN_STR_ADR_2), RTRIM(B.IX_GEN_STR_ADR_3),
			RTRIM(B.IM_GEN_CT)||' '||B.IC_GEN_ST||'  '||RTRIM(B.IF_GEN_ZIP),
			RTRIM(B.IM_GEN_CT)||' '||B.IC_GEN_ST||'  '||SUBSTR(B.IF_GEN_ZIP,1,5)||'-'||SUBSTR(B.IF_GEN_ZIP,6,4),
			SUBSTR(B.IF_GEN_ZIP,6,4), SUBSTR(B.IF_GEN_ZIP,1,5), 
			 SUBSTR(I.BX_CMT,1,8), RTRIM(SUBSTR(I.BX_CMT,9,8)), SUBSTR(I.BX_CMT,17,2)

ORDER BY A.DF_PRS_ID
);

DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA RNEWGARN;
SET WORKLOCL.RNEWGARN;
RUN;
data RNEWGARN;
set RNEWGARN;
IF EmpID = EmpSrvd THEN DO;
	Served = input(Servd, MMDDYY10.);
	WOG = WOG + 1;
	END;
ELSE DO;
	Served = Filed;
	WOG = 1;
	END;
run;

data RNEWGARN;
set RNEWGARN;
where Served < TODAY()-90 OR (Served = . AND Filed < TODAY()-90);
IF WOG = . THEN WOG = 1;
Judge = SUBSTR(Judge,1,INDEX(Judge, ' '));  /*<2>*/
format PADF MMDDYY10.;
run;

DATA _NULL_;
	 RUNDATE = PUT(DATE(),MMDDYY10.);
	 CALL SYMPUT('RUNDT',RUNDATE);
RUN;

data _null_;
set  RNEWGARN                                 end=EFIEOD;
%let _EFIERR_ = 0; /* set the ERROR detection macro variable */
%let _EFIREC_ = 0;     /* clear export record count macro variable */
file REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;
   format SSN $9. ;
   format BORROWER $48. ;
   format BALANCE best12. ;
   format PADF MMDDYY10. ;
   format DOCKNO $15. ;
   format JUDGE $600. ;
   format GARNISHEE $40. ;
   format ADDRESS1 $40. ;
   format ADDRESS2 $40. ;
   format ADDRESS3 $40. ;
   format CITY $49. ;
   format SERVER $9. ;
*   format WOG best12.;
if _n_ = 1 then        /* write column names */
 do;
   put
   "&RUNDT"
   ','
   'BORROWER'
   ','
   'BALANCE'
   ','
   'PADF'
   ','
   'DOCKNO'
   ','
   'JUDGE'
   ','
   'GARNISHEE'
   ','
   'ADDRESS1'
   ','
   'ADDRESS2'
   ','
   'ADDRESS3'
   ','
   'CITY'
   ','
   'SERVER'
   ','
   'WOG'
   
   ;
 end;
 do;
   EFIOUT + 1;
   put SSN $ @;
   put BORROWER $ @;
   put BALANCE @;
   put PADF @;
   put DOCKNO $ @;
   put JUDGE $ @;
   put GARNISHEE $ @;
   put ADDRESS1 $ @;
   put ADDRESS2 $ @;
   put ADDRESS3 $ @;
   put CITY $ @;
   put SERVER $ @;
   put WOG ;
   ;
 end;
if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */
If EFIEOD then
   call symput('_EFIREC_',EFIOUT);
run;