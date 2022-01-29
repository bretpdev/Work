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
						LNXX.LD_LON_ACL_ADD
						,DWXX.WC_DW_LON_STA
						,PDXX.DM_PRS_X
						,PDXX.DM_PRS_LST
						,LNXX.IC_LON_PGM
						,PDXX.DF_SPE_ACC_ID
/*						,LNXX.LN_SEQ*/
/*						,COUNT(LNXX.LN_SEQ) AS CNT*/
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON PDXX.DF_PRS_ID = DWXX.BF_SSN
							AND LNXX.LN_SEQ = DWXX.LN_SEQ
					WHERE
						LNXX.LD_LON_ACL_ADD > 'XX/XX/XXXX'
/*					GROUP BY */
/*						LNXX.LD_LON_ACL_ADD*/
/*						,DWXX.WC_DW_LON_STA*/
/*						,PDXX.DM_PRS_X*/
/*						,PDXX.DM_PRS_LST*/
/*						,LNXX.IC_LON_PGM*/
/*						,PDXX.DF_SPE_ACC_ID*/
/*						,LNXX.LN_SEQ*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMOX; SET LEGEND.DEMO; RUN;

PROC SORT DATA=DEMOX; BY LD_LON_ACL_ADD WC_DW_LON_STA; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMOX
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
