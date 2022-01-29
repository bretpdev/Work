USE UDW
GO

DECLARE @BanaLoan VARCHAR(10) = '829769' --loans from BANA conversion

SELECT
	LN10.BF_SSN,
	LN10.LN_SEQ,
	LN10.LD_EFF_LBR_RTE 'current', --LIBOR date
	LN10.LD_LON_EFF_ADD 'LN10.LD_EFF_LBR_RTE (new)' --effective load add date
FROM
	[dbo].[LN10_LON] LN10
WHERE
	LN10.LF_LON_CUR_OWN = @BanaLoan
	AND ISNULL(LD_EFF_LBR_RTE, '') = '' --no LIBOR date


BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE 
		[dbo].[LN10_LON]
	SET 
		LD_EFF_LBR_RTE = LD_LON_EFF_ADD
	WHERE
		LF_LON_CUR_OWN = @BanaLoan
		AND ISNULL(LD_EFF_LBR_RTE, '') = '' --no LIBOR date

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 0 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		--COMMIT TRANSACTION
		ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
