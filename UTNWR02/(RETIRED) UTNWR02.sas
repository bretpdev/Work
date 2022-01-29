/*****************************************************************************
UTNWR02 - Cash Receipts Report
******************************************************************************/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\WINDOWS\TEMP;
FILENAME REPORT2 "&RPTLIB/UNWR02.NWR02R2";
FILENAME REPORTZ "&RPTLIB/UNWR02.NWR02RZ";
DATA _NULL_;
	CALL SYMPUT('PREV_BEG',"'"||PUT(INTNX('MONTH',TODAY(),-1,'BEGINNING'),MMDDYY10.)||"'");
	CALL SYMPUT('PREV_END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'END'),MMDDYY10.)||"'");
RUN;
%PUT &PREV_BEG;
%PUT &PREV_END;
%SYSLPUT PREV_BEG = &PREV_BEG;
%SYSLPUT PREV_END = &PREV_END;
LIBNAME  WORKLOCL  REMOTE  SERVER=LEGEND  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DNFPUTDL) ;
CREATE TABLE CRRFED AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.PC_TRY_SCH_SRC 
	,A.PC_TRY_SCH_SUB_SRC 
	,A.PD_TRY_SCH_DPS
	,A.PF_TRY_SCH_NUM
	,COALESCE(A.PN_TOT_TRY_SCH_ITM,B.LN_RMT_BCH_ITM_TOT) AS PN_TOT_TRY_SCH_ITM
	,A.PA_TOT_TRY_SCH_AMT
	,A.PC_TRY_SCH_IN_OUT
	,A.PN_TRY_SCH_REC_SEQ
FROM PKUB.FS0A_TRY_SCH_NUM A
LEFT OUTER JOIN (
	SELECT LF_RMT_BCH_SCH_NUM  
		,SUM(LN_RMT_BCH_ITM_TOT) AS LN_RMT_BCH_ITM_TOT
	FROM PKUB.RM10_RMT_BCH
	GROUP BY LF_RMT_BCH_SCH_NUM 
	) B
	ON A.PF_TRY_SCH_NUM = B.LF_RMT_BCH_SCH_NUM
WHERE A.PD_TRY_SCH_UPD BETWEEN &PREV_BEG AND &PREV_END
	AND A.PC_TRY_SCH_TYP IN ('01','02','03','05')
	AND A.PC_TRY_SCH_REC_STA = 'A'
	AND A.PC_SCH_FMS_RPT_STA = 'R'
	AND SUBSTR(A.PF_TRY_SCH_NUM,1,3) NOT IN ('MEM','303','310')
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
ENDRSUBMIT;
DATA CRRFED;
	SET WORKLOCL.CRRFED;
	N = _N_;
RUN;
/********************************************************************
PROCESSING MACROS
*********************************************************************/
%MACRO GRP_SETUP(DSA,MAX_I,DESC1,DESC2,DESC3,DESC4,DESC5);
DATA &DSA;
	LENGTH  GRP 4. GRP_DESC $50.;
	%DO I=1 %TO &MAX_I;
		GRP = &I; GRP_DESC = "&&DESC&I"; OUTPUT;
	%END;
RUN;
%MEND GRP_SETUP;
%MACRO GET_SUBGRP_DS(DS,G,GB_STR,CR8_DSREP);
%IF &CR8_DSREP %THEN %DO;
	PROC SQL;
	CREATE TABLE &DS.REP (drop=bGRP)AS 
		SELECT A.GRP 
			,A.GRP_DESC 
			,B.*
		FROM &DS.GRPS A
		LEFT OUTER JOIN &DS B
			ON A.GRP = B.bGRP
		ORDER BY A.GRP;
	QUIT;
%END;
PROC SQL;
CREATE TABLE &DS.REP_&G AS
	SELECT GRP_DESC AS GRP_DESC_&G
		,PD_TRY_SCH_DPS AS pd_try_sch_dps_&G
		,PF_TRY_SCH_NUM AS PF_TRY_SCH_NUM_&G
		,SUM(PN_TOT_TRY_SCH_ITM) AS pn_tot_try_sch_itm_&G 
		,SUM(PA_TOT_TRY_SCH_AMT) AS PA_TOT_TRY_SCH_AMT_&G
	FROM &DS.REP
	WHERE GRP = &G 
	GROUP BY &GB_STR
;
QUIT;
/*CREATE SUB TOTAL ROWS*/
DATA &DS.REP_&G._SUB; 
	SET &DS.REP_&G END=LAST;
	ITM_TEMP+pn_tot_try_sch_itm_&G;
	TOT_TEMP+PA_TOT_TRY_SCH_AMT_&G;
	DROP pn_tot_try_sch_itm_&G PA_TOT_TRY_SCH_AMT_&G;
	RENAME ITM_TEMP = pn_tot_try_sch_itm_&G
	TOT_TEMP = PA_TOT_TRY_SCH_AMT_&G;
	GRP_DESC_&G = 'SUBTOTAL';
	pd_try_sch_dps_&G = .;
	PF_TRY_SCH_NUM_&G = '';
	IF LAST;
