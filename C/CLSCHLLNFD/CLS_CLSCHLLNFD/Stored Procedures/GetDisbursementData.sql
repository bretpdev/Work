CREATE PROCEDURE [clschllnfd].[GetDisbursementData]
	@BF_SSN CHAR(9),
	@LN_SEQ INT
AS
	SELECT
		ISNULL(LA_DSB, 0.00) - ISNULL(LA_DSB_CAN, 0.00) [DischargeAmount],
		LD_DSB [DischargeDate],
		LN_BR_DSB_SEQ [DisbursementSequence]
	FROM
		[CDW].[dbo].[LN15_DSB]
	WHERE
		BF_SSN = @BF_SSN
		AND LN_SEQ = @LN_SEQ
		AND LC_DSB_TYP = 2
		AND LC_STA_LON15 = 1
RETURN 0