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

	CREATE TABLE FNL_FAT AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						LN10.BF_SSN
						,LN10.LN_SEQ
						,COALESCE(PIF.PIF_DTE, CLM.CLM_DTE) AS FNL_FAT
						,COALESCE(PIF.PYF_TYP,CLM.PYF_TYP) AS PAYOFF_TYPE
					FROM	
						OLWHRM1.LN10_LON LN10
						LEFT JOIN (
								SELECT
									LN90.BF_SSN
									,LN90.LN_SEQ
									,LN90.LD_FAT_EFF AS PIF_DTE
									,CASE
										WHEN LN90.PC_FAT_SUB_TYP IN ('10','20') THEN 'PIF'
										WHEN LN90.PC_FAT_SUB_TYP = '70' THEN 'CONSOL'
									 	ELSE 'ERROR'
									 END AS PYF_TYP
								FROM 
									OLWHRM1.LN90_FIN_ATY LN90
									JOIN(
												SELECT
													LN90.BF_SSN
													,LN90.LN_SEQ
													,LN90.LD_FAT_EFF
													,MAX(LN90.LN_FAT_SEQ) AS LN_FAT_SEQ
												FROM 
													OLWHRM1.LN90_FIN_ATY LN90
													JOIN (
															SELECT
																DW01.BF_SSN
																,DW01.LN_SEQ
																,MAX(LN90.LD_FAT_EFF) AS PIF_DTE
															FROM 
																OLWHRM1.DW01_DW_CLC_CLU DW01
																JOIN OLWHRM1.LN90_FIN_ATY LN90
																	ON DW01.BF_SSN = LN90.BF_SSN
																	AND DW01.LN_SEQ = LN90.LN_SEQ
															WHERE 
																DW01.WC_DW_LON_STA = '22'
																AND
																LN90.PC_FAT_TYP = '10'
																AND
																LN90.PC_FAT_SUB_TYP IN ('10','20','70') 
																AND
																LN90.LC_FAT_REV_REA = ' '
															GROUP BY 
																DW01.BF_SSN
																,DW01.LN_SEQ
															 ) MX_DTE
														ON LN90.BF_SSN = MX_DTE.BF_SSN
														AND LN90.LN_SEQ = MX_DTE.LN_SEQ
														AND LN90.LD_FAT_EFF = MX_DTE.PIF_DTE
												WHERE 	
													LN90.PC_FAT_TYP = '10'
													AND
													LN90.PC_FAT_SUB_TYP IN ('10','20','70') 
													AND
													LN90.LC_FAT_REV_REA = ' '
												GROUP BY 
													LN90.BF_SSN
													,LN90.LN_SEQ
													,LN90.LD_FAT_EFF
										 	) MX_SEQ
										ON LN90.BF_SSN = MX_SEQ.BF_SSN
										AND LN90.LN_SEQ = MX_SEQ.LN_SEQ
										AND LN90.LN_FAT_SEQ = MX_SEQ.LN_FAT_SEQ
									 ) PIF
							ON LN10.BF_SSN = PIF.BF_SSN
							AND LN10.LN_SEQ = PIF.LN_SEQ
						LEFT JOIN (
								SELECT
									DW01.BF_SSN
									,DW01.LN_SEQ
									,'CLAIM' AS PYF_TYP
									,MAX(LN90.LD_FAT_EFF) AS CLM_DTE
								FROM 
									OLWHRM1.DW01_DW_CLC_CLU DW01
									JOIN OLWHRM1.LN40_LON_CLM_PCL LN40
										ON DW01.BF_SSN = LN40.BF_SSN
										AND DW01.LN_SEQ = LN40.LN_SEQ
										AND LN40.LC_TYP_REC_CLP_LON = '1'
										AND LN40.LC_REA_CLP_LON = '06'
									JOIN OLWHRM1.LN90_FIN_ATY LN90
										ON DW01.BF_SSN = LN90.BF_SSN
										AND DW01.LN_SEQ = LN90.LN_SEQ
								WHERE 
									DW01.WC_DW_LON_STA = '12'
									AND
									LN90.PC_FAT_TYP = '10'
									AND
									LN90.PC_FAT_SUB_TYP = '30' 
									AND
									LN90.LC_FAT_REV_REA = ' '
								GROUP BY 
									DW01.BF_SSN
									,DW01.LN_SEQ
									 ) CLM
							ON LN10.BF_SSN = CLM.BF_SSN
							AND LN10.LN_SEQ = CLM.LN_SEQ
					WHERE	
						LN10.IC_LON_PGM NOT IN ('COMPLT','TILP')
						AND
						(PIF.PIF_DTE BETWEEN '02-01-2014' AND '01-31-2016'
						OR
						CLM.CLM_DTE BETWEEN '02-01-2014' AND '01-31-2016')

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA FNL_FAT;
	SET DUSTER.FNL_FAT;
RUN;

DATA FINAL;
	SET FNL_FAT;
	FORMAT MOYR MONYY5.;
	MOYR = FNL_FAT;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.FINAL 
            OUTFILE = "T:\SAS\NH 26171.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
