LIBNAME BSYS ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME CSYS ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
/*'the library references below are to production MS Access databases, sometimes SAS puts the databases (especially PJ) in an inconsistent state so you may want to use the Budget Reporting Copies lib refs below them instead'*/
LIBNAME PJ ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\PJ.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME PMD ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\PMD.dsn; update_lock_typ=nolock; bl_keepnulls=no");
/*'these library references are to copies of production MS Acess databases, be sure to update the copies in X:\PADR\Budget Reporting Copies\ before using them*/
/*LIBNAME PJ ODBC %STR(REQUIRED="FILEDSN=X:\PADR\Budget Reporting Copies\PJ.dsn; update_lock_typ=nolock; bl_keepnulls=no");*/
/*LIBNAME PMD ODBC %STR(REQUIRED="FILEDSN=X:\PADR\Budget Reporting Copies\PMD.dsn; update_lock_typ=nolock; bl_keepnulls=no");*/

/*CHANGE THESE EVERY QUARTER*/
%LET QTR=FY 2021 Q4; *set variable for file name to quarter being reported;
%LET BEG_DATE = '01APR2021'D;
%LET END_DATE = '30APR2021'D;
/***************************/

DATA ALLDATES;
    DO
        QUARTER_RANGE = &BEG_DATE TO &END_DATE;
        DAY_OF_WEEK=WEEKDAY(QUARTER_RANGE);
        OUTPUT;
    END;
    FORMAT QUARTER_RANGE DATE9.;
RUN;

DATA ALLCLEAR;
    DO
        QUARTER_BEGIN = &BEG_DATE;
        QUARTER_END = &END_DATE;
        OUTPUT;
    END;
    FORMAT QUARTER_BEGIN QUARTER_END DATE9.;
RUN;

DATA PROCNO;
    LENGTH AGENT $25 ERROR $50;
RUN;

/*check for errors and create error report*/
%MACRO PROCESSOR(FOLDER,NAME,UHEAA_Email);
/*  add row number to be able to reference original data in Excel*/
    DATA Temp_Time_Tracking;
        SET Temp_Time_Tracking;
        Row = _N_+1;
    RUN;

    PROC SQL;

    /*duplicate entries*/
    CREATE TABLE Duplicate_Entries AS
        SELECT
            TT1.Row
            ,TT2.Date           AS Date
            ,'All'              AS Column_Name
            ,'Duplicate rows'   AS Invalid_Value
          FROM
            Temp_Time_Tracking TT1
            INNER JOIN Temp_Time_Tracking TT2
                ON TT1.Date                             = TT2.Date
                AND COALESCE(TT1.Hours,0)               = COALESCE(TT2.Hours,0)
                AND COALESCE(TT1.Sr__,0)                = COALESCE(TT2.Sr__,0)
                AND COALESCE(TT1.Sasr__,0)              = COALESCE(TT2.Sasr__,0)
                AND COALESCE(TT1.Lts__,0)               = COALESCE(TT2.Lts__,0)
                AND COALESCE(TT1.Pmd__,0)               = COALESCE(TT2.Pmd__,0)
                AND COALESCE(TT1.Project__,0)           = COALESCE(TT2.Project__,0)
                AND COALESCEC(TT1.Generic_Meetings,'0') = COALESCEC(TT2.Generic_Meetings,'0')
                AND COALESCEC(TT1.Batch_Scripts,'0')    = COALESCEC(TT2.Batch_Scripts,'0')
                AND COALESCEC(TT1.Fsa_Cr__,'0')         = COALESCEC(TT2.Fsa_Cr__,'0')
                AND COALESCEC(TT1.Billing_Script,'0')   = COALESCEC(TT2.Billing_Script,'0')
                AND COALESCEC(TT1.Conversion_Activities,'0') = COALESCEC(TT2.Conversion_Activities,'0')
                AND COALESCEC(TT1.Cost_Center,'0')      = COALESCEC(TT2.Cost_Center,'0')
                AND COALESCEC(TT1.Agent,'0')            = COALESCEC(TT2.Agent,'0')
                AND TT1.Row < TT2.Row
                AND TT1.Date ^= .
                AND TT1.Date ^= '01JAN2013'D
                AND TT2.Date ^= .
                AND TT2.Date ^= '01JAN2013'D
				AND TT1.Agent ^= 'ewalker'
				AND TT2.Agent ^= 'ewalker'

        UNION ALL

        SELECT
            TT2.Row
            ,TT1.Date           AS Date
            ,'All'              AS Column_Name
            ,'Duplicate rows'   AS Invalid_Value
          FROM
            Temp_Time_Tracking TT1
            INNER JOIN Temp_Time_Tracking TT2
                ON TT1.Date                             = TT2.Date
                AND COALESCE(TT1.Hours,0)               = COALESCE(TT2.Hours,0)
                AND COALESCE(TT1.Sr__,0)                = COALESCE(TT2.Sr__,0)
                AND COALESCE(TT1.Sasr__,0)              = COALESCE(TT2.Sasr__,0)
                AND COALESCE(TT1.Lts__,0)               = COALESCE(TT2.Lts__,0)
                AND COALESCE(TT1.Pmd__,0)               = COALESCE(TT2.Pmd__,0)
                AND COALESCE(TT1.Project__,0)           = COALESCE(TT2.Project__,0)
                AND COALESCEC(TT1.Generic_Meetings,'0') = COALESCEC(TT2.Generic_Meetings,'0')
                AND COALESCEC(TT1.Batch_Scripts,'0')    = COALESCEC(TT2.Batch_Scripts,'0')
                AND COALESCEC(TT1.Fsa_Cr__,'0')         = COALESCEC(TT2.Fsa_Cr__,'0')
                AND COALESCEC(TT1.Billing_Script,'0')   = COALESCEC(TT2.Billing_Script,'0')
                AND COALESCEC(TT1.Conversion_Activities,'0') = COALESCEC(TT2.Conversion_Activities,'0')
                AND COALESCEC(TT1.Cost_Center,'0')      = COALESCEC(TT2.Cost_Center,'0')
                AND COALESCEC(TT1.Agent,'0')            = COALESCEC(TT2.Agent,'0')
                AND TT1.Row < TT2.Row
                AND TT1.Date ^= .
                AND TT1.Date ^= '01JAN2013'D
                AND TT2.Date ^= .
                AND TT2.Date ^= '01JAN2013'D
				AND TT1.Agent ^= 'ewalker'
				AND TT2.Agent ^= 'ewalker'
    ;

