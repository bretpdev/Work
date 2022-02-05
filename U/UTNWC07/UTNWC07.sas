DATA _NULL_;
/*for previous month use -1, for current use 0, etc*/
RUN_MON = -1;
	CALL SYMPUT('BEGIN',put(INTNX('MONTH',TODAY(),RUN_MON,'B'),5.0));
	CALL SYMPUT('FINISH',put(INTNX('MONTH',TODAY(),RUN_MON,'E'),5.0));
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWC07.NWC07RZ";
FILENAME REPORT2 "&RPTLIB/UNWC07.NWC07R2";
FILENAME REPORT3 "&RPTLIB/UNWC07.NWC07R3";
FILENAME REPORT4 "&RPTLIB/UNWC07.NWC07R4";

%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT FINISH = &FINISH;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK  ;
RSUBMIT legend;

/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;
LIBNAME PKUR DB2 DATABASE=&DB OWNER=PKUR;
LIBNAME pkus DB2 DATABASE=&DB OWNER=pkus;

PROC SQL;
	CREATE TABLE BKR AS
		SELECT DISTINCT
			D.DF_SPE_ACC_ID
			,coalesce(b.bf_ssn,a.df_prs_id) as bf_ssn
			,CASE
				WHEN A.DC_BKR_STA = '05' AND C.DD_BKR_STA > &FINISH THEN 'Suspended'
				WHEN A.DC_BKR_STA = '05' THEN 'Discharged'
				WHEN A.DC_BKR_STA = '06' THEN 'Suspended'
				ELSE ''
			END AS DISSED
			,A.DD_BKR_COR_1_RCV
			,C.DD_BKR_STA
			,A.DD_BKR_NTF
			,A.DD_BKR_FIL
			,CASE
				WHEN A.DC_BKR_STA = '05' AND C.DD_BKR_STA > &FINISH THEN '06'
				ELSE A.DC_BKR_STA
			END AS DC_BKR_STA
			,A.DC_BKR_TYP
			,A.DF_COU_DKT
			,intck('month',A.DD_BKR_COR_1_RCV,&finish) as num_mon
		FROM 
			(
				SELECT 
					DF_PRS_ID
					,MAX(DD_BKR_STA) AS DD_BKR_STA
					,SUBSTR(DF_COU_DKT,1,8) AS DF_COU_DKT
				FROM 
					PKUB.PD24_PRS_BKR
				GROUP BY 
					DF_PRS_ID, 
					SUBSTR(DF_COU_DKT,1,8)
			)	C
			INNER JOIN pkub.PD24_PRS_BKR A
				ON A.DF_PRS_ID = C.DF_PRS_ID
				AND A.DD_BKR_STA = C.DD_BKR_STA
				AND SUBSTR(A.DF_COU_DKT,1,8) = C.DF_COU_DKT
			LEFT OUTER JOIN pkub.LN20_EDS B
				ON A.DF_PRS_ID = B.LF_EDS
			INNER JOIN PKUB.PD10_PRS_NME D
				ON A.DF_PRS_ID = D.DF_PRS_ID
			JOIN PKUB.LN10_LON LN10
				ON D.DF_PRS_ID = LN10.BF_SSN
			JOIN PKUB.DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
		WHERE 
			(
				(A.DC_BKR_STA = '06' AND A.DD_BKR_NTF <= &FINISH)
				OR (A.DC_BKR_STA ='05' AND A.DD_BKR_STA BETWEEN &BEGIN AND &FINISH AND A.DD_BKR_NTF <= &FINISH)
				OR (A.DC_BKR_STA ='05' AND A.DD_BKR_STA > &FINISH AND A.DD_BKR_NTF <= &FINISH)
			)
			AND
			(
				DW01.WC_DW_LON_STA ^= '22'
				OR LN10.LD_PIF_RPT BETWEEN &BEGIN AND &FINISH
			)
	;


