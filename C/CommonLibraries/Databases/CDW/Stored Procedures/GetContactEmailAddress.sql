CREATE PROCEDURE [dbo].[GetContactEmailAddress]
    @AccountNumber varchar(10)

AS
    SELECT 
        DX_CNC_EML_ADR
    FROM
        PH05_ContactEmail
    WHERE
        DF_SPE_ACC_ID = @AccountNumber
RETURN 0
