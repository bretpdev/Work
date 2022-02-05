use cdw 
go
select
     bf_ssn, count(1) datecount
from 
(
    select
        *,
        lag(ld_fat_eff, 1) over (partition by bf_ssn order by ld_fat_eff) PrevDate,
        lag(ld_fat_eff, 2) over (partition by bf_ssn order by ld_fat_eff) BeforePrevDate
        from cdw..LN90_FIN_ATY
        where LC_STA_LON90='A' and LC_FAT_REV_REA='1' and PC_FAT_TYP='10' and PC_FAT_SUB_TYP='10'
) a
where 
    datediff(day, PrevDate, ld_fat_eff) between 27 and 31 and datediff(day, BeforePrevDate, PrevDate) between 27 and 31
			AND LD_FAT_EFF BETWEEN CAST(DATEADD(MONTH,-6,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --make sure this occured in the last 6 months
group by
	bf_ssn

	select * from cdw..PD10_PRS_NME where DF_PRS_ID = '626058400'
	select * from cdw..LN90_FIN_ATY where BF_SSN = '626058400' and LC_STA_LON90='A' and PC_FAT_TYP='10' and PC_FAT_SUB_TYP='10' and LC_FAT_REV_REA = '1' order by LD_FAT_APL
	select * from cdw..LN90_FIN_ATY where BF_SSN = '626058400' and LC_STA_LON90='A' and PC_FAT_TYP='10' and PC_FAT_SUB_TYP='10' order by ld_fat_apl
	select * from cdw..LN94_LON_PAY_FAT where BF_SSN ='626058400' 
	select * from cdw..RM30_BR_RMT where bf_ssn = '626058400' order by LD_RMT_BCH_INI
	select * from cdw..BR30_BR_EFT where BF_SSN = '626058400'
	select * from cdw..LN83_EFT_TO_LON where BF_SSN = '626058400'
	select * from cdw..AY10_BR_LON_ATY where PF_REQ_ACT = 'RMACH' and BF_SSN = '626058400'
	select * from cdw..LN80_LON_BIL_CRF where BF_SSN = '626058400' and LC_STA_LON80 ='A' and LC_BIL_TYP_LON = 'p' AND DATEDIFF(DAY, CAST(GETDATE() AS DATE) ,LD_BIL_DU_LON) > 21 AND LA_TOT_BIL_STS >= LA_BIL_CUR_DU