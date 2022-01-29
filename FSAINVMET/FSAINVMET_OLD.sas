/*Live*/
%LET RPTLIB = Z:\Batch\FTP;
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= SCRA;
/*Test*/
/*%LET RPTLIB = T:\SAS;*/
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= SCRA;*/
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

FILENAME REPORTZ "&RPTLIB/FSAINVMET.FSAINVMETRZ_&SYSDATE";
FILENAME REPORT2 "&RPTLIB/FSAINVMET.FSAINVMETR2_&SYSDATE";
FILENAME REPORT3 "&RPTLIB/FSAINVMET.FSAINVMETR3_&SYSDATE";
FILENAME REPORT4 "&RPTLIB/FSAINVMET.FSAINVMETR4_&SYSDATE";

DATA _NULL_;
    RUN_DAY = today();
	CALL SYMPUT('RUNDATE',"'"||PUT(RUN_DAY, MMDDYYD10.)||"'");
    CALL SYMPUT('FIRST_DAY_PREV_MON',"'"||PUT(INTNX('MONTH',RUN_DAY,-1,'B'), MMDDYYD10.)||"'");
	CALL SYMPUT('FIRST_DAY_CURR_MON',"'"||PUT(INTNX('MONTH',RUN_DAY,0,'B'), MMDDYYD10.)||"'");
    CALL SYMPUT('LAST_DAY_PREV_MON',"'"||PUT(INTNX('MONTH',RUN_DAY,-1,'E'), MMDDYYD10.)||"'");
	CALL SYMPUT('LAST_DAY_CURR_MON',"'"||PUT(INTNX('MONTH',RUN_DAY,0,'E'), MMDDYYD10.)||"'");
    CALL SYMPUT('LAST_DAY_PREV_MON_SAS',"'"||PUT(INTNX('MONTH',RUN_DAY,-1,'E'), DATE9.)||"'d");
	CALL SYMPUT('LAST_DAY_CURR_MON_SAS',"'"||PUT(INTNX('MONTH',RUN_DAY,0,'E'), DATE9.)||"'d");
	CALL SYMPUT('THREE_YEARS_AGO',"'"||PUT(INTNX('YEAR',RUN_DAY,-3,'E'), MMDDYYD10.)||"'");
    CALL SYMPUTX('MOYR',PUT(TODAY(),MMYYN6.));
RUN;

OPTIONS MISSING=' ';
%SYSLPUT RUNDATE = &RUNDATE;
%SYSLPUT LAST_DAY_PREV_MON = &LAST_DAY_PREV_MON;
%SYSLPUT LAST_DAY_CURR_MON = &LAST_DAY_CURR_MON;
%SYSLPUT FIRST_DAY_PREV_MON = &FIRST_DAY_PREV_MON;
%SYSLPUT FIRST_DAY_CURR_MON = &FIRST_DAY_CURR_MON;
%SYSLPUT LAST_DAY_PREV_MON_SAS = &LAST_DAY_PREV_MON_SAS;
%SYSLPUT LAST_DAY_CURR_MON_SAS = &LAST_DAY_CURR_MON_SAS;
%SYSLPUT THREE_YEARS_AGO = &THREE_YEARS_AGO;

PROC SQL;
CREATE TABLE MIL AS
	SELECT
		B.BORROWERACCOUNTNUMBER AS DF_SPE_ACC_ID,
		DTES.BEG_DTE_T,
		CASE WHEN DTES.END_DTE_T IS NULL THEN 73048
			 ELSE DTES.END_DTE_T
		END AS END_DTE_T
	FROM 
		SQL.BORROWERS B
		INNER JOIN 
		(
			SELECT
				B.BORROWERACCOUNTNUMBER,
				MIN(AD.BEGINDATE) AS BEG_DTE_T,
				MAX(AD.ENDDATE) AS END_DTE_T
			FROM 
				SQL.BORROWERS B
				INNER JOIN SQL.ACTIVEDUTY AD
					ON B.BORROWERID = AD.BORROWERID
			GROUP BY B.BORROWERACCOUNTNUMBER
		)DTES
			ON B.BORROWERACCOUNTNUMBER = DTES.BORROWERACCOUNTNUMBER
;
QUIT;

DATA LEGEND.MIL (DROP = BEG_DTE_T END_DTE_T);
	SET MIL;
	BEG_DTE = DATEPART(BEG_DTE_T);
	END_DTE = DATEPART(END_DTE_T);
	FORMAT BEG_DTE END_DTE DATE9.;
RUN;

