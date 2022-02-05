CREATE PROCEDURE [dbo].[LTDB_InsertLetterMappingTable]
	@LetterId varchar(10),
	@HeaderTypeId int,
	@HeaderId int,
	@Order int,
	@User varchar(50)
AS
	DECLARE @ID INT = (SELECT DISTINCT DocDetailId FROM LTDB_DAT_DocDetail WHERE ID = @LetterId)

	INSERT INTO LTDB_Letter_Header_Mapping(LetterId, HeaderTypeId, HeaderId, [Order], CreatedBy)
	VALUES(@ID, @HeaderTypeId, @HeaderId, @Order, @User)
RETURN 0
