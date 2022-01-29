/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

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

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						PD10.DF_SPE_ACC_ID
						,LN50.LD_DFR_END
					FROM	
						OLWHRM1.LN10_LON LN10
						INNER JOIN OLWHRM1.LN50_BR_DFR_APV LN50
							ON LN10.BF_SSN = LN50.BF_SSN
							AND LN10.LN_SEQ = LN50.LN_SEQ
							AND LN50.LC_STA_LON50 = 'A'
						INNER JOIN OLWHRM1.DF10_BR_DFR_REQ DF10
							ON LN10.BF_SSN = DF10.BF_SSN
							AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
							AND DF10.LC_DFR_TYP = '06'
						INNER JOIN (
								SELECT
									A.BF_SSN
									,MAX(A.LD_DFR_END) AS LD_DFR_END
								FROM OLWHRM1.LN50_BR_DFR_APV A
								WHERE A.LC_STA_LON50 = 'A'
								GROUP BY A.BF_SSN
									) MX
							ON LN10.BF_SSN = MX.BF_SSN
						INNER JOIN OLWHRM1.PD10_PRS_NME PD10
							ON LN10.BF_SSN = PD10.DF_PRS_ID
					WHERE	
						LN10.LA_CUR_PRI > 0
						AND LN10.IC_LON_PGM = 'TILP'
						AND MX.LD_DFR_END < CURRENT_DATE

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH 20361.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