RSUBMIT;
LIBNAME PKUB DB2 DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
CONNECT TO DB2 (DATABASE=DNFPUTDL);
CREATE TABLE FIDRF0 AS
    SELECT
        *
    FROM CONNECTION TO DB2 
    (
        SELECT 
            LN10.BF_SSN,
            LN10.LN_SEQ,
            LN10.LC_STA_LON10,
            LN10.LD_STA_LON10,
            LN10.LD_LON_EFF_ADD,
            LN10.LD_LON_ACL_ADD,
            LN10.LD_PIF_RPT,
			LN10.LC_CAM_LON_STA,
/*(1)       LN10.LF_GTR_RFR,*/
            CASE WHEN LN10.IC_LON_PGM IN('DLPCNS','DLPLGB','DLPLUS','DLSCNS','DLSPCN','DLSSPL','DLSTFD','DLUCNS','DLUNST','DLUSPL','TEACH' ) 
					THEN 'DL'
                 WHEN LN10.IC_LON_PGM IN('CNSLDN','PLUS','PLUSGB','SLS','STFFRD','SUBCNS','SUBSPC','UNCNS','UNSPC','UNSTFD','FISL') 
					THEN 'FL'
                 	ELSE ''
            END AS IC_LON_PGM,
            LN10.LA_CUR_PRI AS LA_OTS_PRI_ELG,
            COALESCE(DW01.WA_TOT_BRI_OTS,DW01.LA_NSI_OTS,0) AS WA_TOT_BRI_OTS,
            CASE WHEN LN10.LA_CUR_PRI <= 0 
					THEN 0
                 WHEN LN10.LC_STA_LON10 = 'D' 
					THEN DAYS(CURRENT DATE) - DAYS(LN16.LD_DLQ_OCC) /* get current day delinquency*/
                 WHEN DW01.WC_DW_LON_STA IN ('04','05') 
					THEN 0
                 WHEN FORB.BF_SSN ^= '' 
					THEN 0
                 /*WHEN COALESCE(MAX(LN16.LN_DLQ_MAX) - DAY(CURRENT DATE) + 1,0) < 0 THEN 0*/
                	ELSE COALESCE(MAX(LN16.LN_DLQ_MAX) + 1,0)
            END AS LN_DLQ_MAX,
            CASE WHEN FORB.BF_SSN ^= '' 
					THEN '1'
                 	ELSE '0'
            END AS SPEC_FORB_IND,
            DW01.WC_DW_LON_STA,
            CASE WHEN LN10.LC_STA_LON10 = 'D' AND LN10.LA_CUR_PRI = 0 
					THEN 2
                 WHEN LN10.LD_PIF_RPT IS NOT NULL 
					THEN 1
                 	ELSE 0
            END AS ORD,
            CASE WHEN LN80.BF_SSN IS NULL 
					THEN 0
                 	ELSE 1
            END AS BILL_SATISFIED,
			SegmentLogic.SEGMENT AS SegmentMetric,
			LN10.LF_LON_CUR_OWN
        FROM 
            PKUB.LN10_LON LN10
            LEFT JOIN PKUB.DW01_DW_CLC_CLU DW01
                ON LN10.BF_SSN = DW01.BF_SSN
                AND LN10.LN_SEQ = DW01.LN_SEQ
            LEFT JOIN PKUB.LN16_LON_DLQ_HST LN16
                ON LN10.BF_SSN = LN16.BF_SSN
                AND LN10.LN_SEQ = LN16.LN_SEQ
                AND LN16.LC_STA_LON16 = '1'
                AND LN16.LC_DLQ_TYP = 'P'
            /*ASSIGN INDICATOR FOR FORBEARANCES*/
            LEFT JOIN
            (
                SELECT 
                    FB10.BF_SSN                                       
                FROM 
                    PKUB.FB10_BR_FOR_REQ FB10
                    INNER JOIN PKUB.LN60_BR_FOR_APV LN60
                        ON FB10.BF_SSN = LN60.BF_SSN
                        AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
                WHERE
                    FB10.LC_FOR_STA = 'A'
                    AND LN60.LC_STA_LON60 = 'A'
                    AND FB10.LC_STA_FOR10 = 'A'      
                    AND FB10.LC_FOR_TYP IN ('10','13','14','33','44')
                    AND LN60.LD_FOR_END >= &LAST_DAY_CURR_MON
                    AND LN60.LD_FOR_APL <= &RUNDATE
                    AND LN60.LD_FOR_BEG <= &RUNDATE
            ) FORB
                ON FORB.BF_SSN = LN10.BF_SSN
            LEFT JOIN PKUB.LN80_LON_BIL_CRF LN80
                ON LN10.BF_SSN = LN80.BF_SSN
                AND LN10.LN_SEQ = LN80.LN_SEQ
                AND LN80.LD_BIL_DU_LON BETWEEN &FIRST_DAY_CURR_MON AND &LAST_DAY_CURR_MON
                AND LN80.LA_TOT_BIL_STS >= COALESCE(LN80.LA_BIL_CUR_DU,0)
                AND LN80.LC_STA_LON80 = 'A'
                AND LN80.LC_BIL_TYP_LON = 'P'
			/*Adding in Segment logic*/
			LEFT JOIN
			(
				SELECT DISTINCT
					LN10.BF_SSN AS SSN,
					CASE WHEN 	LN09.BF_SSN IS NOT NULL 
							THEN '6'
						 WHEN	LN10ConPlus.BF_SSN IS NOT NULL 
							THEN '1' 
				 	     WHEN	LN10ConPlus.BF_SSN IS NULL
								AND SD10has01.LF_STU_SSN IS NOT NULL /*grauated*/
								AND COALESCE(SD10Max.LD_SCL_SPR, LN10DeferPlus.GradDeferDate, LN10GradPlus.GradRepayDate) > &THREE_YEARS_AGO 
							THEN '2' /*less than 3 years ago*/
						 WHEN	LN10ConPlus.BF_SSN IS NULL
								AND SD10has01.LF_STU_SSN IS NOT NULL /*grauated*/
								AND COALESCE(SD10Max.LD_SCL_SPR, LN10DeferPlus.GradDeferDate, LN10GradPlus.GradRepayDate) <= &THREE_YEARS_AGO 
							THEN '3' /*More than 3 years ago*/
						 WHEN	LN10ConPlus.BF_SSN IS NULL
								AND SD10has01.LF_STU_SSN IS NULL /*didnt grauate*/
								AND SD10Max.LD_SCL_SPR > &THREE_YEARS_AGO 
							THEN '4' /*Less than 3 years ago*/
					 	 WHEN	LN10ConPlus.BF_SSN IS NULL
								AND SD10has01.LF_STU_SSN IS NULL /*didnt grauate*/
								AND SD10Max.LD_SCL_SPR <= &THREE_YEARS_AGO 
							THEN '5' /*More than 3 years ago*/
					 	 WHEN   COALESCE(SD10Max.LD_SCL_SPR, LN10DeferPlus.GradDeferDate, LN10GradPlus.GradRepayDate) > &THREE_YEARS_AGO 
							THEN '2' /*Less than 3 years ago*/
					 	 WHEN	COALESCE(SD10Max.LD_SCL_SPR, LN10DeferPlus.GradDeferDate, LN10GradPlus.GradRepayDate) <= &THREE_YEARS_AGO 
							THEN '3' /*More than 3 years ago*/
							ELSE '7'
					END AS SEGMENT
				FROM 
					PKUB.LN10_LON LN10
					LEFT JOIN
					(
						SELECT DISTINCT
							BF_SSN
						FROM
							PKUB.LN10_LON
						WHERE
							IC_LON_PGM IN ('DLPLUS', 'DLCNSL', 'DLPCNS', 'DLSCCN', 'DLSCNS', 'DLSCPL', 'DLSCSC', 'DLSCSL', 'DLSCST', 'DLSCUC', 'DLSCUN', 'DLSPCN', 'DLSSPL', 'DLUCNS', 'DLUSPL', 'DSCON', 'DUCON', 'DLSCPG', 'DLPLGB')
					) LN10ConPlus 
						ON LN10ConPlus.BF_SSN = LN10.BF_SSN
					LEFT JOIN
					(
						SELECT DISTINCT
							LN10.BF_SSN,
							MAX(LN15.LD_DSB) - 180 Days AS GradRepayDate
						FROM
							PKUB.LN10_LON LN10
							INNER JOIN PKUB.LN15_DSB LN15
								ON LN15.BF_SSN = LN10.BF_SSN
								AND LN15.LN_SEQ = LN10.LN_SEQ
						WHERE
							LN10.IC_LON_PGM IN ('DGPLUS','DLPLGB')
							AND LN15.LC_STA_LON15 = '1'
							AND LN15.LD_DSB <= CURRENT DATE
						GROUP BY
							LN10.BF_SSN
					) LN10GradPlus 
						ON LN10GradPlus.BF_SSN = LN10.BF_SSN
					LEFT JOIN
					(
						SELECT DISTINCT
							LN10.BF_SSN,
							MAX(LN50.LD_DFR_END) - 180 Days AS GradDeferDate
						FROM
							PKUB.LN10_LON LN10
							INNER JOIN PKUB.LN50_BR_DFR_APV LN50
								ON LN50.BF_SSN = LN10.BF_SSN
								AND LN50.LN_SEQ = LN10.LN_SEQ
							INNER JOIN PKUB.DF10_BR_DFR_REQ DF10
								ON DF10.BF_SSN = LN50.BF_SSN
								AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
						WHERE
							LN10.IC_LON_PGM IN ('DGPLUS','DLPLGB')
							AND LN50.LC_STA_LON50 = 'A'
							AND DF10.LC_DFR_TYP IN ('15','16','18')
							AND DF10.LC_DFR_STA = 'A'
							AND DF10.LC_STA_DFR10 = 'A'
							AND LN50.LD_DFR_END <= CURRENT DATE
						GROUP BY
							LN10.BF_SSN
					) LN10DeferPlus
						ON LN10DeferPlus.BF_SSN = LN10.BF_SSN
					LEFT JOIN 
					(	
						SELECT DISTINCT
							LF_STU_SSN
						FROM
							PKUB.SD10_STU_SPR
						WHERE
							LC_REA_SCL_SPR = '01'
							AND LC_STA_STU10 = 'A'
					) SD10has01
						ON SD10has01.LF_STU_SSN = LN10.BF_SSN
					LEFT JOIN 
					(	
						SELECT DISTINCT
							LF_STU_SSN,
							MAX(LD_SCL_SPR) AS LD_SCL_SPR
						FROM
							PKUB.SD10_STU_SPR
						WHERE
							LC_STA_STU10 = 'A'
							AND LD_SCL_SPR <= CURRENT DATE
						GROUP BY
							LF_STU_SSN		
					) SD10Max
						ON SD10Max.LF_STU_SSN = LN10.BF_SSN
					LEFT JOIN
					(
						SELECT DISTINCT
							BF_SSN
						FROM 
							PKUB.LN09_RPD_PIO_CVN
						WHERE
							LD_LON_RHB_PCV IS NOT NULL OR COALESCE(IF_LON_SRV_DFL_LON,'                ') ^= '                '
					) LN09
						ON LN09.BF_SSN = LN10.BF_SSN
			) SegmentLogic
				ON SegmentLogic.SSN = LN10.BF_SSN 
        WHERE 
            LN10.LC_STA_LON10 IN ('R','D','L')
            AND LN10.LD_LON_ACL_ADD <= &RUNDATE
        GROUP BY 
            LN10.BF_SSN,
            LN10.LN_SEQ,
            LN10.LC_STA_LON10,
            LN10.LD_STA_LON10,
            LN10.LD_LON_EFF_ADD,
            LN10.LD_LON_ACL_ADD,
            LN10.LD_PIF_RPT,
			LN10.LC_CAM_LON_STA,
/*(1)       LN10.LF_GTR_RFR,*/
            CASE WHEN LN10.IC_LON_PGM IN('DLPCNS','DLPLGB','DLPLUS','DLSCNS','DLSPCN','DLSSPL','DLSTFD','DLUCNS','DLUNST','DLUSPL','TEACH') 
					THEN 'DL'
                 WHEN LN10.IC_LON_PGM IN('CNSLDN','PLUS','PLUSGB','SLS','STFFRD','SUBCNS','SUBSPC','UNCNS','UNSPC','UNSTFD','FISL') 
					THEN 'FL'
                 	ELSE ''
            END, 
            LN10.LA_CUR_PRI,
            COALESCE(DW01.WA_TOT_BRI_OTS,DW01.LA_NSI_OTS,0),
            LN10.LC_STA_LON10,
            LN10.LC_SST_LON10,
            LN16.LD_DLQ_OCC,
            LN10.LA_CUR_PRI,
			DW01.WC_DW_LON_STA,
            FORB.BF_SSN,
            CASE WHEN LN80.BF_SSN IS NULL 
					THEN 0
                 	ELSE 1
            END,
			SegmentLogic.SEGMENT,
			LN10.LF_LON_CUR_OWN
        ORDER BY
            LN10.BF_SSN

FOR READ ONLY WITH UR
    )
