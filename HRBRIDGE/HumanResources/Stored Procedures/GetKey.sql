CREATE PROCEDURE [hrbridge].[GetKey]
	@KeyName VARCHAR(100)
AS
	SELECT
		[Key],
		[Secret]
	FROM	
		hrbrdige.Keys
	WHERE
		KeyName = @KeyName
