/*Live*/
%LET RPTLIB = Z:\Batch\FTP;
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= SCRA;
/*Test*/
/*%LET RPTLIB = T:\SAS;
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= SCRA;*/
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

FILENAME REPORTZ "&RPTLIB/UNWC22.NWC22RZ_&SYSDATE";
FILENAME REPORT2 "&RPTLIB/UNWC22.NWC22R2_&SYSDATE";
FILENAME REPORT3 "&RPTLIB/UNWC22.NWC22R3_&SYSDATE";
FILENAME REPORT4 "&RPTLIB/UNWC22.NWC22R4_&SYSDATE";
FILENAME REPORT5 "&RPTLIB/UNWC22.NWC22R5_&SYSDATE";
FILENAME REPORT6 "&RPTLIB/UNWC22.NWC22R6_&SYSDATE";
FILENAME REPORT7 "&RPTLIB/UNWC22.NWC22R7_&SYSDATE";
FILENAME REPORT8 "&RPTLIB/UNWC22.NWC22R8_&SYSDATE";
FILENAME REPORT9 "&RPTLIB/UNWC22.NWC22R9_&SYSDATE";
FILENAME REPORT10 "&RPTLIB/UNWC22.NWC22R10_&SYSDATE";
FILENAME REPORT11 "&RPTLIB/UNWC22.NWC22R11_&SYSDATE";
FILENAME REPORT12 "&RPTLIB/UNWC22.NWC22R12_&SYSDATE";
FILENAME REPORT13 "&RPTLIB/UNWC22.NWC22R13_&SYSDATE";
FILENAME REPORT14 "&RPTLIB/UNWC22.NWC22R14_&SYSDATE";
FILENAME REPORT15 "&RPTLIB/UNWC22.NWC22R15_&SYSDATE";
FILENAME REPORT16 "&RPTLIB/UNWC22.NWC22R16_&SYSDATE";
FILENAME REPORT17 "&RPTLIB/UNWC22.NWC22R17_&SYSDATE";
FILENAME REPORT18 "&RPTLIB/UNWC22.NWC22R18_&SYSDATE";
FILENAME REPORT19 "&RPTLIB/UNWC22.NWC22R19_&SYSDATE";
FILENAME REPORT20 "&RPTLIB/UNWC22.NWC22R20_&SYSDATE";

DATA _NULL_;
    RUN_DAY = today();
    CALL SYMPUT('FIRST_DAY_PREV_MON',"'"||PUT(INTNX('MONTH',RUN_DAY,-1,'B'), MMDDYYD10.)||"'");
    CALL SYMPUT('LAST_DAY_PREV_MON',"'"||PUT(INTNX('MONTH',RUN_DAY,-1,'E'), MMDDYYD10.)||"'");
    CALL SYMPUT('LAST_DAY_PREV_MON_SAS',"'"||PUT(INTNX('MONTH',RUN_DAY,-1,'E'), DATE9.)||"'d");
	CALL SYMPUT('THREE_YEARS_AGO',"'"||PUT(INTNX('YEAR',RUN_DAY,-3,'B'), MMDDYYD10.)||"'");
    CALL SYMPUTX('MOYR',PUT(TODAY(),MMYYN6.));
RUN;

