CREATE PROCEDURE mssasgndft.GetQueueCount
	@Queue VARCHAR(8),
	@FutureDated BIT = 0
AS

	DECLARE @Today DATE = CAST(GETDATE() AS DATE)

	SELECT
		COUNT(*)
	FROM
		ODW..CT30_CALL_QUE
	WHERE
		IC_REC_TYP = 'DFT'
		AND IF_WRK_GRP = @Queue
		AND IC_TSK_STA = 'A'
		AND
		(
			(LD_LST_WRK <= @Today AND @FutureDated = 0)
			OR
			(LD_LST_WRK > @Today AND @FutureDated = 1)
		)