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
			WQ20.BF_SSN
			,WQ20.PF_REQ_ACT
			,WQ20.WN_CTL_TSK
			,WQ20.WX_MSG_1_TSK
			,DW01.WC_DW_LON_STA 
   			,LN65.LC_TYP_SCH_DIS
			,SUM(LN10.LA_CUR_PRI) AS PRINCIPAL
		FROM
			OLWHRM1.LN10_LON LN10
			INNER JOIN OLWHRM1.WQ20_TSK_QUE WQ20
				ON LN10.BF_SSN = WQ20.BF_SSN
			INNER JOIN OLWHRM1.LN65_LON_RPS LN65
				ON LN10.BF_SSN = LN65.BF_SSN
				AND LN10.LN_SEQ = LN65.LN_SEQ
			INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
			AND LN10.LF_LON_CUR_OWN LIKE '829769%'
			AND WQ20.WF_QUE IN ('R0','01')
			AND WQ20.WC_STA_WQUE20 IN ('A','H','P','U','W')
			AND LN65.LC_STA_LON65 = 'A' 
		GROUP BY
			WQ20.BF_SSN
			,WQ20.PF_REQ_ACT
			,WQ20.WN_CTL_TSK
			,WQ20.WX_MSG_1_TSK
			,DW01.WC_DW_LON_STA 
   			,LN65.LC_TYP_SCH_DIS
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

PROC EXPORT
	DATA=Balance
	OUTFILE="&RPTLIB\UNH 26746.xlsx"
	DBMS = EXCEL
	REPLACE;
	SHEET="Borrowers";
RUN;
