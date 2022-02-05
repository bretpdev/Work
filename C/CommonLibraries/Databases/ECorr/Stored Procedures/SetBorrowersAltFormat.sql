CREATE PROCEDURE [dbo].[SetBorrowersAltFormat]
	@AccountNumber char(10),
	@Format int
AS
	if not exists(select * from BorrowerCorrespondenceFormats where AccountNumber = @AccountNumber)
	begin
		insert into BorrowerCorrespondenceFormats (AccountNumber, CorrespondenceFormatId)
		values (@AccountNumber, @Format)
	end

	update BorrowerCorrespondenceFormats
	   set CorrespondenceFormatId = @Format
	 where AccountNumber = @AccountNumber

RETURN 0