;
DISCONNECT FROM DB2;
QUIT;

/*This is to create the population for the R4 file to review active delinquencies on borrowers with a deferment or forbearanace*/
PROC SQL;
CONNECT TO DB2 (DATABASE=DNFPUTDL);
CREATE TABLE DFDELQ AS
    SELECT
        *
    FROM CONNECTION TO DB2 
    (
        SELECT 
            PD10.DF_SPE_ACC_ID,
            LN10.LN_SEQ,
            LN16.LN_DLQ_MAX,
            FORB.LC_FOR_TYP,
            DEFR.LC_DFR_TYP
        FROM 
			PKUB.LN10_LON LN10
            INNER JOIN PKUB.PD10_PRS_NME PD10
                ON LN10.BF_SSN = PD10.DF_PRS_ID
            INNER JOIN PKUB.LN16_LON_DLQ_HST LN16
                ON LN10.BF_SSN = LN16.BF_SSN
                AND LN10.LN_SEQ = LN16.LN_SEQ
                AND LN16.LC_STA_LON16 = '1'
                AND LN16.LC_DLQ_TYP = 'P'                       
            LEFT JOIN
            (
                SELECT 
                    FB10.BF_SSN,
                    LN60.LN_SEQ,
                    FB10.LC_FOR_TYP
                FROM 
                    PKUB.FB10_BR_FOR_REQ FB10
                    INNER JOIN PKUB.LN60_BR_FOR_APV LN60
                        ON FB10.BF_SSN = LN60.BF_SSN
                        AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
                WHERE
                    FB10.LC_FOR_STA = 'A'
                    AND LN60.LC_STA_LON60 = 'A'
                    AND FB10.LC_STA_FOR10 = 'A'      
                    AND LN60.LD_FOR_END >= &LAST_DAY_CURR_MON
                    AND LN60.LD_FOR_APL <= &RUNDATE
                    AND LN60.LD_FOR_BEG <= &RUNDATE
            ) FORB
                ON FORB.BF_SSN = LN10.BF_SSN
                AND FORB.LN_SEQ = LN10.LN_SEQ
            LEFT JOIN
                (
                    SELECT 
                        DF10.BF_SSN,
                        LN50.LN_SEQ,
                        DF10.LC_DFR_TYP 
                    FROM 
                        PKUB.DF10_BR_DFR_REQ DF10
                        INNER JOIN PKUB.LN50_BR_DFR_APV LN50
                            ON DF10.BF_SSN = LN50.BF_SSN
                            AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
                    WHERE
                        DF10.LC_DFR_STA = 'A'
                        AND LN50.LC_STA_LON50 = 'A'
                        AND DF10.LC_STA_DFR10 = 'A'      
                        AND LN50.LD_DFR_END >= &LAST_DAY_CURR_MON
                        AND LN50.LD_DFR_APL <= &RUNDATE
                        AND LN50.LD_DFR_BEG <= &RUNDATE
                ) DEFR
                ON DEFR.BF_SSN = LN10.BF_SSN
                AND DEFR.LN_SEQ = LN10.LN_SEQ                    
        WHERE 
            LN10.LC_STA_LON10 IN ('R','D','L')
            AND LN10.LA_CUR_PRI > 0
            AND 
			(
				FORB.BF_SSN IS NOT NULL 
				OR DEFR.BF_SSN IS NOT NULL
			)                        
        ORDER BY
            PD10.DF_SPE_ACC_ID,
            LN10.LN_SEQ

FOR READ ONLY WITH UR
    )
;
DISCONNECT FROM DB2;
QUIT;