OPTIONS MISSING=' ';
%SYSLPUT LAST_DAY_PREV_MON = &LAST_DAY_PREV_MON;
%SYSLPUT FIRST_DAY_PREV_MON = &FIRST_DAY_PREV_MON;
%SYSLPUT LAST_DAY_PREV_MON_SAS = &LAST_DAY_PREV_MON_SAS;
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
            CASE WHEN LN10.IC_LON_PGM IN('DLPCNS','DLPLGB','DLPLUS','DLSCNS','DLSPCN','DLSSPL','DLSTFD','DLUCNS','DLUNST','DLUSPL','TEACH') 
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
					THEN DAYS(CURRENT DATE) - DAYS(LN16.LD_DLQ_OCC) - DAY(CURRENT DATE)
                 WHEN DW01.WC_DW_LON_STA IN ('04','05') /* Forbearance or Deferment status*/
					THEN 0
                 WHEN FORB.BF_SSN ^= '' 
					THEN 0
                 WHEN COALESCE(MAX(LN16.LN_DLQ_MAX) - DAY(CURRENT DATE) + 1,0) < 0 
					THEN 0
                 	ELSE COALESCE(MAX(LN16.LN_DLQ_MAX) - DAY(CURRENT DATE) + 1,0)
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
			SegmentLogic.SEGMENT AS SegmentMetric
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
                    AND FB10.LC_FOR_TYP IN ('10','13','14','33','44') /*only include these forb types*/
                    AND LN60.LD_FOR_END >= &LAST_DAY_PREV_MON
                    AND LN60.LD_FOR_APL <= &LAST_DAY_PREV_MON
                    AND LN60.LD_FOR_BEG <= &LAST_DAY_PREV_MON
            ) FORB
                ON FORB.BF_SSN = LN10.BF_SSN
            LEFT JOIN PKUB.LN80_LON_BIL_CRF LN80
                ON LN10.BF_SSN = LN80.BF_SSN
                AND LN10.LN_SEQ = LN80.LN_SEQ
                AND LN80.LD_BIL_DU_LON BETWEEN &FIRST_DAY_PREV_MON AND &LAST_DAY_PREV_MON
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
            AND LN10.LD_LON_ACL_ADD <= &LAST_DAY_PREV_MON
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
			SegmentLogic.SEGMENT
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
                        AND LN60.LD_FOR_END >= &LAST_DAY_PREV_MON
                        AND LN60.LD_FOR_APL <= &LAST_DAY_PREV_MON
                        AND LN60.LD_FOR_BEG <= &LAST_DAY_PREV_MON
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
                        AND LN50.LD_DFR_END >= &LAST_DAY_PREV_MON
                        AND LN50.LD_DFR_APL <= &LAST_DAY_PREV_MON
                        AND LN50.LD_DFR_BEG <= &LAST_DAY_PREV_MON
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
	                AND &LAST_DAY_PREV_MON_SAS BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
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
                            AND &LAST_DAY_PREV_MON_SAS BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
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
                            AND &LAST_DAY_PREV_MON_SAS BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
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
                    AND (ASCRA.LN_ATY_SEQ > ISCRA.LN_ATY_SEQ)
                    OR ISCRA.BF_SSN IS NULL
            ) ARCS
            	ON LN10.BF_SSN = ARCS.BF_SSN
        LEFT JOIN PKUB.LN72_INT_RTE_HST LN72
            ON LN10.BF_SSN = LN72.BF_SSN
            AND &LAST_DAY_PREV_MON_SAS BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
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
                    AND &LAST_DAY_PREV_MON_SAS BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
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
                    AND &LAST_DAY_PREV_MON_SAS BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
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
					AND &LAST_DAY_PREV_MON_SAS <= ARCS.VALID_END_DATE
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

DISCONNECT FROM DB2;
QUIT;

ENDRSUBMIT;
DATA FIDRF_REMOTE; SET LEGEND.FIDRF_REMOTE; RUN;
DATA DFDELQ; SET LEGEND.DFDELQ; RUN;

DATA _NULL_;
    CALL SYMPUT('locl_LAST_DAY_PREV_MON',INPUT(COMPRESS(&LAST_DAY_PREV_MON,"'"),MMDDYY10.));
    CALL SYMPUT('locl_FIRST_DAY_PREV_MON',INPUT(COMPRESS(&FIRST_DAY_PREV_MON,"'"),MMDDYY10.));
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
                ELSE IF WC_DW_LON_STA IN ('02','23') AND LN_DLQ_MAX < 1 THEN PF = '01'; /*in school or not originated*/
                ELSE IF WC_DW_LON_STA = '01' AND LN_DLQ_MAX < 1 THEN PF = '02'; /*in grace*/
                ELSE IF WC_DW_LON_STA IN ('03','09','10','11','15','16','17','18','19','20','21','22') THEN /*Categorize based on delinquency*/
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
                ELSE IF WC_DW_LON_STA IN('04','05') THEN /*Deferments/Forbearances are based on delinquency*/
                    DO;
                        IF LN_DLQ_MAX = 0 AND BILL_SATISFIED = 1 THEN PF = '03';
                        ELSE IF LN_DLQ_MAX = 0 AND WC_DW_LON_STA = '04' THEN PF = '05';
						ELSE IF LN_DLQ_MAX = 0 AND WC_DW_LON_STA = '05' THEN PF = '06';
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
    IF LAST.BF_SSN; /*gets most frequent status occurance as the current account status*/
	/*if they have 1,2,or 23 and 22 is the most frequent, pick the other one*/
