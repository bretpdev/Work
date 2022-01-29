%LET RPTLIB = T:\SAS;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ; /*test*/
RSUBMIT;

%LET DB = DLGSUTWH; /*live*/
/*%LET DB = DLGSWQUT; /*test*/

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
	CREATE TABLE Balance AS
		SELECT 
			*
		FROM CONNECTION TO DB2 
			(
				SELECT DISTINCT
					LN10.BF_SSN AS Borrower_SSN
					,LN10.LN_SEQ AS Loan_Sequence
					,LN10.LC_STA_LON10
					,LN10.LA_CUR_PRI
					,DW01.WC_DW_LON_STA
					,LN80.LI_FNL_BIL_LON
					,LN80.LD_BIL_CRT
					,LN80.LC_STA_LON80
					,LN80.LD_BIL_DU_LON
				FROM
					OLWHRM1.LN10_LON LN10
					INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
						ON LN10.BF_SSN = DW01.BF_SSN
						AND LN10.LN_SEQ = DW01.LN_SEQ
					INNER JOIN 
						(
						SELECT
							BF_SSN
							,LN_SEQ
							,LI_FNL_BIL_LON
							,LN80.LC_STA_LON80
							,LN80.LD_BIL_DU_LON
							,MAX(LN80.LD_BIL_CRT) AS LD_BIL_CRT
						FROM
							OLWHRM1.LN80_LON_BIL_CRF LN80
						WHERE
							LN80.LI_FNL_BIL_LON = 'Y'	/*[Has been final billed]*/
							AND DAYS(LN80.LD_BIL_CRT) < DAYS(CURRENT DATE) - 30  /*[Not billed in over a month]*/
							AND LN80.LC_STA_LON80 = 'A'
							AND DAYS(LN80.LD_BIL_DU_LON) <= DAYS(CURRENT DATE) + 21
						GROUP BY
							BF_SSN
							,LN_SEQ
							,LI_FNL_BIL_LON
							,LN80.LC_STA_LON80
							,LN80.LD_BIL_DU_LON
						)LN80
							ON LN10.BF_SSN = LN80.BF_SSN
							AND LN10.LN_SEQ = LN80.LN_SEQ
				WHERE
					LN10.LC_STA_LON10 = 'R'			/*[Released Loan]*/
					AND LN10.LA_CUR_PRI >= 50.00	/*[Greater than $50 principal]*/
					AND DW01.WC_DW_LON_STA NOT IN (
						'04' /*(Deferment)*/
						,'05' /*(Forbearnce)*/
						,'12' /*(Claim Paid)*/
						,'16', '17', '18', '19', '20', '21' /*(DDB Alleg/Verf)*/
						,'22' /*(PIF)*/
						)
			)
	;
DISCONNECT FROM DB2;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  
%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA Balance;
	SET DUSTER.Balance;
RUN;

PROC SORT 
	DATA=Balance
	OUT=Balance_dedupe
	(keep=BORROWER_SSN LOAN_SEQUENCE)
	NODUPKEY;
	BY BORROWER_SSN LOAN_SEQUENCE;
RUN;

PROC EXPORT
		DATA=Balance_dedupe
		OUTFILE="&RPTLIB\UNH 26138.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="FinalBill_NewRS";
RUN;

PROC EXPORT
		DATA=Balance
		OUTFILE="&RPTLIB\UNH 26138.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="details";
RUN;
