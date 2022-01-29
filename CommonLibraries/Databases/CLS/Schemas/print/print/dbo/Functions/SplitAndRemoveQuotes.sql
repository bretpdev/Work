CREATE FUNCTION [dbo].[SplitAndRemoveQuotes]
(@originalValue NVARCHAR (MAX), @delimiter NVARCHAR (MAX), @index INT, @trim BIT)
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [CLS].[UserDefinedFunctions].[SplitAndRemoveQuotes]

