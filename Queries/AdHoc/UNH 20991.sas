LIBNAME CSYL ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");

/*set variables for date range to include*/
DATA _NULL_;
	*Sunday;
	IF WEEKDAY(TODAY()) = 1 THEN 
		DO;
			BEGIN = TODAY() - 6;
			END = TODAY() - 2;
		END;
	*Monday;
	ELSE IF WEEKDAY(TODAY()) = 2 THEN 
		DO;
			BEGIN = TODAY() - 7;
			END = TODAY() - 3;
		END;
	*Tuesday;
	ELSE IF WEEKDAY(TODAY()) = 3 THEN 
		DO;
			BEGIN = TODAY() - 8;
			END = TODAY() - 4;
		END;
	*Wednesday;
	ELSE IF WEEKDAY(TODAY()) = 4 THEN 
		DO;
			BEGIN = TODAY() - 2;
			END = TODAY() + 2;
		END;
	*Thursday;
	ELSE IF WEEKDAY(TODAY()) = 5 THEN
		DO;
			BEGIN = TODAY() - 3;
			END = TODAY() + 1;
		END;
	*Friday;
	ELSE IF WEEKDAY(TODAY()) = 6 THEN 
		DO;
			BEGIN = TODAY() - 4;
			END = TODAY();
		END;
	*Saturday;
	ELSE IF WEEKDAY(TODAY()) = 7 THEN 
		DO;
			BEGIN = TODAY() - 5;
			END = TODAY() - 1;
		END;

	CALL SYMPUT('BEGIN',BEGIN);
	CALL SYMPUT('END',END);
	CALL SYMPUT('BEGINT',PUT(BEGIN,MMDDYY10.));
	CALL SYMPUT('ENDT',PUT(END,MMDDYY10.));
RUN;

%PUT &BEGINT;
%PUT &ENDT;

/*create primer data set*/
DATA Time_Tracking_;
	FORMAT SR $8.;
	FORMAT SASR $8.;
	FORMAT LTS $8.;
	FORMAT PMD $8.;
	FORMAT Project $8.;
	FORMAT Agent $30.;

	Date = .;
	Hours = .;
	SR = '';
	SASR = '';
	LTS = '';
	PMD = '';
	Project = '';
	Agent = '';
RUN;

/*create primer data set*/
DATA PROCERR;
	FORMAT Agent $20.;
	FORMAT ERROR $200.;
	AGENT = '';
	ERROR = '';
RUN;

%MACRO GETQTR(DATE_TO_USE);
	/*set variables for file name*/
	DATA _NULL_;
		IF MONTH(&DATE_TO_USE) IN (1,2,3) THEN
			DO;	
				YR = YEAR(&DATE_TO_USE);
				QTR = 3;
			END;
		ELSE IF MONTH(&DATE_TO_USE) IN (4,5,6) THEN
			DO;	
				YR = YEAR(&DATE_TO_USE);
				QTR = 4;
			END;
		ELSE IF MONTH(&DATE_TO_USE) IN (7,8,9) THEN
			DO;	
				YR = YEAR(&DATE_TO_USE) + 1;
				QTR = 1;
			END;
		ELSE IF MONTH(&DATE_TO_USE) IN (10,11,12) THEN
			DO;	
				YR = YEAR(&DATE_TO_USE) + 1;
				QTR = 2;
			END;

		CALL SYMPUT('YR',PUT(YR,4.));
		CALL SYMPUT('QTR',PUT(QTR,1.));
	RUN;

	%PUT YEAR: &YR;
	%PUT QUARTER: &QTR;
%MEND;


/*add to processing error report data set*/
%MACRO ADDPROCERR(NAME,ERR);
	DATA PROCERR;
		SET PROCERR END=EOF;
		OUTPUT;

		IF EOF THEN 
			DO;		
				AGENT = "&NAME";
				ERROR = "&ERR";
				OUTPUT;
			END;
	RUN;
%MEND;

/*import data from Excel and create data set for import*/
%MACRO PROCIT(FOLDER,NAME,WINNAME);
/*delete results from previous run in case there is an error loading the next spreadsheet*/
	PROC SQL;
		DELETE * FROM Excel_Time_Tracking;
	QUIT;

