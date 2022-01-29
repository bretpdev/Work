/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;

RSUBMIT;

%LET DB = DNFPUTDL; /*live*/
/*%LET DB = DLGSWQUT; /*test*/
LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%LET BEGINDATE = 'XX-XX-XXXX';
%LET ENDDATE = 'XX-XX-XXXX';

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

CREATE TABLE FORBEARANCE AS
	SELECT *
	FROM CONNECTION TO DBX 
		(
		SELECT DISTINCT
			PDXX.DM_PRS_X AS FIRST_NAME
			,PDXX.DM_PRS_LST AS LAST_NAME
			,LNXX.BF_SSN AS SSN
			,LNXX.IC_LON_PGM AS LOAN_TYPE
			,LNXX.LA_LON_AMT_GTR
			,LNXX.LD_TRM_BEG
			,LNXX.LD_TRM_END
			,DATE(FBXX.LD_FOR_REQ_BEG) AS LD_FOR_REQ_BEG
			,DATE(FBXX.LD_FOR_REQ_END) AS LD_FOR_REQ_END
			,LNXX.LC_STA_LONXX
			,FBXX.LC_FOR_STA
			,FBXX.LC_STA_FORXX
			,FBXX.LC_FOR_TYP

		FROM PKUB.LNXX_LON LNXX
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.LNXX_BR_FOR_APV LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
			INNER JOIN PKUB.FBXX_BR_FOR_REQ FBXX
				ON LNXX.BF_SSN = FBXX.BF_SSN
				AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
		WHERE 
			DAYS(FBXX.LD_FOR_REQ_END) - DAYS(FBXX.LD_FOR_REQ_BEG) <= XX
			AND LNXX.LC_STA_LONXX = 'A'
			AND FBXX.LC_FOR_STA = 'A'
			AND FBXX.LC_STA_FORXX = 'A'
			AND FBXX.LC_FOR_TYP = XX /*Collection Suspension*/
			AND 
			(
				DAYS(FBXX.LD_FOR_REQ_BEG) BETWEEN DAYS(&BEGINDATE) AND DAYS(&ENDDATE)
				AND DAYS(FBXX.LD_FOR_REQ_END) BETWEEN DAYS(&BEGINDATE) AND DAYS(&ENDDATE)
			)
		ORDER BY 
			LNXX.BF_SSN
		)
;

CREATE TABLE IDR_RENEW AS
	SELECT *
	FROM CONNECTION TO DBX 
		(
		SELECT DISTINCT
			PDXX.DM_PRS_X AS FIRST_NAME
			,PDXX.DM_PRS_LST AS LAST_NAME
			,LNXX.BF_SSN AS SSN
			,LNXX.IC_LON_PGM AS LOAN_TYPE
			,LNXX.LA_LON_AMT_GTR
			,LNXX.LC_TYP_SCH_DIS AS IDR_TYPE
			,RSXX.BD_ANV_QLF_IBR
			,LNXX.LD_TRM_BEG
			,LNXX.LD_TRM_END

		FROM PKUB.LNXX_LON LNXX
			INNER JOIN PKUB.LNXX_LON_RPS LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.RSXX_IBR_RPS RSXX
				ON LNXX.BF_SSN = RSXX.BF_SSN
		WHERE
			DAYS(RSXX.BD_ANV_QLF_IBR) BETWEEN DAYS(&BEGINDATE) AND DAYS(&ENDDATE)
			AND LNXX.LC_TYP_SCH_DIS IN 
			(
				'IB',/*IBR*/
				'IX',/*IBR XXXX*/
				'CA',/*PAYE*/
				'CX','CX','CX','CL',/*ICR*/
				'IA','IX','RE',/*REPAYE*/
				'IS'/*ISR*/
			)
			AND LNXX.LC_STA_LONXX = 'A'
			AND DAYS(LNXX.LD_CRT_LONXX) >= DAYS(RSXX.BD_ANV_QLF_IBR)
		ORDER BY 
			LNXX.BF_SSN
		)
;
DISCONNECT FROM DBX;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

ENDRSUBMIT;

DATA FORBEARANCE;
	SET LEGEND.FORBEARANCE;
RUN;

DATA IDR_RENEW;
	SET LEGEND.IDR_RENEW;
RUN;

PROC EXPORT
		DATA=FORBEARANCE
		OUTFILE="&RPTLIB\CNH XXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="Forbearance_XXdays";
RUN;

PROC EXPORT
		DATA=IDR_RENEW
		OUTFILE="&RPTLIB\CNH XXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="IDR_renewals";
RUN;
