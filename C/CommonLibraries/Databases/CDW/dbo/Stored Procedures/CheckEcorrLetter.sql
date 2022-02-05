CREATE PROCEDURE [dbo].[CheckEcorrLetter]
    @AccountNumber varchar(10)
AS
    SELECT 
        DF_SPE_ACC_ID AS AccountNumber,
        DX_CNC_EML_ADR as EmailAddress,
        DI_VLD_CNC_EML_ADR as ValidEmail
    FROM
        PH05_ContactEmail
    WHERE
        DF_SPE_ACC_ID = @AccountNumber
        AND DI_CNC_ELT_OPI = 1
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckEcorrLetter] TO [db_executor]
    AS [dbo];

