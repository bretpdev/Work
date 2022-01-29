%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

/*%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;*/

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
				SELECT
					PDXX.DF_PRS_ID AS SSN,
					PDXX.DM_PRS_X AS NAME,
					PDXX.DF_ZIP_CDE AS ZIP, 
					PDXX.DM_CT AS CITY,
					PDXX.DC_DOM_ST AS STATE,
					LNXX.LF_DOE_SCL_ORG AS SCHOOL,
					SCXX.IC_TYP_SCL AS SCHOOL_CODE,
					LNXX.DSB_AMT AS AMOUNT_BORROWED,
					LNXX.LN_SEQ AS LOAN_SEQUENCE,
					LNXX.LA_CUR_PRI AS OUTSTANDING_PRINCIPAL,
					DWXX.WA_TOT_BRI_OTS AS OUTSTANDING_INTEREST,
					LNXX.LN_DLQ_MAX AS DELINQUENT_DAYS,
					SUM(LNXXDEF.DEFDAYS) AS DEFERMENT_DAYS_USED,
					SUM(LNXXFOR.FORDAYS) AS FORBEARANCE_DAYS_USED,
					PDXX.DD_BRT AS BIRTH_DATE
				FROM
					PKUB.PDXX_PRS_NME PDXX
					LEFT JOIN
						PKUB.PDXX_PRS_ADR PDXX
						ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
						AND PDXX.DC_ADR = 'L'
					INNER JOIN
						PKUB.LNXX_LON LNXX
						ON LNXX.BF_SSN = PDXX.DF_PRS_ID
					LEFT JOIN
						PKUB.SCXX_SCH_DMO SCXX
						ON SCXX.IF_DOE_SCL = LNXX.LF_DOE_SCL_ORG
					LEFT JOIN
					(
						SELECT
							BF_SSN,
							LN_SEQ,
							SUM(coalesce(LA_DSB,X.XX) - coalesce(LA_DSB_CAN,X.XX)) AS DSB_AMT
						FROM
							PKUB.LNXX_DSB
						WHERE
							LC_STA_LONXX = 'X'
						GROUP BY
							BF_SSN,
							LN_SEQ
					) LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					LEFT JOIN
						PKUB.DWXX_DW_CLC_CLU DWXX
						ON DWXX.BF_SSN = LNXX.BF_SSN
						AND DWXX.LN_SEQ = LNXX.LN_SEQ
					LEFT JOIN
						PKUB.LNXX_LON_DLQ_HST LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
						AND LNXX.LC_STA_LONXX = 'X'
					LEFT JOIN
						(
							SELECT
								LNXX.BF_SSN,
								LNXX.LN_SEQ, 
								DAYS(COALESCE(LNXX.LD_DFR_END,CURRENT DATE)) - DAYS(LNXX.LD_DFR_BEG) AS DEFDAYS
							FROM
								PKUB.DFXX_BR_DFR_REQ DFXX
							INNER JOIN PKUB.LNXX_BR_DFR_APV LNXX
								ON DFXX.BF_SSN = LNXX.BF_SSN
								AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
							WHERE 
								DFXX.LC_STA_DFRXX = 'A'
								AND LNXX.LC_STA_LONXX = 'A'
								AND LNXX.LD_DFR_BEG IS NOT NULL
						) LNXXDEF
							ON LNXXDEF.BF_SSN = LNXX.BF_SSN
							AND LNXXDEF.LN_SEQ = LNXX.LN_SEQ
					LEFT JOIN
						(
							SELECT
								LNXX.BF_SSN,
								LNXX.LN_SEQ, 
								DAYS(COALESCE(LNXX.LD_FOR_END,CURRENT DATE)) - DAYS(LNXX.LD_FOR_BEG) AS FORDAYS
							FROM
								PKUB.FBXX_BR_FOR_REQ FBXX
							INNER JOIN PKUB.LNXX_BR_FOR_APV LNXX
								ON LNXX.BF_SSN = FBXX.BF_SSN
								AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
							WHERE 
								FBXX.LC_FOR_STA = 'A'
								AND LC_STA_LONXX = 'A'
								AND LNXX.LD_FOR_BEG IS NOT NULL
						) LNXXFOR
							ON LNXXFOR.BF_SSN = LNXX.BF_SSN
							AND LNXXFOR.LN_SEQ = LNXX.LN_SEQ
				WHERE
					LNXX.LC_STA_LONXX = 'R' 
					AND LNXX.LA_CUR_PRI > X
				GROUP BY
					PDXX.DF_PRS_ID,
					PDXX.DM_PRS_X,
					PDXX.DF_ZIP_CDE,
					PDXX.DM_CT,
					PDXX.DC_DOM_ST,
					LNXX.LF_DOE_SCL_ORG,
					SCXX.IC_TYP_SCL,
					LNXX.DSB_AMT,
					LNXX.LN_SEQ,
					LNXX.LA_CUR_PRI,
					LNXX.LN_DLQ_MAX, 			
					PDXX.DD_BRT,
					DWXX.WA_TOT_BRI_OTS
					

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;
	%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;
/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
