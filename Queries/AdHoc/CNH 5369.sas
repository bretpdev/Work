LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE OTH AS
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID,
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LC_TYP_SCH_DIS,
			LNXX.LN_RPS_SEQ,
			LNXX.LA_CUR_PRI + DWXX.WA_TOT_BRI_OTS AS CUR_BAL,
			LNXX.LR_ITR/XXX AS LR_ITR
		FROM
			PKUB.PDXX_PRS_NME PDXX
			JOIN PKUB.LNXX_LON LNXX
				ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				AND LNXX.LA_CUR_PRI > X
				AND LNXX.LC_STA_LONXX = 'R'
			JOIN PKUB.DWXX_DW_CLC_CLU DWXX
				ON LNXX.BF_SSN = DWXX.BF_SSN
				AND LNXX.LN_SEQ = DWXX.LN_SEQ
			JOIN PKUB.RSXX_BR_RPD RSXX
				ON LNXX.BF_SSN = RSXX.BF_SSN
			JOIN PKUB.LNXX_LON_RPS LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
				AND LNXX.LC_TYP_SCH_DIS IN ('IL', 'IB', 'CA', 'CL', 'CP', 'CQ', 'CX', 'CX', 'CX')
				AND RSXX.LC_STA_RPSTXX = 'A'
				AND LNXX.LC_STA_LONXX = 'A'
			JOIN PKUB.LNXX_LON_RPS_SPF LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
			JOIN PKUB.LNXX_INT_RTE_HST LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND TODAY() BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END
	;

	CREATE TABLE PMT_GRDS AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.BF_SSN||PUT(LNXX.LN_SEQ,ZX.) AS LID,
			LNXX.LN_RPS_SEQ,
			RSXX.LD_RPS_X_PAY_DU,
			LNXX.LN_GRD_RPS_SEQ,
			LNXX.LA_RPS_ISL,
			LNXX.LN_RPS_TRM
		FROM
			PKUB.RSXX_BR_RPD RSXX
			JOIN PKUB.LNXX_LON_RPS LNXX
				ON RSXX.BF_SSN = LNXX.BF_SSN
				AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
				AND LNXX.LC_TYP_SCH_DIS IN ('IL', 'IB', 'CA', 'CL', 'CP', 'CQ', 'CX', 'CX', 'CX')
				AND RSXX.LC_STA_RPSTXX = 'A'
				AND LNXX.LC_STA_LONXX = 'A'
			JOIN PKUB.LNXX_LON_RPS_SPF LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
		ORDER BY
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LN_RPS_SEQ,
			LNXX.LN_GRD_RPS_SEQ
	;
QUIT;

DATA PMT_TRMS;
	SET PMT_GRDS;
	LENGTH PREV_LID $ XX;
	FORMAT LD_GRD_BEG LD_GRD_END DATEX.; 

	RETAIN PREV_LID '';
	RETAIN LD_GRD_END; 

/*	determine the beginning of the first term for the gradation each time a new gradation record is read in*/
	IF PREV_LID NE LID THEN 
		DO;
			PMT_DUE = INTNX('MONTH',LD_RPS_X_PAY_DU,-X,'SAME');
			PREV_LID = LID;
		END;
	ELSE 
		PMT_DUE = LD_GRD_END + X; 

/*	create a gradation term record for each repayment term in the gradation*/
	DO LN_GRD_TRM = X TO LN_RPS_TRM;
		IF LN_GRD_TRM = X THEN LD_GRD_BEG = PMT_DUE;
		ELSE LD_GRD_BEG = LD_GRD_END + X;

		LD_GRD_END = INTNX('MONTH',LD_GRD_BEG - X,X,'SAME');

		OUTPUT; 
	END;
RUN;

/*identifies the current payment amount*/
DATA PMT_AMT (KEEP = BF_SSN LN_SEQ LA_RPS_ISL);
	SET PMT_GRDS;
	LENGTH PREV_LID $ XX;
	FORMAT LD_GRD_BEG LD_GRD_END DATEX.; *format dates so they're human readble;

	RETAIN PREV_LID '';
	RETAIN LD_GRD_END; *retain the end date to calculate the begin date of the next gradation if the next gradation is for the same loan as the previous gradation;

	TARG_DATE = TODAY(); *change this to look for a different target date;

/*	determine the beginning of the first term for the gradation each time a new gradation record is read in*/
	IF PREV_LID NE LID THEN  
		DO; *if the loan changed, the first term starts a month before the first payment due for the repayment schedule;
			PMT_DUE = INTNX('MONTH',LD_RPS_X_PAY_DU,-X,'SAME');
			PREV_LID = LID;
		END;
	ELSE 
		PMT_DUE = LD_GRD_END + X; *if the loan did not change, the first term starts after the end of the last term for the previous gradation;

	IF PMT_DUE > TARG_DATE THEN RETURN; *the term for the target date has already been found for the loan so move on to the next observation;

