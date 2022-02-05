CREATE PROCEDURE [dbo].[spCheckByPhone_SetFileName]
	@RecNo INT,
	@FileName VARCHAR(200)
AS
BEGIN    

	UPDATE 
		C
	SET
		C.[FileName] = @FileName
	FROM
		dbo.CheckByPhone C
	WHERE 
		C.RecNo = @RecNo

END