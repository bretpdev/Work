PROC IMPORT OUT= WORK.Source
            DATAFILE= "T:\Copy of CornerStone.xlsx" /*Replace with the actual file name*/
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;


LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
DATA LEGEND.SOURCE; *Send data to Duster;
SET SOURCE;
RUN;

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
					SELECT DISTINCT
						S.SSN,
						CASE
							WHEN AYXX.BF_SSN IS NOT NULL THEN X
							ELSE X
						END AS CALLED 
					FROM
						SOURCE S
						LEFT JOIN
						(
							SELECT DISTINCT
								BF_SSN
							FROM
								PKUB.AYXX_BR_LON_ATY AYXX
							WHERE
								AYXX.PF_REQ_ACT= 'PXXXC'
								AND AYXX.LD_ATY_REQ_RCV BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX',MMDDYYXX.)
						)AYXX
							ON INPUT(AYXX.BF_SSN, BESTXX.) = S.SSN

;
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;



/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH XXXX_OUTPUT.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