RUN;
%MEND GET_SUBGRP_DS;
%MACRO MRGDAT(RDS,MAX_I);
	DATA _NULL_;
		CALL SYMPUT ('STR1','&RDS.REP_&I');
		CALL SYMPUT ('STR2','&RDS.REP_&I._SUB');
	RUN;
	%DO J=1 %TO 2;
		DATA _NULL_;
			LENGTH DS_STR $200;
				RETAIN DS_STR;
				%DO I=1 %TO &MAX_I;
					%IF &I=1 %THEN %DO;
						DS_STR = "&&STR&J" ;
					%END;
					%ELSE %DO;
						DS_STR = CATX(' ',DS_STR,"&&STR&J") ;
					%END;
				%END;
			CALL SYMPUTX ("DS_LST&J",DS_STR);
	 	RUN;
		%PUT >>> DS_LST&J = &&DS_LST&J;
	%END;
	DATA &RDS._G; MERGE &DS_LST1; RUN;
	DATA &RDS._G_SUB; MERGE &DS_LST2; RUN;
	DATA &RDS._G_ALL;
		SET &RDS._G &RDS._G_SUB;
		%DO I=1 %TO &MAX_I;
			IF GRP_DESC_&I ^= 'SUBTOTAL' THEN GRP_DESC_&I = '';
		%END;
	RUN;
	%GLOBAL &RDS.GTN &RDS.GTD;
	DATA _NULL_;
		SET &RDS;
		TOT_NUM+PN_TOT_TRY_SCH_ITM;
		TOT_DOL+PA_TOT_TRY_SCH_AMT;
		CALL SYMPUTX("&RDS.GTN",PUT(TOT_NUM,COMMA9.));
		CALL SYMPUTX("&RDS.GTD",PUT(TOT_DOL,DOLLAR18.2));
	RUN;
	PROC DATASETS;
		DELETE &RDS &RDS.GRPS &RDS.REP &RDS._G &RDS._G_SUB &DS_LST1 &DS_LST2 ;
	QUIT;
%MEND;
%MACRO SECIT(DSNUM,ICT,VI,VT);
	DATA SEC&DSNUM(DROP=I);
		%DO J=&VI %TO &VT;
		FORMAT pd_try_sch_dps_&J mmddyy10. PF_TRY_SCH_NUM_&J $10. 
			pn_tot_try_sch_itm_&J PA_TOT_TRY_SCH_AMT_&J best12.;
		%END;
		DO I=1 TO &ICT;
				RID=I;output;
		END;
	RUN;
%MEND;
/********************************************************************
R2 PROCESSING
*********************************************************************/
%GRP_SETUP(R2GRPS,5,Borrower - Paper,Borrower - ECP,School/Lender,FRB,Billpay ACH);
DATA R2;
	SET CRRFED;
	WHERE PC_TRY_SCH_SRC IN ('LBX','CGW');
	IF PC_TRY_SCH_SUB_SRC IN ('PPR') THEN bGRP = 1; 
	ELSE IF PC_TRY_SCH_SUB_SRC IN ('ECP') THEN bGRP = 2; 
	ELSE IF PC_TRY_SCH_SUB_SRC IN ('IST','CAN') THEN bGRP = 3; 
	ELSE IF PC_TRY_SCH_SUB_SRC IN ('FRB') THEN bGRP = 4; 
	ELSE IF PC_TRY_SCH_SUB_SRC IN ('ACH','REX') THEN bGRP = 5; 
	IF bGRP;
RUN;
DATA R5XOUT; SET R2; RUN;
%GET_SUBGRP_DS(R2,1,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),1);
%GET_SUBGRP_DS(R2,2,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),0);
%GET_SUBGRP_DS(R2,3,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),0);
%GET_SUBGRP_DS(R2,4,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),0);
%GET_SUBGRP_DS(R2,5,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),0);
%MRGDAT(R2,5); 
DATA R2LROW (DROP=GRP_DESC_1-GRP_DESC_5);
	SET R2_G_ALL END=LAST;
	PF_TRY_SCH_NUM_1 = GRP_DESC_1;
	PF_TRY_SCH_NUM_2 = GRP_DESC_2;
	PF_TRY_SCH_NUM_3 = GRP_DESC_3;
	PF_TRY_SCH_NUM_4 = GRP_DESC_4;
	PF_TRY_SCH_NUM_5 = GRP_DESC_5;
	RENAME pd_try_sch_dps_3 = pd_try_sch_dps_4
		PF_TRY_SCH_NUM_3 = PF_TRY_SCH_NUM_4
		pn_tot_try_sch_itm_3 = pn_tot_try_sch_itm_4
		PA_TOT_TRY_SCH_AMT_3 = PA_TOT_TRY_SCH_AMT_4

		pd_try_sch_dps_4 = pd_try_sch_dps_5 
		PF_TRY_SCH_NUM_4 = PF_TRY_SCH_NUM_5
		pn_tot_try_sch_itm_4 = pn_tot_try_sch_itm_5
		PA_TOT_TRY_SCH_AMT_4 = PA_TOT_TRY_SCH_AMT_5

		pd_try_sch_dps_5 = pd_try_sch_dps_7 
		PF_TRY_SCH_NUM_5 = PF_TRY_SCH_NUM_7
		pn_tot_try_sch_itm_5 = pn_tot_try_sch_itm_7 
		PA_TOT_TRY_SCH_AMT_5 = PA_TOT_TRY_SCH_AMT_7;
	IF LAST;
