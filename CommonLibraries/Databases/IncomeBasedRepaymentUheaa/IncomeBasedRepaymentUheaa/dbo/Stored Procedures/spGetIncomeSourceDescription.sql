-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/17/2013
-- Description:	WILL GET THE INCODE SOUCE DESCRIPTION FOR A GIVEN ID
-- =============================================
CREATE PROCEDURE [dbo].[spGetIncomeSourceDescription]

@Id INT

AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		income_source_description
	FROM
		dbo.Income_Source
	WHERE income_source_id = @Id
END
