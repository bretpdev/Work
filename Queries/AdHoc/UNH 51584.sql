UPDATE 
	UDW..LT20_LTR_REQ_PRC
SET 
	PrintedAt = NULL
WHERE 
	RM_DSC_LTR_PRC = 'US06BTSA' 
	AND (datediff(day,PrintedAt,'2017-05-04')=0)