RUN;
DATA R2DAT (DROP=GRP_DESC_1-GRP_DESC_5);
	SET R2_G_ALL END=LAST;
	RID = _N_;
	RENAME pd_try_sch_dps_3 = pd_try_sch_dps_4
		PF_TRY_SCH_NUM_3 = PF_TRY_SCH_NUM_4
		pn_tot_try_sch_itm_3 = pn_tot_try_sch_itm_4
		PA_TOT_TRY_SCH_AMT_3 = PA_TOT_TRY_SCH_AMT_4

		pd_try_sch_dps_4 = pd_try_sch_dps_5 
		PF_TRY_SCH_NUM_4 = PF_TRY_SCH_NUM_5
		pn_tot_try_sch_itm_4 = pn_tot_try_sch_itm_5
		PA_TOT_TRY_SCH_AMT_4 = PA_TOT_TRY_SCH_AMT_5

		pd_try_sch_dps_5 = pd_try_sch_dps_7 
		PF_TRY_SCH_NUM_5 = PF_TRY_SCH_NUM_7
		pn_tot_try_sch_itm_5 = pn_tot_try_sch_itm_7 
		PA_TOT_TRY_SCH_AMT_5 = PA_TOT_TRY_SCH_AMT_7;
	IF LAST THEN DELETE;
RUN;
/*SPLIT OUT SECTION1 GROUP2 INTO ANOTHER GROUP FOR COLUMN THREE IF NECESSARY*/
DATA R2DATS2 R2DATS3;
	SET R2DAT (KEEP=RID pd_try_sch_dps_2 PF_TRY_SCH_NUM_2 pn_tot_try_sch_itm_2 PA_TOT_TRY_SCH_AMT_2);
	IF RID > 23 THEN OUTPUT R2DATS3;
	ELSE OUTPUT R2DATS2;
RUN;
DATA R2DATS3;
	SET R2DATS3;
	RENAME pd_try_sch_dps_2 = pd_try_sch_dps_3
			PF_TRY_SCH_NUM_2 = PF_TRY_SCH_NUM_3
			pn_tot_try_sch_itm_2 = pn_tot_try_sch_itm_3
			PA_TOT_TRY_SCH_AMT_2 = PA_TOT_TRY_SCH_AMT_3;
			RID = _N_;
RUN;
/*Note ICT is one less than the rows because the total row will be stacked*/
%SECIT(1,23,1,3);
%SECIT(2,27,4,6);
%SECIT(3,27,7,9);
%MACRO MPRP(OTDS,K);
	PROC SORT DATA=R2DAT OUT=&OTDS (KEEP=&K);
	BY RID;
	RUN;
%MEND;
/*PUT SECTION 1 TOGETHER*/
%MPRP(R2DATS1,pd_try_sch_dps_1 PF_TRY_SCH_NUM_1 pn_tot_try_sch_itm_1 PA_TOT_TRY_SCH_AMT_1 RID);
DATA SEC1_FIN;
	MERGE SEC1(IN=A) R2DATS1 R2DATS2 R2DATS3;
	BY RID;
	IF A;
RUN;
DATA SEC1_FIN;
	SET SEC1_FIN R2LROW(KEEP=pd_try_sch_dps_1 PF_TRY_SCH_NUM_1 pn_tot_try_sch_itm_1 PA_TOT_TRY_SCH_AMT_1
		pd_try_sch_dps_2 PF_TRY_SCH_NUM_2 pn_tot_try_sch_itm_2 PA_TOT_TRY_SCH_AMT_2);
RUN;
/*PUT SECTION 2 TOGETHER*/
%MPRP(R2DATS2B,pd_try_sch_dps_4 PF_TRY_SCH_NUM_4 pn_tot_try_sch_itm_4 PA_TOT_TRY_SCH_AMT_4 
	pd_try_sch_dps_5 PF_TRY_SCH_NUM_5 pn_tot_try_sch_itm_5 PA_TOT_TRY_SCH_AMT_5 RID);
DATA SEC2_FIN;
	MERGE SEC2(IN=A) R2DATS2B;
	BY RID;
	IF A;
RUN;
DATA SEC2_FIN;
	SET SEC2_FIN R2LROW(KEEP=pd_try_sch_dps_4 PF_TRY_SCH_NUM_4 pn_tot_try_sch_itm_4 PA_TOT_TRY_SCH_AMT_4
		pd_try_sch_dps_5 PF_TRY_SCH_NUM_5 pn_tot_try_sch_itm_5 PA_TOT_TRY_SCH_AMT_5);
RUN;
/*PUT SECTION 3 TOGETHER*/
%MPRP(R2DATS3B,pd_try_sch_dps_7 PF_TRY_SCH_NUM_7 pn_tot_try_sch_itm_7 PA_TOT_TRY_SCH_AMT_7 RID);
DATA SEC3_FIN;
	MERGE SEC3(IN=A) R2DATS3B;
	BY RID;
	IF A;
RUN;
DATA SEC3_FIN;
	SET SEC3_FIN R2LROW(KEEP=pd_try_sch_dps_7 PF_TRY_SCH_NUM_7 pn_tot_try_sch_itm_7 PA_TOT_TRY_SCH_AMT_7);
RUN;
%PUT >>> &R2GTN >>> &R2GTD;
/********************************************************************
R3 PROCESSING
*********************************************************************/
%GRP_SETUP(R3GRPS,3,Direct Debit,Web,Tel,,);
DATA R3;
	SET CRRFED;
	WHERE PC_TRY_SCH_SRC = 'PAY';
	IF PC_TRY_SCH_SUB_SRC IN ('PPD') THEN DO;
		bGRP = 1; 
	END;
	ELSE IF PC_TRY_SCH_SUB_SRC IN ('WEB') THEN DO;
		bGRP = 2; 
	END;
	ELSE IF PC_TRY_SCH_SUB_SRC IN ('TEL') THEN DO;
		bGRP = 3; 
	END;
	IF bGRP;