/*	import data from Excel*/
	PROC IMPORT
			DATAFILE = "Q:\Support Services\Time tracking\&FOLDER\FY &YR Q&QTR Time Tracking - &NAME..xlsx"
			OUT = Excel_Time_Tracking
			DBMS = EXCEL
			REPLACE
			;
		MIXED = YES;
		SHEET = 'Sheet1$'N;
	QUIT;

	%IF &SYSERR %THEN
		%DO;
			%PUT %QUOTE(>>>ERROR: RC=&syserr Temp_Time_Tracking for &Name could NOT be created.);
			%ADDPROCERR(&NAME,error processing file Q:\Support Services\Time tracking\&FOLDER\FY &YR Q&QTR Time Tracking - &NAME..xlsx);
		%END;
	%ELSE
		%DO;
		/*	add calculated fields and remove unneeded records*/
			PROC SQL;
				CREATE TABLE Temp_Time_Tracking AS
					SELECT 
						TT.Date,
						TT.Hours,
						PUT(TT.SR__,8.) AS SR,
						PUT(TT.SASR__,8.) AS SASR,
						PUT(TT.LTS__,8.) AS LTS,
						PUT(TT.PMD__,8.) AS PMD,
						PUT(TT.Project__,8.) AS Project,
						&WINNAME AS Agent
					FROM 
						Excel_Time_Tracking TT
					WHERE 
						TT.Hours IS NOT NULL					/*excludes blank lines*/
						AND TT.Date NE '01JAN2013'D 			/*excludes primer data*/
						AND NOT 								/*excludes line with no allocable data*/
							(
								TT.SR__ IS NULL 
								AND TT.SASR__ IS NULL
								AND TT.LTS__ IS NULL
								AND TT.PMD__ IS NULL
								AND TT.Project__ IS NULL
						)
						AND TT.Date BETWEEN &BEGIN AND &END
					ORDER BY
						TT.Date
				;
			QUIT;

			DATA Temp_Time_Tracking;
				SET Temp_Time_Tracking;
				IF SR = . THEN SR = '';
				IF SASR = . THEN SASR = '';
				IF LTS = . THEN LTS = '';
				IF PMD = . THEN PMD = '';
				IF Project = . THEN Project = '';
			RUN;

			/*add imported data to master data set*/
			DATA Time_Tracking_;
				FORMAT Agent $30.;
				SET Time_Tracking_ Temp_Time_Tracking;
			RUN;
		%END;
%MEND;


%MACRO GETDATA();
	%PROCIT(Alisia,Alisia Wixom,'awixom');
	%PROCIT(Bret,Bret Pehrson,'bpehrson');
	%PROCIT(Chris,Christopher Black,'cblack');
	%PROCIT(Colton,Colton,'cmccomb');
	%PROCIT(Deb,Deb,'dphillips');
	%PROCIT(Eric,Eric Barnes,'ebarnes');
	%PROCIT(Evan,Daniel Evan Walker,'ewalker');
	%PROCIT(Jarom,Jarom Ryan,'jryan');
	%PROCIT(Jay,Jay Davis,'jdavis');
	%PROCIT(Jeremy,Jeremy Blair,'jblair');
	%PROCIT(Jesse,Jesse,'jgutierrez');
	%PROCIT(Kathryn,Kathryn,'kferre');
	%PROCIT(Melanie,Melanie Garfield,'mgarfield');
	%PROCIT(Parish,Parish Snyder,'psnyder');
	%PROCIT(Riley,Riley,'rbigelow');
	%PROCIT(Scott,Scott Briggs,'sbriggs');
	%PROCIT(Teri,Teri,'tvig');
	%PROCIT(Wendy,Wendy Hack,'whack');
	%PROCIT(Josh,Josh,'jswright');
	%PROCIT(Devin,Devin Pili,'dpili');
	%PROCIT(Jessica,Jessica Hanson,'jhanson');
	%PROCIT(Steve,Steve Ostler,'sostler');
%MEND;

/*run the process to read in the data from the Excel spreadsheets*/
DATA _NULL_;
/*	run the process for the quarter in which the begin date lies*/	
	CALL EXECUTE('%GETQTR(&BEGIN)'); 
	CALL EXECUTE('%GETDATA()');

/*	run the process again for the quarter in which the end date lies if the begin and end dates are in different quarters*/
	IF QTR(&BEGIN) NE QTR(&END) THEN 
		DO;
			PUT 'NOTE: QUARTER CROSSOVER DATES DETECTED';
			CALL EXECUTE('%GETQTR(&END)');
			CALL EXECUTE('%GETDATA()');
		END;
RUN;

/*calculate totals*/
PROC SQL;
	CREATE TABLE Time_Tracking AS
		SELECT
			CATX(' ',CU.FirstName,CU.LastName) AS Agent,
			TT.SR,
			TT.SASR,
			TT.LTS,
			TT.PMD,
			TT.Project,
			TT.Date,
			TT.Hours
		FROM
			Time_Tracking_ TT
			JOIN CSYL.SYSA_DAT_Users CU
				ON TT.Agent = CU.WindowsUserName
		WHERE
			TT.Agent NE ''
	;

