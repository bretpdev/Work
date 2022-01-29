
-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/27/2013
-- Description:	WILL GATHER ALL OF THE ARCS FROM dbo.Phone_Consent_Arcs
-- =============================================
CREATE PROCEDURE [dbo].[spGetConsentArcs] 


AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		arc AS Arc,
		endorser As Endorser
	FROM 
		dbo.Phone_Consent_Arcs
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetConsentArcs] TO [db_executor]
    AS [dbo];



