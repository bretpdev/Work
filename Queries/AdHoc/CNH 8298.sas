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
						CONCAT(CONCAT(PDXX.DM_PRS_X, ''), PDXX.DM_PRS_LST) AS Name,
						DFXX.LC_DFR_TYP,
						LNXX.LD_DFR_BEG,
						LNXX.LD_DFR_END
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.DFXX_BR_DFR_REQ DFXX
							ON DFXX.BF_SSN = PDXX.DF_PRS_ID
						INNER JOIN PKUB.LNXX_BR_DFR_APV LNXX
							ON DFXX.BF_SSN = LNXX.BF_SSN
							AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
					WHERE
						DFXX.LC_DFR_TYP IN('XX','XX')
						AND LNXX.LD_DFR_BEG <= 'XXXX-XX-XX' 
						AND LNXX.LD_DFR_END > 'XXXX-XX-XX'
						AND DFXX.LC_DFR_STA = 'A'
						AND DFXX.LC_STA_DFRXX = 'A'
						AND LNXX.LC_STA_LONXX = 'A'
						AND LNXX.LC_DFR_RSP <> 'XXX'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO;
Format LD_DFR_BEG LD_DFR_END mmddyyXX.;
RUN;



/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
