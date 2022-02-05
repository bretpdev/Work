-- =============================================
-- Author:		Eric Lynes
-- Create date: 1/23/2012
-- Description:	Split string
-- =============================================
CREATE FUNCTION [dbo].[SplitString] (@sep VARCHAR(32), @s VARCHAR(MAX))

RETURNS @t TABLE
    (
        val VARCHAR(MAX)
    )   
AS
    BEGIN
        DECLARE @xml XML
        SET @XML = N'<root><r>' + REPLACE(@s, @sep, '</r><r>') + '</r></root>'

        INSERT INTO @t(val)
        SELECT r.value('.','VARCHAR(100)') as Item
        FROM @xml.nodes('//root/r') AS RECORDS(r)

        RETURN
    END