PROC SQL;
CONNECT TO DB2 (DATABASE=DNFPUTDL);
CREATE TABLE MILT_STA AS
    SELECT DISTINCT
        LN10.BF_SSN
        ,'Y' AS IS_ACTIVE_MILT
    FROM
        PKUB.PD10_PRS_NME PD10
        INNER JOIN PKUB.LN10_LON LN10
            ON PD10.DF_PRS_ID = LN10.BF_SSN
        INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
            ON LN10.BF_SSN = DW01.BF_SSN
            AND LN10.LN_SEQ = DW01.LN_SEQ
        INNER JOIN 
		(
            SELECT DISTINCT
                AY10.BF_SSN
                ,COALESCE(MIL.BEG_DTE, LN72.LD_ITR_EFF_BEG, THIRTY_EIGHT.LD_DFR_BEG, FORTY.LD_DFR_BEG) AS BEGIN_DATE
                ,COALESCE(MIL.END_DTE, LN72.LD_ITR_EFF_END, THIRTY_EIGHT.LD_DFR_END, FORTY.LD_DFR_END) AS END_DATE
				,COALESCE(MIL.END_DTE, LN72.LD_ITR_EFF_END, THIRTY_EIGHT.LD_DFR_END, FORTY.LD_DFR_END) AS VALID_END_DATE
            FROM 
				PKUB.AY10_BR_LON_ATY AY10
                INNER JOIN 
				(
                    SELECT
                        AY10.BF_SSN
                        ,MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ
                    FROM 
                        PKUB.AY10_BR_LON_ATY AY10
                    WHERE 
                        AY10.PF_REQ_ACT = 'ASCRA'
                    GROUP BY 
                        AY10.BF_SSN
                ) ASCRA
                    ON AY10.BF_SSN = ASCRA.BF_SSN
                    AND AY10.LN_ATY_SEQ = ASCRA.LN_ATY_SEQ
                LEFT JOIN 
				(
                    SELECT
                        AY10.BF_SSN
                        ,MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ
                    FROM 
                        PKUB.AY10_BR_LON_ATY AY10
                    WHERE 
                        AY10.PF_REQ_ACT = 'ISCRA'
                    GROUP BY 
                        AY10.BF_SSN
                ) ISCRA
                    ON AY10.BF_SSN = ISCRA.BF_SSN
	            LEFT JOIN PKUB.LN72_INT_RTE_HST LN72
	                ON AY10.BF_SSN = LN72.BF_SSN
	                AND &LAST_DAY_CURR_MON_SAS BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
	                AND LN72.LC_STA_LON72 = 'A'
					AND LN72.LC_INT_RDC_PGM = 'M'
	            LEFT JOIN 
	            (
                    SELECT DISTINCT
                        DW01.BF_SSN
						,LN50.LD_DFR_BEG
						,LN50.LD_DFR_END
                    FROM
                        PKUB.DW01_DW_CLC_CLU DW01
                        INNER JOIN PKUB.LN50_BR_DFR_APV LN50
                            ON DW01.BF_SSN = LN50.BF_SSN
                            AND DW01.LN_SEQ = LN50.LN_SEQ
                            AND LN50.LC_STA_LON50 = 'A'
                            AND &LAST_DAY_CURR_MON_SAS BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
                        INNER JOIN PKUB.DF10_BR_DFR_REQ DF10
                            ON DW01.BF_SSN = DF10.BF_SSN
                            AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
                            AND DF10.LC_DFR_STA = 'A'
                            AND DF10.LC_STA_DFR10 = 'A'
                            AND DF10.LC_DFR_TYP = '38'
                    WHERE 
                        DW01.WC_DW_LON_STA = '04'
                ) THIRTY_EIGHT
	                ON AY10.BF_SSN = THIRTY_EIGHT.BF_SSN
	            LEFT JOIN 
                (
                    SELECT DISTINCT
                        DW01.BF_SSN
						,LN50.LD_DFR_BEG
						,LN50.LD_DFR_END
                    FROM
                        PKUB.DW01_DW_CLC_CLU DW01
                        INNER JOIN PKUB.LN50_BR_DFR_APV LN50
                            ON DW01.BF_SSN = LN50.BF_SSN
                            AND DW01.LN_SEQ = LN50.LN_SEQ
                            AND LN50.LC_STA_LON50 = 'A'
                            AND &LAST_DAY_CURR_MON_SAS BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
                        INNER JOIN PKUB.DF10_BR_DFR_REQ DF10
                            ON DW01.BF_SSN = DF10.BF_SSN
                            AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
                            AND DF10.LC_DFR_STA = 'A'
                            AND DF10.LC_STA_DFR10 = 'A'
                            AND DF10.LC_DFR_TYP = '40'
                    WHERE 
                        DW01.WC_DW_LON_STA = '04'
                ) FORTY
	                ON AY10.BF_SSN = FORTY.BF_SSN
                INNER JOIN PKUB.AY20_ATY_TXT AY20
                    ON AY10.BF_SSN = AY20.BF_SSN
                    AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
				INNER JOIN PKUB.PD10_PRS_NME PD10
					ON AY10.BF_SSN = PD10.DF_PRS_ID
				LEFT JOIN MIL 
					ON PD10.DF_SPE_ACC_ID = MIL.DF_SPE_ACC_ID
                WHERE
					AY10.PF_REQ_ACT = 'ASCRA'
                    AND 
					(
						(ASCRA.LN_ATY_SEQ > ISCRA.LN_ATY_SEQ)
                    	OR ISCRA.BF_SSN IS NULL
					)
            ) ARCS
            	ON LN10.BF_SSN = ARCS.BF_SSN
        LEFT JOIN PKUB.LN72_INT_RTE_HST LN72
            ON LN10.BF_SSN = LN72.BF_SSN
            AND &LAST_DAY_CURR_MON_SAS BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
            AND LN72.LC_STA_LON72 = 'A'
        LEFT JOIN 
        (
            SELECT DISTINCT
                DW01.BF_SSN
            FROM
                PKUB.DW01_DW_CLC_CLU DW01
                INNER JOIN PKUB.LN50_BR_DFR_APV LN50
                    ON DW01.BF_SSN = LN50.BF_SSN
                    AND DW01.LN_SEQ = LN50.LN_SEQ
                    AND LN50.LC_STA_LON50 = 'A'
                    AND &LAST_DAY_CURR_MON_SAS BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
                INNER JOIN PKUB.DF10_BR_DFR_REQ DF10
                    ON DW01.BF_SSN = DF10.BF_SSN
                    AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
                    AND DF10.LC_DFR_STA = 'A'
                    AND DF10.LC_STA_DFR10 = 'A'
                    AND DF10.LC_DFR_TYP = '38'
            WHERE 
                DW01.WC_DW_LON_STA = '04'
        ) THIRTY_EIGHT
            ON LN10.BF_SSN = THIRTY_EIGHT.BF_SSN
        LEFT JOIN 
        (
            SELECT DISTINCT
                DW01.BF_SSN
            FROM
                PKUB.DW01_DW_CLC_CLU DW01
                INNER JOIN PKUB.LN50_BR_DFR_APV LN50
                    ON DW01.BF_SSN = LN50.BF_SSN
                    AND DW01.LN_SEQ = LN50.LN_SEQ
                    AND LN50.LC_STA_LON50 = 'A'
                    AND &LAST_DAY_CURR_MON_SAS BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
                INNER JOIN PKUB.DF10_BR_DFR_REQ DF10
                    ON DW01.BF_SSN = DF10.BF_SSN
                    AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
                    AND DF10.LC_DFR_STA = 'A'
                    AND DF10.LC_STA_DFR10 = 'A'
                    AND DF10.LC_DFR_TYP = '40'
            WHERE 
                DW01.WC_DW_LON_STA = '04'
        ) FORTY
            ON LN10.BF_SSN = FORTY.BF_SSN
        LEFT JOIN
        (
            SELECT
                LN16.BF_SSN
                ,MAX(LN16.LN_DLQ_MAX) AS LN_DLQ_MAX
            FROM 
                PKUB.LN16_LON_DLQ_HST LN16
            WHERE 
                LN16.LC_STA_LON16 = '1'
                AND LN16.LN_DLQ_MAX >= 300
            GROUP BY 
                LN16.BF_SSN
        ) LN16
            ON LN10.BF_SSN = LN16.BF_SSN
    WHERE
        (COALESCE(LN10.LA_CUR_PRI,0) + COALESCE(DW01.WA_TOT_BRI_OTS,0) > 0)
        AND LN10.LC_STA_LON10 = 'R'
        AND
            (
                (
					LN72.LR_ITR = 0 
					AND LN72.LC_INT_RDC_PGM = 'S'
				)
                OR 
				(
					LN72.LR_ITR <= 6 
					AND LN72.LC_INT_RDC_PGM = 'M'
				)
                OR 
				(
					LN72.LR_ITR <= 6 
					AND LN10.LD_LON_1_DSB < ARCS.BEGIN_DATE 
					AND &LAST_DAY_CURR_MON_SAS <= ARCS.VALID_END_DATE
				)
                OR THIRTY_EIGHT.BF_SSN IS NOT NULL
                OR FORTY.BF_SSN IS NOT NULL
            )
;

CREATE TABLE FIDRF_REMOTE AS
	SELECT DISTINCT
	    FIDR.*,
	    COALESCE(MSTAB.IS_ACTIVE_MILT,MSTAE.IS_ACTIVE_MILT,'N') AS IS_ACTIVE_MILT
	FROM
	    FIDRF0 FIDR
	    LEFT JOIN MILT_STA MSTAB
	        ON FIDR.BF_SSN = MSTAB.BF_SSN
	    LEFT JOIN PKUB.LN20_EDS LN20
	        ON FIDR.BF_SSN = LN20.BF_SSN
	    LEFT JOIN MILT_STA MSTAE
	        ON LN20.LF_EDS = MSTAE.BF_SSN
;