RUN;
DATA R5XOUT; SET R5XOUT R3; RUN;
%GET_SUBGRP_DS(R3,1,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),1);
%GET_SUBGRP_DS(R3,2,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),0);
%GET_SUBGRP_DS(R3,3,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),0);
%MRGDAT(R3,3); 
%PUT >>> &R3GTN >>> &R3GTD;
/********************************************************************
R4 PROCESSING
*********************************************************************/
%GRP_SETUP(R4GRPS,2,IPAC Received,IPAC Sent,,,);
DATA R4;
	SET CRRFED;
	WHERE PC_TRY_SCH_SRC = 'IPC';
	IF PC_TRY_SCH_IN_OUT IN ('I') THEN DO;
			bGRP = 1; 
	END;	
	ELSE IF PC_TRY_SCH_IN_OUT IN ('O') THEN DO;
			bGRP = 2; 
	END;	
	IF bGRP;
RUN;
DATA R5XOUT; SET R5XOUT R4; RUN;
%GET_SUBGRP_DS(R4,1,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),1);
%GET_SUBGRP_DS(R4,2,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),0);
%MRGDAT(R4,2); 
%PUT >>> &R4GTN >>> &R4GTD;
/********************************************************************
R5 PROCESSING
*********************************************************************/
%GRP_SETUP(R5GRPS,3,MEMREFS,SF 1081 Z-Canellcations,Other,,);

DATA R5;
	SET CRRFED;
	IF SUBSTR(PF_TRY_SCH_NUM,1,6) = 'MEMREF' THEN 
		bGRP = 1; 
	ELSE IF SUBSTR(PF_TRY_SCH_NUM,1,1) = 'Z' THEN
		bGRP = 2;
	IF bGRP;
RUN;

PROC SQL;
	CREATE TABLE R5T AS 
	SELECT DISTINCT A.*
		,3 AS bGRP  
	FROM CRRFED A
	WHERE A.N NOT IN (
		SELECT N
		FROM R5XOUT
		);
QUIT;

DATA R5;
	SET R5 R5T;
RUN;

%GET_SUBGRP_DS(R5,1,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),1);
%GET_SUBGRP_DS(R5,2,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),0);
%GET_SUBGRP_DS(R5,3,%STR(GRP_DESC,PD_TRY_SCH_DPS,PF_TRY_SCH_NUM),0);
%MRGDAT(R5,3); 
%PUT >>> &R5GTN >>> &R5GTD;
/********************************************************************
R6 PROCESSING
*********************************************************************/
PROC SQL;
	CREATE TABLE R6 AS 
	SELECT DISTINCT ' ' AS C1
		,A.PD_TRY_SCH_DPS
		,B.POS_ITM_CT
		,B.POS_TOT_AMT
		,C.NEG_ITM_CT
		,C.NEG_TOT_AMT
		,1 AS SV
	FROM CRRFED A
	LEFT OUTER JOIN (
		SELECT PD_TRY_SCH_DPS
			,SUM(PN_TOT_TRY_SCH_ITM) AS POS_ITM_CT
			,SUM(PA_TOT_TRY_SCH_AMT) AS POS_TOT_AMT
		FROM CRRFED
		WHERE PA_TOT_TRY_SCH_AMT> 0
		GROUP BY PD_TRY_SCH_DPS
	) B
	ON A.PD_TRY_SCH_DPS = B.PD_TRY_SCH_DPS
	LEFT OUTER JOIN (
		SELECT PD_TRY_SCH_DPS
			,SUM(PN_TOT_TRY_SCH_ITM) AS NEG_ITM_CT
			,SUM(PA_TOT_TRY_SCH_AMT) AS NEG_TOT_AMT
		FROM CRRFED
		WHERE PA_TOT_TRY_SCH_AMT< 0
		GROUP BY PD_TRY_SCH_DPS
	) C
	ON A.PD_TRY_SCH_DPS = C.PD_TRY_SCH_DPS
UNION
	SELECT DISTINCT 'TOTALS' AS C1
		,'' AS PD_TRY_SCH_DPS
		,B.POS_ITM_CT
		,B.POS_TOT_AMT
		,C.NEG_ITM_CT
		,C.NEG_TOT_AMT
		,2 AS SV
	FROM (
		SELECT SUM(PN_TOT_TRY_SCH_ITM) AS POS_ITM_CT
			,SUM(PA_TOT_TRY_SCH_AMT) AS POS_TOT_AMT
		FROM CRRFED
		WHERE PA_TOT_TRY_SCH_AMT> 0
	) B
	FULL OUTER JOIN (
		SELECT SUM(PN_TOT_TRY_SCH_ITM) AS NEG_ITM_CT
			,SUM(PA_TOT_TRY_SCH_AMT) AS NEG_TOT_AMT
		FROM CRRFED
		WHERE PA_TOT_TRY_SCH_AMT< 0
	) C
	ON B.POS_ITM_CT = B.POS_ITM_CT
UNION
	SELECT DISTINCT 'NET' AS C1
		,'' AS PD_TRY_SCH_DPS
		,SUM(PN_TOT_TRY_SCH_ITM) AS POS_ITM_CT
		,SUM(PA_TOT_TRY_SCH_AMT) AS POS_TOT_AMT
		,'' AS NEG_ITM_CT
		,'' AS NEG_TOT_AMT
		,3 AS SV
	FROM CRRFED
;
QUIT;
PROC SORT DATA=R6;
	BY SV PD_TRY_SCH_DPS;
