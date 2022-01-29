PROC IMPORT OUT= WORK.SAUCEX 
             DATAFILE= "T:\Populations.xlsx"  
             DBMS=EXCEL REPLACE; 
      RANGE="SheetX$";  
      GETNAMES=YES; 
      MIXED=NO; 
      SCANTEXT=YES; 
      USEDATE=YES; 
      SCANTIME=YES; 
 RUN; 

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.SAUCE;
	SET SAUCEX;
RUN;


RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);
	CREATE TABLE DEMO AS
		SELECT	
			WQXX.WF_QUE
			,WQXX.WF_SUB_QUE
			,WQXX.WN_CTL_TSK
			,WQXX.PF_REQ_ACT
			,WQXX.BF_SSN
		FROM
			PKUB.WQXX_TSK_QUE WQXX
			LEFT JOIN SAUCE S
				ON input(WQXX.BF_SSN,bestXX.) = S.BF_SSN
		WHERE
			WQXX.WF_QUE = 'RX'	
			AND
			S.BF_SSN IS NULL
	;

	DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX VX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

