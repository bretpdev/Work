-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/14/2013
-- Description:	WILL GET ALL DATA NEEDED TO CREATE THE ab RECORDS IN THE NSLDS FILE.
-- =============================================
CREATE PROCEDURE [dbo].[spGetAbRecordData]

@StartDate DateTime = NULL,
@EndDate DateTime = NULL


AS
BEGIN

	SET NOCOUNT ON;

	SELECT DISTINCT 
		APP.application_id AS AppId,
		APP.award_id AS AwardId,
		ROL.person_role AS ABPersonRole,
		SPO.SSN AS ABSpouseSsn,
		SPO.birth_date AS ABSpouseDob,
		SPO.first_name AS ABSpouseFirstName,
		SPO.last_name AS ABSpouseLastName,
		SPO.middle_name AS ABSpouseMiddleName,
		SPO.pseudo_ssn AS ABPersonsSsnIndicator,
		SPO.driver_license_or_state_id AS ABDriversLicense,
		SPO.state_code AS ABDriversLicenseSt,
		ISNULL(LNS.loan_seq, '    ')  AS LoanSeq
		
	FROM 
		Applications APP
	LEFT JOIN Spouses SPO
		ON SPO.spouse_id = APP.spouse_id
	LEFT JOIN dbo.Person_Roles ROL
		ON ROL.person_role_id = SPO.person_role_id
	LEFT JOIN dbo.Loans LNS
		ON LNS.application_id = APP.application_id
		AND LNS.award_id = APP.award_id
	LEFT JOIN be_data_history BE on BE.application_id = app.application_id

	WHERE
		SPO.updated_at  BETWEEN @StartDate AND @EndDate
		AND BE.substatus_mapping_id <> 13
		AND APP.Active = 1
		AND (APP.created_by != 'IDRXMLDATA' AND app.updated_by != 'IDRXMLDATA')--has been imported but not reviewed
		
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetAbRecordData] TO [db_executor]
    AS [dbo];

