%LET RPTLIB = T:\SAS;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ; /*test*/
RSUBMIT;

%LET DB = DLGSUTWH; /*live*/
/*%LET DB = DLGSWQUT; /*test*/

/*report-level variables*/
%LET BEGINDATE = '10-01-2012';
%LET ENDDATE = '09-30-2015';

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
CONNECT TO DB2 (DATABASE=&DB);

CREATE TABLE FORBEARANCE AS
	SELECT *
	FROM CONNECTION TO DB2 
		(
		SELECT DISTINCT
			PD10.DM_PRS_1 AS FIRST_NAME
			,PD10.DM_PRS_LST AS LAST_NAME
			,LN10.BF_SSN AS SSN
			,LN10.IC_LON_PGM AS LOAN_TYPE
			,LN10.LD_LON_GTR AS GUARANTY_DATE
			,LN10.LA_LON_AMT_GTR AS GUARANTY_AMOUNT
			,LN10.LD_TRM_BEG
			,LN10.LD_TRM_END
			,DATE(FB10.LD_FOR_REQ_BEG) AS LD_FOR_REQ_BEG
			,DATE(FB10.LD_FOR_REQ_END) AS LD_FOR_REQ_END
			,LN60.LC_STA_LON60
			,FB10.LC_FOR_STA
			,FB10.LC_STA_FOR10
			,FB10.LC_FOR_TYP

		FROM OLWHRM1.LN10_LON LN10
			INNER JOIN OLWHRM1.PD10_PRS_NME PD10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN OLWHRM1.LN60_BR_FOR_APV LN60
				ON LN10.BF_SSN = LN60.BF_SSN
				AND LN10.LN_SEQ = LN60.LN_SEQ
			INNER JOIN OLWHRM1.FB10_BR_FOR_REQ FB10
				ON LN10.BF_SSN = FB10.BF_SSN
				AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
		WHERE 
			DAYS(FB10.LD_FOR_REQ_END) - DAYS(FB10.LD_FOR_REQ_BEG) <= 60
			AND LN60.LC_STA_LON60 = 'A'
			AND FB10.LC_FOR_STA = 'A'
			AND FB10.LC_STA_FOR10 = 'A'
			AND 
			(
				DAYS(FB10.LD_FOR_REQ_BEG) BETWEEN DAYS(&BEGINDATE) AND DAYS(&ENDDATE)
				AND DAYS(FB10.LD_FOR_REQ_END) BETWEEN DAYS(&BEGINDATE) AND DAYS(&ENDDATE)
			)
			AND FB10.LC_FOR_TYP IN 
			(
				'02','04','09','10','13','14','15','16','18',
				'20','21','25','28','34','35','41','42'
			)
		ORDER BY 
			LN10.BF_SSN
		)
;

CREATE TABLE IDR_RENEW AS
	SELECT *
	FROM CONNECTION TO DB2 
		(
		SELECT DISTINCT
			PD10.DM_PRS_1 AS FIRST_NAME
			,PD10.DM_PRS_LST AS LAST_NAME
			,LN10.BF_SSN AS SSN
			,LN10.IC_LON_PGM AS LOAN_TYPE
			,LN10.LD_LON_GTR AS GUARANTY_DATE
			,LN10.LA_LON_AMT_GTR AS GUARANTY_AMOUNT
			,LN65.LC_TYP_SCH_DIS AS IDR_TYPE
			,RS05.BD_ANV_QLF_IBR
			,LN10.LD_TRM_BEG
			,LN10.LD_TRM_END

		FROM OLWHRM1.LN10_LON LN10
			INNER JOIN OLWHRM1.LN65_LON_RPS LN65
				ON LN10.BF_SSN = LN65.BF_SSN
				AND LN10.LN_SEQ = LN65.LN_SEQ
			INNER JOIN OLWHRM1.PD10_PRS_NME PD10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN OLWHRM1.RS05_IBR_RPS RS05
				ON LN10.BF_SSN = RS05.BF_SSN
		WHERE
			DAYS(RS05.BD_ANV_QLF_IBR) BETWEEN DAYS(&BEGINDATE) AND DAYS(&ENDDATE)
			AND LN65.LC_TYP_SCH_DIS IN 
			(
				'IB',/*IBR*/
				'I3',/*IBR2014*/
				'CA',/*PAYE*/
				'C1','C2','C3','CL',/*ICR*/
				'IA','I5','RE',/*REPAYE*/
				'IS'/*RE ISR*/
			)
			AND LN65.LC_STA_LON65 = 'A'
			AND DAYS(LN65.LD_CRT_LON65) >= DAYS(RS05.BD_ANV_QLF_IBR)
		ORDER BY 
			LN10.BF_SSN
		)
;
DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

ENDRSUBMIT;

DATA FORBEARANCE;
	SET DUSTER.FORBEARANCE;
RUN;

DATA IDR_RENEW;
	SET DUSTER.IDR_RENEW;
RUN;

PROC EXPORT
		DATA=FORBEARANCE
		OUTFILE="&RPTLIB\UNH 26503.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="Forbearance_60days";
RUN;

PROC EXPORT
		DATA=IDR_RENEW
		OUTFILE="&RPTLIB\UNH 26503.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="IDR_renewals";
RUN;
