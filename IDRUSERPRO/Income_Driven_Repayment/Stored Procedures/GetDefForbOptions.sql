CREATE PROCEDURE [dbo].[GetDefForbOptions]
	
AS
	SELECT 
		current_def_forb_option_Id AS DefForbId,
		current_def_forb_option AS DefForbOption
	FROM
		Current_Def_Forb_Options
RETURN 0