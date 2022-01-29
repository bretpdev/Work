
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE NOPRECLAIM AS
		SELECT
			*
		FROM
			CONNECTION TO DB2
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID,
						LN10.BF_SSN,
						LN10.LN_SEQ,
						GA14.AC_LON_STA_TYP,
						DW01.WC_DW_LON_STA,
						CL10.LN_SEQ_CLM_PCL
					FROM
						OLWHRM1.PD10_PRS_NME PD10
						JOIN OLWHRM1.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
							AND LN10.LC_STP_PUR NOT IN ('P','Y')
							AND LN10.LI_CON_PAY_STP_PUR ^= 'Y'
						JOIN OLWHRM1.LN16_LON_DLQ_HST LN16
							ON LN10.BF_SSN = LN16.BF_SSN
							AND LN10.LN_SEQ = LN16.LN_SEQ
							AND LN10.LA_CUR_PRI > 0
							AND LN10.LC_STA_LON10 = 'R'
							AND LN16.LC_STA_LON16 = '1'
							AND LN16.LN_DLQ_MAX > 60
						JOIN OLWHRM1.GA14_LON_STA GA14
							ON LN10.LF_LON_ALT = GA14.AF_APL_ID
							AND LN10.LN_LON_ALT_SEQ = GA14.AF_APL_ID_SFX
							AND GA14.AC_STA_GA14 = 'A'
							AND GA14.AC_LON_STA_TYP NOT IN ('UI','CP','PC','PF','PM','PN')
							AND GA14.AC_LON_STA_REA NOT IN ('DU','BC','BO','DE','DI','FC')
						JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
							AND DW01.WC_DW_LON_STA NOT IN ('06','12','20','21','16','17','18','19','22')
						LEFT JOIN OLWHRM1.DC01_LON_CLM_INF DC01
							ON GA14.AF_APL_ID = DC01.AF_APL_ID
							AND GA14.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
							AND DC01.LC_STA_DC10 IN ('01','03')
						LEFT JOIN OLWHRM1.CL10_CLM_PCL CL10
							ON LN10.BF_SSN = CL10.BF_SSN
					WHERE
						DC01.AF_APL_ID IS NULL
						AND NOT 
							(
								GA14.AC_LON_STA_TYP = 'CR' 
								AND DW01.WC_DW_LON_STA IN ('07','08','13','14')
							)
					ORDER BY
						DF_SPE_ACC_ID,
						LN_SEQ

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;

ENDRSUBMIT;

PROC EXPORT
		DATA = DUSTER.NOPRECLAIM
		OUTFILE = "T:\SAS\NHUH 16929.XLS"
		DBMS = EXCEL
		REPLACE;
RUN;

/*only needed one time*/
/*PROC IMPORT OUT= WORK.JESSE*/
/*            DATAFILE= "Q:\Account Services\JESSE's\COM\NH 16929\NH 169291.XLS" */
/*            DBMS=EXCEL REPLACE;*/
/*     RANGE="NOPRECLAIM"; */
/*     GETNAMES=YES;*/
/*     MIXED=NO;*/
/*     SCANTEXT=YES;*/
/*     USEDATE=YES;*/
/*     SCANTIME=YES;*/
/*RUN;*/
/**/
/*PROC SQL;*/
/*	CREATE TABLE NH16929REDUX AS*/
/*		SELECT DISTINCT*/
/*			A.**/
/*		FROM*/
/*			DUSTER.NOPRECLAIM A*/
/*			JOIN JESSE B*/
/*				ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID*/
/*				AND A.LN_SEQ = B.LN_SEQ*/
/*	;*/
/*QUIT;*/
/**/
/*PROC EXPORT*/
/*		DATA = NH16929REDUX*/
/*		OUTFILE = "T:\SAS\NH 16929-AUG20.XLS"*/
/*		DBMS = EXCEL*/
/*		REPLACE;*/
/*RUN;*/