RUN;
/********************************************************************
Set up data dictionary data set
*********************************************************************/
data dd;
	format tab $30. Desc $1000.;
	sv=1; tab='Lockbox'; Desc ='Borrower – Paper = All non-ECP payments'; output;
	sv=1; tab='Lockbox'; Desc =' Borrower – ECP = All payments processed under Electronic Check Presentment (ECP)'; output;
	sv=1; tab='Lockbox'; Desc =' School/Lender – All payments received from schools or lenders'; output;
	sv=1; tab='Lockbox'; Desc =' FRB – All payments processed at the Federal Reserve Bank (FRB)'; output;
	sv=1; tab='Lockbox'; Desc =' Billpay ACH – All electronic payments processed by lockbox (billpay vendors)'; output;
	sv=1; tab='Lockbox'; Desc =' Schedule Date = Date of the schedule'; output;
	sv=1; tab='Lockbox'; Desc =' Schedule Number = Schedule (deposit) number assigned to the batches of payments'; output;
	sv=1; tab='Lockbox'; Desc =' Schedule Type = Type of schedule for the payments'; output;
	sv=1; tab='Lockbox'; Desc =' Item Count = Total number of payments in a schedule number'; output;
	sv=1; tab='Lockbox'; Desc =' Total Amount = Total dollar amount of payments in a schedule number'; output;
	sv=1; tab='Lockbox'; Desc =' Subtotals = Total of items and dollar amount for each payment type'; output;
	sv=1; tab='Lockbox'; Desc =' Grand Total = Total of items and dollar amount for all lockbox payments for the month'; output;

	sv=2; tab='Pay.gov'; Desc =' ACH = Recurring direct debit payments Web = One time payments received through the Online Payment Process (OPS)'; output;
	sv=2; tab='Pay.gov'; Desc =' Tel = One time telephone paymentsSchedule Date = Date of the schedule'; output;
	sv=2; tab='Pay.gov'; Desc =' Schedule Number = Schedule (deposit) number assigned to the batches of payments'; output;
	sv=2; tab='Pay.gov'; Desc =' Schedule Type = Type of schedule for the payments'; output;
	sv=2; tab='Pay.gov'; Desc =' Item Count = Total number of payments in a schedule number'; output;
	sv=2; tab='Pay.gov'; Desc =' Total Amount = Total dollar amount of payments in a schedule number'; output;
	sv=2; tab='Pay.gov'; Desc =' Subtotals = Total of items and dollar amount for each payment type'; output;
	sv=2; tab='Pay.gov'; Desc =' Grand Total = Total of items and dollar amount for all lockbox payments for the month'; output;

	sv=3; tab='IPAC'; Desc =' IPAC Received = Payments forwarded from other servicers'; output;
	sv=3; tab='IPAC'; Desc =' IPAC Sent = Payments forwarded to other servicersSchedule Number = Schedule (deposit) number assigned to the batches of payments'; output;
	sv=3; tab='IPAC'; Desc =' Schedule Type = Type of schedule for the payments'; output;
	sv=3; tab='IPAC'; Desc =' Item Count = Total number of payments in a schedule number'; output;
	sv=3; tab='IPAC'; Desc =' Total Amount = Total dollar amount of payments in a schedule number'; output;
	sv=3; tab='IPAC'; Desc =' Subtotals = Total of items and dollar amount for each payment type'; output;
	sv=3; tab='IPAC'; Desc =' Grand Total = Total of items and dollar amount for all lockbox payments for the month'; output;

	sv=4; tab='Other'; Desc ='(We will include the information for each payment type that can be identified from month-to-month'; output;
	sv=4;tab='Other'; Desc ='Schedule Type = Type of schedule for the payments'; output;
	sv=4;tab='Other'; Desc ='Item Count = Total number of payments in a schedule number'; output;
	sv=4;tab='Other'; Desc ='Total Amount = Total dollar amount of payments in a schedule number'; output;
	sv=4;tab='Other'; Desc ='Subtotals = Total of items and dollar amount for each payment type'; output;
	sv=4;tab='Other'; Desc ='Grand Total = Total of items and dollar amount for all lockbox payments for the month'; output;

	sv=5; tab='Totals'; Desc =' Schedule Date = Date of the schedule'; output;
	sv=5; tab='Totals'; Desc =' Item Count = Total number of payments received in schedules for the reported date'; output;
	sv=5; tab='Totals'; Desc =' Total Amount = Total dollar amount of payments received in schedules for the reported date'; output;
	sv=5; tab='Totals'; Desc =' Item Count = Total number of returns/adjustments received in schedules for the reported date'; output;
	sv=5; tab='Totals'; Desc =' Total Amount = Total dollar amount of returns/adjustments received for the reported date'; output;
	sv=5; tab='Totals'; Desc =' Subtotals = Total of items and dollar amount for the payments and returns/adjustments'; output;
	sv=5; tab='Totals'; Desc =' Grand Totals = Total of items and net dollar amount for all payments and returns/adjustments for the month being reported'; output;
run;
/********************************************************************
REPORTING
*********************************************************************/
/*filename trep 'T:\Windows\Temp\trepx';*/
OPTIONS MISSING='';
ODS LISTING CLOSE;
ODS TAGSETS.EXCELXP STYLE=MINIMAL FILE=REPORT2 options(
	embedded_titles='yes' 
	sheet_name='Cash Receipts Report - Lockbox'
	width_points='1'
	width_fudge='1'
	absolute_column_width='47,49,50,65'
	embedded_footnotes='yes'
	sheet_interval='none'
	ORIENTATION = 'landscape'
	autofit_height = 'yes'
	);
