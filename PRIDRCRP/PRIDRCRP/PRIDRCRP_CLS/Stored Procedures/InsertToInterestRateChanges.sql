CREATE PROCEDURE [pridrcrp].[InsertToInterestRateChanges]
(
	@BorrowerInformationId INT,
	@BorrowerActivityId INT,
	@InterestRate DECIMAL(14,3),
	@EffectiveDate DATE = NULL
)
AS

DECLARE @ExistingRecord INT = (SELECT InterestRateChangeId FROM [pridrcrp].[InterestRateChanges] WHERE [BorrowerInformationId] = @BorrowerInformationId AND [BorrowerActivityId] = @BorrowerActivityId AND [InterestRate] = @InterestRate AND [EffectiveDate] = @EffectiveDate)

IF @ExistingRecord IS NULL
	BEGIN
		INSERT INTO [pridrcrp].[InterestRateChanges]
				   ([BorrowerInformationId]
				   ,[BorrowerActivityId]
				   ,[InterestRate]
				   ,[EffectiveDate])
			 VALUES
				   (@BorrowerInformationId,
				   @BorrowerActivityId,
				   @InterestRate,
				   @EffectiveDate)
	END
GO


