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

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID,
						SCRA_ARC.LN_ATY_SEQ as scra_seq,
						SCRA_ARC.ARC_DATE as scra_date,
						INVALID_SCRA_ARC.LN_ATY_SEQ as Invalid_scra_seq,
						INVALID_SCRA_ARC.ARC_DATE as Invalid_scra_date,
						CASE
							WHEN INVALID_SCRA_ARC.BF_SSN IS NULL THEN 'Y'
							WHEN INVALID_SCRA_ARC.ARC_DATE > SCRA_ARC.ARC_DATE THEN 'N'
							WHEN INVALID_SCRA_ARC.ARC_DATE < SCRA_ARC.ARC_DATE THEN 'Y'
							WHEN INVALID_SCRA_ARC.ARC_DATE = SCRA_ARC.ARC_DATE AND INVALID_SCRA_ARC.LN_ATY_SEQ > SCRA_ARC.LN_ATY_SEQ THEN 'N'
							WHEN INVALID_SCRA_ARC.ARC_DATE = SCRA_ARC.ARC_DATE AND INVALID_SCRA_ARC.LN_ATY_SEQ < SCRA_ARC.LN_ATY_SEQ THEN 'Y'
							ELSE 'X'
						END AS IS_ELIGIBLE
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON DWXX.BF_SSN = LNXX.BF_SSN
							AND DWXX.LN_SEQ = LNXX.LN_SEQ
						LEFT JOIN
						(
							SELECT
								BF_SSN,
								LN_ATY_SEQ,
								MAX(LD_ATY_REQ_RCV) AS ARC_DATE
							FROM 
								PKUB.AYXX_BR_LON_ATY
							WHERE
								PF_REQ_ACT = 'ASCRA'
							GROUP BY
								BF_SSN,
								LN_ATY_SEQ
						) SCRA_ARC
							ON SCRA_ARC.BF_SSN = LNXX.BF_SSN
						LEFT JOIN
						(
							SELECT
								BF_SSN,
								LN_ATY_SEQ,
								MAX(LD_ATY_REQ_RCV) AS ARC_DATE
							FROM 
								PKUB.AYXX_BR_LON_ATY
							WHERE
								PF_REQ_ACT = 'ISCRA'
							GROUP BY
								BF_SSN,
								LN_ATY_SEQ
						) INVALID_SCRA_ARC
							ON INVALID_SCRA_ARC.BF_SSN = LNXX.BF_SSN
					WHERE
						DWXX.WC_DW_LON_STA = 'XX'
						AND SCRA_ARC.BF_SSN IS NOT NULL


					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO;
	SET LEGEND.DEMO; 
	WHERE IS_ELIGIBLE = 'Y';
RUN;


/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\MILITARY SCRA BORROWERS THAT RECEIVED A DEATH DISCHARGE.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
