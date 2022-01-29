

/*read in MR33 file*/
DATA MR33_;
	INFILE 'T:\SAS\MR-33_Feb 2021.txt';
	INPUT
		SSN_ $ 1-11 
		CLAIM_ID $16-19
		CLID_ $ 25-43
		PRINC_ $ 47-61
		ACINT_ $ 65-79
		CHRGS_ $ 83-97
		;
RUN;

/*read in MR34 file*/
DATA MR34_;
	INFILE 'T:\SAS\MR-34_Feb 2021.txt';
	INPUT
		SSN_ $ 1-11 
		CLAIM_ID $16-19
		CLID_ $ 25-43
		PRINC_ $ 47-61
		ACINT_ $ 65-79
		CHRGS_ $ 83-97
		;
RUN;

/*read in MR35 file*/
DATA MR35_;
	INFILE 'T:\SAS\MR-35_Feb 2021.txt';
	INPUT
		SSN_ $ 1-11 
		CLAIM_ID $16-19
		CLID_ $ 25-43
		PRINC_ $ 47-61
		ACINT_ $ 65-79
		CHRGS_ $ 83-97
		;
RUN;

/*read in MR36 file*/
DATA MR36_;
	INFILE 'T:\SAS\MR-36_Feb 2021.txt';
	INPUT
		SSN_ $ 1-11 
		CLAIM_ID $16-19
		CLID_ $ 25-43
		PRINC_ $ 47-61
		ACINT_ $ 65-79
		CHRGS_ $ 83-97
		;
RUN;

/*read in MR37 file*/
DATA MR37_;
	INFILE 'T:\SAS\MR-37_Feb 2021.txt';
	INPUT
		SSN_ $ 1-11 
		CLAIM_ID $16-19
		CLID_ $ 25-43
		PRINC_ $ 47-61
		ACINT_ $ 65-79
		CHRGS_ $ 83-97
		;
RUN;

/*read in MR38 file*/
DATA MR38_;
	INFILE 'T:\SAS\MR-38_Feb 2021.txt';
	INPUT
		SSN_ $ 1-11 
		CLAIM_ID $16-19
		CLID_ $ 25-43
		PRINC_ $ 47-61
		ACINT_ $ 65-79
		CHRGS_ $ 83-97
		;
	RUN;

/*read in MR39 file*/
DATA MR39_;
	INFILE 'T:\SAS\MR-39_Feb 2021.txt';
	INPUT
		SSN_ $ 1-11 
		CLAIM_ID $16-19
		CLID_ $ 25-43
		PRINC_ $ 47-61
		ACINT_ $ 65-79
		CHRGS_ $ 83-97
		;
	RUN;

/*read in MR40 file*/
DATA MR40_;
	INFILE 'T:\SAS\MR-40_Feb 2021.txt';
	INPUT
		SSN_ $ 1-11 
		CLAIM_ID $16-19
		CLID_ $ 25-43
		PRINC_ $ 47-61
		ACINT_ $ 65-79
		CHRGS_ $ 83-97
		;
	RUN;


/*combine all data read from files*/
DATA MR_DATA_;
	SET MR33_ MR34_ MR35_ MR36_ MR37_ MR38_ MR39_ MR40_;
RUN;


/*remove hyphens and dollar signs and get unique ID and unique ID sfx*/
DATA MR_DATA;
	SET MR_DATA_;
	SSN = COMPRESS(TRANSLATE(SSN_,'','-'));
	AF_APL_ID = SUBSTR(CLID_,1,17);
	AF_APL_ID_SFX  = SUBSTR(CLID_,18,2);
	PRINC = TRANSLATE(PRINC_,' ','$');
	ACINT = TRANSLATE(ACINT_,' ','$');
	CHRGS = TRANSLATE(CHRGS_,' ','$');
/*	PRINC = COMPRESS(TRANSLATE(PRINC_,' ','$'));*/
/*	ACINT = COMPRESS(TRANSLATE(ACINT_,' ','$'));*/
/*	CHRGS = COMPRESS(TRANSLATE(CHRGS_,' ','$'));*/
RUN;

/*only select actual data rows to get rid of the headers and other garbage read in from files*/
PROC SQL;
	CREATE TABLE MR_DATA_FINAL AS
	SELECT
