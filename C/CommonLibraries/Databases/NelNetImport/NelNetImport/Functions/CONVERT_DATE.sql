CREATE FUNCTION [dbo].[CONVERT_DATE](@nelnet_date CHAR(7))
	RETURNS DATE
	AS
BEGIN
--Nelnet date format:  CYYMMDD
--DECLARE @nelnet_date varchar(7) = '0400101'
--DECLARE @nelnet_date varchar(7) = '0000000'
	DECLARE @Century CHAR(2)
	DECLARE @year CHAR(2)
	DECLARE @Month CHAR(2)
	DECLARE @Day CHAR(2)

	SELECT 
		@Century =
			CASE 
				LEFT(@nelnet_date, 1)
				WHEN  '0' THEN
					'19'
				ELSE
					'20'
			END

	SELECT @Year = SUBSTRING(@nelnet_date, 2,2)
	SELECT @Month = SUBSTRING(@nelnet_date, 4,2)
	SELECT @Day = SUBSTRING(@nelnet_date, 6,2)

	SELECT @Century = NULL, @Year = NULL, @Month = NULL, @Day = NULL WHERE @nelnet_date in ('0000000', '0000001', '1000000', '       ')-- OR @nelnet_date < '0600000'

	
	RETURN CAST(@Century + @YEAR + @Month + @Day as DATE)

END;
