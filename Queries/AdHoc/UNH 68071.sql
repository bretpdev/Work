SELECT * FROM OPENQUERY (DUSTER, '
SELECT 
* 
FROM 
    OLWHRM1.LP06_ITR_AND_TYP LP06 
WHERE 
    (
        lP06.PC_ITR_TYP NOT IN (''C1'', ''C2'', ''SV'', ''F2'')
        AND LP06.PD_EFF_END_LPD06 = (''9999-01-01'')
    )
    OR
    (
        LP06.PC_ITR_TYP IN (''C1'', ''C2'', ''SV'', ''F2'')
        AND LP06.PD_EFF_SR_LPD06 = (''2020-07-01'') 
        AND LP06.PD_EFF_END_LPD06 = (''2021-06-30'')
    )')