/*%LET RPTLIB = %SYSGET(reportdir);*/
/*Live*/
%LET RPTLIB = Z:\Batch\FTP;
/*Test*/
/*%LET RPTLIB = T:\SAS;*/
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
/*Live*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= SCRA;
/*Test*/
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= SCRA;*/


FILENAME REPORTX "&RPTLIB/NH XXXX ACS TRANSFER RX_&SYSDATE";
FILENAME REPORTX "&RPTLIB/NH XXXX COD RX_&SYSDATE";


DATA _NULL_;
    RUN_DAY = today();
    CALL SYMPUT('FIRST_DAY_PREV_MON',"'"||PUT(INTNX('MONTH',RUN_DAY,-X,'B'), MMDDYYDXX.)||"'");
    CALL SYMPUT('LAST_DAY_PREV_MON',"'"||PUT(INTNX('MONTH',RUN_DAY,-X,'E'), MMDDYYDXX.)||"'");
    CALL SYMPUT('LAST_DAY_PREV_MON_SAS',"'"||PUT(INTNX('MONTH',RUN_DAY,-X,'E'), DATEX.)||"'d");
    CALL SYMPUTX('MOYR',PUT(TODAY(),MMYYNX.));
RUN;

OPTIONS MISSING=' ';
%SYSLPUT LAST_DAY_PREV_MON = &LAST_DAY_PREV_MON;
%SYSLPUT FIRST_DAY_PREV_MON = &FIRST_DAY_PREV_MON;
%SYSLPUT LAST_DAY_PREV_MON_SAS = &LAST_DAY_PREV_MON_SAS ;