/*  dates outside of quarter date range*/
    CREATE TABLE Outside_Range_Dates AS
        SELECT
            TT.Row,
            TT.Date,
            'Date' AS Column_Name,
            'Entry date out of quarter range' AS Invalid_Value
        FROM
            TEMP_TIME_TRACKING TT
        WHERE
            TT.Date NOT BETWEEN &BEG_DATE AND &END_DATE
            AND TT.Date ^= .
            AND TT.Date ^= '01JAN2013'D;
    ;

    /*  missing dates*/
        CREATE TABLE Missing_Entry_Dates AS
            SELECT
                TT.Row,
                TT.Date,
                'Date' AS Column_Name,
                'Blank entry date' AS Invalid_Value
            FROM
                TEMP_TIME_TRACKING TT
            WHERE
                TT.Date IS NULL
                AND
                    (
                        SR__ IS NOT NULL
                        OR SASR__ IS NOT NULL
                        OR LTS__ IS NOT NULL
                        OR PMD__ IS NOT NULL
                        OR Project__ IS NOT NULL
                        OR Generic_Meetings NE ''
                        OR FSA_CR__ NE ''
                        OR Billing_Script NE ''
                        OR Conversion_Activities NE ''
                        OR Cost_Center NE ''
                    )
        ;

    /*  missing hours*/
        CREATE TABLE Missing_Hours AS
            SELECT
                TT.Row,
                TT.Date,
                'Hours' AS Column_Name,
                'Blank hours' AS Invalid_Value
            FROM
                TEMP_TIME_TRACKING TT
            WHERE
                TT.Hours IS NULL
                AND
                    (
                        SR__ IS NOT NULL
                        OR SASR__ IS NOT NULL
                        OR LTS__ IS NOT NULL
                        OR PMD__ IS NOT NULL
                        OR Project__ IS NOT NULL
                        OR Generic_Meetings NE ''
                        OR FSA_CR__ NE ''
                        OR Billing_Script NE ''
                        OR Conversion_Activities NE ''
                        OR Cost_Center NE ''
                    )
        ;

    /*  invalid SRs*/
        CREATE TABLE Scr AS
            SELECT
                TT.Row,
                TT.Date,
                'SR #' AS Column_Name,
                LEFT(PUT(TT.Sr__,8.)) AS Invalid_Value
            FROM
                TEMP_TIME_TRACKING TT
                LEFT JOIN BSYS.SCKR_DAT_ScriptRequests SR
                    ON TT.Sr__ = SR.Request
            WHERE
                TT.Sr__ IS NOT NULL
                AND SR.Request IS NULL
        ;

    /*  invalid SASRs*/
        CREATE TABLE Sas AS
            SELECT
                TT.Row,
                TT.Date,
                'SASR #' AS Column_Name,
                LEFT(PUT(TT.SASR__,8.)) AS Invalid_Value
            FROM
                Temp_Time_Tracking TT
                LEFT JOIN BSYS.SCKR_DAT_SASRequests SASR
                    ON TT.SASR__ = SASR.Request
            WHERE
                TT.SASR__ IS NOT NULL
                AND SASR.Request IS NULL
        ;

    /*  invalid LRs*/
        CREATE TABLE Lts AS
            SELECT
                TT.Row,
                TT.Date,
                'LTS #' AS Column_Name,
                LEFT(PUT(TT.LTS__,8.)) AS Invalid_Value
            FROM
                Temp_Time_Tracking TT
                LEFT JOIN BSYS.LTDB_DAT_Requests LR
                    ON TT.LTS__ = LR.Request
            WHERE
                TT.LTS__ IS NOT NULL
                AND LR.Request IS NULL
        ;

    /*  invalid PPRs*/
        CREATE TABLE Pmd AS
            SELECT
                TT.Row,
                TT.Date,
                'PMD #' AS Column_Name,
                LEFT(PUT(TT.PMD__,8.)) AS Invalid_Value
            FROM
                Temp_Time_Tracking TT
                LEFT JOIN PMD.datRequests PPR
                    ON TT.PMD__ = PPR.Request
            WHERE
                TT.PMD__ IS NOT NULL
                AND PPR.Request IS NULL
        ;

    /*  invalid project numbers*/
        CREATE TABLE Pj AS
            SELECT
                TT.Row,
                TT.Date,
                'Project #' AS Column_Name,
                LEFT(PUT(TT.Project__,8.)) AS Invalid_Value
            FROM
                Temp_Time_Tracking TT
                LEFT JOIN PJ.datProjects PJ
                    ON TT.Project__ = PJ.pNo
            WHERE
                TT.Project__ IS NOT NULL
                AND PJ.pNo IS NULL
        ;

    /*  project not 100 percent allocated to cost centers*/
        CREATE TABLE PjAllocation AS
            SELECT
                TT.Row,
                TT.Date,
                'Project not 100% allocated to cost centers' AS Column_Name,
                LEFT(PUT(TT.Project__,8.)) AS Invalid_Value
            FROM
                Temp_Time_Tracking TT
                INNER JOIN PJ.datProjects PJ
                    ON TT.Project__ = PJ.pNo
                LEFT JOIN
                    (
                        SELECT
                            pNo,
                            SUM(cPercentAllocated) AS PercentAllocated
                        FROM
                            PJ.refCostCenters
                        GROUP BY
                            pNo
                    ) PA
                    ON PJ.pNo = PA.pNo
            WHERE
                TT.Project__ IS NOT NULL
                AND COALESCE(PA.PercentAllocated,0) < 100
                AND PJ.pNo ^= 1
        ;

    /*  invalid billing value*/
        CREATE TABLE Billing AS
            SELECT
                TT.Row,
                TT.Date,
                'Billing Script' AS Column_Name,
                LEFT(PUT(TT.Billing_Script,$1.)) AS Invalid_Value
            FROM
                Temp_Time_Tracking TT
            WHERE
                TT.Billing_Script NOT IN ('C','U','')
        ;

    /*  invalid conversion activities value */
        CREATE TABLE ConversionActivities AS
            SELECT
                TT.Row,
                TT.Date,
                'Conversion Activities' AS Column_Name,
                LEFT(PUT(TT.Conversion_Activities,$1.)) AS Invalid_Value
            FROM
                Temp_Time_Tracking TT
            WHERE
                TT.Conversion_Activities NOT IN ('C','U','')
        ;

    /*  invalid cost center*/
        CREATE TABLE CostCenters AS
            SELECT
                TT.Row,
                TT.Date,
                'Cost Center' AS Column_Name,
                LEFT(PUT(TT.Cost_Center,$255.)) AS Invalid_Value
            FROM
                Temp_Time_Tracking TT
                LEFT JOIN CSYS.COST_DAT_CostCenters CC
                    ON TT.Cost_Center = CC.CostCenter
            WHERE
                TT.Cost_Center IS NOT NULL
                AND CC.CostCenter IS NULL
        ;

	/*  Need Help time tracked in Timesheet*/
        CREATE TABLE NeedHelp AS
			SELECT
				Row,
				Date,
				'Generic Meetings' AS Column_Name,
				'NeedHelp time tracked in Timesheet' AS Invalid_Value
			FROM
				Temp_Time_Tracking
			WHERE
			  UPCASE(Generic_Meetings) LIKE '%NEED HELP%'
			  OR UPCASE(Generic_Meetings) LIKE '%NEEDHELP%'
			  OR UPCASE(Generic_Meetings) LIKE '%UNH%' 
			  OR UPCASE(Generic_Meetings) LIKE '%CNH%'
			  OR UPCASE(Generic_Meetings) LIKE '%NH TI%'
		;

	/*  TILP cost center not correctly assigned*/
		CREATE TABLE Tilp AS
			SELECT
				Row,
				Date,
				'Generic Meetings' AS Column_Name,
				'TILP cost center not selected' AS Invalid_Value
			FROM
				Temp_Time_Tracking
			WHERE
				UPCASE(Generic_Meetings) LIKE '%TILP%'
				AND (
						COALESCE(Cost_Center,'') = ''
						OR Cost_Center ^= 'TILP'
					)
		;

		/* homework entries*/
			CREATE TABLE Homework AS
				SELECT
					Row,
					Date,
					'Generic Meetings' AS Column_Name,
					CASE
						WHEN UPCASE(TRIM(Generic_Meetings)) = 'HOMEWORK'
						THEN Generic_Meetings
						WHEN UPCASE(TRIM(Generic_Meetings)) LIKE '%HOMEWORK%'
						THEN CATX(' ', 'notify Debbie about this:', Generic_Meetings)
						ELSE ''
					END AS Invalid_Value
				FROM
					Temp_Time_Tracking
				WHERE
					UPCASE(TRIM(Generic_Meetings)) = 'HOMEWORK'
					OR UPCASE(TRIM(Generic_Meetings)) LIKE '%HOMEWORK%'
		;
	QUIT;

