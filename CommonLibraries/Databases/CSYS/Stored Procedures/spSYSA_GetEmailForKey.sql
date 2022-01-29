

CREATE PROCEDURE [dbo].[spSYSA_GetEmailForKey] 
                @Key       VARCHAR(100),
                @BU        INT = NULL
AS
BEGIN
                -- SET NOCOUNT ON added to prevent extra result sets from
                -- interfering with SELECT statements.
                SET NOCOUNT ON;

    SELECT DISTINCT U.EMail
    FROM   SYSA_DAT_Users U
    INNER JOIN SYSA_DAT_UserKeyAssignment A
    ON U.SqlUserId = A.SqlUserId
    WHERE A.UserKey = @Key 
    AND (@BU IS NULL OR A.BusinessUnit = @BU)
    AND A.EndDate IS NULL

END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetEmailForKey] TO [db_executor]
    AS [dbo];