RUN;

/*CREATE BORROWER LEVEL DATA SET */
PROC SQL;
CREATE TABLE FIDRF2 AS 
    SELECT DISTINCT 
        FIDR.BF_SSN,
/*(1)   FIDR.LF_GTR_RFR,*/
 		FIDR.LD_LON_EFF_ADD,
        CASE WHEN HeirarchyStatus.BF_SSN IS NULL 
				THEN Stat.WC_DW_LON_STA 
			 WHEN HeirarchyStatus.BF_SSN IS NOT NULL AND Stat.WC_DW_LON_STA = '22' 
				THEN HeirarchyStatus.WC_DW_LON_STA
				ELSE Stat.WC_DW_LON_STA 
		END AS WC_DW_LON_STA,
        FIDR.SPEC_FORB_IND,
/*(1)   SUBSTR(FIDR.LF_GTR_RFR,6) AS IID,*/
        MAX(LN_DLQ_MAX) AS LN_DLQ_MAX,
		FIDR.LC_CAM_LON_STA,
        SUM(COALESCE(FIDR.LA_OTS_PRI_ELG,0)) AS LA_CUR_PRI,
        SUM(INT((ROUND(FIDR.WA_TOT_BRI_OTS,.000001)) * 100) / 100) AS WA_TOT_BRI_OTS,
        CALCULATED LA_CUR_PRI + CALCULATED WA_TOT_BRI_OTS AS TOT_AMT,
        CASE WHEN COUNT(DISTINCT FIDR.IC_LON_PGM) = 1 
				THEN FIDR.IC_LON_PGM
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
				WC_DW_LON_STA IN('01','02','23') /*in grace, school, or not originated*/
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
                &locl_FIRST_DAY_PREV_MON > LD_PIF_RPT 
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
                &locl_FIRST_DAY_PREV_MON <= LD_PIF_RPT <= &locl_LAST_DAY_PREV_MON 
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
                AND &locl_FIRST_DAY_PREV_MON > LD_STA_LON10  
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
                AND &locl_FIRST_DAY_PREV_MON <= LD_STA_LON10 <= &locl_LAST_DAY_PREV_MON
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
    IF SUM(PIF_CT_B4_REP_MO,PIF_CT_REP_MO) = LOAN_COUNT THEN 
		DO;
	        IF PIF_CT_REP_MO ^= 0 
				THEN ORD = 1;
	        	ELSE ORD = 3;
    END;
/*DETERMINE DECONVERTED BORROWERS FOR THE REPORTING MONTH*/
    ELSE IF SUM(DSTAT_CT_B4_REP_MO,DSTAT_CT_REP_MO) = LOAN_COUNT THEN 
		DO;
	        IF DSTAT_CT_REP_MO ^= 0 
				THEN ORD = 2;
	        	ELSE ORD = 4;
    END;
/*PIF and Deconverted MIX*/
    ELSE IF SUM(PIF_CT_B4_REP_MO,PIF_CT_REP_MO,DSTAT_CT_B4_REP_MO,DSTAT_CT_REP_MO)= LOAN_COUNT THEN
        DO;
            IF DSTAT_CT_REP_MO ^= 0 
				THEN ORD = 2;
            ELSE IF PIF_CT_REP_MO ^= 0 
				THEN ORD = 1;
            	ELSE ORD = 4;
        END;
    ELSE
        DO;
            ORD = 0;
        END;

    IF ORD = 1 
		THEN PIF_TRN_DT = LD_PIF_RPT;
    ELSE IF ORD = 2 
		THEN PIF_TRN_DT = LD_STA_LON10;
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
	IF PF IN('01','02','04','05','06') 
		THEN SEGMENT = 0;
RUN;

PROC SORT DATA=FIDRF3 NODUPKEY; 
    BY BOR_INV_NUM; 
RUN;

/*"C18" detail*/
DATA _NULL_;
    SET FIDRF3;
    FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
    FORMAT BF_SSN $9. LD_LON_EFF_ADD MMDDYY10. LON_PGM $2. PF $3. PIF_TRN_DT MMDDYY10.;
/*(1)	FORMAT IID $12.;*/

    IF _N_ = 1 THEN 
        DO;