/*		SUBSTR(SSN,1,3)*/
		SSN
		,AF_APL_ID
		,AF_APL_ID_SFX
		,CLAIM_ID
		,CLID_ AS CLID
		,PRINC
		,ACINT
		,CHRGS
	FROM
		MR_DATA
	WHERE CLID NOT IN ('','UNIQUE ID')
	ORDER BY CLID;
QUIT;

/*Table WORK.MR_DATA_FINAL should be created, with 18995 rows and 8 columns. (each MR file has the record count at the bottom the sum of these counts is 18995)*/


/*create LIBNAME for remote server's WORK fold and copy data from MR files to remote server*/
LIBNAME WORKLOCL REMOTE SERVER=DUSTER SLIBREF=WORK;
DATA WORKLOCL.MR_DATA_FINAL; SET MR_DATA_FINAL; RUN;

/*SUBMIT CODE TO REMOTE SERVER*/
RSUBMIT DUSTER;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%let DB = DLGSUTWH;
%let OWNR = OLWHRM1;

PROC SQL STIMER;
	CONNECT TO DB2 (DATABASE=&DB); 

/*	get data for all loans from DB2 warehouse*/
	CREATE TABLE MR_and_DB2_DATA AS
		SELECT 
			MR.*
			,DB2D.DM_PRS_LST
			,DB2D.DM_PRS_1
			,DB2D.BD_CLM_PAY_PRC
			,DB2D.LC_STA_DC10
			,DB2D.LD_TRX_EFF
			,DB2D.LA_APL_PRI
			,DB2D.LA_APL_INT
			,DB2D.LA_APL_OTH
			'749' AS GA_Code
		FROM 
			CONNECTION TO DB2 
			(
				SELECT DISTINCT
					DC01.AF_APL_ID
					,DC01.AF_APL_ID_SFX
					,PD01.DM_PRS_LST
					,PD01.DM_PRS_1
					,DC01.BD_CLM_PAY_PRC
					,DC01.LC_STA_DC10
					,LSTPAY.LD_TRX_EFF
					,LSTPAY.LA_APL_PRI
					,LSTPAY.LA_APL_INT
					,LSTPAY.LA_APL_OTH



				FROM 
					&OWNR..PD01_PDM_INF PD01
					INNER JOIN &OWNR..DC01_LON_CLM_INF DC01
						ON PD01.DF_PRS_ID = DC01.BF_SSN
					INNER JOIN
						(
							SELECT
								AF_APL_ID
								,AF_APL_ID_SFX
								,MAX(LF_CRT_DTS_DC10) AS LF_CRT_DTS_DC10
							FROM
								&OWNR..DC01_LON_CLM_INF
							GROUP BY
								AF_APL_ID
								,AF_APL_ID_SFX
						) MAXDC01
							ON DC01.AF_APL_ID = MAXDC01.AF_APL_ID
							AND DC01.AF_APL_ID_SFX = MAXDC01.AF_APL_ID_SFX
							AND DC01.LF_CRT_DTS_DC10 = MAXDC01.LF_CRT_DTS_DC10
/*						join to derived table of information about the last payment for each loan*/
					LEFT JOIN
						(
							SELECT DISTINCT
								DC11.AF_APL_ID
								,DC11.AF_APL_ID_SFX
								,DC11.LF_CRT_DTS_DC10
								,DC11.LD_TRX_EFF
								,DC11.LA_APL_PRI
								,DC11.LA_APL_INT
								,DC11.LA_APL_LEG_CST + DC11.LA_APL_OTH_CHR + DC11.LA_APL_COL_CST AS LA_APL_OTH
							FROM
								&OWNR..DC11_LON_FAT DC11
								INNER JOIN
									(
										SELECT
											AF_APL_ID
											,AF_APL_ID_SFX
											,LF_CRT_DTS_DC10
											,MAX(LF_CRT_DTS_DC11) AS LF_CRT_DTS_DC11
										FROM
											&OWNR..DC11_LON_FAT DC11	
										GROUP BY
											AF_APL_ID
											,AF_APL_ID_SFX
											,LF_CRT_DTS_DC10
									) MAX_DTS_DC11
										ON DC11.AF_APL_ID = MAX_DTS_DC11.AF_APL_ID
										AND DC11.AF_APL_ID_SFX = MAX_DTS_DC11.AF_APL_ID_SFX
										AND DC11.LF_CRT_DTS_DC10 = MAX_DTS_DC11.LF_CRT_DTS_DC10
										AND DC11.LF_CRT_DTS_DC11 = MAX_DTS_DC11.LF_CRT_DTS_DC11
								INNER JOIN
									(
										SELECT
											AF_APL_ID
											,AF_APL_ID_SFX
											,LF_CRT_DTS_DC10		
											,MAX(LD_TRX_EFF) AS LD_TRX_EFF
										FROM
											&OWNR..DC11_LON_FAT DC11
										GROUP BY
											AF_APL_ID
											,AF_APL_ID_SFX
											,LF_CRT_DTS_DC10								
									) MAXDC11
										ON DC11.AF_APL_ID = MAXDC11.AF_APL_ID
										AND DC11.AF_APL_ID_SFX = MAXDC11.AF_APL_ID_SFX
										AND DC11.LF_CRT_DTS_DC10 = MAXDC11.LF_CRT_DTS_DC10
										AND DC11.LD_TRX_EFF = MAXDC11.LD_TRX_EFF
						) LSTPAY
							ON DC01.AF_APL_ID = LSTPAY.AF_APL_ID
							AND DC01.AF_APL_ID_SFX = LSTPAY.AF_APL_ID_SFX
							AND MAXDC01.LF_CRT_DTS_DC10 = LSTPAY.LF_CRT_DTS_DC10


		
				FOR READ ONLY WITH UR
			) DB2D
