CREATE PROCEDURE [dbo].[NullPrinted]
	 @DocumentDetailsId int
AS
	UPDATE 
		ECorrFed..[DocumentDetails] 
	SET 
		Printed = NULL 
	WHERE 
		DocumentDetailsId = @DocumentDetailsId
RETURN 0
