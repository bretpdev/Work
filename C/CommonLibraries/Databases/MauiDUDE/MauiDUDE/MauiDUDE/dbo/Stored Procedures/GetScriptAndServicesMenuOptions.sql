CREATE PROCEDURE [dbo].[GetScriptAndServicesMenuOptions]
	@HomePage varchar(200),
	@ParentMenu varchar(200)
AS
	SELECT 
		ScriptID,
		InternalOrExternal,
		ParentMenu,
		DisplayName,
		SubToBeCalled,
		ToBeCalledImm,
		ToBeCalledAtEnd,
		HomePage,
		DataForFunctionCall,
		CallForNoteDUDECleanUp,
		CompletionFile,
		DisableKey,
		DLLToLoad,
		DLLsToCopy,
		ObjectToCreate 
	FROM 
		MenuOptionsScriptsAndServices 
	WHERE 
		HomePage = @HomePage
		AND ParentMenu = @ParentMenu
RETURN 0