/*          PUT "BORROWER SSN,FSA INVOICE NUMBER,DATE ADDED,PERFORMANCE CATEGORY,PRINCIPAL BALANCE,INTEREST BALANCE,TOTAL PRINCIPAL AND INTEREST,MAX DAYS DELINQUENT,TOTAL NUMBER OF LOANS,PIF DATE";*/
            PUT "BORROWER SSN,DATE ADDED,PERFORMANCE CATEGORY,PRINCIPAL BALANCE,INTEREST BALANCE,TOTAL PRINCIPAL AND INTEREST,MAX DAYS DELINQUENT,TOTAL NUMBER OF LOANS,LOAN PROGRAM,PIF DATE";
        END;
    DO;
        PUT BF_SSN $ @;
/*(1)       PUT IID $ @;*/
        PUT LD_LON_EFF_ADD @;
        PUT PF $ @;
        PUT LA_CUR_PRI @;
        PUT WA_TOT_BRI_OTS @;
        PUT TOT_AMT @;
        PUT LN_DLQ_MAX @;
        PUT LOAN_COUNT @;
        PUT LON_PGM @;
        PUT PIF_TRN_DT ;
    END;
RUN;

/*"C17 Jeremy" detail */
DATA _NULL_;
    SET FIDRF3;
    WHERE PF NOT IN ('PIF','TRN');

    FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;
    FORMAT 
        BF_SSN $9. 
        LD_LON_EFF_ADD MMDDYY10. 
/*(1)       IID $12. */
        LON_PGM $2. 
        PF $3. 
    ;
    IF _N_ = 1 THEN
        DO;
/*(1)       PUT "BORROWER SSN,FSA INVOICE NUMBER,DATE ADDED,PERFORMANCE CATEGORY,PRINCIPAL BALANCE,INTEREST BALANCE,TOTAL PRINCIPAL AND INTEREST,MAX DAYS DELINQUENT,TOTAL NUMBER OF LOANS";*/
            PUT "BORROWER SSN,DATE ADDED,PERFORMANCE CATEGORY,PRINCIPAL BALANCE,INTEREST BALANCE,TOTAL PRINCIPAL AND INTEREST,MAX DAYS DELINQUENT,TOTAL NUMBER OF LOANS";
        END;

    DO;
        PUT BF_SSN $ @;
/*(1)   PUT LF_GTR_RFR $ @;*/
        PUT LD_LON_EFF_ADD @;
        PUT PF $ @;
        PUT LA_CUR_PRI @;
        PUT WA_TOT_BRI_OTS @;
        PUT TOT_AMT @;
        PUT LN_DLQ_MAX @;
        PUT LOAN_COUNT;
    END;
RUN;

/*R4 file*/
DATA _NULL_;
    SET DFDELQ;  

    FILE REPORT4 DELIMITER=',' DSD DROPOVER LRECL=32767;
    FORMAT 
		DF_SPE_ACC_ID $10. 
		LN_SEQ LC_FOR_TYP LC_DFR_TYP $2.    
    ;

    IF _N_ = 1 THEN
        DO;
            PUT "ACCOUNT NUMBER,LOAN SEQUENCE,MAX DELINQUENCY,FORBEARANCE TYPE,DEFERMENT TYPE";
        END;

    DO;
        PUT DF_SPE_ACC_ID $ @;
        PUT LN_SEQ $ @;
        PUT LN_DLQ_MAX  @;
        PUT LC_FOR_TYP $ @;
        PUT LC_DFR_TYP $ ;
    END;
RUN;

/*Parse for each flat file category*/
DATA S7 S8 S9 S10 S11 S12 S13 S14 S15 S16 S17 S18 S19 S20;
    SET FIDRF3;
    IF PF = '01' THEN OUTPUT S7;
    IF PF = '02' THEN OUTPUT S8;
    IF PF = '03' THEN OUTPUT S9;
    IF PF = '04' THEN OUTPUT S10;
    IF PF = '05' THEN OUTPUT S11;
    IF PF = '06' THEN OUTPUT S12;
    IF PF = '07' THEN OUTPUT S13;
    IF PF = '08' THEN OUTPUT S14;
    IF PF = '09' THEN OUTPUT S15;
    IF PF = '10' THEN OUTPUT S16;
    IF PF = '11' THEN OUTPUT S17;
    IF PF = '12' THEN OUTPUT S18;
    IF PF = 'PIF' THEN OUTPUT S19;
    IF PF = 'TRN' THEN OUTPUT S20;
