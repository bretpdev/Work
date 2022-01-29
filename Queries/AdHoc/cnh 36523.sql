select * from openquery(legend,
'
select * from aes.PH06_CNC_EML_HST where hf_spe_id = ''7408830987''
')