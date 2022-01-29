/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTX "&RPTLIB/CNH XXXXX.&sysdate..txt";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE EMAIL_DATA AS
        SELECT *
        FROM CONNECTION TO DBX
            (
                SELECT DISTINCT
                    PDXX.DF_SPE_ACC_ID ,
                    RTRIM(PDXX.DM_PRS_X) AS DM_PRS_X,
					RTRIM(PDXX.DM_PRS_LST) AS DM_PRS_LST,
                    COALESCE(PDXXA.DX_ADR_EML,PDXXB.DX_ADR_EML,PDXXC.DX_ADR_EML) AS DX_ADR_EML 
                FROM PKUB.LNXX_LON LNXX
	                LEFT OUTER JOIN PKUB.PDXX_PRS_ADR_EML PDXXA
	                    ON LNXX.BF_SSN = PDXXA.DF_PRS_ID
	                    AND PDXXA.DC_ADR_EML = 'H'
	                    AND PDXXA.DC_STA_PDXX = 'A'
	                    AND PDXXA.DI_VLD_ADR_EML = 'Y'
	                LEFT OUTER JOIN PKUB.PDXX_PRS_ADR_EML PDXXB
	                    ON LNXX.BF_SSN = PDXXB.DF_PRS_ID
	                    AND PDXXB.DC_ADR_EML = 'A'
	                    AND PDXXB.DC_STA_PDXX = 'A'
	                    AND PDXXB.DI_VLD_ADR_EML = 'Y'
	                LEFT OUTER JOIN PKUB.PDXX_PRS_ADR_EML PDXXC
	                    ON LNXX.BF_SSN = PDXXC.DF_PRS_ID
	                    AND PDXXC.DC_ADR_EML = 'W'
	                    AND PDXXC.DC_STA_PDXX = 'A'
	                    AND PDXXC.DI_VLD_ADR_EML = 'Y'
	                LEFT OUTER JOIN PKUB.PDXX_PRS_NME PDXX
	                    ON LNXX.BF_SSN = PDXX.DF_PRS_ID
                WHERE 
					LNXX.LC_STA_LONXX = 'R'
	                AND LNXX.LA_CUR_PRI > X
	                AND (
							PDXXA.DX_ADR_EML IS NOT NULL 
							OR PDXXB.DX_ADR_EML IS NOT NULL 
							OR PDXXC.DX_ADR_EML IS NOT NULL
						)

                FOR READ ONLY WITH UR
                )
;

    DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;

DATA EMAIL_DATA; SET LEGEND.EMAIL_DATA; RUN;

DATA _NULL_;
		SET EMAIL_DATA ;

		FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
		IF _N_ = X THEN DO;
			PUT "DF_SPE_ACC_ID,DM_PRS_X,DM_PRS_LST,DX_ADR_EML";
		END;
		DO;
		   PUT DF_SPE_ACC_ID @;
		   PUT DM_PRS_X @;
		   PUT DM_PRS_LST @;
		   PUT DX_ADR_EML $ ;
		END;
RUN;
