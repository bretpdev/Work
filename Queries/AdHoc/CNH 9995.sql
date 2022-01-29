--SELECT				
--	*			
--INTO				
--	_LegendData			
--FROM				
--	OPENQUERY			
--	(			
--		LEGEND,		
--		'		
--			SELECT	
--				PDXX.DF_SPE_ACC_ID,
--				LNXX.LN_SEQ,
--				LNXX.LF_DOE_SCL_ORG,
--				PDXX. DM_PRS_X || '' '' || PDXX.DM_PRS_LST AS Address,
--				PDXX.DX_STR_ADR_X,
--				PDXX.DX_STR_ADR_X,
--				PDXX.DM_CT,
--				PDXX.DC_DOM_ST,
--				PDXX.DF_ZIP_CDE,
--				PDXX.DM_FGN_CNY,
--				PDXX.DI_VLD_ADR,
--				PDXX.DC_ADR,
--				SDXX.LF_DOE_SCL_ENR_CUR,
--				SDXX.LD_SCL_SPR,
--				SDXX.LC_REA_SCL_SPR,
--				SDXX.LC_STA_STUXX
--			FROM	
--				PKUB.PDXX_PRS_NME PDXX
--				INNER JOIN PKUB.LNXX_LON LNXX ON LNXX.BF_SSN = PDXX.DF_PRS_ID
--				LEFT JOIN PKUB.PDXX_PRS_ADR PDXX ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
--				LEFT JOIN PKUB.SDXX_STU_SPR SDXX ON SDXX.LF_STU_SSN = PDXX.DF_PRS_ID AND SDXX.LC_STA_STUXX = ''A''
--		'		
--	) L
				

DECLARE @SchoolCodes TABLE				
(				
	SchoolCode CHAR(X)			
)				
				
INSERT INTO @SchoolCodes				
VALUES				
('XXXXXXX'),
('XXXXXXX'),
('XXXXXXX'),
('XXXXXXX'),
('XXXXXXX'),
('XXXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX'),
('XXXXXXXX')	

DECLARE @AddressPriority TABLE
(
	AddressType VARCHAR(X),
	AddressValidity CHAR(X),
	AddressPriority tinyint
)

INSERT INTO 
	@AddressPriority
VALUES
	('L', 'Y', X),
	('B', 'Y', X),
	('D', 'Y', X),
	('L', 'N', X),
	('B', 'N', X),
	('D', 'N', X)

UPDATE @SchoolCodes SET SchoolCode = RIGHT('XXXXXXXX' + LTRIM(RTRIM(SchoolCode)), X)				
	
SELECT DISTINCT
	*
FROM
	(			
		SELECT
			PDXX.DF_PRS_ID [SSN], -- was requested after the _LegendData populating query was completed (which takes XX minutes or so to run) so just joined on local PDXX to obtain			
			LD.*,
			ROW_NUMBER() OVER (PARTITION BY LD.DF_SPE_ACC_ID, LD.LN_SEQ ORDER BY AddressPriority) [AP]
		FROM 				
			_LegendData LD
			INNER JOIN CDW..PDXX_PRS_NME PDXX ON PDXX.DF_SPE_ACC_ID = LD.DF_SPE_ACC_ID			
			INNER JOIN @SchoolCodes SC ON (SC.SchoolCode = LD.LF_DOE_SCL_ORG OR SC.SchoolCode = LD.LF_DOE_SCL_ENR_CUR)
			LEFT JOIN @AddressPriority AP ON AP.AddressType = LD.DC_ADR AND AP.AddressValidity = AP.AddressValidity
	) X
WHERE
	X.AP = X

--SELECT
--	*
--FROM
--	_LegendData