RUN;

%MACRO CNT(NUM);
DATA S&NUM;
    SET S&NUM;
    COUNTER = PUT(_N_,Z8.);
    FORMAT PF $2.;
RUN;
%MEND CNT;
%CNT(7);
%CNT(8);
%CNT(9);
%CNT(10);
%CNT(11);
%CNT(12);
%CNT(13);
%CNT(14);
%CNT(15);
%CNT(16);
%CNT(17);
%CNT(18);
%CNT(19);
%CNT(20);

PROC FORMAT;
	PICTURE FSABAL LOW-HIGH = '9999999.99';
RUN;

/*flat file macro*/
%MACRO OUTPT(NUM);
DATA _NULL_;
    SET S&NUM;
    FILE REPORT&NUM DROPOVER LRECL=32767;
	MONTH_NUMBER = MONTH(TODAY()) - 1;
	MONEND = PUT(INTNX('MONTH',TODAY(),-1,'E'),MMDDYYN8.);
	SELECT;
		WHEN (MONTH_NUMBER IN (1,2,4,5,7,8,10,11))
			METRIC = 0; /*set metric = 0 for non reporting months*/
		WHEN (MONTH_NUMBER IN (3,6,9,0)) /*December is 0 for this purpose*/
			METRIC = SEGMENT;
		OTHERWISE
			METRIC = 0;
	END;
	FORMAT LA_CUR_PRI WA_TOT_BRI_OTS FSABAL.;

    PUT COUNTER @;
    PUT @10 '700502' @;
    PUT @17 BF_SSN @;
    PUT @27 PF @;
    PUT @30 LA_CUR_PRI @;
    PUT @41 WA_TOT_BRI_OTS @;
    PUT @52 MONEND @;
	PUT @61	METRIC @;
	PUT @99 ' ';
RUN;
%MEND OUTPT;
%OUTPT(7);
%OUTPT(8);
%OUTPT(9);
%OUTPT(10);
%OUTPT(11);
%OUTPT(12);
%OUTPT(13);
%OUTPT(14);
%OUTPT(15);
%OUTPT(16);
%OUTPT(17);
%OUTPT(18);
%OUTPT(19);
%OUTPT(20);

%MACRO GETSAMPLE(PF);
    DATA SOURCESET;
        SET FIDRF3;
        WHERE PF = "&PF";
    RUN;
    
    DATA B&PF (KEEP=BF_SSN PF);
        SAMPSIZE=MIN(5,TOTOBS);
        OBSLEFT=TOTOBS;
        DO WHILE(SAMPSIZE>0);
            PICKIT+1;
            IF RANUNI(0)<SAMPSIZE/OBSLEFT THEN 
                DO;
                    SET SOURCESET POINT=PICKIT NOBS=TOTOBS;
                    OUTPUT;
                    SAMPSIZE=SAMPSIZE-1;
                END;
            OBSLEFT=OBSLEFT-1;
        END;
        STOP;
    RUN;
%MEND GETSAMPLE;

/*"C17 Jeremy" file samples*/
%GETSAMPLE(01);
%GETSAMPLE(02);
%GETSAMPLE(03);
%GETSAMPLE(04);
%GETSAMPLE(05);
%GETSAMPLE(06);
%GETSAMPLE(07);
%GETSAMPLE(08);
%GETSAMPLE(09);
%GETSAMPLE(10);
%GETSAMPLE(11);
%GETSAMPLE(12);

DATA SAMPLES;
    SET B01 B02 B03 B04 B05 B06 B07 B08 B09 B10 B11 B12;
RUN;

PROC SQL;
CREATE TABLE SAMPLE_DATA AS
    SELECT
        FIDRF.*,
        S.PF AS BRW_PF
    FROM
        FIDRF F
        JOIN SAMPLES S
            ON F.BF_SSN = S.BF_SSN
    ORDER BY
        S.PF,
        FIDRF.BF_SSN,
        FIDRF.LN_SEQ
    ;
QUIT;

