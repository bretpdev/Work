/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS46.NWS46RZ";
FILENAME REPORT2 "&RPTLIB/UNWS46.NWS46R2";

LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;
RSUBMIT;

LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

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
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID
						,LN10.LC_STA_LON10
						,DW01.WC_DW_LON_STA
						,PD31.DI_VLD_ADR_HST
						,PD30.DI_VLD_ADR
					FROM
						OLWHRM1.LN10_LON LN10
						INNER JOIN OLWHRM1.BR30_BR_EFT BR30
							ON LN10.BF_SSN = BR30.BF_SSN
							AND  DAYS(BR30.BD_EFT_STA) BETWEEN DAYS(LN10.LD_LON_ACL_ADD) AND DAYS(LN10.LD_LON_ACL_ADD) + 2
						LEFT JOIN (
								SELECT DISTINCT
									A.BF_SSN
								FROM 
									OLWHRM1.LN10_LON B
									INNER JOIN OLWHRM1.RM30_BR_RMT A
										ON A.BF_SSN = B.BF_SSN
									INNER JOIN OLWHRM1.BR30_BR_EFT C
										ON B.BF_SSN = C.BF_SSN
										AND  DAYS(C.BD_EFT_STA) BETWEEN DAYS(B.LD_LON_ACL_ADD) AND DAYS(B.LD_LON_ACL_ADD) + 2
								WHERE 
									A.LC_RMT_BCH_SRC_IPT = 'E'
									AND
									A.PC_FAT_TYP = '10'
									AND
									A.PC_FAT_SUB_TYP = '10'
									AND
									DAYS(A.LD_RMT_PAY_EFF) BETWEEN DAYS(B.LD_LON_ACL_ADD) AND DAYS(LD_LON_ACL_ADD) + 180
									) EXLD
							ON LN10.BF_SSN = EXLD.BF_SSN
						INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
						INNER JOIN OLWHRM1.PD10_PRS_NME PD10
							ON LN10.BF_SSN = PD10.DF_PRS_ID
						LEFT JOIN OLWHRM1.PD30_PRS_ADR PD30
							ON LN10.BF_SSN = PD30.DF_PRS_ID
							AND PD30.DC_ADR = 'L'
						LEFT JOIN OLWHRM1.PD31_PRS_INA PD31
							ON LN10.BF_SSN = PD31.DF_PRS_ID
							AND PD31.DC_ADR_HST = 'L'
							AND PD31.DN_ADR_SEQ = '1'
					WHERE
						EXLD.BF_SSN IS NULL
					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET DUSTER.DEMO; RUN;

/*DATA COMP;*/
/*	SET MSBTFF;*/
/*	NET = LA_CUR_PRI - LA_FAT_CUR_PRI;*/
/*	KEEP NET LA_CUR_PRI LA_FAT_CUR_PRI DF_SPE_ACC_ID BF_SSN LN_SEQ;*/
/*RUN;*/

/*PROC SORT DATA=COMP; BY NET BF_SSN LN_SEQ; RUN;*/


/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH 18823.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
