SELECT * FROM ODW..PD01_PDM_INF WHERE DF_SPE_ACC_ID = '4150103149'

SELECT * FROM OPENQUERY(DUSTER,
'
SELECT * FROM OLWHRM1.BR07_BR_DSA WHERE BF_SSN = ''554688578''
')

