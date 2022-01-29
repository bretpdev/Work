DATA _NULL_;
/*for previous month use -X, for current use X, etc*/
RUN_MON = -X;
	CALL SYMPUT('BEGIN',put(INTNX('MONTH',TODAY(),RUN_MON,'B'),X.X));
	CALL SYMPUT('FINISH',put(INTNX('MONTH',TODAY(),RUN_MON,'E'),X.X));
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWCXX.NWCXXRZ";
FILENAME REPORTX "&RPTLIB/UNWCXX.NWCXXRX";
FILENAME REPORTX "&RPTLIB/UNWCXX.NWCXXRX";
FILENAME REPORTX "&RPTLIB/UNWCXX.NWCXXRX";

%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT FINISH = &FINISH;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK  ;
RSUBMIT legend;

/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;
LIBNAME PKUR DBX DATABASE=&DB OWNER=PKUR;
LIBNAME pkus DBX DATABASE=&DB OWNER=pkus;

PROC SQL;
	CREATE TABLE BKR AS
		SELECT DISTINCT
			D.DF_SPE_ACC_ID
			,coalesce(b.bf_ssn,a.df_prs_id) as bf_ssn
			,CASE
				WHEN A.DC_BKR_STA = 'XX' AND C.DD_BKR_STA > &FINISH THEN 'Suspended'
				WHEN A.DC_BKR_STA = 'XX' THEN 'Discharged'
				WHEN A.DC_BKR_STA = 'XX' THEN 'Suspended'
				ELSE ''
			END AS DISSED
			,A.DD_BKR_COR_X_RCV
			,C.DD_BKR_STA
			,A.DD_BKR_NTF
			,A.DD_BKR_FIL
			,CASE
				WHEN A.DC_BKR_STA = 'XX' AND C.DD_BKR_STA > &FINISH THEN 'XX'
				ELSE A.DC_BKR_STA
			END AS DC_BKR_STA
			,A.DC_BKR_TYP
			,A.DF_COU_DKT
			,intck('month',A.DD_BKR_COR_X_RCV,&finish) as num_mon
		FROM 
			(
				SELECT 
					DF_PRS_ID
					,MAX(DD_BKR_STA) AS DD_BKR_STA
					,SUBSTR(DF_COU_DKT,X,X) AS DF_COU_DKT
				FROM 
					PKUB.PDXX_PRS_BKR
				GROUP BY 
					DF_PRS_ID, 
					SUBSTR(DF_COU_DKT,X,X)
			)	C
			INNER JOIN pkub.PDXX_PRS_BKR A
				ON A.DF_PRS_ID = C.DF_PRS_ID
				AND A.DD_BKR_STA = C.DD_BKR_STA
				AND SUBSTR(A.DF_COU_DKT,X,X) = C.DF_COU_DKT
			LEFT OUTER JOIN pkub.LNXX_EDS B
				ON A.DF_PRS_ID = B.LF_EDS
			INNER JOIN PKUB.PDXX_PRS_NME D
				ON A.DF_PRS_ID = D.DF_PRS_ID
			JOIN PKUB.LNXX_LON LNXX
				ON D.DF_PRS_ID = LNXX.BF_SSN
			JOIN PKUB.DWXX_DW_CLC_CLU DWXX
				ON LNXX.BF_SSN = DWXX.BF_SSN
				AND LNXX.LN_SEQ = DWXX.LN_SEQ
		WHERE 
			(
				(A.DC_BKR_STA = 'XX' AND A.DD_BKR_NTF <= &FINISH)
				OR (A.DC_BKR_STA ='XX' AND A.DD_BKR_STA BETWEEN &BEGIN AND &FINISH AND A.DD_BKR_NTF <= &FINISH)
				OR (A.DC_BKR_STA ='XX' AND A.DD_BKR_STA > &FINISH AND A.DD_BKR_NTF <= &FINISH)
			)
			AND
			(
				DWXX.WC_DW_LON_STA ^= 'XX'
				OR LNXX.LD_PIF_RPT BETWEEN &BEGIN AND &FINISH
			)
	;


