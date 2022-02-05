/*test*/
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= dbo;*/
/*%INCLUDE "Y:\Codebase\SAS\ArcAdd Common.SAS";*/
/*%LET RPTLIB = T:\SAS;*/

/*live*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= dbo;
%INCLUDE "Z:\Codebase\SAS\ArcAdd Common.SAS";
%LET RPTLIB = Z:\Batch\FTP\;

FILENAME REPORTZ "&RPTLIB/UNWO17.NWO17RZ";
FILENAME REPORT2 "&RPTLIB/UNWO17.NWO17R2";

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
			,'SASR' AS CREATED_BY
		FROM	
			CONNECTION TO DB2 
			(
				SELECT DISTINCT
					PD10.DF_SPE_ACC_ID
				FROM
					PKUB.PD10_PRS_NME PD10
					INNER JOIN PKUB.PD22_PRS_DSA PD22
						ON PD22.DF_PRS_ID = PD10.DF_PRS_ID
					INNER JOIN PKUB.LN10_LON LN10
						ON LN10.BF_SSN = PD10.DF_PRS_ID
					INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
						ON DW01.BF_SSN = LN10.BF_SSN
						AND DW01.LN_SEQ = LN10.LN_SEQ
					LEFT JOIN 
					(
						SELECT DISTINCT
							FB10.BF_SSN,
							LN60.LN_SEQ,
							FB10.LC_FOR_TYP,
							COALESCE(Max(LN60Max.LD_FOR_END),LN60.LD_FOR_END) AS LD_FOR_END
						FROM
							PKUB.LN60_BR_FOR_APV LN60
							INNER JOIN PKUB.FB10_BR_FOR_REQ FB10
								ON LN60.BF_SSN = FB10.BF_SSN
								AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
							LEFT OUTER JOIN 
							(
								SELECT
									BF_SSN,
									LN_SEQ,
									LD_FOR_BEG,
									LD_FOR_END
								FROM
									PKUB.LN60_BR_FOR_APV
								WHERE
									LC_STA_LON60 = 'A'
							) LN60Max
								ON LN60.BF_SSN = LN60Max.BF_SSN
								AND LN60.LN_SEQ = LN60Max.LN_SEQ
								AND LN60.LD_FOR_END + 1 Days = LN60Max.LD_FOR_BEG
						WHERE
							FB10.LC_FOR_STA = 'A'
							AND LN60.LC_STA_LON60 = 'A'
							AND FB10.LC_STA_FOR10 = 'A'
							AND FB10.LC_FOR_TYP ^= '14'
							AND CURRENT DATE BETWEEN LN60.LD_FOR_BEG AND LN60.LD_FOR_END
						GROUP BY
							FB10.BF_SSN,
							LN60.LN_SEQ,
							FB10.LC_FOR_TYP,
							LN60.LD_FOR_END
					) FOR
						ON LN10.BF_SSN = FOR.BF_SSN
						AND LN10.LN_SEQ = FOR.LN_SEQ
					LEFT JOIN 
					(
						SELECT
							DF10.BF_SSN,
							LN50.LN_SEQ,
							COALESCE(Max(LN50Max.LD_DFR_END),LN50.LD_DFR_END) AS LD_DFR_END
						FROM
							PKUB.DF10_BR_DFR_REQ DF10
							INNER JOIN PKUB.LN50_BR_DFR_APV LN50
								ON DF10.BF_SSN = LN50.BF_SSN
								AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
							LEFT OUTER JOIN 
							(
								SELECT
									BF_SSN,
									LN_SEQ,
									LD_DFR_BEG,
									LD_DFR_END
								FROM
									PKUB.LN50_BR_DFR_APV
								WHERE
									LC_STA_LON50 = 'A'
							) LN50Max
								ON LN50.BF_SSN = LN50Max.BF_SSN
								AND LN50.LN_SEQ = LN50Max.LN_SEQ
								AND LN50.LD_DFR_END + 1 Days = LN50Max.LD_DFR_BEG
						WHERE
							DF10.LC_DFR_STA = 'A'
							AND LN50.LC_STA_LON50 = 'A'
							AND DF10.LC_STA_DFR10 = 'A'
							AND CURRENT DATE BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
						GROUP BY
							DF10.BF_SSN,
							LN50.LN_SEQ,
							LN50.LD_DFR_END
					) DEF
						ON LN10.BF_SSN = DEF.BF_SSN
						AND LN10.LN_SEQ = DEF.LN_SEQ
					LEFT JOIN 
					(
						SELECT DISTINCT
							AY10.BF_SSN
						FROM 
							PKUB.AY10_BR_LON_ATY AY10
						WHERE 
							AY10.PF_REQ_ACT = 'TPDFR'
					) ARC
						ON ARC.BF_SSN = LN10.BF_SSN
						
				WHERE
					LN10.LA_CUR_PRI > 0
					AND PD22.DX_PRS_DSA_TPD_REA IN ('120SUSP', 'INDEFSUSP', 'APPAPPR')
					AND
					(
						(DW01.WC_DW_LON_STA = '01' AND DAYS(LN10.LD_END_GRC_PRD) - DAYS(CURRENT DATE) <= 15) /*GRACE*/
						OR
						(DW01.WC_DW_LON_STA IN('18','19') AND DAYS(DEF.LD_DFR_END) - DAYS(CURRENT DATE) <= 15) /*DEFERMENT*/
						OR
						(DW01.WC_DW_LON_STA IN('18','19') AND DAYS(FOR.LD_FOR_END) - DAYS(CURRENT DATE) <= 15) /*FORBEARANCE EXCLUDE TPD FORBEARANCE*/
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
DATA DEMO; SET LEGEND.DEMO; RUN;

/*write to arc add*/
%GENERAL_ARC_ADD_PROCESSING(WORK.DEMO, DF_SPE_ACC_ID, COMMENT, RECIPIENT_ID, IS_REFERENCE, IS_ENDORSER, PROCESS_FROM, PROCESS_TO, NEEDED_BY, REGARDS_TO, REGARDS_CODE, CREATED_BY, 1, 'TPDFR', 'UTNW017');

