SELECT * FROM UDW..PD10_PRS_NME WHERE DF_SPE_ACC_ID = '3927393211'

SELECT * FROM OPENQUERY (DUSTER,'
SELECT * FROM OLWHRM1.RS20_IBR_IRL_LON
WHERE BF_SSN = ''528637358''
');