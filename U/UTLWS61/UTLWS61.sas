/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWS61.LWS61RZ";
FILENAME REPORT2 "&RPTLIB/ULWS61.LWS61R2";
FILENAME REPORT3 "&RPTLIB/ULWS61.LWS61R3";

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;

/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

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

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

/*	get borrowers meeting main query criteria*/
	CREATE TABLE MAIN AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT	
						DISTINCT
						PD10.DF_SPE_ACC_ID
						,LN10.BF_SSN
						,DF10.LD_DFR_REQ_BEG
						,DF10.LD_DFR_REQ_END
						,DF10.LC_DFR_STA
						,DF10.LC_STA_DFR10
					FROM
						OLWHRM1.PD10_PRS_NME PD10
						INNER JOIN OLWHRM1.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						INNER JOIN OLWHRM1.DF10_BR_DFR_REQ DF10
							ON PD10.DF_PRS_ID = DF10.BF_SSN
							AND DF10.LC_DFR_TYP = '38'
							AND DF10.LC_DFR_STA = 'A'
							AND DF10.LC_STA_DFR10 = 'A'
						INNER JOIN OLWHRM1.LN50_BR_DFR_APV LN50
							ON DF10.BF_SSN = LN50.BF_SSN
							AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
							AND LN50.LC_STA_LON50 = 'A'
							AND DAYS(CURRENT_DATE) BETWEEN DAYS(LN50.LD_DFR_BEG) AND DAYS(LN50.LD_DFR_END)
					WHERE
						LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI > 0

					FOR READ ONLY WITH UR
				)
	;

/*	get borrowers with MILRV ARC to be used to create MILRV365 and MILRV730 tables*/
	CREATE TABLE MILRV AS
		SELECT
			*
		FROM
			CONNECTION TO DB2
				(
					SELECT
						BF_SSN
						,LD_ATY_REQ_RCV
					FROM
						OLWHRM1.AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = 'MILRV'
				)
	;

/*	get borrowers who are in main query, have MILRV ARC, and meet 365 criteria*/
	CREATE TABLE MILRV365 AS
		SELECT
			DISTINCT
			N.DF_SPE_ACC_ID
		FROM
			MAIN N
			LEFT OUTER JOIN MILRV V
				ON N.BF_SSN = V.BF_SSN			
		WHERE
			N.LD_DFR_REQ_END - N.LD_DFR_REQ_BEG > 365
			AND TODAY() - N.LD_DFR_REQ_BEG >= 365
			AND (V.LD_ATY_REQ_RCV IS NULL OR V.LD_ATY_REQ_RCV < N.LD_DFR_REQ_BEG + 365)
	;

/*	get borrowers who are in main query, have MILRV ARC, and meet 730 criteria*/
	CREATE TABLE MILRV730 AS
		SELECT
			DISTINCT
			N.DF_SPE_ACC_ID
		FROM
			MAIN N
			LEFT OUTER JOIN MILRV V
				ON N.BF_SSN = V.BF_SSN			
		WHERE
			N.LD_DFR_REQ_END - N.LD_DFR_REQ_BEG > 365
			AND TODAY() - N.LD_DFR_REQ_BEG >= 730
			AND (V.LD_ATY_REQ_RCV IS NULL OR V.LD_ATY_REQ_RCV < N.LD_DFR_REQ_BEG + 730)
	;

/*	get borrowers who meet initial criteria for R3*/
	CREATE TABLE MILRN AS
		SELECT
			*
		FROM
			CONNECTION TO DB2
				(
					SELECT
						DISTINCT
						LN65.BF_SSN
						,AY10.LN_ATY_SEQ
						,AY10.LD_ATY_REQ_RCV
						,RS10.LD_RPS_1_PAY_DU
					FROM
						OLWHRM1.LN65_LON_RPS LN65
						INNER JOIN OLWHRM1.RS10_BR_RPD RS10
							ON LN65.BF_SSN = RS10.BF_SSN
							AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
						LEFT OUTER JOIN OLWHRM1.AY10_BR_LON_ATY AY10
							ON LN65.BF_SSN = AY10.BF_SSN
							AND AY10.PF_REQ_ACT = 'MILRN'
					WHERE
						LN65.LC_TYP_SCH_DIS IN ('IB')
						AND DAYS(CURRENT_DATE) - DAYS(RS10.LD_RPS_1_PAY_DU) >= 330
				)
	;

/*	get max MILRN ARC date for borrowers who meet initial criteria for R3*/
	CREATE TABLE MILRN_MAX AS
		SELECT
			BF_SSN
			,MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
		FROM
			MILRN
		WHERE
			LD_ATY_REQ_RCV IS NOT NULL
		GROUP BY
			BF_SSN
	;

/*	get count of MILRN ARCs for borrowers who meet initial criteria for R3*/
	CREATE TABLE MILRN_CNT AS
		SELECT
			BF_SSN
			,COUNT(DISTINCT LN_ATY_SEQ) AS CNT
		FROM
			MILRN
		WHERE
			LN_ATY_SEQ IS NOT NULL
		GROUP BY
			BF_SSN
	;

/*	get borrowers who meet main query criteria and all criteria for R3*/
	CREATE TABLE R3 AS
		SELECT
			I.DF_SPE_ACC_ID
		FROM
			MAIN I
			INNER JOIN MILRN N
				ON I.BF_SSN = N.BF_SSN
			LEFT OUTER JOIN MILRN_MAX M
				ON N.BF_SSN = M.BF_SSN
			LEFT OUTER JOIN MILRN_CNT C
				ON N.BF_SSN = C.BF_SSN
		WHERE
			C.CNT < 3
			AND M.LD_ATY_REQ_RCV < N.LD_RPS_1_PAY_DU + 330
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

/*union data sets of borrowers for R2*/
DATA R2;
	SET MILRV365 MILRV730;
RUN;

ENDRSUBMIT;
DATA R2; SET DUSTER.R2; RUN;
DATA R3; SET DUSTER.R3; RUN;

/*export to queue builder (fed) file*/
DATA _NULL_;
	SET R2 ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	PUT DF_SPE_ACC_ID 'MILRV,,,,,,,ALL,Military Deferment eligibility review required' ;
RUN;

DATA _NULL_;
	SET R3 ;
	FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;
	PUT DF_SPE_ACC_ID 'MILRN,,,,,,,ALL,Military IDR extension review required' ;
RUN;
