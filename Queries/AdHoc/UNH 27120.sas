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
CREATE TABLE Queues AS
	SELECT 
		*
	FROM CONNECTION TO DB2 
		(
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID,
			WQ20.WF_QUE,
			WQ20.WF_SUB_QUE,
			LN16.LN_DLQ_MAX
		FROM
			OLWHRM1.LN10_LON LN10
			INNER JOIN OLWHRM1.PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			INNER JOIN OLWHRM1.WQ20_TSK_QUE WQ20
				ON LN10.BF_SSN = WQ20.BF_SSN
			INNER JOIN OLWHRM1.LN16_LON_DLQ_HST LN16
				ON LN10.BF_SSN = LN16.BF_SSN
				AND LN10.LN_SEQ = LN16.LN_SEQ
		WHERE
			LN16.LN_DLQ_MAX >= 290
			AND LN16.LC_STA_LON16 = '1'
			AND LN10.IC_LON_PGM <> 'TILP'
			AND DAYS(LN16.LD_DLQ_MAX) = DAYS(Current Date - 1 Day)
			AND
			(
				(WQ20.WF_QUE ='VB' AND WQ20.WF_SUB_QUE = 'FB')
				OR (WQ20.WF_QUE ='SF' AND WQ20.WF_SUB_QUE = '01')
				OR (WQ20.WF_QUE ='VR' AND WQ20.WF_SUB_QUE = 'FB')
				OR (WQ20.WF_QUE ='ED' AND WQ20.WF_SUB_QUE = '01')
				OR (WQ20.WF_QUE ='VB' AND WQ20.WF_SUB_QUE = 'FB')
				OR (WQ20.WF_QUE ='RB' AND WQ20.WF_SUB_QUE IN('00','10','11','22'))
				OR (WQ20.WF_QUE ='DR' AND WQ20.WF_SUB_QUE = '01')
			)			
		)
;
CREATE TABLE Arcs AS
	SELECT 
		*
	FROM CONNECTION TO DB2 
		(
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID,
			AY10.PF_REQ_ACT,
			AY10.LD_ATY_REQ_RCV,
			LN16.LN_DLQ_MAX
		FROM
			OLWHRM1.LN10_LON LN10
			INNER JOIN OLWHRM1.PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			INNER JOIN OLWHRM1.AY10_BR_LON_ATY AY10
				ON LN10.BF_SSN = AY10.BF_SSN
			INNER JOIN OLWHRM1.LN16_LON_DLQ_HST LN16
				ON LN10.BF_SSN = LN16.BF_SSN
				AND LN10.LN_SEQ = LN16.LN_SEQ
		WHERE
			AY10.LD_ATY_REQ_RCV BETWEEN '2016-03-14' AND Current Date
			AND AY10.PF_REQ_ACT IN ('DIDDA','DIDFR')
			AND LN16.LN_DLQ_MAX >= 290
			AND LN16.LC_STA_LON16 = '1'
			AND LN10.IC_LON_PGM <> 'TILP'
			AND DAYS(LN16.LD_DLQ_MAX) = DAYS(Current Date - 1 Day)
		)
;


DISCONNECT FROM DB2;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  
%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA Queues;
	SET DUSTER.Queues;
RUN;

DATA Arcs;
	SET DUSTER.Arcs;
RUN;

PROC EXPORT
	DATA=Queues
	OUTFILE="&RPTLIB\UNH 27120.xlsx"
	DBMS = EXCEL
	REPLACE;
	SHEET="Queues";
RUN;


PROC EXPORT
	DATA=Arcs
	OUTFILE="&RPTLIB\UNH 27120.xlsx"
	DBMS = EXCEL
	REPLACE;
	SHEET="Arcs";
RUN;
