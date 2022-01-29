-- =============================================
-- Author:		Bret Pehrson
-- Create date: 2/14/2013
-- Description:	Returns a list of all the associated files for a given application
-- =============================================
CREATE PROCEDURE [dbo].[spGetACDC_AssociatedFiles] 
	@Application_ID int
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		AF.source_path,
		AF.destination_path,
		FT.file_type,
		AF.associated_file_id,
		AF.associated_file_name
	FROM
		ASSOCIATED_FILES AF
			JOIN FILE_TYPE FT
			ON AF.file_type_id = FT.file_type_id
	WHERE
		AF.application_id = @Application_ID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetACDC_AssociatedFiles] TO [db_executor]
    AS [dbo];