/*if there is an XX and XX with the same Case # and Status Date, only include the XX record*/
	CREATE TABLE ONLY_XX_RECORDS AS
		SELECT DISTINCT
			A.DF_SPE_ACC_ID
			,A.BF_SSN
			,A.DISSED
			,A.DD_BKR_COR_X_RCV
			,A.DD_BKR_STA
			,A.DD_BKR_NTF
			,A.DD_BKR_FIL
			,A.DC_BKR_STA
			,A.DC_BKR_TYP
			,A.DF_COU_DKT
			,A.num_mon
		FROM	
			BKR A
		WHERE	
			A.DC_BKR_STA = 'XX'
			OR NOT EXISTS
				(
					SELECT
						*
					FROM
						BKR B
					WHERE	
						B.DC_BKR_STA = 'XX'	
						AND A.DD_BKR_STA = B.DD_BKR_STA
						AND A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
						AND 
							(
							/*	If case number numeric digits and state characters are the same, this will be TRUE. If all case numbers have the state and the state (or the digits) is not the same, this will be FALSE*/
								A.DF_COU_DKT = B.DF_COU_DKT
								OR
							/*	If not all case numbers have the state, only use the case number digits in the comparison*/
								(
							/*		one or both of the case  numbers is missing the state*/
									(LENGTH(A.DF_COU_DKT) = X OR LENGTH(B.DF_COU_DKT) = X)
									AND
							/*		the numeric portion matches*/
									SUBSTR(A.DF_COU_DKT,X,X) = SUBSTR(B.DF_COU_DKT,X,X)
								)
							)
				)
	;

	CREATE TABLE BWRS AS
		SELECT DISTINCT
			BF_SSN
			,MAX(A.DD_BKR_FIL) AS DD_BKR_FIL
		FROM 
			ONLY_XX_RECORDS A
		GROUP BY 
			BF_SSN
	;

	CREATE TABLE DOL AS
		SELECT 
			DISTINCT A.*
			,COUNT(DISTINCT A.BF_SSN) AS BOR_NUM
			,COUNT(DISTINCT b.LN_SEQ) AS LON_NUM
			,SUM(coalesce(C.LA_OTS_PRI_ELG,X)) AS PBO
			,sum(C.la_nsi_acr) as IRB
			,CALCULATED PBO + CALCULATED IRB AS TOT
			,CASE
				WHEN B.IC_LON_PGM LIKE ('DL%') OR B.IC_LON_PGM IN ('TEACH') THEN 'RX'
				ELSE 'RX'
			END AS RNUM
		FROM 
			BWRS A
			INNER JOIN pkus.LNXX_LON B
				ON A.BF_SSN = B.BF_SSN
			left outer join pkus.LNXX_LON_MTH_BAL C
				ON B.BF_SSN = C.BF_SSN
				AND B.LN_SEQ = C.LN_SEQ
				and c.LC_STA_LONXX = 'A'
				AND C.LD_EFF_MTH_BAL = &finish
			left outer join pkub.LNXX_INT_RTE_HST D
				ON C.BF_SSN = D.BF_SSN
				AND C.LN_SEQ = D.LN_SEQ
				AND D.LD_ITR_EFF_BEG <= C.LD_EFF_MTH_BAL
				AND D.LD_ITR_EFF_END >= C.LD_EFF_MTH_BAL
				and d.lc_sta_lonXX = 'A'
		WHERE 
			b.ld_lon_X_dsb < A.DD_BKR_FIL
			AND B.LC_STA_LONXX = 'R'
			AND B.LC_STA_LONXX <> 'D'
		GROUP BY 
			A.BF_SSN, 
			RNUM
	;

	CREATE TABLE DEMO AS
		SELECT DISTINCT
			A.DF_SPE_ACC_ID
			,A.DISSED
			,A.DD_BKR_COR_X_RCV
			,A.DD_BKR_NTF
			,A.DC_BKR_STA
			,A.DD_BKR_STA
			,A.DC_BKR_TYP
			,A.NUM_MON
			,B.*
			,CASE
				WHEN RNUM = 'RX' THEN 'D'
				ELSE 'F'
			END AS PORTFOLIO
		FROM 
			ONLY_XX_RECORDS A
			INNER JOIN DOL B
				ON A.BF_SSN = B.BF_SSN
	;