%MACRO PRINT_SAMPLE_DATA(RNO);
    DATA _NULL_;
        SET SAMPLE_DATA;
        FILE REPORT&RNO DELIMITER=',' DSD DROPOVER LRECL=32767;
        FORMAT BF_SSN $9. BRW_PF PF $3. LD_PIF_RPT MMDDYY10.;

        TOT_AMT = LA_OTS_PRI_ELG + WA_TOT_BRI_OTS;

        IF _N_ = 1 THEN 
            DO;
                PUT "BORROWER SSN,PERFORMANCE CATEGORY BILLED,PERFORMANCE CATEGORY LOAN,LOAN SEQUENCE,PRINCIPAL BALANCE,INTEREST BALANCE,TOTAL PRINCIPAL AND INTEREST,MAX DAYS DELINQUENT,PIF DATE";
            END;
        DO;
            PUT BF_SSN $ @;
            PUT BRW_PF $ @;
            PUT PF $ @;
            PUT LN_SEQ @;
            PUT LA_OTS_PRI_ELG @;
            PUT WA_TOT_BRI_OTS @;
            PUT TOT_AMT @;
            PUT LN_DLQ_MAX @;
            PUT LD_PIF_RPT;
        END;
    RUN;
%MEND PRINT_SAMPLE_DATA;

%PRINT_SAMPLE_DATA(5);


/*"C18" file samples*/
%GETSAMPLE(01);
%GETSAMPLE(02);
%GETSAMPLE(03);
%GETSAMPLE(04);
%GETSAMPLE(05);
%GETSAMPLE(06);
%GETSAMPLE(07);
%GETSAMPLE(08);
%GETSAMPLE(09);
%GETSAMPLE(10);
%GETSAMPLE(11);
%GETSAMPLE(12);
%GETSAMPLE(PIF);
%GETSAMPLE(TRN);

DATA SAMPLES;
    SET B01 B02 B03 B04 B05 B06 B07 B08 B09 B10 B11 B12 BPIF BTRN;
RUN;

PROC SQL;
CREATE TABLE SAMPLE_DATA AS
    SELECT
        FIDRF.*,
        S.PF AS BRW_PF
    FROM
        FIDRF F
        JOIN SAMPLES S
            ON F.BF_SSN = S.BF_SSN
    ORDER BY
        S.PF,
        FIDRF.BF_SSN,
        FIDRF.LN_SEQ
    ;
QUIT;

%PRINT_SAMPLE_DATA(6);

/*R21 report*/

PROC SQL;
CREATE TABLE FSAMetricTracking AS
SELECT DISTINCT
	CASE WHEN PF = '03' 
			THEN 'Metric1'
	     WHEN PF IN('09','10') 
			THEN 'Metric2'
		 WHEN PF = '11' 
			THEN 'Metric3'
		 	ELSE 'Other' 
	END AS Metric,
	SEGMENT,
	COUNT(DISTINCT BF_SSN) AS BorrowerCount
FROM 
	FIDRF3 
WHERE
	PF IN('03','07','08','09','10','11')
GROUP BY
	CASE WHEN PF = '03' 
			THEN 'Metric1'
	     WHEN PF IN('09','10') 
			THEN 'Metric2'
		 WHEN PF = '11' 
			THEN 'Metric3'
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

PROC EXPORT
	DATA=MetricSegmentSummary
	OUTFILE = "&RPTLIB\UNWC22.NWC22R21_&SYSDATE..xlsx"
	DBMS = EXCEL
	REPLACE;
RUN;

PROC SQL; /*R22 report*/
CREATE TABLE MetricDetail AS
SELECT DISTINCT
	BF_SSN,
	CASE WHEN PF = '03' 
			THEN 'Metric1'
	     WHEN PF IN('09','10') 
			THEN 'Metric2'
		 WHEN PF = '11' 
			THEN 'Metric3'
		 	ELSE 'Other' 
	END AS Metric,
	PF,
	SEGMENT
FROM 
	FIDRF3 
WHERE
	PF IN('03','07','08','09','10','11')
;
QUIT;

PROC EXPORT
	DATA=MetricDetail
	OUTFILE = "&RPTLIB\UNWC22.NWC22R22_&SYSDATE..xlsx"
	DBMS = EXCEL
	REPLACE;
RUN;


/*only needed for testing*/
/*PROC EXPORT*/
/*      DATA=FIDRF*/
/*      OUTFILE='T:\SAS\FIDRF.XLSX'*/
/*      REPLACE;*/
/*RUN;*/


/*(1) commented out IID and LF_GTR_RFR as we won't get that information from FSA for a while but it will need to be reinstated at some point*/
