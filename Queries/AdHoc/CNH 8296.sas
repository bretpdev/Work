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
					SELECT
						PDXX.DF_SPE_ACC_ID,
						CONCAT(CONCAT(PDXX.DM_PRS_X, ''), PDXX.DM_PRS_LST) AS Name,
						LNXX.LN_DLQ_MAX,
						AYXX.PF_REQ_ACT,
						LNXX.LD_DSB
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_LON LNXX
							ON LNXX.BF_SSN = PDXX.DF_PRS_ID
						INNER JOIN PKUB.LNXX_LON_DLQ_HST LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LC_STA_LONXX = X
							AND LNXX.LD_STA_LONXX = 'XXXX-XX-XX'
						INNER JOIN 
							(
								SELECT 
									LNXXI.BF_SSN,
									LNXXI.LN_SEQ,
									MAX(LNXXI.LD_DSB) + X DAY AS LD_DSB
								FROM 
									PKUB.LNXX_DSB LNXXI
								GROUP BY
									LNXXI.BF_SSN,
									LNXXI.LN_SEQ
							) LNXX ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.AYXX_BR_LON_ATY AYXX
							ON AYXX.BF_SSN = LNXX.BF_SSN
					WHERE
						LNXX.IC_LON_PGM = 'DLPLUS'
						AND AYXX.LD_ATY_REQ_RCV >= 'XXXX-XX-XX'
						AND AYXX.PF_REQ_ACT IN('IBDNX','IBDNX','IBDNX','IBDNX','IBDNX','IBDNX','IBDNX','IBDNY','IBDNY','ICDNY','PEDNY','IDRDN' )

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO;
Format LD_DSB mmddyyXX.;
RUN;



/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
