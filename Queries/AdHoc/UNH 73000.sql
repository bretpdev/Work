SELECT * FROM OPENQUERY(DUSTER,
'
SELECT * FROM OLWHRM1.LP14_DD_ACT_WDO WHERE PC_STA_LPD13 = ''A''
')

SELECT * FROM OPENQUERY(DUSTER,
'
SELECT * FROM OLWHRM1.LP15_DD_ACT_STE WHERE PC_STA_LPD13 = ''A''
')

SELECT * FROM OPENQUERY(DUSTER,
'
SELECT * FROM OLWHRM1.LP17_STP_PUR WHERE PC_STA_LPD17 = ''A''
')

SELECT * FROM OPENQUERY(DUSTER,
'
SELECT * FROM OLWHRM1.LP18_DD_CYC_PAR WHERE PC_STA_LPD18 = ''A''
')

SELECT * FROM OPENQUERY(DUSTER,
'
SELECT * FROM OLWHRM1.LP19_DD_SKP_PAR WHERE PC_STA_LPD19 = ''A''
')