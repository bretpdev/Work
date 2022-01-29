CREATE PROCEDURE [dbo].[InactiveDocumentDetailRecord]
    @DocumentDetailsId INT
AS
    
    UPDATE 
        [dbo].[DocumentDetails]
    SET
        [Active] = 0
    WHERE
        [DocumentDetailsId] = @DocumentDetailsId
RETURN 0
