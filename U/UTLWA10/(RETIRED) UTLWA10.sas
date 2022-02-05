*-------------------------------------------*
| UTLWA10 - OneLINK Refund Not Processed QC |
*-------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWA10.LWA10R2";
FILENAME REPORT3 "&RPTLIB/ULWA10.LWA10R3";
FILENAME REPORTZ "&RPTLIB/ULWA10.LWA10RZ";
LIBNAME  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
/*%MACRO SQLCHECK (SQLRPT= );*/
/*%IF &SQLXRC NE 0 %THEN %DO;*/
/*	DATA _NULL_;*/
/*    FILE REPORTZ NOTITLES;*/
/*    PUT @01 " ********************************************************************* "*/
/*      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "*/
/*      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       */
/*      / @01 " ********************************************************************* "*/
/*      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "*/
/*      / @01 " ****  &SQLXMSG   **** "*/
/*      / @01 " ********************************************************************* ";*/
/*	RUN;*/
/*%END;*/
/*%MEND;*/
PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);
	CREATE TABLE FD01 AS
		SELECT *
		FROM 
			CONNECTION TO DB2 (
				SELECT DISTINCT
					PD01.DF_PRS_ID 
					,PD01.DF_SPE_ACC_ID
					,PD01.DM_PRS_LST
					,FD01.LC_DSB_STA
					,FD01.LD_STA_UPD
					,FD01.LA_DSB_AMT_PSF
					,Q1.IRAC
					,Q2.IREX
				FROM 
					OLWHRM1.FD01_QUE_TAB FD01
					INNER JOIN OLWHRM1.PD01_PDM_INF PD01
						ON FD01.LF_PRS_ID = PD01.DF_PRS_ID
					LEFT OUTER JOIN (
						SELECT DISTINCT 
							DF_PRS_ID_BR
							,'X' AS IRAC
						FROM 
							OLWHRM1.CT30_CALL_QUE 
						WHERE 
							IF_WRK_GRP = 'RACTRFNDQ'
						) Q1
						ON PD01.DF_PRS_ID = Q1.DF_PRS_ID_BR
					LEFT OUTER JOIN (
						SELECT DISTINCT 
							DF_PRS_ID_BR
							,'X' AS IREX
						FROM 
							OLWHRM1.CT30_CALL_QUE 
						WHERE 
							IF_WRK_GRP = 'REXTRFND'
						) Q2
						ON PD01.DF_PRS_ID = Q2.DF_PRS_ID_BR
				WHERE 
					FD01.LC_DSB_STA IN ('A','E')
				FOR READ ONLY WITH UR
			);
	DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWA10.LWA10RZ);*/
/*QUIT;*/

ENDRSUBMIT;

DATA FD01; SET WORKLOCL.FD01; RUN;

DATA FD01;
	SET FD01;
	IF LC_DSB_STA = 'A' AND	LD_STA_UPD < TODAY()-30 AND IRAC = '' 
		THEN DO;
			TARGETID = DF_PRS_ID;
			QUEUENAME = 'RACTRFND';
			INSTITUTIONID = '';
			INSTITUTIONTYPE = '';
			DATEDUE = '';
			TIMEDUE = '';
			COMMENTS = 'EXTRACT BORROWER REFUND';
			RFILE = 'R2';
		END;
	ELSE IF LC_DSB_STA = 'E' AND LD_STA_UPD < TODAY()-21 AND IREX = ''
		THEN DO;
				TARGETID = DF_PRS_ID;
				QUEUENAME = 'REXTRFND';
				INSTITUTIONID = '';
				INSTITUTIONTYPE = '';
				DATEDUE = '';
				TIMEDUE = '';
				COMMENTS = 'EXTRACT ACTIVE REFUND';
				RFILE = 'R3';
		END;
RUN;

%MACRO WRTFL(F_IND,RNUM);
	DATA tFD01;
	SET FD01;
	WHERE RFILE = "&F_IND";
RUN;

PROC SORT DATA=tFD01 NODUPKEY; BY TARGETID;RUN;

DATA _NULL_;
	SET  WORK.tFD01;
	FILE REPORT&RNUM DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT TARGETID $9. ;
	FORMAT QUEUENAME $8. ;
	FORMAT INSTITUTIONID $1. ;
	FORMAT INSTITUTIONTYPE $1. ;
	FORMAT DATEDUE $1. ;
	FORMAT TIMEDUE $1. ;
	FORMAT COMMENTS $23. ;
	DO;
	   PUT TARGETID $ @;
	   PUT QUEUENAME $ @;
	   PUT INSTITUTIONID $ @;
	   PUT INSTITUTIONTYPE $ @;
	   PUT DATEDUE $ @;
	   PUT TIMEDUE $ @;
	   PUT COMMENTS $ ;
	END;
RUN;
%MEND WRTFL;

%WRTFL(R2,2);
%WRTFL(R3,3);
