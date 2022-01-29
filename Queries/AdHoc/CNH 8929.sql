update CDW.dbo.LTXX_LetterRequests  
set PrintedAt = null
where PrintedAt > = 'XXXX-XX-XX XX:XX:XX.XXX' 
and RM_DSC_LTR_PRC like '%TSXX%'
