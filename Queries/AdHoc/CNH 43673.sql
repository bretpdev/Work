select 
	ADXX.* 
from 
	cdw..PDXX_PRS_NME PDXX
	INNER JOIN CDW..ADXX_PCV_ATY_ADJ ADXX
		ON ADXX.BF_SSN = PDXX.DF_PRS_ID
where 
	df_spe_acc_id in ('XXXXXXXXXX','XXXXXXXXXX')