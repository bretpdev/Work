/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
FILENAME REPORTZ "&RPTLIB/UNWRXX.NWRXXRZ";
FILENAME REPORTX "&RPTLIB/UNWRXX.NWRXXRX";
FILENAME REPORTX "&RPTLIB/UNWRXX.NWRXXRX";
FILENAME REPORTX "&RPTLIB/UNWRXX.NWRXXRX";

DATA _NULL_;
	RUN_DAY = TODAY();
	CALL SYMPUT('MON_AGO_X_BEG',"'"||PUT(INTNX('MONTH',RUN_DAY,-X,'B'), MMDDYYDXX.)||"'");
	CALL SYMPUT('MON_AGO_X_END',"'"||PUT(INTNX('MONTH',RUN_DAY,-X,'E'), MMDDYYDXX.)||"'");
	CALL SYMPUT('RUN_DATE',RUN_DAY);
	CALL SYMPUTX('TITLE_DATE',PUT(INTNX('MONTH',RUN_DAY,-X,'E'), worddateXX.));
RUN;

%SYSLPUT MON_AGO_X_BEG = &MON_AGO_X_BEG;
%SYSLPUT MON_AGO_X_END = &MON_AGO_X_END;
RSUBMIT LEGEND;
LIBNAME LGND '/sas/whse/progrevw';

PROC SQL;
CONNECT TO DBX (DATABASE=DNFPUTDL);
CREATE TABLE MSBTFF AS 
SELECT A.*
	,B.WC_DW_LON_STA
FROM CONNECTION TO DBX (
	SELECT E.DF_SPE_ACC_ID
		,A.BF_SSN
		,A.LN_SEQ
		,B.IC_LON_PGM
		,B.LC_FED_PGM_YR
		,B.LF_FED_CLC_RSK
		,B.LA_CUR_PRI
		,A.LA_FAT_CUR_PRI
		,A.LA_FAT_NSI
		,D.LA_FMS_MTH_INT_ADJ
		,D.LA_FMS_MTH_INT_ACR
		,A.LA_FAT_NSI + D.LA_FMS_MTH_INT_ACR + D.LA_FMS_MTH_INT_ADJ AS INT_FINAL
		,COALESCE(A.LA_FAT_LTE_FEE,X) AS LA_FAT_LTE_FEE
		,COALESCE(DAYS(CURRENT DATE) - DAYS(F.LD_DLQ_OCC),X) AS DAYS_DELQ
		,G.LA_OTS_PRI_ELG
		,FSXX.LF_FED_AWD
		,FSXX.LN_FED_AWD_SEQ
	FROM PKUS.LNXX_LON B
	INNER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,SUM(LA_FAT_CUR_PRI) AS LA_FAT_CUR_PRI
			,SUM(LA_FAT_NSI) AS LA_FAT_NSI
			,SUM(LA_FAT_LTE_FEE) AS LA_FAT_LTE_FEE
		FROM PKUS.LNXX_FIN_ATY 
		WHERE LD_FAT_APL <= &MON_AGO_X_END
		GROUP BY BF_SSN
			,LN_SEQ
		) A
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
	INNER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,SUM(COALESCE(LA_FMS_MTH_INT_ADJ,X)) AS LA_FMS_MTH_INT_ADJ
			,SUM(COALESCE(LA_FMS_MTH_INT_ACR,X)) AS LA_FMS_MTH_INT_ACR
		FROM PKUR.FRXX_MTH_INT_RPT
		GROUP BY BF_SSN
			,LN_SEQ
		) D
		ON A.BF_SSN = D.BF_SSN
		AND A.LN_SEQ = D.LN_SEQ
	INNER JOIN PKUS.PDXX_PRS_NME E
		ON B.BF_SSN = E.DF_PRS_ID
	LEFT OUTER JOIN PKUS.LNXX_LON_DLQ_HST F
		ON B.BF_SSN = F.BF_SSN
		AND B.LN_SEQ = F.LN_SEQ
		AND F.LC_STA_LONXX = 'X'
		AND F.LC_DLQ_TYP = 'P'
	LEFT OUTER JOIN PKUS.LNXX_LON_MTH_BAL G
		ON B.BF_SSN = G.BF_SSN
		AND B.LN_SEQ = G.LN_SEQ
		AND G.LD_EFF_MTH_BAL = &MON_AGO_X_END
	JOIN PKUB.FSXX_DL_LON FSXX
		ON B.BF_SSN = FSXX.BF_SSN
		AND B.LN_SEQ = FSXX.LN_SEQ
	FOR READ ONLY WITH UR
	) A 
	LEFT OUTER JOIN  LGND.DWXX_PREV_MONTH B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
	;
DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;
DATA MSBTFF; SET LEGEND.MSBTFF; RUN;

PROC EXPORT
		DATA=MSBTFF
		OUTFILE='T:\SAS\UTNWRXX DETAIL.XLSX'
		REPLACE;
RUN;
