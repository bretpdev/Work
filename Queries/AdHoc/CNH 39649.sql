SELECT * FROM CDW..PDXX_PRS_NME WHERE DF_SPE_ACC_ID = 'XXXXXXXXXX'
SELECT * FROM OPENQUERY(LEGEND, 'SELECT * FROM PKUB.RSXX_IBR_RPS WHERE BF_SSN = ''XXXXXXXXX''')
SELECT * FROM OPENQUERY(LEGEND, 'SELECT * FROM PKUB.RSXX_BR_RPD WHERE BF_SSN = ''XXXXXXXXX''')
SELECT * FROM OPENQUERY(LEGEND, 'SELECT * FROM PKUB.RSXX_IBR_IRL_LON WHERE BF_SSN = ''XXXXXXXXX''')
SELECT * FROM OPENQUERY(LEGEND, 'SELECT * FROM PKUB.LNXX_LON_RPS WHERE BF_SSN = ''XXXXXXXXX''')