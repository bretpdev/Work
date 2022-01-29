
CREATE PROCEDURE [dbo].[spCentralizedPrintingCreateFaxRec]

@LID				VARCHAR(10),
@AcctNum			VARCHAR(20),
@BU					INT,
@FaxNum				VARCHAR(20),
@NewRecNum			BIGINT OUTPUT

AS

INSERT INTO dbo.CentralizedPrintingFax(LetterId,AccountNumber,Requested,BusinessUnitId,FaxNumber)
			VALUES (@LID, @AcctNum, GETDATE(),@BU,@FaxNum)

--get ID
SET @NewRecNum =  @@Identity

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingCreateFaxRec] TO [db_executor]
    AS [dbo];



