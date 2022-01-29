
CREATE Procedure BorrowerSelect_Demographics
	(
       @AccountNumber char(10)
	)
AS
select addr.DX_STR_ADR_1 as Address1,
       addr.DX_STR_ADR_2 as Address2,
       addr.DM_CT as City,
       addr.DC_DOM_ST as State,
       addr.DF_ZIP_CDE as Zip,
       addr.DM_FGN_ST as ForeignState,
       addr.DM_FGN_CNY as Country,
       addr.DD_VER_ADR as AddressVerifiedDate,
       addr.DI_VLD_ADR as AddressIsValid,

       hp.LINE_TYP as HomePhoneMBLIndicator,
       hp.CONSENT_IND as HomePhoneHasConsent,
       hp.DD_PHN_VER as HomePhoneVerifiedDate,
       hp.DI_PHN_VLD as HomePhoneIsValid,
       hp.DN_DOM_PHN_ARA as HomePhoneAreaCode,
       hp.DN_DOM_PHN_XCH as HomePhoneExchange,
       hp.DN_DOM_PHN_LCL as HomePhoneLast4,
       hp.DN_PHN_XTN as HomePhoneExtension,
       hp.DN_FGN_PHN_CNY as HomePhoneForeignCountry,
       hp.DN_FGN_PHN_CT as HomePhoneForeignCity,
       hp.DN_FGN_PHN_LCL as HomePhoneForeignLocalNumber,
       --alternate phone info
       ap.LINE_TYP as AlternatePhoneMBLIndicator,
       ap.CONSENT_IND as AlternatePhoneHasConsent,
       ap.DD_PHN_VER as AlternatePhoneVerifiedDate,
       ap.DI_PHN_VLD as AlternatePhoneIsValid,
       ap.DN_DOM_PHN_ARA as AlternatePhoneAreaCode,
       ap.DN_DOM_PHN_XCH as AlternatePhoneExchange,
       ap.DN_DOM_PHN_LCL as AlternatePhoneLast4,
       ap.DN_PHN_XTN as AlternatePhoneExtension,
       ap.DN_FGN_PHN_CNY as AlternatePhoneForeignCountry,
       ap.DN_FGN_PHN_CT as AlternatePhoneForeignCity,
       ap.DN_FGN_PHN_LCL as AlternatePhoneForeignLocalNumber,
       --work phone info
       wp.LINE_TYP as WorkPhoneMBLIndicator,
       wp.CONSENT_IND as WorkPhoneHasConsent,
       wp.DD_PHN_VER as WorkPhoneVerifiedDate,
       wp.DI_PHN_VLD as WorkPhoneIsValid,
       wp.DN_DOM_PHN_ARA as WorkPhoneAreaCode,
       wp.DN_DOM_PHN_XCH as WorkPhoneExchange,
       wp.DN_DOM_PHN_LCL as WorkPhoneLast4,
       wp.DN_PHN_XTN as WorkPhoneExtension,
       wp.DN_FGN_PHN_CNY as WorkPhoneForeignCountry,
       wp.DN_FGN_PHN_CT as WorkPhoneForeignCity,
       wp.DN_FGN_PHN_LCL as WorkPhoneForeignLocalNumber,
       --home email info
       he.DX_ADR_EML as HomeEmail,
       he.DD_VER_ADR_EML as HomeEmailVerifiedDate,
       he.DI_VLD_ADR_EML as HomeEmailIsValid,
       --alternate email info
       ae.DX_ADR_EML as AlternateEmail,
       ae.DD_VER_ADR_EML as AlternateEmailVerifiedDate,
       ae.DI_VLD_ADR_EML as AlternateEmailIsValid,
       --work email info
       we.DX_ADR_EML as WorkEmail,
       we.DD_VER_ADR_EML as WorkEmailVerifiedDate,
       we.DI_VLD_ADR_EML as WorkEmailIsValid
  from dbo.PD30_Address addr 
  left join dbo.PD42_Phone hp on hp.DF_SPE_ACC_ID = addr.DF_SPE_ACC_ID and hp.DC_PHN = 'H'
  left join dbo.PD42_Phone ap on ap.DF_SPE_ACC_ID = addr.DF_SPE_ACC_ID and ap.DC_PHN = 'A'
  left join dbo.PD42_Phone wp on wp.DF_SPE_ACC_ID = addr.DF_SPE_ACC_ID and wp.DC_PHN = 'W'
  left join dbo.PD32_Email he on he.DF_SPE_ACC_ID = addr.DF_SPE_ACC_ID and he.DC_ADR_EML = 'H'
  left join dbo.PD32_Email ae on ae.DF_SPE_ACC_ID = addr.DF_SPE_ACC_ID and ae.DC_ADR_EML = 'A'
  left join dbo.PD32_Email we on we.DF_SPE_ACC_ID = addr.DF_SPE_ACC_ID and we.DC_ADR_EML = 'W'
 where addr.DF_SPE_ACC_ID = @AccountNumber