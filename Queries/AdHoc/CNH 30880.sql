SELECT * FROM OPENQUERY(LEGEND,
'
	SELECT 
		*
	FROM
		PKUB.LNXX_LON
	WHERE
		LF_DOE_SCL_ORG like ''XXXXXX%''
'
)