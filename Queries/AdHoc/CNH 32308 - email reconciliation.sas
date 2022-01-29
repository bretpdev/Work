/******************   IT CSV FILES   *******************************/
*IT files - X,XXX RECORDS;
PROC IMPORT 
	OUT= WORK.CornerStone_Disaster_Relief 
	DATAFILE= "Q:\Support Services\JR\hurricane CR\CornerStone Disaster Relief.csv" 
	DBMS=CSV REPLACE;
	GETNAMES=YES;
	GUESSINGROWS=XXXX;
	DATAROW=X; 
RUN;

*IT files - XX,XXX RECORDS;
PROC IMPORT 
	OUT= WORK.Federal_Disaster_Relief_Area 
	DATAFILE= "Q:\Support Services\JR\hurricane CR\Federal Disaster Relief Area.csv" 
	DBMS=CSV REPLACE;
	GETNAMES=YES;
	GUESSINGROWS=XXXXX;
	DATAROW=X; 
RUN;

*all population;
DATA IT_ALL;
	SET CornerStone_Disaster_Relief  (DROP=TIME SIZE SCORE DESTINATION_SERVER)
		Federal_Disaster_Relief_Area (DROP=TIME SIZE SCORE DESTINATION_SERVER);
RUN;

/******************   UHEAA TEXT FILES   *******************************/
DATA Curr_Harvey;
	INFILE "Q:\Support Services\JR\hurricane CR\CurrentNaturalDisasterEmailPopHarvey.txt" 
		DLM = ',' FIRSTOBS = X;
	LENGTH POPULATION $ XX.;
	FORMAT DF_SPE_ACC_ID ZXX.;
	INPUT 	DF_SPE_ACC_ID XX. 
			DM_PRS_X   : $ XXX.
			DM_PRS_LST : $ XXX.
			DX_ADR_EML : $ XXX. ;
	POPULATION = 'HARVEY CURRENT';
RUN; 

DATA Delq_Harvey;
	INFILE "Q:\Support Services\JR\hurricane CR\DelqNaturalDisasterEmailPopHarvey.txt" 
		DLM = ',' FIRSTOBS = X;
	LENGTH POPULATION $ XX.;
	FORMAT DF_SPE_ACC_ID ZXX.;
	INPUT 	DF_SPE_ACC_ID XX. 
			DM_PRS_X   : $ XXX.
			DM_PRS_LST : $ XXX.
			DX_ADR_EML : $ XXX. ;
	POPULATION = 'HARVEY DELINQUENT';
RUN; 

DATA Curr_Irma;
	INFILE "Q:\Support Services\JR\hurricane CR\CurrentNaturalDisasterEmailPopIrma.txt"
		DLM = ',' FIRSTOBS = X;
	LENGTH POPULATION $ XX.;
	FORMAT DF_SPE_ACC_ID ZXX.;
	INPUT 	DF_SPE_ACC_ID XX. 
			DM_PRS_X   : $ XXX.
			DM_PRS_LST : $ XXX.
			DX_ADR_EML : $ XXX. ;
	POPULATION = 'IRMA CURRENT';
RUN; 

DATA Delq_Irma;
	INFILE "Q:\Support Services\JR\hurricane CR\DelqNaturalDisasterEmailPopIrma.txt"
		DLM = ',' FIRSTOBS = X;
	LENGTH POPULATION $ XX.;
	FORMAT DF_SPE_ACC_ID ZXX.;
	INPUT 	DF_SPE_ACC_ID XX. 
			DM_PRS_X   : $ XXX.
			DM_PRS_LST : $ XXX.
			DX_ADR_EML : $ XXX. ;
	POPULATION = 'IRMA DELINQUENT';
RUN; 

*Total XX,XXX UHEAA pop;
DATA UHEAA_POP;
	SET Delq_Harvey
		Curr_Harvey
		Curr_Irma
		Delq_Irma;
RUN;

*no IT record: XX,XXX to be resent;
PROC SQL;
	CREATE TABLE NO_IT_RECORD AS
	SELECT DISTINCT
		UP.*
	FROM
		UHEAA_POP UP
		LEFT JOIN IT_ALL IA
			ON UP.DX_ADR_EML = IA.TO
	WHERE
		IA.TO IS NULL
	;
QUIT;

