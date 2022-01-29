CREATE PROCEDURE [pretnfrnot].[GetPreTransferParagraph]
	@PreTransferParagraphId int
AS

	SELECT
		Paragraph
	FROM
		PreTransferParagraph
	WHERE
		PreTransferParagraphId = @PreTransferParagraphId

RETURN 0
