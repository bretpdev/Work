IF EXISTS(select null from sys.objects where object_id=OBJECT_ID('BorrowersSearch') and type in ('P', 'PC'))
begin
	drop procedure BorrowersSearch
end
GO
CREATE Procedure BorrowersSearch
	@FirstName nvarchar(max) = null,
	@LastName nvarchar(max) = null
AS

SELECT
	b.BF_SSN as SSN,
    b.DM_PRS_1 as FirstName, 
    b.DM_PRS_LST as LastName, 
    b.DM_PRS_MID as MiddleInitial,  
    b.DD_BRT as DOB, 
    d.DX_STR_ADR_1 as Address1, 
    d.DX_STR_ADR_2 as Address2, 
    d.DM_CT as City,
    d.DC_DOM_ST as StateCode, 
    d.DF_ZIP as Zip, 
    d.DN_PHN as HomePhone,
    case when d.DC_CEP = 'N' then 'N' else 'Y' end as HomePhoneConsent,
    '' as WorkPhone,
    'N' as WorkPhoneConsent,
    d.DN_ALT_PHN as AlternatePhone,
    case when d.DC_ALT_CEP = 'N' then 'N' else 'Y' end as AlternatePhoneConsent,
    d.DX_EML_ADR as HomeEmail,
    '' as WorkEmail,
    '' as AlternateEmail
 FROM
	PD10_PRS_NME PD10
 INNER JOIN
	PD30_PRS_ADR PD30
		ON PD10.DF_SPE_ACC_ID = PD30.DF_SPE_ACC_ID
 WHERE 
	(@FirstName IS NULL OR b.DM_PRS_1 LIKE @FirstName + '%') and
	(@LastName IS NULL OR b.DM_PRS_LST like @LastName + '%')


GO