/*			join DB2 data to local data*/
			INNER JOIN MR_DATA_FINAL MR
				ON DB2D.AF_APL_ID = MR.AF_APL_ID
				AND DB2D.AF_APL_ID_SFX = MR.AF_APL_ID_SFX
	;
	DISCONNECT FROM DB2;

QUIT;

ENDRSUBMIT;

/*copy down data set of remote data joined with local data to local WORK folder*/
DATA MR_and_DB2_DATA; SET WORKLOCL.MR_and_DB2_DATA; RUN;


/*create an csv copy of the data for testing and review*/
ODS CSV FILE="T:\SAS\UNH 71356.csv";
PROC PRINT DATA=WORK.MR_and_DB2_DATA;
RUN;
ODS CSV CLOSE;


/*remove commas - this should have been done before but wasn't caught until the final data set had been created and numbers validated*/
DATA MR_and_DB2_DATA_;
	SET MR_and_DB2_DATA;
	PRINC_ = COMPRESS(TRANSLATE(PRINC,'',','));
	ACINT_ = COMPRESS(TRANSLATE(ACINT,'',','));
	CHRGS_ = COMPRESS(TRANSLATE(CHRGS,'',','));
RUN;


/*write data to flat file*/
DATA _NULL_;
	SET MR_and_DB2_DATA_;
	FILE 'T:\SAS\UNH 71356.txt';

	FORMAT BD_CLM_PAY_PRC YYMMDDn8.; /*	"n" in format specifies no separators (no dashes or slashes)*/
	FORMAT LD_TRX_EFF YYMMDDn8.;

	PUT 
		@1 DM_PRS_LST /*char data automatically left aligned, use format and switch to alter alignment (e.g. $32 -c to center or $32 -r to right align)*/
		@33 DM_PRS_1
		@65 SSN
		@74 CLID
		@104 PRINC_ $15. -r /*-r switch overrides default alignment for char format and right aligns data*/
		@119 ACINT_ $15. -r
		@134 CHRGS_ $15. -r
		@149 BD_CLM_PAY_PRC
		@157 LC_STA_DC10 $8. -r
		@165 LD_TRX_EFF  
		@173 LA_APL_PRI 15.2 /*numeric data automatically right aligned, use format and switch to alter alignment (e.g. 15.2 -c to center or $15.2 -l to left align)*/
		@188 LA_APL_INT 15.2
		@203 LA_APL_OTH 15.2
		@218 '749'
	;
RUN;
