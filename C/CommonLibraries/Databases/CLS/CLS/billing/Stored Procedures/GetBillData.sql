CREATE PROCEDURE billing.[GetBillData]
	

AS 
	SELECT
		BillDataIdId,
		SASFieldName,
		XCoord,
		YCoord,
		VertialAlign,
		HorizontalAlign,
		BD.FontTypeId,
		FT.FontType, 
		FT.EnumValue,
		FT.FontSize,
		FT.IsBold
	FROM
		billing.BillData BD
		INNER JOIN billing.FontType FT
			ON FT.FontTypeId = BD.FontTypeId

		
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[billing].[GetBillData] TO [db_executor]
    AS [dbo];

