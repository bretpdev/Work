USE NobleCalls
GO

DECLARE @LastRefresh VARCHAR(23) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(CWUD.startdatetime)), '1-1-1900 00:00:00'), 21) FROM agenttabl.ContactWrapUpDataUCCX CWUD)
PRINT 'Last Refreshed timestamp at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		agenttabl.ContactWrapUpDataUCCX CWUD
	USING
		(
			SELECT
				sessionID,
				sessionSeqNum, 
				resourceID, 
				wrapupData, 
				nodeid, 
				qindex, 
				CAST(startDateTime AS VARCHAR(23)) AS startDateTime,
				wrapupindex,
				contactid
			FROM
				OPENQUERY
				(
					UCCX,
					''
						SELECT DISTINCT
							CWUD.sessionID, 
							CWUD.sessionSeqNum, 
							CWUD.resourceID, 
							CWUD.wrapupData, 
							CWUD.nodeid, 
							CWUD.qindex, 
							CAST(CWUD.startDateTime AS VARCHAR(23)) AS startDateTime, 
							CWUD.wrapupindex, 
							CWUD.contactid 
						FROM 
							AgentConnectionDetail ACD, ContactCallDetail CCD, ContactQueueDetail CQD, ContactWrapUpData CWUD, ContactServiceQueue CSQ
						WHERE
							--ContactCallDetail Inner Join
							CCD.contactid = ACD.contactid
							AND CCD.sessionID = ACD.sessionID
							AND CCD.sessionSeqNum = ACD.sessionSeqNum
							AND CCD.nodeID = ACD.nodeID
							AND CCD.profileID = ACD.profileID
							--ContactQueueDetail INNER JOIN	
							AND CQD.contactid = CCD.contactid
							AND CQD.sessionID = CCD.sessionID
							AND CQD.sessionSeqNum = CCD.sessionSeqNum
							AND CQD.profileID = CCD.profileID
							AND CQD.nodeID = CCD.nodeID
							AND CQD.targetType = 0
							--ContactWrapUpData Inner Join
							AND CWUD.sessionID = CCD.sessionID
							AND CWUD.sessionSeqNum = CCD.sessionSeqNum
							AND CWUD.nodeid = CCD.nodeid
							AND CWUD.contactid = CCD.contactid
							--ContactServiceQueue Inner Join
							AND CQD.targetID = CSQ.recordID
							AND CSQ.active
							AND
							(
								CSQ.CSQName LIKE ''''LGP%''''
								OR CSQ.CSQName LIKE ''''LPP%''''
								OR CSQ.CSQName LIKE ''''RC%''''
							) 
							AND CWUD.startDateTime > ''''' + @LastRefresh + '''''
					''
				) 
		) Remote 
			ON Remote.sessionID = CWUD.sessionID
			AND Remote.sessionSeqNum = CWUD.sessionSeqNum
			AND Remote.resourceID = CWUD.resourceID
			AND Remote.wrapupData = CWUD.wrapupData
			AND Remote.nodeid = CWUD.nodeid
			AND Remote.qindex = CWUD.qindex
			AND CAST(Remote.startDateTime AS DATETIME2) = CWUD.startDateTime
	WHEN MATCHED THEN 
		UPDATE SET 
			CWUD.wrapupindex = Remote.wrapupindex,
			CWUD.contactid = Remote.contactid
	WHEN NOT MATCHED THEN
		INSERT 
		(
			sessionID,
			sessionSeqNum, 
			resourceID, 
			wrapupData, 
			nodeid, 
			qindex, 
			startDateTime,
			wrapupindex,
			contactid
		)
		VALUES 
		(
			Remote.sessionID,
			Remote.sessionSeqNum, 
			Remote.resourceID, 
			Remote.wrapupData, 
			Remote.nodeid, 
			Remote.qindex, 
			CAST(Remote.startDateTime AS DATETIME2),
			Remote.wrapupindex,
			Remote.contactid
		)
	-- !!!  uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'
--select @SQLStatement
PRINT @SQLStatement
EXEC (@SQLStatement)

