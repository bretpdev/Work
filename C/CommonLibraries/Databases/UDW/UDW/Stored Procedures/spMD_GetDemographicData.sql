/****** Object:  StoredProcedure [dbo].[spMD_GetDemographicData]    Script Date: 09/02/2016 09:19:11 ******/
CREATE  PROCEDURE [dbo].[spMD_GetDemographicData]
	@AccountNumber			varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @HasPendingDisbursementCancellation as bit = 0
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AD20_FinActAdjustment'))
	BEGIN
		set @HasPendingDisbursementCancellation = cast(case when exists(select * from dbo.AD20_FinActAdjustment FIN where FIN.DF_SPE_ACC_ID = @AccountNumber and LC_TYP_FAT_ADJ_REQ = '71') then 1 else 0 end as bit)
	END

	declare @HasSulaLoans bit = 0
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FS14_LoanSubsidy'))
	BEGIN
		set @HasSulaLoans = cast(case when exists(select * from dbo.FS14_LoanSubsidy where DF_SPE_ACC_ID = @AccountNumber and LC_INC_SUB_STA = 'L' and LC_STA_FS14 = 'A') then 1 else 0 end as bit)
	END

	declare @NeedsDeconArc as bit = 0
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DW01_Loan'))
	BEGIN
		set @NeedsDeconArc = cast(case when 
			(select count(*) from dbo.DW01_Loan where DF_SPE_ACC_ID = @AccountNumber and DW_LON_STA = 'DECONVERTED') = (select count(*) from dbo.DW01_Loan where DF_SPE_ACC_ID = @AccountNumber)
			then 1 else 0 end as bit)
	END

	declare @RelationshipStartDate as datetime = null
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LN10_LON'))
	BEGIN
	 select @RelationshipStartDate = min(lon.LD_LON_ACL_ADD)
	   from LN10_LON LON
	   join PD10_Borrower BOR on BOR.BF_SSN = LON.BF_SSN
	  where BOR.DF_SPE_ACC_ID = @AccountNumber
    END

	declare @EndProcessingAfterDemosPage as bit = 0
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LN10_LOAN'))
	BEGIN
	  --end processing if there are 'P' records and ONLY 'P' records
	  select @EndProcessingAfterDemosPage = 1
	    from LN10_LOAN LON
	   where LON.DF_SPE_ACC_ID = @AccountNumber and LON.LC_STA_LON10 = 'P'

	  select @EndProcessingAfterDemosPage = 0
	    from LN10_LOAN LON
	   where LON.DF_SPE_ACC_ID = @AccountNumber and LON.LC_STA_LON10 <> 'P'
	END

	SELECT
		--borrower info
		BORR.DF_SPE_ACC_ID AS AccountNumber,
		BORR.BF_SSN AS SSN,
		BORR.DM_PRS_1 AS FirstName,
		BORR.DM_PRS_LST AS LastName,
		BORR.DM_PRS_MID AS MI,
		CASE BORR.DM_PRS_MID
			WHEN '' THEN BORR.DM_PRS_1+' '+BORR.DM_PRS_LST
			ELSE BORR.DM_PRS_1+' '+BORR.DM_PRS_MID+' '+BORR.DM_PRS_LST
		END	AS FullName,
		BORR.DD_BRT AS DOB,
		--address info
		ADDR.DX_STR_ADR_1 AS Address1,
		ADDR.DX_STR_ADR_2 AS Address2,
		ADDR.DM_CT AS City,
		ADDR.DC_DOM_ST AS State,
		ADDR.DF_ZIP_CDE AS Zip,
		ADDR.DM_FGN_ST AS ForeignState,
		ADDR.DM_FGN_CNY AS Country,
		ADDR.DD_VER_ADR AS AddressVerifiedDate,
		ADDR.DI_VLD_ADR AS AddressValidityIndicator,
		--home phone info
		COALESCE(HPHN.LINE_TYP, '') AS HomePhoneMBLIndicator,
		COALESCE(HPHN.CONSENT_IND, '') AS HomePhoneConsentIndicator,
		COALESCE(HPHN.DD_PHN_VER, '') AS HomePhoneVerifiedDate,
		COALESCE(HPHN.DI_PHN_VLD, '') AS HomePhoneValidityIndicator,
		COALESCE(HPHN.DN_DOM_PHN_ARA+HPHN.DN_DOM_PHN_XCH+HPHN.DN_DOM_PHN_LCL, '') AS HomePhone,
		COALESCE(HPHN.DN_PHN_XTN, '') AS HomePhoneExtension,
		COALESCE(HPHN.DN_FGN_PHN_CNY, '') AS HomePhoneForeignCountry,
		COALESCE(HPHN.DN_FGN_PHN_CT, '') AS HomePhoneForeignCity,
		COALESCE(HPHN.DN_FGN_PHN_LCL, '') AS HomePhoneForeignLocalNumber,
		--alternate phone info
		COALESCE(APHN.LINE_TYP, '') AS AlternatePhoneMBLIndicator,
		COALESCE(APHN.CONSENT_IND, '') AS AlternatePhoneConsentIndicator,
		COALESCE(APHN.DD_PHN_VER, '') AS AlternatePhoneVerifiedDate,
		COALESCE(APHN.DI_PHN_VLD, '') AS AlternatePhoneValidityIndicator,
		COALESCE(APHN.DN_DOM_PHN_ARA+APHN.DN_DOM_PHN_XCH+APHN.DN_DOM_PHN_LCL, '') AS AlternatePhone,
		COALESCE(APHN.DN_PHN_XTN, '') AS AlternatePhoneExtension,
		COALESCE(APHN.DN_FGN_PHN_CNY, '') AS AlternatePhoneForeignCountry,
		COALESCE(APHN.DN_FGN_PHN_CT, '') AS AlternatePhoneForeignCity,
		COALESCE(APHN.DN_FGN_PHN_LCL, '') AS AlternatePhoneForeignLocalNumber,
		--work phone info
		COALESCE(WPHN.LINE_TYP, '') AS WorkPhoneMBLIndicator,
		COALESCE(WPHN.CONSENT_IND, '') AS WorkPhoneConsentIndicator,
		COALESCE(WPHN.DD_PHN_VER, '') AS WorkPhoneVerifiedDate,
		COALESCE(WPHN.DI_PHN_VLD, '') AS WorkPhoneValidityIndicator,
		COALESCE(WPHN.DN_DOM_PHN_ARA+WPHN.DN_DOM_PHN_XCH+WPHN.DN_DOM_PHN_LCL, '') AS WorkPhone,
		COALESCE(WPHN.DN_PHN_XTN, '') AS WorkPhoneExtension,
		COALESCE(WPHN.DN_FGN_PHN_CNY, '') AS WorkPhoneForeignCountry,
		COALESCE(WPHN.DN_FGN_PHN_CT, '') AS WorkPhoneForeignCity,
		COALESCE(WPHN.DN_FGN_PHN_LCL, '') AS WorkPhoneForeignLocalNumber,
		--mobile phone info
		COALESCE(MPHN.LINE_TYP, '') AS MobilePhoneMBLIndicator,
		COALESCE(MPHN.CONSENT_IND, '') AS MobilePhoneConsentIndicator,
		COALESCE(MPHN.DD_PHN_VER, '') AS MobilePhoneVerifiedDate,
		COALESCE(MPHN.DI_PHN_VLD, '') AS MobilePhoneValidityIndicator,
		COALESCE(MPHN.DN_DOM_PHN_ARA+MPHN.DN_DOM_PHN_XCH+MPHN.DN_DOM_PHN_LCL, '') AS MobilePhone,
		COALESCE(MPHN.DN_PHN_XTN, '') AS MobilePhoneExtension,
		COALESCE(MPHN.DN_FGN_PHN_CNY, '') AS MobilePhoneForeignCountry,
		COALESCE(MPHN.DN_FGN_PHN_CT, '') AS MobilePhoneForeignCity,
		COALESCE(MPHN.DN_FGN_PHN_LCL, '') AS MobilePhoneForeignLocalNumber,
		--home email info
		COALESCE(HEML.DX_ADR_EML, '') AS HomeEmail,
		COALESCE(HEML.DD_VER_ADR_EML, '') AS HomeEmailVerifiedDate,
		COALESCE(HEML.DI_VLD_ADR_EML, '') AS HomeEmailValidityIndicator,
		--alternate email info
		COALESCE(AEML.DX_ADR_EML, '') AS AlternateEmail,
		COALESCE(AEML.DD_VER_ADR_EML, '') AS AlternateEmailVerifiedDate,
		COALESCE(AEML.DI_VLD_ADR_EML, '') AS AlternateEmailValidityIndicator,
		--work email info
		COALESCE(WEML.DX_ADR_EML, '') AS WorkEmail,
		COALESCE(WEML.DD_VER_ADR_EML, '') AS WorkEmailVerifiedDate,
		COALESCE(WEML.DI_VLD_ADR_EML, '') AS WorkEmailValidityIndicator,
		--E-CORR Indicator
		CAST(CASE WHEN ECORR.DI_CNC_ELT_OPI = 'Y' THEN 1 ELSE 0 END AS BIT) as EcorrCorrespondence,
		CAST(CASE WHEN ECORR.DI_CNC_EBL_OPI = 'Y' THEN 1 ELSE 0 END AS BIT) as EcorrBilling,
		CAST(CASE WHEN ECORR.DI_CNC_TAX_OPI = 'Y' THEN 1 ELSE 0 END AS BIT) as EcorrTax,
		@HasPendingDisbursementCancellation as HasPendingDisbursementCancellation,
		@HasSulaLoans as HasSulaLoans,
		@NeedsDeconArc as NeedsDeconArc,
		@RelationshipStartDate as RelationshipStartDate,
		@EndProcessingAfterDemosPage EndProcessingAfterDemosPage
	FROM
		dbo.PD10_Borrower BORR 
		INNER JOIN dbo.PD30_Address ADDR ON
			BORR.DF_SPE_ACC_ID = ADDR.DF_SPE_ACC_ID
		LEFT OUTER JOIN (SELECT * FROM dbo.PD42_Phone WHERE DC_PHN = 'H') HPHN ON
			BORR.DF_SPE_ACC_ID = HPHN.DF_SPE_ACC_ID
		LEFT OUTER JOIN (SELECT * FROM dbo.PD42_Phone WHERE DC_PHN = 'A') APHN ON
			BORR.DF_SPE_ACC_ID = APHN.DF_SPE_ACC_ID
		LEFT OUTER JOIN (SELECT * FROM dbo.PD42_Phone WHERE DC_PHN = 'W') WPHN ON
			BORR.DF_SPE_ACC_ID = WPHN.DF_SPE_ACC_ID
		LEFT OUTER JOIN (SELECT * FROM dbo.PD42_Phone WHERE DC_PHN = 'M') MPHN ON
			BORR.DF_SPE_ACC_ID = MPHN.DF_SPE_ACC_ID
		LEFT OUTER JOIN (SELECT * FROM dbo.PD32_Email WHERE DC_ADR_EML = 'H') HEML ON
			BORR.DF_SPE_ACC_ID = HEML.DF_SPE_ACC_ID
		LEFT OUTER JOIN (SELECT * FROM dbo.PD32_Email WHERE DC_ADR_EML = 'A') AEML ON
			BORR.DF_SPE_ACC_ID = AEML.DF_SPE_ACC_ID
		LEFT OUTER JOIN (SELECT * FROM dbo.PD32_Email WHERE DC_ADR_EML = 'W') WEML ON
			BORR.DF_SPE_ACC_ID = WEML.DF_SPE_ACC_ID
		LEFT JOIN PH05_CNC_EML ECORR ON ECORR.DF_SPE_ID = BORR.DF_SPE_ACC_ID
	WHERE BORR.DF_SPE_ACC_ID = @AccountNumber
			
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetDemographicData] TO [UHEAA\Imaging Users]
    AS [dbo];
