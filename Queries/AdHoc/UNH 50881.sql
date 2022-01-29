USE BSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 2

	DELETE 
		RT 
	FROM 
		dbo.RTML_DAT_BarcodeData RT
		INNER JOIN dbo.LTDB_DAT_DOCDETAIL DD
			ON RT.LetterId = DD.ID
	WHERE
		--DD.DocName like '%fed%'
		--AND 
		RT.RecipientId IN 
		(
			'5171912430',
			'1793428646',
			'1676038887',
			'3906215195',
			'0064116515',
			'4492671655',
			'5153791434',
			'7161904064',
			'4773341088',
			'1971969215',
			'5167144349',
			'0064116515',
			'3023493986',
			'8294977616',
			'8294977616',
			'9238506532',
			'6498862533',
			'7064477780',
			'2818021623',
			'8503389408',
			'7920406887',
			'4748998736',
			'8100333829',
			'6847498597',
			'6847498597',
			'6847498597',
			'8503389408',
			'7477652978',
			'4350623958'
		)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