/*  add to processing error report data set if there was an error*/
    %IF &SYSERR %THEN
        %DO;
            %PUT %QUOTE(>>>ERROR: RC=&syserr Proc SQL error FOR &NAME.);
            %ADDPROCERR(&NAME,Data not Processed);
        %END;
/*  print error report for the agent*/
    %ELSE
        %DO;
/*          combine individual errors data sets into one data set*/
            DATA ERRORS;
                LENGTH Column_Name $255 Invalid_Value $255;
                FORMAT Date MMDDYY10. Invalid_Value $255.;
                SET
                    Duplicate_Entries
                    Missing_Entry_Dates
                    Missing_Hours
                    Outside_Range_Dates
                    Scr
                    Sas
                    Lts
                    Pmd
                    Pj
                    PjAllocation
                    Billing
                    ConversionActivities
                    CostCenters
					NeedHelp
					Tilp
					Homework
                ;
            RUN;

            PROC SORT DATA=ERRORS;
                BY Row;
            RUN;

/*          get number of observations in ERRORS data set*/
            DATA _NULL_;
                IF 0 THEN SET ERRORS NOBS=N;
                CALL SYMPUTX('ERRS',N);
                STOP;
            RUN;

            %IF &ERRS > 0 %THEN
                %DO;
/*                  print error report if ERRORS data set has any observations*/
					ODS _ALL_ CLOSE;
					ODS HTML PATH = "T:\SAS" (URL = NONE) FILE = "Time Tracking Errors - &NAME..html" STYLE=Ocean;
                    TITLE "Time Tracking Errors - &NAME";
                    PROC PRINT DATA=ERRORS LABEL ;
                        VAR Row Date Column_Name Invalid_Value;
                        LABEL
                            Row = 'Row'
                            Date = 'Date'
                            Column_Name = 'Column Name'
                            Invalid_Value = 'Invalid Value'
                        ;
                    RUN;
                    ODS HTML CLOSE;

