CREATE PROCEDURE [pridrcrp].[InsertToBorrowerInformation]
(
	@Ssn VARCHAR(9),
	@InterestRate DECIMAL(14,3),
	@FirstPayDue DATE,
	@PaymentAmount DECIMAL(14,2),
	@RepayPlan VARCHAR(50),
	@Page CHAR(1),
	@ZipFile VARCHAR(200)
)
AS

DECLARE @BorrowerInformationId INT = 
(
	SELECT 
		BorrowerInformationId 
	FROM 
		[pridrcrp].BorrowerInformation 
	WHERE 
		@Ssn = Ssn
		AND @InterestRate = InterestRate
		AND @FirstPayDue = FirstPayDue
		AND @PaymentAmount = PaymentAmount
		AND @RepayPlan = RepayPlan 
		AND @Page = [Page]
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL
)

IF @BorrowerInformationId IS NULL
	BEGIN
		INSERT INTO [pridrcrp].[BorrowerInformation]
				   ([Ssn]
				   ,[InterestRate]
				   ,[FirstPayDue]
				   ,[PaymentAmount]
				   ,[RepayPlan]
				   ,[Page]
				   ,[DeletedAt]
				   ,[DeletedBy]
				   ,[ZipFile])
			 VALUES
				   (@Ssn
				   ,@InterestRate
				   ,@FirstPayDue
				   ,@PaymentAmount
				   ,@RepayPlan
				   ,@Page
				   ,NULL
				   ,NULL
				   ,@ZipFile)

		SELECT  CAST(SCOPE_IDENTITY() AS INT)
	END
ELSE
	SELECT @BorrowerInformationId