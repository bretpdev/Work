-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spUpdateEfsTempKey] 

AS
BEGIN

	SET NOCOUNT ON;

UPDATE dbo.GENR_DAT_EnterpriseFileSystem SET Path = 'T:\' WHERE [Key] = 'LOGS' AND Region = 'CornerStone' AND TestMode = 0
END
