LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=OLWHRM1;

RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
ENDRSUBMIT;

PROC EXPORT	DATA=DUSTER.LP01_CAP_OPT_RUL OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP02_DFR_PAR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP03_RDI OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP04_UPD_SQR_ORD OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP05_FOR_PAR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP06_ITR_AND_TYP OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP08_PAY_APL_PAR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP09_OWN_DSB_PAR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP10_RPY_PAR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP11_WUP_RFD_PAR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP12_GTR_DSB_PAR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP13_DD_ACT_STA OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP14_DD_ACT_WDO OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP15_DD_ACT_STE OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP17_STP_PUR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP18_DD_CYC_PAR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP19_DD_SKP_PAR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP20_GTR_OWN_FEE OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP21_TIR_SCL_FEE OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP23_TIR_SCL_ITR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP51_CAP_RUL_ERR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP56_HST_LPD06 OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP60_RPY_PAR_ERR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP68_DD_CYC_ERR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
PROC EXPORT	DATA=DUSTER.LP70_GTR_FEE_ERR OUTFILE='T:\SAS\LP TABLES.XLSX' REPLACE; RUN;
