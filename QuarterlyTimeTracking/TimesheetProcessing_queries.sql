--calculate dynamic fiscal year quarter begin & end dates
DECLARE @MONTH TINYINT = MONTH(GETDATE()),
		@YEAR SMALLINT = YEAR(GETDATE());
DECLARE @FY_YEAR VARCHAR(5) =
			(CASE
				WHEN @MONTH BETWEEN 7 AND 12 THEN @YEAR+1
				WHEN @MONTH BETWEEN 1 AND 6	THEN @YEAR
				ELSE 'ERROR'
			END),
		@QUARTER VARCHAR(5) =
			(CASE
				WHEN @MONTH BETWEEN 7 AND 9	THEN ' Q1'
				WHEN @MONTH BETWEEN 10 AND 12 THEN ' Q2'
				WHEN @MONTH BETWEEN 1 AND 3	THEN ' Q3'
				WHEN @MONTH BETWEEN 4 AND 6	THEN ' Q4'
				ELSE 'ERROR'
			END),
		@QUARTER_BEGIN VARCHAR(10) = 
			(CASE
				WHEN @MONTH BETWEEN 7 AND 9	THEN CONCAT(@YEAR,'-07-01') --Q1 begin
				WHEN @MONTH BETWEEN 10 AND 12 THEN CONCAT(@YEAR,'-10-01') --Q2 begin
				WHEN @MONTH BETWEEN 1 AND 3	THEN CONCAT(@YEAR,'-01-01') --Q3 begin
				WHEN @MONTH BETWEEN 4 AND 6	THEN CONCAT(@YEAR,'-04-01') --Q4 begin
				ELSE 'ERROR'
			END),
		@QUARTER_END VARCHAR(10) = 
			(CASE
				WHEN @MONTH BETWEEN 7 AND 9	THEN CONCAT(@YEAR,'-09-30') --September always has 30 days
				WHEN @MONTH BETWEEN 10 AND 12 THEN CONCAT(@YEAR,'-12-31') --December always has 31 days
				WHEN @MONTH BETWEEN 1 AND 3	THEN CONCAT(@YEAR,'-03-31') --March always has 31 days
				WHEN @MONTH BETWEEN 4 AND 6	THEN CONCAT(@YEAR,'-06-30') --June always has 30 days
				ELSE 'ERROR'
			END);
--select @MONTH,@YEAR,@FY_YEAR,@QUARTER,@QUARTER_BEGIN,@QUARTER_END;

--CTE's identify staff that do not have a timesheet or timesheet is not properly named
;WITH CURRENT_STAFF AS
	(--gets current staff
		SELECT 
			 SqlUserId
			,WindowsUserName
			,FirstName
			,LastName
			,CASE
				WHEN WindowsUserName IN 
				(
					'ccole',
					'cmccomb',
					'jgutierrez',
					'rbigelow'
				)
				THEN FirstName
				WHEN WindowsUserName = 'dphillips'
				THEN 'Deb'
				WHEN WindowsUserName = 'ewalker'
				THEN CONCAT('Daniel ', FirstName, ' ', LastName)
				WHEN WindowsUserName = 'jlwright'
				THEN 'Josh'
				WHEN WindowsUserName = 'nburnham'
				THEN CONCAT('Nick ', LastName)
				WHEN WindowsUserName = 'sostler'
				THEN CONCAT('Steve ', LastName)
				ELSE FirstName + ' ' + LastName 
			END AS TIMESHEET_NAME
			,Email
			,[Status]
			,BusinessUnit
		FROM
			--CSYS.._SYSA_DAT_Users --OPSDEV
			CSYS..SYSA_DAT_Users --NOCHOUSE
		WHERE
			[Status] = 'Active'
			AND BusinessUnit IN 
			(
				 30 --application development
				,34 --strategic projects
				,35 --systems support
				,49 --operations
			)
			AND SqlUserId NOT IN
			(
				 83--batch scripts
				,1119 --Brenda Cox
				,4347 --Ben Gerona
			)
	)
