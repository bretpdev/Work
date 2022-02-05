-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE ThrowError

AS
BEGIN
	
	select [Key] from GENR_DAT_EnterpriseFileSystem where 1 = 0
	
	if @@ROWCOUNT = 0 RAISERROR ('error', 18, 1)
	
END