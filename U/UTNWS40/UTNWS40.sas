/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = Y:\Development\SAS Test Files\SASR 3775\2014-01-15 1208PM;
FILENAME REPORTZ "&RPTLIB/UNWS40.NWS40RZ";
FILENAME REPORT2 "&RPTLIB/UNWS40.NWS40R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

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
	CREATE TABLE BNK AS
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID,
			CASE WHEN PD24.DC_BKR_STA = '' THEN 'status' ELSE '' END AS DC_BKR_STA,
			CASE WHEN PD24.DC_BKR_TYP = '' THEN 'chapter type' ELSE '' END AS DC_BKR_TYP,
			CASE WHEN PD24.DF_COU_DKT = '' THEN 'case number' ELSE '' END AS DF_COU_DKT,
			CASE WHEN PD24.DD_BKR_COR_1_RCV IS NULL THEN 'doc rec date' ELSE '' END AS DD_BKR_COR_1_RCV,
			CASE WHEN PD24.DF_ATT IS NULL THEN 'attorney' ELSE '' END AS DF_ATT
		FROM
			PKUB.PD10_PRS_NME PD10
			JOIN PKUB.LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
				AND LN10.LA_CUR_PRI > 0
				AND LN10.LC_STA_LON10 = 'R'
			JOIN PKUB.PD24_PRS_BKR PD24
				ON PD10.DF_PRS_ID = PD24.DF_PRS_ID
				AND PD24.DC_BKR_STA <> '05'
			LEFT JOIN PKUB.AY10_BR_LON_ATY AY10
				ON PD10.DF_PRS_ID = AY10.BF_SSN
				AND AY10.PF_REQ_ACT = 'BKDQC'
				AND AY10.LD_ATY_REQ_RCV > PD24.DD_BKR_STA
		WHERE
			AY10.BF_SSN IS NULL
			AND 
			(
				PD24.DC_BKR_STA = ''
				OR PD24.DC_BKR_TYP = ''
				OR PD24.DF_COU_DKT = ''
				OR PD24.DD_BKR_COR_1_RCV IS NULL
				OR PD24.DF_ATT IS NULL		
			)
		ORDER BY 
			PD10.DF_SPE_ACC_ID
	;

	CREATE TABLE R2 AS
		SELECT DISTINCT
			DF_SPE_ACC_ID,
			'missing bankruptcy ' || CATX(', ',DC_BKR_STA,DC_BKR_TYP,DF_COU_DKT,DD_BKR_COR_1_RCV,DF_ATT) AS TEXT
		FROM 
			BNK
	;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA R2; SET LEGEND.R2; RUN;

DATA _NULL_;
	SET R2 ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	PUT DF_SPE_ACC_ID 'BKDQC,,,,,,,ALL,' TEXT ;
RUN;
