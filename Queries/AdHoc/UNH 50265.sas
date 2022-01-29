/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT3 "&RPTLIB/UNH 50265.&sysdate..txt";

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=WORK  ;*/ /*LIVE
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ; /*TEST*/
RSUBMIT;
%LET DB = DLGSUTWH ; /*LIVE*/
/*%LET DB = DLGSWQUT ; /*TEST*/

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


PROC SQL ;
    CONNECT TO DB2 (DATABASE=&DB);

    CREATE TABLE EMAIL_DATA AS
        SELECT *
        FROM CONNECTION TO DB2
            (
                SELECT DISTINCT
                    PD10.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
                    RTRIM(PD10.DM_PRS_1)|| ' ' ||  RTRIM(PD10.DM_PRS_MID)|| ' ' ||  RTRIM(PD10.DM_PRS_LST) AS NAME,
                    COALESCE(PD32A.DX_ADR_EML,PD32B.DX_ADR_EML,PD32C.DX_ADR_EML) AS RECIPIENT
                FROM OLWHRM1.LN10_LON LN10
	                LEFT OUTER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32A
	                    ON LN10.BF_SSN = PD32A.DF_PRS_ID
	                    AND PD32A.DC_ADR_EML = 'H'
	                    AND PD32A.DC_STA_PD32 = 'A'
	                    AND PD32A.DI_VLD_ADR_EML = 'Y'
	                LEFT OUTER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32B
	                    ON LN10.BF_SSN = PD32B.DF_PRS_ID
	                    AND PD32B.DC_ADR_EML = 'A'
	                    AND PD32B.DC_STA_PD32 = 'A'
	                    AND PD32B.DI_VLD_ADR_EML = 'Y'
	                LEFT OUTER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32C
	                    ON LN10.BF_SSN = PD32C.DF_PRS_ID
	                    AND PD32C.DC_ADR_EML = 'W'
	                    AND PD32C.DC_STA_PD32 = 'A'
	                    AND PD32C.DI_VLD_ADR_EML = 'Y'
	                LEFT OUTER JOIN OLWHRM1.PD10_PRS_NME PD10
	                    ON LN10.BF_SSN = PD10.DF_PRS_ID
                WHERE 
					LN10.LC_STA_LON10 = 'R'
	                AND LN10.LA_CUR_PRI > 0
	                AND (
							PD32A.DX_ADR_EML IS NOT NULL 
							OR PD32B.DX_ADR_EML IS NOT NULL 
							OR PD32C.DX_ADR_EML IS NOT NULL
						)

                FOR READ ONLY WITH UR
                )
;

    DISCONNECT FROM DB2;

QUIT;

ENDRSUBMIT;

DATA EMAIL_DATA;
	SET DUSTER.EMAIL_DATA;
RUN;

/*Output R3*/
DATA _NULL_;
	SET EMAIL_DATA;
	FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT ACCOUNT_NUMBER $10.;
	FORMAT NAME $200. ;
	FORMAT RECIPIENT $200. ;

	/* WRITE COLUMN NAMES */
	IF _N_ = 1 THEN        
	DO;
		PUT
		'ACCOUNT NUMBER' ','
		'NAME' ','
		'RECIPIENT' ;
	END;
	DO;
		PUT ACCOUNT_NUMBER $ @;
		PUT NAME $ @;
		PUT RECIPIENT $ ;
	END;
RUN;
