-- =============================================
-- Author:		Bret Pehrson
-- Create date: 2/14/2013
-- Description:	Returns a list of arguments associated with an application
-- =============================================
CREATE PROCEDURE [dbo].[spGetACDC_Arguments] 
	@Application_ID int
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		ARG.argument_id,
		ARG.argument,
		A.argument_order
	FROM
		APPLICATION_ARGUMENTS A
			JOIN ARGUMENTS ARG
			ON A.argument_id = ARG.argument_id
	WHERE
		A.application_id = @Application_ID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetACDC_Arguments] TO [db_executor]
    AS [dbo];

