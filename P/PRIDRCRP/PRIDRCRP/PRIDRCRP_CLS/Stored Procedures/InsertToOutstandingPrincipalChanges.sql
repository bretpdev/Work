CREATE PROCEDURE [pridrcrp].[InsertToOutstandingPrincipalChanges]
(
	@BorrowerInformationId INT,
	@BorrowerActivityId INT,
	@OutstandingPrincipal DECIMAL(14,2),
	@EffectiveDate DATE = NULL
)
AS

DECLARE @ExistingRecord INT = (SELECT OutstandingPrincipalChangeId FROM [pridrcrp].[OutstandingPrincipalChanges] WHERE [BorrowerInformationId] = @BorrowerInformationId AND [BorrowerActivityId] = @BorrowerActivityId AND [OutstandingPrincipal] = @OutstandingPrincipal AND [EffectiveDate] = @EffectiveDate)

IF @ExistingRecord IS NULL
	BEGIN
		INSERT INTO [pridrcrp].[OutstandingPrincipalChanges]
				   ([BorrowerInformationId]
				   ,[BorrowerActivityId]
				   ,[OutstandingPrincipal]
				   ,[EffectiveDate])
			 VALUES
				   (@BorrowerInformationId,
				   @BorrowerActivityId,
				   @OutstandingPrincipal,
				   @EffectiveDate)
	END
GO


