CREATE PROCEDURE [dbo].[GetAllBatchUsers]
	
AS
	SELECT DISTINCT
		L.UserName,
		dbo.Decryptor(L.EncrypedPassword) AS [Password]
	from
		[Login] L
		INNER JOIN [LoginType] LT
			ON LT.LoginTypeId = L.LoginTypeId
	WHERE
		LT.LoginType IN ('BatchCornerstone', 'BatchUheaa', 'SSCornerstone', 'SSUHEAA')
		AND DATEDIFF(DAY, L.LastUpdated, GETDATE()) != 0
	
		
RETURN 0
