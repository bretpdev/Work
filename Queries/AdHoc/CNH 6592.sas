/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

/*DATA _NULL_;*/
/*	CALL SYMPUT('EOM',"'"||PUT(INTNX('MONTH',TODAY(),-X,'E'),MMDDYYXX.)||"'");*/
/*RUN;*/
/*%PUT &EOM;*/

%MACRO SQLCHECK ;
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
%MEND  ;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE MILBEN AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN
						,RTRIM(PDXX.DM_PRS_X) || ' ' || PDXX.DM_PRS_LST AS NAME
						,CASE
							WHEN LNXX.LR_ITR = X AND LNXX.LC_INT_RDC_PGM = 'S' THEN 'Hostile Area X%'
							ELSE ''
						END AS QUALIFYING_BENEFIT_X
						,CASE
							WHEN LNXX.LR_ITR <= X AND (LNXX.LC_INT_RDC_PGM = 'M' OR (DAYS(LNXX.LD_LON_X_DSB) < DAYS(ARCS.BEGIN_DATE) AND DAYS(CURRENT_DATE) <= DAYS(ARCS.VALID_END_DATE))) THEN 'SCRA'
							ELSE ''
						END AS QUALIFYING_BENEFIT_X
						,CASE
							WHEN THIRTY_EIGHT.BF_SSN IS NOT NULL THEN 'Military Service Deferment'
							WHEN FORTY.BF_SSN IS NOT NULL THEN 'Post Active Duty Deferment'
							ELSE ''
						 END AS QUALIFYING_BENEFIT_X
						,ARCS.BEGIN_DATE 
						,ARCS.END_DATE 
						,LNXX.LN_DLQ_MAX
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON LNXX.BF_SSN = DWXX.BF_SSN
							AND LNXX.LN_SEQ = DWXX.LN_SEQ
						JOIN (
								SELECT DISTINCT
									AYXX.BF_SSN
									,SUBSTR(AYXX.LX_ATY,LOCATE('BORROWER',AYXX.LX_ATY) + X, XX) AS BEGIN_DATE
									,SUBSTR(AYXX.LX_ATY,LOCATE('DATE =',AYXX.LX_ATY) + X,XX) AS END_DATE
/*									this case statement creates a valid date from the dummy value so the attribute can be used in date comparisons*/
									,CASE
										WHEN SUBSTR(AYXX.LX_ATY,LOCATE('DATE =',AYXX.LX_ATY) + X,X) = 'XX' THEN 'XX/XX/XXXX'
										ELSE SUBSTR(AYXX.LX_ATY,LOCATE('DATE =',AYXX.LX_ATY) + X,XX)
									END AS VALID_END_DATE
								FROM PKUB.AYXX_BR_LON_ATY AYXX
									JOIN (
											SELECT
												AYXX.BF_SSN
												,MAX(AYXX.LN_ATY_SEQ) AS LN_ATY_SEQ
											FROM 
												PKUB.AYXX_BR_LON_ATY AYXX
											WHERE 
												AYXX.PF_REQ_ACT = 'ASCRA'
											GROUP BY 
												AYXX.BF_SSN
												) ASCRA
										ON AYXX.BF_SSN = ASCRA.BF_SSN
										AND AYXX.LN_ATY_SEQ = ASCRA.LN_ATY_SEQ
									LEFT JOIN (
											SELECT
												AYXX.BF_SSN
												,MAX(AYXX.LN_ATY_SEQ) AS LN_ATY_SEQ
											FROM 
												PKUB.AYXX_BR_LON_ATY AYXX
											WHERE 
												AYXX.PF_REQ_ACT = 'ISCRA'
											GROUP BY 
												AYXX.BF_SSN
												) ISCRA
										ON AYXX.BF_SSN = ISCRA.BF_SSN
									JOIN PKUB.AYXX_ATY_TXT AYXX
										ON AYXX.BF_SSN = AYXX.BF_SSN
										AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
									WHERE AYXX.PF_REQ_ACT = 'ASCRA'
										AND (ASCRA.LN_ATY_SEQ > ISCRA.LN_ATY_SEQ)
										OR ISCRA.BF_SSN IS NULL
								) ARCS
							ON LNXX.BF_SSN = ARCS.BF_SSN
						LEFT JOIN PKUB.LNXX_INT_RTE_HST LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND	CURRENT_DATE BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END
							AND LNXX.LC_STA_LONXX = 'A'
						LEFT JOIN 
							(
								SELECT DISTINCT
									DWXX.BF_SSN
								FROM
									PKUB.DWXX_DW_CLC_CLU DWXX
									JOIN PKUB.LNXX_BR_DFR_APV LNXX
										ON DWXX.BF_SSN = LNXX.BF_SSN
										AND DWXX.LN_SEQ = LNXX.LN_SEQ
										AND LNXX.LC_STA_LONXX = 'A'
										AND CURRENT_DATE BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
									JOIN PKUB.DFXX_BR_DFR_REQ DFXX
										ON DWXX.BF_SSN = DFXX.BF_SSN
										AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
										AND DFXX.LC_DFR_STA = 'A'
										AND DFXX.LC_STA_DFRXX = 'A'
										AND DFXX.LC_DFR_TYP = 'XX'
								WHERE 
									DWXX.WC_DW_LON_STA = 'XX'
							) THIRTY_EIGHT
							ON LNXX.BF_SSN = THIRTY_EIGHT.BF_SSN
						LEFT JOIN 
							(
								SELECT DISTINCT
									DWXX.BF_SSN
								FROM
									PKUB.DWXX_DW_CLC_CLU DWXX
									JOIN PKUB.LNXX_BR_DFR_APV LNXX
										ON DWXX.BF_SSN = LNXX.BF_SSN
										AND DWXX.LN_SEQ = LNXX.LN_SEQ
										AND LNXX.LC_STA_LONXX = 'A'
										AND CURRENT_DATE BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
									JOIN PKUB.DFXX_BR_DFR_REQ DFXX
										ON DWXX.BF_SSN = DFXX.BF_SSN
										AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
										AND DFXX.LC_DFR_STA = 'A'
										AND DFXX.LC_STA_DFRXX = 'A'
										AND DFXX.LC_DFR_TYP = 'XX'
								WHERE 
									DWXX.WC_DW_LON_STA = 'XX'
							) FORTY
							ON LNXX.BF_SSN = FORTY.BF_SSN
						LEFT JOIN
							(
								SELECT
									LNXX.BF_SSN
									,MAX(LNXX.LN_DLQ_MAX) AS LN_DLQ_MAX
								FROM 
									PKUB.LNXX_LON_DLQ_HST LNXX
								WHERE 
									LNXX.LC_STA_LONXX = 'X'
									AND LNXX.LN_DLQ_MAX >= XXX
								GROUP BY 
									LNXX.BF_SSN
							) LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
					WHERE
						(LNXX.LA_CUR_PRI + DWXX.WA_TOT_BRI_OTS > X)
						AND LNXX.LC_STA_LONXX = 'R'
						AND
							(
								(LNXX.LR_ITR = X AND LNXX.LC_INT_RDC_PGM = 'S')
								OR (LNXX.LR_ITR <= X AND LNXX.LC_INT_RDC_PGM = 'M')
								OR (LNXX.LR_ITR <= X AND DAYS(LNXX.LD_LON_X_DSB) < DAYS(ARCS.BEGIN_DATE) AND DAYS(CURRENT_DATE) <= DAYS(ARCS.VALID_END_DATE))
								OR THIRTY_EIGHT.BF_SSN IS NOT NULL
								OR FORTY.BF_SSN IS NOT NULL
							)
					ORDER BY
						LNXX.BF_SSN

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA MILBEN; SET LEGEND.MILBEN; RUN;