/*                  emails error report to agents*/
                    FILENAME mail
                    EMAIL TO="&UHEAA_Email"
                    SUBJECT="Time Tracking Errors"
                    CONTENT_TYPE="text";
                    ODS LISTING CLOSE;
                    ODS LISTING BODY=mail;
                    OPTIONS NODATE NONUMBER NOCENTER;

                    TITLE1 'Please fix the following issue(s) in your Time Tracking spreadsheet:';
                    TITLE4 "TIME TRACKING ISSUES - &NAME";
                    PROC PRINT DATA=ERRORS NOOBS LABEL DOUBLE;
                        VAR
                            Row
                            Date
                            Column_Name
                            Invalid_Value
                        ;
                        LABEL
                            Row = 'Row '
                            Date = 'Entry Date'
                            Column_Name = 'Column'
                            Invalid_Value = 'Invalid Value'
                        ;
                        FORMAT
                            Invalid_Value $65.
                        ;
                    RUN;

                    ODS LISTING CLOSE;
                %END;
            %ELSE
                %DO;
                    FILENAME MyFile "T:\SAS\Time Tracking Errors - &NAME..html";
                    DATA _NULL_;
                        RC = FDELETE("MyFile");
                    RUN;
                    FILENAME MyFile CLEAR;
					ODS _ALL_ CLOSE;
					ODS HTML PATH = "T:\SAS" (URL = NONE) FILE = "No Time Tracking Errors - &NAME..html" STYLE=Ocean;
                    TITLE "No Time Tracking Errors for &NAME";
                    PROC PRINT DATA=ALLCLEAR NOOBS;
                    RUN;
                    ODS HTML CLOSE;
                %END;
        %END;
