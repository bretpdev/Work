/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/NH XXXX.txt";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";



/*LIVE*/
%LET CLS = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
/*TEST*/
/*%LET CLS = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no");*/


PROC SQL NOPRINT;
      CONNECT TO ODBC AS SCRA (&CLS);
      CREATE TABLE BorrowerData AS
            SELECT *
            FROM CONNECTION TO SCRA
                  (
                        SELECT
                              BOR.BorrowerAccountNumber,
                              Max(ADB.BeginDate) AS BeginDateX,
                              Max(ADB.EndDate) as EndDateX
                        FROM
                              [scra].[Borrowers] BOR
                        INNER JOIN [scra].[ActiveDuty] ADB
                              ON BOR.BorrowerId = ADB.BorrowerId
                              AND ADB.Active = X
                        GROUP BY 
                              BOR.BorrowerAccountNumber

                  );
DISCONNECT FROM SCRA;
QUIT;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.BorrowerData (DROP = BeginDateX EndDateX);  *Send data to Duster;
SET BorrowerData;
BeginDate = DATEPART(BeginDateX); 
    EndDate = DATEPART(EndDateX); 
    FORMAT BeginDate EndDate DATEX.;

RUN;


RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

DATA _NULL_;
	CALL SYMPUT('EOM',"'"||PUT(INTNX('MONTH',TODAY(),X,'E'),dateX.)||"'");
RUN;
%PUT &EOM;

