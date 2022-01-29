-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/28/2013
-- Description:	Gets the attribites and values for a given tag for the ecorr xml file
-- =============================================
CREATE PROCEDURE [dbo].[GetAttributesAndValues]

@Tag varchar(100)

AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		TAV.Attribute,
		TAV.Value
		
	FROM
		[dbo].[Tags] T
	INNER JOIN [dbo].[TagAttributeValueMapping] TAVM
		ON TAVM.[TagId] = T.[TagId]
	INNER JOIN [dbo].[TagAttributeValues] TAV
		ON TAV.[TagAttributeValueId] = TAVM.[TagAttributeValueId]
	WHERE T.Tag = @Tag

END