PROC SQL;
	CREATE TABLE MIL AS
		SELECT
			B.BORROWERACCOUNTNUMBER AS DF_SPE_ACC_ID
			,DTES.BEG_DTE_T
			,CASE
				WHEN DTES.END_DTE_T IS NULL THEN XXXXX
				ELSE DTES.END_DTE_T
			 END AS END_DTE_T
		FROM 
			SQL.BORROWERS B
			INNER JOIN (
					SELECT
						B.BORROWERACCOUNTNUMBER 
						,MIN(AD.BEGINDATE) AS BEG_DTE_T
						,MAX(AD.ENDDATE) AS END_DTE_T
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
	FORMAT BEG_DTE END_DTE DATEX.;
RUN;

RSUBMIT ;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
    CONNECT TO DBX (DATABASE=DNFPUTDL);
    CREATE TABLE FIDRFX AS
        SELECT
            *
        FROM 
            CONNECTION TO DBX 
                (
                    SELECT 
                        LNXX.BF_SSN
                        ,LNXX.LN_SEQ
                        ,LNXX.LC_STA_LONXX
                        ,LNXX.LD_STA_LONXX
                        ,LNXX.LD_LON_EFF_ADD
                        ,LNXX.LD_LON_ACL_ADD
                        ,LNXX.LD_PIF_RPT
/*(X)                       ,LNXX.LF_GTR_RFR*/
                        ,CASE
                            WHEN LNXX.IC_LON_PGM IN 
                                  (
                                        'DLPCNS','DLPLGB','DLPLUS','DLSCNS','DLSPCN','DLSSPL',
                                        'DLSTFD','DLUCNS','DLUNST','DLUSPL','TEACH'
                                  ) THEN 'DL'
                            WHEN LNXX.IC_LON_PGM IN 
                                  (
                                        'CNSLDN','PLUS','PLUSGB','SLS','STFFRD','SUBCNS',
                                        'SUBSPC','UNCNS','UNSPC','UNSTFD','FISL'
                                  ) THEN 'FL'
                            ELSE ''
                        END AS IC_LON_PGM
                        ,LNXX.LA_CUR_PRI AS LA_OTS_PRI_ELG 
                        ,COALESCE(DWXX.WA_TOT_BRI_OTS,DWXX.LA_NSI_OTS) AS WA_TOT_BRI_OTS
                        ,CASE
                            WHEN LNXX.LA_CUR_PRI <= X THEN X
                            WHEN LNXX.LC_STA_LONXX = 'D' THEN DAYS(CURRENT DATE) - DAYS(LNXX.LD_DLQ_OCC) - DAY(CURRENT DATE)
                            WHEN DWXX.WC_DW_LON_STA IN ('XX','XX') THEN X
                            WHEN FORB.BF_SSN ^= '' THEN X
                            WHEN COALESCE(MAX(LNXX.LN_DLQ_MAX) - DAY(CURRENT DATE) + X,X) < X THEN X
                            ELSE COALESCE(MAX(LNXX.LN_DLQ_MAX) - DAY(CURRENT DATE) + X,X)
                        END AS LN_DLQ_MAX
                        ,CASE
                            WHEN FORB.BF_SSN ^= '' THEN 'X'
                            ELSE 'X'
                        END AS SPEC_FORB_IND
                        ,DWXX.WC_DW_LON_STA
                        ,CASE
                            WHEN LNXX.LC_STA_LONXX = 'D' AND LNXX.LA_CUR_PRI = X THEN X
                            WHEN LNXX.LD_PIF_RPT IS NOT NULL THEN X
                            ELSE X
                        END AS ORD
                        ,CASE
                            WHEN LNXX.BF_SSN IS NULL THEN X
                            ELSE X
                        END AS BILL_SATISFIED
						,CASE
							WHEN COD.BF_SSN IS NOT NULL THEN X
							ELSE X
						END AS IS_COD
                    FROM 
                        PKUB.LNXX_LON LNXX
						LEFT JOIN
						(
							SELECT DISTINCT
								LNXX.BF_SSN,
								LNXX.LN_SEQ
							FROM
								PKUB.LNXX_FIN_ATY LNXX
							WHERE
								LNXX.PC_FAT_TYP IN ('XX') AND LNXX.PC_FAT_SUB_TYP IN ('XX') AND LNXX.LC_FAT_REV_REA IN ('') AND LNXX.LC_STA_LONXX IN ('A')

						)COD
							ON COD.BF_SSN = LNXX.BF_SSN
							AND COD.LN_SEQ = LNXX.LN_SEQ
                        LEFT JOIN PKUB.DWXX_DW_CLC_CLU DWXX
                            ON LNXX.BF_SSN = DWXX.BF_SSN
                            AND LNXX.LN_SEQ = DWXX.LN_SEQ
                        LEFT JOIN PKUB.LNXX_LON_DLQ_HST LNXX
                            ON LNXX.BF_SSN = LNXX.BF_SSN
                            AND LNXX.LN_SEQ = LNXX.LN_SEQ
                            AND LNXX.LC_STA_LONXX = 'X'
                            AND LNXX.LC_DLQ_TYP = 'P'
                        /*ASSIGN INDICATOR FOR FORBEARANCES*/
                        LEFT JOIN
                            (
                                SELECT 
                                    A.BF_SSN                                       
                                FROM 
                                    PKUB.FBXX_BR_FOR_REQ A
                                    INNER JOIN PKUB.LNXX_BR_FOR_APV B
                                        ON A.BF_SSN = B.BF_SSN
                                        AND A.LF_FOR_CTL_NUM = B.LF_FOR_CTL_NUM
                                WHERE
                                    A.LC_FOR_STA = 'A'
                                    AND B.LC_STA_LONXX = 'A'
                                    AND A.LC_STA_FORXX = 'A'      
                                    AND A.LC_FOR_TYP IN ('XX','XX','XX','XX','XX','XX','XX')
                                    AND B.LD_FOR_END >= &LAST_DAY_PREV_MON
                                    AND B.LD_FOR_APL <= &LAST_DAY_PREV_MON
                                    AND B.LD_FOR_BEG <= &LAST_DAY_PREV_MON
                            ) FORB
                            ON FORB.BF_SSN = LNXX.BF_SSN
                        LEFT JOIN PKUB.LNXX_LON_BIL_CRF LNXX
                            ON LNXX.BF_SSN = LNXX.BF_SSN
                            AND LNXX.LN_SEQ = LNXX.LN_SEQ
                            AND LNXX.LD_BIL_DU_LON BETWEEN &FIRST_DAY_PREV_MON AND &LAST_DAY_PREV_MON
                            AND LNXX.LA_TOT_BIL_STS >= COALESCE(LNXX.LA_BIL_CUR_DU,X)
                            AND LNXX.LC_STA_LONXX = 'A'
                            AND LNXX.LC_BIL_TYP_LON = 'P'   
                    WHERE 
                        LNXX.LC_STA_LONXX IN ('R','D','L')
                        AND LNXX.LD_LON_ACL_ADD <= &LAST_DAY_PREV_MON
                    GROUP BY 
                        LNXX.BF_SSN
                        ,LNXX.LN_SEQ
                        ,LNXX.LC_STA_LONXX
                        ,LNXX.LD_STA_LONXX
                        ,LNXX.LD_LON_EFF_ADD
                        ,LNXX.LD_LON_ACL_ADD
                        ,LNXX.LD_PIF_RPT
/*(X)                       ,LNXX.LF_GTR_RFR*/
                        ,CASE
                            WHEN LNXX.IC_LON_PGM IN 
                                  (
                                        'DLPCNS','DLPLGB','DLPLUS','DLSCNS','DLSPCN','DLSSPL',
                                        'DLSTFD','DLUCNS','DLUNST','DLUSPL','TEACH'
                                  ) THEN 'DL'
                            WHEN LNXX.IC_LON_PGM IN 
                                  (
                                        'CNSLDN','PLUS','PLUSGB','SLS','STFFRD','SUBCNS',
                                        'SUBSPC','UNCNS','UNSPC','UNSTFD','FISL'
                                  ) THEN 'FL'
                            ELSE ''
                        END 
                        ,LNXX.LA_CUR_PRI 
                        ,COALESCE(DWXX.WA_TOT_BRI_OTS,DWXX.LA_NSI_OTS)
                        ,LNXX.LC_STA_LONXX
                        ,LNXX.LC_SST_LONXX
                        ,LNXX.LD_DLQ_OCC
                        ,LNXX.LA_CUR_PRI
                        ,LNXX.LA_CUR_PRI
                        ,DWXX.WC_DW_LON_STA
                        ,FORB.BF_SSN
                        ,CASE
                            WHEN LNXX.BF_SSN IS NULL THEN X
                            ELSE X
                        END
						,CASE
							WHEN COD.BF_SSN IS NOT NULL THEN X
							ELSE X
						END
                    ORDER BY
                        LNXX.BF_SSN

            FOR READ ONLY WITH UR
        )
    ;
    DISCONNECT FROM DBX;
QUIT;

/*This is to create the population for the RX file to review active delinquencies on borrowers with a deferment or forbearanace*/
PROC SQL;
    CONNECT TO DBX (DATABASE=DNFPUTDL);
    CREATE TABLE DFDELQ AS
        SELECT
            *
        FROM 
            CONNECTION TO DBX 
                (
                    SELECT 
                        PDXX.DF_SPE_ACC_ID,
                        LNXX.LN_SEQ,
                        LNXX.LN_DLQ_MAX,
                        FORB.LC_FOR_TYP,
                        DEFR.LC_DFR_TYP
                    FROM 
						PKUB.LNXX_LON LNXX
                    	JOIN PKUB.PDXX_PRS_NME PDXX
                        	ON LNXX.BF_SSN = PDXX.DF_PRS_ID
                    	JOIN PKUB.LNXX_LON_DLQ_HST LNXX
                            ON LNXX.BF_SSN = LNXX.BF_SSN
                            AND LNXX.LN_SEQ = LNXX.LN_SEQ
                            AND LNXX.LC_STA_LONXX = 'X'
                            AND LNXX.LC_DLQ_TYP = 'P'                       
                    	LEFT JOIN
                            (
                                SELECT 
                                    FBXX.BF_SSN,
                                    LNXX.LN_SEQ,
                                    FBXX.LC_FOR_TYP
                                FROM 
                                    PKUB.FBXX_BR_FOR_REQ FBXX
                                    JOIN PKUB.LNXX_BR_FOR_APV LNXX
                                        ON FBXX.BF_SSN = LNXX.BF_SSN
                                        AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
                                WHERE
                                    FBXX.LC_FOR_STA = 'A'
                                    AND LNXX.LC_STA_LONXX = 'A'
                                    AND FBXX.LC_STA_FORXX = 'A'      
                                    AND LNXX.LD_FOR_END >= &LAST_DAY_PREV_MON
                                    AND LNXX.LD_FOR_APL <= &LAST_DAY_PREV_MON
                                    AND LNXX.LD_FOR_BEG <= &LAST_DAY_PREV_MON
                            ) FORB
                            ON FORB.BF_SSN = LNXX.BF_SSN
                            AND FORB.LN_SEQ = LNXX.LN_SEQ
                        LEFT JOIN
                            (
                                SELECT 
                                    DFXX.BF_SSN,
                                    LNXX.LN_SEQ,
                                    DFXX.LC_DFR_TYP 
                                FROM 
                                    PKUB.DFXX_BR_DFR_REQ DFXX
                                    JOIN PKUB.LNXX_BR_DFR_APV LNXX
                                        ON DFXX.BF_SSN = LNXX.BF_SSN
                                        AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
                                WHERE
                                    DFXX.LC_DFR_STA = 'A'
                                    AND LNXX.LC_STA_LONXX = 'A'
                                    AND DFXX.LC_STA_DFRXX = 'A'      
                                    AND LNXX.LD_DFR_END >= &LAST_DAY_PREV_MON
                                    AND LNXX.LD_DFR_APL <= &LAST_DAY_PREV_MON
                                    AND LNXX.LD_DFR_BEG <= &LAST_DAY_PREV_MON
                            ) DEFR
                            ON DEFR.BF_SSN = LNXX.BF_SSN
                            AND DEFR.LN_SEQ = LNXX.LN_SEQ                    
                    WHERE 
                        LNXX.LC_STA_LONXX IN ('R','D','L')
                        AND LNXX.LA_CUR_PRI > X
                        AND (FORB.BF_SSN IS NOT NULL OR DEFR.BF_SSN IS NOT NULL)                        
                    ORDER BY
                        PDXX.DF_SPE_ACC_ID,
                        LNXX.LN_SEQ

            FOR READ ONLY WITH UR
        )
    ;
    DISCONNECT FROM DBX;
QUIT;

PROC SQL;
    CONNECT TO DBX (DATABASE=DNFPUTDL);
    CREATE TABLE MILT_STA AS
	        SELECT DISTINCT
	            LNXX.BF_SSN
	            ,'Y' AS IS_ACTIVE_MILT
	        FROM
	            PKUB.PDXX_PRS_NME PDXX
	            JOIN PKUB.LNXX_LON LNXX
	                ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	            JOIN PKUB.DWXX_DW_CLC_CLU DWXX
	                ON LNXX.BF_SSN = DWXX.BF_SSN
	                AND LNXX.LN_SEQ = DWXX.LN_SEQ
	            JOIN (
	                    SELECT DISTINCT
	                        AYXX.BF_SSN
	                        ,COALESCE(MIL.BEG_DTE, LNXX.LD_ITR_EFF_BEG, THIRTY_EIGHT.LD_DFR_BEG, FORTY.LD_DFR_BEG) AS BEGIN_DATE
	                        ,COALESCE(MIL.END_DTE, LNXX.LD_ITR_EFF_END, THIRTY_EIGHT.LD_DFR_END, FORTY.LD_DFR_END) AS END_DATE
							,COALESCE(MIL.END_DTE, LNXX.LD_ITR_EFF_END, THIRTY_EIGHT.LD_DFR_END, FORTY.LD_DFR_END) AS VALID_END_DATE
	                    FROM PKUB.AYXX_BR_LON_ATY AYXX
	                        JOIN (
	                                SELECT
	                                    AYXX.BF_SSN
	                                    ,MAX(AYXX.LN_ATY_SEQ) AS LN_ATY_SEQ
	                                FROM 
	                                    PKUB.AYXX_BR_LON_ATY AYXX
	                                WHERE 
	                                    AYXX.PF_REQ_ACT = 'ASCRA'
	                                GROUP BY 
	                                    AYXX.BF_SSN
	                                    ) ASCRA
	                            ON AYXX.BF_SSN = ASCRA.BF_SSN
	                            AND AYXX.LN_ATY_SEQ = ASCRA.LN_ATY_SEQ
	                        LEFT JOIN (
	                                SELECT
	                                    AYXX.BF_SSN
	                                    ,MAX(AYXX.LN_ATY_SEQ) AS LN_ATY_SEQ
	                                FROM 
	                                    PKUB.AYXX_BR_LON_ATY AYXX
	                                WHERE 
	                                    AYXX.PF_REQ_ACT = 'ISCRA'
	                                GROUP BY 
	                                    AYXX.BF_SSN
	                                    ) ISCRA
	                            ON AYXX.BF_SSN = ISCRA.BF_SSN
				            LEFT JOIN PKUB.LNXX_INT_RTE_HST LNXX
				                ON AYXX.BF_SSN = LNXX.BF_SSN
				                AND &LAST_DAY_PREV_MON_SAS BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END
				                AND LNXX.LC_STA_LONXX = 'A'
								AND LNXX.LC_INT_RDC_PGM = 'M'
				            LEFT JOIN 
				                (
				                    SELECT DISTINCT
				                        DWXX.BF_SSN
										,LNXX.LD_DFR_BEG
										,LNXX.LD_DFR_END
				                    FROM
				                        PKUB.DWXX_DW_CLC_CLU DWXX
				                        JOIN PKUB.LNXX_BR_DFR_APV LNXX
				                            ON DWXX.BF_SSN = LNXX.BF_SSN
				                            AND DWXX.LN_SEQ = LNXX.LN_SEQ
				                            AND LNXX.LC_STA_LONXX = 'A'
				                            AND &LAST_DAY_PREV_MON_SAS BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
				                        JOIN PKUB.DFXX_BR_DFR_REQ DFXX
				                            ON DWXX.BF_SSN = DFXX.BF_SSN
				                            AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
				                            AND DFXX.LC_DFR_STA = 'A'
				                            AND DFXX.LC_STA_DFRXX = 'A'
				                            AND DFXX.LC_DFR_TYP = 'XX'
				                    WHERE 
				                        DWXX.WC_DW_LON_STA = 'XX'
				                ) THIRTY_EIGHT
				                ON AYXX.BF_SSN = THIRTY_EIGHT.BF_SSN
				            LEFT JOIN 
				                (
				                    SELECT DISTINCT
				                        DWXX.BF_SSN
										,LNXX.LD_DFR_BEG
										,LNXX.LD_DFR_END
				                    FROM
				                        PKUB.DWXX_DW_CLC_CLU DWXX
				                        JOIN PKUB.LNXX_BR_DFR_APV LNXX
				                            ON DWXX.BF_SSN = LNXX.BF_SSN
				                            AND DWXX.LN_SEQ = LNXX.LN_SEQ
				                            AND LNXX.LC_STA_LONXX = 'A'
				                            AND &LAST_DAY_PREV_MON_SAS BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
				                        JOIN PKUB.DFXX_BR_DFR_REQ DFXX
				                            ON DWXX.BF_SSN = DFXX.BF_SSN
				                            AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
				                            AND DFXX.LC_DFR_STA = 'A'
				                            AND DFXX.LC_STA_DFRXX = 'A'
				                            AND DFXX.LC_DFR_TYP = 'XX'
				                    WHERE 
				                        DWXX.WC_DW_LON_STA = 'XX'
				                ) FORTY
				                ON AYXX.BF_SSN = FORTY.BF_SSN
	                        JOIN PKUB.AYXX_ATY_TXT AYXX
	                            ON AYXX.BF_SSN = AYXX.BF_SSN
	                            AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
							INNER JOIN PKUB.PDXX_PRS_NME PDXX
								ON AYXX.BF_SSN = PDXX.DF_PRS_ID
							LEFT JOIN MIL 
								ON PDXX.DF_SPE_ACC_ID = MIL.DF_SPE_ACC_ID
	                        WHERE AYXX.PF_REQ_ACT = 'ASCRA'
	                            AND (ASCRA.LN_ATY_SEQ > ISCRA.LN_ATY_SEQ)
	                            OR ISCRA.BF_SSN IS NULL
	                    ) ARCS
	                ON LNXX.BF_SSN = ARCS.BF_SSN
	            LEFT JOIN PKUB.LNXX_INT_RTE_HST LNXX
	                ON LNXX.BF_SSN = LNXX.BF_SSN
	                AND &LAST_DAY_PREV_MON_SAS BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END
	                AND LNXX.LC_STA_LONXX = 'A'
	            LEFT JOIN 
	                (
	                    SELECT DISTINCT
	                        DWXX.BF_SSN
	                    FROM
	                        PKUB.DWXX_DW_CLC_CLU DWXX
	                        JOIN PKUB.LNXX_BR_DFR_APV LNXX
	                            ON DWXX.BF_SSN = LNXX.BF_SSN
	                            AND DWXX.LN_SEQ = LNXX.LN_SEQ
	                            AND LNXX.LC_STA_LONXX = 'A'
	                            AND &LAST_DAY_PREV_MON_SAS BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
	                        JOIN PKUB.DFXX_BR_DFR_REQ DFXX
	                            ON DWXX.BF_SSN = DFXX.BF_SSN
	                            AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
	                            AND DFXX.LC_DFR_STA = 'A'
	                            AND DFXX.LC_STA_DFRXX = 'A'
	                            AND DFXX.LC_DFR_TYP = 'XX'
	                    WHERE 
	                        DWXX.WC_DW_LON_STA = 'XX'
	                ) THIRTY_EIGHT
	                ON LNXX.BF_SSN = THIRTY_EIGHT.BF_SSN
	            LEFT JOIN 
	                (
	                    SELECT DISTINCT
	                        DWXX.BF_SSN
	                    FROM
	                        PKUB.DWXX_DW_CLC_CLU DWXX
	                        JOIN PKUB.LNXX_BR_DFR_APV LNXX
	                            ON DWXX.BF_SSN = LNXX.BF_SSN
	                            AND DWXX.LN_SEQ = LNXX.LN_SEQ
	                            AND LNXX.LC_STA_LONXX = 'A'
	                            AND &LAST_DAY_PREV_MON_SAS BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
	                        JOIN PKUB.DFXX_BR_DFR_REQ DFXX
	                            ON DWXX.BF_SSN = DFXX.BF_SSN
	                            AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
	                            AND DFXX.LC_DFR_STA = 'A'
	                            AND DFXX.LC_STA_DFRXX = 'A'
	                            AND DFXX.LC_DFR_TYP = 'XX'
	                    WHERE 
	                        DWXX.WC_DW_LON_STA = 'XX'
	                ) FORTY
	                ON LNXX.BF_SSN = FORTY.BF_SSN
	            LEFT JOIN
	                (
	                    SELECT
	                        LNXX.BF_SSN
	                        ,MAX(LNXX.LN_DLQ_MAX) AS LN_DLQ_MAX
	                    FROM 
	                        PKUB.LNXX_LON_DLQ_HST LNXX
	                    WHERE 
	                        LNXX.LC_STA_LONXX = 'X'
	                        AND LNXX.LN_DLQ_MAX >= XXX
	                    GROUP BY 
	                        LNXX.BF_SSN
	                ) LNXX
	                ON LNXX.BF_SSN = LNXX.BF_SSN
	        WHERE
	            (LNXX.LA_CUR_PRI + DWXX.WA_TOT_BRI_OTS > X)
	            AND LNXX.LC_STA_LONXX = 'R'
	            AND
	                (
	                    (LNXX.LR_ITR = X AND LNXX.LC_INT_RDC_PGM = 'S')
	                    OR (LNXX.LR_ITR <= X AND LNXX.LC_INT_RDC_PGM = 'M')
	                    OR (LNXX.LR_ITR <= X AND LNXX.LD_LON_X_DSB < ARCS.BEGIN_DATE AND &LAST_DAY_PREV_MON_SAS <= ARCS.VALID_END_DATE)
	                    OR THIRTY_EIGHT.BF_SSN IS NOT NULL
	                    OR FORTY.BF_SSN IS NOT NULL
	                )
    ;

    CREATE TABLE FIDRF_REMOTE AS
        SELECT DISTINCT
            FIDR.*,
            COALESCE(MSTAB.IS_ACTIVE_MILT,MSTAE.IS_ACTIVE_MILT,'N') AS IS_ACTIVE_MILT
        FROM
            FIDRFX FIDR
            LEFT JOIN MILT_STA MSTAB
                ON FIDR.BF_SSN = MSTAB.BF_SSN
            LEFT JOIN PKUB.LNXX_EDS LNXX
                ON FIDR.BF_SSN = LNXX.BF_SSN
            LEFT JOIN MILT_STA MSTAE
                ON LNXX.LF_EDS = MSTAE.BF_SSN
    ;

DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;

DATA DFDELQ; SET LEGEND.DFDELQ; RUN;

DATA FIDRF_REMOTE; SET legend.FIDRF_REMOTE; RUN;




DATA _NULL_;
    CALL SYMPUT('locl_LAST_DAY_PREV_MON',INPUT(COMPRESS(&LAST_DAY_PREV_MON,"'"),MMDDYYXX.));
    CALL SYMPUT('locl_FIRST_DAY_PREV_MON',INPUT(COMPRESS(&FIRST_DAY_PREV_MON,"'"),MMDDYYXX.));
RUN;

DATA FIDRFX STATUS_ERR;
    SET FIDRF_REMOTE;
    IF WC_DW_LON_STA IN ('XX','XX','XX') THEN OUTPUT STATUS_ERR;
    ELSE OUTPUT FIDRFX;
RUN;

PROC SQL;
	CREATE TABLE COD AS 
		SELECT
			*
		FROM
			FIDRFX
		WHERE
			IS_COD = X
;
	CREATE TABLE ACS_TRANSFER AS 
		SELECT
			*
		FROM
			FIDRFX
		WHERE
			IS_COD = X
;
QUIT;

/*assign categories*/
%MACRO ASSIGN_CATS(INFILE,OUTFILE,ERRFILE);

    DATA &OUTFILE &ERRFILE;
        SET &INFILE;
        BOR_INV_NUM = _N_;

        LENGTH PF $X.;

        IF ORD=X THEN
            DO;
                IF IS_ACTIVE_MILT = 'Y' THEN PF = 'XX';
                ELSE IF WC_DW_LON_STA IN ('XX','XX') THEN PF = 'XX';
                ELSE IF WC_DW_LON_STA = 'XX' THEN PF = 'XX';
                ELSE IF WC_DW_LON_STA IN ('XX','XX','XX','XX','XX','XX','XX','XX','XX','XX', 'XX','XX','XX') THEN 
                    DO; /*XX WILL BE IN THIS GROUP, BUT NOT ALL LOANS PIF*/
                        IF SPEC_FORB_IND IN ('X') AND LN_DLQ_MAX = X THEN PF = 'XX';
                        ELSE IF X <= LN_DLQ_MAX <= X THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XXX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XXX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XXX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX > XXX THEN PF = 'XX';
                        ELSE PF = 'XX';
                    END;
                ELSE IF WC_DW_LON_STA = 'XX' THEN 
                    DO;
                        IF LN_DLQ_MAX = X AND BILL_SATISFIED = X THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX = X THEN PF = 'XX';
                        ELSE IF X <= LN_DLQ_MAX <= X THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XXX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XXX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XXX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX > XXX THEN PF = 'XX';
                        ELSE PF = 'XX';
                    END;
                ELSE IF WC_DW_LON_STA = 'XX' THEN
                    DO;
                        IF LN_DLQ_MAX = X AND BILL_SATISFIED = X THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX = X THEN PF = 'XX';
                        ELSE IF X <= LN_DLQ_MAX <= X THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XXX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XXX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX <= XXX THEN PF = 'XX';
                        ELSE IF LN_DLQ_MAX > XXX THEN PF = 'XX';
                        ELSE PF = 'XX';
                    END;
                ELSE IF LN_DLQ_MAX <= X THEN PF = 'XX';
                ELSE IF LN_DLQ_MAX <= XX THEN PF = 'XX';
                ELSE IF LN_DLQ_MAX <= XX THEN PF = 'XX';
                ELSE IF LN_DLQ_MAX <= XXX THEN PF = 'XX';
                ELSE IF LN_DLQ_MAX <= XXX THEN PF = 'XX';
                ELSE IF LN_DLQ_MAX <= XXX THEN PF = 'XX';
                ELSE IF LN_DLQ_MAX > XXX THEN PF = 'XX';
                ELSE PF = 'XX';
            END;
    /*  PIF OR DECONVERTED BORROWERS*/
        ELSE IF ORD=X THEN PF = 'PIF';
        ELSE IF ORD=X THEN PF = 'TRN';
        ELSE PF = 'PRV';

        IF ORD <= X THEN OUTPUT &OUTFILE;
        ELSE OUTPUT &ERRFILE;
    RUN;
%MEND;

%ASSIGN_CATS(ACS_TRANSFER,ACS_TRANSFER_OUT,OTH_LNS);
%ASSIGN_CATS(COD,COD_OUT,OTH_LNS);

PROC FREQ DATA=ACS_TRANSFER_OUT NOPRINT;
    TABLE BF_SSN*WC_DW_LON_STA / OUT=STAT_TABX(DROP=PERCENT) ;
RUN;
PROC FREQ DATA=COD_OUT NOPRINT;
    TABLE BF_SSN*WC_DW_LON_STA / OUT=STAT_TABX(DROP=PERCENT) ;
RUN;

PROC SORT DATA=STAT_TABX;
    BY BF_SSN COUNT;
RUN;
PROC SORT DATA=STAT_TABX;
    BY BF_SSN COUNT;
RUN;

DATA STAT_TABX(KEEP=BF_SSN WC_DW_LON_STA);
    SET STAT_TABX;
    BY BF_SSN;
    IF LAST.BF_SSN;
RUN;
DATA STAT_TABX(KEEP=BF_SSN WC_DW_LON_STA);
    SET STAT_TABX;
    BY BF_SSN;
    IF LAST.BF_SSN;
RUN;


/*CREATE BORROWER LEVEL DATA SET */
PROC SQL;
    CREATE TABLE FIDRFX_COD AS 
        SELECT DISTINCT 
            A.BF_SSN
/*(X)           ,A.LF_GTR_RFR*/
            ,A.LD_LON_EFF_ADD
            ,B.WC_DW_LON_STA
            ,A.SPEC_FORB_IND
/*(X)           ,SUBSTR(A.LF_GTR_RFR,X) AS IID*/
            ,MAX(LN_DLQ_MAX) AS LN_DLQ_MAX
            ,SUM(COALESCE(A.LA_OTS_PRI_ELG,X)) AS LA_CUR_PRI
            ,SUM(INT((ROUND(A.WA_TOT_BRI_OTS,.XXXXXX)) * XXX) / XXX) AS WA_TOT_BRI_OTS
            ,CALCULATED LA_CUR_PRI + CALCULATED WA_TOT_BRI_OTS AS TOT_AMT
            ,CASE
                WHEN COUNT(DISTINCT A.IC_LON_PGM) = X THEN A.IC_LON_PGM
                ELSE 'MX'
            END AS LON_PGM
            ,COUNT(DISTINCT A.LN_SEQ) AS LOAN_COUNT
            ,COALESCE(C.PIF_CT_BX_REP_MO,X) AS PIF_CT_BX_REP_MO
            ,COALESCE(D.PIF_CT_REP_MO,X) AS PIF_CT_REP_MO
            ,COALESCE(E.DSTAT_CT_BX_REP_MO,X) AS DSTAT_CT_BX_REP_MO
            ,COALESCE(F.DSTAT_CT_REP_MO,X) AS DSTAT_CT_REP_MO
            ,MAX(LD_PIF_RPT) AS LD_PIF_RPT
            ,MAX(LD_STA_LONXX) AS LD_STA_LONXX
            ,IS_ACTIVE_MILT
            ,MIN(BILL_SATISFIED) AS BILL_SATISFIED
        FROM
            COD_OUT A
            JOIN STAT_TABX B
                ON A.BF_SSN = B.BF_SSN
/*GET COUNT OF PIF LOANS PREVIOUS TO THE RUN MONTH*/
            LEFT JOIN
                (
                    SELECT
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                        ,COUNT(*) AS PIF_CT_BX_REP_MO
                    FROM 
                        COD_OUT 
                    WHERE 
                        &locl_FIRST_DAY_PREV_MON > LD_PIF_RPT 
                        AND LD_PIF_RPT IS NOT NULL 
                    GROUP BY 
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                ) C
                ON A.BF_SSN = C.BF_SSN
/*(X)               AND A.LF_GTR_RFR = C.LF_GTR_RFR*/
/*GET COUNT OF PIF LOANS IN THE RUN MONTH*/
            LEFT JOIN
                (
                    SELECT
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                        ,COUNT(*) AS PIF_CT_REP_MO
                    FROM 
                        COD_OUT 
                    WHERE 
                        &locl_FIRST_DAY_PREV_MON <= LD_PIF_RPT <= &locl_LAST_DAY_PREV_MON 
                    GROUP BY
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                ) D
                ON A.BF_SSN = D.BF_SSN
/*(X)               AND A.LF_GTR_RFR = D.LF_GTR_RFR*/
/*GET COUNT OF DCONVERTED LOANS PREVIOUS TO THE RUN MONTH*/
            LEFT OUTER JOIN
                (
                    SELECT 
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                        ,COUNT(*) AS DSTAT_CT_BX_REP_MO
                    FROM 
                        COD_OUT 
                    WHERE 
                        LC_STA_LONXX = 'D' 
                        AND LA_OTS_PRI_ELG  = X 
                        AND &locl_FIRST_DAY_PREV_MON > LD_STA_LONXX  
                        AND LD_STA_LONXX IS NOT NULL
                    GROUP BY 
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                ) E
                ON A.BF_SSN = E.BF_SSN
/*(X)               AND A.LF_GTR_RFR = E.LF_GTR_RFR*/
/*GET COUNT OF DCONVERTED LOANS IN THE RUN MONTH*/
            LEFT OUTER JOIN 
                (
                    SELECT 
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                        ,COUNT(*) AS DSTAT_CT_REP_MO
                    FROM 
                        COD_OUT 
                    WHERE 
                        LC_STA_LONXX = 'D' 
                        AND LA_OTS_PRI_ELG  = X
                        AND &locl_FIRST_DAY_PREV_MON <= LD_STA_LONXX <= &locl_LAST_DAY_PREV_MON
                    GROUP BY 
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                ) F
                ON A.BF_SSN = F.BF_SSN
/*(X)               AND A.LF_GTR_RFR = F.LF_GTR_RFR*/
        GROUP BY 
            A.BF_SSN
/*(X)           ,A.LF_GTR_RFR*/
        ORDER BY 
/*(X)           CALCULATED IID,*/
            A.BF_SSN
    ;
QUIT;

PROC SQL;
    CREATE TABLE FIDRFX_ACS_TRANSFER AS 
        SELECT DISTINCT 
            A.BF_SSN
/*(X)           ,A.LF_GTR_RFR*/
            ,A.LD_LON_EFF_ADD
            ,B.WC_DW_LON_STA
            ,A.SPEC_FORB_IND
/*(X)           ,SUBSTR(A.LF_GTR_RFR,X) AS IID*/
            ,MAX(LN_DLQ_MAX) AS LN_DLQ_MAX
            ,SUM(COALESCE(A.LA_OTS_PRI_ELG,X)) AS LA_CUR_PRI
            ,SUM(INT((ROUND(A.WA_TOT_BRI_OTS,.XXXXXX)) * XXX) / XXX) AS WA_TOT_BRI_OTS
            ,CALCULATED LA_CUR_PRI + CALCULATED WA_TOT_BRI_OTS AS TOT_AMT
            ,CASE
                WHEN COUNT(DISTINCT A.IC_LON_PGM) = X THEN A.IC_LON_PGM
                ELSE 'MX'
            END AS LON_PGM
            ,COUNT(DISTINCT A.LN_SEQ) AS LOAN_COUNT
            ,COALESCE(C.PIF_CT_BX_REP_MO,X) AS PIF_CT_BX_REP_MO
            ,COALESCE(D.PIF_CT_REP_MO,X) AS PIF_CT_REP_MO
            ,COALESCE(E.DSTAT_CT_BX_REP_MO,X) AS DSTAT_CT_BX_REP_MO
            ,COALESCE(F.DSTAT_CT_REP_MO,X) AS DSTAT_CT_REP_MO
            ,MAX(LD_PIF_RPT) AS LD_PIF_RPT
            ,MAX(LD_STA_LONXX) AS LD_STA_LONXX
            ,IS_ACTIVE_MILT
            ,MIN(BILL_SATISFIED) AS BILL_SATISFIED
        FROM
            ACS_TRANSFER_OUT A
            JOIN STAT_TABX B
                ON A.BF_SSN = B.BF_SSN
/*GET COUNT OF PIF LOANS PREVIOUS TO THE RUN MONTH*/
            LEFT JOIN
                (
                    SELECT
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                        ,COUNT(*) AS PIF_CT_BX_REP_MO
                    FROM 
                        ACS_TRANSFER_OUT 
                    WHERE 
                        &locl_FIRST_DAY_PREV_MON > LD_PIF_RPT 
                        AND LD_PIF_RPT IS NOT NULL 
                    GROUP BY 
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                ) C
                ON A.BF_SSN = C.BF_SSN
/*(X)               AND A.LF_GTR_RFR = C.LF_GTR_RFR*/
/*GET COUNT OF PIF LOANS IN THE RUN MONTH*/
            LEFT JOIN
                (
                    SELECT
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                        ,COUNT(*) AS PIF_CT_REP_MO
                    FROM 
                        ACS_TRANSFER_OUT 
                    WHERE 
                        &locl_FIRST_DAY_PREV_MON <= LD_PIF_RPT <= &locl_LAST_DAY_PREV_MON 
                    GROUP BY
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                ) D
                ON A.BF_SSN = D.BF_SSN
/*(X)               AND A.LF_GTR_RFR = D.LF_GTR_RFR*/
/*GET COUNT OF DCONVERTED LOANS PREVIOUS TO THE RUN MONTH*/
            LEFT OUTER JOIN
                (
                    SELECT 
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                        ,COUNT(*) AS DSTAT_CT_BX_REP_MO
                    FROM 
                        ACS_TRANSFER_OUT 
                    WHERE 
                        LC_STA_LONXX = 'D' 
                        AND LA_OTS_PRI_ELG  = X 
                        AND &locl_FIRST_DAY_PREV_MON > LD_STA_LONXX  
                        AND LD_STA_LONXX IS NOT NULL
                    GROUP BY 
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                ) E
                ON A.BF_SSN = E.BF_SSN
/*(X)               AND A.LF_GTR_RFR = E.LF_GTR_RFR*/
/*GET COUNT OF DCONVERTED LOANS IN THE RUN MONTH*/
            LEFT OUTER JOIN 
                (
                    SELECT 
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                        ,COUNT(*) AS DSTAT_CT_REP_MO
                    FROM 
                        ACS_TRANSFER_OUT 
                    WHERE 
                        LC_STA_LONXX = 'D' 
                        AND LA_OTS_PRI_ELG  = X
                        AND &locl_FIRST_DAY_PREV_MON <= LD_STA_LONXX <= &locl_LAST_DAY_PREV_MON
                    GROUP BY 
                        BF_SSN
/*(X)                       ,LF_GTR_RFR*/
                ) F
                ON A.BF_SSN = F.BF_SSN
/*(X)               AND A.LF_GTR_RFR = F.LF_GTR_RFR*/
        GROUP BY 
            A.BF_SSN
/*(X)           ,A.LF_GTR_RFR*/
        ORDER BY 
/*(X)           CALCULATED IID,*/
            A.BF_SSN
    ;
QUIT;


/*DETERMINE PIF BORROWERS AND DECONVERTED BORROWERS FOR THE REPORTING MONTH*/
DATA FIDRFX_COD;
    SET FIDRFX_COD;
/*    DETERMINE PIF BORROWERS FOR THE REPORTING MONTH*/
    IF SUM(PIF_CT_BX_REP_MO,PIF_CT_REP_MO) = LOAN_COUNT THEN DO;
        IF PIF_CT_REP_MO ^= X THEN ORD = X;
        ELSE ORD = X;
    END;
/*DETERMINE DECONVERTED BORROWERS FOR THE REPORTING MONTH*/
    ELSE IF SUM(DSTAT_CT_BX_REP_MO,DSTAT_CT_REP_MO) = LOAN_COUNT THEN DO;
        IF DSTAT_CT_REP_MO ^= X THEN ORD = X;
        ELSE ORD = X;
    END;
/*PIF and Deconverted MIX*/
    ELSE IF SUM(PIF_CT_BX_REP_MO,PIF_CT_REP_MO,DSTAT_CT_BX_REP_MO,DSTAT_CT_REP_MO)= LOAN_COUNT THEN
        DO;
            IF DSTAT_CT_REP_MO ^= X THEN ORD = X;
            ELSE IF PIF_CT_REP_MO ^= X THEN ORD = X;
            ELSE ORD = X;
        END;
    ELSE
        DO;
            ORD = X;
        END;

    IF ORD = X THEN PIF_TRN_DT = LD_PIF_RPT;
    ELSE IF ORD = X THEN PIF_TRN_DT = LD_STA_LONXX;
    ELSE PIF_TRN_DT = .;

RUN;

DATA FIDRFX_ACS_TRANSFER;
    SET FIDRFX_ACS_TRANSFER;
/*    DETERMINE PIF BORROWERS FOR THE REPORTING MONTH*/
    IF SUM(PIF_CT_BX_REP_MO,PIF_CT_REP_MO) = LOAN_COUNT THEN DO;
        IF PIF_CT_REP_MO ^= X THEN ORD = X;
        ELSE ORD = X;
    END;
/*DETERMINE DECONVERTED BORROWERS FOR THE REPORTING MONTH*/
    ELSE IF SUM(DSTAT_CT_BX_REP_MO,DSTAT_CT_REP_MO) = LOAN_COUNT THEN DO;
        IF DSTAT_CT_REP_MO ^= X THEN ORD = X;
        ELSE ORD = X;
    END;
/*PIF and Deconverted MIX*/
    ELSE IF SUM(PIF_CT_BX_REP_MO,PIF_CT_REP_MO,DSTAT_CT_BX_REP_MO,DSTAT_CT_REP_MO)= LOAN_COUNT THEN
        DO;
            IF DSTAT_CT_REP_MO ^= X THEN ORD = X;
            ELSE IF PIF_CT_REP_MO ^= X THEN ORD = X;
            ELSE ORD = X;
        END;
    ELSE
        DO;
            ORD = X;
        END;

    IF ORD = X THEN PIF_TRN_DT = LD_PIF_RPT;
    ELSE IF ORD = X THEN PIF_TRN_DT = LD_STA_LONXX;
    ELSE PIF_TRN_DT = .;

RUN;

PROC SORT DATA=FIDRFX_ACS_TRANSFER DUPOUT=DUPDS NODUPKEY; 
/*(X)   BY ORD LF_GTR_RFR BF_SSN; */
    BY ORD BF_SSN; 
RUN;

PROC SORT DATA=FIDRFX_COD DUPOUT=DUPDS NODUPKEY; 
/*(X)   BY ORD LF_GTR_RFR BF_SSN; */
    BY ORD BF_SSN; 
RUN;

%ASSIGN_CATS(FIDRFX_COD,FIDRFX_COD_OUT,OTH_BRWS);
%ASSIGN_CATS(FIDRFX_ACS_TRANSFER,FIDRFX_ACS_TRANSFER_OUT,OTH_BRWS);


PROC SORT DATA=FIDRFX_COD_OUT NODUPKEY; 
    BY BOR_INV_NUM; 
RUN;

PROC SORT DATA=FIDRFX_ACS_TRANSFER_OUT NODUPKEY; 
    BY BOR_INV_NUM; 
RUN;

/*ACS Transfer*/
DATA _NULL_;
    SET FIDRFX_COD_OUT;
    WHERE PF NOT IN ('PIF','TRN');

    FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
    FORMAT 
        BF_SSN $X. 
        LD_LON_EFF_ADD MMDDYYXX. 
/*(X)       IID $XX. */
        LN_DLQ_MAX LA_CUR_PRI WA_TOT_BRI_OTS TOT_AMT LOAN_COUNT BESTXX. 
        LON_PGM $X. 
        PF $X. 
    ;
    IF _N_ = X THEN
        DO;
/*(X)           PUT "BORROWER SSN,FSA INVOICE NUMBER,DATE ADDED,PERFORMANCE CATEGORY,PRINCIPAL BALANCE,INTEREST BALANCE,TOTAL PRINCIPAL AND INTEREST,MAX DAYS DELINQUENT,TOTAL NUMBER OF LOANS";*/
            PUT "BORROWER SSN,DATE ADDED,PERFORMANCE CATEGORY,PRINCIPAL BALANCE,INTEREST BALANCE,TOTAL PRINCIPAL AND INTEREST,MAX DAYS DELINQUENT,TOTAL NUMBER OF LOANS";
        END;

    DO;
        PUT BF_SSN $ @;
/*(X)       PUT LF_GTR_RFR $ @;*/
        PUT LD_LON_EFF_ADD @;
        PUT PF $ @;
        PUT LA_CUR_PRI @;
        PUT WA_TOT_BRI_OTS @;
        PUT TOT_AMT @;
        PUT LN_DLQ_MAX @;
        PUT LOAN_COUNT ;
    END;
RUN;

/*COD */
DATA _NULL_;
    SET FIDRFX_ACS_TRANSFER_OUT;
    WHERE PF NOT IN ('PIF','TRN');

    FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
    FORMAT 
        BF_SSN $X. 
        LD_LON_EFF_ADD MMDDYYXX. 
/*(X)       IID $XX. */
        LN_DLQ_MAX LA_CUR_PRI WA_TOT_BRI_OTS TOT_AMT LOAN_COUNT BESTXX. 
        LON_PGM $X. 
        PF $X. 
    ;
    IF _N_ = X THEN
        DO;
/*(X)           PUT "BORROWER SSN,FSA INVOICE NUMBER,DATE ADDED,PERFORMANCE CATEGORY,PRINCIPAL BALANCE,INTEREST BALANCE,TOTAL PRINCIPAL AND INTEREST,MAX DAYS DELINQUENT,TOTAL NUMBER OF LOANS";*/
            PUT "BORROWER SSN,DATE ADDED,PERFORMANCE CATEGORY,PRINCIPAL BALANCE,INTEREST BALANCE,TOTAL PRINCIPAL AND INTEREST,MAX DAYS DELINQUENT,TOTAL NUMBER OF LOANS";
        END;

    DO;
        PUT BF_SSN $ @;
/*(X)       PUT LF_GTR_RFR $ @;*/
        PUT LD_LON_EFF_ADD @;
        PUT PF $ @;
        PUT LA_CUR_PRI @;
        PUT WA_TOT_BRI_OTS @;
        PUT TOT_AMT @;
        PUT LN_DLQ_MAX @;
        PUT LOAN_COUNT ;
    END;
RUN;
/**/
/*/*RX file*/*/
/*DATA _NULL_;*/
/*    SET DFDELQ;  */
/**/
/*    FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*    FORMAT */
/*		DF_SPE_ACC_ID $XX. */
/*		LN_DLQ_MAX  BESTXX. */
/*		LN_SEQ LC_FOR_TYP LC_DFR_TYP $X.    */
/*    ;*/
/**/
/*    IF _N_ = X THEN*/
/*        DO;*/
/*            PUT "ACCOUNT NUMBER,LOAN SEQUENCE,MAX DELINQUENCY,FORBEARANCE TYPE,DEFERMENT TYPE";*/
/*        END;*/
/**/
/*    DO;*/
/*        PUT DF_SPE_ACC_ID $ @;*/
/*        PUT LN_SEQ $ @;*/
/*        PUT LN_DLQ_MAX  @;*/
/*        PUT LC_FOR_TYP $ @;*/
/*        PUT LC_DFR_TYP $ ;*/
/*    END;*/
/*RUN;*/
/**/
/*/*Parse for each flat file category*/*/
/*DATA SX SX SX SXX SXX SXX SXX SXX SXX SXX SXX SXX SXX SXX;*/
/*    SET FIDRFX;*/
/*    IF PF = 'XX' THEN OUTPUT SX;*/
/*    IF PF = 'XX' THEN OUTPUT SX;*/
/*    IF PF = 'XX' THEN OUTPUT SX;*/
/*    IF PF = 'XX' THEN OUTPUT SXX;*/
/*    IF PF = 'XX' THEN OUTPUT SXX;*/
/*    IF PF = 'XX' THEN OUTPUT SXX;*/
/*    IF PF = 'XX' THEN OUTPUT SXX;*/
/*    IF PF = 'XX' THEN OUTPUT SXX;*/
/*    IF PF = 'XX' THEN OUTPUT SXX;*/
/*    IF PF = 'XX' THEN OUTPUT SXX;*/
/*    IF PF = 'XX' THEN OUTPUT SXX;*/
/*    IF PF = 'XX' THEN OUTPUT SXX;*/
/*    IF PF = 'PIF' THEN OUTPUT SXX;*/
/*    IF PF = 'TRN' THEN OUTPUT SXX;*/
/*RUN;*/
/**/
/*%MACRO CNT(NUM);*/
/*DATA S&NUM;*/
/*    SET S&NUM;*/
/*    COUNTER = PUT(_N_,ZX.);*/
/*    FORMAT PF $X.;*/
/*RUN;*/
/*%MEND CNT;*/
/*%CNT(X);*/
/*%CNT(X);*/
/*%CNT(X);*/
/*%CNT(XX);*/
/*%CNT(XX);*/
/*%CNT(XX);*/
/*%CNT(XX);*/
/*%CNT(XX);*/
/*%CNT(XX);*/
/*%CNT(XX);*/
/*%CNT(XX);*/
/*%CNT(XX);*/
/*%CNT(XX);*/
/*%CNT(XX);*/
/**/
/*PROC FORMAT;*/
/*	PICTURE FSABAL LOW-HIGH = 'XXXXXXX.XX';*/
/*RUN;*/
/**/
/*/*flat file macro*/*/
/*%MACRO OUTPT(NUM);*/
/*DATA _NULL_;*/
/*    SET S&NUM;*/
/*    FILE REPORT&NUM DROPOVER LRECL=XXXXX;*/
/*	MONEND = PUT(INTNX('MONTH',TODAY(),-X,'E'),MMDDYYNX.);*/
/*	FORMAT LA_CUR_PRI WA_TOT_BRI_OTS FSABAL.;*/
/**/
/*    PUT COUNTER @;*/
/*    PUT @XX 'XXXXXX' @;*/
/*    PUT @XX BF_SSN @;*/
/*    PUT @XX PF @;*/
/*    PUT @XX LA_CUR_PRI @;*/
/*    PUT @XX WA_TOT_BRI_OTS @;*/
/*    PUT @XX MONEND;*/
/*RUN;*/
/*%MEND OUTPT;*/
/*%OUTPT(X);*/
/*%OUTPT(X);*/
/*%OUTPT(X);*/
/*%OUTPT(XX);*/
/*%OUTPT(XX);*/
/*%OUTPT(XX);*/
/*%OUTPT(XX);*/
/*%OUTPT(XX);*/
/*%OUTPT(XX);*/
/*%OUTPT(XX);*/
/*%OUTPT(XX);*/
/*%OUTPT(XX);*/
/*%OUTPT(XX);*/
/*%OUTPT(XX);*/
/**/
/*%MACRO GETSAMPLE(PF);*/
/*    DATA SOURCESET;*/
/*        SET FIDRFX;*/
/*        WHERE PF = "&PF";*/
/*    RUN;*/
/*    */
/*    DATA B&PF (KEEP=BF_SSN PF);*/
/*        SAMPSIZE=MIN(X,TOTOBS);*/
/*        OBSLEFT=TOTOBS;*/
/*        DO WHILE(SAMPSIZE>X);*/
/*            PICKIT+X;*/
/*            IF RANUNI(X)<SAMPSIZE/OBSLEFT THEN */
/*                DO;*/
/*                    SET SOURCESET POINT=PICKIT NOBS=TOTOBS;*/
/*                    OUTPUT;*/
/*                    SAMPSIZE=SAMPSIZE-X;*/
/*                END;*/
/*            OBSLEFT=OBSLEFT-X;*/
/*        END;*/
/*        STOP;*/
/*    RUN;*/
/*%MEND;*/
/**/
/*/*"CXX Jeremy" file samples*/*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/**/
/*DATA SAMPLES;*/
/*    SET BXX BXX BXX BXX BXX BXX BXX BXX BXX BXX BXX BXX;*/
/*RUN;*/
/**/
/*PROC SQL;*/
/*    CREATE TABLE SAMPLE_DATA AS*/
/*        SELECT*/
/*            FIDRF.*,*/
/*            S.PF AS BRW_PF*/
/*        FROM*/
/*            FIDRF F*/
/*            JOIN SAMPLES S*/
/*                ON F.BF_SSN = S.BF_SSN*/
/*        ORDER BY*/
/*            S.PF,*/
/*            FIDRF.BF_SSN,*/
/*            FIDRF.LN_SEQ*/
/*    ;*/
/*QUIT;*/
/**/
/*%MACRO PRINT_SAMPLE_DATA(RNO);*/
/*    DATA _NULL_;*/
/*        SET SAMPLE_DATA;*/
/*        FILE REPORT&RNO DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*        FORMAT BF_SSN $X. BRW_PF PF $X. LA_OTS_PRI_ELG WA_TOT_BRI_OTS TOT_AMT LN_DLQ_MAX BESTXX. LD_PIF_RPT MMDDYYXX.;*/
/**/
/*        TOT_AMT = LA_OTS_PRI_ELG + WA_TOT_BRI_OTS;*/
/**/
/*        IF _N_ = X THEN */
/*            DO;*/
/*                PUT "BORROWER SSN,PERFORMANCE CATEGORY BILLED,PERFORMANCE CATEGORY LOAN,LOAN SEQUENCE,PRINCIPAL BALANCE,INTEREST BALANCE,TOTAL PRINCIPAL AND INTEREST,MAX DAYS DELINQUENT,PIF DATE";*/
/*            END;*/
/*        DO;*/
/*            PUT BF_SSN $ @;*/
/*            PUT BRW_PF $ @;*/
/*            PUT PF $ @;*/
/*            PUT LN_SEQ @;*/
/*            PUT LA_OTS_PRI_ELG @;*/
/*            PUT WA_TOT_BRI_OTS @;*/
/*            PUT TOT_AMT @;*/
/*            PUT LN_DLQ_MAX @;*/
/*            PUT LD_PIF_RPT;*/
/*        END;*/
/*    RUN;*/
/*%MEND;*/
/**/
/*%PRINT_SAMPLE_DATA(X);*/
/**/
/**/
/*/*"CXX" file samples*/*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(XX);*/
/*%GETSAMPLE(PIF);*/
/*%GETSAMPLE(TRN);*/
/**/
/*DATA SAMPLES;*/
/*    SET BXX BXX BXX BXX BXX BXX BXX BXX BXX BXX BXX BXX BPIF BTRN;*/
/*RUN;*/
/**/
/*PROC SQL;*/
/*    CREATE TABLE SAMPLE_DATA AS*/
/*        SELECT*/
/*            FIDRF.*,*/
/*            S.PF AS BRW_PF*/
/*        FROM*/
/*            FIDRF F*/
/*            JOIN SAMPLES S*/
/*                ON F.BF_SSN = S.BF_SSN*/
/*        ORDER BY*/
/*            S.PF,*/
/*            FIDRF.BF_SSN,*/
/*            FIDRF.LN_SEQ*/
/*    ;*/
/*QUIT;*/
/**/
/*%PRINT_SAMPLE_DATA(X);*/
/**/
/*/*only needed for testing*/*/
/*/*PROC EXPORT*/*/
/*/*      DATA=FIDRF*/*/
/*/*      OUTFILE='T:\SAS\FIDRF.XLSX'*/*/
/*/*      REPLACE;*/*/
/*/*RUN;*/*/
/**/
/**/
/*/*(X) commented out IID and LF_GTR_RFR as we won't get that information from FSA for a while but it will need to be reinstated at some point*/*/
