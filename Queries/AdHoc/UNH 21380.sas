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
					SELECT DISTINCT
						LN10.BF_SSN
					FROM
						OLWHRM1.LN10_LON LN10
						INNER JOIN (
								SELECT DISTINCT
									LN16.BF_SSN
								FROM OLWHRM1.LN16_LON_DLQ_HST LN16
									INNER JOIN (
											SELECT
												LN16.BF_SSN
												,MAX(LN16.LN_DLQ_MAX) AS LN_DLQ_MAX
											FROM 
												OLWHRM1.LN16_LON_DLQ_HST LN16
											WHERE 
												LN16.LC_STA_LON16 = '1'
												AND LN16.LN_DLQ_MAX > 30
											GROUP BY 
												LN16.BF_SSN
												) MX
										ON LN16.BF_SSN = MX.BF_SSN
										AND LN16.LN_DLQ_MAX = MX.LN_DLQ_MAX
										) LN16
							ON LN10.BF_SSN = LN16.BF_SSN
						LEFT JOIN OLWHRM1.SD20_STU_ENR SD20
							ON LN10.BF_SSN = SD20.LF_STU_SSN
							AND SD20.LC_STA_STU_ENR = 'G' 
					WHERE
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'
						AND SD20.LF_STU_SSN IS NULL

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
            OUTFILE = "T:\SAS\NH 21380.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
