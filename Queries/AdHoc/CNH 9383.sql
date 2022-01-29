SELECT * FROM OPENQUERY (LEGEND,'

	SELECT
		BF_SSN
		,LN_SEQ
		,LD_LON_GTR
		,LF_DOE_SCL_ORG
	FROM
		PKUB.LNXX_LON
	WHERE
		LF_DOE_SCL_ORG IN 
		(
			 ''XXXXXXXX'' 
			,''XXXXXXXX'' 
			,''XXXXXXXX'' 
			,''XXXXXXXX'' 
			,''XXXXXXXX'' 
			,''XXXXXXXX'' 
			,''XXXXXXXX'' 
			,''XXXXXXXX'' 
		)

');