%MEND PROCESSOR;

/*add to processing error report data set*/
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

/*import data from Excel and check for errors*/
%MACRO PROCIT(FOLDER,NAME,UHEAA_Email);

    PROC IMPORT
            DATAFILE = "Q:\Support Services\Time tracking\&FOLDER\&QTR Time Tracking - &NAME..xlsx"
            OUT = Temp_Time_Tracking
            DBMS = EXCEL
            REPLACE
            ;
        MIXED = YES;
        SHEET = 'Sheet1$'N;
    QUIT;

    %IF &SYSERR %THEN
        %DO;
            %PUT %QUOTE(>>>ERROR: RC=&syserr Temp_Time_Tracking for &NAME could NOT be created.);
            %ADDPROCERR(&NAME,Temp_Time_Tracking not Created);
        %END;
    %ELSE
        %PROCESSOR(&FOLDER,&NAME,&UHEAA_Email);
%MEND PROCIT;

/*%PROCIT(Bret,Bret Pehrson,bpehrson@utahsbr.edu);*/
/*%PROCIT(Candice,Candice,ccole@utahsbr.edu);*/
/*%PROCIT(Conor,Conor MacDonald,cmacdonald@utahsbr.edu);*/
/*%PROCIT(David,David Halladay,dhalladay@utahsbr.edu);*/
/*%PROCIT(Deb,Deb,dphillips@utahsbr.edu);*/
/*%PROCIT(Evan,Daniel Evan Walker,ewalker@utahsbr.edu);*/
/*%PROCIT(Jacob Kramer,Jacob Kramer,jkramer@utahsbr.edu);*/
/*%PROCIT(Jared Kieschnick,Jared Kieschnick,jkieschnick@utahsbr.edu);*/
/*%PROCIT(Jarom,Jarom Ryan,jryan@utahsbr.edu);*/
/*%PROCIT(Jeremy,Jeremy Blair,jblair@utahsbr.edu);*/
/*%PROCIT(Jesse,Jesse,jgutierrez@utahsbr.edu);*/
/*%PROCIT(Jessica,Jessica Hanson,jhanson@utahsbr.edu);*/
%PROCIT(Josh,Josh,jlwright@utahsbr.edu);
/*%PROCIT(Karleann Westerman,Karleann Westerman,kwesterman@utahsbr.edu);*/
/*%PROCIT(Melanie,Melanie Garfield,mgarfield@utahsbr.edu);*/
/*%PROCIT(Riley,Riley,rbigelow@utahsbr.edu);*/
/*%PROCIT(Savanna,Savanna Gregory,sgregory@utahsbr.edu);*/
/*%PROCIT(Steve,Steve Ostler,sostler@utahsbr.edu);*/
/*%PROCIT(Wendy,Wendy Hack,whack@utahsbr.edu);*/

/*print error report*/
ODS _ALL_ CLOSE;
ODS HTML PATH = "T:\SAS\" (URL = NONE) FILE = "Time Tracking Errors - Time Tracking Not Processed.html" STYLE=Money;
TITLE "Time Tracking Not Processed";

PROC PRINT DATA=PROCNO LABEL ;
    VAR Agent Error;
    WHERE Agent NE '';
RUN;
ODS _ALL_ CLOSE;

/*ODS HTML;*/
/*TITLE "Timesheet Entries Per Month";*/
/*PROC SQL;*/
/*SELECT DISTINCT*/
/*	MONTH(DATE) AS Quarter_Month,*/
/*	MAX(DATE) AS Max_Entry_Date FORMAT MMDDYY10.*/
/*FROM */
/*	Temp_Time_Tracking*/
/*WHERE*/
/*	DATE ^= .*/
/*	AND DATE ^= MDY(1,1,2013)*/
/*GROUP BY*/
/*	MONTH(DATE)*/
/*;QUIT;*/
/**/
/*ODS HTML CLOSE;*/
