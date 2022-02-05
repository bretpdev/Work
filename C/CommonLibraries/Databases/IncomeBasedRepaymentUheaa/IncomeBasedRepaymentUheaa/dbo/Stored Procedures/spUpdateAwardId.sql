-- =============================================
-- Author:		JAROM RYAN
-- Create date: 05/29/2013
-- Description:	UPDATES THE AWARD ID FOR A GIVEN APPLICATION ID
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateAwardId]

@AwardId varchar(50),
@AppId int

AS
BEGIN
	
	UPDATE
		dbo.Applications
	SET
		award_id = @AwardId
	WHERE 
		application_id = @AppId
	
END
