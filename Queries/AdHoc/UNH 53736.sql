SELECT
	ID [Letter ID],
	DocName [Name]
FROM
	LTDB_DAT_DocDetail
WHERE
	Unit IN ('Loan Management','Postclaim Services')
	AND
	[Status] NOT IN ('Retired','Withdrawn')
	AND
	DocTyp = 'OneLINK'