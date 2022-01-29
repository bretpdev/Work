CREATE PROCEDURE [imgclimprt].[GetDocIdFromDocumentType]
	@DocumentTypeValue varchar(50)
AS
	select 
		di.DocIdValue
	from
		[imgclimprt].DocIds di
	join
		[imgclimprt].DocumentTypes dt on dt.DocIdId = di.DocIdId
	where
		dt.DocumentTypeValue = @DocumentTypeValue
RETURN 0