ODS ESCAPECHAR='^';
Title 'FedLoan Servicing';
Title2 'Monthly Cash Receipts and Debits Detail Report';
Title3 'Fed Servicer ID: 700502';
footnote; 
footnote2; 
PROC REPORT DATA=sec1_fin NOWD SPLIT='~' 
	style(header)={background=lightgray font_size=9pt font=(Calibri) font_weight=bold}
	style(column)={font_size=8pt font=(Calibri) };
COLUMN ('Borrower (#530210) - PAPER'( 
	pd_try_sch_dps_1
	PF_TRY_SCH_NUM_1 
	pn_tot_try_sch_itm_1 
	PA_TOT_TRY_SCH_AMT_1)) 
('Borrower (#530210) - ECP'(
	pd_try_sch_dps_2 
	PF_TRY_SCH_NUM_2 
	pn_tot_try_sch_itm_2 
	PA_TOT_TRY_SCH_AMT_2)) 
('Borrower (#530210) - ECP(cont)'(
	pd_try_sch_dps_3 
	PF_TRY_SCH_NUM_3 
	pn_tot_try_sch_itm_3 
	PA_TOT_TRY_SCH_AMT_3)) 
;
	DEFINE pd_try_sch_dps_1 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_1 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_1 / DISPLAY "Item Count"
		style(column)={tagattr='Format:#,##0_);(#,##0)'}
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_1 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};

	DEFINE pd_try_sch_dps_2 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_2 / DISPLAY format=$10. "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_2 / DISPLAY "Item Count"
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_2 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};

	DEFINE pd_try_sch_dps_3 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_3 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_3 / DISPLAY "Item Count"
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_3 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};
RUN;
TITLE ;
TITLE2;
TITLE3;
PROC REPORT DATA=sec2_fin NOWD SPLIT='~' 
	style(header)={background=lightgray font_size=9pt font=(Calibri) font_weight=bold}
	style(column)={font_size=8pt font=(Calibri) };
COLUMN ('School/Lender(#530246)'( 
	pd_try_sch_dps_4
	PF_TRY_SCH_NUM_4 
	pn_tot_try_sch_itm_4 
	PA_TOT_TRY_SCH_AMT_4)) 
('FRB'(
	pd_try_sch_dps_5 
	PF_TRY_SCH_NUM_5 
	pn_tot_try_sch_itm_5 
	PA_TOT_TRY_SCH_AMT_5)) 
('<INTENTIONALLY BLANK>'(
	pd_try_sch_dps_6 
	PF_TRY_SCH_NUM_6 
	pn_tot_try_sch_itm_6 
	PA_TOT_TRY_SCH_AMT_6)) 
;
	DEFINE pd_try_sch_dps_4 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_4 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_4 / DISPLAY "Item Count"
		style(column)={tagattr='Format:#,##0_);(#,##0)'}
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_4 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};

	DEFINE pd_try_sch_dps_5 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_5 / DISPLAY format=$10. "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_5 / DISPLAY "Item Count"
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_5 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};

	DEFINE pd_try_sch_dps_6 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_6 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_6 / DISPLAY "Item Count"
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_6 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};
RUN;
TITLE ;
TITLE2;
TITLE3;
footnote j=l "^S={font=('Calibri',10pt,Bold)} Items Grand Total: &r2GTN ";
footnote2 j=l "^S={font=('Calibri',10pt,Bold)}Amount Grand Total: &r2GTD";
PROC REPORT DATA=sec3_fin NOWD SPLIT='~' 
	style(header)={background=lightgray font_size=9pt font=(Calibri) font_weight=bold}
	style(column)={font_size=8pt font=(Calibri) };
COLUMN ('Billpay ACH'( 
	pd_try_sch_dps_7
	PF_TRY_SCH_NUM_7 
	pn_tot_try_sch_itm_7 
	PA_TOT_TRY_SCH_AMT_7)) 
('<INTENTIONALLY BLANK>'(
	pd_try_sch_dps_8 
	PF_TRY_SCH_NUM_8 
	pn_tot_try_sch_itm_8 
	PA_TOT_TRY_SCH_AMT_8)) 
('<INTENTIONALLY BLANK>'(
	pd_try_sch_dps_9 
	PF_TRY_SCH_NUM_9 
	pn_tot_try_sch_itm_9 
	PA_TOT_TRY_SCH_AMT_9)) 
;
	DEFINE pd_try_sch_dps_7 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_7 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_7 / DISPLAY "Item Count"
		style(column)={tagattr='Format:#,##0_);(#,##0)'}
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_7 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};

	DEFINE pd_try_sch_dps_8 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_8 / DISPLAY format=$10. "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_8 / DISPLAY "Item Count"
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_8 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};

	DEFINE pd_try_sch_dps_9 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_9 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_9 / DISPLAY "Item Count"
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_9 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};
RUN;
/*R3*/
ODS TAGSETS.EXCELXP STYLE=MINIMAL options(
	embedded_titles='yes' 
	sheet_name='Cash Receipts Report - Pay.Gov'
	width_points='1'
	width_fudge='1'
	absolute_column_width='40,47,50,65,65'
	embedded_footnotes='yes'
	sheet_interval='proc'
	ORIENTATION = 'landscape'
	);
Title 'FedLoan Servicing';
Title2 'Monthly Cash Receipts and Debits Detail Report';
Title3 'Fed Servicer ID: 700502';
footnote j=l "^S={font=('Calibri',10pt,Bold)} Items Grand Total: &r3GTN ";
footnote2 j=l "^S={font=('Calibri',10pt,Bold)}Amount Grand Total: &r3GTD";
PROC REPORT DATA=R3_G_ALL NOWD SPLIT='~' 
	style(header)={background=lightgray font_size=9pt font=(Calibri) font_weight=bold}
	style(column)={font_size=8pt font=(Calibri) };
