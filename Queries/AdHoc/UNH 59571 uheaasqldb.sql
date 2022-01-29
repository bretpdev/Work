USE BSYS

GO

DECLARE @ID VARCHAR(10) = 'US06IDTINB' --CHANGE THIS

DECLARE @LetterId INT = (SELECT [DocDetailId] FROM [dbo].[LTDB_DAT_DocDetail] WHERE ID = @ID)

INSERT INTO LTDB_SystemLettersStoredProcedures(LetterId, StoredProcedureName, ReturnTypeId)
VALUES(@LetterId, 'LT_Header_Footer', 1)