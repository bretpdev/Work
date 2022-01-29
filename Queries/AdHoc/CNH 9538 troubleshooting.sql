select * from openquery (legend,'
	select * from pkub.lnXX_lon where bf_ssn in (
	''XXXXXXXXX'',
	''XXXXXXXXX'',
	''XXXXXXXXX'',
	''XXXXXXXXX'',
	''XXXXXXXXX'',
	''XXXXXXXXX'',
	''XXXXXXXXX'')
');

select * from openquery (legend,'
	select * from pkub.DWXX_DW_CLC_CLU where bf_ssn in (
	''XXXXXXXXX'',
	''XXXXXXXXX'',
	''XXXXXXXXX'',
	''XXXXXXXXX'',
	''XXXXXXXXX'',
	''XXXXXXXXX'',
	''XXXXXXXXX'')
');

select * from openquery (legend,'
	select * from PKUB.PDXX_PRS_ADR_EML PDXX where df_prs_id in (''XXXXXXXXX'',''XXXXXXXXX'')
');

select * from openquery (legend,'
	select * from PKUB.LNXX_INT_RTE_HST where bf_ssn in (''XXXXXXXXX'',''XXXXXXXXX'')
	and days(current date) BETWEEN days(LD_ITR_EFF_BEG) AND days(LD_ITR_EFF_END)
');