PROC SQL;
	CREATE TABLE MILBENX AS
		SELECT
			M.BF_SSN AS SSN
			,M.NAME
			,CATX('; ',M.QUALIFYING_BENEFIT_X,M.QUALIFYING_BENEFIT_X,M.QUALIFYING_BENEFIT_X) AS QUALIFYING_BENEFIT
			,M.BEGIN_DATE
			,M.END_DATE
		FROM 
			MILBEN M
;
QUIT;

PROC SQL;
	CREATE TABLE MILBENX AS
		SELECT
			M.BF_SSN AS SSN
			,M.NAME
			,M.LN_DLQ_MAX AS DAYS_DELQ
		FROM 
			MILBEN M
		WHERE 
			M.LN_DLQ_MAX IS NOT NULL
;
QUIT;

/*write to comma delimited file*/
DATA _NULL_;
	SET		WORK.MILBENX;
	FILE
		REPORTX
		DELIMITER = ','
		DSD
		DROPOVER
		LRECL = XXXXX
	;

	FORMAT
		SSN $X.
		NAME $XX.
		QUALIFYING_BENEFIT $XX.
		BEGIN_DATE $XX.
		END_DATE $XX.
	;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = X THEN
		DO;
			PUT	
				'SSN'
				','
				'NAME'
				','
				'QUALIFYING_BENEFIT'
				','
				'ELEGIBILITY_START_DATE'
				','
				'ELEGIBILITY_END_DATE'
			;
		END;

	/* write data*/	
	DO;
		PUT SSN $ @;
		PUT NAME $ @;
		PUT QUALIFYING_BENEFIT $ @;
		PUT BEGIN_DATE $ @;
		PUT END_DATE $;
		;
	END;
RUN;

DATA _NULL_;
	SET		WORK.MILBENX;
	FILE
		REPORTX
		DELIMITER = ','
		DSD
		DROPOVER
		LRECL = XXXXX
	;

	FORMAT
		SSN $X.
		NAME $XX.
		DAYS_DELQ BESTXX.
	;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = X THEN
		DO;
			PUT	
				'SSN'
				','
				'NAME'
				','
				'DAYS_DELQ'
			;
		END;

	/* write data*/	
	DO;
		PUT SSN $ @;
		PUT NAME $ @;
		PUT DAYS_DELQ;
		;
	END;
RUN;