PROC SQL;
	CREATE TABLE MILBEN AS
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID,
			LNXX.BF_SSN,
			COMPRESS(PDXX.DM_PRS_X) || ' ' || PDXX.DM_PRS_LST AS NAME,
			CASE
				WHEN LNXX.LR_ITR = X AND LNXX.LC_INT_RDC_PGM = 'S' THEN 'Hostile Area X%'
				ELSE ''
			END AS QUALIFYING_BENEFIT_X,
			CASE
				WHEN LNXX.LR_ITR <= X AND (LNXX.LC_INT_RDC_PGM = 'M') THEN 'SCRA'
				ELSE ''
			END AS QUALIFYING_BENEFIT_X,
			CASE
				WHEN THIRTY_EIGHT.BF_SSN IS NOT NULL THEN 'Military Service Deferment'
				WHEN FORTY.BF_SSN IS NOT NULL THEN 'Post Active Duty Deferment'
				ELSE ''
			 END AS QUALIFYING_BENEFIT_X,
			 CASE
			 	WHEN BD.BeginDate IS NULL AND THIRTY_EIGHT.BF_SSN IS NOT NULL THEN THIRTY_EIGHT.BeginDate
				WHEN BD.BeginDate IS NULL AND FORTY.BF_SSN IS NOT NULL THEN FORTY.BeginDate
				WHEN BD.BeginDate IS NULL AND (LNXX.LR_ITR <= X AND LNXX.LC_INT_RDC_PGM = 'M' AND TODAY() BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END) THEN LNXX.LD_ITR_EFF_BEG
				WHEN BD.BeginDate IS NULL AND (LNXX.LR_ITR = X AND LNXX.LC_INT_RDC_PGM = 'S' AND TODAY() BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END) THEN LNXX.LD_ITR_EFF_BEG
				ELSE BD.BeginDate
			 END AS BeginDate,
			 CASE
			 	WHEN BD.EndDate IS NULL AND THIRTY_EIGHT.BF_SSN IS NOT NULL THEN THIRTY_EIGHT.EndDate
				WHEN BD.EndDate IS NULL AND FORTY.BF_SSN IS NOT NULL THEN FORTY.EndDate
				WHEN BD.EndDate IS NULL AND (LNXX.LR_ITR <= X AND LNXX.LC_INT_RDC_PGM = 'M' AND TODAY() BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END) THEN LNXX.LD_ITR_EFF_END
				WHEN BD.EndDate IS NULL AND (LNXX.LR_ITR = X AND LNXX.LC_INT_RDC_PGM = 'S' AND TODAY() BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END) THEN LNXX.LD_ITR_EFF_END
				ELSE BD.EndDate
			 END AS EndDate,
			 LNXX.LN_DLQ_MAX
		FROM
			PKUB.PDXX_PRS_NME PDXX
			LEFT JOIN BorrowerData BD
				ON BD.BorrowerAccountNumber = PDXX.DF_SPE_ACC_ID
			JOIN PKUB.LNXX_LON LNXX
				ON PDXX.DF_PRS_ID = LNXX.BF_SSN
			INNER JOIN
			(
				SELECT
					BF_SSN,
					MAX(LN_ATY_SEQ) AS LN_ATY_SEQ
				FROM
					PKUB.AYXX_BR_LON_ATY 
				WHERE
					PF_REQ_ACT = 'ASCRA'
				group by bf_ssn
			)ASCRA
				ON ASCRA.BF_SSN = LNXX.BF_SSN
			LEFT JOIN
			(
				SELECT
					BF_SSN,
					MAX(LN_ATY_SEQ) AS LN_ATY_SEQ
				FROM
					PKUB.AYXX_BR_LON_ATY 
				WHERE
					PF_REQ_ACT = 'ISCRA'
				group by
					bf_ssn
			)ISCRA
				ON ISCRA.BF_SSN = LNXX.BF_SSN
			JOIN PKUB.DWXX_DW_CLC_CLU DWXX
				ON LNXX.BF_SSN = DWXX.BF_SSN
				AND LNXX.LN_SEQ = DWXX.LN_SEQ
			LEFT JOIN PKUB.LNXX_INT_RTE_HST LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND	INPUT(&EOM, dateX.) BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END
				AND LNXX.LD_ITR_APL <= INPUT(&EOM, dateX.)
				AND LNXX.LC_STA_LONXX = 'A'
			LEFT JOIN 
				(
					SELECT DISTINCT
						DWXX.BF_SSN,
						LNXX.LD_DFR_BEG AS BEGINDATE,
						LNXX.LD_DFR_END AS ENDDATE
					FROM
						PKUB.DWXX_DW_CLC_CLU DWXX
						JOIN PKUB.LNXX_BR_DFR_APV LNXX
							ON DWXX.BF_SSN = LNXX.BF_SSN
							AND DWXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LC_STA_LONXX = 'A'
							AND INPUT(&EOM, dateX.) BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
							AND LNXX.LD_DFR_APL <= INPUT(&EOM, dateX.)
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
						DWXX.BF_SSN,
						LNXX.LD_DFR_BEG AS BEGINDATE,
						LNXX.LD_DFR_END AS ENDDATE
					FROM
						PKUB.DWXX_DW_CLC_CLU DWXX
						JOIN PKUB.LNXX_BR_DFR_APV LNXX
							ON DWXX.BF_SSN = LNXX.BF_SSN
							AND DWXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LC_STA_LONXX = 'A'
							AND INPUT(&EOM, dateX.) BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
							AND LNXX.LD_DFR_APL <= INPUT(&EOM, dateX.)
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
			AND (ISCRA.BF_SSN IS NULL OR ASCRA.LN_ATY_SEQ > ISCRA.LN_ATY_SEQ)
			AND
				(
					(LNXX.LR_ITR = X AND LNXX.LC_INT_RDC_PGM = 'S')
					OR (LNXX.LR_ITR <= X AND LNXX.LC_INT_RDC_PGM = 'M')
					OR (LNXX.LR_ITR <= X AND LNXX.LD_LON_X_DSB < BD.BeginDate AND INPUT(&EOM, DateX.) <= BD.EndDate)
					OR THIRTY_EIGHT.BF_SSN IS NOT NULL
					OR FORTY.BF_SSN IS NOT NULL
				)
		ORDER BY
			LNXX.BF_SSN
;
QUIT;

ENDRSUBMIT;

DATA MILBEN; SET LEGEND.MILBEN; RUN;

PROC SORT DATA=MILBEN; BY DF_SPE_ACC_ID; RUN;

PROC SQL;
	CREATE TABLE MILBENX AS
		SELECT
			M.BF_SSN AS SSN
			,M.NAME
			,CATX('; ',M.QUALIFYING_BENEFIT_X,M.QUALIFYING_BENEFIT_X,M.QUALIFYING_BENEFIT_X) AS QUALIFYING_BENEFIT
			,M.BeginDate
			,M.EndDate
		FROM 
			MILBEN M
		WHERE QUALIFYING_BENEFIT_X LIKE 'SCRA'
;
QUIT;

DATA MILBENX (drop=BeginDate EndDate);
	SET MILBENX;
	BeginDateX= put(BeginDate, mmddyyXX.);
	EndDateX= put(EndDate, mmddyyXX.);
RUN;

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
		PUT BeginDateX $ @;
		PUT EndDateX $;
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
