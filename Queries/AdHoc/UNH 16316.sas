/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS46.NWS46RZ";
FILENAME REPORT2 "&RPTLIB/UNWS46.NWS46R2";

/*LEGEND*/
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PF_REQ_ACT AS ARC,
						COUNT(PF_REQ_ACT) AS COUNT
						
					FROM
						PKUB.AY10_BR_LON_ATY AY10
						INNER JOIN PKUB.LN10_LON LN10
							ON LN10.BF_SSN = AY10.BF_SSN
					WHERE
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'
						AND DAYS(LD_ATY_REQ_RCV) BETWEEN DAYS('07/01/2012') AND DAYS('06/12/2013')
					GROUP BY PF_REQ_ACT

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;


/*export to Excel spreadsheet*/
PROC EXPORT DATA= LEGEND.DEMO 
            OUTFILE= "T:\SAS\NH 16316_Arcs.xls" 
            DBMS=EXCEL REPLACE;
     SHEET="CornerStone"; 
RUN;

/*DUSTER*/
/*%LET RPTLIB = T:\SAS;*/
/*LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;*/
/*RSUBMIT;*/
/*/**/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=DLGSUTWH);*/
/**/
/*	CREATE TABLE DEMO AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT DISTINCT*/
/*						PF_REQ_ACT AS ARC,*/
/*						COUNT(PF_REQ_ACT) AS COUNT*/
/*						*/
/*					FROM*/
/*						OLWHRM1.AY10_BR_LON_ATY AY10*/
/*						INNER JOIN OLWHRM1.LN10_LON LN10*/
/*							ON LN10.BF_SSN = AY10.BF_SSN*/
/*					WHERE*/
/*						LN10.LA_CUR_PRI > 0*/
/*						AND LN10.LC_STA_LON10 = 'R'*/
/*						AND DAYS(LD_ATY_REQ_RCV) BETWEEN DAYS('07/01/2012') AND DAYS('06/12/2013')*/
/*					GROUP BY PF_REQ_ACT*/
/**/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/**/
/*	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/*/
/*	/*%SQLCHECK;*/*/
/*QUIT;*/
/**/
/*ENDRSUBMIT;*/
/**/
/**/
/*PROC EXPORT DATA= DUSTER.DEMO */
/*            OUTFILE= "T:\SAS\NH 16316_Arcs.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="UHEAA"; */
/*RUN;*/
