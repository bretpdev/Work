--onelink table dumps
SELECT * FROM OPENQUERY (DUSTER, 'SELECT * FROM OLWHRM1.ZL04_OLK_LTR_HDR');
SELECT * FROM OPENQUERY (DUSTER, 'SELECT * FROM OLWHRM1.ZL05_OLK_LTR_PGH');
SELECT * FROM OPENQUERY (DUSTER, 'SELECT * FROM OLWHRM1.ZL06_OLK_LTR_DSC');