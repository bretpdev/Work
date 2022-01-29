-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/14/2013
-- Description:	WILL GET ALL DATA NEEDED TO CREATE THE ab RECORDS IN THE NSLDS FILE.
-- =============================================
CREATE PROCEDURE [dbo].[spGetAbRecordData]

@StartDate DateTime = NULL,
@EndDate DateTime = NULL,
@AppId INT = NULL


AS
BEGIN

	SET NOCOUNT ON;

	IF(@AppId = -1) --Uses this when selecting records by date range and not application ID
		BEGIN 
			SELECT DISTINCT 
				APP.application_id AS ApplicationId,
				APP.award_id AS AwardId,
				ROL.person_role AS ABPersonRole,
				SPO.SSN AS ABSpouseSsn,
				SPO.birth_date AS ABSpouseDob,
				SPO.first_name AS ABSpouseFirstName,
				SPO.last_name AS ABSpouseLastName,
				SPO.middle_name AS ABSpouseMiddleName,
				ISNULL(SPO.pseudo_ssn,0) AS ABPersonsSsnIndicator,
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
				AND APP.updated_by != 'IDRXMLDATA'--has been imported but not reviewed
				AND SPO.SSN IS NOT NULL
		END
	ELSE
		BEGIN
			SELECT DISTINCT 
				APP.application_id AS AppId,
				APP.award_id AS AwardId,
				ROL.person_role AS ABPersonRole,
				SPO.SSN AS ABSpouseSsn,
				SPO.birth_date AS ABSpouseDob,
				SPO.first_name AS ABSpouseFirstName,
				SPO.last_name AS ABSpouseLastName,
				SPO.middle_name AS ABSpouseMiddleName,
				ISNULL(SPO.pseudo_ssn,0) AS ABPersonsSsnIndicator,
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
				APP.application_id = @AppId
				AND BE.substatus_mapping_id <> 13
				AND APP.Active = 1
				AND APP.updated_by != 'IDRXMLDATA'--has been imported but not reviewed
				AND SPO.SSN IS NOT NULL
		END
END