USE NobleCalls
GO
DECLARE @LastRefresh VARCHAR(23) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(ASD.eventDateTime)), '1-1-1900 00:00:00'), 21) FROM agenttabl.AgentStateDetailUCCX ASD)
PRINT 'Last Refreshed timestamp at: ' + @LastRefresh


DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		agenttabl.AgentStateDetailUCCX ASD
	USING
		(
			SELECT
				agentid,
				eventdatetime,
				gmtoffset,
				eventtype,
				reasoncode,
				profileid,
				contactid,
				loginsessionid
			FROM
				OPENQUERY
				(
					UCCX,
					''
						SELECT
							ASD.agentid,
							ASD.eventdatetime,
							ASD.gmtoffset,
							ASD.eventtype,
							ASD.reasoncode,
							ASD.profileid,
							ASD.contactid,
							ASD.loginsessionid
						FROM
							AgentStateDetail ASD, Resource R, Team T, ResourceGroup RG
						-- comment WHERE clause for full table refresh
						WHERE
							--Resource Inner Join
							R.ResourceId = ASD.agentid
							--Team Inner Join
							AND T.teamid = R.assignedteamid
							AND T.teamname IN (''''LPP'''',''''LGP'''',''''Default'''')
							--ResourceGroup Inner Join
							AND RG.resourcegroupid = R.resourcegroupid
							AND RG.resourcegroupname LIKE ''''UHEAA%''''
							AND ASD.eventDateTime > ''''' + @LastRefresh + '''''
					''
				) 
		) Remote 
			ON Remote.agentid = ASD.agentid
			AND Remote.eventDateTime = ASD.eventDateTime
			AND Remote.eventType = ASD.eventType
			AND Remote.reasonCode = ASD.reasonCode
			AND Remote.profileID = ASD.profileID
	WHEN MATCHED THEN 
		UPDATE SET 
			ASD.gmtoffset = Remote.gmtoffset,
			ASD.contactid = Remote.contactid,
			ASD.loginsessionid = Remote.loginsessionid
	WHEN NOT MATCHED THEN
		INSERT 
		(
			agentid,
			eventdatetime,
			gmtoffset,
			eventtype,
			reasoncode,
			profileid,
			contactid,
			loginsessionid
		)
		VALUES 
		(
			Remote.agentid,
			Remote.eventdatetime,
			Remote.gmtoffset,
			Remote.eventtype,
			Remote.reasoncode,
			Remote.profileid,
			Remote.contactid,
			Remote.loginsessionid
		)
	-- !!!  uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'
--select @SQLStatement
PRINT @SQLStatement
EXEC (@SQLStatement)

