CREATE PROCEDURE [dbo].[EncryptString]
    @text nvarchar(max)
AS
BEGIN
    SELECT dbo.Encrypt(@text)
END