COLUMN ('Direct Debit'( GRP_DESC_1
	pd_try_sch_dps_1
	PF_TRY_SCH_NUM_1 
	pn_tot_try_sch_itm_1 
	PA_TOT_TRY_SCH_AMT_1)) 
('Web'(GRP_DESC_2 
	pd_try_sch_dps_2 
	PF_TRY_SCH_NUM_2 
	pn_tot_try_sch_itm_2 
	PA_TOT_TRY_SCH_AMT_2)) 
('Tel'(GRP_DESC_3 
	pd_try_sch_dps_3 
	PF_TRY_SCH_NUM_3 
	pn_tot_try_sch_itm_3 
	PA_TOT_TRY_SCH_AMT_3)) 
;
	DEFINE GRP_DESC_1 / display order=internal '~'
		style(header)={background=white};
	DEFINE pd_try_sch_dps_1 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_1 / DISPLAY "Schedule~Number" 
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_1 / DISPLAY "Item Count"
		style(column)={tagattr='Format:#,##0_);(#,##0)'}
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_1 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};

	DEFINE GRP_DESC_2 / display  order=internal '~'
		style(header)={background=white};
	DEFINE pd_try_sch_dps_2 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_2 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_2 / DISPLAY "Item Count"
		style(column)={tagattr='Format:#,##0_);(#,##0)'}
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_2 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};

	DEFINE GRP_DESC_3 / display  order=internal '~'
		style(header)={background=white};
	DEFINE pd_try_sch_dps_3 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_3 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_3 / DISPLAY "Item Count"
		style(column)={tagattr='Format:#,##0_);(#,##0)'}
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_3 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};
RUN;
/*R4*/
ODS TAGSETS.EXCELXP STYLE=MINIMAL options(
	embedded_titles='yes' 
	sheet_name='Cash Receipts Report - IPAC'
	width_points='1'
	width_fudge='1'
	absolute_column_width='40,47,50,65,65'
	embedded_footnotes='yes'
	sheet_interval='proc'
	ORIENTATION = 'landscape'
	);
footnote j=l "^S={font=('Calibri',10pt,Bold)} Items Grand Total: &r4GTN ";
footnote2 j=l "^S={font=('Calibri',10pt,Bold)}Amount Grand Total: &r4GTD";
PROC REPORT DATA=R4_G_ALL NOWD SPLIT='~' 
	style(header)={background=lightgray font_size=9pt font=(Calibri) font_weight=bold}
	style(column)={font_size=8pt font=(Calibri) };
COLUMN ('IPAC Received'( GRP_DESC_1
	pd_try_sch_dps_1
	PF_TRY_SCH_NUM_1 
	pn_tot_try_sch_itm_1 
	PA_TOT_TRY_SCH_AMT_1)) 
('IPAC Sent'(GRP_DESC_2 
	pd_try_sch_dps_2 
	PF_TRY_SCH_NUM_2 
	pn_tot_try_sch_itm_2 
	PA_TOT_TRY_SCH_AMT_2)) 
;
	DEFINE GRP_DESC_1 / display order=internal '~'
		style(header)={background=white};
	DEFINE pd_try_sch_dps_1 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_1 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_1 / DISPLAY "Item Count"
		style(column)={tagattr='Format:#,##0_);(#,##0)'}
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_1 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};

	DEFINE GRP_DESC_2 / display  order=internal '~'
		style(header)={background=white};
	DEFINE pd_try_sch_dps_2 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_2 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_2 / DISPLAY "Item Count"
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_2 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};
	
RUN;
/*R5*/
ODS TAGSETS.EXCELXP STYLE=MINIMAL options(
	embedded_titles='yes' 
	sheet_name='Cash Receipts Report - Other'
	width_points='1'
	width_fudge='1'
	absolute_column_width='40,47,50,65,65'
	embedded_footnotes='yes'
	sheet_interval='proc'
	ORIENTATION = 'landscape'
	);
footnote j=l "^S={font=('Calibri',10pt,Bold)} Items Grand Total: &r5GTN ";
footnote2 j=l "^S={font=('Calibri',10pt,Bold)}Amount Grand Total: &r5GTD";
PROC REPORT DATA=R5_G_ALL NOWD SPLIT='~' 
	style(header)={background=lightgray font_size=9pt font=(Calibri) font_weight=bold}
	style(column)={font_size=8pt font=(Calibri) };
COLUMN ('MEMREFS'( GRP_DESC_1
	pd_try_sch_dps_1
	PF_TRY_SCH_NUM_1 
	pn_tot_try_sch_itm_1 
	PA_TOT_TRY_SCH_AMT_1)) 

('SF 1081 Z-Cancellations'( GRP_DESC_2
	pd_try_sch_dps_2
	PF_TRY_SCH_NUM_2 
	pn_tot_try_sch_itm_2 
	PA_TOT_TRY_SCH_AMT_2))

('Other'( GRP_DESC_3
	pd_try_sch_dps_3
	PF_TRY_SCH_NUM_3 
	pn_tot_try_sch_itm_3 
	PA_TOT_TRY_SCH_AMT_3)) 
