/*QSASQ13 PLUS PREAPPROVAL RESULTS BY SCHOOL*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE PPRBS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.IF_IST
	,A.IC_DOE_LDR_STA
	,A.IC_LDR_SUB_STA
	,A.ID_LDR_SUB_UPD
	,A.IC_LDR_USB_STA
	,A.ID_LDR_USB_UPD
	,A.IC_LDR_PLS_STA
	,A.ID_LDR_PLS_UPD
	,A.IC_LDR_SLS_STA
	,A.ID_LDR_SLS_UPD
	,A.IC_LDR_CON_STA
	,A.ID_LDR_CON_UPD
FROM OLWHRM1.LR01_LGS_LDR_INF A
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA PPRBS;
SET WORKLOCL.PPRBS;
RUN;

PROC SORT DATA=PPRBS;
BY IF_IST;
RUN;

DATA _NULL_;                                      
CURRENT=TODAY();                                  
CALL SYMPUT('CURDATE',PUT(CURRENT,MMDDYY10.));       
RUN; 

/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS PAGENO=1 LS=132 CENTER NODATE;
PROC PRINT NOOBS SPLIT='/' DATA=PPRBS WIDTH=UNIFORM;
FORMAT ID_LDR_SUB_UPD MMDDYY10. ID_LDR_USB_UPD MMDDYY10. ID_LDR_PLS_UPD MMDDYY10.
	   ID_LDR_SLS_UPD MMDDYY10. ID_LDR_CON_UPD MMDDYY10.;
VAR IF_IST
	IC_DOE_LDR_STA
	IC_LDR_SUB_STA
	ID_LDR_SUB_UPD
	IC_LDR_USB_STA
	ID_LDR_USB_UPD
	IC_LDR_PLS_STA
	ID_LDR_PLS_UPD
	IC_LDR_SLS_STA
	ID_LDR_SLS_UPD
	IC_LDR_CON_STA
	ID_LDR_CON_UPD;
LABEL IF_IST = 'LENDER/ID'
	IC_DOE_LDR_STA = 'DOE/LENDER/STATUS/CODE'
	IC_LDR_SUB_STA = 'SUB/GUAR/STATUS/CODE'
	ID_LDR_SUB_UPD = 'DATE/SUB/GUAR/STAT/CODE/UPDTD'
	IC_LDR_USB_STA = 'UNSUB/GUAR/STATUS/CODE'
	ID_LDR_USB_UPD = 'DATE/UNSUB/GUAR/STAT/CODE/UPDTD'
	IC_LDR_PLS_STA = 'PLS/GUAR/STATUS/CODE'
	ID_LDR_PLS_UPD = 'DATE/PLS/GUAR/STAT/CODE/UPDTD'
	IC_LDR_SLS_STA = 'SLS/GUAR/STATUS/CODE'
	ID_LDR_SLS_UPD = 'DATE/SLS/GUAR/STAT/CODE/UPDTD'
	IC_LDR_CON_STA = 'CON/GUAR/STATUS/CODE'
	ID_LDR_CON_UPD = 'DATE/CON/GUAR/STAT/CODE/UPDTD';
TITLE 'PLUS PREAPPROVAL RESULTS BY SCHOOL';
TITLE2 "RUNDATE &CURDATE";
FOOTNOTE 'JOB=QSASQ13          REPORT=QSASQ13.R2';
RUN;


