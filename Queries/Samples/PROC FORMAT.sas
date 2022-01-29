PROC FORMAT;
	VALUE $LONSTA
		'01' = 'IN GRACE'	
		'02' = 'IN SCHOOL'
		'03' = 'IN REPAYMENT'
		'04' = 'IN DEFERMENT'	
		'05' = 'IN FORBEARANCE'	
		'06' = 'IN CURE'	
		'07' = 'CLAIM PENDING'	
		'08' = 'CLAIM SUBMITTED'	
		'09' = 'CLAIM CANCELLED'		
		'10' = 'CLAIM REJECTED'	
		'11' = 'CLAIM RETURNED'	
		'12' = 'CLAIM PAID'	
		'13' = 'PRE-CLAIM PENDING'
		'14' = 'PRE-CLAIM SUBMITTED'	
		'15' = 'PRE-CLAIM CANCELLED'	
		'16' = 'DEATH ALLEGED'	
		'17' = 'DEATH VERIFIED'	
		'18' = 'DISABILITY ALLEGED'	
		'19' = 'DISABILITY VERIFIED'
		'20' = 'BANKRUPTCY ALLEGED'
		'21' = 'BANKRUPTCY VERIFIED'	
		'22' = 'PAID IN FULL'	
		'23' = 'NOT FULLY ORIGINATED'	
		'88' = 'PROCESSING ERROR'	
		'98' = 'UNKNOWN'	
	;
QUIT;


DATA FORMATTED;
SET DEMO;
FORMAT COMMENT $LONSTA.;
RUN;

