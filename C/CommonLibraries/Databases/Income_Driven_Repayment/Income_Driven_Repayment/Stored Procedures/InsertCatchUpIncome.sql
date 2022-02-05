CREATE PROCEDURE [dbo].[InsertCatchUpIncome]
	@ApplicationId INT,
	@RepayeEndDate DATETIME,
	@CatchUpIncomeData CatchUpIncome READONLY
AS
	DELETE CUID
	FROM Catch_Up_Income_Data CUID
	INNER JOIN Catch_Up_Income CUI
		ON CUI.Catch_Up_Income_Id = CUID.Catch_Up_Income_Id
	WHERE
		CUI.application_id = @ApplicationId

	DELETE FROM Catch_Up_Income
	WHERE application_id = @ApplicationId
	
	DECLARE @Catch_Up_Income_Id INT

	INSERT INTO Catch_Up_Income(application_id, Repaye_End_Date)
	VALUES(@ApplicationId, @RepayeEndDate)

	SET @Catch_Up_Income_Id = (SELECT SCOPE_IDENTITY())

	INSERT INTO Catch_Up_Income_Data(Catch_Up_Income_Id, [Year], AGI, [State], Catch_Up_Family_Size, Source_Code, External_Debt)
	SELECT
		@Catch_Up_Income_Id,
		CI.[Year],
		CI.AGI,
		CI.[State],
		CI.FamilySize,
		CI.[Source],
		CI.ExternalDebt
	FROM
		@CatchUpIncomeData CI

RETURN 0