PROC SQL;
CREATE TABLE OutputDemos AS

	SELECT DISTINCT
		PD10.DF_PRS_ID,
		PD10.DM_PRS_1,
		PD10.DM_PRS_LST,
		PD30.DX_STR_ADR_1,
		PD30.DX_STR_ADR_2,
		PD30.DX_STR_ADR_3,
		PD30.DM_CT,
		PD30.DC_DOM_ST,
		PD30.DF_ZIP_CDE,
		PD30.DM_FGN_CNY,
		PD30.DM_FGN_ST,
		PD30.DI_VLD_ADR,
		PD40_HOME.HOME_CONSENT,
		PD40_HOME.HOME_PHONE,
		PD40_ALT.ALT_CONSENT,
		PD40_ALT.ALT_PHONE,
		PD40_WORK.WORK_CONSENT,
		PD40_WORK.WORK_PHONE,
		F.LF_LON_CUR_OWN,
		COALESCE(AMT_DUE.AMOUNT_DUE, 0) AS AMOUNT_DUE,
		SUM(COALESCE(F.LA_OTS_PRI_ELG,0) + COALESCE(F.WA_TOT_BRI_OTS,0)) AS TOTAL_BALANCE,
		PD32.DX_ADR_EML,
		LN10.LF_FED_CLC_RSK 
	FROM 
		FIDRF0 F
		INNER JOIN PKUB.PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = F.BF_SSN
		LEFT OUTER JOIN 
		(
			SELECT
				LN10.BF_SSN,
				MAX(LN10.LF_FED_CLC_RSK) AS LF_FED_CLC_RSK
			FROM
				PKUB.LN10_LON LN10
				INNER JOIN 
				( 
					SELECT
						BF_SSN,
						MAX(LD_LON_1_DSB) AS LD_LON_1_DSB
					FROM
						PKUB.LN10_LON
					WHERE
						IC_LON_PGM NOT IN('DLCNSL','DLSCNS','DLSSPL','DLUCNS','DLUSPL','DLPCNS')
					GROUP BY
						BF_SSN
				) LN10Newest
					ON LN10Newest.BF_SSN = LN10.BF_SSN
					AND LN10Newest.LD_LON_1_DSB = LN10.LD_LON_1_DSB
			GROUP BY
				LN10.BF_SSN
		)LN10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		LEFT JOIN PKUB.PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
			AND PD30.DC_ADR = 'L'
		LEFT JOIN
		(
			SELECT DISTINCT
				PD10.DF_PRS_ID,
				COALESCE(PD32H.DX_ADR_EML, PD32A.DX_ADR_EML, PD32W.DX_ADR_EML) AS DX_ADR_EML
			FROM
				PKUB.PD10_PRS_NME PD10
				LEFT JOIN
				(
					SELECT
						PD32.DF_PRS_ID,
						PD32.DX_ADR_EML
					FROM
						PKUB.PD32_PRS_ADR_EML PD32
						INNER JOIN
						(
							SELECT
								DF_PRS_ID,
								MAX(DF_CRT_DTS_PD32) AS DateUpdated
							FROM	
								PKUB.PD32_PRS_ADR_EML
							WHERE
								DI_VLD_ADR_EML = 'Y'
								AND DC_STA_PD32 = 'A'
								AND DC_ADR_EML = 'H'
							GROUP BY
								DF_PRS_ID
						) PD32MAX
							ON PD32MAX.DF_PRS_ID = PD32.DF_PRS_ID
							AND PD32MAX.DateUpdated = PD32.DF_CRT_DTS_PD32	
					WHERE
						PD32.DI_VLD_ADR_EML = 'Y'
						AND PD32.DC_STA_PD32 = 'A'
						AND PD32.DC_ADR_EML = 'H'
				)PD32H
					ON PD10.DF_PRS_ID = PD32H.DF_PRS_ID
				LEFT JOIN
				(
					SELECT
						PD32.DF_PRS_ID,
						PD32.DX_ADR_EML
					FROM
						PKUB.PD32_PRS_ADR_EML PD32
						INNER JOIN
						(
							SELECT
								DF_PRS_ID,
								MAX(DF_CRT_DTS_PD32) AS DateUpdated
							FROM	
								PKUB.PD32_PRS_ADR_EML
							WHERE
								DI_VLD_ADR_EML = 'Y'
								AND DC_STA_PD32 = 'A'
								AND DC_ADR_EML = 'A'
							GROUP BY
								DF_PRS_ID
						) PD32MAX
							ON PD32MAX.DF_PRS_ID = PD32.DF_PRS_ID
							AND PD32MAX.DateUpdated = PD32.DF_CRT_DTS_PD32	
					WHERE
						PD32.DI_VLD_ADR_EML = 'Y'
						AND PD32.DC_STA_PD32 = 'A'
						AND PD32.DC_ADR_EML = 'A'
				)PD32A
					ON PD10.DF_PRS_ID = PD32A.DF_PRS_ID
				LEFT JOIN
				(
					SELECT
						PD32.DF_PRS_ID,
						PD32.DX_ADR_EML
					FROM
						PKUB.PD32_PRS_ADR_EML PD32
						INNER JOIN
						(
							SELECT
								DF_PRS_ID,
								MAX(DF_CRT_DTS_PD32) AS DateUpdated
							FROM	
								PKUB.PD32_PRS_ADR_EML
							WHERE
								DI_VLD_ADR_EML = 'Y'
								AND DC_STA_PD32 = 'A'
								AND DC_ADR_EML = 'W'
							GROUP BY
								DF_PRS_ID
						) PD32MAX
							ON PD32MAX.DF_PRS_ID = PD32.DF_PRS_ID
							AND PD32MAX.DateUpdated = PD32.DF_CRT_DTS_PD32	
					WHERE
						PD32.DI_VLD_ADR_EML = 'Y'
						AND PD32.DC_STA_PD32 = 'A'
						AND PD32.DC_ADR_EML = 'W'
				)PD32W
					ON PD10.DF_PRS_ID = PD32W.DF_PRS_ID
		) PD32
			ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
		LEFT JOIN
		(
			SELECT
				PD40.DF_PRS_ID,
				(PD40.DN_DOM_PHN_ARA || PD40.DN_DOM_PHN_XCH || PD40.DN_DOM_PHN_LCL) AS HOME_PHONE,
				CASE
					WHEN PD40.DC_ALW_ADL_PHN IN ('L','Q','P','X') THEN 'Y'
					ELSE 'N'
				END AS HOME_CONSENT
			FROM 
				PKUB.PD40_PRS_PHN PD40
			WHERE
				PD40.DC_PHN = 'H'
				AND PD40.DI_PHN_VLD = 'Y'
		)PD40_HOME
			ON PD40_HOME.DF_PRS_ID = PD10.DF_PRS_ID
		LEFT JOIN
		(
			SELECT
				PD40.DF_PRS_ID,
				(PD40.DN_DOM_PHN_ARA || PD40.DN_DOM_PHN_XCH || PD40.DN_DOM_PHN_LCL) AS ALT_PHONE,
				CASE
					WHEN PD40.DC_ALW_ADL_PHN IN ('L','Q','P','X') THEN 'Y'
					ELSE 'N'
				END AS ALT_CONSENT
			FROM 
				PKUB.PD40_PRS_PHN PD40
			WHERE
				PD40.DC_PHN = 'A'
				AND PD40.DI_PHN_VLD = 'Y'
		)PD40_ALT
			ON PD40_ALT.DF_PRS_ID = PD10.DF_PRS_ID
		LEFT JOIN
		(
			SELECT
				PD40.DF_PRS_ID,
				(PD40.DN_DOM_PHN_ARA || PD40.DN_DOM_PHN_XCH || PD40.DN_DOM_PHN_LCL) AS WORK_PHONE,
				CASE
					WHEN PD40.DC_ALW_ADL_PHN IN ('L','Q','P','X') THEN 'Y'
					ELSE 'N'
				END AS WORK_CONSENT
			FROM 
				PKUB.PD40_PRS_PHN PD40
			WHERE
				PD40.DC_PHN = 'W'
				AND PD40.DI_PHN_VLD = 'Y'

		)PD40_WORK
			ON PD40_WORK.DF_PRS_ID = PD10.DF_PRS_ID
		LEFT JOIN /*GETS THE BORROWERS CURRENT AMOUNT DUE*/
		(
			SELECT
				LN80.BF_SSN,
				SUM(LN80.LA_BIL_PAS_DU + LN80.LA_BIL_CUR_DU + LN80.LA_LTE_FEE_OTS_PRT) AS AMOUNT_DUE
			FROM
				PKUB.LN80_LON_BIL_CRF LN80
			WHERE 
				LN80.LC_BIL_TYP_LON = 'P'
				AND LN80.LC_STA_LON80 = 'A'
				AND LN80.LD_BIL_CRT > TODAY() - 31 
			GROUP BY
				LN80.BF_SSN
		) AMT_DUE
			ON AMT_DUE.BF_SSN = PD10.DF_PRS_ID
	WHERE
		(PD40_HOME.DF_PRS_ID IS NOT NULL OR PD40_ALT.DF_PRS_ID IS NOT NULL OR PD40_WORK.DF_PRS_ID IS NOT NULL OR PD32.DX_ADR_EML IS NOT NULL)/*HAS AT LEAST 1 VALID PHONE OR EMAIL*/
	GROUP BY 
		PD10.DF_PRS_ID,
		PD10.DM_PRS_1,
		PD10.DM_PRS_LST,
		PD30.DX_STR_ADR_1,
		PD30.DX_STR_ADR_2,
		PD30.DX_STR_ADR_3,
		PD30.DM_CT,
		PD30.DC_DOM_ST,
		PD30.DF_ZIP_CDE,
		PD30.DM_FGN_CNY,
		PD30.DM_FGN_ST,
		PD30.DI_VLD_ADR,
		PD40_HOME.HOME_CONSENT,
		PD40_HOME.HOME_PHONE,
		PD40_ALT.ALT_CONSENT,
		PD40_ALT.ALT_PHONE,
		PD40_WORK.WORK_CONSENT,
		PD40_WORK.WORK_PHONE,
		F.LF_LON_CUR_OWN,
		AMT_DUE.AMOUNT_DUE,
		PD32.DX_ADR_EML,
		LN10.LF_FED_CLC_RSK
		;
