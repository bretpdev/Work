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
						FSXX.LF_CON_LON_ORG	
					FROM		
						PKUB.FSXX_DL_LON FSXX	
						JOIN PKUB.PDXX_PRS_NME PDXX	
							ON FSXX.BF_SSN = PDXX.DF_PRS_ID
					WHERE		
						PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'	
						AND FSXX.LN_SEQ = X	
							
					FOR READ ONLY WITH UR		
				)			
	;						
							
	DISCONNECT FROM DBX;						
							
	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/						
	/*%SQLCHECK;*/						
QUIT;							
							
ENDRSUBMIT;							
							
DATA DEMO; SET LEGEND.DEMO; RUN;							
							
/*create printed report*/							
PROC PRINTTO PRINT=REPORTX NEW; RUN;							
							
PROC PRINTTO; RUN;							
							
/*export to Excel spreadsheet*/							
PROC EXPORT DATA = WORK.DEMO 							
            OUTFILE = "T:\SAS\FSXX_Disb_Adjustment.xls" 							
            DBMS = EXCEL							
			REPLACE;				
     SHEET="A"; 							
RUN;							
