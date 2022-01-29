/*CREATE LIBNAME FOR REMOTE SERVER'S WORK FOLDER*/
/*TODO: delete the unneeded line*/
LIBNAME WORKLOCL REMOTE SERVER=DUSTER SLIBREF=WORK;
LIBNAME WORKLOCL REMOTE SERVER=LEGEND SLIBREF=WORK;

/*CREATE FILENAME FOR OUTPUT*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT1 "&RPTLIB/CNH_34146.XLSX"; /*TODO: update the outfile name*/

/*CREATE LIBRARY OF EXCEL DATA*/
LIBNAME XLIB XLSX 'T:\CNH 34146\NSLDS Reporting DCR.xlsx'; /*TODO: update the infile name*/

/*CREATE DATA SET FROM A SPECIFIC SHEET IN THE EXCEL FILE*/
DATA XLDATA; SET XLIB.SHEET1; RUN; /*TODO: update the sheet name*/

/*CAST CHAR SEQ ATTRIBUTE TO NUMERIC ATTRIBUTE WITH THE SAME NAME AND COPY NEW DATA SET TO REMOTE SERVER*/
/*TODO: update attributes names or delete this block if unneeded*/
DATA XDATA (DROP = CHAR_SEQ);
	RETAIN BF_SSN WN_SEQ_GR10; /*only the attribute that was cast and any attributes to the left of it are needed*/
	SET XLDATA (RENAME = (WN_SEQ_GR10 = CHAR_SEQ));
	WN_SEQ_GR10 = INPUT(CHAR_SEQ, 2.);
RUN;
DATA WORKLOCL.XDATA; SET XDATA; RUN;

/*COPY EXCEL DATA TO REMOTE SERVER*/
/*TODO: delete this block if the CAST block above was used*/
DATA WORKLOCL.XDATA; SET XLDATA; RUN;

/*SUBMIT CODE TO REMOTE SERVER*/
/*TODO: delete the unneeded block*/
RSUBMIT DUSTER;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%let DB = DLGSUTWH;
%let OWNR = OLWHRM1;

RSUBMIT LEGEND;
LIBNAME PKUB DB2 DATABASE=DNFPUTDL OWNER=PKUB;
LIBNAME AES DB2 DATABASE=DNFPUTDL OWNER=AES;
%let DB = DNFPUTDL;
%let OWNR = PKUB;

PROC SQL STIMER;
	CONNECT TO DB2 (DATABASE=&DB); 

/*TODO: update SQL below*/
/*	GET DATA FOR ALL LOANS*/
	CREATE TABLE JOINDATA AS
		SELECT 
			XDAT.*
			,DB2D.WC_NDS_LN_DSB_RPT

		FROM 
			CONNECTION TO DB2 
			(
				SELECT DISTINCT 
					GR10.BF_SSN 
					,GR10.WN_SEQ_GR10
					,GR10.WC_NDS_LN_DSB_RPT
				FROM 
					&OWNR..GR10_RPT_LON_APL GR10
		
				FOR READ ONLY WITH UR
			) DB2D
			INNER JOIN XDATA XDAT
				ON DB2D.BF_SSN = XDAT.BF_SSN
				AND DB2D.WN_SEQ_GR10 = XDAT.WN_SEQ_GR10
	;
	DISCONNECT FROM DB2;


QUIT;

ENDRSUBMIT;

/*COPY JOINED DATA TO WORK*/
DATA JOINDATA; SET WORKLOCL.JOINDATA; RUN;

/*EXPORT JOINED DATA TO A REPORT*/
PROC EXPORT DATA= WORK.JOINDATA
	OUTFILE= REPORT1 
	REPLACE
	DBMS=XLSX;
RUN;