SELECT * FROM OPENQUERY(LEGEND,
'
SELECT
	*
FROM
	PKUB.ADXX_PCV_ATY_ADJ
WHERE BF_SSN IN
	(''XXXXXXXXX'',
''XXXXXXXXX'',
''XXXXXXXXX'',
''XXXXXXXXX'',
''XXXXXXXXX'',
''XXXXXXXXX'',
''XXXXXXXXX'',
''XXXXXXXXX'',
''XXXXXXXXX'')


')