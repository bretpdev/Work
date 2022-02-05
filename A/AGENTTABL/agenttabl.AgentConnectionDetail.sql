USE NobleCalls
GO

DECLARE @LastRefresh VARCHAR(23) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(ACD.startdatetime)), '1-1-1900 00:00:00'), 21) FROM agenttabl.AgentConnectionDetailUCCX ACD)
PRINT 'Last Refreshed timestamp at: ' + @LastRefresh


DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		agenttabl.AgentConnectionDetailUCCX ACD
	USING
		(
			SELECT
				sessionid,
				sessionseqnum,
				nodeid,
				profileid,
				resourceid,
				startdatetime,
				enddatetime,
				qindex,
				gmtoffset,
				ringtime,
				talktime,
				holdtime,
				worktime,
				callwrapupdata,
				callresult,
				dialinglistid,
				rna,
				contactid,
				loginsessionid,
				csqrecordid,
				consultsessionid
			FROM
				OPENQUERY
				(
					UCCX,
					''
						SELECT DISTINCT
							ACD.sessionid,
							ACD.sessionseqnum,
							ACD.nodeid,
							ACD.profileid,
							ACD.resourceid,
							ACD.startdatetime,
							ACD.enddatetime,
							ACD.qindex,
							ACD.gmtoffset,
							ACD.ringtime,
							ACD.talktime,
							ACD.holdtime,
							ACD.worktime,
							ACD.callwrapupdata,
							ACD.callresult,
							ACD.dialinglistid,
							ACD.rna,
							ACD.contactid,
							ACD.loginsessionid,
							ACD.csqrecordid,
							ACD.consultsessionid
						FROM
							AgentConnectionDetail ACD, ContactCallDetail CCD, ContactQueueDetail CQD, ContactServiceQueue CSQ
						WHERE
							--ContactCallDetail Inner Join
							CCD.contactid = ACD.contactid
							AND CCD.sessionID = ACD.sessionID
							AND CCD.sessionSeqNum = ACD.sessionSeqNum
							AND CCD.nodeID = ACD.nodeID
							AND CCD.profileID = ACD.profileID
							AND CCD.originatortype = 3 --Incoming call?
							--ContactQueueDetail INNER JOIN	
							AND CQD.contactid = CCD.contactid
							AND CQD.sessionID = CCD.sessionID
							AND CQD.sessionSeqNum = CCD.sessionSeqNum
							AND CQD.profileID = CCD.profileID
							AND CQD.nodeID = CCD.nodeID
							AND CQD.targetType = 0
							--ContactServiceQueue Inner Join
							AND CQD.targetID = CSQ.recordID
							AND CSQ.active
							AND
							(
								CSQ.CSQName LIKE ''''LGP%''''
								OR CSQ.CSQName LIKE ''''LPP%''''
								OR CSQ.CSQName LIKE ''''RC%''''
							)
							AND ACD.startdatetime > ''''' + @LastRefresh + '''''
					''
				) 
		) Remote 
			ON Remote.sessionID = ACD.sessionID
			AND Remote.sessionSeqNum = ACD.sessionSeqNum
			AND Remote.nodeID = ACD.nodeID
			AND Remote.profileID = ACD.profileID
			AND Remote.resourceID = ACD.resourceID
			AND Remote.startdatetime = ACD.startdatetime
			AND Remote.qIndex = ACD.qIndex
	WHEN MATCHED THEN 
		UPDATE SET 
			ACD.enddatetime = Remote.enddatetime,
			ACD.gmtoffset = Remote.gmtoffset,
			ACD.ringtime = Remote.ringtime,
			ACD.talktime = Remote.talktime,
			ACD.holdtime = Remote.holdtime,
			ACD.worktime = Remote.worktime,
			ACD.callwrapupdata = Remote.callwrapupdata,
			ACD.callresult = Remote.callresult,
			ACD.dialinglistid = Remote.dialinglistid,
			ACD.rna = Remote.rna,
			ACD.contactid = Remote.contactid,
			ACD.loginsessionid = Remote.loginsessionid,
			ACD.csqrecordid = Remote.csqrecordid,
			ACD.consultsessionid = Remote.consultsessionid
	WHEN NOT MATCHED THEN
		INSERT 
		(
			sessionid,
			sessionseqnum,
			nodeid,
			profileid,
			resourceid,
			startdatetime,
			enddatetime,
			qindex,
			gmtoffset,
			ringtime,
			talktime,
			holdtime,
			worktime,
			callwrapupdata,
			callresult,
			dialinglistid,
			rna,
			contactid,
			loginsessionid,
			csqrecordid,
			consultsessionid
		)
		VALUES 
		(
			Remote.sessionid,
			Remote.sessionseqnum,
			Remote.nodeid,
			Remote.profileid,
			Remote.resourceid,
			Remote.startdatetime,
			Remote.enddatetime,
			Remote.qindex,
			Remote.gmtoffset,
			Remote.ringtime,
			Remote.talktime,
			Remote.holdtime,
			Remote.worktime,
			Remote.callwrapupdata,
			Remote.callresult,
			Remote.dialinglistid,
			Remote.rna,
			Remote.contactid,
			Remote.loginsessionid,
			Remote.csqrecordid,
			Remote.consultsessionid
		)
	-- !!!  uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'
--select @SQLStatement
PRINT @SQLStatement
EXEC (@SQLStatement)

