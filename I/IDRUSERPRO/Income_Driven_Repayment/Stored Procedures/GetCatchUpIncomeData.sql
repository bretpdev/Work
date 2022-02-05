CREATE PROCEDURE [dbo].[GetCatchUpIncomeData]
	@applicationId int
AS
	SELECT 
		CUI.Repaye_End_Date AS RepayeEndDate,
		CUI.Create_Date AS Create_Date,
		CUID.[Year] AS [Year],
		CUID.AGI AS AGI,
		CUID.[State] AS [State],
		CUID.Catch_Up_Family_Size AS FamilySize,
		CUID.Source_Code AS [Source],
		CUID.External_Debt AS ExternalDebt
	FROM
		Catch_Up_Income CUI
		INNER JOIN Catch_Up_Income_Data CUID
			ON CUID.Catch_Up_Income_Id = CUI.Catch_Up_Income_Id
	WHERE
		CUI.application_id = @applicationId
RETURN 0