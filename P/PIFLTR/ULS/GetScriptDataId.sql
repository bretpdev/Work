CREATE PROCEDURE [dbo].[GetScriptDataId]
@isConsol bit 
AS
	SELECT ScriptDataId
	FROM [ULS].[print].[ScriptData]
	WHERE ScriptId = 'PIFLTR'
	
RETURN 0
