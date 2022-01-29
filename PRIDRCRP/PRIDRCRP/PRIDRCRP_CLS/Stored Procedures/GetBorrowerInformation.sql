CREATE PROCEDURE [pridrcrp].[GetBorrowerInformation]
(
	@BorrowerInformationId INT
)
AS

SELECT 
    [Ssn],
    [InterestRate],
    [FirstPayDue],
    [PaymentAmount],
    [RepayPlan],
    [Page]
 FROM 
	[pridrcrp].[BorrowerInformation]
WHERE
	[BorrowerInformationId] = @BorrowerInformationId
	AND [DeletedAt] IS NULL
	AND [DeletedBy] IS NULL
GO


