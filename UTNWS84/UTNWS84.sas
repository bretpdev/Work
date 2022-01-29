/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS84.NWS84RZ";
FILENAME REPORT2 "&RPTLIB/UNWS84.NWS84R2";

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

PROC SQL ;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE S84 AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID
					FROM
						PKUB.PD10_PRS_NME PD10
						JOIN PKUB.LN90_FIN_ATY DSCH /*discharged*/
							ON PD10.DF_PRS_ID = DSCH.BF_SSN
							AND DSCH.PC_FAT_TYP = '50'
							AND DSCH.PC_FAT_SUB_TYP = '02'
						JOIN PKUB.LN90_FIN_ATY DISB /*disbursement*/
							ON DSCH.BF_SSN = DISB.BF_SSN
							AND DSCH.LN_SEQ = DISB.LN_SEQ
							AND
								(
									(DISB.PC_FAT_TYP = '01' AND DISB.PC_FAT_SUB_TYP = '01')
									OR (DISB.PC_FAT_TYP = '07' AND DISB.PC_FAT_SUB_TYP = '85')
									OR (DISB.PC_FAT_TYP = '07' AND DISB.PC_FAT_SUB_TYP = '86') 
								)
					WHERE
						DISB.LD_FAT_PST > DSCH.LD_FAT_PST /*disbursement after the discharge*/
		
					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA S84; SET LEGEND.S84; RUN;

/*export to queue builder (fed) file*/
DATA _NULL_;
	SET S84;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	PUT DF_SPE_ACC_ID 'DISBA,,,,,,,ALL,Disbursement or Disbursement adjustment exists after discharge. Please review account' ;
RUN;
