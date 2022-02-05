USE NobleCalls
GO

DECLARE @LastRefresh VARCHAR(23) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(CCD.startdatetime)), '1-1-1900 00:00:00'), 21) FROM agenttabl.ContactCallDetailUCCX CCD)
PRINT 'Last Refreshed timestamp at: ' + @LastRefresh


DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		agenttabl.ContactCallDetailUCCX CCD
	USING
		(
			SELECT
				sessionid,
				sessionseqnum,
				nodeid,
				profileid,
				contacttype,
				contactdisposition,
				dispositionreason,
				originatortype,
				originatorid,
				originatordn,
				destinationtype,
				destinationid,
				destinationdn,
				startdatetime,
				enddatetime,
				gmtoffset,
				callednumber,
				origcallednumber,
				applicationtaskid,
				applicationid,
				applicationname,
				connecttime,
				customvariable1,
				customvariable2,
				customvariable3,
				customvariable4,
				customvariable5,
				customvariable6,
				customvariable7,
				customvariable8,
				customvariable9,
				customvariable10,
				accountnumber,
				callerentereddigits,
				badcalltag,
				transfer,
				redirect,
				conference,
				flowout,
				metservicelevel,
				campaignid,
				origprotocolcallref,
				destprotocolcallref,
				callresult,
				dialinglistid,
				contactid,
				lastleg
			FROM
				OPENQUERY
				(
					UCCX,
					''
						SELECT DISTINCT
							CCD.sessionid,
							CCD.sessionseqnum,
							CCD.nodeid,
							CCD.profileid,
							CCD.contacttype,
							CCD.contactdisposition,
							CCD.dispositionreason,
							CCD.originatortype,
							CCD.originatorid,
							CCD.originatordn,
							CCD.destinationtype,
							CCD.destinationid,
							CCD.destinationdn,
							CCD.startdatetime,
							CCD.enddatetime,
							CCD.gmtoffset,
							CCD.callednumber,
							CCD.origcallednumber,
							CCD.applicationtaskid,
							CCD.applicationid,
							CCD.applicationname,
							CCD.connecttime,
							CCD.customvariable1,
							CCD.customvariable2,
							CCD.customvariable3,
							CCD.customvariable4,
							CCD.customvariable5,
							CCD.customvariable6,
							CCD.customvariable7,
							CCD.customvariable8,
							CCD.customvariable9,
							CCD.customvariable10,
							CCD.accountnumber,
							CCD.callerentereddigits,
							CCD.badcalltag,
							CCD.transfer,
							CCD.redirect,
							CCD.conference,
							CCD.flowout,
							CCD.metservicelevel,
							CCD.campaignid,
							CCD.origprotocolcallref,
							CCD.destprotocolcallref,
							CCD.callresult,
							CCD.dialinglistid,
							CCD.contactid,
							CCD.lastleg
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
							AND CCD.startdatetime > ''''' + @LastRefresh + '''''
					''
				) 
		) Remote 
			ON Remote.sessionID = CCD.sessionID
			AND Remote.sessionSeqNum = CCD.sessionSeqNum
			AND Remote.nodeID = CCD.nodeID
			AND Remote.profileID = CCD.profileID
	WHEN MATCHED THEN 
		UPDATE SET 
			CCD.contacttype = Remote.contacttype,
			CCD.contactdisposition = Remote.contactdisposition,
			CCD.dispositionreason = Remote.dispositionreason,
			CCD.originatortype = Remote.originatortype,
			CCD.originatorid = Remote.originatorid,
			CCD.originatordn = Remote.originatordn,
			CCD.destinationtype = Remote.destinationtype,
			CCD.destinationid = Remote.destinationid,
			CCD.destinationdn = Remote.destinationdn,
			CCD.startdatetime = Remote.startdatetime,
			CCD.enddatetime = Remote.enddatetime,
			CCD.gmtoffset = Remote.gmtoffset,
			CCD.callednumber = Remote.callednumber,
			CCD.origcallednumber = Remote.origcallednumber,
			CCD.applicationtaskid = Remote.applicationtaskid,
			CCD.applicationid = Remote.applicationid,
			CCD.applicationname = Remote.applicationname,
			CCD.connecttime = Remote.connecttime,
			CCD.customvariable1 = Remote.customvariable1,
			CCD.customvariable2 = Remote.customvariable2,
			CCD.customvariable3 = Remote.customvariable3,
			CCD.customvariable4 = Remote.customvariable4,
			CCD.customvariable5 = Remote.customvariable5,
			CCD.customvariable6 = Remote.customvariable6,
			CCD.customvariable7 = Remote.customvariable7,
			CCD.customvariable8 = Remote.customvariable8,
			CCD.customvariable9 = Remote.customvariable9,
			CCD.customvariable10 = Remote.customvariable10,
			CCD.accountnumber = Remote.accountnumber,
			CCD.callerentereddigits = Remote.callerentereddigits,
			CCD.badcalltag = Remote.badcalltag,
			CCD.transfer = Remote.transfer,
			CCD.redirect = Remote.redirect,
			CCD.conference = Remote.conference,
			CCD.flowout = Remote.flowout,
			CCD.metservicelevel = Remote.metservicelevel,
			CCD.campaignid = Remote.campaignid,
			CCD.origprotocolcallref = Remote.origprotocolcallref,
			CCD.destprotocolcallref = Remote.destprotocolcallref,
			CCD.callresult = Remote.callresult,
			CCD.dialinglistid = Remote.dialinglistid,
			CCD.contactid = Remote.contactid,
			CCD.lastleg = Remote.lastleg
	WHEN NOT MATCHED THEN
		INSERT 
		(
			sessionid,
			sessionseqnum,
			nodeid,
			profileid,
			contacttype,
			contactdisposition,
			dispositionreason,
			originatortype,
			originatorid,
			originatordn,
			destinationtype,
			destinationid,
			destinationdn,
			startdatetime,
			enddatetime,
			gmtoffset,
			callednumber,
			origcallednumber,
			applicationtaskid,
			applicationid,
			applicationname,
			connecttime,
			customvariable1,
			customvariable2,
			customvariable3,
			customvariable4,
			customvariable5,
			customvariable6,
			customvariable7,
			customvariable8,
			customvariable9,
			customvariable10,
			accountnumber,
			callerentereddigits,
			badcalltag,
			transfer,
			redirect,
			conference,
			flowout,
			metservicelevel,
			campaignid,
			origprotocolcallref,
			destprotocolcallref,
			callresult,
			dialinglistid,
			contactid,
			lastleg
		)
		VALUES 
		(
			Remote.sessionid,
			Remote.sessionseqnum,
			Remote.nodeid,
			Remote.profileid,
			Remote.contacttype,
			Remote.contactdisposition,
			Remote.dispositionreason,
			Remote.originatortype,
			Remote.originatorid,
			Remote.originatordn,
			Remote.destinationtype,
			Remote.destinationid,
			Remote.destinationdn,
			Remote.startdatetime,
			Remote.enddatetime,
			Remote.gmtoffset,
			Remote.callednumber,
			Remote.origcallednumber,
			Remote.applicationtaskid,
			Remote.applicationid,
			Remote.applicationname,
			Remote.connecttime,
			Remote.customvariable1,
			Remote.customvariable2,
			Remote.customvariable3,
			Remote.customvariable4,
			Remote.customvariable5,
			Remote.customvariable6,
			Remote.customvariable7,
			Remote.customvariable8,
			Remote.customvariable9,
			Remote.customvariable10,
			Remote.accountnumber,
			Remote.callerentereddigits,
			Remote.badcalltag,
			Remote.transfer,
			Remote.redirect,
			Remote.conference,
			Remote.flowout,
			Remote.metservicelevel,
			Remote.campaignid,
			Remote.origprotocolcallref,
			Remote.destprotocolcallref,
			Remote.callresult,
			Remote.dialinglistid,
			Remote.contactid,
			Remote.lastleg
		)
	-- !!!  uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'
--select @SQLStatement
PRINT @SQLStatement
EXEC (@SQLStatement)

