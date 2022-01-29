UPDATE
	PP
SET
	OnEcorr = CASE
				WHEN PHXX.[DI_CNC_EBL_OPI ] = X AND PHXX.DI_VLD_CNC_EML_ADR = X THEN X
				ELSE X
			  END
FROM
	[CLS].[print].[PrintProcessing] PP
	JOIN [CDW]..PHXX_ContactEmail PHXX ON pp.AccountNumber = PHXX.DF_SPE_ACC_ID
WHERE
	CAST(AddedAt AS DATE) = 'X/X/XX'