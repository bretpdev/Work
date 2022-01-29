SELECT * FROM OPENQUERY(LEGEND,
'
select 
	c.*
from 
	sysibm.systables st
	inner join  syscat.columns c
		on c.tabname = st.name
where 
	type = ''T''
	AND CREATOR NOT IN (''SYSIBM'',''ZDBA'',''SYSTOOLS'',''INFO'',''SEPEOM'')
	and 
		(
			c.colname like ''DD_STA%''
			or
			c.column like ''LC_STA%''
		)

')