/*if there is an 05 and 06 with the same Case # and Status Date, only include the 06 record*/
	CREATE TABLE ONLY_06_RECORDS AS
		SELECT DISTINCT
			A.DF_SPE_ACC_ID
			,A.BF_SSN
			,A.DISSED
			,A.DD_BKR_COR_1_RCV
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
			A.DC_BKR_STA = '06'
			OR NOT EXISTS
				(
					SELECT
						*
					FROM
						BKR B
					WHERE	
						B.DC_BKR_STA = '06'	
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
									(LENGTH(A.DF_COU_DKT) = 8 OR LENGTH(B.DF_COU_DKT) = 8)
									AND
							/*		the numeric portion matches*/
									SUBSTR(A.DF_COU_DKT,1,8) = SUBSTR(B.DF_COU_DKT,1,8)
								)
							)
				)
	;

	CREATE TABLE BWRS AS
		SELECT DISTINCT
			BF_SSN
			,MAX(A.DD_BKR_FIL) AS DD_BKR_FIL
		FROM 
			ONLY_06_RECORDS A
		GROUP BY 
			BF_SSN
	;

	CREATE TABLE DOL AS
		SELECT 
			DISTINCT A.*
			,COUNT(DISTINCT A.BF_SSN) AS BOR_NUM
			,COUNT(DISTINCT b.LN_SEQ) AS LON_NUM
			,SUM(coalesce(C.LA_OTS_PRI_ELG,0)) AS PBO
			,sum(C.la_nsi_acr) as IRB
			,CALCULATED PBO + CALCULATED IRB AS TOT
			,CASE
				WHEN B.IC_LON_PGM LIKE ('DL%') OR B.IC_LON_PGM IN ('TEACH') THEN 'R2'
				ELSE 'R3'
			END AS RNUM
		FROM 
			BWRS A
			INNER JOIN pkus.LN10_LON B
				ON A.BF_SSN = B.BF_SSN
			left outer join pkus.LN37_LON_MTH_BAL C
				ON B.BF_SSN = C.BF_SSN
				AND B.LN_SEQ = C.LN_SEQ
				and c.LC_STA_LON37 = 'A'
				AND C.LD_EFF_MTH_BAL = &finish
			left outer join pkub.LN72_INT_RTE_HST D
				ON C.BF_SSN = D.BF_SSN
				AND C.LN_SEQ = D.LN_SEQ
				AND D.LD_ITR_EFF_BEG <= C.LD_EFF_MTH_BAL
				AND D.LD_ITR_EFF_END >= C.LD_EFF_MTH_BAL
				and d.lc_sta_lon72 = 'A'
		WHERE 
			b.ld_lon_1_dsb < A.DD_BKR_FIL
			AND B.LC_STA_LON10 = 'R'
			AND B.LC_STA_LON10 <> 'D'
		GROUP BY 
			A.BF_SSN, 
			RNUM
	;

	CREATE TABLE DEMO AS
		SELECT DISTINCT
			A.DF_SPE_ACC_ID
			,A.DISSED
			,A.DD_BKR_COR_1_RCV
			,A.DD_BKR_NTF
			,A.DC_BKR_STA
			,A.DD_BKR_STA
			,A.DC_BKR_TYP
			,A.NUM_MON
			,B.*
			,CASE
				WHEN RNUM = 'R2' THEN 'D'
				ELSE 'F'
			END AS PORTFOLIO
		FROM 
			ONLY_06_RECORDS A
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
LENGTH AGE $21.;
if num_mon <= 6 then age = '< 6 MONTHS';
else if num_mon <= 12 then age = '> 6 MONTHS <= 1 YEAR';
else if num_mon <= 24 then age = '> 1 YEAR <= 2 YEARS';
else if num_mon > 60 then age = '> 5 YEARS';
ELSE DO;
	I = FLOOR(NUM_MON/12);
	J = I + 1;
	AGE = '> ' || TRIM(LEFT(put(I,3.0))) || ' YEARS <= ' || TRIM(LEFT(put(J,3.0))) || ' YEARS';
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
		format 	DD_BKR_COR_1_RCV DD_BKR_STA mmddyy10.;
		format pbo irb tot dollar18.2;
		FILE REPORT&R DELIMITER=',' DSD DROPOVER LRECL=32767;
		IF _N_ = 1 THEN DO;
			PUT "ACCOUNT #,BANKRUPTCY TYPE,AGE,# MNTHS,BANKRUPTCY STATUS,RECVD DT,STATUS DATE,# LOANS,PBO,IRB,TOTAL";
		END;
		DO;
		   PUT DF_SPE_ACC_ID @;
		   PUT DC_BKR_TYP @;
		   PUT AGE @;
		   PUT NUM_MON @;
		   PUT DC_BKR_STA @;
		   PUT DD_BKR_COR_1_RCV @;
		   PUT DD_BKR_STA @;
		   PUT LON_NUM @;
		   PUT PBO @;
		   PUT IRB @;
		   PUT TOT $ ;
		END;
	RUN;
%MEND;
%REP(2);
%REP(3);

DATA _NULL_;
	SET SUMMARY ;
	FORMAT NO_BRWS NO_LOANS COMMA8.;
	FORMAT DOLLARS DOLLAR18.2;

	FILE REPORT4 DELIMITER=',' DSD DROPOVER LRECL=32767;
	IF _N_ = 1 THEN DO;
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
