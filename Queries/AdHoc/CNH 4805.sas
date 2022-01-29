/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

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

PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID
						,LNXX.BF_SSN
						,LNXX.LN_SEQ
						,LNXX.LC_TYP_SCH_DIS
/*						,SUM(LNXX.LA_RPS_ISL) AS LA_RPS_ISL*/
						,SUM(LNXX.LA_CUR_PRI) + SUM(DWXX.WA_TOT_BRI_OTS) AS TOTAL_CURRENT_BALANCE
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.LNXX_LON_RPS LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LC_STA_LONXX = 'A'
							AND LNXX.LC_TYP_SCH_DIS IN ('CA','CL','CP','CQ','CX','CX','CX','IB','IL')
/*						INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXX*/
/*							ON LNXX.BF_SSN = LNXX.BF_SSN*/
/*							AND LNXX.LN_SEQ = LNXX.LN_SEQ*/
/*							AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ*/
						INNER JOIN PKUB.PDXX_PRS_NME PDXX
							ON LNXX.BF_SSN = PDXX.DF_PRS_ID
						INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON LNXX.BF_SSN = DWXX.BF_SSN
							AND LNXX.LN_SEQ = DWXX.LN_SEQ
/*					WHERE*/
/*						LNXX.LC_STA_LONXX = 'A'*/
/*						AND LNXX.LC_TYP_SCH_DIS IN ('CA','CL','CP','CQ','CX','CX','CX','IB','IL')*/
					GROUP BY
						PDXX.DF_SPE_ACC_ID
						,LNXX.BF_SSN
						,LNXX.LN_SEQ
						,LNXX.LC_TYP_SCH_DIS

					FOR READ ONLY WITH UR
				)
	;
/*get sample data to work with, returns a list of gradations*/

	CREATE TABLE PMT_GRDS AS
		SELECT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LC_TYP_SCH_DIS,
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
				AND RSXX.LC_STA_RPSTXX = 'A'
				AND LNXX.LC_STA_LONXX = 'A'
				AND LNXX.LC_TYP_SCH_DIS IN ('CA','CL','CP','CQ','CX','CX','CX','IB','IL')
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
	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMOX; SET LEGEND.DEMO; RUN;
DATA PMT_GRDS; SET LEGEND.PMT_GRDS; RUN;

/*identifies the current payment amount*/
DATA PMT_AMT (KEEP = BF_SSN LN_SEQ LA_RPS_ISL LC_TYP_SCH_DIS);
	SET PMT_GRDS;
	LENGTH PREV_LID $ XX;
	FORMAT LD_GRD_BEG LD_GRD_END DATEX.; *format dates so they're human readble;

	RETAIN PREV_LID '';
	RETAIN LD_GRD_END; *retain the end date to calculate the begin date of the next gradation if the next gradation is for the same loan as the previous gradation;

	TARG_DATE = TODAY(); *change this to look for a different target date;

/*	determine the beginning of the first term for the gradation each time a new gradation record is read in*/
	IF PREV_LID NE LID THEN  
		DO; *if the loan changed, the first term starts with the first payment due for the repayment schedule;
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

PROC SQL;
CREATE TABLE TOG AS
	SELECT DISTINCT
		D.DF_SPE_ACC_ID
		,D.LC_TYP_SCH_DIS
		,SUM(P.LA_RPS_ISL) AS TOT_PAYMENT
		,SUM(D.TOTAL_CURRENT_BALANCE) AS TOT_OUTSTANDING
	FROM 
		DEMOX D
		INNER JOIN PMT_AMT P
			ON D.BF_SSN = P.BF_SSN
			AND D.LN_SEQ = P.LN_SEQ
			AND D.LC_TYP_SCH_DIS = P.LC_TYP_SCH_DIS
	GROUP BY 
		D.DF_SPE_ACC_ID
		,D.LC_TYP_SCH_DIS
;
QUIT;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.TOG
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
