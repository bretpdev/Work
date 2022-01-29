CREATE PROCEDURE [dbo].[GetFormAndPathForLetterId]
    @LetterId varchar(10)

AS
    SELECT 
        LetterId,
        (FormPath + Form) as PathAndForm
    FROM
        [dbo].[LetterFormMapping]
    WHERE
        LetterId = @LetterId
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFormAndPathForLetterId] TO [db_executor]
    AS [dbo];

