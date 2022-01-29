/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWS62.LWS62RZ";
FILENAME REPORT2 "&RPTLIB/ULWS62.LWS62R2";
FILENAME REPORT3 "&RPTLIB/ULWS62.LWS62R3";
FILENAME REPORT4 "&RPTLIB/ULWS62.LWS62R4";
FILENAME REPORT5 "&RPTLIB/ULWS62.LWS62R5";

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

/*	all Compass borrowers, students, endorsers, co-borrowers, co-makers, and co-signers with open loans*/
	CREATE TABLE COMPASS AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT 
						DISTINCT	
						PD10.DF_PRS_ID,
						CASE
							WHEN PD10.DF_SPE_ACC_ID = '' THEN PRS.DF_PRS_ID
							ELSE PD10.DF_SPE_ACC_ID 
						END AS DF_SPE_ACC_ID
					FROM	
						OLWHRM1.PD10_PRS_NME PD10
						INNER JOIN
							(
								SELECT 
									BF_SSN AS DF_PRS_ID
								FROM
									OLWHRM1.LN10_LON 
								WHERE
									LA_CUR_PRI > 0 
									AND LC_STA_LON10 = 'R'
							UNION
								SELECT 
									LF_STU_SSN AS DF_PRS_ID
								FROM
									OLWHRM1.LN10_LON
								WHERE
									LA_CUR_PRI > 0 
									AND LC_STA_LON10 = 'R'
							UNION
								SELECT 
									END.LF_EDS AS DF_PRS_ID
								FROM
									OLWHRM1.LN10_LON LN10
									INNER JOIN OLWHRM1.LN20_EDS END
										ON LN10.BF_SSN = END.BF_SSN
								WHERE
									LN10.LA_CUR_PRI > 0 
									AND LN10.LC_STA_LON10 = 'R'
									AND END.LC_STA_LON20 = 'A'
							) PRS
								ON PRS.DF_PRS_ID = PD10.DF_PRS_ID

					FOR READ ONLY WITH UR
				)
	;

/*	Compass borrowers, etc. with their valid phone numbers*/
	CREATE TABLE R2 AS
		SELECT
			DISTINCT
			COMP.DF_SPE_ACC_ID AS ACC_ID,
			PD42.PHN_NUM
		FROM
			COMPASS COMP
			INNER JOIN CONNECTION TO DB2
				(
					SELECT
						DISTINCT
						DF_PRS_ID,
						RTRIM(DN_DOM_PHN_ARA) || '' || RTRIM(DN_DOM_PHN_XCH) || '' || DN_DOM_PHN_LCL AS PHN_NUM
					FROM
						OLWHRM1.PD42_PRS_PHN
					WHERE
						DC_ALW_ADL_PHN IN ('L', 'Q', 'U', 'X')
						AND DI_PHN_VLD = 'Y'
				) PD42
					ON COMP.DF_PRS_ID = PD42.DF_PRS_ID
		WHERE
			PD42.PHN_NUM IS NOT NULL
	;

/*	all defaulted borrowers*/
	CREATE TABLE DEFAULTS AS
		SELECT
			*
		FROM
			CONNECTION TO DB2
				(
					SELECT
						DISTINCT
						PD01.DF_PRS_ID,
						PD01.DF_SPE_ACC_ID
					FROM
						OLWHRM1.GA14_LON_STA GA14
						INNER JOIN OLWHRM1.DC01_LON_CLM_INF DC01
							ON GA14.AF_APL_ID = DC01.AF_APL_ID
							AND GA14.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
						INNER JOIN OLWHRM1.PD01_PDM_INF PD01
							ON DC01.BF_SSN = PD01.DF_PRS_ID
					WHERE
						GA14.AC_STA_GA14 = 'A'
						AND GA14.AC_LON_STA_TYP = 'CP'
						AND DC01.LC_STA_DC10 = '03'
						AND DC01.LC_REA_CLM_ASN_DOE NOT IN ('01','03','04','08','21','23','24','25')
				)
	;

/*	defaulted borrowers with their valid phone numbers*/
	CREATE TABLE R3A AS
		SELECT
			DISTINCT
			DFTS.DF_SPE_ACC_ID AS ACC_ID,
			PHNS.PHN_NUM
		FROM
			DEFAULTS DFTS
			INNER JOIN CONNECTION TO DB2
				(
					SELECT
						DF_PRS_ID,
						PHN_NUM
					FROM
						(
							SELECT 
								DF_PRS_ID,
								DN_PHN AS PHN_NUM
							FROM
								OLWHRM1.PD03_PRS_ADR_PHN
							WHERE
								DC_CEP IN ('L', 'U', 'T')
								AND DI_PHN_VLD = 'Y'
								AND DN_PHN <> ''
						UNION
							SELECT 
								DF_PRS_ID,
								DN_ALT_PHN AS PHN_NUM
							FROM
								OLWHRM1.PD03_PRS_ADR_PHN
							WHERE
								DC_ALT_CEP IN ('L', 'U', 'T')
								AND DI_ALT_PHN_VLD = 'Y'
								AND DN_ALT_PHN <> ''
						) 							
				) PHNS
					ON DFTS.DF_PRS_ID = PHNS.DF_PRS_ID
	;

/*  Exclude accounts that have the same phone number AND account number in the R2 and R3 file*/
	CREATE TABLE R3 AS
		SELECT
			A.*
		FROM
			R3A A
		WHERE
			NOT EXISTS
				(
					SELECT
						*
					FROM
						R2 B
					WHERE
						A.ACC_ID = B.ACC_ID
						AND A.PHN_NUM = B.PHN_NUM
				)
	;

