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
						PDXX.DF_PRS_ID AS BF_SSN
						,PDXX.DM_PRS_X
						,PDXX.DM_PRS_LST
						,DLQ.LN_DLQ_MAX
						,PDXX.DC_ADR
						,PDXX.DX_STR_ADR_X
						,PDXX.DX_STR_ADR_X
						,PDXX.DM_CT
						,PDXX.DC_DOM_ST
						,PDXX.DF_ZIP_CDE
						,PDXX.DC_PHN
						,PDXX.DN_DOM_PHN_ARA
						,PDXX.DN_DOM_PHN_XCH
						,PDXX.DN_DOM_PHN_LCL
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN (
								SELECT
									LNXX.BF_SSN
									,MX_DLQ.MAX_LN_DLQ_MAX AS LN_DLQ_MAX
								FROM PKUB.LNXX_LON_DLQ_HST LNXX
									JOIN (
											SELECT
												LNXX.BF_SSN
												,MAX(LNXX.LN_DLQ_MAX) AS MAX_LN_DLQ_MAX
											FROM 
												PKUB.LNXX_LON_DLQ_HST LNXX
												JOIN PKUB.LNXX_LON LNXX
													ON LNXX.BF_SSN = LNXX.BF_SSN
													AND LNXX.LN_SEQ = LNXX.LN_SEQ
													AND LNXX.LC_STA_LONXX = 'R'
													AND LNXX.LA_CUR_PRI > X
											WHERE
												LNXX.LC_STA_LONXX = 'X'
											GROUP BY
												LNXX.BF_SSN
											) MX_DLQ
										ON LNXX.BF_SSN = MX_DLQ.BF_SSN
									WHERE MX_DLQ.MAX_LN_DLQ_MAX BETWEEN XX AND XXX
								) DLQ
							ON PDXX.DF_PRS_ID = DLQ.BF_SSN
						LEFT JOIN PKUB.PDXX_PRS_ADR PDXX
							ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
							AND PDXX.DI_VLD_ADR = 'N'
						LEFT JOIN PKUB.PDXX_PRS_PHN PDXX
							ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
							AND PDXX.DI_PHN_VLD = 'N'
						LEFT JOIN PKUB.PDXX_PRS_ADR PDXXY
							ON PDXX.DF_PRS_ID = PDXXY.DF_PRS_ID
							AND PDXXY.DI_VLD_ADR = 'Y'
						LEFT JOIN PKUB.PDXX_PRS_PHN PDXXY
							ON PDXX.DF_PRS_ID = PDXXY.DF_PRS_ID
							AND PDXXY.DI_PHN_VLD = 'Y'
					WHERE
						(PDXXY.DF_PRS_ID IS NULL OR PDXXY.DF_PRS_ID IS NULL)

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\CNH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
