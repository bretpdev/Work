-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/16/2012
-- Description:	Will return the Script Name based on the scriptId
-- =============================================
CREATE PROCEDURE [dbo].[spGetScriptName] 

@scriptId as Varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT Script
   FROM SCKR_DAT_Scripts WHERE ID = @ScriptId
END