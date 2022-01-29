CREATE PROCEDURE [dbo].[NullPrinted]
	 @DocumentDetailsId int
AS
	UPDATE 
		EcorrUheaa..[DocumentDetails] 
	SET 
		Printed = NULL 
	WHERE 
		DocumentDetailsId = @DocumentDetailsId
RETURN 0
