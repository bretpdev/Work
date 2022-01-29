CREATE PROCEDURE [dbo].[InactiveLetterRecord]
    @LetterId int
AS
    
    UPDATE
        [dbo].[Letters]
    SET
        [Active] = 0
    WHERE
        [LetterId] = @LetterId

RETURN 0
