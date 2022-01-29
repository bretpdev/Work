CREATE FUNCTION [dbo].[CONVERT_DEFFORB_DATE](@nelnet_date CHAR(7))
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
	DECLARE @ReturnDate DATE

	SELECT 
		@Century =
			CASE 
				WHEN SUBSTRING(@nelnet_date, 2,2) < 80 THEN
					'20'
				ELSE
					'19'
			END

	SELECT @Year = SUBSTRING(@nelnet_date, 2,2)
	SELECT @Month = SUBSTRING(@nelnet_date, 4,2)
	SELECT @Day = SUBSTRING(@nelnet_date, 6,2)

	SELECT 
		@ReturnDate = 
			CASE 
				WHEN ISDATE(@Century + @YEAR + '-' + @Month + '-' + @Day) = 1 THEN CAST(@Century + @YEAR + '-' + @Month + '-' + @Day as DATE)
				ELSE NULL
			END
		
	RETURN @ReturnDate

END
