-- =============================================
-- Author:		Jarom Ryan
-- Create date: 03/06/2012
-- Description:	This sp will insert data in the CompassPifBwrLevel table
-- =============================================
CREATE PROCEDURE [dbo].[spCompassPifInsertBwrInfo]
	-- Add the parameters for the stored procedure here
	@AccountNumber  VARCHAR(10),
	@FirstName		VARCHAR(100),
	@LastName		VARCHAR(100),
	@Address1		VARCHAR(200),
	@Address2		VARCHAR(200),
	@City			VARCHAR(50),
	@State			VARCHAR(2),
	@Zip			VARCHAR(25),
	@Country		VARCHAR(150),
	@EffectiveDate	Date,
	@IsConsol		bit,
	@CostCenterCode VARCHAR(10)
	
AS
BEGIN

	SET NOCOUNT ON;
	if (SELECT Count(AccountNumber)
	FROM CompassPifBwrLevel
	WHERE AccountNumber = @AccountNumber) = 0
	
	Begin 
	
	Insert Into CompassPifBwrLevel (AccountNumber, FirstName, LastName, Address1, Address2, City, State, Zip, Country, EffectiveDate,ConsolPif,CostCenterCode)
	Values (@AccountNumber, @FirstName, @LastName,@Address1,@Address2,@City,@State,@Zip,@Country,@EffectiveDate,@IsConsol,@CostCenterCode) 
	END
END