QUIT;

ENDRSUBMIT;
DATA FIDRF_REMOTE; SET LEGEND.FIDRF_REMOTE; RUN;
DATA DFDELQ; SET LEGEND.DFDELQ; RUN;
DATA OutputDemos; SET LEGEND.OutputDemos; RUN;


DATA _NULL_;
    CALL SYMPUT('locl_LAST_DAY_PREV_MON',INPUT(COMPRESS(&LAST_DAY_PREV_MON,"'"),MMDDYY10.));
    CALL SYMPUT('locl_FIRST_DAY_PREV_MON',INPUT(COMPRESS(&FIRST_DAY_PREV_MON,"'"),MMDDYY10.));
	CALL SYMPUT('locl_LAST_DAY_CURR_MON',INPUT(COMPRESS(&LAST_DAY_CURR_MON,"'"),MMDDYY10.));
    CALL SYMPUT('locl_FIRST_DAY_CURR_MON',INPUT(COMPRESS(&FIRST_DAY_CURR_MON,"'"),MMDDYY10.));
RUN;

DATA FIDRF1 STATUS_ERR;
    SET FIDRF_REMOTE;
    IF WC_DW_LON_STA IN ('06','88','98') THEN OUTPUT STATUS_ERR;
    ELSE OUTPUT FIDRF1;
RUN;

/*assign categories*/
%MACRO ASSIGN_CATS(INFILE,OUTFILE,ERRFILE);

    DATA &OUTFILE &ERRFILE;
        SET &INFILE;
        BOR_INV_NUM = _N_;

        LENGTH PF $3.;

        IF ORD=0 THEN
            DO;
                IF IS_ACTIVE_MILT = 'Y' THEN PF = '04';
                ELSE IF WC_DW_LON_STA IN ('02','23') AND LN_DLQ_MAX < 1 THEN PF = '01';
                ELSE IF WC_DW_LON_STA = '01' AND LN_DLQ_MAX < 1 THEN PF = '02';
                ELSE IF WC_DW_LON_STA IN ('03','20','09','10','11','15','16','18','20','22','17','19','21') THEN 
                    DO; /*22 WILL BE IN THIS GROUP, BUT NOT ALL LOANS PIF*/
                        IF SPEC_FORB_IND IN ('1') AND LN_DLQ_MAX = 0 THEN PF = '06';
						ELSE IF LN_DLQ_MAX = 0 AND LC_CAM_LON_STA = '02' THEN PF = '01';
						ELSE IF LN_DLQ_MAX = 0 AND LC_CAM_LON_STA = '01' THEN PF = '02';
                        ELSE IF 0 <= LN_DLQ_MAX <= 5 THEN PF = '03';
                        ELSE IF LN_DLQ_MAX <= 30 THEN PF = '07';
                        ELSE IF LN_DLQ_MAX <= 90 THEN PF = '08';
                        ELSE IF LN_DLQ_MAX <= 150 THEN PF = '09';
                        ELSE IF LN_DLQ_MAX <= 270 THEN PF = '10';
                        ELSE IF LN_DLQ_MAX <= 360 THEN PF = '11';
                        ELSE IF LN_DLQ_MAX > 360 THEN PF = '12';
                        ELSE PF = '99';
                    END;
                ELSE IF WC_DW_LON_STA = '04' THEN 
                    DO;
                        IF LN_DLQ_MAX = 0 AND BILL_SATISFIED = 1 THEN PF = '03';
                        ELSE IF LN_DLQ_MAX = 0 THEN PF = '05';
                        ELSE IF 1 <= LN_DLQ_MAX <= 5 THEN PF = '03';
                        ELSE IF LN_DLQ_MAX <= 30 THEN PF = '07';
                        ELSE IF LN_DLQ_MAX <= 90 THEN PF = '08';
                        ELSE IF LN_DLQ_MAX <= 150 THEN PF = '09';
                        ELSE IF LN_DLQ_MAX <= 270 THEN PF = '10';
                        ELSE IF LN_DLQ_MAX <= 360 THEN PF = '11';
                        ELSE IF LN_DLQ_MAX > 360 THEN PF = '12';
                        ELSE PF = '99';
                    END;
                ELSE IF WC_DW_LON_STA = '05' THEN
                    DO;
                        IF LN_DLQ_MAX = 0 AND BILL_SATISFIED = 1 THEN PF = '03';
                        ELSE IF LN_DLQ_MAX = 0 THEN PF = '06';
                        ELSE IF 1 <= LN_DLQ_MAX <= 5 THEN PF = '03';
                        ELSE IF LN_DLQ_MAX <= 30 THEN PF = '07';
                        ELSE IF LN_DLQ_MAX <= 90 THEN PF = '08';
                        ELSE IF LN_DLQ_MAX <= 150 THEN PF = '09';
                        ELSE IF LN_DLQ_MAX <= 270 THEN PF = '10';
                        ELSE IF LN_DLQ_MAX <= 360 THEN PF = '11';
                        ELSE IF LN_DLQ_MAX > 360 THEN PF = '12';
                        ELSE PF = '99';
                    END;
                ELSE IF LN_DLQ_MAX <= 5 THEN PF = '03';
                ELSE IF LN_DLQ_MAX <= 30 THEN PF = '07';
                ELSE IF LN_DLQ_MAX <= 90 THEN PF = '08';
                ELSE IF LN_DLQ_MAX <= 150 THEN PF = '09';
                ELSE IF LN_DLQ_MAX <= 270 THEN PF = '10';
                ELSE IF LN_DLQ_MAX <= 360 THEN PF = '11';
                ELSE IF LN_DLQ_MAX > 360 THEN PF = '12';
                ELSE PF = '99';
            END;
    /*  PIF OR DECONVERTED BORROWERS*/
        ELSE IF ORD=1 THEN PF = 'PIF';
        ELSE IF ORD=2 THEN PF = 'TRN';
        ELSE PF = 'PRV';

        IF ORD <= 2 THEN OUTPUT &OUTFILE;
        ELSE OUTPUT &ERRFILE;
    RUN;
%MEND ASSIGN_CATS;

%ASSIGN_CATS(FIDRF1,FIDRF,OTH_LNS);

PROC FREQ DATA=FIDRF NOPRINT;
    TABLE BF_SSN*WC_DW_LON_STA / OUT=STAT_TAB(DROP=PERCENT) ;
RUN;

