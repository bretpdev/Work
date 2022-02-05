USE CDW
GO

--SELECT TOP 1
--	*
--INTO
--	AC11_ACT_REQ_LTR
--FROM
--	OPENQUERY
--	(
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.AC11_ACT_REQ_LTR AC11
--		'
--	) 

MERGE 
	dbo.AC11_ACT_REQ_LTR AC11
USING
	(
		SELECT
			*
		FROM
			OPENQUERY
			(
				LEGEND,
				'
					SELECT
						*
					FROM
						PKUB.AC11_ACT_REQ_LTR AC11
				'
			) 
	) L ON 	L.PF_REQ_ACT = AC11.PF_REQ_ACT
WHEN MATCHED THEN 
	UPDATE SET 
		AC11.PF_LTR = L.PF_LTR,
		AC11.PF_LST_DTS_AC11 = L.PF_LST_DTS_AC11,
		AC11.PI_BKR_EDS_SND_LTR = L.PI_BKR_EDS_SND_LTR,
		AC11.PI_BKR_ATN_SND_LTR = L.PI_BKR_ATN_SND_LTR,
		AC11.PI_BKR_LTR_OVR = L.PI_BKR_LTR_OVR
WHEN NOT MATCHED THEN
	INSERT 
	(
		PF_REQ_ACT,
		PF_LTR,
		PF_LST_DTS_AC11,
		PI_BKR_EDS_SND_LTR,
		PI_BKR_ATN_SND_LTR,
		PI_BKR_LTR_OVR
	)
	VALUES 
	(
		L.PF_REQ_ACT,
		L.PF_LTR,
		L.PF_LST_DTS_AC11,
		L.PI_BKR_EDS_SND_LTR,
		L.PI_BKR_ATN_SND_LTR,
		L.PI_BKR_LTR_OVR
	)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE
;


-- ###### VALIDATION
DECLARE 
	@CountDifference INT

SELECT
	@CountDifference = L.LocalCount - R.RemoteCount
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				PKUB.AC11_ACT_REQ_LTR
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			AC11_ACT_REQ_LTR
	) L ON 1 = 1
	
IF @CountDifference != 0
BEGIN
	RAISERROR('AC11_ACT_REQ_LTR - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
END
