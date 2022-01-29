USE CDW
GO

DECLARE @StartDate DATE = 'XXXX-X-X'
DECLARE @EndDate DATE = GETDATE()

SELECT
	AYXX.BF_SSN,
	AYXX.PF_REQ_ACT,
	AYXX.LD_ATY_REQ_RCV
FROM
	CDW..AYXX_BR_LON_ATY AYXX
WHERE
	AYXX.PF_REQ_ACT IN ('BRRPF','XFORB','DDFRB','DIFRB','EHFRB','FVRXX','GVFRB','GXXXB','GXXXD','HDFRB','IRFRB','LDBFB','LSXXX','SCFRB','TFRFB','VBFRB','WRFXX','WRFXX','WRFXX','DIDFR','EMDFR','LSXXX','LSXXX','LSXXX','LSXXX','LSXXX','LSXXX','LSXXX','WRDXX','TBRDG')
	AND 
	AYXX.LD_ATY_REQ_RCV BETWEEN @StartDate AND @EndDate