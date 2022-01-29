
BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		[NeedHelpUheaa].[dbo].[DAT_Ticket]
	SET
		Unit = '18'
	WHERE
		Ticket IN ('21897',
					'21906',
					'21925',
					'21943',
					'21955',
					'21966',
					'22023',
					'22032',
					'22117',
					'22129',
					'22140',
					'22155',
					'22174',
					'22204',
					'22226',
					'22237',
					'22312',
					'22363',
					'22404',
					'22410',
					'22413',
					'22433',
					'22441',
					'22475',
					'22601',
					'22619',
					'22627',
					'22696',
					'21882',
					'21979',
					'21999',
					'22005',
					'22048',
					'22056',
					'22079',
					'22108',
					'22188',
					'22254',
					'22328',
					'22380',
					'22640',
					'22686',
					'22396',
					'22459',
					'22491',
					'22508',
					'22531',
					'22542',
					'22568',
					'22588',
					'22665',
					'22705',
					'21861',
					'21915',
					'22273',
					'22290',
					'22302')

	-- Save/Set the row count and error number (if any) from the previously executed statement

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


IF @ROWCOUNT = 57 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
