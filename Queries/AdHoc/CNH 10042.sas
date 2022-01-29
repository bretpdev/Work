PROC IMPORT 
	OUT= WORK.LionTeam 
	DATAFILE= "C:\Users\jlwright\Desktop\Lion Team ARC Add List.xlsx"
	DBMS= xlsx
	REPLACE;
	GETNAMES=YES;
RUN;

%INCLUDE "Z:\Codebase\SAS\ArcAdd Common.sas";

LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no"; 

DATA LEGEND.LionTeam; SET LionTeam; RUN;

RSUBMIT;
/*%LET DB = DNFPRQUT;  *This is test;*/
/*%LET DB = DNFPRUUT;  *This is VUKX test;*/
%LET DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL;
CREATE TABLE ArcData AS
SELECT
	PDXX.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	'' AS COMMENT,
	'' AS RECIPIENT_ID,
	X AS IS_REFERENCE,
	X AS IS_ENDORSER,
	. AS PROCESS_FROM,
	. AS PROCESS_TO,
	. AS NEEDED_BY,
	'' AS REGARDS_TO,
	'' AS REGARDS_CODE,
	'DCR' AS CREATED_BY,
	X AS ARC_TYPE,
	'BLADJ' AS ARCTOADD,
	'DCR' AS SAS_ID
FROM
	LionTeam L 
	INNER JOIN PKUB.PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = L.BF_SSN
;

CREATE TABLE Loans AS
SELECT
	PDXX.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	L.LN_SEQ
FROM
	LionTeam L 
	INNER JOIN PKUB.PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = L.BF_SSN
;


QUIT;

ENDRSUBMIT;

DATA ArcData; SET LEGEND.ArcData; RUN;
DATA Loans; SET LEGEND.Loans; RUN;

%ARC_ADD_PROCESSING_BY_LOAN(ArcData,Loans,ACCOUNT_NUMBER,COMMENT,RECIPIENT_ID,IS_REFERENCE,IS_ENDORSER,PROCESS_FROM,PROCESS_TO,NEEDED_BY,REGARDS_TO,REGARDS_CODE,CREATED_BY,ARC_TYPE,ARCTOADD,SAS_ID);
