DECLARE @SSNS TABLE (SSN VARCHAR(X), LN_SEQ INT);

INSERT INTO @SSNS (SSN, LN_SEQ)
VALUES
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X),
('XXXXXXXXX',	X)
;

--SELECT * FROM @SSNS

SELECT
	BF_SSN,
    LNXX.LN_SEQ,
    LF_FOR_CTL_NUM,
    LN_FOR_OCC_SEQ,
    LC_FOR_RSP,
    CAST(LD_FOR_BEG AS DATE) AS LD_FOR_BEG,
    CAST(LD_FOR_END AS DATE) AS LD_FXR_END,
    CAST(LD_STA_LONXX AS DATE) AS LD_STA_LONXX,
    LC_STA_LONXX,
    CAST(LD_FOR_APL AS DATE) AS LD_FOR_APL,
    LF_LST_DTS_LNXX,
    LI_FOR_XX_RPT,
    LC_LON_LEV_FOR_CAP,
    LA_FOR_XX_INT_ACR,
    LA_ACL_RDC_PAY,
    LI_FOR_VRB_DFL_RUL
FROM
	@SSNS SSNS
	INNER JOIN CDW..LNXX_BR_FOR_APV LNXX
		ON LNXX.BF_SSN = SSNS.SSN
		AND LNXX.LN_SEQ = SSNS.LN_SEQ