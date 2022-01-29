%LET BEGINDATE = 'XX/XX/XXXX';
%LET RPTLIB = T:\SAS;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
%SYSLPUT BEGINDATE = &BEGINDATE;
RSUBMIT LEGEND;

%LET DB = DNFPUTDL;  *This is live;

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

PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN
						,LNXX.LN_SEQ
						,LNXX.LD_FAT_EFF AS DEFAULT_DATE
					FROM
						PKUB.LNXX_FIN_ATY LNXX
					WHERE
						LNXX.PC_FAT_TYP = 'XX'
						AND	LNXX.PC_FAT_SUB_TYP = 'XX'
						AND	LNXX.LC_FAT_REV_REA = ' '
						AND	DAYS(LNXX.LD_FAT_EFF) >= DAYS(&BEGINDATE)

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
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT 
	DATA = WORK.DEMO 
	OUTFILE = "&RPTLIB\NH XXXX.xlsx" 
	DBMS = EXCEL
	REPLACE;
	SHEET="CornerStone_default"; 
RUN;