QUIT;

ENDRSUBMIT;
DATA DEMO; SET LEGEND.DEMO; RUN;
DATA BKR; SET LEGEND.BKR; RUN;
DATA BWRS; SET LEGEND.BWRS; RUN;
DATA DOL; SET LEGEND.DOL; RUN;

/*add a record ID so a count of records can be calculated later*/
DATA DEMO;
	SET DEMO;
	RECORD_ID = _N_;
RUN;

data demo(DROP=I J);
set demo;
LENGTH AGE $XX.;
if num_mon <= X then age = '< X MONTHS';
else if num_mon <= XX then age = '> X MONTHS <= X YEAR';
else if num_mon <= XX then age = '> X YEAR <= X YEARS';
else if num_mon > XX then age = '> X YEARS';
ELSE DO;
	I = FLOOR(NUM_MON/XX);
	J = I + X;
	AGE = '> ' || TRIM(LEFT(put(I,X.X))) || ' YEARS <= ' || TRIM(LEFT(put(J,X.X))) || ' YEARS';
END;
RUN;

PROC SQL;
	CREATE TABLE SUMMARY AS
		SELECT
			PORTFOLIO
			,DC_BKR_TYP
			,AGE
			,COUNT(RECORD_ID) AS NO_BRWS
			,SUM(LON_NUM) AS NO_LOANS
			,SUM(TOT) AS DOLLARS
		FROM
			DEMO
		GROUP BY
			PORTFOLIO
			,DC_BKR_TYP
			,AGE
	;
QUIT;

%MACRO REP(R);
	DATA _NULL_;
		SET DEMO ;
		WHERE RNUM = "R&R";
		format 	DD_BKR_COR_X_RCV DD_BKR_STA mmddyyXX.;
		format pbo irb tot dollarXX.X;
		FILE REPORT&R DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
		IF _N_ = X THEN DO;
			PUT "ACCOUNT #,BANKRUPTCY TYPE,AGE,# MNTHS,BANKRUPTCY STATUS,RECVD DT,STATUS DATE,# LOANS,PBO,IRB,TOTAL";
		END;
		DO;
		   PUT DF_SPE_ACC_ID @;
		   PUT DC_BKR_TYP @;
		   PUT AGE @;
		   PUT NUM_MON @;
		   PUT DC_BKR_STA @;
		   PUT DD_BKR_COR_X_RCV @;
		   PUT DD_BKR_STA @;
		   PUT LON_NUM @;
		   PUT PBO @;
		   PUT IRB @;
		   PUT TOT $ ;
		END;
	RUN;
%MEND;
%REP(X);
%REP(X);

DATA _NULL_;
	SET SUMMARY ;
	FORMAT NO_BRWS NO_LOANS COMMAX.;
	FORMAT DOLLARS DOLLARXX.X;

	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	IF _N_ = X THEN DO;
		PUT "PORTFOLIO,CHAPTER TYPE,AGE TYPE,NO BORROWERS,NO LOANS,DOLLARS";
	END;
	DO;
	   PUT PORTFOLIO @;
	   PUT DC_BKR_TYP @;
	   PUT AGE @;
	   PUT NO_BRWS @;
	   PUT NO_LOANS @;
	   PUT DOLLARS $;
	END;
RUN;

/******** ORIGINAL CODE ABOVE ***********/



/******** TROUBLESHOOTING CODE BELOW ********/
PROC SQL;
	SELECT 
		DF_SPE_ACC_ID,
		PORTFOLIO,
		DC_BKR_TYP,
		AGE,
		NUM_MON
	FROM 
		DEMO
	WHERE
		AGE LIKE '%> X YEARS%'
	ORDER BY 
		NUM_MON
;QUIT;
