PROC IMPORT OUT= WORK.T1098E
            DATAFILE= "T:\Cornerstone_1098E.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Sheet1$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

PROC IMPORT OUT= WORK.T1099C
            DATAFILE= "T:\Cornerstone_1099C.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Sheet1$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

DATA T1099C;
	SET T1099C;
	format SSN z9.;
RUN;

DATA T1098E;
	SET T1098E;
	format SSN z9.;
RUN;


LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.T1098E; *Send data to Duster;
SET T1098E;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.T1099C; *Send data to Duster;
SET T1099C;
RUN;


RSUBMIT;
LIBNAME PKUB DB2 DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE T1098E_OUTPUT AS
		SELECT
			PD10.DF_SPE_ACC_ID,
			T.Addressee_Email AS DX_ADR_EML,
			PD10.DM_PRS_1,
			PD10.DM_PRS_LST
		FROM
			T1098E T
		INNER JOIN PKUB.PD10_PRS_NME PD10
			ON INPUT(PD10.DF_PRS_ID, BEST12.) = T.SSN
		
;

PROC SQL;
	CREATE TABLE T1099C_OUTPUT AS
		SELECT
			PD10.DF_SPE_ACC_ID,
			T.Addressee_Email AS DX_ADR_EML,
			PD10.DM_PRS_1,
			PD10.DM_PRS_LST
		FROM
			T1099C T
		INNER JOIN PKUB.PD10_PRS_NME PD10
			ON INPUT(PD10.DF_PRS_ID, BEST12.) = T.SSN	
		
;
QUIT;
ENDRSUBMIT;

DATA T1098E_OUTPUT;
	SET LEGEND.T1098E_OUTPUT;
RUN;

DATA T1099C_OUTPUT;
	SET LEGEND.T1099C_OUTPUT;
RUN;

DATA _NULL_;
	SET		WORK.T1099C_OUTPUT;
	FILE
		'T:\T1099C_OUTPUT.txt'
		DELIMITER = ','
		DSD
		DROPOVER
		LRECL = 32767
	;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
		DO;
			PUT	
				'DX_ADR_EML'
				','
				'DF_SPE_ACC_ID'
				','
				'DM_PRS_1'
				','
				'DM_PRS_LST'
			;
		END;

	/* write data*/	
	DO;
		PUT DX_ADR_EML $ @;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_LST;
		;
	END;
RUN;

DATA _NULL_;
	SET		WORK.T1098E_OUTPUT;
	FILE
		'T:\T1098E_OUTPUT.txt'
		DELIMITER = ','
		DSD
		DROPOVER
		LRECL = 32767
	;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
		DO;
			PUT	
				'DX_ADR_EML'
				','
				'DF_SPE_ACC_ID'
				','
				'DM_PRS_1'
				','
				'DM_PRS_LST'
			;
		END;

	/* write data*/	
	DO;
		PUT DX_ADR_EML $ @;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_LST;
		;
	END;
RUN;