/*	OneLINK references*/
	CREATE TABLE OL_REFS AS
		SELECT
			DISTINCT
			DF_PRS_ID_BR,
			DF_PRS_ID_RFR,
			PHN_NUM
		FROM
			CONNECTION TO DB2
				(
					SELECT
						DISTINCT
						DF_PRS_ID_BR,
						DF_PRS_ID_RFR,
						PHN_NUM
					FROM						
						(
							SELECT 
								DF_PRS_ID_BR,
								DF_PRS_ID_RFR,
								BN_RFR_DOM_PHN AS PHN_NUM
							FROM
								OLWHRM1.BR03_BR_REF
							WHERE
								BC_PRI_PHN_TYP IN ('L', 'U', 'T')
								AND BI_DOM_PHN_VLD = 'Y'
								AND BN_RFR_DOM_PHN <> ''
								AND BC_STA_BR03 = 'A'
						UNION
							SELECT 
								DF_PRS_ID_BR,
								DF_PRS_ID_RFR,
								BN_RFR_ALT_PHN AS PHN_NUM
							FROM
								OLWHRM1.BR03_BR_REF
							WHERE
								BC_ALT_PHN_TYP IN ('L', 'U', 'T')
								AND BI_ALT_PHN_VLD = 'Y'
								AND BN_RFR_ALT_PHN <> ''
								AND BC_STA_BR03 = 'A'
						) 						
				) 
	;

/*	Defaulted Borrowers' OneLINK references*/
	CREATE TABLE R4 AS
		SELECT
			DISTINCT
			REFS.DF_PRS_ID_RFR AS ACC_ID,
			REFS.PHN_NUM
		FROM
			DEFAULTS DFTS
			INNER JOIN OL_REFS REFS
				ON DFTS.DF_PRS_ID = REFS.DF_PRS_ID_BR
	;

	DISCONNECT FROM DB2;
QUIT;

ENDRSUBMIT;


DATA R2; SET DUSTER.R2; RUN;
DATA R3; SET DUSTER.R3; RUN;
DATA R4; SET DUSTER.R4; RUN;

%MACRO ENCRYPT(RNO);
	DATA R&RNO (DROP = ACC_ID PHN_NUM);
		SET R&RNO;
		FORMAT DF_SPE_ACC_ID_EP $20.;

		IF SUBSTR(ACC_ID,1,1) IN ('P','R') THEN DF_SPE_ACC_ID_EP = ACC_ID;
		ELSE DF_SPE_ACC_ID_EP = STRIP(PUT(JULDATE(TODAY()) * INPUT(SUBSTR(ACC_ID,ANYDIGIT(ACC_ID)),BEST12.), NUMX20.));

		PHONE = PHN_NUM;
		FORMAT RUN_DATE MMDDYY10.;
		RUN_DATE = TODAY();
	RUN;
%MEND;

%ENCRYPT(2);
%ENCRYPT(3);
%ENCRYPT(4);

PROC SQL;
	CREATE TABLE FILE_COUNTS AS
		SELECT
			DESCRIPTION
			,CNT
		FROM
			(
				SELECT
					'Total number of phone numbers for Compass borrowers, students, endorsers, comakers, cosigners, and coborrowers' AS DESCRIPTION
					,COUNT(DF_SPE_ACC_ID_EP) AS CNT
					,'R2' AS FNO
				FROM
					R2
			UNION
				SELECT
					'Total number of phone numbers of Defaulted borrowers' AS DESCRIPTION
					,COUNT(DF_SPE_ACC_ID_EP) AS CNT
					,'R3' AS FNO
				FROM
					R3
			UNION
				SELECT
					'Total number of phone numbers for Defaulted Borrowers'' OneLINK references' AS DESCRIPTION
					,COUNT(DF_SPE_ACC_ID_EP) AS CNT
					,'R4' AS FNO
				FROM
					R4
			)
		ORDER BY
			FNO
	;

	CREATE TABLE TOT_COUNT AS
		SELECT
			'Sum of all phone numbers that were identified by all reports' AS DESCRIPTION
			,SUM(CNT) AS CNT
		FROM
			FILE_COUNTS
	;
			
QUIT;


DATA R5;
	SET FILE_COUNTS TOT_COUNT;
RUN;

%MACRO CRT_FILE(RNO);
/*	data null used instead of proc export because proc export returns errors if the data set is empty*/
	DATA _NULL_;
		SET		WORK.R&RNO;
		FILE	REPORT&RNO delimiter=',' DSD DROPOVER lrecl=32767;

		IF _N_ = 1 THEN
			DO;
				PUT	'DF_SPE_ACC_ID_EP'
					','
					'PHONE'
					','
					'RUN_DATE';
			END;

		DO;
			PUT DF_SPE_ACC_ID_EP $ @;
			PUT PHONE $ @;
			PUT RUN_DATE $;
			;
		END;
	RUN;

%MEND;

%CRT_FILE(2);
%CRT_FILE(3);
%CRT_FILE(4);

/*print counts report*/
DATA _NULL_;
	SET		WORK.R5; 
	FILE	REPORT5 delimiter=',' DSD DROPOVER lrecl=32767;

	DO;
		PUT DESCRIPTION $ @;
		PUT CNT $;
		;
	END;
RUN;