*not delivered: XXX;
DATA IT_FAIL;
	SET CornerStone_Disaster_Relief  (DROP=TIME SIZE SCORE DESTINATION_SERVER)
	    Federal_Disaster_Relief_Area (DROP=TIME SIZE SCORE DESTINATION_SERVER);
	IF Delivery_Status ^= 'Delivered';
	USERNAME=SUBSTR(TO,X,FIND(TO,'@',X)-X);
RUN;

*resend: XXX;
PROC SQL;
	CREATE TABLE IT_FAIL_RESEND AS
	SELECT
		UP.*
	FROM
		UHEAA_POP UP
		INNER JOIN IT_FAIL IF
			ON UP.DX_ADR_EML = IF.TO
	;
QUIT;

*ignore: X;
PROC SQL;
	CREATE TABLE IGNORE AS
	SELECT
		IF.TO
	FROM
		IT_FAIL IF		
		LEFT JOIN UHEAA_POP UP
			ON UP.DX_ADR_EML = IF.TO
	WHERE
		UP.DX_ADR_EML IS NULL
	;
QUIT;

*combine all resend data;
DATA ALL;
	SET NO_IT_RECORD
	IT_FAIL_RESEND;
RUN;

*comment out if want to see individual subgroups;
PROC DATASETS NOPRINT;
	DELETE Cornerstone_disaster_relief
			Curr_harvey
			Curr_irma
			Delq_harvey
			Delq_irma
			Federal_disaster_relief_area
			Ignore
			It_all
			It_fail
			It_fail_resend
			No_it_record
			Uheaa_pop;
QUIT;

*prepare output files;
DATA _HARVEY_DQ (DROP=POPULATION)
	 _HARVEY_CURRENT (DROP = POPULATION)
	 _IRMA_CURRENT (DROP = POPULATION)
	 _IRMA_DQ (DROP = POPULATION)
	 _ERRORS;
	SET ALL;
	IF POPULATION = 'HARVEY DELINQUENT'
		THEN OUTPUT _HARVEY_DQ;
	ELSE IF POPULATION = 'HARVEY CURRENT'
		THEN OUTPUT _HARVEY_CURRENT;
	ELSE IF POPULATION = 'IRMA CURRENT'
		THEN OUTPUT _IRMA_CURRENT;
	ELSE IF POPULATION = 'IRMA DELINQUENT'
		THEN OUTPUT _IRMA_DQ;
	ELSE OUTPUT _ERRORS;
RUN;

%LET RPT = T:\SAS;
DATA _NULL_;
	SET  _HARVEY_DQ;
	FILE "&RPT/HARVEY_DELINQUENT" DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	IF _N_ = X THEN
		DO;
			PUT 'DF_SPE_ACC_ID' ','
				'DM_PRS_X' ','
				'DM_PRS_LST' ','
				'DX_ADR_EML';
		END;
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT DM_PRS_X $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_ADR_EML $ ;
	END;
RUN;
DATA _NULL_;
	SET  _HARVEY_CURRENT;
	FILE "&RPT/HARVEY_CURRENT" DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	IF _N_ = X THEN
		DO;
			PUT 'DF_SPE_ACC_ID' ','
				'DM_PRS_X' ','
				'DM_PRS_LST' ','
				'DX_ADR_EML';
		END;
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT DM_PRS_X $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_ADR_EML $ ;
	END;
RUN;
DATA _NULL_;
	SET  _IRMA_CURRENT;
	FILE "&RPT/IRMA_CURRENT" DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	IF _N_ = X THEN
		DO;
			PUT 'DF_SPE_ACC_ID' ','
				'DM_PRS_X' ','
				'DM_PRS_LST' ','
				'DX_ADR_EML';
		END;
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT DM_PRS_X $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_ADR_EML $ ;
	END;
RUN;
DATA _NULL_;
	SET  _IRMA_DQ;
	FILE "&RPT/IRMA_DELINQUENT" DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	IF _N_ = X THEN
		DO;
			PUT 'DF_SPE_ACC_ID' ','
				'DM_PRS_X' ','
				'DM_PRS_LST' ','
				'DX_ADR_EML';
		END;
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT DM_PRS_X $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_ADR_EML $ ;
	END;
RUN;

/*PROC REPORT DATA=IT_FAIL;*/
/*	COMPUTE TO;*/
/*	COUNT+X;*/
/*	IF MOD(COUNT,X) THEN */
/*		DO;*/
/*			CALL DEFINE(_ROW_, "STYLE", "STYLE=[BACKGROUND=#DCDCDC");*/
/*		END;*/
/*	ENDCOMP;*/
/*RUN;*/
/**/