/*	calculate the gradation term begin and end for each payment in the gradation*/
	DO LN_GRD_TRM = X TO LN_RPS_TRM;
		IF LN_GRD_TRM = X THEN LD_GRD_BEG = PMT_DUE;	*use the beginning of the first term calculated above for the first term of the gradation;
		ELSE LD_GRD_BEG = LD_GRD_END + X;	*add a day to the end of the previous term to get the begin date of the next term;

		LD_GRD_END = INTNX('MONTH',LD_GRD_BEG - X,X,'SAME'); *add a month (less a day) to the term begin date to get the term end date;

/*		once the current gradation is found, output the data and then move on to the next observation */
		IF TARG_DATE >= LD_GRD_BEG AND TARG_DATE <= LD_GRD_END THEN
			DO;
				OUTPUT;
				RETURN;
			END;
	END;
RUN;

/*sum loan level payment and balances at the interest rate level (groups loans with the same interest rate)*/
PROC SQL;
	CREATE TABLE IDR_ITR_LVL AS
		SELECT DISTINCT
			OTH.LN_RPS_SEQ,
			OTH.DF_SPE_ACC_ID,
			OTH.LC_TYP_SCH_DIS,
			OTH.LR_ITR,
			SUM(PMT.LA_RPS_ISL) AS LA_RPS_ISL,
			SUM(OTH.CUR_BAL) AS CUR_BAL
		FROM
			OTH
			JOIN PMT_AMT PMT
				ON OTH.BF_SSN = PMT.BF_SSN
				AND OTH.LN_SEQ = PMT.LN_SEQ
		GROUP BY
			OTH.LN_RPS_SEQ,
			OTH.DF_SPE_ACC_ID,
			OTH.LC_TYP_SCH_DIS,
			OTH.LR_ITR
		ORDER BY
			OTH.DF_SPE_ACC_ID,
			OTH.LN_RPS_SEQ
	;
QUIT;

ENDRSUBMIT;

DATA IDR_ITR_LVL; SET LEGEND.IDR_ITR_LVL; RUN;



/*get data at the repayment schedule level*/
PROC SQL;
	CREATE TABLE IDR_RS_LVL AS
		SELECT DISTINCT
			DF_SPE_ACC_ID,
			LN_RPS_SEQ,
			LC_TYP_SCH_DIS,
			SUM(LA_RPS_ISL) AS LA_RPS_ISL,
			SUM(CUR_BAL) AS BAL
		FROM 
			IDR_ITR_LVL
		GROUP BY
			DF_SPE_ACC_ID,
			LN_RPS_SEQ,
			LC_TYP_SCH_DIS
	;
QUIT;

/*calculate a weighted interest rate for each rate within the repayment schedule*/
PROC SQL;
	CREATE TABLE WGT_ITR AS
		SELECT DISTINCT
			IDR.DF_SPE_ACC_ID,
			IDR.LN_RPS_SEQ,
			IDR.LR_ITR*IDR.CUR_BAL/BAL.BAL AS WGT_ITR
		FROM
			IDR_ITR_LVL IDR
			JOIN IDR_RS_LVL BAL
				ON IDR.DF_SPE_ACC_ID = BAL.DF_SPE_ACC_ID
				AND IDR.LN_RPS_SEQ = BAL.LN_RPS_SEQ
	;

QUIT;

/*sum the weighted rates to get a cumulative weighted interest rate for each repayment schedule*/
PROC SQL;
	CREATE TABLE WGT_ITR_RS_LVL AS
		SELECT DISTINCT
			DF_SPE_ACC_ID,
			LN_RPS_SEQ,
			SUM(WGT_ITR) AS WGT_ITR
		FROM
			WGT_ITR
		GROUP BY
			DF_SPE_ACC_ID,
			LN_RPS_SEQ
	;
QUIT;

PROC SQL;
	CREATE TABLE IDR_WITH_WGT_ITR AS
		SELECT DISTINCT
			IDR.DF_SPE_ACC_ID,
			IDR.LN_RPS_SEQ,
			IDR.LC_TYP_SCH_DIS,
			IDR.LA_RPS_ISL,
			IDR.BAL,
			ITR.WGT_ITR
		FROM
			IDR_RS_LVL IDR
			JOIN WGT_ITR_RS_LVL ITR
				ON IDR.DF_SPE_ACC_ID = ITR.DF_SPE_ACC_ID
				AND IDR.LN_RPS_SEQ = ITR.LN_RPS_SEQ
	;
QUIT;

/*calculate the number of payment remaining, note:  the function errors if any of the conditions in the if statement are true so it can't calculate the payments remaining in those cases*/
DATA IDR;
	SET IDR_WITH_WGT_ITR;
	IF BAL <= X OR LA_RPS_ISL <= X OR COALESCE(WGT_ITR,X) <= X OR BAL * WGT_ITR/XX > LA_RPS_ISL THEN PMTS_REMAINING = .;
	ELSE PMTS_REMAINING = MORT(BAL,LA_RPS_ISL,WGT_ITR/XX,.);
RUN;
	
PROC EXPORT
		DATA=IDR
		OUTFILE='T:\SAS\IDR.XLSX'
		REPLACE;
RUN;

