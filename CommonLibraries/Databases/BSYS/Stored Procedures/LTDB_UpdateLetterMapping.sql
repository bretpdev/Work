CREATE PROCEDURE [dbo].[LTDB_UpdateLetterMapping]
	@MappingId int,
	@HeaderTypeId int,
	@HeaderId int,
	@Order int,
	@User varchar(50),
	@Active bit
AS
	
	UPDATE
		LTDB_Letter_Header_Mapping
	SET
		HeaderTypeId = @HeaderTypeId,
		HeaderId = @HeaderId,
		[Order] = @Order,
		UpdatedBy = @User,
		UpdatedAt = GETDATE(),
		Active = @Active
	WHERE
		LetterHeaderMappingId = @MappingId
RETURN 0
