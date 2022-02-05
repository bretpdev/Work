CREATE PROCEDURE [dbo].[UpdateSpouseXmlData]
	@SpouseId INT,
	@Separated BIT,
	@AccessToIncomeInfo BIT
AS
	UPDATE 
		S
	SET
		S.separated_from_spouse = @Separated,
		S.access_spouse_income_info = @AccessToIncomeInfo,
		S.updated_at = GETDATE()
	FROM
		dbo.Spouses S
	WHERE
		S.spouse_id = @SpouseId
	
RETURN 0