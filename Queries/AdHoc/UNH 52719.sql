﻿--used for troubleshooting SAS
SELECT * FROM OPENQUERY (DUSTER,'
SELECT * FROM OLWHRM1.LN80_LON_BIL_CRF
WHERE BF_SSN = ''100460092''
AND LD_BIL_CRT BETWEEN ''2016-12-05'' AND ''2017-07-06''
ORDER BY LD_BIL_CRT
');

SELECT * FROM OPENQUERY (DUSTER,'
SELECT * FROM OLWHRM1.BL10_BR_BIL
WHERE BF_SSN = ''100460092''
AND LD_BIL_CRT BETWEEN ''2016-12-05'' AND ''2017-07-06''
ORDER BY LD_BIL_CRT
');