,TIMESHEET_DATA AS
	(--get timesheet names
		SELECT DISTINCT 
			 SOURCEFILE
			,(CHARINDEX('-', SOURCEFILE) + 2) AS STARTVALUE --gets value 2 over from hyphen (+2 accounts for blank space and hyphen)
			,CHARINDEX('.', SOURCEFILE) AS STOPVALUE --gets value at period
			,SUBSTRING (SOURCEFILE,
				(CHARINDEX('-', SOURCEFILE) + 2), --STARTVALUE
				(--take difference between STOPVALUE and STARTVALUE to get LENGTH
					CHARINDEX('.', SOURCEFILE) --STOPVALUE
					- (CHARINDEX('-', SOURCEFILE) + 2) --STARTVALUE
				)--LENGTH
			) AS TIMESHEET_NAME
		FROM 
			CSYS..COST_DAT_TimesheetProcessing
	)
,MATCHUP AS
	(--match current users to timesheet
		SELECT
			 TD.SourceFile
			,CS.SqlUserId
			,CS.WindowsUserName
			,CS.FirstName
			,CS.LastName
			,CS.Email
			,CS.[Status]
			,CS.BusinessUnit
			,TD.TIMESHEET_NAME
			,CASE
				WHEN TD.TIMESHEET_NAME LIKE '%(Autosaved)'
				THEN LEFT(TD.TIMESHEET_NAME, CHARINDEX('(',TD.TIMESHEET_NAME)-2) --the -2 accounts for the ( and a space
				ELSE NULL
			END AS TIMESHEET_NAME_ALT
		FROM
			CURRENT_STAFF CS
			FULL OUTER JOIN TIMESHEET_DATA TD
				ON CS.TIMESHEET_NAME = TD.TIMESHEET_NAME
	)
,FLAGGING AS
	(--identify timesheet issues
		SELECT
			 MU.SourceFile
			,MU.SqlUserId
			,MU.WindowsUserName
			,CONCAT(MU.FirstName, ' ', MU.LastName) AS [Name]
			,MU.Email
			,MU.[Status]
			,MU.BusinessUnit
			,MU.TIMESHEET_NAME
			,MU2.TIMESHEET_NAME AS TIMESHEET_NAME_2
			,CASE
				WHEN MU.TIMESHEET_NAME = 'Template'
				THEN 'Ignore'
				WHEN MU.TIMESHEET_NAME IS NULL
				THEN CONCAT('Timesheet not found for FY ', @FY_YEAR, @QUARTER, '. Create timesheet or update name.')
				WHEN MU2.TIMESHEET_NAME IS NOT NULL
				THEN 'Multiple timesheets for same quarter'
				WHEN MU.SqlUserId IS NULL
				THEN 'User not on file'
				ELSE 'OK!'
			END AS TASK_FLAG
		FROM
			MATCHUP MU
			LEFT JOIN
			(
				SELECT
					 TIMESHEET_NAME
					,TIMESHEET_NAME_ALT
				FROM
					MATCHUP 
				WHERE
					TIMESHEET_NAME_ALT IS NOT NULL
			) MU2
				ON MU.TIMESHEET_NAME = MU2.TIMESHEET_NAME_ALT
	)
SELECT
	ROW_NUMBER() OVER(PARTITION BY ALLERRORS.[Name]
		ORDER BY 
			 ALLERRORS.SourceFile
			,ALLERRORS.RowNumber
			,ALLERRORS.TaskDate
			,ALLERRORS.ColumnName
			,ALLERRORS.InvalidValue
		) AS ERRORS
	,ALLERRORS.SourceFile
	,ALLERRORS.RowNumber
	,ALLERRORS.TaskDate
	,ALLERRORS.ColumnName
	,ALLERRORS.InvalidValue
	,ALLERRORS.EMail --for shared data set, keep only this field
	,ALLERRORS.[Name]
FROM
	(	--spreadsheet problems (no timesheet, invalid name, multiple timesheets)
		SELECT DISTINCT
			 NULL AS SourceFile
			,NULL AS RowNumber
			,NULL AS TaskDate
			,NULL AS ColumnName
			,FLAGGING.TASK_FLAG AS InvalidValue
			,EMail
			,[Name]
		FROM
			FLAGGING
		WHERE
			TASK_FLAG NOT IN ('OK!','Ignore','User not on file')
		
		UNION ALL

		/**** data validation queries begin here  ****/
		SELECT
			 VALIDATIONERRORS.SourceFile
			,VALIDATIONERRORS.RowNumber
			,VALIDATIONERRORS.TaskDate
			,VALIDATIONERRORS.ColumnName
			,VALIDATIONERRORS.InvalidValue
			,FLAGGING.EMail
			,FLAGGING.[Name]
		FROM
			(	--duplicate entries
				SELECT DISTINCT
					 SourceFile
					,RowNumber
					,TaskDate
					,NULL AS ColumnName
					,'Duplicate rows - please consolidate or resolve' AS InvalidValue
				FROM
					(	
						SELECT
							 TP1.SourceFile
							,TP1.RowNumber --gets first row of dupes
							,TP2.TaskDate
						FROM
							CSYS..COST_DAT_TimesheetProcessing TP1
							INNER JOIN CSYS..COST_DAT_TimesheetProcessing TP2
								ON TP1.SourceFile = TP2.SourceFile
								AND TP1.TaskDate = TP2.TaskDate
								AND COALESCE(TP1.[Hours],0)	= COALESCE(TP2.[Hours],0)
								AND COALESCE(TP1.Sr,0) = COALESCE(TP2.Sr,0)
								AND COALESCE(TP1.Sasr,0) = COALESCE(TP2.Sasr,0)
								AND COALESCE(TP1.Lts,0)	= COALESCE(TP2.Lts,0)
								AND COALESCE(TP1.Pmd,0)	= COALESCE(TP2.Pmd,0)
								AND COALESCE(TP1.Project,0)	= COALESCE(TP2.Project,0)
								AND COALESCE(TP1.GenericMeetings,'0') = COALESCE(TP2.GenericMeetings,'0')
								AND COALESCE(TP1.BatchScripts,'0') = COALESCE(TP2.BatchScripts,'0')
								AND COALESCE(TP1.FsaCr,'0')	= COALESCE(TP2.FsaCr,'0')
								AND COALESCE(TP1.BillingScript,'0')	= COALESCE(TP2.BillingScript,'0')
								AND COALESCE(TP1.ConversionActivities,'0') = COALESCE(TP2.ConversionActivities,'0')
								AND COALESCE(TP1.CostCenter,'0') = COALESCE(TP2.CostCenter,'0')
								AND COALESCE(TP1.Agent,'0')	= COALESCE(TP2.Agent,'0')
								AND TP1.RowNumber < TP2.RowNumber
								AND TP1.TaskDate <> '2013-01-01'
								AND TP2.TaskDate <> '2013-01-01'
						WHERE
							TP1.SourceFile NOT LIKE '%Daniel Evan Walker%'
							AND TP2.SourceFile NOT LIKE '%Daniel Evan Walker%'

						UNION ALL

						SELECT
							 TP1.SourceFile
							,TP2.RowNumber --gets send row of dupes
							,TP1.TaskDate
						FROM
							CSYS..COST_DAT_TimesheetProcessing TP1
							INNER JOIN CSYS..COST_DAT_TimesheetProcessing TP2
								ON TP1.SourceFile = TP2.SourceFile
								AND TP1.TaskDate = TP2.TaskDate
								AND COALESCE(TP1.[Hours],0)	= COALESCE(TP2.[Hours],0)
								AND COALESCE(TP1.Sr,0) = COALESCE(TP2.Sr,0)
								AND COALESCE(TP1.Sasr,0) = COALESCE(TP2.Sasr,0)
								AND COALESCE(TP1.Lts,0)	= COALESCE(TP2.Lts,0)
								AND COALESCE(TP1.Pmd,0)	= COALESCE(TP2.Pmd,0)
								AND COALESCE(TP1.Project,0)	= COALESCE(TP2.Project,0)
								AND COALESCE(TP1.GenericMeetings,'0') = COALESCE(TP2.GenericMeetings,'0')
								AND COALESCE(TP1.BatchScripts,'0') = COALESCE(TP2.BatchScripts,'0')
								AND COALESCE(TP1.FsaCr,'0')	= COALESCE(TP2.FsaCr,'0')
								AND COALESCE(TP1.BillingScript,'0')	= COALESCE(TP2.BillingScript,'0')
								AND COALESCE(TP1.ConversionActivities,'0') = COALESCE(TP2.ConversionActivities,'0')
								AND COALESCE(TP1.CostCenter,'0') = COALESCE(TP2.CostCenter,'0')
								AND COALESCE(TP1.Agent,'0')	= COALESCE(TP2.Agent,'0')
								AND TP1.RowNumber < TP2.RowNumber
								AND TP1.TaskDate <> '2013-01-01'
								AND TP2.TaskDate <> '2013-01-01'
						WHERE
							TP1.SourceFile NOT LIKE '%Daniel Evan Walker%'
							AND TP2.SourceFile NOT LIKE '%Daniel Evan Walker%'
					) ALLDUPES

				UNION ALL

				--dates outside of quarter date range
				SELECT
					 SourceFile
					,RowNumber
					,TaskDate
					,'Date' AS ColumnName
					,'Date out of quarter range' AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing
				WHERE
					TaskDate NOT BETWEEN CAST(@QUARTER_BEGIN AS DATE) AND CAST(@QUARTER_END AS DATE)
					AND TaskDate <> '2013-01-01'
					AND SUBSTRING(SourceFile, 47, 7) = CONCAT(@FY_YEAR, @QUARTER) --matches timesheet to current quarter

				UNION ALL

				--missing dates
				SELECT
					 SourceFile
					,RowNumber
					,TaskDate
					,'Date' AS ColumnName
					,'Blank entry date' AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing
				WHERE
					TaskDate IS NULL
					AND	(
							[Hours] IS NOT NULL
							OR SR IS NOT NULL
							OR SASR IS NOT NULL
							OR LTS IS NOT NULL
							OR PMD IS NOT NULL
							OR Project IS NOT NULL
							OR GenericMeetings <> ''
							OR FsaCr <> ''
							OR BillingScript <> ''
							OR ConversionActivities <> ''
							OR CostCenter <> ''
						)

				UNION ALL

				--missing hours
				SELECT
					 SourceFile
					,RowNumber
					,TaskDate
					,'Hours' AS ColumnName
					,'Blank hours' AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing
				WHERE
					[Hours] IS NULL
					AND	(
							TaskDate IS NOT NULL
							OR SR IS NOT NULL
							OR SASR IS NOT NULL
							OR LTS IS NOT NULL
							OR PMD IS NOT NULL
							OR Project IS NOT NULL
							OR GenericMeetings <> ''
							OR FsaCr <> ''
							OR BillingScript <> ''
							OR ConversionActivities <> ''
							OR CostCenter <> ''
						)

				UNION ALL

				--invalid Script Requests
				SELECT
					 TP.SourceFile
					,TP.RowNumber
					,TP.TaskDate
					,'SR #' AS ColumnName
					,CAST(TP.Sr AS VARCHAR(9)) AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing TP
					LEFT JOIN BSYS..SCKR_DAT_ScriptRequests SR
						ON TP.Sr = SR.Request
				WHERE
					TP.Sr IS NOT NULL
					AND SR.Request IS NULL

				UNION ALL

				--invalid SAS Requests
				SELECT
					 TP.SourceFile
					,TP.RowNumber
					,TP.TaskDate
					,'SASR #' AS ColumnName
					,CAST(TP.SASR AS VARCHAR(9)) AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing TP
					LEFT JOIN BSYS..SCKR_DAT_SASRequests SASR
						ON TP.SASR = SASR.Request
				WHERE
					TP.SASR IS NOT NULL
					AND SASR.Request IS NULL

				UNION ALL

				--invalid Letter Requests
				SELECT
					 TP.SourceFile
					,TP.RowNumber
					,TP.TaskDate
					,'LTS #' AS ColumnName --LTS: Letter Tracking System
					,CAST(TP.LTS AS VARCHAR(9)) AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing TP
					LEFT JOIN BSYS..LTDB_DAT_Requests LR
						ON TP.LTS = LR.Request
				WHERE
					TP.LTS IS NOT NULL
					AND LR.Request IS NULL

				UNION ALL

				--invalid billing value
				SELECT
					 TP.SourceFile
					,TP.RowNumber
					,TP.TaskDate
					,'Billing Script' AS ColumnName
					,TP.BillingScript AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing TP
				WHERE
					TP.BillingScript NOT IN ('C','U','')

				UNION ALL

				--invalid conversion activities value
				SELECT
					 TP.SourceFile
					,TP.RowNumber
					,TP.TaskDate
					,'Conversion Activities' AS ColumnName
					,TP.ConversionActivities AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing TP
				WHERE
					TP.ConversionActivities NOT IN ('C','U','')

				UNION ALL

				--invalid cost center
				SELECT
					 TP.SourceFile
					,TP.RowNumber
					,TP.TaskDate
					,'Cost Center' AS ColumnName
					,TP.CostCenter AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing TP
					LEFT JOIN CSYS..COST_DAT_CostCenters CC  
						ON TP.CostCenter = CC.CostCenter
				WHERE
					TP.CostCenter IS NOT NULL
					AND TP.CostCenter != ''
					AND CC.CostCenter IS NULL

				UNION ALL

				--invalid Procedure Revision Request
				SELECT
					 TP.SourceFile
					,TP.RowNumber
					,TP.TaskDate
					,'PMD #' AS ColumnName --PMD: Procedures Management Database
					,CAST(TP.PMD AS VARCHAR(9)) AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing TP
					LEFT JOIN CSYS.._PMD_datRequests PRR --PRR: Procedure Revision Request
						ON TP.PMD = PRR.Request
				WHERE
					TP.PMD IS NOT NULL
					AND PRR.Request IS NULL
		
				UNION ALL

				--invalid project numbers
				SELECT
					 TP.SourceFile
					,TP.RowNumber
					,TP.TaskDate
					,'Project #' AS ColumnName
					,CAST(TP.Project AS VARCHAR(9)) AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing TP
					LEFT JOIN CSYS.._PJ_datProjects PJ --Projects Database datProjects table
						ON TP.Project = PJ.pNo
				WHERE
					TP.Project IS NOT NULL
					AND PJ.pNo IS NULL

				UNION ALL

				--project not 100 percent allocated to cost centers
				SELECT
					 TP.SourceFile
					,TP.RowNumber
					,TP.TaskDate
					,'Project not 100% allocated to cost centers' AS ColumnName
					,CAST(TP.Project AS VARCHAR(9)) AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing TP
					INNER JOIN CSYS.._PJ_datProjects PJ --Projects Database datProjects table
						ON TP.Project = PJ.pNo
					LEFT JOIN
					(
						SELECT
							pNo,
							SUM(cPercentAllocated) AS PercentAllocated
						FROM
							CSYS.._PJ_refCostCenters --Projects Database refCostCenters table
						GROUP BY
							pNo
					) PA
						ON PJ.pNo = PA.pNo
				WHERE
					TP.Project IS NOT NULL
					AND COALESCE(PA.PercentAllocated,0) < 100
					AND PJ.pNo <> 1

				UNION ALL

				--Need Help time tracked in Timesheet
				SELECT
					 SourceFile
					,RowNumber
					,TaskDate
					,'Generic Meetings' AS ColumnName
					,'NeedHelp time tracked in Timesheet' AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing
				WHERE
				  UPPER(GenericMeetings) LIKE '%NEED HELP%'
				  OR UPPER(GenericMeetings) LIKE '%NEEDHELP%'
				  OR UPPER(GenericMeetings) LIKE '%UNH%' 
				  OR UPPER(GenericMeetings) LIKE '%CNH%'
				  OR UPPER(GenericMeetings) LIKE '%NH TI%'

				UNION ALL

				--TILP cost center not correctly assigned
				SELECT
					 SourceFile
					,RowNumber
					,TaskDate
					,'Generic Meetings' AS ColumnName
					,'TILP cost center not selected' AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing
				WHERE
					UPPER(GenericMeetings) LIKE '%TILP%'
					AND (
							ISNULL(CostCenter,'') = ''
							OR CostCenter != 'TILP'
						)

				UNION ALL

				--homework entries
				SELECT 
					 TP.SourceFile
					,TP.RowNumber
					,TP.TaskDate
					,'Generic Meetings' AS ColumnName
					,CASE
						WHEN UPPER(LTRIM(RTRIM(TP.GenericMeetings))) = 'HOMEWORK'
						THEN TP.GenericMeetings
						WHEN UPPER(LTRIM(RTRIM(TP.GenericMeetings))) LIKE ('%HOMEWORK%')
						THEN CONCAT('notify Debbie about this: ', TP.GenericMeetings)
						ELSE NULL
					END AS InvalidValue
				FROM
					CSYS..COST_DAT_TimesheetProcessing TP
				WHERE
					UPPER(LTRIM(RTRIM(TP.GenericMeetings))) = 'HOMEWORK'
					OR UPPER(LTRIM(RTRIM(TP.GenericMeetings))) LIKE ('%HOMEWORK%')

				UNION ALL

				--incomplete timesheets/no timesheet entries
				SELECT DISTINCT
					 MONTHTABLE.SourceFile
					,NULL AS RowNumber
					,NULL AS TaskDate
					,NULL AS ColumnName
					,'No timesheet entries for '+ MONTHTABLE.NamedMonth AS InvalidValue

					--monthly report fields:
					--MONTHTABLE.SourceFile
					--,MONTHTABLE.TIMESHEET_NAME
					--,MONTHTABLE.TaskMonth
					--,MONTHTABLE.NamedMonth AS [MONTH]
					--,ISNULL(EntryCounts.Entries,0) AS Entries
					----,CONCAT(MONTHTABLE.TIMESHEET_NAME,MONTHTABLE.TaskMonth,YEAR(GETDATE()))
				FROM
					(
						SELECT DISTINCT 
							TP.SourceFile,
							SUBSTRING(TP.SourceFile,
										(CHARINDEX('-', TP.SourceFile) + 2), --STARTVALUE
										(--take difference between STOPVALUE and STARTVALUE to get LENGTH
											CHARINDEX('.', TP.SourceFile) --STOPVALUE
											- (CHARINDEX('-', TP.SourceFile) + 2) --STARTVALUE
										)--LENGTH
									) AS TIMESHEET_NAME
							,GetMonths.TaskMonth
							,GetMonths.NamedMonth
						FROM
							(
								SELECT DISTINCT
									MONTH(TaskDate) AS TaskMonth
									,{fn MONTHNAME(TaskDate)} AS NamedMonth
								FROM
									CSYS..COST_DAT_TimesheetProcessing
								WHERE
									FsaCr != '1234, 5678'
							) GetMonths
							OUTER APPLY CSYS..COST_DAT_TimesheetProcessing TP
						WHERE
							TP.TaskDate >= @QUARTER_BEGIN
					) MONTHTABLE
					LEFT JOIN 
					(
						SELECT DISTINCT 
							SUBSTRING(SourceFile,
										(CHARINDEX('-', SourceFile) + 2), --STARTVALUE
										(--take difference between STOPVALUE and STARTVALUE to get LENGTH
											CHARINDEX('.', SourceFile) --STOPVALUE
											- (CHARINDEX('-', SourceFile) + 2) --STARTVALUE
										)--LENGTH
									) AS TIMESHEET_NAME
							,MONTH(TaskDate) AS TaskMonth
							,COUNT(*) AS Entries
						FROM
							CSYS..COST_DAT_TimesheetProcessing
						WHERE
							TaskDate >= @QUARTER_BEGIN
						GROUP BY
							SourceFile
							,MONTH(TaskDate)
					) EntryCounts
						ON MONTHTABLE.TIMESHEET_NAME = EntryCounts.TIMESHEET_NAME
						AND MONTHTABLE.TaskMonth = EntryCounts.TaskMonth
				WHERE 
					MONTHTABLE.TIMESHEET_NAME != 'Template'
					AND ISNULL(EntryCounts.Entries,0) < 3 --fewer than 3 entries
					AND	CONCAT(MONTHTABLE.TIMESHEET_NAME,MONTHTABLE.TaskMonth,YEAR(GETDATE())) NOT IN 
						('Trevor Eckhardt102019') --new hires exclude months not here
					AND MONTHTABLE.TaskMonth IS NOT NULL
					AND DAY(GETDATE()) > 15 --process after the 15th of the month
			) VALIDATIONERRORS
			LEFT JOIN FLAGGING
				ON VALIDATIONERRORS.SourceFile = FLAGGING.SourceFile
		WHERE
			VALIDATIONERRORS.SourceFile NOT LIKE '%Template%'
	) ALLERRORS
WHERE
	COALESCE(ALLERRORS.TaskDate, DATEADD(DAY,-1,CONVERT(DATE,GETDATE()))) != CONVERT(DATE,GETDATE())
--	AND ALLERRORS.EMail IN (@EMail) --LIVE for SSRS, remove for shared dataset
ORDER BY
	 ALLERRORS.[Name]
	,ALLERRORS.SourceFile
	,ALLERRORS.RowNumber
	,ALLERRORS.TaskDate
	,ALLERRORS.ColumnName
	,ALLERRORS.InvalidValue
	--ALLERRORS.EMail --for shared data set, keep this only and remove all other order by items
;