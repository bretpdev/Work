/*NOTE: 
the SSAE XX FED School Refunds report (which will generate the results and the query) is available via SSRS at:  
https://uheaassrs.uheaa.org/Reports/Pages/Folder.aspx?ItemPath=%XfAuditor+Reports&ViewMode=List
*/


--check when XXXX/XXXX codes were last used
SELECT
	LNXX.PC_FAT_TYP,
	LNXX.PC_FAT_SUB_TYP,
	MAX(LNXX.LD_FAT_EFF) AS MAX_LD_FAT_EFF
FROM 
	CDW..LNXX_FIN_ATY LNXX
WHERE 
	LNXX.PC_FAT_TYP = 'XX'
	AND LNXX.PC_FAT_SUB_TYP IN ('XX','XX')
GROUP BY
	LNXX.PC_FAT_TYP,
	LNXX.PC_FAT_SUB_TYP

SELECT * FROM OPENQUERY (LEGEND,'
SELECT
	LNXX.PC_FAT_TYP,
	LNXX.PC_FAT_SUB_TYP,
	MAX(LNXX.LD_FAT_EFF) AS MAX_LD_FAT_EFF
FROM 
	PKUB.LNXX_FIN_ATY LNXX
WHERE 
	LNXX.PC_FAT_TYP = ''XX''
	AND LNXX.PC_FAT_SUB_TYP IN (''XX'',''XX'')
GROUP BY
	LNXX.PC_FAT_TYP,
	LNXX.PC_FAT_SUB_TYP
');