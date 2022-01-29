DECLARE 
	@UNIT VARCHAR(2) = 35 --systems support unit
	,@DATE1 DATETIME = '7/01/2016 00:00:01'
	,@DATE2 DATETIME = '12/31/2016 23:59:59'
;

--creates table of ticket types to ignore
DECLARE @Ignore TABLE 
(
	IgnoreControlDisplayText VARCHAR(200)
);

INSERT INTO @Ignore	(IgnoreControlDisplayText)
VALUES
	('CornerStone Policy/Regulatory/Compliance Issue')
	,('Policy/Regulatory/Compliance Issue')
	,('CornerStone Financial Adjustment Request')
	,('Financial Adjustment Request')
	,('Facilities Request')
	,('DO NOT USE')
	,('CornerStone Funds Transfer')
	,('Funds Transfer')
	,('CornerStone School Eligibility')
	,('School Eligibility')
	,('UHEAA Audit Coordination')
	,('CornerStone Audit Coordination')
;

--creates table of ticket types to consider
DECLARE @Closed TABLE
(
	ClosedId INT IDENTITY(1,1) NOT NULL
	,Region VARCHAR(15) NULL
	,FlowID VARCHAR(15) NULL
	,OpenTicketType VARCHAR(100) NOT NULL
);

INSERT INTO	@Closed	(OpenTicketType)
VALUES
	('Action Code Creation')
	,('Activity Deletion Form')
	,('ARC Assignment')
	,('ARC Creation')
	,('ARC Modification')
	,('Borrower Benefit Override')
	,('Data Change Request')
	,('IDEM Updates')
	,('LPDs')
	,('Other System Assistance')
	,('Parm Cards')
	--,('Policy/Regulatory/Compliance Issue')
	,('Queue Assignment')
	,('Queue Creation')
	,('Special Handling of a Borrower Account')
	,('System Access Request')
	,('System Functionality and Use')
	,('System Problem')
	,('Table of Codes')
	,('CornerStone Action Code Creation')
	,('CornerStone Activity Deletion Form')
	,('CornerStone ARC Assignment')
	,('CornerStone ARC Creation')
	,('CornerStone ARC Modification')
	,('CornerStone Borrower Benefit Override')
	,('CornerStone Data Change Request')
	,('CornerStone IDEM Updates')
	,('CornerStone LPDs')
	,('CornerStone Other System Assistance')
	--,('CornerStone Policy/Regulatory/Compliance Issue')
	,('CornerStone Queue Assignment')
	,('CornerStone Queue Creation')
	,('CornerStone Special Handling of a Borrower Account')
	,('CornerStone System Functionality and Use')
	,('CornerStone System Problem')
	,('CornerStone Table of Codes')
;

--inserts Region into Ticket Type table
UPDATE 
	CLOZ
SET
	CLOZ.FlowID = FDF.FlowID
	,CLOZ.Region = CASE 
		WHEN FDF.ControlDisplayText LIKE 'CornerStone%' 
		THEN 'CornerStone' 
		ELSE 'UHEAA' 
		END
FROM
	CSYS.dbo.FLOW_DAT_Flow FDF
	INNER JOIN @Closed CLOZ
		ON CLOZ.OpenTicketType = FDF.ControlDisplayText
	INNER JOIN 
	(
		SELECT
			ROW_PART.FlowID
			,ROW_PART.Status
		FROM
			(
				SELECT
					FDF.FlowID
					,FDF.FlowStepSequenceNumber
					,FDF.Status
					,ROW_NUMBER() OVER (PARTITION BY FDF.FlowID ORDER BY FDF.FlowStepSequenceNumber DESC) ReverseSequenceNumber
				FROM
					CSYS.dbo.FLOW_DAT_FlowStep FDF
				WHERE
					FDF.FlowID != ''
			) ROW_PART
		WHERE
			ReverseSequenceNumber = 1
	) NO1
		ON NO1.FlowID = FDF.FlowID
;

--gets NOCHOUSE hours for Systems Support staff
SELECT
	CAST(SUM(CAST(DATEDIFF(SECOND, TT.StartTime, TT.EndTime) AS FLOAT) / 60 / 60) AS FLOAT) AS Hours_JD16
