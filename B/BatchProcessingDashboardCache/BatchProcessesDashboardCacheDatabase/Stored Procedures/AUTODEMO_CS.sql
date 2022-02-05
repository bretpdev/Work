CREATE PROCEDURE [dbo].[AUTODEMO_CS]

AS
	SELECT
		SUM(UnworkedCount)
	FROM
		OPENQUERY
		(
			LEGEND,
			'
				SELECT
					COUNT(*) "UnworkedCount"
				FROM
					PKUB.WQ20_TSK_QUE WQ20
				WHERE
					WQ20.WF_QUE IN (''EX'', ''M1'', ''LX'', ''MT'')
					AND
					WQ20.WF_SUB_QUE = ''01''
					AND
					WQ20.WC_STA_WQUE20 = ''U''
					AND
					DAYS(WQ20.WD_ACT_REQ) < DAYS(CURRENT DATE) -4
			'
		)
RETURN 0
