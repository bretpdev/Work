LIBNAME BSYS ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME CSYS ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME CSYT ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CSYSTEST.dsn; update_lock_typ=nolock; bl_keepnulls=no") ignore_read_only_columns=yes;
/*'the library references below are to production MS Access databases, sometimes SAS puts the databases (especially PJ) in an inconsistent state so you may want to use the Budget Reporting Copies lib refs below them instead'*/
LIBNAME PJ ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\PJ.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME PMD ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\PMD.dsn; update_lock_typ=nolock; bl_keepnulls=no");
/*'these library references are to copies of production MS Acess databases, be sure to update the copies in X:\PADR\Budget Reporting Copies\ before using them*/
/*LIBNAME PJ ODBC %STR(REQUIRED="FILEDSN=X:\PADR\Budget Reporting Copies\PJ.dsn; update_lock_typ=nolock; bl_keepnulls=no");*/
/*LIBNAME PMD ODBC %STR(REQUIRED="FILEDSN=X:\PADR\Budget Reporting Copies\PMD.dsn; update_lock_typ=nolock; bl_keepnulls=no");*/

%LET QTR=FY 2021 Q4; *change to quarter being reported;
%LET TBL = COST_DAT_TimeTracking_Trial_Run; *test run;
/*%LET TBL = COST_DAT_TimeTracking; *final run;*/

*load data to SQL Server;
%MACRO PROCESSOR();
	PROC SQL;
		INSERT INTO 
			CSYT.&TBL(DBSASTYPE=(TaskDate='DATE'))
		SELECT 
			TT.TimeTrackingId,
			TT.Date,
			TT.Hours,
			TT.SR__,
			TT.SASR__,
			TT.LTS__,
			TT.PMD__,
			TT.Project__,
			TT.Generic_Meetings,
			TT.Batch_Scripts,
			TT.FSA_CR__,
			TT.Billing_Script,
			TT.Conversion_Activities,
			TT.Cost_Center,
			TT.Agent,
			TT.CostCenterId,
			TT.SqlUserId
	FROM 
			Temp_Time_Tracking TT
		;
	QUIT;
%MEND PROCESSOR;

*add to processing error report data set;
%MACRO ADDPROCERR(NAME,ERR);
	DATA PROCNO;
		SET PROCNO END=EOF;
		OUTPUT;

		IF EOF THEN 
			DO;		
				AGENT = "&NAME";
				ERROR = "&ERR";
				OUTPUT;
			END;
	RUN;
%MEND ADDPROCERR;

*import data from Excel and create data set for import;
%MACRO PROCIT(FOLDER,NAME,WINNAME);
	*import data from Excel;
	PROC IMPORT
			DATAFILE = "Q:\Support Services\Time tracking\&FOLDER\&QTR Time Tracking - &NAME..xlsx" 
			OUT = Excel_Time_Tracking
			DBMS = EXCEL
			REPLACE
			;
		MIXED = YES;
		SHEET = 'Sheet1$'N;
	QUIT;

	DATA Excel_Time_Tracking;
	SET Excel_Time_Tracking;
	FORMAT 	Hours 18.3
			Generic_Meetings $500.
			Batch_Scripts $50.
			FSA_CR__ $50.
			Billing_Script $1.
			Conversion_Activities $1.
			Cost_Center $50.
			Agent $50.
		;
	RUN;

	*add calculated fields and remove unneeded records;
	PROC SQL;
		CREATE TABLE Temp_Time_Tracking AS
			SELECT 
				. AS TimeTrackingId,
				TT.Date,
				TT.Hours,
				TT.SR__,
				TT.SASR__,
				TT.LTS__,
				TT.PMD__,
				TT.Project__,
				TT.Generic_Meetings,
				TT.Batch_Scripts,
				TT.FSA_CR__,
				TT.Billing_Script,
				TT.Conversion_Activities,
				TT.Cost_Center,
				&WINNAME AS Agent,
				CC.CostCenterId,
				U.SqlUserId
			FROM 
				Excel_Time_Tracking TT
				INNER JOIN CSYS.SYSA_DAT_Users U /*uses live user data*/
					ON U.WindowsUserName = &WINNAME
					AND U.Status = 'Active'
				LEFT JOIN CSYS.COST_DAT_CostCenters CC /*at the point this is run, the data in test is the most up to date*/
					ON TT.Cost_Center = CC.CostCenter
			WHERE 
				TT.Hours IS NOT NULL		/*excludes blank lines*/
				AND TT.Date NE '01JAN2013'D /*excludes primer data*/
				AND NOT 					/*excludes line with no allocable data*/
					(
						TT.SR__ IS NULL 
						AND TT.SASR__ IS NULL
						AND TT.LTS__ IS NULL
						AND TT.PMD__ IS NULL
						AND TT.Project__ IS NULL
						AND TT.Generic_Meetings IS NULL
						AND	TT.FSA_CR__ IS NULL
						AND TT.Billing_Script IS NULL
						AND TT.Conversion_Activities IS NULL
						AND TT.Cost_Center IS NULL
				)
			ORDER BY
				TT.Date
		;
	QUIT;

	%IF &SYSERR %THEN
		%DO;
			%PUT %QUOTE(>>>ERROR: RC=&syserr Temp_Time_Tracking for &Name could NOT be created.);
			%ADDPROCERR(&NAME,Temp_Time_Tracking not Created);
		%END;
	%ELSE
		%PROCESSOR();
