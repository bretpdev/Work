CREATE Procedure BorrowersSearch
	@FirstName nvarchar(max) = null,
	@LastName nvarchar(max) = null,
	@MiddleInitial nvarchar(1) = null,
	@DOB nvarchar(max) = null,
	@Address nvarchar(max) = null,
	@City nvarchar(max) = null,
	@StateCode nvarchar(2) = null,
	@Zip nvarchar(max) = null,
	@Phone nvarchar(max) = null,
	@Email nvarchar(max) = null
AS

declare @MatchingSSNs table (SSN nvarchar(max))
insert into @MatchingSSNs
select b.BF_SSN as SSN
  from PD10_Borrower b
 inner join PD30_Address a on b.DF_SPE_ACC_ID = a.DF_SPE_ACC_ID
  left join PD32_Email e on e.DF_SPE_ACC_ID = a.DF_SPE_ACC_ID
  left join PD42_Phone p on p.DF_SPE_ACC_ID = a.DF_SPE_ACC_ID
 where (@FirstName is null or b.DM_PRS_1 like @FirstName) and
	   (@LastName is null or b.DM_PRS_LST like @LastName) and
	   (@MiddleInitial is null or b.DM_PRS_MID like @MiddleInitial) and
	   (@DOB is null or b.DD_BRT like @DOB) and
	   (@Address is null or a.DX_STR_ADR_1 like @Address or a.DX_STR_ADR_2 like @Address) and
	   (@City is null or a.DM_CT like @City) and
	   (@StateCode is null or a.DC_DOM_ST like @StateCode) and
	   (@Zip is null or a.DF_ZIP_CDE like @Zip) and
       (@Phone is null or p.DN_DOM_PHN_ARA + p.DN_DOM_PHN_XCH + p.DN_DOM_PHN_LCL like @Phone or DN_FGN_PHN_CNY + DN_FGN_PHN_CT + DN_FGN_PHN_LCL like @Phone) and
	   (@Email is null or e.DX_ADR_EML like @Email)

select b.BF_SSN as SSN, 
       MAX(b.DM_PRS_1) as FirstName, 
       MAX(b.DM_PRS_LST) as LastName, 
       MAX(b.DM_PRS_MID) as MiddleInitial,  
       MAX(b.DD_BRT) as DOB, 
       MAX(a.DX_STR_ADR_1) as Address1, 
       MAX(a.DX_STR_ADR_2) as Address2, 
       MAX(a.DM_CT) as City,
       MAX(a.DC_DOM_ST) as StateCode, 
       MAX(a.DF_ZIP_CDE) as Zip, 
	   MAX(case when p.DC_PHN = 'H' then isnull(nullif(p.DN_DOM_PHN_ARA + ' ' + p.DN_DOM_PHN_XCH + ' ' + p.DN_DOM_PHN_LCL, '  '), nullif(DN_FGN_PHN_CNY + ' ' + DN_FGN_PHN_CT + ' ' + DN_FGN_PHN_LCL, '  ')) end) as HomePhone,
	   MAX(case when p.DC_PHN = 'H' then p.CONSENT_IND end) as HomePhoneConsent,
	   MAX(case when p.DC_PHN = 'W' then isnull(nullif(p.DN_DOM_PHN_ARA + ' ' + p.DN_DOM_PHN_XCH + ' ' + p.DN_DOM_PHN_LCL, '  '), nullif(DN_FGN_PHN_CNY + ' ' + DN_FGN_PHN_CT + ' ' + DN_FGN_PHN_LCL, '  ')) end) as WorkPhone,
	   MAX(case when p.DC_PHN = 'W' then p.CONSENT_IND end) as WorkPhoneConsent,
	   MAX(case when p.DC_PHN = 'A' then isnull(nullif(p.DN_DOM_PHN_ARA + ' ' + p.DN_DOM_PHN_XCH + ' ' + p.DN_DOM_PHN_LCL, '  '), nullif(DN_FGN_PHN_CNY + ' ' + DN_FGN_PHN_CT + ' ' + DN_FGN_PHN_LCL, '  ')) end) as AlternatePhone,
	   MAX(case when p.DC_PHN = 'A' then p.CONSENT_IND end) as AlternatePhoneConsent,
       MAX(case when e.DC_ADR_EML = 'H' then e.DX_ADR_EML end) as HomeEmail,
       MAX(case when e.DC_ADR_EML = 'W' then e.DX_ADR_EML end) as WorkEmail,
       MAX(case when e.DC_ADR_EML = 'A' then e.DX_ADR_EML end) as AlternateEmail
  from PD10_Borrower b
 inner join @MatchingSSNs s on s.SSN = b.BF_SSN
 inner join PD30_Address a on b.DF_SPE_ACC_ID = a.DF_SPE_ACC_ID
  left join PD32_Email e on e.DF_SPE_ACC_ID = a.DF_SPE_ACC_ID
  left join PD42_Phone p on p.DF_SPE_ACC_ID = a.DF_SPE_ACC_ID
 group by b.BF_SSN