﻿CREATE FUNCTION [schrpt].[FormatMoney]
(
    @InputValue NUMERIC(20,2)
)
RETURNS VARCHAR(12)
AS
BEGIN
	RETURN CASE WHEN RIGHT(CAST(@InputValue AS VARCHAR), 3) = '.00' 
	THEN SUBSTRING(CAST(@InputValue AS VARCHAR),0,CHARINDEX('.',CAST(@InputValue AS VARCHAR),0)) 
	ELSE REPLACE(CAST(CONVERT(DECIMAL(11,2),@InputValue) AS VARCHAR),'.','') END
END