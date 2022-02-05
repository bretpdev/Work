/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS26.NWS26RZ";
FILENAME REPORT2 "&RPTLIB/UNWS26.NWS26R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL ;
CONNECT TO DB2 (DATABASE=&DB);
CREATE TABLE SKIP AS
SELECT	*
FROM CONNECTION TO DB2 
(
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID,
		PD10.DM_PRS_1,
		PD10.DM_PRS_LST,
		PD32.DX_ADR_EML
		,ln10.bf_ssn
	FROM PKUB.LN10_LON LN10
		LEFT JOIN (
					SELECT 
						A.DF_PRS_ID
					FROM PKUB.PD30_PRS_ADR A
					WHERE A.DI_VLD_ADR = 'Y'
					) PD30
			ON LN10.BF_SSN = PD30.DF_PRS_ID
		LEFT JOIN (
					SELECT 
						A.DF_PRS_ID
					FROM PKUB.PD40_PRS_PHN A
					WHERE A.DI_PHN_VLD = 'Y'
					) PD40
			ON LN10.BF_SSN = PD40.DF_PRS_ID
		INNER JOIN PKUB.PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN ( 
			SELECT XX.DF_PRS_ID,
					COALESCE(X.DX_ADR_EML,COALESCE(Y.DX_ADR_EML,Z.DX_ADR_EML))  AS "DX_ADR_EML"
			FROM PKUB.PD32_PRS_ADR_EML XX
				LEFT OUTER JOIN PKUB.PD32_PRS_ADR_EML X
					ON XX.DF_PRS_ID = X.DF_PRS_ID
					AND X.DC_ADR_EML = 'H'
					AND X.DC_STA_PD32 = 'A'
					AND X.DI_VLD_ADR_EML = 'Y'
				LEFT OUTER JOIN PKUB.PD32_PRS_ADR_EML Y
					ON XX.DF_PRS_ID = Y.DF_PRS_ID
					AND Y.DC_ADR_EML = 'A'
					AND Y.DC_STA_PD32 = 'A'
					AND Y.DI_VLD_ADR_EML = 'Y'
				LEFT OUTER JOIN PKUB.PD32_PRS_ADR_EML Z
					ON XX.DF_PRS_ID = Z.DF_PRS_ID
					AND Z.DC_ADR_EML = 'W'
					AND Z.DC_STA_PD32 = 'A'
					AND Z.DI_VLD_ADR_EML = 'Y'
			) PD32
			ON LN10.BF_SSN = PD32.DF_PRS_ID
		LEFT JOIN PKUB.DW01_A DW01
			ON LN10.BF_SSN = DW01.BF_SSN
			AND LN10.LN_SEQ = DW01.LN_SEQ
		LEFT JOIN (
					SELECT 
						LN60.BF_SSN,
						LN60.LN_SEQ	
					FROM PKUB.LN60_BR_FOR_APV LN60
						INNER JOIN PKUB.FB10_BR_FOR_REQ FB10
							ON LN60.BF_SSN = FB10.BF_SSN
							AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
					WHERE FB10.LC_FOR_STA = 'A'
					AND FB10.LC_STA_FOR10 = 'A'
					AND LN60.LC_STA_LON60 = 'A'
					AND FB10.LC_FOR_TYP = '28'
					) FORB
			ON LN10.BF_SSN = FORB.BF_SSN
			AND LN10.LN_SEQ = FORB.LN_SEQ
		LEFT JOIN (
					SELECT A.BF_SSN
					FROM	PKUB.AY10_BR_LON_ATY A
					WHERE	A.PF_REQ_ACT = 'KEMAL'
					AND		DAYS(A.LD_ATY_REQ_RCV) >= DAYS(CURRENT_DATE) -45
					) AY10
			ON LN10.BF_SSN = AY10.BF_SSN
	WHERE
		LN10.LA_CUR_PRI > 0
		AND LN10.LC_STA_LON10 = 'R'
		AND (PD30.DF_PRS_ID IS NULL
		OR 
		PD40.DF_PRS_ID IS NULL)
		AND PD32.DX_ADR_EML IS NOT NULL
		AND DW01.WC_DW_LON_STA NOT IN ('21','20','17','16','22','12','19','18')
		AND FORB.BF_SSN IS NULL
		AND AY10.BF_SSN IS NULL
	FOR READ ONLY WITH UR
)
;

DISCONNECT FROM DB2;

QUIT;

ENDRSUBMIT;
DATA SKIP; SET LEGEND.SKIP; RUN;

/*write to comma delimited file*/
DATA _NULL_;
	SET		WORK.SKIP;
	FILE	REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;
	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
		DO;
			PUT	
				'DF_SPE_ACC_ID'
				','
				'DM_PRS_1'
				','
				'DM_PRS_LST'
				','
				'DX_ADR_EML'
			;
		END;

	/* write data*/	
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_ADR_EML $;
		;
	END;
RUN;
