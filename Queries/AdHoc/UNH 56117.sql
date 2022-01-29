select * from openquery(qadbd004, 'select * from olwhrm1.LN93_DSB_FIN_TRX WHERE BF_SSN = ''144768284''')
select * from openquery(qadbd004, 'select * from olwhrm1.LN90_FIN_ATY WHERE BF_SSN = ''144768284''')
select * from openquery(QADBD004, 'SELECT * FROM OLWHRM1.PD10_PRS_NME WHERE DF_SPE_ACC_ID = ''5160733243''')