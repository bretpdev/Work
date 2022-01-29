USE NobleCalls
GO

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		agenttabl.ContactServiceQueueUCCX CSQ
	USING
		(
			SELECT
				contactservicequeueid,
				profileid,
				csqname,
				resourcepooltype,
				resourcegroupid,
				selectioncriteria,
				skillgroupid,
				servicelevel,
				servicelevelpercentage,
				active,
				autowork,
				dateinactive,
				queuealgorithm,
				recordid,
				orderlist,
				wrapuptime,
				prompt,
				privatedata,
				queuetype,
				queuetypename,
				emailauthtype,
				emailoauthdetails,
				accountuserid,
				accountpassword,
				channelproviderid,
				reviewqueueid,
				routingtype,
				foldername,
				pollinginterval,
				snapshotage,
				feedid
			FROM
				OPENQUERY
				(
					UCCX,
					''
						SELECT DISTINCT
							CSQ.contactservicequeueid,
							CSQ.profileid,
							CSQ.csqname,
							CSQ.resourcepooltype,
							CSQ.resourcegroupid,
							CSQ.selectioncriteria,
							CSQ.skillgroupid,
							CSQ.servicelevel,
							CSQ.servicelevelpercentage,
							CSQ.active,
							CSQ.autowork,
							CSQ.dateinactive,
							CSQ.queuealgorithm,
							CSQ.recordid,
							CSQ.orderlist,
							CSQ.wrapuptime,
							CSQ.prompt,
							CSQ.privatedata,
							CSQ.queuetype,
							CSQ.queuetypename,
							CSQ.emailauthtype,
							CSQ.emailoauthdetails,
							CSQ.accountuserid,
							CSQ.accountpassword,
							CSQ.channelproviderid,
							CSQ.reviewqueueid,
							CSQ.routingtype,
							CSQ.foldername,
							CSQ.pollinginterval,
							CSQ.snapshotage,
							CSQ.feedid
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
					''
				) 
		) Remote 
			ON Remote.recordID = CSQ.recordID
	WHEN MATCHED THEN 
		UPDATE SET 
			CSQ.contactservicequeueid = Remote.contactservicequeueid,
			CSQ.profileid = Remote.profileid,
			CSQ.csqname = Remote.csqname,
			CSQ.resourcepooltype = Remote.resourcepooltype,
			CSQ.resourcegroupid = Remote.resourcegroupid,
			CSQ.selectioncriteria = Remote.selectioncriteria,
			CSQ.skillgroupid = Remote.skillgroupid,
			CSQ.servicelevel = Remote.servicelevel,
			CSQ.servicelevelpercentage = Remote.servicelevelpercentage,
			CSQ.active = Remote.active,
			CSQ.autowork = Remote.autowork,
			CSQ.dateinactive = Remote.dateinactive,
			CSQ.queuealgorithm = Remote.queuealgorithm,
			CSQ.orderlist = Remote.orderlist,
			CSQ.wrapuptime = Remote.wrapuptime,
			CSQ.prompt = Remote.prompt,
			CSQ.privatedata = Remote.privatedata,
			CSQ.queuetype = Remote.queuetype,
			CSQ.queuetypename = Remote.queuetypename,
			CSQ.emailauthtype = Remote.emailauthtype,
			CSQ.emailoauthdetails = Remote.emailoauthdetails,
			CSQ.accountuserid = Remote.accountuserid,
			CSQ.accountpassword = Remote.accountpassword,
			CSQ.channelproviderid = Remote.channelproviderid,
			CSQ.reviewqueueid = Remote.reviewqueueid,
			CSQ.routingtype = Remote.routingtype,
			CSQ.foldername = Remote.foldername,
			CSQ.pollinginterval = Remote.pollinginterval,
			CSQ.snapshotage = Remote.snapshotage,
			CSQ.feedid = Remote.feedid
	WHEN NOT MATCHED THEN
		INSERT 
		(
			contactservicequeueid,
			profileid,
			csqname,
			resourcepooltype,
			resourcegroupid,
			selectioncriteria,
			skillgroupid,
			servicelevel,
			servicelevelpercentage,
			active,
			autowork,
			dateinactive,
			queuealgorithm,
			recordid,
			orderlist,
			wrapuptime,
			prompt,
			privatedata,
			queuetype,
			queuetypename,
			emailauthtype,
			emailoauthdetails,
			accountuserid,
			accountpassword,
			channelproviderid,
			reviewqueueid,
			routingtype,
			foldername,
			pollinginterval,
			snapshotage,
			feedid
		)
		VALUES 
		(
			Remote.contactservicequeueid,
			Remote.profileid,
			Remote.csqname,
			Remote.resourcepooltype,
			Remote.resourcegroupid,
			Remote.selectioncriteria,
			Remote.skillgroupid,
			Remote.servicelevel,
			Remote.servicelevelpercentage,
			Remote.active,
			Remote.autowork,
			Remote.dateinactive,
			Remote.queuealgorithm,
			Remote.recordid,
			Remote.orderlist,
			Remote.wrapuptime,
			Remote.prompt,
			Remote.privatedata,
			Remote.queuetype,
			Remote.queuetypename,
			Remote.emailauthtype,
			Remote.emailoauthdetails,
			Remote.accountuserid,
			Remote.accountpassword,
			Remote.channelproviderid,
			Remote.reviewqueueid,
			Remote.routingtype,
			Remote.foldername,
			Remote.pollinginterval,
			Remote.snapshotage,
			Remote.feedid
		)
	-- !!!  uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'
--select @SQLStatement
PRINT @SQLStatement
EXEC (@SQLStatement)

