select MONTH(processeddate), year(processeddate), COUNT(*) from cls..CheckByPhone where year(processeddate) >= 2017 and DataSource NOT LIKE '%IVR%' GROUP BY MONTH(processeddate), year(processeddate) ORDER BY  year(processeddate), MONTH(processeddate)