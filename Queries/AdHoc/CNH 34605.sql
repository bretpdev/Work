

	SELECT
		*
	FROM
		CDW..LNXX_FIN_ATY
	WHERE
		PC_FAT_TYP IN ('XX','XX','XX')
		AND PC_FAT_SUB_TYP IN ('XX','XX','XX')
		AND LD_FAT_APL = 'XX/XX/XXXX'