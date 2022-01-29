CREATE PROCEDURE [cpp].[InsertPaymentSource]
	@PaymentSource varchar(20),
	@InstitutionId varchar(6),
	@FileName varchar(25),
	@FileTypeId int

AS
	INSERT INTO PaymentSources(PaymentSource, InstitutionId, [FileName], FileTypeId)
	VALUES(@PaymentSource, @InstitutionId, @FileName, @FileTypeId)

RETURN 0

GRANT EXECUTE ON [cpp].[InsertPaymentSource] TO db_executor