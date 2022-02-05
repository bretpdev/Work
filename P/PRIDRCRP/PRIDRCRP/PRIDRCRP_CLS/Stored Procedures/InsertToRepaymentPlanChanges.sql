CREATE PROCEDURE [pridrcrp].[InsertToRepaymentPlanChanges]
	@BorrowerInformationId INT,
	@BorrowerActivityId INT NULL,
	@PlanType VARCHAR(MAX),
	@EffectiveDate DATE = NULL
AS

DECLARE @ExistingRecord INT = (SELECT RepaymentPlanChangeId FROM [pridrcrp].[RepaymentPlanChanges] WHERE [BorrowerInformationId] = @BorrowerInformationId AND [BorrowerActivityId] = @BorrowerActivityId AND [PlanType] = @PlanType AND [EffectiveDate] = @EffectiveDate)

IF @ExistingRecord IS NULL
	BEGIN
		INSERT INTO [pridrcrp].[RepaymentPlanChanges](BorrowerInformationId, BorrowerActivityId, PlanType, EffectiveDate)
		VALUES
		(
			@BorrowerInformationId,
			@BorrowerActivityId,
			@PlanType,
			@EffectiveDate
		)
	END
GO

