/*Issue:
Identify the count of all documents in the imaging database with the exception of the doc IDs highlighted on the attached document.  
List each applicable Doc ID along with the total number of each Doc ID received for the past 11 years.

If you are able to gather the same information for the documents currently in process but not archived, please include these in the total.
*/


--('AMAUD','AMCOR','AS3RD','ASKEY','ASREF','CSCSH','CSPSH','DACAN','DAREQ','LSACH','LSARC','LSCKS','LSCLS','LSCOR','LSCRE','LSDDS','LSDEF','LSDIS','LSEHD','LSEVR','LSFCR','LSFOR','LSFRG','LSGFD','LSIBR','LSINC','LSISR','LSLBX','LSLVC','LSMFB','LSMIL','LSPPD','LSPSD','LSRTD','LSSCD','LSSDB','LSTAX','LSTFF','LSTLF','LSTPD','LSUND','LSUPR','LSWMD','PL3RD','REAFF','SKCOR','SKREF','SKSCH')
USE UHEAA

GO

SELECT 
	COUNT(*) AS [Count],
	UPPER(DOC_ID) AS DocId, 
	YEAR(SCAN_DATE) AS [YEAR] 
FROM 
	UHEAA..UHEAA_UHEAA_COMMERCIAL_TAB 
WHERE 
	DOC_ID IN ('AMAUD','AMCOR','AS3RD','ASKEY','ASREF','CSCSH','CSPSH','DACAN','DAREQ','LSACH','LSARC','LSCKS','LSCLS','LSCOR','LSCRE','LSDDS','LSDEF','LSDIS','LSEHD','LSEVR','LSFCR','LSFOR','LSFRG','LSGFD','LSIBR','LSINC','LSISR','LSLBX','LSLVC','LSMFB','LSMIL','LSPPD','LSPSD','LSRTD','LSSCD','LSSDB','LSTAX','LSTFF','LSTLF','LSTPD','LSUND','LSUPR','LSWMD','PL3RD','REAFF','SKCOR','SKREF','SKSCH') 
GROUP BY 
	UPPER(DOC_ID), 
	YEAR(SCAN_DATE) 
ORDER BY 
	UPPER(DOC_ID), 
	YEAR(SCAN_DATE)

