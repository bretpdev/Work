CREATE PROCEDURE [pridrcrp].[InsertToPaymentAmountChanges]
(
	@BorrowerInformationId INT,
	@BorrowerActivityId INT,
	@PaymentAmount DECIMAL(14,2),
	@EffectiveDate DATE = NULL
)
AS

DECLARE @ExistingRecord INT = (SELECT PaymentAmountChangeId FROM [pridrcrp].[PaymentAmountChanges] WHERE [BorrowerInformationId] = @BorrowerInformationId AND [BorrowerActivityId] = @BorrowerActivityId AND [PaymentAmount] = @PaymentAmount AND [EffectiveDate] = @EffectiveDate)

IF @ExistingRecord IS NULL
	BEGIN
		INSERT INTO [pridrcrp].[PaymentAmountChanges]
				   ([BorrowerInformationId]
				   ,[BorrowerActivityId]
				   ,[PaymentAmount]
				   ,[EffectiveDate])
			 VALUES
				   (@BorrowerInformationId,
				   @BorrowerActivityId,
				   @PaymentAmount,
				   @EffectiveDate)

		--The purpose of this is to inactivate previous payment amounts 
		--that the current update should override
		UPDATE 
			PAC
		SET
			InactivatedAt = GETDATE(),
			InactivatedBy = CAST(@BorrowerActivityId AS VARCHAR(50))
		FROM
			[pridrcrp].[PaymentAmountChanges] PAC
			INNER JOIN pridrcrp.BorrowerActivityHistory BAH
				ON PAC.BorrowerActivityId = BAH.BorrowerActivityId
				AND PAC.BorrowerInformationId = BAH.BorrowerInformationId
			INNER JOIN pridrcrp.BorrowerActivityHistory CURRENT_BAH
				ON @BorrowerActivityId = CURRENT_BAH.BorrowerActivityId
				AND @BorrowerInformationId = CURRENT_BAH.BorrowerInformationId
		WHERE
			PAC.BorrowerActivityId != @BorrowerActivityId
			AND PAC.BorrowerInformationId = @BorrowerInformationId
			AND PAC.EffectiveDate >= @EffectiveDate
			AND BAH.ActivityDate <= CURRENT_BAH.ActivityDate
	END
GO