CREATE PROCEDURE [dbo].[spMD_HasAnticipatedDisbursement]
	@AccountNumber char(10)
AS
	select cast(case when exists
	(
		SELECT 
			*
		FROM
			LN15_Disbursement
		WHERE
			DF_SPE_ACC_ID = @AccountNumber
			AND
			LC_DSB_TYP = 1
	) then 1 else 0 end as BIT) as HasAnticipatedDisbursement
	
RETURN 0