PROC SORT DATA=STAT_TAB;
    BY BF_SSN COUNT;
RUN;

DATA STAT_TAB(KEEP=BF_SSN WC_DW_LON_STA);
    SET STAT_TAB;
    BY BF_SSN;
    IF LAST.BF_SSN;
RUN;

/*CREATE BORROWER LEVEL DATA SET */
PROC SQL;
CREATE TABLE FIDRF2 AS 
    SELECT DISTINCT 
        FIDR.BF_SSN,
/*(1)   FIDR.LF_GTR_RFR,*/
 		FIDR.LD_LON_EFF_ADD,
        CASE WHEN HeirarchyStatus.BF_SSN IS NULL THEN Stat.WC_DW_LON_STA 
			 WHEN HeirarchyStatus.BF_SSN IS NOT NULL AND Stat.WC_DW_LON_STA = '22' THEN HeirarchyStatus.WC_DW_LON_STA
			 ELSE Stat.WC_DW_LON_STA END AS WC_DW_LON_STA,
        FIDR.SPEC_FORB_IND,
/*(1)   SUBSTR(FIDR.LF_GTR_RFR,6) AS IID,*/
        MAX(LN_DLQ_MAX) AS LN_DLQ_MAX,
		FIDR.LC_CAM_LON_STA,
        SUM(COALESCE(FIDR.LA_OTS_PRI_ELG,0)) AS LA_CUR_PRI,
        SUM(INT((ROUND(FIDR.WA_TOT_BRI_OTS,.000001)) * 100) / 100) AS WA_TOT_BRI_OTS,
        CALCULATED LA_CUR_PRI + CALCULATED WA_TOT_BRI_OTS AS TOT_AMT,
        CASE WHEN COUNT(DISTINCT FIDR.IC_LON_PGM) = 1 THEN FIDR.IC_LON_PGM
             ELSE 'MX'
        END AS LON_PGM,
		COUNT(DISTINCT FIDR.LN_SEQ) AS LOAN_COUNT,
        COALESCE(PifPrevious.PIF_CT_B4_REP_MO,0) AS PIF_CT_B4_REP_MO,
        COALESCE(PifCurrent.PIF_CT_REP_MO,0) AS PIF_CT_REP_MO,
        COALESCE(DcvPrevious.DSTAT_CT_B4_REP_MO,0) AS DSTAT_CT_B4_REP_MO,
        COALESCE(DcvCurrent.DSTAT_CT_REP_MO,0) AS DSTAT_CT_REP_MO,
        MAX(LD_PIF_RPT) AS LD_PIF_RPT,
        MAX(LD_STA_LON10) AS LD_STA_LON10,
        IS_ACTIVE_MILT,
        MIN(BILL_SATISFIED) AS BILL_SATISFIED,
		FIDR.SegmentMetric AS SEGMENT
        FROM
        FIDRF FIDR
		LEFT OUTER JOIN 
		(
			SELECT DISTINCT
				BF_SSN,
				MIN(WC_DW_LON_STA) AS WC_DW_LON_STA /*take least billed*/
			FROM
				FIDRF
			WHERE
				WC_DW_LON_STA IN('01','02','23')
			GROUP BY
				BF_SSN
		) HeirarchyStatus
			ON HeirarchyStatus.BF_SSN = FIDRF.BF_SSN
        INNER JOIN STAT_TAB Stat
            ON FIDR.BF_SSN = Stat.BF_SSN
		/*GET COUNT OF PIF LOANS PREVIOUS TO THE RUN MONTH*/
        LEFT JOIN
        (
            SELECT
                BF_SSN,
/*(1)           LF_GTR_RFR,*/
				COUNT(*) AS PIF_CT_B4_REP_MO
            FROM 
                FIDRF 
            WHERE 
                &locl_FIRST_DAY_CURR_MON > LD_PIF_RPT 
                AND LD_PIF_RPT IS NOT NULL 
            GROUP BY 
                BF_SSN
/*(1)           LF_GTR_RFR*/
		) PifPrevious
            ON FIDR.BF_SSN = PifPrevious.BF_SSN
/*(1)       AND FIDR.LF_GTR_RFR = PifPrevious.LF_GTR_RFR*/
/*GET COUNT OF PIF LOANS IN THE RUN MONTH*/
        LEFT JOIN
        (
            SELECT
                BF_SSN,
/*(1)           LF_GTR_RFR,*/
                COUNT(*) AS PIF_CT_REP_MO
            FROM 
                FIDRF 
            WHERE 
                &locl_FIRST_DAY_CURR_MON <= LD_PIF_RPT <= &locl_LAST_DAY_CURR_MON 
            GROUP BY
                BF_SSN
/*(1)           LF_GTR_RFR*/
        ) PifCurrent
            ON FIDR.BF_SSN = PifCurrent.BF_SSN
/*(1)       AND FIDR.LF_GTR_RFR = PifCurrent.LF_GTR_RFR*/
/*GET COUNT OF DCONVERTED LOANS PREVIOUS TO THE RUN MONTH*/
        LEFT JOIN
        (
            SELECT 
                BF_SSN,
/*(1)           LF_GTR_RFR*/
                COUNT(*) AS DSTAT_CT_B4_REP_MO
            FROM 
                FIDRF 
            WHERE 
                LC_STA_LON10 = 'D' 
                AND LA_OTS_PRI_ELG  = 0 
                AND &locl_FIRST_DAY_CURR_MON > LD_STA_LON10  
                AND LD_STA_LON10 IS NOT NULL
            GROUP BY 
                BF_SSN
/*(1)           LF_GTR_RFR*/
        ) DcvPrevious
            ON FIDR.BF_SSN = DcvPrevious.BF_SSN
/*(1)       AND FIDR.LF_GTR_RFR = DcvPrevious.LF_GTR_RFR*/
/*GET COUNT OF DCONVERTED LOANS IN THE RUN MONTH*/
        LEFT JOIN 
        (
            SELECT 
                BF_SSN,
/*(1)           LF_GTR_RFR*/
                COUNT(*) AS DSTAT_CT_REP_MO
            FROM 
                FIDRF 
            WHERE 
                LC_STA_LON10 = 'D' 
                AND LA_OTS_PRI_ELG  = 0
                AND &locl_FIRST_DAY_CURR_MON <= LD_STA_LON10 <= &locl_LAST_DAY_CURR_MON
            GROUP BY 
                BF_SSN
/*(1)           LF_GTR_RFR*/
        ) DcvCurrent
            ON FIDR.BF_SSN = DcvCurrent.BF_SSN
/*(1)               AND FIDR.LF_GTR_RFR = DcvCurrent.LF_GTR_RFR*/
        GROUP BY 
            FIDR.BF_SSN
/*(1)       FIDR.LF_GTR_RFR*/
        ORDER BY 
/*(1)       CALCULATED IID,*/
            FIDR.BF_SSN
    ;
QUIT;


/*DETERMINE PIF BORROWERS AND DECONVERTED BORROWERS FOR THE REPORTING MONTH*/
DATA FIDRF2;
    SET FIDRF2;
/*    DETERMINE PIF BORROWERS FOR THE REPORTING MONTH*/
    IF SUM(PIF_CT_B4_REP_MO,PIF_CT_REP_MO) = LOAN_COUNT THEN DO;
        IF PIF_CT_REP_MO ^= 0 THEN ORD = 1;
        ELSE ORD = 3;
    END;
/*DETERMINE DECONVERTED BORROWERS FOR THE REPORTING MONTH*/
    ELSE IF SUM(DSTAT_CT_B4_REP_MO,DSTAT_CT_REP_MO) = LOAN_COUNT THEN DO;
        IF DSTAT_CT_REP_MO ^= 0 THEN ORD = 2;
        ELSE ORD = 4;
    END;
