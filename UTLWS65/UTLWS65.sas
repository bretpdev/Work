/*%LET RPTLIB = %SYSGET(reportdir);*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ULS.DSN; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= dbo;
%INCLUDE "X:\Sessions\Local SAS Schedule\ArcAdd Common.SAS";

%LET RPTLIB = X:\PADD\FTP;
FILENAME REPORTZ "&RPTLIB/ULWS65.LWS65RZ";
FILENAME REPORT2 "&RPTLIB/ULWS65.LWS65R2";

/*TEST*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK;*/
/*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

RSUBMIT;

/*%let DB = DLGSWQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;


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
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
			,'REVIEW ACCOUNT FOR DETERMINATION OF TPD FORBEARANCE' AS COMMENT
			,'' AS RECIPIENT_ID
			,0 AS IS_REFERENCE
			,0 AS IS_ENDORSER
			,. AS PROCESS_FROM 
			,. AS PROCESS_TO 
			,. AS NEEDED_BY 
	    	,'' AS REGARDS_TO
			,'' AS REGARDS_CODE
			,'UTLWS65' AS CREATED_BY
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID
					FROM
						OLWHRM1.PD10_PRS_NME PD10
						INNER JOIN OLWHRM1.PD22_PRS_DSA PD22
							ON PD22.DF_PRS_ID = PD10.DF_PRS_ID
						INNER JOIN OLWHRM1.LN10_LON LN10
							ON LN10.BF_SSN = PD10.DF_PRS_ID
						INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
							ON DW01.BF_SSN = LN10.BF_SSN
							AND DW01.LN_SEQ = LN10.LN_SEQ
						LEFT JOIN 
							(
								SELECT
									FB10.BF_SSN,
									LN60.LN_SEQ,
									FB10.LC_FOR_TYP,
									LN60.LD_FOR_END
								FROM
									OLWHRM1.LN60_BR_FOR_APV LN60
									INNER JOIN OLWHRM1.FB10_BR_FOR_REQ FB10
										ON LN60.BF_SSN = FB10.BF_SSN
										AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
								WHERE
									FB10.LC_FOR_STA = 'A'
									AND LN60.LC_STA_LON60 = 'A'
									AND FB10.LC_STA_FOR10 = 'A'
							) FOR
							ON LN10.BF_SSN = FOR.BF_SSN
							AND LN10.LN_SEQ = FOR.LN_SEQ
						LEFT JOIN 
						(
							SELECT
								DF10.BF_SSN,
								LN50.LN_SEQ,
								LN50.LD_DFR_END
							FROM
								OLWHRM1.DF10_BR_DFR_REQ DF10
								INNER JOIN OLWHRM1.LN50_BR_DFR_APV LN50
									ON DF10.BF_SSN = LN50.BF_SSN
									AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
							WHERE
								DF10.LC_DFR_STA = 'A'
								AND LN50.LC_STA_LON50 = 'A'
								AND DF10.LC_STA_DFR10 = 'A'
						) DEF
							ON LN10.BF_SSN = DEF.BF_SSN
							AND LN10.LN_SEQ = DEF.LN_SEQ
						LEFT JOIN 
							(
								SELECT DISTINCT
									AY10.BF_SSN
								FROM 
									OLWHRM1.AY10_BR_LON_ATY AY10
								WHERE 
									AY10.PF_REQ_ACT = 'TPDFR'
							) ARC
							ON ARC.BF_SSN = LN10.BF_SSN
							
					WHERE
						LN10.LA_CUR_PRI > 0
						AND PD22.DX_PRS_DSA_TPD_REA IN ('120SUSP', 'INDEFSUSP', 'APPAPPR')
						AND
						(
							(DAYS(LN10.LD_END_GRC_PRD) - DAYS(CURRENT DATE) BETWEEN 0 AND 15)
							OR
							(DAYS(DEF.LD_DFR_END) - DAYS(CURRENT DATE) BETWEEN 0 AND 15)
							OR
							(FOR.LC_FOR_TYP ^= '14' AND DAYS(FOR.LD_FOR_END) - DAYS(CURRENT DATE) BETWEEN 0 AND 15)
						)
						AND ARC.BF_SSN IS NULL

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO; SET DUSTER.DEMO; RUN;

/*write to arc add*/
%GENERAL_ARC_ADD_PROCESSING(WORK.DEMO, DF_SPE_ACC_ID, COMMENT, RECIPIENT_ID, IS_REFERENCE, IS_ENDORSER, PROCESS_FROM, PROCESS_TO, NEEDED_BY, REGARDS_TO, REGARDS_CODE, CREATED_BY, 1, 'TPDFR', 'UTLWS65');