%MEND PROCIT;

*ACTIVE AGENTS: be sure CSYS.SYSA_DAT_Users reference (line 106) Status = 'Active' is uncommented ;
*%PROCIT(Bret,Bret Pehrson,'bpehrson');
*%PROCIT(Candice,Candice,'ccole');
*%PROCIT(Conor,Conor MacDonald,'cmacdonald');
*%PROCIT(David,David Halladay,'dhalladay');
%PROCIT(Deb,Deb,'dphillips');
*%PROCIT(Evan,Daniel Evan Walker,'ewalker');
*%PROCIT(Jacob Kramer,Jacob Kramer,'jkramer');
*%PROCIT(Jared Kieschnick,Jared Kieschnick,'jkieschnick');
*%PROCIT(Jarom,Jarom Ryan,'jryan');
*%PROCIT(Jeremy,Jeremy Blair,'jblair');
*%PROCIT(Jesse,Jesse,'jgutierrez');
*%PROCIT(Jessica,Jessica Hanson,'jhanson');
*%PROCIT(Josh,Josh,'jlwright');
*%PROCIT(Karleann Westerman,Karleann Westerman,'kwesterman');
*%PROCIT(Melanie,Melanie Garfield,'mgarfield');
*%PROCIT(Riley,Riley,'rbigelow');
*%PROCIT(Savanna,Savanna Gregory,'sgregory');
*%PROCIT(Steve,Steve Ostler,'sostler');
*%PROCIT(Wendy,Wendy Hack,'whack');
*INACTIVE AGENTS contain partial quarter data that still needs to be accounted for;
*NOTE: must comment out CSYS.SYSA_DAT_Users reference (line 106) Status = 'Active' ;
*NOTE: some inactive agents might have more than one SQL User ID, so check first and use the most recent;

/*ON FINAL RUN: double-check how many rows added to table*/
/*ODS HTML;*/
/*TITLE "Records Inserted";*/
/*PROC SQL NUMBER;*/
/*	SELECT*/
/*		Agent,*/
/*		Trial_Count,*/
/*		Final_Count,*/
/*		CASE*/
/*			WHEN Trial_Count ^= Final_Count*/
/*			THEN 'MISMATCH!'*/
/*			ELSE ''*/
/*		END AS Error_Flag*/
/*	FROM*/
/*		(*/
/*			SELECT DISTINCT */
/*				TRIAL.Agent*/
/*				,COUNT(*) AS Trial_Count*/
/*				,FINAL.Final_Count*/
/*			FROM */
/*				CSYT.COST_DAT_TimeTracking_Trial_Run TRIAL*/
/*				LEFT JOIN*/
/*				(*/
/*					SELECT DISTINCT */
/*						Agent*/
/*						,COUNT(*) AS Final_Count*/
/*					FROM */
/*		/*				CSYT.COST_DAT_TimeTracking_Trial_Run*/*/
/*						CSYT.COST_DAT_TimeTracking*/
/*					WHERE*/
/*						TaskDate >= MDY(10,1,2020)/*beginning of quarter*/*/
/*					GROUP BY */
/*						AGENT*/
/*				) FINAL*/
/*					ON TRIAL.Agent = FINAL.Agent*/
/*			GROUP BY */
/*				TRIAL.Agent*/
/*		) AS COMPS*/
/*	ORDER BY*/
/*		Agent;*/
/*QUIT;*/

/*print error report*/
ODS _ALL_ CLOSE;
ODS HTML PATH = "T:\SAS\" (URL = NONE) FILE = "Time Tracking Errors - Time Tracking Not Loaded.html" STYLE=Money;

TITLE "Time Tracking not Loaded";

PROC PRINT DATA=PROCNO LABEL ;
	VAR Agent Error;
	WHERE Agent NE '';
RUN;

ODS HTML CLOSE;
