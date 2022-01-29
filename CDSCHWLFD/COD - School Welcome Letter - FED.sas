LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
 
RSUBMIT;
LIBNAME PKUB DB2 DATABASE=DNFPUTDL OWNER=PKUB;
LIBNAME PROGREVW V8 '/sas/whse/progrevw';

%LET WEEKOFFSET = -1;

data _null_  ;
   begin=intnx('week',today(),&WEEKOFFSET,'beginning')  ;
   call symput  ( 'begin', "'"||put(begin,DATE10.)|| "'D")   ;

   end=intnx('week',today(),&WEEKOFFSET,'end')  ;
   call symput  ( 'end', "'"||put(end,DATE10.)|| "'D")  ;
run;

%put &begin;
%put &end;

PROC SQL;
	CREATE TABLE LOANS AS
		SELECT DISTINCT
			LN10.BF_SSN,
			LN10.LN_SEQ,
			LN10.LD_LON_ACL_ADD,
			SC10.IM_SCL_FUL,
			SC10.IF_DOE_SCL,
			DPT4.IC_SCL_DPT AS DPT4_IC_SCL_DPT,
			DPT4.IX_SCL_STR_ADR_1 AS DPT4_IX_SCL_STR_ADR_1,
			DPT4.IX_SCL_STR_ADR_2 AS DPT4_IX_SCL_STR_ADR_2,
			DPT4.IX_SCL_STR_ADR_3 AS DPT4_IX_SCL_STR_ADR_3,
			DPT4.IM_SCL_CT AS DPT4_IM_SCL_CT,
			DPT4.IC_SCL_DOM_ST AS DPT4_IC_SCL_DOM_ST,
			DPT4.IF_SCL_ZIP_CDE AS DPT4_IF_SCL_ZIP_CDE,
			DPT0.IC_SCL_DPT AS DPT0_IC_SCL_DPT,
			DPT0.IX_SCL_STR_ADR_1 AS DPT0_IX_SCL_STR_ADR_1,
			DPT0.IX_SCL_STR_ADR_2 AS DPT0_IX_SCL_STR_ADR_2,
			DPT0.IX_SCL_STR_ADR_3 AS DPT0_IX_SCL_STR_ADR_3,
			DPT0.IM_SCL_CT AS DPT0_IM_SCL_CT,
			DPT0.IC_SCL_DOM_ST AS DPT0_IC_SCL_DOM_ST,
			DPT0.IF_SCL_ZIP_CDE AS DPT0_IF_SCL_ZIP_CDE
		FROM
			PKUB.LN10_LON LN10
			JOIN PKUB.DW01_DW_CLC_CLU DW01
				 ON LN10.BF_SSN = DW01.BF_SSN
				 AND LN10.LN_SEQ = DW01.LN_SEQ
			LEFT JOIN
				(
					SELECT
						LN50.BF_SSN,
						LN50.LN_SEQ,
						DF10.LC_DFR_TYP
					FROM
						PKUB.DF10_BR_DFR_REQ DF10
						JOIN PKUB.LN50_BR_DFR_APV LN50
							ON DF10.BF_SSN = LN50.BF_SSN
							AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
					WHERE
						LN50.LC_STA_LON50 = 'A'
						AND DF10.LC_DFR_STA = 'A'
						AND DF10.LC_STA_DFR10 = 'A'
						AND TODAY() BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
				) DF10
				ON LN10.BF_SSN = DF10.BF_SSN
				AND LN10.LN_SEQ = DF10.LN_SEQ
			JOIN PKUB.SC10_SCH_DMO SC10
				ON LN10.LF_DOE_SCL_ORG = SC10.IF_DOE_SCL
			LEFT JOIN PKUB.SC25_SCH_DPT DPT4
				ON SC10.IF_DOE_SCL = DPT4.IF_DOE_SCL
				AND DPT4.IC_SCL_DPT = '004'
			LEFT JOIN PKUB.SC25_SCH_DPT DPT0
				ON SC10.IF_DOE_SCL = DPT0.IF_DOE_SCL
				AND DPT0.IC_SCL_DPT = '000'
		WHERE
			LN10.LD_LON_ACL_ADD BETWEEN &begin AND &end
			AND (DW01.WC_DW_LON_STA IN ('02','23') OR (DW01.WC_DW_LON_STA = '04' AND DF10.LC_DFR_TYP = '45'))
	;
QUIT;

PROC SQL;
	CREATE TABLE SCHLS AS
		SELECT DISTINCT
			MIN(LD_LON_ACL_ADD) AS LD_LON_ACL_ADD LABEL='Loan Add Date' FORMAT DATE10.,
			IM_SCL_FUL LABEL='School Name',
			IF_DOE_SCL LABEL='School Code',
			COALESCE(DPT4_IC_SCL_DPT,DPT0_IC_SCL_DPT) AS IC_SCL_DPT LABEL='School Department',
			COALESCE(DPT4_IX_SCL_STR_ADR_1,DPT0_IX_SCL_STR_ADR_1) AS IX_SCL_STR_ADR_1 LABEL='School Address 1',
			COALESCE(DPT4_IX_SCL_STR_ADR_2,DPT0_IX_SCL_STR_ADR_2) AS IX_SCL_STR_ADR_2 LABEL='School Address 2',
			COALESCE(DPT4_IX_SCL_STR_ADR_3,DPT0_IX_SCL_STR_ADR_3) AS IX_SCL_STR_ADR_3 LABEL='School Address 3',
			COALESCE(DPT4_IM_SCL_CT,DPT0_IM_SCL_CT) AS IM_SCL_CT LABEL='City',
			COALESCE(DPT4_IC_SCL_DOM_ST,DPT0_IC_SCL_DOM_ST) AS IC_SCL_DOM_ST LABEL='State',
			COALESCE(DPT4_IF_SCL_ZIP_CDE,DPT0_IF_SCL_ZIP_CDE) AS IF_SCL_ZIP_CDE LABEL='Zip Code'
		FROM
			LOANS
		GROUP BY
			IM_SCL_FUL,
			IF_DOE_SCL,
			COALESCE(DPT4_IC_SCL_DPT,DPT0_IC_SCL_DPT),
			COALESCE(DPT4_IX_SCL_STR_ADR_1,DPT0_IX_SCL_STR_ADR_1),
			COALESCE(DPT4_IX_SCL_STR_ADR_2,DPT0_IX_SCL_STR_ADR_2),
			COALESCE(DPT4_IX_SCL_STR_ADR_3,DPT0_IX_SCL_STR_ADR_3),
			COALESCE(DPT4_IM_SCL_CT,DPT0_IM_SCL_CT),
			COALESCE(DPT4_IC_SCL_DOM_ST,DPT0_IC_SCL_DOM_ST),
			COALESCE(DPT4_IF_SCL_ZIP_CDE,DPT0_IF_SCL_ZIP_CDE)
	;
QUIT;

ENDRSUBMIT;

DATA SCHLS; SET LEGEND.SCHLS; RUN;

PROC EXPORT
		DATA=SCHLS
		OUTFILE='T:\COD - SCHOOL WELCOME LETTER - FED.XLSX'
		LABEL
		REPLACE;
RUN;

/*DATA LOANS; SET LEGEND.LOANS; RUN;*/
/**/
/*PROC EXPORT*/
/*		DATA=LOANS*/
/*		OUTFILE='T:\COD SCHOOL WELCOME LETTER DETAIL.XLSX'*/
/*		REPLACE;*/
/*RUN;*/