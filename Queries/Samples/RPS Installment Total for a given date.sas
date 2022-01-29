libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT LN66.BF_SSN
	,LN66.LN_RPS_SEQ
	,LN66.LN_GRD_RPS_SEQ
	,SUM(LN66.LA_RPS_ISL) AS SUM_LA_RPS_ISL
	,LN66.LN_RPS_TRM
	,RS10.LD_RPS_1_PAY_DU
FROM OLWHRM1.LN66_LON_RPS_SPF LN66
INNER JOIN OLWHRM1.RS10_BR_RPD RS10
	ON LN66.BF_SSN = RS10.BF_SSN
	AND LN66.LN_RPS_SEQ = RS10.LN_RPS_SEQ

WHERE LN66.BF_SSN = '002745910'
AND RS10.LC_STA_RPST10 = 'A'
GROUP BY LN66.BF_SSN,LN66.LN_RPS_SEQ,LN66.LN_GRD_RPS_SEQ,LN66.LN_RPS_TRM
,RS10.LD_RPS_1_PAY_DU
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA DEMO; 
SET WORKLOCL.DEMO; 
RUN;

PROC SORT DATA=DEMO;
BY BF_SSN LN_RPS_SEQ LN_GRD_RPS_SEQ;
RUN;

DATA DEMO; 
SET DEMO; 
MO_DIF = INTCK('MONTH',LD_RPS_1_PAY_DU, DATE() ) + 1;
/*MO_DIF = INTCK('MONTH',LD_RPS_1_PAY_DU, '25OCT2013'D ) + 1;*/
RUN;

DATA DEMO;
SET DEMO;
BY BF_SSN LN_RPS_SEQ;
RETAIN MO_OF_PMT;
IF FIRST.LN_RPS_SEQ THEN DO;
	MO_OF_PMT = LN_RPS_TRM;
	END;
ELSE DO;
	MO_OF_PMT + LN_RPS_TRM;
	END;
RUN;

DATA DEMO; 
SET DEMO; 
IF MO_DIF <= MO_OF_PMT THEN DO;
	RECENT_INSTALLMENT = SUM_LA_RPS_ISL;
	OUTPUT;
	END;
RUN;

DATA DEMO; 
SET DEMO; 
BY BF_SSN LN_RPS_SEQ;
IF FIRST.LN_RPS_SEQ;
RUN;

DATA DEMO (KEEP=BF_SSN TOTAL_INSTALLMENT LD_RPS_1_PAY_DU);
SET DEMO;
BY BF_SSN;
TOTAL_INSTALLMENT + RECENT_INSTALLMENT;
IF LAST.BF_SSN THEN OUTPUT;
RUN;

