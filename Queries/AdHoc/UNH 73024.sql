UPDATE 
	udw..LT20_LTR_REQ_PRC 
SET 
	OnEcorr = 1 
WHERE 
	RM_DSC_LTR_PRC = 'US09B110CP' 
	AND ((PrintedAt IS NULL AND OnEcorr = 0) OR EcorrDocumentCreatedAt IS NULL) 
	AND InactivatedAt IS NULL