/*calculate totals for tasks (requests)*/
	CREATE TABLE Tasks AS
		SELECT
			Agent,
			SUM(Hours) AS Hours,
			SR,
			SASR,
			LTS,
			PMD,
			Project
		FROM
			Time_Tracking
		WHERE
			Hours IS NOT NULL
			AND NOT 								
				(
					SR = ''
					AND SASR = ''
					AND LTS = ''
					AND PMD = ''
					AND Project = ''
				)
		GROUP BY
			Agent,
			SR,
			SASR,
			LTS,
			PMD,
			Project
	;


	CREATE TABLE Totals_Agent_Hours AS
		SELECT
			Agent,
			SUM(Hours) AS Hours
		FROM
			Time_Tracking
		WHERE
			Hours IS NOT NULL
		GROUP BY
			Agent
	;

	CREATE TABLE Totals_SR AS
		SELECT
			Agent,
			PUT(SUM(Hours),8.2) AS SR
		FROM
			Time_Tracking
		WHERE
			SR IS NOT NULL
		GROUP BY
			Agent
	;

	CREATE TABLE Totals_SASR AS
		SELECT
			Agent,
			PUT(SUM(Hours),8.2) AS SASR
		FROM
			Time_Tracking
		WHERE
			SASR IS NOT NULL
		GROUP BY
			Agent
	;

	CREATE TABLE Totals_LTS AS
		SELECT
			Agent,
			PUT(SUM(Hours),8.2) AS LTS
		FROM
			Time_Tracking
		WHERE
			LTS IS NOT NULL
		GROUP BY
			Agent
	;

	CREATE TABLE Totals_PMD AS
		SELECT
			Agent,
			PUT(SUM(Hours),8.2) AS PMD
		FROM
			Time_Tracking
		WHERE
			PMD IS NOT NULL
		GROUP BY
			Agent
	;

	CREATE TABLE Totals_PJ AS
		SELECT
			Agent,
			PUT(SUM(Hours),8.2) AS Project
		FROM
			Time_Tracking
		WHERE
			Project IS NOT NULL
		GROUP BY
			Agent
	;

	CREATE TABLE Totals_All AS
		SELECT
			CATX(' - ',A.Agent,'Total Hours') AS Agent,
			A.Hours,
			SR.SR,
			SASR.SASR,
			LTS.LTS,
			PMD.PMD,
			Project
		FROM
			Totals_Agent_Hours A
			LEFT JOIN Totals_SR SR
				ON A.Agent = SR.Agent
			LEFT JOIN Totals_SASR SASR
				ON A.Agent = SASR.Agent
			LEFT JOIN Totals_LTS LTS
				ON A.Agent = LTS.Agent
			LEFT JOIN Totals_PMD PMD
				ON A.Agent = PMD.Agent
			LEFT JOIN Totals_PJ PJ
				ON A.Agent = PJ.Agent
		ORDER BY
			Agent
	;		
QUIT;

PROC SQL;
	CREATE TABLE Totals_by_Task AS
		SELECT
			CATX(' ','SR',SR) AS TASK,
			SUM(HOURS) AS HOURS
		FROM
			Tasks
		WHERE
			SR NE ''
		GROUP BY
			SR

		UNION ALL

		SELECT
			CATX(' ','SASR',SASR) AS TASK,
			SUM(HOURS) AS HOURS
		FROM
			Tasks
		WHERE
			SASR NE ''
		GROUP BY
			SASR

		UNION ALL

		SELECT
			CATX(' ','LTS',LTS) AS TASK,
			SUM(HOURS) AS HOURS
		FROM
			Tasks
		WHERE
			LTS NE ''
		GROUP BY
			LTS

		UNION ALL

		SELECT
			CATX(' ','PMD',PMD) AS TASK,
			SUM(HOURS) AS HOURS
		FROM
			Tasks
		WHERE
			PMD NE ''
		GROUP BY
			PMD

		UNION ALL

		SELECT
			CATX(' ','Project',Project) AS TASK,
			SUM(HOURS) AS HOURS
		FROM
			Tasks
		WHERE
			Project NE ''
		GROUP BY
			Project
	;
QUIT;
	
DATA Tasks_with_Totals;
	SET Tasks Totals_All;
RUN;

PROC SORT DATA = Tasks_with_Totals;
	BY Agent;
RUN;


/*print report*/	
ODS HTML FILE="T:\SAS\Project Time Tracking Report.html" STYLE=HTMLBlue;

TITLE "Project Time Tracking";
TITLE2 "&BEGINT - &ENDT";
	
PROC PRINT NOOBS SPLIT='/' DATA=Tasks_with_Totals WIDTH=UNIFORM WIDTH=MIN LABEL;
	VAR 
		Agent
		Hours
		SR
		SASR
		LTS
		PMD
		Project;
	BY
		Agent;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=Totals_by_Task WIDTH=UNIFORM WIDTH=MIN LABEL;
	VAR 
		TASK
		HOURS;
	SUM
		HOURS;
RUN;

ODS HTML CLOSE;

/*print error report - the error report is not being generated because SAS isn't seeing the &SYSERR macro variable, it must have something to do with being buried in several layers of macro calls, it wasn't requested anyway*/
/*ODS HTML FILE="T:\SAS\Project Time Tracking Report Errors.html" STYLE=Money;*/
/**/
/*TITLE "Project Time Tracking not Loaded";*/
/**/
/*PROC PRINT DATA=PROCERR LABEL ;*/
/*	VAR Agent Error;*/
/*	WHERE Agent NE '';*/
/*RUN;*/
/**/
/*ODS HTML CLOSE;*/
