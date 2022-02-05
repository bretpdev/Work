CREATE PROCEDURE [cpp].[UpdatePaymentSource]
	@PaymentSourceId int,
	@InstitutionId varchar(6),
	@FileName varchar(25),
	@FileTypeId int
AS

	UPDATE
		PaymentSources
	SET
		InstitutionId = @InstitutionId,
		[FileName] = @FileName,
		FileTypeId = @FileTypeId
	WHERE
		PaymentSourcesId = @PaymentSourceId

RETURN 0

GRANT EXECUTE ON [cpp].[UpdatePaymentSource] TO db_executor