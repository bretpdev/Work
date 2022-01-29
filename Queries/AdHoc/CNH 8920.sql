UPDATE
	NobleCalls.dbo.NobleCallHistory
SET deleted = X, deletedby = 'DCR', deletedat = GETDATE()
where CallType = X and (AccountIdentifier is null or AccountIdentifier ='')
--XXXX rows should be affected.