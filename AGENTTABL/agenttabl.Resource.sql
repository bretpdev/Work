USE NobleCalls
GO

DECLARE @LastRefresh VARCHAR(23) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(R.dateinactive)), '1-1-1900 00:00:00'), 21) FROM agenttabl.ResourceUCCX R)
PRINT 'Last Refreshed timestamp at: ' + @LastRefresh


DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		agenttabl.ResourceUCCX R
	USING
		(
			SELECT
				resourceid,
				profileid,
				resourceloginid,
				resourcename,
				resourcegroupid,
				resourcetype,
				active,
				autoavail,
				extension,
				orderinrg,
				dateinactive,
				resourceskillmapid,
				assignedteamid,
				resourcefirstname,
				resourcelastname,
				resourcealias,
				capabilities,
				resourceemailid
			FROM
				OPENQUERY
				(
					UCCX,
					''
						SELECT
							resourceid,
							profileid,
							resourceloginid,
							resourcename,
							resourcegroupid,
							resourcetype,
							active,
							autoavail,
							extension,
							orderinrg,
							dateinactive,
							resourceskillmapid,
							assignedteamid,
							resourcefirstname,
							resourcelastname,
							resourcealias,
							capabilities,
							resourceemailid
						FROM
							Resource
						WHERE
							dateinactive IS NULL
							OR dateinactive > ''''' + @LastRefresh + '''''
						-- comment WHERE clause for full table refresh
					''
				) 
		) Remote 
			ON Remote.resourceID = R.resourceID
			AND Remote.profileID = R.profileID
	WHEN MATCHED THEN 
		UPDATE SET 
			R.resourceloginid = Remote.resourceloginid,
			R.resourcename = Remote.resourcename,
			R.resourcegroupid = Remote.resourcegroupid,
			R.resourcetype = Remote.resourcetype,
			R.active = Remote.active,
			R.autoavail = Remote.autoavail,
			R.extension = Remote.extension,
			R.orderinrg = Remote.orderinrg,
			R.dateinactive = Remote.dateinactive,
			R.resourceskillmapid = Remote.resourceskillmapid,
			R.assignedteamid = Remote.assignedteamid,
			R.resourcefirstname = Remote.resourcefirstname,
			R.resourcelastname = Remote.resourcelastname,
			R.resourcealias = Remote.resourcealias,
			R.capabilities = Remote.capabilities,
			R.resourceemailid = Remote.resourceemailid
	WHEN NOT MATCHED THEN
		INSERT 
		(
			resourceid,
			profileid,
			resourceloginid,
			resourcename,
			resourcegroupid,
			resourcetype,
			active,
			autoavail,
			extension,
			orderinrg,
			dateinactive,
			resourceskillmapid,
			assignedteamid,
			resourcefirstname,
			resourcelastname,
			resourcealias,
			capabilities,
			resourceemailid
		)
		VALUES 
		(
			Remote.resourceid,
			Remote.profileid,
			Remote.resourceloginid,
			Remote.resourcename,
			Remote.resourcegroupid,
			Remote.resourcetype,
			Remote.active,
			Remote.autoavail,
			Remote.extension,
			Remote.orderinrg,
			Remote.dateinactive,
			Remote.resourceskillmapid,
			Remote.assignedteamid,
			Remote.resourcefirstname,
			Remote.resourcelastname,
			Remote.resourcealias,
			Remote.capabilities,
			Remote.resourceemailid
		)
	-- !!!  uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'
--select @SQLStatement
PRINT @SQLStatement
EXEC (@SQLStatement)