;

	DEFINE GRP_DESC_1 / display order=internal '~'
		style(header)={background=white};
	DEFINE pd_try_sch_dps_1 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_1 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_1 / DISPLAY "Item Count"
		style(column)={tagattr='Format:#,##0_);(#,##0)'}
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_1 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};

	DEFINE GRP_DESC_2 / display order=internal '~'
		style(header)={background=white};
	DEFINE pd_try_sch_dps_2 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_2 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_2 / DISPLAY "Item Count"
		style(column)={tagattr='Format:#,##0_);(#,##0)'}
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_2 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};

	DEFINE GRP_DESC_3 / display order=internal '~'
		style(header)={background=white};
	DEFINE pd_try_sch_dps_3 / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"}
		style(header)={background=white};
	DEFINE PF_TRY_SCH_NUM_3 / DISPLAY "Schedule~Number"
		style(column)={tagattr="Format:@"}
		style(header)={background=white};
	DEFINE pn_tot_try_sch_itm_3 / DISPLAY "Item Count"
		style(column)={tagattr='Format:#,##0_);(#,##0)'}
		style(header)={background=white};
	DEFINE PA_TOT_TRY_SCH_AMT_3 / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'}
		style(header)={background=white};
RUN;
/*R6*/
ODS TAGSETS.EXCELXP STYLE=MINIMAL options(
	embedded_titles='yes' 
	sheet_name='Cash Receipts Report - Totals'
	width_points='1'
	width_fudge='1'
	absolute_column_width='40,47,50,65,65'
	embedded_footnotes='yes'
	sheet_interval='proc'
	ORIENTATION = 'landscape'
	);
footnote; 
footnote2; 
PROC REPORT DATA=R6 NOWD SPLIT='~' 
	style(header)={background=lightgray font_size=9pt font=(Calibri) font_weight=bold}
	style(column)={font_size=8pt font=(Calibri) };
COLUMN C1 PD_TRY_SCH_DPS POS_ITM_CT POS_TOT_AMT NEG_ITM_CT NEG_TOT_AMT SV;
	DEFINE C1 / display order=internal '~' ;
	DEFINE PD_TRY_SCH_DPS / DISPLAY "Schedule~Date" FORMAT=MMDDYY10.
		style(column)={tagattr="Format:mm/dd/yyyy"};
	DEFINE POS_ITM_CT / DISPLAY "Item Count"
		style(column)={tagattr='Format:#,##0_);(#,##0)'};
	DEFINE POS_TOT_AMT / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'};
	DEFINE NEG_ITM_CT / DISPLAY "Item Count"
		style(column)={tagattr='Format:#,##0_);(#,##0)'};
	DEFINE NEG_TOT_AMT / DISPLAY "Total Amount" FORMAT=18.2
		style(column)={tagattr='Format:$#,##0.00_);[Red]($#,##0.00)'};
	DEFINE SV / DISPLAY NOPRINT ORDER=INTERNAL;

RUN;
/*DATA DICTIONARY*/
ODS TAGSETS.EXCELXP STYLE=MINIMAL options(
	embedded_titles='yes' 
	absolute_column_width='75,500'
	ORIENTATION = 'landscape'
	sheet_name='Data Dictionary'
	);
title;
title2;
title3;
PROC REPORT DATA=dd NOWD SPLIT='~' 
	style(header)={background=lightgray font_size=9pt font=(Calibri) font_weight=bold}
	style(column)={font_size=8pt font=(Calibri) };
COLUMN sv tab Desc;
	define sv / group noprint;
	DEFINE tab / group order=internal 'Tab' ;
	DEFINE Desc / DISPLAY "Data Definition" ;
RUN;

ODS TAGSETS.EXCELXP CLOSE;
ODS LISTING;
OPTIONS MISSING=.;

/*detail file*/
data _null_;
file 'T:\Windows\Temp\def.csv' delimiter=',' DSD DROPOVER lrecl=32767;
if _n_ = 1 then       
 do;
   put
      "LF_RMT_BCH_USR_INI"
   ','
      "LC_BCH_TRX_SUB_TYP"
   ','
      "PD_TRY_SCH_DPS"
   ','
      "PF_TRY_SCH_NUM"
   ','
      "PN_TOT_TRY_SCH_ITM"
   ','
      "PA_TOT_TRY_SCH_AMT"
   ','
      "LD_RMT_BCH_INI"
   ','
      "LC_BCH_REV_REA"
   ','
      "LC_RMT_BCH_SCH_TYP"
   ','
      "LC_RMT_PAY_SRC"
   ;
 end;
set  WORK.CRRFED   end=EFIEOD;
   format LF_RMT_BCH_USR_INI $8. ;
   format LC_BCH_TRX_SUB_TYP $2. ;
   format PD_TRY_SCH_DPS date9. ;
   format PF_TRY_SCH_NUM $10. ;
   format PN_TOT_TRY_SCH_ITM 5. ;
   format PA_TOT_TRY_SCH_AMT 11.2 ;
   format LD_RMT_BCH_INI date9. ;
   format LC_BCH_REV_REA $1. ;
   format LC_RMT_BCH_SCH_TYP $2. ;
   format LC_RMT_PAY_SRC $2. ;
   format N best12. ;
   format IsEven best12. ;
   format Is_N_Even best12. ;
 do;
   put LF_RMT_BCH_USR_INI $ @;
   put LC_BCH_TRX_SUB_TYP $ @;
   put PD_TRY_SCH_DPS @;
   put PF_TRY_SCH_NUM $ @;
   put PN_TOT_TRY_SCH_ITM @;
   put PA_TOT_TRY_SCH_AMT@;
   put LD_RMT_BCH_INI @;
   put LC_BCH_REV_REA $ @;
   put LC_RMT_BCH_SCH_TYP $ @;
   put LC_RMT_PAY_SRC $ @;
   ;
 end;
run;


