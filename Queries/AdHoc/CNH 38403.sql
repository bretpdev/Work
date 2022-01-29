SELECT
	*
FROM
	OPENQUERY(LEGEND,
		'
			SELECT
				*
			FROM
				PKUB.LNXX_LON_RFD LNXX
			WHERE
				LNXX.LD_FAT_EFF BETWEEN ''X/X/XXXX'' AND ''X/XX/XXXX''
				OR LNXX.LD_RFD_CAN_SCH BETWEEN ''X/X/XXXX'' AND ''X/XX/XXXX''
		'
	)

SELECT
	*
FROM
	OPENQUERY(LEGEND,
		'
			SELECT
				*
			FROM
				PKUB.RMXX_RMT_SPS_RFD RMXX
			WHERE
				RMXX.LD_STA_REMTXX BETWEEN ''X/X/XXXX'' AND ''X/XX/XXXX''
				OR RMXX.LD_SPS_CAN_SCH BETWEEN ''X/X/XXXX'' AND ''X/XX/XXXX''
		'
	)