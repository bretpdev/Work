PROC IMPORT OUT = WORK.SOURCE
            DATAFILE = "Q:\Support Services\Jarom\DCR_WENDY.xls" 
            DBMS = xls REPLACE;
   			SHEET = 'Sheet1'; 
RUN;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=WORK;
DATA DUSTER.SOURCE; *Send data to Legend;
SET SOURCE;
RUN;


RSUBMIT DUSTER;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE DATA AS
		SELECT	
			S.BF_SSN,
			S.LN_SEQ,
			LN40.LN_SEQ_CLM_PCL,
			LN40.LC_REA_CAN_CLM_PCL,
			LN40.LD_CAN_CLM_PCL

		FROM
			 SOURCE S
		INNER JOIN OLWHRM1.LN40_LON_CLM_PCL LN40
			ON LN40.BF_SSN = S.BF_SSN
			AND LN40.LN_SEQ = S.LN_SEQ
			AND LN40.LN_SEQ_CLM_PCL = S.LN_SEQ_CLM_PCL
		WHERE
/*			LN40.LC_REA_CAN_CLM_PCL <> 'OT'*/
			 LN40.LN_SEQ_CLM_PCL  = 3
			AND LN40.LD_CAN_CLM_PCL > INPUT('06/25/2013',mmddyy10.)

;
QUIT;
ENDRSUBMIT;

PROC EXPORT DATA= DUSTER.DATA 
            OUTFILE= "Q:\Support Services\Jarom\Matched_Dcr_Wendy_LN_SEQ_CLM_PCL.xls" 
            DBMS=EXCEL REPLACE;
     SHEET="A"; 
RUN;