/*PIF and Deconverted MIX*/
    ELSE IF SUM(PIF_CT_B4_REP_MO,PIF_CT_REP_MO,DSTAT_CT_B4_REP_MO,DSTAT_CT_REP_MO)= LOAN_COUNT THEN
        DO;
            IF DSTAT_CT_REP_MO ^= 0 THEN ORD = 2;
            ELSE IF PIF_CT_REP_MO ^= 0 THEN ORD = 1;
            ELSE ORD = 4;
        END;
    ELSE
        DO;
            ORD = 0;
        END;

    IF ORD = 1 THEN PIF_TRN_DT = LD_PIF_RPT;
    ELSE IF ORD = 2 THEN PIF_TRN_DT = LD_STA_LON10;
    ELSE PIF_TRN_DT = .;

RUN;

PROC SORT DATA=FIDRF2 DUPOUT=DUPDS NODUPKEY; 
/*(1)   BY ORD LF_GTR_RFR BF_SSN; */
    BY ORD BF_SSN; 
RUN;

%ASSIGN_CATS(FIDRF2,FIDRF3,OTH_BRWS);

DATA FIDRF3;
	SET FIDRF3;
	
	/*Set SEGMENT = 0 for categories 1,2,4,5,6*/
	IF PF IN('01','02','04','05','06') THEN SEGMENT = 0;
RUN;

PROC SORT DATA=FIDRF3 NODUPKEY; 
    BY BOR_INV_NUM; 
RUN;

PROC FORMAT;
	PICTURE FSABAL LOW-HIGH = '9999999.99';
RUN;

/*R2 report*/

PROC SQL;

CREATE TABLE PFTotals AS
SELECT DISTINCT
	PF AS PF,
	COUNT(DISTINCT BF_SSN) AS BorrowerCount
FROM 
	FIDRF3 
GROUP BY
	PF
;

CREATE TABLE FSAMetricTracking AS
SELECT DISTINCT
	CASE WHEN PF = '03' THEN 'Metric1'
	     WHEN PF IN('09','10') THEN 'Metric2'
		 WHEN PF = '11' THEN 'Metric3'
		 ELSE 'Other' 
	END AS Metric,
	SEGMENT,
	COUNT(DISTINCT BF_SSN) AS BorrowerCount
FROM 
	FIDRF3 
WHERE
	PF IN('03','07','08','09','10','11')
GROUP BY
	CASE WHEN PF = '03' THEN 'Metric1'
	     WHEN PF IN('09','10') THEN 'Metric2'
		 WHEN PF = '11' THEN 'Metric3'
		 ELSE 'Other' 
	END,
	SEGMENT
;

CREATE TABLE Segments AS
SELECT DISTINCT
	Metric,
	SEGMENT
FROM
	FSAMetricTracking
WHERE
	SEGMENT ^= '7'
;

CREATE TABLE SegmentSummary AS
SELECT
	S.Metric,
	S.SEGMENT,
	SUM(COALESCE(F.BorrowerCount,0)) AS BorrowerCount
FROM
	Segments S
	LEFT OUTER JOIN FSAMetricTracking F
		ON F.SEGMENT = S.SEGMENT
		AND F.Metric = S.Metric
GROUP BY
	S.Metric,
	S.SEGMENT
;

CREATE TABLE SegmentTotal AS
SELECT
	S.SEGMENT,
	SUM(S.BorrowerCount) AS SegmentTotal
FROM
	SegmentSummary S
GROUP BY
	S.SEGMENT
;

CREATE TABLE MetricTotal AS
SELECT
	S.Metric,
	SUM(S.BorrowerCount) AS MetricTotal
FROM
	SegmentSummary S
GROUP BY
	S.Metric
;

CREATE TABLE MetricSegmentSummary AS
SELECT 
	SD.Metric, 
	SD.Segment,
	SD.BorrowerCount,
	COALESCE(ST.SegmentTotal,0) AS SegmentTotal,
	COALESCE(MT.MetricTotal,0) AS MetricTotal,
	SD.BorrowerCount/ST.SegmentTotal AS SegmentPercentage,
	SD.BorrowerCount/MT.MetricTotal AS MetricPercentage
FROM 
	SegmentSummary SD 
	LEFT OUTER JOIN SegmentTotal ST 
		ON ST.SEGMENT = SD.SEGMENT
	LEFT OUTER JOIN MetricTotal MT 
		ON MT.Metric = SD.Metric
ORDER BY
	SD.Metric,
	SD.Segment
;
QUIT;

/*R2 output*/
PROC EXPORT
	DATA=MetricSegmentSummary
	OUTFILE = "&RPTLIB\FSAINVMETR2 - &SYSDATE"
	DBMS = EXCEL
	REPLACE;
	SHEET = "Metrics";
RUN;
PROC EXPORT
	DATA=PFTotals
	OUTFILE = "&RPTLIB\FSAINVMETR2 - &SYSDATE"
	DBMS = EXCEL
	REPLACE;
	SHEET = "PeformanceCategory";
RUN;

/*R3 Output*/
PROC SQL;

CREATE TABLE DialerFile AS
	SELECT
		D.DF_PRS_ID,
		D.DM_PRS_1||' '||D.DM_PRS_LST AS NAME,
		F.LN_DLQ_MAX AS DAYS_DELQ,
		D.HOME_CONSENT,
		D.HOME_PHONE,
		D.ALT_CONSENT,
		D.ALT_PHONE,
		D.WORK_CONSENT,
		D.WORK_PHONE,
		D.DX_STR_ADR_1,
		D.DX_STR_ADR_2,
		D.DX_STR_ADR_3,
		D.DM_CT,
		D.DC_DOM_ST,
		D.DF_ZIP_CDE,		
		D.LF_LON_CUR_OWN,
		D.AMOUNT_DUE,
		D.TOTAL_BALANCE,
		F.SEGMENT,
		F.PF,
		D.LF_FED_CLC_RSK AS CRC_Code
	FROM
		FIDRF3 F
		INNER JOIN OutputDemos D
			ON F.BF_SSN = D.DF_PRS_ID
	WHERE
		(
			D.HOME_PHONE IS NOT NULL 
			OR D.ALT_PHONE IS NOT NULL 
			OR D.WORK_PHONE IS NOT NULL
		)
		AND F.PF NOT IN('PIF','TRN')
;

CREATE TABLE EmailFile AS
	SELECT
		D.DM_PRS_1,
		D.DM_PRS_LST,
		D.DX_ADR_EML,
		F.SEGMENT,
		F.PF
	FROM
		FIDRF3 F
		INNER JOIN OutputDemos D
			ON F.BF_SSN = D.DF_PRS_ID
	WHERE
		D.DX_ADR_EML IS NOT NULL
		AND F.PF NOT IN('PIF','TRN') 
;

QUIT;

/*R3*/
DATA _NULL_;
    SET DialerFile;
    FILE REPORT3 DROPOVER LRECL=32767;
    DO;
        PUT @1 DF_PRS_ID   			
		@15 NAME
		@70 DAYS_DELQ
		@159 HOME_CONSENT
		@160 HOME_PHONE
		@175 ALT_CONSENT
		@176 ALT_PHONE
		@191 WORK_CONSENT
		@192 WORK_PHONE
		@260 DX_STR_ADR_1
		@290 DX_STR_ADR_2
		@320 DX_STR_ADR_3
		@350 DM_CT
		@370 DC_DOM_ST
		@372 DF_ZIP_CDE
		@414 LF_LON_CUR_OWN
		@422 AMOUNT_DUE
		@429 TOTAL_BALANCE
		@440 SEGMENT
		@443 PF
		@446 CRC_Code
;
    END;
RUN;

/*R4*/
DATA _NULL_;
    SET EmailFile;
    FILE REPORT4 DELIMITER=',' DSD DROPOVER LRECL=32767;
   
    IF _N_ = 1 THEN
        DO;
            PUT "First Name, Last Name, Email Address, Segment, Peformance Category";
        END;

    DO;
        PUT DM_PRS_1 $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_ADR_EML $ @;
		PUT SEGMENT @;
		PUT PF;
    END;
RUN;



/*(1) commented out IID and LF_GTR_RFR as we won't get that information from FSA for a while but it will need to be reinstated at some point*/
