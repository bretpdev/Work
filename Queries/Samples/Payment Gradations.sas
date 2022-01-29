LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DB2 DATABASE=DNFPUTDL OWNER=PKUB;

/*get sample data to work with, returns a list of gradations*/
PROC SQL INOBS = 1000;
	CREATE TABLE PMT_GRDS AS
		SELECT
			LN65.BF_SSN,
			LN65.LN_SEQ,
			LN65.BF_SSN||PUT(LN65.LN_SEQ,Z3.) AS LID,
			LN65.LN_RPS_SEQ,
			RS10.LD_RPS_1_PAY_DU,
			LN66.LN_GRD_RPS_SEQ,
			LN66.LA_RPS_ISL,
			LN66.LN_RPS_TRM
		FROM
			PKUB.RS10_BR_RPD RS10
			JOIN PKUB.LN65_LON_RPS LN65
				ON RS10.BF_SSN = LN65.BF_SSN
				AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ
				AND RS10.LC_STA_RPST10 = 'A'
				AND LN65.LC_STA_LON65 = 'A'
			JOIN PKUB.LN66_LON_RPS_SPF LN66
				ON LN65.BF_SSN = LN66.BF_SSN
				AND LN65.LN_SEQ = LN66.LN_SEQ
				AND LN65.LN_RPS_SEQ = LN66.LN_RPS_SEQ
		ORDER BY
			LN65.BF_SSN,
			LN65.LN_SEQ,
			LN65.LN_RPS_SEQ,
			LN66.LN_GRD_RPS_SEQ
	;
QUIT;
ENDRSUBMIT;

DATA PMT_GRDS; SET LEGEND.PMT_GRDS; RUN;

/*creates a data set with all of the terms for each gradation*/
DATA PMT_TRMS;
	SET PMT_GRDS;
	LENGTH PREV_LID $ 12;
	FORMAT LD_GRD_BEG LD_GRD_END DATE9.; *format dates so they're human readble;

	RETAIN PREV_LID '';
	RETAIN LD_GRD_END; *retain the end date to calculate the begin date of the next gradation if the next gradation is for the same loan as the previous gradation;

/*	determine the beginning of the first term for the gradation each time a new gradation record is read in*/
	IF PREV_LID NE LID THEN  *if the loan changed, the first term starts a month before the first payment due for the repayment schedule;
		DO;
			PMT_DUE = INTNX('MONTH',LD_RPS_1_PAY_DU,-1,'SAME');
			PREV_LID = LID;
		END;
	ELSE *if the loan did not change, the first term starts after the end of the last term for the previous gradation;
		PMT_DUE = LD_GRD_END + 1; 

/*	create a gradation term record for each repayment term in the gradation*/
	DO LN_GRD_TRM = 1 TO LN_RPS_TRM;
		IF LN_GRD_TRM = 1 THEN LD_GRD_BEG = PMT_DUE;	*use the beginning of the first term calculated above for the first term of the gradation;
		ELSE LD_GRD_BEG = LD_GRD_END + 1;	*add a day to the end of the previous term to get the begin date of the next term;

		LD_GRD_END = INTNX('MONTH',LD_GRD_BEG - 1,1,'SAME'); *add a month (less a day) to the term begin date to get the term end date;

		OUTPUT; *output the data;
	END;
RUN;

/*identifies the current payment amount*/
DATA PMT_AMT (KEEP = BF_SSN LN_SEQ LA_RPS_ISL);
	SET PMT_GRDS;
	LENGTH PREV_LID $ 12;
	FORMAT LD_GRD_BEG LD_GRD_END DATE9.; *format dates so they're human readble;

	RETAIN PREV_LID '';
	RETAIN LD_GRD_END; *retain the end date to calculate the begin date of the next gradation if the next gradation is for the same loan as the previous gradation;

	TARG_DATE = TODAY(); *change this to look for a different target date;

/*	determine the beginning of the first term for the gradation each time a new gradation record is read in*/
	IF PREV_LID NE LID THEN  
		DO; *if the loan changed, the first term starts a month before the first payment due for the repayment schedule;
			PMT_DUE = INTNX('MONTH',LD_RPS_1_PAY_DU,-1,'SAME');
			PREV_LID = LID;
		END;
	ELSE 
		PMT_DUE = LD_GRD_END + 1; *if the loan did not change, the first term starts after the end of the last term for the previous gradation;

	IF PMT_DUE > TARG_DATE THEN RETURN; *the term for the target date has already been found for the loan so move on to the next observation;

/*	calculate the gradation term begin and end for each payment in the gradation*/
	DO LN_GRD_TRM = 1 TO LN_RPS_TRM;
		IF LN_GRD_TRM = 1 THEN LD_GRD_BEG = PMT_DUE;	*use the beginning of the first term calculated above for the first term of the gradation;
		ELSE LD_GRD_BEG = LD_GRD_END + 1;	*add a day to the end of the previous term to get the begin date of the next term;

		LD_GRD_END = INTNX('MONTH',LD_GRD_BEG - 1,1,'SAME'); *add a month (less a day) to the term begin date to get the term end date;

/*		once the current gradation is found, output the data and then move on to the next observation */
		IF TARG_DATE >= LD_GRD_BEG AND TARG_DATE <= LD_GRD_END THEN
			DO;
				OUTPUT;
				RETURN;
			END;
	END;
RUN;