FROM
	Reporting.dbo.TimeTracking TT
WHERE
	TT.SqlUserID IN
	(
		SELECT 
			SDU.SqlUserID
		FROM 
			CSYS.dbo.SYSA_DAT_Users SDU
		WHERE 
			SDU.SqlUserID IN 
			(
				 1152 --Wendy
				,1198 --Jeremy
				,1399 --Candice
				,1519 --David
				,1550 --Seth
				,1633 --Hope
			)
	)
	AND TT.StartTime > @DATE1 
	AND TT.StartTime < @DATE2
;	

/***********HOURS ACTIVE tab in Excel spreadsheet***********/
--creates big table of closed ticket counts with hours, and open tickets
SELECT 
	 HRS#TIX.Region
	,HRS#TIX.FlowID
	,COALESCE(HRS#TIX.TicketCount_JD16,0) TicketCount_JD16
	,COALESCE(HRS#TIX.Hours_JD16,0) Hours_JD16
	,OPENTIX.OpenTicketType
	,OPENTIX.FlowID
	,COALESCE(OPENTIX.OpenTicketCount,0) OpenTicketCount
FROM 
(	--gets # of UHEAA tickets & hours Jul-Dec 2016
	SELECT
		TT.Region
		,FDF.FlowID
		,COUNT(*) AS TicketCount_JD16
		,CAST(SUM(CAST(DATEDIFF(SECOND, TT.StartTime, TT.EndTime) AS FLOAT) / 60 / 60) AS FLOAT) AS Hours_JD16
	FROM 
		NeedHelpUheaa.dbo.DAT_Ticket DT
		INNER JOIN Reporting.dbo.TimeTracking TT
			ON DT.Ticket = TT.TicketID
		INNER JOIN CSYS.dbo.FLOW_DAT_Flow FDF
			ON FDF.FlowID = DT.TicketCode
		LEFT JOIN @Ignore IG
			ON IG.IgnoreControlDisplayText = FDF.ControlDisplayText
	WHERE 
		IG.IgnoreControlDisplayText IS NULL
		AND TT.SqlUserID IN 
		(
			 1152 --Wendy
			,1198 --Jeremy
			,1399 --Candice
			,1519 --David
			,1550 --Seth
			,1633 --Hope
		)
		AND Requested BETWEEN @DATE1 AND @DATE2
	GROUP BY
		TT.Region
		,FDF.FlowID

	UNION ALL
	--gets # of CornerStone tickets & hours Jul-Dec 2016
	SELECT
		TT.Region
		,FDF.FlowID
		,COUNT(*) AS TicketCount_JD16
		,CAST(SUM(CAST(DATEDIFF(SECOND, TT.StartTime, TT.EndTime) AS FLOAT) / 60 / 60) AS FLOAT) AS Hours_JD16
	FROM 
		NeedHelpCornerStone.dbo.DAT_Ticket DT
		INNER JOIN Reporting.dbo.TimeTracking TT
			ON DT.Ticket = TT.TicketID
		INNER JOIN CSYS.dbo.FLOW_DAT_Flow FDF
			ON FDF.FlowID = DT.TicketCode
		LEFT JOIN @Ignore IG
			ON IG.IgnoreControlDisplayText = FDF.ControlDisplayText
	WHERE 
		IG.IgnoreControlDisplayText IS NULL
		AND TT.SqlUserID IN 
		(
			 1152 --Wendy
			,1198 --Jeremy
			,1399 --Candice
			,1519 --David
			,1550 --Seth
			,1633 --Hope
		)
		AND Requested BETWEEN @DATE1 AND @DATE2
	GROUP BY
		TT.Region
		,FDF.FlowID
) HRS#TIX
	FULL OUTER JOIN 
	(	--gets currently open UHEAA tickets
		SELECT
			CLOZ.OpenTicketType
			,CLOZ.FlowID
			,ISNULL(OPEN_U.OpenTicketCount, 0) OpenTicketCount
		FROM
			@Closed CLOZ
			LEFT JOIN
			(	--open tickets UHEAA
				SELECT
					'UHEAA' Region
					,FDF.ControlDisplayText TicketType
					,COUNT(DISTINCT DT.Ticket) OpenTicketCount
				FROM
					NeedHelpUHEAA.dbo.DAT_Ticket DT
					INNER JOIN CSYS.dbo.FLOW_DAT_Flow FDF 
						ON FDF.FlowID = DT.TicketCode
					INNER JOIN NeedHelpUHEAA.dbo.DAT_TicketsAssociatedUserID TAU
						ON TAU.Ticket = DT.Ticket 
						AND TAU.Role = 'court'
					INNER JOIN CSYS.dbo.SYSA_DAT_USERS SDU 
						ON SDU.SqlUserId = TAU.SqlUserId
					INNER JOIN @Closed CLOZ
						ON CLOZ.FlowID = FDF.FlowID
				WHERE
					TAU.SqlUserId IS NOT NULL
					OR
					(
						TAU.SqlUserId = 1119 -- Brenda Cox
						AND	DT.Status != 'BS Approval'
					)
				GROUP BY
					FDF.ControlDisplayText
			) OPEN_U 
				ON OPEN_U.TicketType = CLOZ.OpenTicketType
			LEFT JOIN @Ignore IG
				ON IG.IgnoreControlDisplayText = OPEN_U.TicketType
		WHERE
			IG.IgnoreControlDisplayText IS NULL
			AND CLOZ.Region = 'UHEAA'

		UNION ALL
		--gets currently open CornerStone tickets
		SELECT
			CLOZ.OpenTicketType
			,CLOZ.FlowID
			,ISNULL(OPEN_C.OpenTicketCount, 0) OpenTicketCount
		FROM
			@Closed CLOZ
			LEFT JOIN
			(	-- open tickets CornerStone
				SELECT
					'CornerStone' Region
					,FDF.ControlDisplayText TicketType
					,COUNT(DISTINCT DT.Ticket) OpenTicketCount
				FROM
					NeedHelpCornerStone.dbo.DAT_ticket DT
					INNER JOIN CSYS.dbo.FLOW_DAT_Flow FDF 
						ON FDF.FlowID = DT.TicketCode
					INNER JOIN NeedHelpCornerStone.dbo.DAT_TicketsAssociatedUserID TAU 
						ON TAU.Ticket = DT.Ticket 
						AND TAU.Role = 'court'
					INNER JOIN CSYS.dbo.SYSA_DAT_USERS SDU 
						ON SDU.SqlUserId = TAU.SqlUserId
					INNER JOIN @Closed CLOZ
						ON CLOZ.FlowID = FDF.FlowID
				WHERE
					TAU.SqlUserId is NOT NULL
					OR
					(
						TAU.SqlUserId = 1119 -- Brenda Cox
						AND	DT.Status != 'BS Approval'
					)
				GROUP BY
					FDF.ControlDisplayText
			) OPEN_C
				ON OPEN_C.TicketType = CLOZ.OpenTicketType
			LEFT JOIN @Ignore IG
				ON IG.IgnoreControlDisplayText = OPEN_C.TicketType
		WHERE
			IG.IgnoreControlDisplayText IS NULL
			AND CLOZ.Region = 'CornerStone'
) OPENTIX
	ON HRS#TIX.FlowID = OPENTIX.FlowID
ORDER BY
	HRS#TIX.Region DESC
	,HRS#TIX.FlowID


/*********************HOURS SIT in Excel spreadsheet**************/
--gets # of UHEAA & CornerStone hours/days and accompanying Request/Court/Complete statuses assigned to Systems Support
;WITH UHEAA_CTE AS 
	(
		SELECT 
			DTU.TicketCode
			,CAST(SUM(CAST(DATEDIFF(SECOND, DTU.Requested, DTU.LastUpdated) AS FLOAT) / 60 / 60) AS FLOAT) AS #_D1
			,CAST(SUM(CAST(DATEDIFF(SECOND, DTU.Requested, DTU.CourtDate) AS FLOAT) / 60 / 60) AS FLOAT) AS #_D3
			,CAST(SUM(CAST(DATEDIFF(SECOND, DTU.CourtDate, DTU.LastUpdated) AS FLOAT) / 60 / 60) AS FLOAT) AS #_D5
		FROM 
			NeedHelpUheaa.dbo.DAT_Ticket DTU
		WHERE 
			DTU.Unit = @UNIT
			AND DTU.Requested BETWEEN @DATE1 AND @DATE2
		GROUP BY
			 DTU.TicketCode
			,DTU.Requested
			,DTU.LastUpdated
			,DTU.CourtDate
		HAVING 
			DATEDIFF(SECOND, DTU.Requested, DTU.LastUpdated) != 0
			OR DATEDIFF(SECOND, DTU.Requested, DTU.CourtDate) != 0
			OR DATEDIFF(SECOND, DTU.CourtDate, DTU.LastUpdated) != 0
	),
	CORNERSTONE_CTE AS 
	(
		SELECT 
			DTC.TicketCode
			,CAST(SUM(CAST(DATEDIFF(SECOND, DTC.Requested, DTC.LastUpdated) AS FLOAT) / 60 / 60) AS FLOAT) AS #_D2
			,CAST(SUM(CAST(DATEDIFF(SECOND, DTC.Requested, DTC.CourtDate) AS FLOAT) / 60 / 60) AS FLOAT) AS #_D4
			,CAST(SUM(CAST(DATEDIFF(SECOND, DTC.CourtDate, DTC.LastUpdated) AS FLOAT) / 60 / 60) AS FLOAT) AS #_D6
		FROM
			NeedHelpCornerStone.dbo.DAT_Ticket DTC
		WHERE 
			DTC.Unit = @UNIT
			AND DTC.Requested BETWEEN @DATE1 AND @DATE2
		GROUP BY
			 DTC.TicketCode
			,DTC.Requested
			,DTC.LastUpdated
			,DTC.CourtDate
		HAVING 
			DATEDIFF(SECOND, DTC.Requested, DTC.LastUpdated) != 0
			OR DATEDIFF(SECOND, DTC.Requested, DTC.CourtDate) != 0
			OR DATEDIFF(SECOND, DTC.CourtDate, DTC.LastUpdated) != 0
	)
SELECT
	'UHEAA' Region
	,UHEAA_CTE.TicketCode
	,SUM(#_D1)		Hrs_Request_to_Complete_JD16
	,SUM(#_D1)/24	Days_Request_to_Complete_JD16
	,SUM(#_D3)		Hrs_Request_to_Court_JD16
	,SUM(#_D3)/24	Days_Request_to_Court_JD16
	,SUM(#_D5)		Hrs_Court_to_Complete_JD16
	,SUM(#_D5)/24	Days_Court_to_Complete_JD16
FROM 
	UHEAA_CTE
GROUP BY 
	UHEAA_CTE.TicketCode

UNION ALL

SELECT
	'Cornerstone' Region
	,CORNERSTONE_CTE.TicketCode
	,SUM(#_D2)		Hrs_Request_to_Complete_JD16
	,SUM(#_D2)/24	Days_Request_to_Complete_JD16
	,SUM(#_D4)		Hrs_Request_to_Court_JD16
	,SUM(#_D4)/24	Days_Request_to_Court_JD16
	,SUM(#_D6)		Hrs_Court_to_Complete_JD16
	,SUM(#_D6)/24	Days_Court_to_Complete_JD16
FROM 
	CORNERSTONE_CTE
GROUP BY 
	CORNERSTONE_CTE.TicketCode


--/****************mismatched region******************/
--Debbie said to leave as-is
----UHEAA ticket but CornerStone region
--SELECT DISTINCT
--	DT.Ticket
--	,DT.TicketCode
--	,Region
--	,Subject
--FROM 
--	NeedHelpUheaa.dbo.DAT_Ticket DT
--	INNER JOIN Reporting.dbo.TimeTracking TT
--	ON DT.Ticket = TT.TicketID
--WHERE 
--	Region = 'cornerstone'
--	AND Requested >= '2016-07-01'

----CornerStone ticket but UHEAA region
--SELECT DISTINCT
--	DT.Ticket
--	,DT.TicketCode
--	,Region
--	,Subject
--FROM 
--	NeedHelpCornerStone.dbo.DAT_Ticket DT
--	INNER JOIN Reporting.dbo.TimeTracking TT
--	ON DT.Ticket = TT.TicketID
--WHERE 
--	Region = 'uheaa'
--	AND Requested >= '2016-07-01'

