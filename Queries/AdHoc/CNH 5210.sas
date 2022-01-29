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
/*						COUNT(FBXX.LF_FOR_CTL_NUM)*/
/*						,COUNT(RPS_CHG.LF_FOR_CTL_NUM)*/
						FBXX.BF_SSN
						,FBXX.LF_FOR_CTL_NUM
						,RPS_CHG.BF_SSN AS REP_BF_SSN
						,RPS_CHG.LF_FOR_CTL_NUM AS REP_LF_FOR_CTL_NUM
					FROM
						PKUB.FBXX_BR_FOR_REQ FBXX
						INNER JOIN PKUB.LNXX_BR_FOR_APV LNXX	
							ON FBXX.BF_SSN = LNXX.BF_SSN
							AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
/*							AND FBXX.LN_FOR_OCC_SEQ = LNXX.LN_FOR_OCC_SEQ*/
						INNER JOIN PKUB.LNXX_LON LNXX
							ON FBXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LD_LON_EFF_ADD < LNXX.LD_FOR_BEG
/*						INNER JOIN PKUB.PDXX_PRS_ADR PDXX*/
/*							ON FBXX.BF_SSN = PDXX.DF_PRS_ID*/
/*							AND PDXX.DI_VLD_ADR != 'Y'*/
							LEFT JOIN (
									SELECT DISTINCT
										FBXX.BF_SSN
										,FBXX.LF_FOR_CTL_NUM
									FROM
										PKUB.FBXX_BR_FOR_REQ FBXX
										INNER JOIN PKUB.LNXX_BR_FOR_APV LNXX	
											ON FBXX.BF_SSN = LNXX.BF_SSN
											AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
				/*							AND FBXX.LN_FOR_OCC_SEQ = LNXX.LN_FOR_OCC_SEQ*/
										INNER JOIN PKUB.LNXX_LON LNXX
											ON FBXX.BF_SSN = LNXX.BF_SSN
											AND LNXX.LN_SEQ = LNXX.LN_SEQ
											AND LNXX.LD_LON_EFF_ADD < LNXX.LD_FOR_BEG
/*										INNER JOIN PKUB.PDXX_PRS_ADR PDXX*/
/*											ON FBXX.BF_SSN = PDXX.DF_PRS_ID*/
/*											AND PDXX.DI_VLD_ADR != 'Y'*/
										INNER JOIN PKUB.AYXX_BR_LON_ATY AYXX
											ON FBXX.BF_SSN = AYXX.BF_SSN
											AND AYXX.PF_REQ_ACT = 'GXXXE'
/*											AND DAYS(AYXX.LD_STA_ACTYXX) BETWEEN DAYS(LNXX.LD_FOR_END) - XX AND DAYS(LNXX.LD_FOR_END) + XX*/
									WHERE
										FBXX.LC_FOR_TYP IN ('XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX')
										AND LNXX.LD_FOR_BEG BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'						
										AND FBXX.LC_STA_FORXX = 'A'
										AND LNXX.LC_STA_LONXX = 'A'
										AND DAYS(AYXX.LD_STA_ACTYXX) BETWEEN DAYS(LNXX.LD_FOR_END) - XX AND DAYS(LNXX.LD_FOR_END) + XX
										) RPS_CHG
								ON FBXX.BF_SSN = RPS_CHG.BF_SSN
								AND FBXX.LF_FOR_CTL_NUM = RPS_CHG.LF_FOR_CTL_NUM
					WHERE
						FBXX.LC_FOR_TYP IN ('XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX')
						AND LNXX.LD_FOR_BEG BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
						AND FBXX.LC_STA_FORXX = 'A'
						AND LNXX.LC_STA_LONXX = 'A'

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
            OUTFILE = "Y:\Development\SAS Test Files\Riley\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
