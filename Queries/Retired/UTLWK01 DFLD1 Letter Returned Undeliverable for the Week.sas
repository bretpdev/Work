/*

LETTERS RETURNED UNDELIVERABLE FOR THE WEEK

Lists borrowers with a DLDF1 letter returned undeliverable during the past 7 days.  Used by
collections to verify the DLDF1 invalid indicator is being set properly.

*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWK01.LWK01R2";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.BF_SSN			AS SSN,
	B.BF_CRT_DTS_AY01	AS DT_DLDF1,
	A.li_dfl_ltr_1_ivl	AS DLDF1_IND
FROM	OLWHRM1.DC01_LON_CLM_INF A INNER JOIN
		OLWHRM1.AY01_BR_ATY B ON
			A.bf_ssn = B.df_prs_id AND
			A.lc_sta_DC10 = '03' AND
			A.ld_clm_asn_doe IS NULL AND
			B.PF_ACT = 'DLDF1' AND
			DAYS(B.BF_CRT_DTS_AY01) >= DAYS(CURRENT DATE) - 7
WHERE	EXISTS	(SELECT *
				 FROM OLWHRM1.AY01_BR_ATY X
				 WHERE  A.bf_ssn = X.df_prs_id AND
				 		X.PF_ACT = 'KLINV' AND
						DAYS(X.BF_CRT_DTS_AY01) > DAYS(B.BF_CRT_DTS_AY01)) AND
		(SELECT MAX(Y.BF_CRT_DTS_AY01)
		 FROM OLWHRM1.AY01_BR_ATY Y
		 WHERE 	A.bf_ssn = Y.df_prs_id AND
				Y.PF_ACT ='KLINV') >
		(SELECT MAX(Z.BF_CRT_DTS_AY01)
		 FROM OLWHRM1.AY01_BR_ATY Z
		 WHERE 	A.bf_ssn = Z.df_prs_id AND
				Z.PF_ACT ='DLDF1')

ORDER BY A.li_dfl_ltr_1_ivl, A.BF_SSN
);
DISCONNECT FROM DB2;
ENDRSUBMIT;
/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS PAGENO=1 LS=80;
PROC PRINT NOOBS SPLIT='/' DATA=WORKLOCL.DEMO N='NUMBER OF BORROWERS = ';
by DLDF1_IND;
VAR SSN
	DT_DLDF1
	DLDF1_IND;
LABEL	DT_DLDF1 = 'DLDF1 LETTER SENT DATE'
		DLDF1_IND = 'DLDF1 LETTER INVALID INDICATOR';
TITLE	'DLDF1 LETTERS RETURNED UNDELIVERABLE FOR THE WEEK';
RUN;